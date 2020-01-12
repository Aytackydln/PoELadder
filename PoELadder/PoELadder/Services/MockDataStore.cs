using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PoELadder.Models;

namespace PoELadder.Services
{
    public class MockDataStore : INotifyPropertyChanged
    {
        public List<League> Leagues { get; }
        public List<string> Realms { get; set; } = new List<string>() { "pc", "sony", "xbox" };
        LadderType ladderType;
        public LadderType LadderType
        {
            get { return ladderType; }
            set { SetProperty(ref ladderType, value); }
        }
        public League SelectedLeague { get; set; }
        string selectedRealm;
        public string SelectedRealm
        {
            get { return selectedRealm; }
            set { SetProperty(ref selectedRealm, value); }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private int pageNo=1;
        public int PageNo
        {
            get { return pageNo; }
            set { SetProperty(ref pageNo, value); }
        }


        public ObservableCollection<LadderEntry> Items { get; set; } = new ObservableCollection<LadderEntry>();

        private readonly HttpClient officialPoeClient = new HttpClient();
        private readonly int pageSize=10;

        public MockDataStore()
        {
            officialPoeClient.BaseAddress = new Uri("http://api.pathofexile.com/");
            Leagues = GetLeagues();

            if (App.Current.Properties.ContainsKey("selectedRealm"))
            {
                SelectedRealm = Realms[(int)App.Current.Properties["selectedRealm"]];
            }
            else
            {
                SelectedRealm = Realms[0];
            }


            if (App.Current.Properties.ContainsKey("selectedLeague"))
            {
                SelectedLeague = Leagues[(int)App.Current.Properties["selectedLeague"]];
            }
            else
            {
                SelectedLeague = Leagues[0];
            }

        }


        public async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await GetItemsAsync(SelectedLeague);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }

            catch (Exception)
            {
                Items.Clear();
            }

            IsBusy = false;
        }

        #region INotifyPropertyChanged
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private List<League> GetLeagues()
        {
            var response = officialPoeClient.GetAsync("leagues");
            return JsonConvert.DeserializeObject<List<League>>(response.Result.Content.ReadAsStringAsync().Result);
        }
        private async Task<IEnumerable<LadderEntry>> GetItemsAsync(League league)
        {
            var response = await officialPoeClient.GetAsync("ladders/" + league.Id + "?"+getPageParameters()+"&realm=" + SelectedRealm + LadderType.Parameter);
            var ladderJson = response.Content.ReadAsStringAsync().Result;

            LadderResponse ladderResponse = await Task.Run(() =>
                ladderResponse = JsonConvert.DeserializeObject<LadderResponse>(ladderJson)
            );

            return ladderResponse.Entries;

        }

        private string getPageParameters()
        {
            int offset = (PageNo - 1) * pageSize;
            return "offset="+offset+"&limit="+pageSize;
        }
    }
}