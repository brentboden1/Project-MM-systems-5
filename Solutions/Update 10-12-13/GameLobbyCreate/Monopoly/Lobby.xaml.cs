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

namespace Monopoly
{
    public partial class Lobby : PhoneApplicationPage
    {
        public ObservableCollection<LobbyItems> Data { get; set; }
        private LobbyItems data;

        public Lobby()
        {
            InitializeComponent();
            Data = new ObservableCollection<LobbyItems>();
            LobbyList.ItemsSource = Data;
            App.client.GatAvailablePlayLobbiesCompleted+=client_GatAvailablePlayLobbiesCompleted;
            App.client.GatAvailablePlayLobbiesAsync();
            App.client.CreateLobbyCompleted += client_CreateLobbyCompleted;
            App.client.GatAvailablePlayLobbiesCompleted += client_GatAvailablePlayLobbiesCompleted;
            App.client.JoinLobbyRoomCompleted += client_JoinLobbyRoomCompleted;
        }

        private void CreateLobby_Click_1(object sender, RoutedEventArgs e)
        {
            App.Host = App.Me;
            App.client.CreateLobbyAsync(App.Me);            
        }

        void client_CreateLobbyCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            App.client.GatAvailablePlayLobbiesAsync();
            NavigationService.Navigate(new Uri("/WaitLobby.xaml", UriKind.Relative));
        }

        void client_GatAvailablePlayLobbiesCompleted(object sender, Monopoly.ServiceReference1.GatAvailablePlayLobbiesCompletedEventArgs e)
        {
            Data.Clear();
            foreach (var item in e.Result)
            {
                Data.Add(new LobbyItems() { LobbyName = item.LobbyId.LobbyId.ToString(), PlayerId = item.HostPlayer.PlayerId, PlayerName = item.HostPlayer.PlayerName });
            }
        }

        private void LobbyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                data = (LobbyItems)((ListBox)(sender)).SelectedItem;
                App.Host = new Monopoly.ServiceReference1.Player() { PlayerId = data.PlayerId, PlayerName = data.PlayerName };
                App.client.JoinLobbyRoomAsync(App.Me, App.Host);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        void client_JoinLobbyRoomCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/WaitLobby.xaml", UriKind.Relative));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Refresh_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.GatAvailablePlayLobbiesAsync();
        }
    }
}