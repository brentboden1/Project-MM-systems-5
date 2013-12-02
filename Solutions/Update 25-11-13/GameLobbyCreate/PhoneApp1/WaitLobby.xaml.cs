using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace PhoneApp1
{
    public partial class WaitLobby : PhoneApplicationPage
    {
        private DispatcherTimer dt;
        public ObservableCollection<PlayerData> plData { get; set; }

        public WaitLobby()
        {
            InitializeComponent();
            plData = new ObservableCollection<PlayerData>();
            LobbyList.ItemsSource = plData;
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(3.0);
            dt.Tick += dt_Tick;
            dt.Start();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            App.client.ShowPlayersInLobbyRoomCompleted += client_ShowPlayersInLobbyRoomCompleted;
            App.client.ShowPlayersInLobbyRoomAsync(App.Host.PlayerId);
        }

        void client_ShowPlayersInLobbyRoomCompleted(object sender, ServiceReference1.ShowPlayersInLobbyRoomCompletedEventArgs e)
        {
            plData.Clear();
            foreach (var item in e.Result)
            {
                plData.Add(new PlayerData() { PlayerId = item.PlayerId, PlayerName = item.PlayerName });
                if (plData.Count >= 4)
                {
                    App.client.StartGameCompleted += client_StartGameCompleted;
                    App.client.StartGameAsync(App.Host);
                }
            }
        }

        void client_StartGameCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Game.xaml", UriKind.Relative));
        }
    }

}