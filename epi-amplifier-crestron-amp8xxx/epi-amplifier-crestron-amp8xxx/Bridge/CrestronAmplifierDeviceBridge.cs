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
            


        }
    }
}