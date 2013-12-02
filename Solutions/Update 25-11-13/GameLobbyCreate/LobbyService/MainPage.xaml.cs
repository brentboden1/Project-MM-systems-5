using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LobbyService
{
    public partial class MainPage : UserControl
    {
        ServiceReference1.Service1Client client1;
        DataObjects data;
        ServiceReference1.Player host;
        ServiceReference1.Player me;
        ServiceReference1.Player staticHost;
        bool runTimer;
        DispatcherTimer dt;
        bool start = false;
        string MyName;

        public ObservableCollection<DataObjects> Data { get; set; }
        // Constructor
        public MainPage()
        {   
            runTimer = false;
            //  lstbox2.Items.Clear();
            InitializeComponent();
            Data = new ObservableCollection<DataObjects>();
            client1 = new ServiceReference1.Service1Client();
            lstbox2.ItemsSource = Data;
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1.0);
            dt.Tick += dt_Tick;
            dt.Start();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            /*client1.GatAvailablePlayLobbiesCompleted += client1_GatAvailablePlayLobbiesCompleted;
            client1.GatAvailablePlayLobbiesAsync();*/
        }

        void client1_GetPlayerByIdCompleted(object sender, ServiceReference1.GetPlayerByIdCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    staticHost = e.Result;

                    client1.StartGameCompleted += client1_StartGameCompleted;
                    client1.StartGameAsync(staticHost);

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.InnerException + "\n\n" + ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }

        void dt_Tick(object sender, EventArgs e)
        {
            try
            {
              if (runTimer)
              {
                    if (staticHost != null)
                    {
                     // client1.GetGameUpdateCompleted += client1_GetGameUpdateCompleted;
                      client1.GetGameUpdateAsync(staticHost);
                     // client1.GetUpdateCompleted += client1_GetUpdateCompleted;
                     // client1.GetUpdateAsync(staticHost);
                    }
                    else
                    {
                        return;
                    }
              }
            }
            catch (NullReferenceException)
            {
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       /* void client1_GetPlayerLocationCompleted(object sender, ServiceReference1.GetPlayerLocationCompletedEventArgs e)
        {
            try
            {
                lst2.Items.Add("Location = " + e.Result.ToString());
            }
            catch (TargetInvocationException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }*/

        void client1_GetUpdateCompleted(object sender, ServiceReference1.GetUpdateCompletedEventArgs e)
        {
            try
            {
              //  MessageBox.Show(e.Result.ToString());
                lst2.Items.Add(e.Result.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            try
            {
                //  client.AddPlayerAsync(p);
                client1.GetPlayersCompleted += client1_GetPlayersCompleted;
                client1.GetPlayersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

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
            try
            {
                client1.CreateLobbyCompleted += client1_CreateLobbyCompleted;
                me = e.Result;
                client1.CreateLobbyAsync(me);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void client1_CreateLobbyCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                LstCreatedLobby.Items.Clear();
                LstCreatedLobby.Items.Add("Host : " + me.PlayerName.ToString() + " " + me.PlayerId.ToString());
                client1.GatAvailablePlayLobbiesCompleted += client1_GatAvailablePlayLobbiesCompleted;
                client1.GatAvailablePlayLobbiesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void client1_GatAvailablePlayLobbiesCompleted(object sender, ServiceReference1.GatAvailablePlayLobbiesCompletedEventArgs e)
        {
            try
            {
                Data.Clear();
                //List<ServiceReference1.PlayerLobby> pl = e.Result.ToList();               
                foreach (var item in e.Result)
                {
                    Data.Add(new DataObjects() { LobbyID = item.LobbyId.LobbyId, PlayerID = item.HostPlayer.PlayerId, PlayerName = item.HostPlayer.PlayerName });
                }
                /*foreach (var item in e.Result)
                {
                    lstbox2.Items.Add(item);
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DeletePlayers_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                client1.DeleteAllPlayersCompleted += client1_DeleteAllPlayersCompleted;
                client1.DeleteAllPlayersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
            try
            {
                client1.DeleteAllLobbiesCompleted += client1_DeleteAllLobbiesCompleted;
                client1.DeleteAllLobbiesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
            catch (WebException ex)
            {
                MessageBox.Show("Please turn on the internet" + "\n" + ex.Response.ToString() + "\n" + ex.Message.ToString());
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
            catch (WebException ex)
            {
                MessageBox.Show("Please turn on the internet" + "\n" + ex.Response.ToString() + "\n" + ex.Message.ToString());
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("please be sure you entered an Nickname");
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
            catch (WebException ex)
            {
                MessageBox.Show("Please turn on the internet" + "\n" + ex.Response.ToString() + "\n" + ex.Message.ToString());
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
                foreach (var item in e.Result)
                {
                    lst2.Items.Add("Dice = " + item.ToString());
                }
                client1.GetPlayerLocationCompleted+=client1_GetPlayerLocationCompleted;
                client1.GetPlayerLocationAsync(staticHost, staticHost);
            }
                catch(TargetInvocationException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        void client1_GetPlayerLocationCompleted(object sender, ServiceReference1.GetPlayerLocationCompletedEventArgs e)
        {

            try
            {
                lst2.Items.Add(e.Result.ToString());
            }
            catch (TargetException)
            {
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString());
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
                client1.CheckPlayerCountAsync(staticHost.PlayerName);
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
                    runTimer = true;
                    client1.StartGameCompleted += client1_StartGameCompleted;
                    client1.StartGameAsync(staticHost);
                }
                else
                {
                    return;
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
                runTimer = true;
                client1.GetGameUpdateCompleted += client1_GetGameUpdateCompleted;
               client1.GetGameUpdateAsync(staticHost);
               //client1.GetPlayerLocationCompleted += client1_GetPlayerLocationCompleted;
              // client1.GetPlayerLocationAsync(staticHost, staticHost);
            }
            catch (NullReferenceException)
            {

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
            //   client1.StartGameCompleted+=client1_StartGameCompleted;
               client1.StartGameAsync(staticHost);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            client1.GetPlayerByIdCompleted += client1_GetPlayerByIdCompleted;
            client1.GetPlayerByIdAsync(0);
        }
    }
}
