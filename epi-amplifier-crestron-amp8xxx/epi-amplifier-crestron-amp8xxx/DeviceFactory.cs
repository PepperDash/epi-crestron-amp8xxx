using System;
using Crestron.SimplSharpPro;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Config;
using PepperDash.Core;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.Diagnostics;
using Crestron.SimplSharpPro.AudioDistribution;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Crestron.SimplSharp.Reflection;




namespace epi_amplifier_crestron_amp8xxx
{
    public class Amp8xxxFactory : EssentialsPluginDeviceFactory<CrestronAmplifierDevice>
    {

        public Amp8xxxFactory()
        {
            MinimumEssentialsFrameworkVersion = "1.7.5";

            TypeNames = new List<string> { "amp8xxx" };
        }

        public override EssentialsDevice BuildDevice(DeviceConfig dc)
        {
            Debug.Console(1, "Factory Attempting to create new Crestron Amp8xxx Device");

            return new CrestronAmplifierDevice(dc, GetAmpDevice(dc));
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

    }
}