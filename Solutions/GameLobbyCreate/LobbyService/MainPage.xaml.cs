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
        ServiceReference3.Service3Client client3;

        ServiceReference3.Player me;
        ServiceReference1.Player p;
        ServiceReference2.Player pl;

        public ObservableCollection<DataObjects> Data { get; set; }
        // Constructor
        public MainPage()
        {
          //  lstbox2.Items.Clear();
            InitializeComponent();
            Data = new ObservableCollection<DataObjects>();
            client1 = new ServiceReference1.Service1Client();
            client2 = new ServiceReference2.Service2Client();
            client3 = new ServiceReference3.Service3Client();
            me = new ServiceReference3.Player();
            lstbox2.ItemsSource = Data;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void Add_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                client1.AddPlayerCompleted += client_AddPlayerCompleted;
                p = new ServiceReference1.Player();
                p.PlayerId = int.Parse(ID.Text);
                p.PlayerName = Name.Text;
                
                client1.AddPlayerAsync(p);

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
                    if (item.AlreadExist)
                    {
                        MessageBox.Show("Please enter another Nickname");
                        return;
                    }
                    else
                    {
                        lstbox.Items.Add(item.PlayerName);
                    }
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
                client2.GetPlayerAsync(Host.Text.ToString());
            }
            catch (Exception)
            {
            }
        }

        void client2_GetPlayerCompleted(object sender, ServiceReference2.GetPlayerCompletedEventArgs e)
        {
            client2.CreateLobbyCompleted += client2_CreateLobbyCompleted;
            client2.CreateLobbyAsync(e.Result, int.Parse(LobbyId.Text));
        }

        void client2_CreateLobbyCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            client2.GatAvailablePlayLobbiesCompleted += client2_GatAvailablePlayLobbiesCompleted;
            client2.GatAvailablePlayLobbiesAsync();
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
            MessageBox.Show(e.Error.ToString());
        }

        private void DeleteLobbies_Click_1(object sender, RoutedEventArgs e)
        {
            client2.DeleteAllLobbiesCompleted += client2_DeleteAllLobbiesCompleted;
            client2.DeleteAllLobbiesAsync();
        }

        void client2_DeleteAllLobbiesCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show(e.Error.ToString());
        }

        private void DIce_Click_1(object sender, RoutedEventArgs e)
        {
            client3.RollDiceCompleted+=client3_RollDiceCompleted;
            client3.RollDiceAsync(1);

        }

        void client3_RollDiceCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
           client3.ShowDiceRollCompleted += client3_ShowDiceRollCompleted;
           client3.ShowDiceRollAsync();
        }

        void client3_ShowDiceRollCompleted(object sender, ServiceReference3.ShowDiceRollCompletedEventArgs e)
        {
            try
            {
                lstbox3.Items.Clear();
                foreach (var item in e.Result)
                {
                    lstbox3.Items.Add(item.DiceOne + " " + item.DiceTwo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
