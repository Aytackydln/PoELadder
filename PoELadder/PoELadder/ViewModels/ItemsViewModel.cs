using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using PoELadder.Models;
using PoELadder.Views;

namespace PoELadder.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            await DataStore.LoadItemsAsync();
        }
    }
}