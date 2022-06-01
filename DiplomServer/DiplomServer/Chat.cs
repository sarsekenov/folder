using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using DiplomServer.Models;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace DiplomServer
{
    
    public class Chat : Hub
    {
        List<ChatUser> Users = new List<ChatUser>();
        public void Hello()
        {
            Clients.All.hello();
        }
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }

        // Подключение нового пользователя
        public void Connect(string userName)  
        {
            var id = Context.ConnectionId;
            if (Users.Count(x => x.ConnectionId == id) == 0)
            {
                Users.Add(new ChatUser { ConnectionId = id, Name = userName });

                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(id, userName, Users);

                // Посылаем сообщение всем пользователям, кроме текущего
                Clients.AllExcept(id).onNewUserConnected(id, userName);
            }
        }

        // Отключение пользователя
        /*public override System.Threading.Tasks.Task OnDisconnected()
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Name);
            }

            return base.OnDisconnected();
        }*/

    }
}
 
