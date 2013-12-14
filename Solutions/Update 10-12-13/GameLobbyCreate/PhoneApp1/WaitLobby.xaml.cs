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
       // private DispatcherTimer dt;
        public ObservableCollection<PlayerData> plData { get; set; }

        public WaitLobby()
        {
            InitializeComponent();
            plData = new ObservableCollection<PlayerData>();
            LobbyList.ItemsSource = plData;
            App.client.ShowPlayersInLobbyRoomCompleted += client_ShowPlayersInLobbyRoomCompleted;
            App.client.StartGameCompleted += client_StartGameCompleted;
            if (App.Host == App.Me)
            {
                StartGame.Visibility = Visibility.Visible;
            }
        }

        void client_ShowPlayersInLobbyRoomCompleted(object sender, ServiceReference1.ShowPlayersInLobbyRoomCompletedEventArgs e)
        {
            plData.Clear();
            foreach (var item in e.Result)
            {
                plData.Add(new PlayerData() { PlayerId = item.PlayerId, PlayerName = item.PlayerName });
            }
        }

        void client_StartGameCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Game.xaml", UriKind.Relative));
        }

        private void Refresh_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.ShowPlayersInLobbyRoomAsync(App.Host.PlayerId);
        }

        private void StartGame_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.StartGameAsync(App.Host);
        }
    }

}