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

namespace PhoneApp1
{
    public partial class Game : PhoneApplicationPage
    {
        private PlayerLocation loc = new PlayerLocation();
        private DispatcherTimer dt;
        private int pos = 8;
        int location = 0;
        public Game()
        {
            dt = new DispatcherTimer();
            InitializeComponent();
            GamePanel.DataContext = loc;
            dt.Interval = TimeSpan.FromSeconds(5.0);
            dt.Tick += dt_Tick;
            dt.Start();
            App.client.RollDiceCompleted += client_RollDiceCompleted;
            App.client.EndOfTurnCompleted += client_EndOfTurnCompleted;
            App.client.GetStateCompleted += client_GetStateCompleted;
        }

        void client_GetStateCompleted(object sender, ServiceReference1.GetStateCompletedEventArgs e)
        {
            if (location != e.Result.ActivePlayer.Location)
            {
                location = e.Result.ActivePlayer.Location;
                getLocation(e.Result.ActivePlayer.Location);
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
            while (PositionPlayer != 0)
            {
                if (loc.X != 9 && loc.Y == 9)
                {
                    PositionPlayer--;
                    loc.X++;
                }
                else if (loc.Y != 0 && loc.X == 9)
                {
                    PositionPlayer--;
                    loc.Y--;

                }
                else if (loc.X != 0 && loc.Y == 0)
                {
                    PositionPlayer--;
                    loc.X--;
                }
                else if (loc.Y != 9 && loc.X == 0)
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
        }

        private void RollDice_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.RollDiceAsync();
        }

        private void EndPhase_Click_1(object sender, RoutedEventArgs e)
        {
            App.client.EndOfTurnAsync();
        }
    }
}