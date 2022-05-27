using Diplomapp.Views;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplomapp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
        public static string email;
        public static HttpClient client= new HttpClient();
        public static string BaseUrl = "https://webapplication120220422170733.azurewebsites.net/";
        public static string localUrl = "http://localhost:44314/";
        
        public static string accessToken;
        public static string userId;
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
