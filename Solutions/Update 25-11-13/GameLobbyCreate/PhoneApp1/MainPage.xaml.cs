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

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Login_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
               App.client.AddPlayerCompleted += client_AddPlayerCompleted;
               App.client.AddPlayerAsync(TxbPlName.Text.ToString());

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
                App.client.GetPlayerByNameCompleted += client_GetPlayerByNameCompleted;
                App.client.GetPlayerByNameAsync(TxbPlName.Text.ToString());
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
