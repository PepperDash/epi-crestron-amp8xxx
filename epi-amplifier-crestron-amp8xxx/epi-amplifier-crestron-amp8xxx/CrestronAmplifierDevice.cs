using System.Collections.Generic;
using Crestron.SimplSharpPro;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Bridges;
using Newtonsoft.Json;
using PepperDash.Essentials.Core.Config;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.AudioDistribution;
using epi_amplifier_crestron_amp8xxx.Bridge.JoinMaps;


namespace epi_amplifier_crestron_amp8xxx
{
    public class CrestronAmplifierDevice : CrestronGenericBaseDevice, IBridgeAdvanced
    {
        public Amp8xxxBase Device { get; protected set; }
        protected DeviceConfig Config;
        protected AmpPropertiesConfig PropsConfig;

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



        public CrestronAmplifierDevice(DeviceConfig config, Amp8xxxBase device)
            : base(config.Key, config.Name, device)
        {
            Device = device;
            Config = config;
            PropsConfig = JsonConvert.DeserializeObject<AmpPropertiesConfig>(config.Properties.ToString());

            Init();
        }

        

        private void Init()
        {
            ClipFeedback = new Dictionary<uint, BoolFeedback>();
            OverCurrentFaultFeedback = new Dictionary<uint,BoolFeedback>();
            VoltageFaultFeedback = new Dictionary<uint,BoolFeedback>();
            TemperatureFaultFeedback = new Dictionary<uint,BoolFeedback>();
            DcFaultFeedback = new Dictionary<uint,BoolFeedback>();
            ClippingFeedback = new Dictionary<uint, BoolFeedback>();

            foreach (var item in Device.Amplifiers)
            {
                var i = item;
                OverCurrentFaultFeedback.Add(i.Number, new BoolFeedback(() => i.OverCurrentFaultFeedback.BoolValue));
                VoltageFaultFeedback.Add(i.Number, new BoolFeedback(() => i.VoltageFaultFeedback.BoolValue));
                TemperatureFaultFeedback.Add(i.Number, new BoolFeedback(() => i.TemperatureFaultFeedback.BoolValue));
                DcFaultFeedback.Add(i.Number, new BoolFeedback(() => i.DcFaultFeedback.BoolValue));
                ClippingFeedback.Add(i.Number, new BoolFeedback(() => i.ClippingFeedback.BoolValue));
            }
            foreach (var item in Device.AudioInputs)
            {
                var i = item;

                ClipFeedback.Add(i.Number, new BoolFeedback(() => i.ClipFeedback.BoolValue));
            }
            TemperatureFeedback = new IntFeedback(() => Device.TemperatureFeedback.UShortValue);
            Device.BaseEvent += _device_BaseEvent;
        }


        void _device_BaseEvent(GenericBase device, BaseEventArgs args)
        {
            if (args.EventId == CommercialAmplifier.ClipFeedbackEventId)
            {
                ClipFeedback[(uint)args.Index].FireUpdate();
            }
            if (args.EventId == CommercialAmplifier.DcFaultFeedbackEventId)
            {
                DcFaultFeedback[(uint)args.Index].FireUpdate();
            }
            if (args.EventId == CommercialAmplifier.OverCurrentFaultFeedbackEventId)
            {
                OverCurrentFaultFeedback[(uint)args.Index].FireUpdate();
            }
            if (args.EventId == CommercialAmplifier.TemperatureFaultFeedbackEventId)
            {
                TemperatureFaultFeedback[(uint)args.Index].FireUpdate();
            }
            if (args.EventId == CommercialAmplifier.VoltageFaultFeedbackEventId)
            {
                VoltageFaultFeedback[(uint)args.Index].FireUpdate();
            }
            if (args.EventId == CommercialAmplifier.ClippingFeedbackEventId)
            {
                ClippingFeedback[(uint)args.Index].FireUpdate();
            }
            if (args.EventId == CommercialAmplifier.TemperatureFeedbackEventId)
            {
                TemperatureFeedback.FireUpdate();
            }
        }

        #region IBridge Members

        public void LinkToApi(BasicTriList trilist, uint joinStart, string joinMapKey, EiscApiAdvanced bridge)
        {
            var joinMap = new CrestronAmplifierJoinMapAdv(joinStart);

            Debug.Console(1, this, "Linking to Trilist '{0}'", trilist.ID.ToString("X"));

            if (bridge != null)
            {
                bridge.AddJoinMap(Key, joinMap);
            }

            CommunicationMonitor.IsOnlineFeedback.LinkInputSig(trilist.BooleanInput[joinMap.Online.JoinNumber]);
            trilist.StringInput[(joinMap.Name.JoinNumber)].StringValue = Name;
            TemperatureFeedback.LinkInputSig(trilist.UShortInput[joinMap.Temperature.JoinNumber]);

            foreach (var item in Device.Amplifiers)
            {
                var i = item;
                var index = i.Number;
                var offset = ((index - 1) * 8);
                Debug.Console(2, this, "Amp Output {0} Connect", i.Number);
                OverCurrentFaultFeedback[index].LinkInputSig(trilist.BooleanInput[joinMap.CurrentFault.JoinNumber + offset]);
                TemperatureFaultFeedback[index].LinkInputSig(trilist.BooleanInput[joinMap.TempFault.JoinNumber + offset]);
                DcFaultFeedback[index].LinkInputSig(trilist.BooleanInput[joinMap.DcFault.JoinNumber + offset]);
                VoltageFaultFeedback[index].LinkInputSig(trilist.BooleanInput[joinMap.VoltageFault.JoinNumber + offset]);
            }

        }

        #endregion
    }

}

