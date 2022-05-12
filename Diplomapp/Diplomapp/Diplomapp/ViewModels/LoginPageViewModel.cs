using Diplomapp.Views;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Diplomapp.ViewModels
{
    public class LoginPageViewModel:BindableObject
    {
        public LoginPageViewModel()
        {
            register = new AsyncCommand(Registeraction);
            login = new AsyncCommand(Login); 
        }
        string username;
        string password;
        public string Username { get { return username; } 
            set 
            {
                if(value == username)
                    return;
                username = value;
                OnPropertyChanged();
            } }
        public string Password
        {
            get { return password; }
            set
            {
                if (value == password)
                    return;
                password = value;
                OnPropertyChanged();
            }
        }
        public AsyncCommand login { get; set; }
        public AsyncCommand register { get; set; }
        async Task Registeraction() 
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
        internal class Token
            {
                [JsonProperty("access_token")]
                public string AccessToken { get; set; }

                [JsonProperty("token_type")]
                public string TokenType { get; set; }

                [JsonProperty("expires_in")]
                public int ExpiresIn { get; set; }

                [JsonProperty("refresh_token")]
                public string RefreshToken { get; set; }
            }
        async Task Login()
        {
            var keyValues = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username", Username),
                new KeyValuePair<string, string>("password", Password),
                new KeyValuePair<string, string>("grant_type", "password")
            };
            var request = new HttpRequestMessage(HttpMethod.Post, App.localUrl + "Token");

            request.Content = new FormUrlEncodedContent(keyValues);

            var client = new HttpClient();
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                var pairs = JsonConvert.DeserializeObject<Token>(content);
                if (pairs.AccessToken != null)
                {
                    App.accessToken = pairs.AccessToken;
                    App.email = Username;
                    await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
                }
                else 
                {
                    await Application.Current.MainPage.DisplayAlert("Error ", "No user with this username and password","Ok");
                }
            }
            catch 
            {
                await Application.Current.MainPage.DisplayAlert("Error ", "Server isnt working", "Ok");
            }
            
        }

    }
    
}
