using Diplomapp.Models;
using Diplomapp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
namespace Diplomapp.ViewModels
{
    public class RegisterPageViewModel:BindableObject
    {
        public RegisterPageViewModel()
        {
            client = new HttpClient();
            Reg = new Command(reg);
        }
        private string email  = String.Empty;
        public string Email
        {
            get 
            { 
                return email; 
            } 
            set 
            {
                if (value == email) 
                        return;
                email = value ;
                OnPropertyChanged();
            } 
        }
        private string password = String.Empty;
        public string Password
        {
            get => password;
            set
            {
                if (value == password)
                    return;
                password = value;
                OnPropertyChanged();
            }
        }
        private string confirmpassword = String.Empty;
        public string ConfirmPassword
        {
            get
            {
                return confirmpassword;
            }
            set
            {
                if (value == confirmpassword)
                    return;
                confirmpassword = value;
                OnPropertyChanged();
            }
        }

        HttpClient client;
        public Command Reg { get; set; }
        public async void reg()
        {
            var user = new RegUser()
            {
                Email = Email,
                Password = Password,
                ConfirmPassword = ConfirmPassword
            };
            var json = JsonConvert.SerializeObject(user);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            
            
            var response = await  client.PostAsync(App.localUrl +"/api/Account/Register", content);
            

            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.DisplayAlert("Аккаунт успешно зарегестрирован", "Поздравляем", "Ок");
                await Shell.Current.GoToAsync("///" + nameof(LoginPage));
            }
            else 
            {
               await Shell.Current.DisplayAlert("Не удалось зарегестрировать аккаунт"," ","Ок");
            }
        }


    }
}
