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
        public Game()
        {
            dt = new DispatcherTimer();
            InitializeComponent();
            GamePanel.DataContext = loc;
            dt.Interval = TimeSpan.FromMilliseconds(100.0);
            dt.Tick+=dt_Tick;
            dt.Start();
        }

        void dt_Tick(object sender, EventArgs e)
        {
            getLocation(pos);
        }

        private void getLocation(int PositionPlayer)
        {
            if (PositionPlayer != 0)
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
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            loc.X = 9;
            loc.Y = 9;
        }
    }
}