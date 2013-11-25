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
        private ServiceReference1.Service1Client client;

        public MainPage()
        {
            InitializeComponent();
            client = new ServiceReference1.Service1Client();
        }

        private void Login_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                client.AddPlayerCompleted += client_AddPlayerCompleted;
                client.AddPlayerAsync(TxbPlName.ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void client_AddPlayerCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //  client.AddPlayerAsync(p);
            NavigationService.Navigate(new Uri("/Lobby.xaml", UriKind.Relative));
        }
    }
}
