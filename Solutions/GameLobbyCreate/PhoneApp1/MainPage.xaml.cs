using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Resources;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
                InitializeComponent();
                App.client.AddPlayerCompleted += client_AddPlayerCompleted;
                App.client.GetPlayerByNameCompleted += client_GetPlayerByNameCompleted;
        }

        private void Login_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                App.client.AddPlayerAsync(TxbPlName.Text.ToString());

            }
            catch (TargetInvocationException)
            {
                MessageBox.Show("Please try another Nickname \n This one already exists");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void client_AddPlayerCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                App.client.GetPlayerByNameAsync(TxbPlName.Text.ToString());
            }
            catch (TargetInvocationException)
            {
                MessageBox.Show("Please try another Nickname \n This one already exists");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        void client_GetPlayerByNameCompleted(object sender, ServiceReference1.GetPlayerByNameCompletedEventArgs e)
        {
            try
            {
                App.Me = e.Result;
                NavigationService.Navigate(new Uri("/Lobby.xaml", UriKind.Relative));
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please try another Nickname \n This one already exists");
            }
            catch (TargetInvocationException)
            {
                MessageBox.Show("Please try another Nickname \n This one already exists");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageBrush background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("/Images/Monopoly.png", UriKind.Relative))
                };
                background.Stretch = Stretch.Fill;
                Monopoly.Background = background;
            }
            catch (WebBrowserNavigationException)
            {
                MessageBox.Show("There is no internet, please turn it on");
            }
            catch (WebException)
            {
                MessageBox.Show("There is no internet, please turn it on");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
