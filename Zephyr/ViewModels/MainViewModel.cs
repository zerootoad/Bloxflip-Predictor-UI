using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Zephyr.Helpers;
using Zephyr.Core;

namespace Zephyr.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public RestSharpClient client;
        private bool _hasStarted = false;

        public MainViewModel()
        {
            client = new RestSharpClient("http://127.0.0.1:5000");
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
