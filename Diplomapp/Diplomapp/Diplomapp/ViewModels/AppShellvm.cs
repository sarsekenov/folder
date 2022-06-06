using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diplomapp.ViewModels
{
    public class AppShellvm:BaseViewModel
    {
        public AppShellvm()
        {
            
        }
        string username;
        public string Username { get => username; set => SetProperty(ref username, value); }
    }
}
