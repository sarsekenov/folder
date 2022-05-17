using Diplomapp.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Diplomapp.ViewModels
{
    class UserInfo : BaseViewModel
    {

        public UserInfo()
        {
            Info = new Models.UserInfo();
            getinfo = new AsyncCommand(getin);
            com = new AsyncCommand(comm);
        }
        public AsyncCommand com { get; set; }
        async Task comm()
        {
            using (App.client = new System.Net.Http.HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                Info.UserId = App.userId;
                var json = JsonConvert.SerializeObject(Info);
                var con = new StringContent(json);
                con.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var res = await App.client.PostAsync(App.localUrl + "api/UserInfoes", con);
                if (res.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Done", "", "Ok");
                }
            }

        }
        public AsyncCommand getinfo { get; set; }
        async Task getin() 
        {
            using (App.client = new System.Net.Http.HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var json = await App.client.GetAsync(App.localUrl + $"api/UserInfoes?id={App.userId}");
                if (json.IsSuccessStatusCode)
                {
                    var json2 = await json.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<Models.UserInfo>(json2);
                    if (res != null)
                    {
                        Info = res;
                    }
                }
                else { }
                
            }

        }
            Models.UserInfo info;
        public Models.UserInfo Info { get=>info; set=>SetProperty(ref info,value); }
    }
}
