using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoELadder.Models
{
    public class League
    {
        [JsonProperty("id")]
        public string Id { get; set; }

    }
}
