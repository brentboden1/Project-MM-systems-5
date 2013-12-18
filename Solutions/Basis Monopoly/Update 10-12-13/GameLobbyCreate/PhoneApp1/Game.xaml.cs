using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace PhoneApp1
{
    public partial class Game : PhoneApplicationPage
    {
        private ObservableCollection<string> Log;
        private List<CardData> localData;
        private PlayerLocation loc = new PlayerLocation();
        private DispatcherTimer dt;
        int location = 0;
        public Game()
        {
            Log = new ObservableCollection<string>();
            dt = new DispatcherTimer();
            localData = new List<CardData>();
            InitializeComponent();
            lstLog.ItemsSource = Log;
            GamePanel.DataContext = loc;
            dt.Interval = TimeSpan.FromSeconds(5.0);
            dt.Tick += dt_Tick;
            dt.Start();
            App.client.RollDiceCompleted += client_RollDiceCompleted;
            App.client.EndOfTurnCompleted += client_EndOfTurnCompleted;
            App.client.GetStateCompleted += client_GetStateCompleted;
            App.client.BuyTileCompleted += client_BuyTileCompleted;
            App.client.GetLocalHouseDataCompleted += client_GetLocalHouseDataCompleted;
            App.client.GetLocalHouseDataAsync();
            App.client.OrderNumCompleted += client_OrderNumCompleted;
            App.client.OrderNumAsync(App.Me);
        }

        void client_BuyTileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Tile Bought");
        }

        void client_OrderNumCompleted(object sender, ServiceReference1.OrderNumCompletedEventArgs e)
        {
            try
            {
                switch (e.Result)
                {
                    case 0:
                        Player0.Fill = new SolidColorBrush(Colors.Red);
                        break;
                    case 1:
                        Player1.Fill = new SolidColorBrush(Colors.Blue);
                        break;
                    case 2:
                        Player2.Fill = new SolidColorBrush(Colors.Green);
                        break;
                    case 3:
                        Player3.Fill = new SolidColorBrush(Colors.Yellow);
                        break;
                    default:
                        Player0.Fill = new SolidColorBrush(Colors.Red);
                        Player1.Fill = new SolidColorBrush(Colors.Blue);
                        Player2.Fill = new SolidColorBrush(Colors.Green);
                        Player3.Fill = new SolidColorBrush(Colors.Yellow);
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        void client_GetLocalHouseDataCompleted(object sender, ServiceReference1.GetLocalHouseDataCompletedEventArgs e)
        {
            try
            {
                localData.Clear();
                foreach (var item in e.Result)
                {
                    localData.Add(new CardData() { BuyCost = item.BuyCost, Group = item.Group, HouseCost = item.HouseCost, ID = item.ID, Name = item.Name, Position = item.Position, Rent0 = item.Rent0, Rent1 = item.Rent1, Rent2 = item.Rent2, Rent3 = item.Rent3, Rent4 = item.Rent4, Rent5 = item.Rent5, Type = item.Type });
                }
            }
            catch (Exception ex)
            {

            }
        }

        void client_GetStateCompleted(object sender, ServiceReference1.GetStateCompletedEventArgs e)
        {
            try
            {
                if (location != e.Result.ActivePlayer.Location)
                {
                    location = e.Result.ActivePlayer.Location;
                    getLocation(location);
                    MessageBox.Show(location.ToString());
                }
                if (e.Result.Log != null)
                {
                    Log.Clear();
                    foreach (var item in e.Result.Log)
                    {
                        Log.Add(item);
                    }
                }
                MoneyBox.Text = "Money : " +  e.Result.ActivePlayer.Cash.ToString();
            }
            catch (Exception ex)
            {

            }



        }

        void client_EndOfTurnCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("End of the turn");
        }

        void client_RollDiceCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Roll dice");
        }

        void dt_Tick(object sender, EventArgs e)
        {
            //getLocation(pos);
            App.client.GetStateAsync(App.Me);
        }

        private PlayerLocation getLocation(int PositionPlayer)
        {
            loc.X = 10;
            loc.Y = 10;
            while (PositionPlayer != 0)
            {
                if (loc.X != 0 && loc.Y == 10)
                {
                    PositionPlayer--;
                    loc.X--;
                }
                else if (loc.Y != 0 && loc.X == 0)
                {
                    PositionPlayer--;
                    loc.Y--;

                }
                else if (loc.Y == 0 && loc.X != 10)
                {
                    PositionPlayer--;
                    loc.X++;
                }
                else if (loc.X == 10 && loc.Y != 10)
                {
                    PositionPlayer--;
                    loc.Y++;
                }
            }
            return new PlayerLocation() { X = loc.X, Y = loc.Y };
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            loc.X = 10;
            loc.Y = 10;
        }

        private void RollDice_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.RollDiceAsync();
            BuyTile.Visibility = Visibility.Visible;
            RollDice.Visibility = Visibility.Collapsed;
        }

        private void EndPhase_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.EndOfTurnAsync();
        }

        private void BuyTile_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.BuyTileAsync();
            BuyTile.Visibility = Visibility.Collapsed;
            RollDice.Visibility = Visibility.Visible;
        }
    }
}