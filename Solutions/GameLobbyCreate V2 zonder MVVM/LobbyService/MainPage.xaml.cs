using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LobbyService
{
    public partial class MainPage : UserControl
    {
        ServiceReference1.Service1Client client1;
        ServiceReference2.Service2Client client2;

        ServiceReference2.Player me;

        string MyName;

        public ObservableCollection<DataObjects> Data { get; set; }
        // Constructor
        public MainPage()
        {
          //  lstbox2.Items.Clear();
            InitializeComponent();
            Data = new ObservableCollection<DataObjects>();
            client1 = new ServiceReference1.Service1Client();
            client2 = new ServiceReference2.Service2Client();
            lstbox2.ItemsSource = Data;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            client2.GatAvailablePlayLobbiesCompleted += client2_GatAvailablePlayLobbiesCompleted;
            client2.GatAvailablePlayLobbiesAsync();
        }

        private void Add_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                MyName = Name.Text.ToString();
                client1.AddPlayerCompleted += client_AddPlayerCompleted;
                client1.AddPlayerAsync(Name.Text.ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void client1_GetPlayersCompleted(object sender, ServiceReference1.GetPlayersCompletedEventArgs e)
        {
            try
            {
                lstbox.Items.Clear();
                foreach (var item in e.Result)
                {
                    lstbox.Items.Add(item.PlayerName + " " + item.PlayerId);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void client_AddPlayerCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //  client.AddPlayerAsync(p);
                client1.GetPlayersCompleted += client1_GetPlayersCompleted;
                client1.GetPlayersAsync();
            
        }

        private void Create_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                client2.GetPlayerCompleted += client2_GetPlayerCompleted;
                //client2.GetPlayerAsync(int.Parse(Host.Text));
                client2.GetPlayerAsync(MyName);
            }
            catch (Exception)
            {
            }
        }

        void client2_GetPlayerCompleted(object sender, ServiceReference2.GetPlayerCompletedEventArgs e)
        {
            client2.CreateLobbyCompleted += client2_CreateLobbyCompleted;
            me = e.Result;
            client2.CreateLobbyAsync(e.Result);
        }

        void client2_CreateLobbyCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            LstCreatedLobby.Items.Clear();
            LstCreatedLobby.Items.Add("Host : " + me.PlayerName.ToString() + " " + me.PlayerId.ToString());
        }

        void client2_GatAvailablePlayLobbiesCompleted(object sender, ServiceReference2.GatAvailablePlayLobbiesCompletedEventArgs e)
        {
            Data.Clear();
            List<ServiceReference2.PlayerLobby> pl = e.Result.ToList();
            foreach (var item in pl)
            {
               Data.Add(new DataObjects() { LobbyID = item.LobbyId.LobbyId, PlayerID = item.HostPlayer.PlayerId, PlayerName = item.HostPlayer.PlayerName });
            }
            /*foreach (var item in e.Result)
            {
                lstbox2.Items.Add(item);
            }*/
        }

        private void DeletePlayers_Click_1(object sender, RoutedEventArgs e)
        {
                client2.DeleteAllPlayersCompleted += client2_DeleteAllPlayersCompleted;
                client2.DeleteAllPlayersAsync();
        }

        void client2_DeleteAllPlayersCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                MessageBox.Show(e.UserState.ToString());
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("There are no Players to be deletet");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteLobbies_Click_1(object sender, RoutedEventArgs e)
        {
            client2.DeleteAllLobbiesCompleted += client2_DeleteAllLobbiesCompleted;
            client2.DeleteAllLobbiesAsync();
        }

        void client2_DeleteAllLobbiesCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                MessageBox.Show(e.Error.ToString());
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("There are no Lobbies to be deletet");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lstbox2_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                client2.JoinLobbyRoomCompleted += client2_JoinLobbyRoomCompleted;
                DataObjects data = (DataObjects)((ListBox)(sender)).SelectedItem;
                ServiceReference2.Player pl = new ServiceReference2.Player() { PlayerId = data.PlayerID, PlayerName = data.PlayerName };
                client2.JoinLobbyRoomAsync(me, pl.PlayerId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        void client2_JoinLobbyRoomCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            client2.ShowPlayersInLobbyRoomCompleted += client2_ShowPlayersInLobbyRoomCompleted;
            client2.ShowPlayersInLobbyRoomAsync();
        }

        void client2_ShowPlayersInLobbyRoomCompleted(object sender, ServiceReference2.ShowPlayersInLobbyRoomCompletedEventArgs e)
        {
            try
            {
                List<ServiceReference2.Player> pl = e.Result.ToList();
                foreach (var item in pl)
                {
                    LstCreatedLobby.Items.Add(item.PlayerId + " " + item.PlayerName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Refresh_Click_1(object sender, RoutedEventArgs e)
        {
            client2.GatAvailablePlayLobbiesCompleted+=client2_GatAvailablePlayLobbiesCompleted;
            client2.GatAvailablePlayLobbiesAsync();
        }
    }
}
