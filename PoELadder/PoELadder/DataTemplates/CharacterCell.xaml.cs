using PoELadder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoELadder.DataTemplates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterCell : ViewCell
    {
        public CharacterCell()
        {
            InitializeComponent();
            
            if (DesignMode.IsDesignModeEnabled)
            {
                LadderEntry entry = new LadderEntry()
                {
                    Rank = 2,
                    AccountName = "Account Name",
                    CharacterClass = "Ascendant",
                    CharacterName = "Character Name",
                    CharacterLevel = 65,
                    CharacterExperience = 69420
                };
                entry.LoadImageSource();
                BindingContext = entry;
            }
        }
    }
}