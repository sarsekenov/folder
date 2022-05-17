using Diplomapp.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Diplomapp.ViewModels
{
    [QueryProperty(nameof(Name), "name")]
    [QueryProperty(nameof(Id), "Id")]
    public class CreateInviteViewModel: BaseViewModel
    {
        public CreateInviteViewModel() 
        {
            getemail = new AsyncCommand(getlistofUsers);
            Selecteditem = new AsyncCommand<user>(selected);
            sendInvite = new AsyncCommand(sendinv);
            Users = new ObservableRangeCollection<user>();
            
        }

        string name;
        int id;
        public string Name { get => name; set => SetProperty(ref name, value); }
        public int Id { get => id; set => SetProperty(ref id, value); }

        string role;
        string position;
        public string Role { get => role; set => SetProperty(ref role, value); }
        public string Position { get => position; set => SetProperty(ref position, value); }
        
        string email;
        Invite invite;
        public Invite Invite { get=>invite; set=>SetProperty(ref invite,value); }
        string invitation;
        public string Invitation { get => invitation; set => SetProperty(ref invitation, value); }
        public async Task getlistofUsers() 
        {
            using (App.client = new System.Net.Http.HttpClient())
            {
                Users.Clear();
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                //HttpContent content = new StringContent(Email);
                if(Email != null)
                {
                    var res = await App.client.GetStringAsync(App.localUrl + $"api/Account/GetUsers?Name={Email}");
                    var userlist = JsonConvert.DeserializeObject<List<user>>(res);
                    
                    Users.AddRange(userlist.Where(c=>c.UserName!=App.email));
                }
                
            }
        }
        public ObservableRangeCollection<user> Users { get; set; }
        
        public AsyncCommand getemail { get; set; }
        public AsyncCommand sendInvite { get; set; }
        public async Task sendinv() 
        {

            using (App.client = new HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                var j_id = await App.client.GetStringAsync(App.localUrl + $"api/Account/Getid?mail={Email}");
                var id = JsonConvert.DeserializeObject<string>(j_id);
                var j_mail = await App.client.GetStringAsync(App.localUrl + "api/Account/Getmail");
                var mail = JsonConvert.DeserializeObject<string>(j_mail);
                Invite = new Invite() { Projectid = Id, ProjectName = Name, Userid = id, OwnerEmail = mail, Invitation = Invitation, Role = Role,Position = Position};
                var con = JsonConvert.SerializeObject(Invite);
                var content = new StringContent(con);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var res = await App.client.PostAsync(App.localUrl + "api/Invites", content);
                if (res.StatusCode == System.Net.HttpStatusCode.Created) 
                {
                     await Shell.Current.DisplayAlert("Pozdravlayem", "Priglashenie otpravleno", "Ok");
                }
            }

        }
        public string Email { get=>email; set=>SetProperty(ref email,value); }
        async Task selected(user user)
        {
            if (user == null)
                return;
            Email = user.Email;
            Users.Clear();
        }
        
        public AsyncCommand<user> Selecteditem { get; set; }
    }
    public class user
    {
            [JsonProperty("Email")]
            public string Email { get; set; }
            [JsonProperty("Id")]
            public string Id { get; set; }
            [JsonProperty("UserName")]
            public string UserName { get; set; }
            /*[JsonProperty("PhoneNumber")]
            public long PhoneNumber { get; set; }*/
    }
}
