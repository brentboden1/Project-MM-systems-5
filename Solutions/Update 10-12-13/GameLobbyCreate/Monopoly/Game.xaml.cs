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

namespace Monopoly
{
    public partial class Game : PhoneApplicationPage
    {
        private PlayerLocation loc = new PlayerLocation();
        private DispatcherTimer dt;
        private List<CardData> _LocalCardDate;
        private ObservableCollection<string> _Logs;
        //private int pos = 8;
        int location = 0;
        public Game()
        {
            InitializeComponent();
            _LocalCardDate = new List<CardData>();
            _Logs = new ObservableCollection<string>();
            GamePanel.DataContext = loc;
            LstLogs.ItemsSource = _Logs;
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(5.0);
            dt.Tick += dt_Tick;
            dt.Start();
            App.client.RollDiceCompleted += client_RollDiceCompleted;
            App.client.EndOfTurnCompleted += client_EndOfTurnCompleted;
            App.client.GetStateCompleted += client_GetStateCompleted;
            App.client.BuyTileCompleted += client_BuyTileCompleted;
            App.client.GetLocalHouseDataCompleted += client_GetLocalHouseDataCompleted;
        }

        void client_BuyTileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Tile bought");
        }

        void client_GetLocalHouseDataCompleted(object sender, Monopoly.ServiceReference1.GetLocalHouseDataCompletedEventArgs e)
        {
            foreach (var item in e.Result.ToList())
            {
                _LocalCardDate.Add(ConvertToCardData(item));
            }
        }

        //get the card data from the database
        private CardData ConvertToCardData(Monopoly.ServiceReference1.CardDataToClient item)
        {
            CardData data = new CardData();
            data.BuyCost = item.BuyCost;
            data.Group = item.Group;
            data.HouseCost = item.HouseCost;
            data.ID = item.ID;
            data.Name = item.Name;
            data.Position = item.Position;
            data.Rent0 = item.Rent0;
            data.Rent1 = item.Rent1;
            data.Rent2 = item.Rent2;
            data.Rent3 = item.Rent3;
            data.Rent4 = item.Rent4;
            data.Rent5 = item.Rent5;
            data.Type = item.Type;

            return data;
        }


        void client_GetStateCompleted(object sender, Monopoly.ServiceReference1.GetStateCompletedEventArgs e)
        {
            if (location != e.Result.ActivePlayer.Location)
            {
                location = e.Result.ActivePlayer.Location;
                getLocation(e.Result.ActivePlayer.Location);
            }
            _Logs = e.Result.Log;
            
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
            while (PositionPlayer != 0)
            {
                if (loc.X != 0 && loc.Y == 9)
                {
                    PositionPlayer--;
                    loc.X--;
                }
                else if (loc.Y != 0 && loc.X == 0)
                {
                    PositionPlayer--;
                    loc.Y--;

                }
                else if (loc.Y == 0 && loc.X != 9)
                {
                    PositionPlayer--;
                    loc.X++;
                }
                else if (loc.X == 9 && loc.Y != 9)
                {
                    PositionPlayer--;
                    loc.Y++;
                }
            }
            return new PlayerLocation() { X = loc.X, Y = loc.Y };
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            loc.X = 9;
            loc.Y = 9;
        //    App.client.GetLocalHouseDataAsync();
            RollDice.Visibility = Visibility.Visible;
            BuyTile.Visibility = Visibility.Collapsed;
            EndPhase.IsEnabled = false;
        }

        private void RollDice_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.RollDiceAsync();
            EndPhase.IsEnabled = true;
            RollDice.Visibility = Visibility.Collapsed;
            BuyTile.Visibility = Visibility.Visible;
        }

        private void EndPhase_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.EndOfTurnAsync();
            EndPhase.IsEnabled = false;
            RollDice.IsEnabled = false;
            BuyTile.IsEnabled = false;
        }

        private void BuyTile_Click_1(object sender, RoutedEventArgs e)
        {
            EndPhase.IsEnabled = true;
            App.client.BuyTileAsync();
            BuyTile.Visibility = Visibility.Collapsed;
        }
    }
}