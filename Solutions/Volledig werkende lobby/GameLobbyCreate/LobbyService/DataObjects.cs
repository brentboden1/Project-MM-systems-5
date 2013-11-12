using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LobbyService
{
    public class DataObjects : INotifyPropertyChanged
    {
        private int playerID;
        public int PlayerID
        {
            get
            {
                return playerID;
            }
            set
            {
                playerID = value;
                OnPropertyChanged("PlayerID");
            }
        }

        private int lobbyID;
        public int LobbyID 
        {
            get
            {
                return lobbyID;
            }
            set
            {
                lobbyID = value;
                OnPropertyChanged("LobbyID");
            }
        }

        private string playerName;
        public string PlayerName 
        {
            get
            {
                return playerName;
            }
            set
            {
                playerName = value;
                OnPropertyChanged("PlayerName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
