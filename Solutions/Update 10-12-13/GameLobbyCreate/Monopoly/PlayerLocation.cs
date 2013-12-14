using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly
{
   public class PlayerLocation : INotifyPropertyChanged
    {
        private int x;
        public int X { get { return x; } set { x = value; OnPropertyChanged("X"); } }
        private int y;
        public int Y { get { return y; } set { y = value; OnPropertyChanged("Y"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
