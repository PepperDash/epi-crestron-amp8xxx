using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;
using PepperDash.Essentials.Core;
namespace epi_amplifier_crestron_amp8xxx.Bridge.JoinMaps
{
    public class CrestronAmplifierJoinMapAdv : JoinMapBaseAdvanced
    {
        public CrestronAmplifierJoinMapAdv(uint joinStart)
            : base(joinStart, typeof(CrestronAmplifierJoinMapAdv))
        {
        }

        [JoinName("Online")]
        public JoinDataComplete Online =
            new JoinDataComplete(new JoinData { JoinNumber = 1, JoinSpan = 1 },
            new JoinMetadata
            {
                Description = "Device Online",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Digital
            });

        [JoinName("Temperature")]
        public JoinDataComplete Temperature =
            new JoinDataComplete(new JoinData { JoinNumber = 1, JoinSpan = 1 },
            new JoinMetadata
            {
                Description = "Device Temperature",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Analog
            });

        [JoinName("Name")]
        public JoinDataComplete Name =
            new JoinDataComplete(new JoinData { JoinNumber = 1, JoinSpan = 1 },
            new JoinMetadata
            {
                Description = "Device Name",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Serial
            });

        [JoinName("DcFault")]
        public JoinDataComplete DcFault =
            new JoinDataComplete(new JoinData { JoinNumber = 2, JoinSpan = 8 },
            new JoinMetadata
            {
                Description = "Device DcFault",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Digital
            });


        [JoinName("VoltageFault")]
        public JoinDataComplete VoltageFault =
            new JoinDataComplete(new JoinData { JoinNumber = 10, JoinSpan = 8 },
            new JoinMetadata
            {
                Description = "Device VoltageFault",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Digital
            });


        [JoinName("CurrentFault")]
        public JoinDataComplete CurrentFault =
            new JoinDataComplete(new JoinData { JoinNumber = 18, JoinSpan = 8 },
            new JoinMetadata
            {
                Description = "Device CurrentFault",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Digital
            });


        [JoinName("TempFault")]
        public JoinDataComplete TempFault =
            new JoinDataComplete(new JoinData { JoinNumber = 26, JoinSpan = 8 },
            new JoinMetadata
            {
                Description = "Device TempFault",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Digital
            });


    }
}