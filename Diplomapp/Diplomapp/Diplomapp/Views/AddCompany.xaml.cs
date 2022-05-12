using Diplomapp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplomapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCompany : ContentPage
    {
        public AddCompany()
        {
            InitializeComponent();
        }
        async Task createProject()
        {
            using (App.client  = new HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                var json = JsonConvert.SerializeObject(new Project() { Name = NameProject.Text, OwnerId = "0", Description = DescriptionProject.Text  });
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await App.client.PostAsync(App.localUrl + "api/Projects", content);
                string res = await response.Content.ReadAsStringAsync();
                Project = JsonConvert.DeserializeObject<Project>(res);
                await Shell.Current.GoToAsync($"{nameof(ProjectPage)}?name={Project.Name}&OwnerId={Project.OwnerId}&Description={Project.Description}&Id={Project.Id}");
                
                /*json = JsonConvert.SerializeObject(new CompanyMember() { CompanyId = newres.Id, UserId = newres.OwnerId, Role = "Admin" });
                content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(App.localUrl + "api/CompanyMembers", content);
                var res1 = await response.Content.ReadAsStringAsync();
                var newres1 = JsonConvert.DeserializeObject<CompanyMember>(res1);*/
                
            }
        }
        public static  Project Project { get; set; }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await createProject();
        }

    }
}