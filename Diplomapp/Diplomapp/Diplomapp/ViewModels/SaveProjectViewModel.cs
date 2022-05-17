using Diplomapp.Models;
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
    
    [QueryProperty(nameof(Name), "name")]// Project Name
    [QueryProperty(nameof(Id), "Id")] // Project Id
    [QueryProperty(nameof(OwnerId), "OwnerId")]
    [QueryProperty(nameof(Description), "Description")]
    class SaveProjectViewModel : BaseViewModel
    {
        string name;
        int id;
        public string Name { get => name; set => SetProperty(ref name, value); }
        public int Id { get => id; set => SetProperty(ref id, value); }
        string description;
        string ownerId;
        public string OwnerId { get => ownerId; set => SetProperty(ref ownerId, value); }
        public string Description { get => description; set => SetProperty(ref description, value); }
        public SaveProjectViewModel()
        {
            SendInfo = new AsyncCommand(sendinfo);
            ProjectInfo = new ProjectInfo();
        }
        ProjectInfo pro;
        public ProjectInfo ProjectInfo { get=>pro; set=>SetProperty(ref pro,value); }
        public AsyncCommand SendInfo { get; set; }
        async Task sendinfo()
        {
            using (App.client = new System.Net.Http.HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",App.accessToken);

                ProjectInfo.ProjectId = Id;
                
                var json = JsonConvert.SerializeObject(ProjectInfo);
                var con = new StringContent(json);
                con.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var res = await App.client.PostAsync(App.localUrl + $"api/ProjectInfoes", con);
                if (res.IsSuccessStatusCode) 
                {
                    await Shell.Current.GoToAsync($"{nameof(ProjectPage)}?name={Name}&OwnerId={OwnerId}&Description={Description}&Id={Id}");
                }

                
            }
            
        }
    }
}
