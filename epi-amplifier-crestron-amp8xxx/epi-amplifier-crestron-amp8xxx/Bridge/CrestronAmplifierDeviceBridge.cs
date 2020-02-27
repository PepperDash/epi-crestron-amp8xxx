using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.DeviceSupport;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Devices.Common;
using PepperDash.Essentials.Bridges;
using Newtonsoft.Json;
using Crestron.SimplSharp.Reflection;
using epi_amplifier_crestron_amp8xxx.Bridge.JoinMaps;

namespace epi_amplifier_crestron_amp8xxx.Bridge
{
    public static class CrestronAmplifierDeviceApiExtensions
    {
        public static void LinkToApiExt(this CrestronAmplifierDevice ampDevice, BasicTriList trilist, uint joinStart, string joinMapKey)
        {
            CrestronAmplifierJoinMap joinMap = new CrestronAmplifierJoinMap();

            Debug.Console(1, ampDevice, "Linking to Trilist '{0}'", trilist.ID.ToString("X"));
            joinMap.OffsetJoinNumbers(joinStart);

            ampDevice.CommunicationMonitor.IsOnlineFeedback.LinkInputSig(trilist.BooleanInput[joinMap.Online]);
            trilist.StringInput[(joinMap.Name)].StringValue = ampDevice.Name;
            ampDevice.TemperatureFeedback.LinkInputSig(trilist.UShortInput[joinMap.Temperature]);

            foreach (var item in ampDevice._device.Amplifiers)
            {
                var i = item;
                uint index = i.Number;
                var offset = ((index - 1) * 8);
                Debug.Console(2, ampDevice, "Amp Output {0} Connect", i.Number);
                ampDevice.OverCurrentFaultFeedback[index].LinkInputSig(trilist.BooleanInput[joinMap.CurrentFault + offset]);
                ampDevice.TemperatureFaultFeedback[index].LinkInputSig(trilist.BooleanInput[joinMap.TempFault + offset]);
                ampDevice.DcFaultFeedback[index].LinkInputSig(trilist.BooleanInput[joinMap.DcFault + offset]);
                ampDevice.VoltageFaultFeedback[index].LinkInputSig(trilist.BooleanInput[joinMap.VoltageFault]);
            }

            {

            }


        }
    }
}