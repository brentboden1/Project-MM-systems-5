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
        ServiceReference1.Service1Client client;
        ServiceReference1.Player p;
        
        // Constructor
        public MainPage()
        {
            (App.Current as App).client = new ServiceReference1.Service1Client();
            InitializeComponent();
            client = new ServiceReference1.Service1Client();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void Add_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                client.AddPlayerCompleted += client_AddPlayerCompleted;
                client.AddPlayerAsync(Name.Text);
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void client_GetPlayersCompleted(object sender, ServiceReference1.GetPlayersCompletedEventArgs e)
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
          //  client.AddPlayerAsync(p);
            client.GetPlayersCompleted += client_GetPlayersCompleted;
            client.GetPlayersAsync();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}