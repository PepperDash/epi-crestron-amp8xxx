using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;
using PepperDash.Essentials.Core;
namespace epi_amplifier_crestron_amp8xxx.Bridge.JoinMaps
{
    public class CrestronAmplifierJoinMap : JoinMapBase
    {
        #region Digital
        public uint Online { get; set; }
        public uint InputClip { get; set; }
        public uint OutputClip { get; set; }
        public uint DcFault { get; set; }
        public uint VoltageFault { get; set; }
        public uint CurrentFault { get; set; }
        public uint TempFault { get; set; }
        #endregion

        #region Analog
        public uint Temperature { get; set; }
        #endregion

        #region Serial
        public uint Name { get; set; }
        #endregion

        public CrestronAmplifierJoinMap()
        {
            //Singles
            Online = 1;
            Temperature = 1;
            Name = 1;

            //Array
            DcFault = 2;
            VoltageFault = 10;
            CurrentFault = 18;
            TempFault = 26;
        }
        public override void OffsetJoinNumbers(uint joinStart)
        {
            Online += joinStart - 1;
            Temperature += joinStart - 1;
            Name += joinStart - 1;
            DcFault += joinStart - 1;
            VoltageFault += joinStart - 1;
            CurrentFault += joinStart - 1;
            TempFault += joinStart - 1;

        }
    }
}