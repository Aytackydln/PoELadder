using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PoELadder.Models;
using PoELadder.Views;
using PoELadder.ViewModels;
using PoELadder.Services;

namespace PoELadder.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        public MockDataStore DataStore => DependencyService.Get<MockDataStore>();
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();

            if (App.Current.Properties.ContainsKey("selectedRealm"))
            {
                RealmPicker.SelectedIndex = (int)App.Current.Properties["selectedRealm"];
            }
            else
            {
                RealmPicker.SelectedIndex = 0;
            }
            RealmPicker.SelectedIndexChanged += async (object sender, EventArgs args) =>
            {
                DataStore.SelectedRealm = (string)RealmPicker.SelectedItem;
                App.Current.Properties["selectedRealm"] = RealmPicker.SelectedIndex;
                viewModel.LoadItemsCommand.Execute(null);
                await Application.Current.SavePropertiesAsync();
            };

            if (App.Current.Properties.ContainsKey("selectedLeague"))
            {
                object persistedLeagueName = Application.Current.Properties["selectedLeague"];
                for (int i = 0; i<DataStore.Leagues.Count; i++)
                {
                    if (DataStore.Leagues[i].Id.Equals(persistedLeagueName))
                    {
                        LeaguePicker.SelectedIndex = i;
                        DataStore.SelectedLeague = DataStore.Leagues[i];
                    }
                }
            }else
            {
                LeaguePicker.SelectedIndex = 0;
            }

            DataStore.SelectedLeague = (League)LeaguePicker.SelectedItem;
            LeaguePicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
            {
                DataStore.SelectedLeague = (League)LeaguePicker.SelectedItem;
                Application.Current.Properties["selectedLeague"] = DataStore.SelectedLeague.Id;
                viewModel.LoadItemsCommand.Execute(null);
                await Application.Current.SavePropertiesAsync();
            };
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            /*
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.*/
            ItemsListView.SelectedItem = null;
        }

        async void ChooseRealm(object sender, EventArgs e)
        {
            RealmPicker.Focus();
        }
        async void FirstPage(object sender, EventArgs e)
        {
            DataStore.PageNo=1;
            await DataStore.LoadItemsAsync();
        }
        async void PreviousPage(object sender, EventArgs e)
        {
            DataStore.PageNo--;
            await DataStore.LoadItemsAsync();
        }
        async void NextPage(object sender, EventArgs e)
        {
            DataStore.PageNo++;
            await DataStore.LoadItemsAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (DataStore.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}