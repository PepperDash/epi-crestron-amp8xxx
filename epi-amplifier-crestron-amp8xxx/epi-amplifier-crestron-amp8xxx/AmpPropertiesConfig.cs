using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PepperDash.Core;

namespace epi_amplifier_crestron_amp8xxx
{
    public class AmpPropertiesConfig
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("control")]
        public ControlPropertiesConfig Control { get; set; }
    }
}