using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Devices;
using PepperDash.Essentials.Devices.Common.Codec;
using PepperDash.Essentials.Devices.Common.DSP;
using System.Text.RegularExpressions;
using Crestron.SimplSharp.Reflection;
using Newtonsoft.Json;
using PepperDash.Essentials.Core.Config;
using PepperDash.Essentials.Bridges;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.Diagnostics;
using Crestron.SimplSharpPro.AudioDistribution;
using epi_amplifier_crestron_amp8xxx.Bridge;


namespace epi_amplifier_crestron_amp8xxx
{
    public class CrestronAmplifierDevice : CrestronGenericBaseDevice, IBridge
    {
        public Amp8xxxBase _device { get; protected set; }
        protected DeviceConfig _config;
        protected AmpPropertiesConfig _propsConfig;

        public Dictionary<uint, BoolFeedback> ClipFeedback;

        public Dictionary<uint, BoolFeedback> ClippingFeedback;

        public Dictionary<uint, BoolFeedback> VoltageFaultFeedback;

        public Dictionary<uint, BoolFeedback> OverCurrentFaultFeedback;

        public Dictionary<uint, BoolFeedback> TemperatureFaultFeedback;

        public Dictionary<uint, BoolFeedback> DcFaultFeedback;

        public IntFeedback TemperatureFeedback;
        private int _Temperature { get; set; }
        public int Temperature
        {
            get
            {
                return _Temperature;
            }
            protected set
            {
                _Temperature = value;
                TemperatureFeedback.FireUpdate();
            }
        }

        public static void LoadPlugin()
        {
            DeviceFactory.AddFactoryForType("Amp8xxx", CrestronAmplifierDevice.Build);
        }

        public static CrestronAmplifierDevice Build(DeviceConfig config)
        {
            var device = CrestronAmplifierDevice.GetAmpDevice(config);
            return new CrestronAmplifierDevice(config, device);
        }

        public CrestronAmplifierDevice(DeviceConfig config, Amp8xxxBase device)
            : base(config.Key, config.Name, device)
        {
            _device = device;
            _config = config;
            _propsConfig = JsonConvert.DeserializeObject<AmpPropertiesConfig>(config.Properties.ToString());

            Init();
        }

        protected static Amp8xxxBase GetAmpDevice(DeviceConfig config)
        {
            var deviceConfig = JsonConvert.DeserializeObject<AmpPropertiesConfig>(config.Properties.ToString());

            try
            {
                var ampDeviceType = typeof(Amp8xxxBase)
                    .GetCType()
                    .Assembly
                    .GetTypes()
                    .FirstOrDefault(x => x.Name.Equals(deviceConfig.Model, StringComparison.OrdinalIgnoreCase));

                if (ampDeviceType == null) throw new NullReferenceException();
                if (deviceConfig.Control.IpId == null) throw new Exception("The IPID for this device must be defined");

                var newDevice = ampDeviceType
                    .GetConstructor(new CType[] { typeof(ushort).GetCType(), typeof(CrestronControlSystem) })
                    .Invoke(new object[] { Convert.ToUInt16(deviceConfig.Control.IpId, 16), Global.ControlSystem });

                var ampDevice = newDevice as Amp8xxxBase;
                if (ampDevice == null) throw new NullReferenceException("Could not find the base Amplifier type");


                return ampDevice;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Init()
        {
            ClipFeedback = new Dictionary<uint, BoolFeedback>();
            OverCurrentFaultFeedback = new Dictionary<uint,BoolFeedback>();
            VoltageFaultFeedback = new Dictionary<uint,BoolFeedback>();
            TemperatureFaultFeedback = new Dictionary<uint,BoolFeedback>();
            DcFaultFeedback = new Dictionary<uint,BoolFeedback>();
            ClippingFeedback = new Dictionary<uint, BoolFeedback>();

            foreach (var item in _device.Amplifiers)
            {
                var i = item;
                OverCurrentFaultFeedback.Add(i.Number, new BoolFeedback(() => i.OverCurrentFaultFeedback.BoolValue));
                VoltageFaultFeedback.Add(i.Number, new BoolFeedback(() => i.VoltageFaultFeedback.BoolValue));
                TemperatureFaultFeedback.Add(i.Number, new BoolFeedback(() => i.TemperatureFaultFeedback.BoolValue));
                DcFaultFeedback.Add(i.Number, new BoolFeedback(() => i.DcFaultFeedback.BoolValue));
                ClippingFeedback.Add(i.Number, new BoolFeedback(() => i.ClippingFeedback.BoolValue));
            }
            foreach (var item in _device.AudioInputs)
            {
                var i = item;

                ClipFeedback.Add(i.Number, new BoolFeedback(() => i.ClipFeedback.BoolValue));
            }
            TemperatureFeedback = new IntFeedback(() => _device.TemperatureFeedback.UShortValue);
            _device.BaseEvent += new BaseEventHandler(_device_BaseEvent);
        }


        void _device_BaseEvent(GenericBase device, BaseEventArgs args)
        {
            if (args.EventId == Amp8xxxBase.ClipFeedbackEventId)
            {
                ClipFeedback[(uint)args.Index].FireUpdate();
            }
            if (args.EventId == Amp8xxxBase.DcFaultFeedbackEventId)
            {
                DcFaultFeedback[(uint)args.Index].FireUpdate();
            }
            if (args.EventId == Amp8xxxBase.OverCurrentFaultFeedbackEventId)
            {
                OverCurrentFaultFeedback[(uint)args.Index].FireUpdate();
            }
            if (args.EventId == Amp8xxxBase.TemperatureFaultFeedbackEventId)
            {
                TemperatureFaultFeedback[(uint)args.Index].FireUpdate();
            }
            if (args.EventId == Amp8xxxBase.VoltageFaultFeedbackEventId)
            {
                VoltageFaultFeedback[(uint)args.Index].FireUpdate();
            }
            if (args.EventId == Amp8xxxBase.ClippingFeedbackEventId)
            {
                ClippingFeedback[(uint)args.Index].FireUpdate();
            }
            if (args.EventId == Amp8xxxBase.TemperatureFeedbackEventId)
            {
                TemperatureFeedback.FireUpdate();
            }
        }

        #region IBridge Members

        public void LinkToApi(BasicTriList trilist, uint joinStart, string joinMapKey)
        {
            this.LinkToApiExt(trilist, joinStart, joinMapKey);
        }

        #endregion
    }

}

