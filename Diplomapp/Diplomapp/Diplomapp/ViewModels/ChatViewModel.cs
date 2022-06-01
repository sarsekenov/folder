using Microsoft.AspNet.SignalR.Client;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
namespace Diplomapp.ViewModels
{
    public  class ChatViewModel:BaseViewModel
    {
        public ChatViewModel() 
        {
            connection = new HubConnection(App.localUrl + "/chat");
            connection.ConnectionId = App.userId;
            
            connection.Start();
        }
        HubConnection connection;
    }
}
