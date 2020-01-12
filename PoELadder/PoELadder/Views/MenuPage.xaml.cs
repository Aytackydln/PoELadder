using PoELadder.Models;
using PoELadder.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoELadder.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<LadderType> menuItems;
        public MockDataStore DataStore => DependencyService.Get<MockDataStore>();

        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<LadderType>
            {
                new LadderType { Title="Leagues", Parameter="&type=league" },
                new LadderType { Title="PVP", Parameter="&type=pvp" },
                new LadderType { Title="Labyrinth(Normal)", Parameter="&type=labyrinth&difficulty=1" },
                new LadderType { Title="Labyrinth(Cruel)", Parameter="&type=labyrinth&difficulty=2" },
                new LadderType { Title="Labyrinth(Merciless)", Parameter="&type=labyrinth&difficulty=3" },
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                DataStore.LadderType = (LadderType)ListViewMenu.SelectedItem;
                App.Current.Properties["selectedLadder"] = ListViewMenu.SelectedItem;
                if (RootPage != null)
                {
                    RootPage.CollapsaNavigation();
                    await DataStore.LoadItemsAsync();
                }
            };


            if (App.Current.Properties.ContainsKey("selectedLeague"))
            {
                object ladderTypeObject = App.Current.Properties["selectedLeague"];
                if(ladderTypeObject is LadderType)
                {
                    ListViewMenu.SelectedItem = (LadderType)ladderTypeObject;
                }
            }
            else
            {
                ListViewMenu.SelectedItem = menuItems[0];
            }

            
        }
    }
}