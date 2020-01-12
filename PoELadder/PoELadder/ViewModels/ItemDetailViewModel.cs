using System;

using PoELadder.Models;

namespace PoELadder.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public LadderEntry Item { get; set; }
        public ItemDetailViewModel(LadderEntry item = null)
        {
            Title = item?.CharacterName;
            Item = item;
        }
    }
}
