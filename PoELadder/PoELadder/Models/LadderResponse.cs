using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PoELadder.Models
{
    public class LadderResponse
    {
        public List<LadderEntry> Entries { get; set; }

        [OnDeserialized()]
        internal void LoadImageSources(StreamingContext context)
        {
            foreach (LadderEntry entry in 
                Entries) {
                entry.LoadImageSource(context);
            }
        }
    }
}
