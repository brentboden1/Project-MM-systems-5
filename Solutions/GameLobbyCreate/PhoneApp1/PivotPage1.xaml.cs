using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace PhoneApp1
{
    public partial class PivotPage1 : PhoneApplicationPage
    {
        private SolidColorBrush kleur;
        private ObservableCollection<string> Log;
        private bool Morguage;
        private byte SetLevel;
        private byte HouseLevel;
        private PositionData temporary;
        private ObservableCollection<RollDIceData> Dice;
        private int OrderNumberPlayer = 0;
        private string PlayerName;
        private ObservableCollection<PositionData> PropData;
        private CardData temp = new CardData();
        private List<CardData> localData;
        private ObservableCollection<PositionData> PosData;
        private PlayerLocation loc = new PlayerLocation();
        private DispatcherTimer dt;
        int location = 0;
        public PivotPage1()
        {
            kleur = new SolidColorBrush();
            Dice = new ObservableCollection<RollDIceData>();
            PropData = new ObservableCollection<PositionData>();
            PosData = new ObservableCollection<PositionData>();
            Log = new ObservableCollection<string>();
            dt = new DispatcherTimer();
            localData = new List<CardData>();
            InitializeComponent();
            lstLog.ItemsSource = Log;
            lstDice.ItemsSource = Dice;
            lstProps.ItemsSource = PropData;
            lstInformation.ItemsSource = PosData;
            GamePanel.DataContext = loc;
            dt.Interval = TimeSpan.FromSeconds(5.0);
            dt.Tick += dt_Tick;
            dt.Start();
            App.client.RollDiceCompleted += client_RollDiceCompleted;
            App.client.EndOfTurnCompleted += client_EndOfTurnCompleted;
            App.client.GetStateCompleted += client_GetStateCompleted;
            App.client.BuyTileCompleted += client_BuyTileCompleted;
            App.client.EscapePrisonCompleted += client_EscapePrisonCompleted;
            App.client.BuyHouseCompleted += client_BuyHouseCompleted;
            App.client.GetGamePlayerByOrderCompleted += client_GetGamePlayerByOrderCompleted;
            App.client.GetLocalHouseDataCompleted += client_GetLocalHouseDataCompleted;
            App.client.GetLocalHouseDataAsync();
            App.client.OrderNumCompleted += client_OrderNumCompleted;
            App.client.OrderNumAsync(App.Me);
        }

        void client_BuyHouseCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("House Bought");
        }

        void client_EscapePrisonCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Escaped Prison");
        }

        void client_GetGamePlayerByOrderCompleted(object sender, ServiceReference1.GetGamePlayerByOrderCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                PlayerName = e.Result;
            }
            else
            {
                PlayerName = null;
            }
        }

        void client_BuyTileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Tile Bought");
        }

        void client_OrderNumCompleted(object sender, ServiceReference1.OrderNumCompletedEventArgs e)
        {
            try
            {
                OrderNumberPlayer = e.Result;
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

                PosData.Clear();
                PropData.Clear();
                Dice.Clear();
                if (location != e.Result.ActivePlayer.Location)
                {
                    location = e.Result.ActivePlayer.Location;
                    getLocation(location);
                    // MessageBox.Show(location.ToString());
                }
                if (e.Result.Log != null)
                {
                    Log.Clear();
                    foreach (var item in e.Result.Log)
                    {
                        Log.Add(item);
                    }
                }
                bool test = false;

                foreach (var item in e.Result.OwnedProps)
                {
                    Morguage = item.Morguage;
                    SetLevel = item.SetLevel;
                    HouseLevel = item.HouseLevel;
                }

                foreach (var item in localData)
                {
                    switch (item.Group)
                    {
                        case 1: kleur = new SolidColorBrush(Colors.Purple);
                            break;
                        case 2: kleur = new SolidColorBrush(Colors.Blue);
                            break;
                        case 3: kleur = new SolidColorBrush(Colors.Magenta);
                            break;
                        case 4: kleur = new SolidColorBrush(Colors.Orange);
                            break;
                        case 5: kleur = new SolidColorBrush(Colors.Red);
                            break;
                        case 6: kleur = new SolidColorBrush(Colors.Yellow);
                            break;
                        case 7: kleur = new SolidColorBrush(Colors.Green);
                            break;
                        case 8: kleur = new SolidColorBrush(Colors.Black);
                            break;
                        case 9: kleur = new SolidColorBrush(Colors.LightGray);
                            break;
                        default: kleur = new SolidColorBrush(Colors.White);
                            break;
                    }
                    if (item.Position == location)
                    {
                        test = true;
                        string temp = null;
                        if (e.Result.IsBought[item.ID])
                        {
                            App.client.GetGamePlayerByOrderAsync((byte)e.Result.Ownership[item.ID]);
                            temp = "is al verkocht aan " + PlayerName;
                        }
                        else
                        {
                            temp = "is nog te koop";
                        }

                        PosData.Add(new PositionData() { Name = item.Name, Cost = item.BuyCost, Group = item.Group.ToString(), HouseCost = item.HouseCost.ToString(), Rent0 = item.Rent0, Rent1 = item.Rent1, Rent2 = item.Rent2, Rent3 = item.Rent3, Rent4 = item.Rent4, Rent5 = item.Rent5, Owner = temp, Kleur = kleur });

                    }

                    if (e.Result.IsBought[item.ID])
                    {
                        if (e.Result.Ownership[item.ID] == OrderNumberPlayer)
                        {
                            PropData.Add(new PositionData() { Cost = item.BuyCost, Group = item.Group.ToString(), HouseCost = item.HouseCost.ToString(), Name = item.Name, Rent0 = item.Rent0, Rent1 = item.Rent1, Rent2 = item.Rent2, Rent3 = item.Rent3, Rent4 = item.Rent4, Rent5 = item.Rent5, Morguage = Morguage, HouseLevel = HouseLevel, SetLevel = SetLevel, Kleur = kleur });
                        }
                    }
                }
                if (!test)
                {

                    PosData.Add(new PositionData() { Name = e.Result.ActiveTileName });
                }

                foreach (var item in e.Result.lastDieRoll)
                {
                    Dice.Add(new RollDIceData() { DiceRoll = " Dice : " + item.ToString() });
                }

                if (e.Result.ActivePlayer.IsPrison)
                {
                    EscapePrison.Visibility = Visibility.Visible;
                }

                MoneyBox.Text = "Money : " + e.Result.ActivePlayer.Cash.ToString();
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

        private void RollDice_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.RollDiceAsync();
            BuyTile.Visibility = Visibility.Visible;
            RollDice.Visibility = Visibility.Collapsed;
            EndPhase.IsEnabled = true;
        }

        private void EndPhase_Click_1(object sender, RoutedEventArgs e)
        {
            RollDice.Visibility = Visibility.Visible;
            BuyTile.Visibility = Visibility.Collapsed;
            EndPhase.IsEnabled = false;
            App.client.EndOfTurnAsync();
        }

        private void BuyTile_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.BuyTileAsync();
            BuyTile.Visibility = Visibility.Collapsed;
            RollDice.Visibility = Visibility.Visible;
        }

        private void PhoneApplicationPage_Loaded_2(object sender, RoutedEventArgs e)
        {
            ImageBrush background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("/Images/Board.png", UriKind.Relative))
            };
            background.Stretch = Stretch.Fill;
            GamePanel.Background = background;
            loc.X = 10;
            loc.Y = 10;
        }

        private void BuyHouse_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                temporary = (PositionData)(sender);
                byte tempID = 0;
                foreach (var item in localData)
                {
                    if (item.Name == temporary.Name)
                    {
                        tempID = (byte)item.ID;
                        App.client.BuyHouseAsync(App.Me, tempID);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void EscapePrison_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.EscapePrisonAsync(App.Me);
        }
    }
}