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
        DataObjects data;
        ServiceReference1.Player host;
        ServiceReference1.Player me;

        string MyName;

        public ObservableCollection<DataObjects> Data { get; set; }
        // Constructor
        public MainPage()
        {
            //  lstbox2.Items.Clear();
            InitializeComponent();
            Data = new ObservableCollection<DataObjects>();
            client1 = new ServiceReference1.Service1Client();
            lstbox2.ItemsSource = Data;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            client1.GatAvailablePlayLobbiesCompleted += client1_GatAvailablePlayLobbiesCompleted;
            client1.GatAvailablePlayLobbiesAsync();
        }

        private void Add_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                MyName = Name.Text.ToString();
                client1.AddPlayerCompleted += client_AddPlayerCompleted;
                client1.AddPlayerAsync(MyName);

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
                client1.GetPlayerCompleted += client1_GetPlayerCompleted;
                client1.GetPlayerAsync(MyName);
            }
            catch (Exception)
            {
            }
        }

        void client1_GetPlayerCompleted(object sender, ServiceReference1.GetPlayerCompletedEventArgs e)
        {
            client1.CreateLobbyCompleted += client1_CreateLobbyCompleted;
            me = e.Result;
            client1.CreateLobbyAsync(me);
        }

        void client1_CreateLobbyCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            LstCreatedLobby.Items.Clear();
            LstCreatedLobby.Items.Add("Host : " + me.PlayerName.ToString() + " " + me.PlayerId.ToString());
            client1.GatAvailablePlayLobbiesAsync();
        }

        void client1_GatAvailablePlayLobbiesCompleted(object sender, ServiceReference1.GatAvailablePlayLobbiesCompletedEventArgs e)
        {
            Data.Clear();
            List<ServiceReference1.PlayerLobby> pl = e.Result.ToList();
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
            client1.DeleteAllPlayersCompleted += client1_DeleteAllPlayersCompleted;
            client1.DeleteAllPlayersAsync();
        }

        void client1_DeleteAllPlayersCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
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
                MessageBox.Show(ex.InnerException.ToString());
            }
        }

        private void DeleteLobbies_Click_1(object sender, RoutedEventArgs e)
        {
            client1.DeleteAllLobbiesCompleted += client1_DeleteAllLobbiesCompleted;
            client1.DeleteAllLobbiesAsync();
        }

        void client1_DeleteAllLobbiesCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
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
                MessageBox.Show(ex.InnerException.ToString());
            }
        }

        private void lstbox2_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                data = (DataObjects)((ListBox)(sender)).SelectedItem;
                host = new ServiceReference1.Player() { PlayerId = data.PlayerID, PlayerName = data.PlayerName };
                client1.GetPlayerByNameCompleted += client1_GetPlayerByNameCompleted;
                client1.GetPlayerByNameAsync(MyName);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }

        void client1_GetPlayerByNameCompleted(object sender, ServiceReference1.GetPlayerByNameCompletedEventArgs e)
        {
            try
            {
                me = e.Result;
                //host = new ServiceReference1.Player() { PlayerId = data.PlayerID, PlayerName = data.PlayerName };
                client1.JoinLobbyRoomCompleted += client1_JoinLobbyRoomCompleted;
                client1.JoinLobbyRoomAsync(me, host);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Data + "\n" + ex.InnerException);
            }
        }

        void client1_JoinLobbyRoomCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                client1.ShowPlayersInLobbyRoomCompleted += client1_ShowPlayersInLobbyRoomCompleted;
                client1.ShowPlayersInLobbyRoomAsync(host.PlayerId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }

        void client1_GetGameUpdateCompleted(object sender, ServiceReference1.GetGameUpdateCompletedEventArgs e)
        {
            try
            {
                lst2.Items.Clear();
                lst2.Items.Add(e.Result.Last().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }

        void client1_ShowPlayersInLobbyRoomCompleted(object sender, ServiceReference1.ShowPlayersInLobbyRoomCompletedEventArgs e)
        {
            try
            {
                LstCreatedLobby.Items.Clear();
                //   List<ServiceReference1.Player> pl = e.Result.ToList();
                foreach (var item in e.Result)
                {
                    LstCreatedLobby.Items.Add(item.PlayerId + " " + item.PlayerName);
                }
                client1.CheckPlayerCountCompleted += client1_CheckPlayerCountCompleted;
                client1.CheckPlayerCountAsync(host.PlayerName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }

        void client1_CheckPlayerCountCompleted(object sender, ServiceReference1.CheckPlayerCountCompletedEventArgs e)
        {
            try
            {
                if (e.Result == 4)
                {
                    client1.StartGameCompleted += client1_StartGameCompleted;
                    client1.StartGameAsync(host);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }

        void client1_StartGameCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                client1.GetGameUpdateCompleted += client1_GetGameUpdateCompleted;
                client1.GetGameUpdateAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }

        private void Refresh_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                client1.GatAvailablePlayLobbiesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }
    }
}
