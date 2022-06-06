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
    [QueryProperty(nameof(Id), "Id")]
    class UserInfo : BaseViewModel
    {

        public UserInfo()
        {
            getinfo = new AsyncCommand(getin);
            com = new AsyncCommand(comm);
        }
        string id;
        public string Id { get => id; set => SetProperty(ref id,value); }
        public AsyncCommand com { get; set; }
        async Task comm()
        {
            using (App.client = new System.Net.Http.HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var json = JsonConvert.SerializeObject(new Models.UserInfo { AboutMe = AboutMe, UserId = App.userId, Name = Name, PhoneNumber = PhoneNumber }) ;
                var con = new StringContent(json);
                con.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var res = await App.client.PostAsync(App.localUrl + "api/UserInfoes", con);
                if (res.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Done", "", "Ok");
                }
            }
        }
        string name;
        public string Name { get => name; set => SetProperty(ref name, value); }
        string phonenumber;
        public string PhoneNumber { get => phonenumber; set => SetProperty(ref phonenumber, value); }
        string aboutme;
        public string AboutMe { get => aboutme; set => SetProperty(ref aboutme, value); }
        public AsyncCommand getinfo { get; set; }
        async Task getin()
        {
            using (App.client = new System.Net.Http.HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);

                if (Id == null)
                {
                    var json = await App.client.GetAsync(App.localUrl + $"api/UserInfoes");
                    if (json.IsSuccessStatusCode)
                    {
                        var json2 = await json.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<List<Models.UserInfo>>(json2);
                        if (res.Count > 0)
                        {
                            Name = res[res.Count - 1].Name;
                            PhoneNumber = res[res.Count - 1].PhoneNumber;
                            AboutMe = res[res.Count - 1].AboutMe;
                        }
                        else
                        {
                            Name = null;
                            PhoneNumber = null;
                            AboutMe = null;
                        }
                    }
                }
                else
                {
                    var json2 = await App.client.GetAsync(App.localUrl + $"getinfobyid?Id={Id}");
                    if (json2.IsSuccessStatusCode)
                    {
                        var json3 = await json2.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<List<Models.UserInfo>>(json3);
                        if (res.Count > 0)
                        {
                            Name = res[res.Count - 1].Name;
                            PhoneNumber = res[res.Count - 1].PhoneNumber;
                            AboutMe = res[res.Count - 1].AboutMe;
                        }
                        else
                        {
                            Name = null;
                            PhoneNumber = null;
                            AboutMe = null;
                        }
                    }
                }
            }
        }
    }
}
