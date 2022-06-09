using Diplomapp.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;

namespace Diplomapp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            GetMyProjects = new AsyncCommand(getMyProjects);
            GetProjects = new AsyncCommand(getProjects);
            Projects = new ObservableRangeCollection<Project>();
            MyProjects = new ObservableRangeCollection<Project>();
            ProjectGroups = new ObservableRangeCollection<Grouping<string, Project>>();
            GetAllUsers = new AsyncCommand(getid);
            command = new AsyncCommand(getlist);
            Empty = true;
        }
        bool empty;
        public bool Empty { get=>empty; set=>SetProperty(ref empty,value); }
        #region
        //public AsyncCommand GetCompanies { get; set; }
        public AsyncCommand GetAllUsers { get; set; }
        public AsyncCommand GetProjects { get; set; }
        public AsyncCommand GetMyProjects { get; set; }

        public ObservableRangeCollection<Project> Projects { get; set; }
        public ObservableRangeCollection<Project> MyProjects { get; set; }
        public ObservableRangeCollection<Grouping<string, Project>> ProjectGroups { get; set; }
        #endregion
        public async Task getlist()
        {
            ProjectGroups.Clear();
            MyProjects.Clear();
            Projects.Clear();
                    await GetMyProjects.ExecuteAsync();
                    if (MyProjects.Count != 0)
                    { 
                        ProjectGroups.Add(new Grouping<string, Project>("MyProjects", MyProjects));
                    }
                    await GetProjects.ExecuteAsync();
                    if (Projects.Count != 0) 
                    {
                        ProjectGroups.Add(new Grouping<string, Project>("Projects", Projects));  
                    }
            if (Projects.Count > 0 || MyProjects.Count > 0) { Empty = false; }
            else { Empty = true; }
        }
        public async Task getMyProjects() // получить все компании пользователя 
        {
            using (App.client = new HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);

                var response = await App.client.GetStringAsync(App.localUrl + "api/Projects");
                if (response != null) 
                {
                     var projects = JsonConvert.DeserializeObject<List<Project>>(response);
                    if(projects.Count != 0) {
                    MyProjects.AddRange(projects); }
                }
            }
        }
        public async Task getProjects()
        {
            using(App.client = new HttpClient())
            { 
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);

                var json = await  App.client.GetStringAsync(App.localUrl + "api/ProjectMembers");
                
                var members = JsonConvert.DeserializeObject<List<ProjectMember>>(json);
                if(members.Count != 0) 
                {
                    foreach (var member in members) 
                    {
                        json = null;
                        json = await App.client.GetStringAsync(App.localUrl + $"api/Projects/{member.ProjectID}");
                        var projects = JsonConvert.DeserializeObject<Project>(json);
                        if(projects != null) { 
                        Projects.Add(projects);}
                    } 
                }
                
            }
        }
        public AsyncCommand command { get; set; }
        //users
        #region
        internal class user
        {
            [JsonProperty("Email")]
            public string Email { get; set; }
            [JsonProperty("Id")]
            public string Id { get; set; }
            [JsonProperty("UserName")]
            public string UserName { get; set; }                               
            /*[JsonProperty("PhoneNumber")]
            public long PhoneNumber { get; set; }*/
        }//класс юзеров
        public async Task getid() // команда получения всех юзеров 
        {
            var client = new HttpClient(); 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);

            var json = await client.GetStringAsync(App.localUrl + "api/Account/GetId");

            var users = JsonConvert.DeserializeObject<string>(json);
            App.userId = users;
        }
        #endregion
       

    }
    
}
