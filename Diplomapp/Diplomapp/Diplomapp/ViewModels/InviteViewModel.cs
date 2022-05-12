using Diplomapp.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Diplomapp.ViewModels
{
    public class InviteViewModel : BaseViewModel
    {
        public InviteViewModel()
        {
            Invites = new ObservableRangeCollection<Invite>();
            command = new AsyncCommand(getinv);
            Acceptinv = new AsyncCommand<Invite>(acceptinv);
            Decline = new AsyncCommand<Invite> (decline);
        }
        public ObservableRangeCollection<Invite> Invites { get; set; }
        public AsyncCommand command { get; set; }
        public async Task getinv()
        {
            using (App.client = new HttpClient())
            {
                Invites.Clear();
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                var json = await App.client.GetStringAsync(App.localUrl + "api/Invites");
                var invites = JsonConvert.DeserializeObject<List<Invite>>(json);
                if (invites.Count > 0)
                {
                    Invites.AddRange(invites);
                }

            }
        }
        public AsyncCommand<Invite> Acceptinv { get; set; }
        async Task acceptinv(Invite invite)
        {
            using (App.client = new HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                var mem = new ProjectMember() { UserID = invite.Userid,ProjectID = invite.Projectid , Position = invite.Position, Role = invite.Role,UserName = App.email};
                var json = JsonConvert.SerializeObject(mem);
                var con = new StringContent(json);
                con.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var res = await App.client.PostAsync(App.localUrl + $"api/ProjectMembers", con);
                if (res.IsSuccessStatusCode) 
                {
                    var res2 = await App.client.DeleteAsync(App.localUrl + "api/Invites/" + invite.Id);
                    Invites.Remove(invite);
                    await AppShell.Current.DisplayAlert("Приглашение принято",$"Вы были добавлены в рабочую группу проекта{invite.ProjectName}","ok");
                    
                }
            }
        }
        public AsyncCommand<Invite> Decline { get; set; }
        async Task decline(Invite invite)
        {
            using (App.client = new HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                var res = await App.client.DeleteAsync(App.localUrl + "api/Invites/" + invite.Id);

            }
                Invites.Remove(invite);
        }
    }

    

}
