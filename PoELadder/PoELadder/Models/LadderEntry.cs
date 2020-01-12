using Newtonsoft.Json;
using PoELadder.Util;
using System;
using System.Runtime.Serialization;
using Xamarin.Forms;

namespace PoELadder.Models
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class LadderEntry
    {
        public ImageSource ClassImage { get; set; }
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("dead")]
        public bool IsDead { get; set; }

        [JsonProperty("online")]
        public bool IsOnline { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("character.name")]
        public string CharacterName { get; set; }

        [JsonProperty("character.level")]
        public int CharacterLevel { get; set; }

        [JsonProperty("character.class")]
        public string CharacterClass { get; set; }

        [JsonProperty("character.experience")]
        public long CharacterExperience { get; set; }

        [JsonProperty("account.name")]
        public string AccountName { get; set; }

        [JsonProperty("account.realm")]
        public string Realm { get; set; }

        [JsonProperty("account.twitch.name")]
        public string TwitchName {
            get;
            set; 
        }

        public bool TwitchExists
        {
            get { return TwitchName != null; }
        }

        [OnDeserialized()]
        internal void LoadImageSource(StreamingContext context)
        {
            LoadImageSource();
        }
        public void LoadImageSource()
        {
            ClassImage = ImageSource.FromResource("PoELadder.Images.Avatars." + CharacterClass + ".png");
        }

    }
}
