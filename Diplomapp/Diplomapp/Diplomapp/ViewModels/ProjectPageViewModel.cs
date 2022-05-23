using Diplomapp.Models;
using Diplomapp.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Net.Http;

namespace Diplomapp.ViewModels
{
    [QueryProperty(nameof(Name), "name")]// Project Name
    [QueryProperty(nameof(Id), "Id")] // Project Id
    [QueryProperty(nameof(OwnerId), "OwnerId")]
    [QueryProperty(nameof(Description), "Description")]
    public class ProjectPageViewModel : BaseViewModel
    {
        string name;
        int id;
        public string Name { get => name; set => SetProperty(ref name, value); }
        public int Id { get => id; set => SetProperty(ref id, value); }
        string description;
        string ownerId;
        public string OwnerId { get => ownerId; set => SetProperty(ref ownerId, value); }
        public string Description { get => description; set => SetProperty(ref description, value); }

        //Project project;
        //public Project Project { get=>project; set=> SetProperty(ref project,value); }
        public ProjectPageViewModel()
        {
            AddSal = new AsyncCommand(addSal);
            initialize = new AsyncCommand(Init);
            GetProblems = new AsyncCommand(getProblems);
            CreateTask = new AsyncCommand(createTask);
            invite = new AsyncCommand(inviteuser);
            getempl = new AsyncCommand(GetEmployees);
            Members = new ObservableRangeCollection<ProjectMember>();
            Members.Clear();
            oborudovanies = new ObservableRangeCollection<oborudovanie>();
            Salaries = new ObservableRangeCollection<Salary>();
            Problems = new ObservableRangeCollection<Problem>();
            GetSalaries = new AsyncCommand(getSalaries);
            SelectedProblem = new AsyncCommand<Problem>(selectedProblem);
            Getoborudovanies = new AsyncCommand(getobor);
            Pick = new AsyncCommand(pickFile);
            ProjectInfo = new ProjectInfo();
            GetInfo = new AsyncCommand(getinfo);
            ProjectMembers = new List<ProjectMember>();
            SelectedMember = new ProjectMember();
            Addobor = new AsyncCommand(addobor);
        }
        async Task getinfo()
        {
            using (App.client = new System.Net.Http.HttpClient()) 
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var res = await App.client.GetAsync(App.localUrl + $"api/ProjectInfoes?id={Id}");
                
                if (res.IsSuccessStatusCode) 
                {
                    var json = await res.Content.ReadAsStringAsync();
                    var info = JsonConvert.DeserializeObject<ProjectInfo>(json);
                    ProjectInfo = info;
                }
            }
        }
        public AsyncCommand GetInfo { get; set; }
        ProjectInfo projectinfo;
        public ProjectInfo ProjectInfo { get => projectinfo; set => SetProperty(ref projectinfo, value); }
        public ObservableRangeCollection<Salary> Salaries { get; set; }
        public ObservableRangeCollection<oborudovanie> oborudovanies { get; set; }
        
        public AsyncCommand GetSalaries { get; set; }
        public async Task getSalaries()
        {
            using (App.client = new System.Net.Http.HttpClient()) 
            {
                
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var res = await App.client.GetAsync(App.localUrl + $"GetProjectSalaries?id={Id}");
                if (res.IsSuccessStatusCode) 
                {
                    var result = await res.Content.ReadAsStringAsync();
                    var salaries = JsonConvert.DeserializeObject<List<Salary>>(result);
                    foreach (var sal in salaries) 
                    {
                        foreach (var mem in Members) 
                        {
                            if (mem.UserID == sal.UserId) 
                            {
                                sal.UserId = mem.UserName;
                            }
                        }
                    }
                    if (salaries.Count > 0) 
                    {
                        
                        Salaries.Clear();
                        Salaries.AddRange(salaries);
                    }
                }
                
            }
        }
        public AsyncCommand Getoborudovanies { get; set; }
        public async Task getobor() 
        {
            using (App.client = new System.Net.Http.HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var res = await App.client.GetAsync(App.localUrl + $"GetProjectoborudovanies?id={Id}");
                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsStringAsync();
                    var salaries = JsonConvert.DeserializeObject<List<oborudovanie>>(result);
                    oborudovanies.Clear();
                    oborudovanies.AddRange(salaries);
                }
            }
        }

        public async Task Init() 
        {
            await GetInfo.ExecuteAsync();
            await getempl.ExecuteAsync();
            await GetSalaries.ExecuteAsync();
            await Getoborudovanies.ExecuteAsync();
        }
        public AsyncCommand initialize { get; set; }
        public AsyncCommand getempl { get; set; }
        public async Task inviteuser() 
        {
            await Shell.Current.GoToAsync(nameof(CreateInvitePage)+$"?name={Name}&Id={Id}");//передаем значения в форму 
        }
        public async Task createTask()
        {
            await Shell.Current.GoToAsync($"{nameof(TaskDetailPage)}?Id={Id}");
        }
        public AsyncCommand Pick { get; set; }
        public async Task pickFile() 
        {
            var files = await Xamarin.Essentials.FilePicker.PickMultipleAsync();
            if (files == null)
                return;
            foreach (var file in files) 
            {
              var stream =  await file.OpenReadAsync();
              
            }
        }
        string stav;
        public string Stavka { get => stav; set => SetProperty(ref stav, value); }
        string zpa;
        public string Zp { get => zpa; set => SetProperty(ref zpa, value); }
        async Task addSal()
        {
            using (App.client = new System.Net.Http.HttpClient()) 
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",App.accessToken);
                var sal = new Salary() { ProjectId = Id, stavka = Convert.ToSingle(Stavka), UserId = SelectedMember.UserID, zp = Convert.ToSingle(Zp) };
                var json = JsonConvert.SerializeObject(sal);
                var con = new StringContent(json);
                con.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var res = await App.client.PostAsync(App.localUrl + "api/Salaries", con);
                if (res.IsSuccessStatusCode) 
                {
                    var dson = await res.Content.ReadAsStringAsync();
                    var salar = JsonConvert.DeserializeObject<Salary>(dson);
                    salar.UserId = SelectedMember.UserName;
                    Salaries.Add(salar);
                }   
            }
        }
        string oborname;
        public string OborName { get=>oborname; set=>SetProperty(ref oborname,value); }
        string measure;
        public string Measure { get => measure; set => SetProperty(ref measure, value); }
        float count;
        public float Count { get => count; set => SetProperty(ref count, value); }
        float price;
        public float Price { get => price; set => SetProperty(ref price, value); }
        public AsyncCommand Addobor { get; set; }
        async Task addobor() 
        {
            using (App.client = new System.Net.Http.HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var sal = new oborudovanie() { ProjectId = Id, Count = Count, Measure = Measure, Name = OborName, Price= Price};
                var json = JsonConvert.SerializeObject(sal);
                var con = new StringContent(json);
                con.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var res = await App.client.PostAsync(App.localUrl + "api/oborudovanies", con);
                if (res.IsSuccessStatusCode)
                {
                    var dson = await res.Content.ReadAsStringAsync();
                    var salar = JsonConvert.DeserializeObject<oborudovanie>(dson);
                    oborudovanies.Add(salar);
                }
            }
        }
        public ProjectMember SelectedMember { get; set; }
        public AsyncCommand AddSal { get; set; }
        public AsyncCommand CreateTask { get; set; }
        public async Task GetEmployees() // Получаем всех работников этого проекта 
        {
            Members.Clear();
            ProjectMembers.Clear();
            using (App.client = new System.Net.Http.HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var json = await App.client.GetAsync(App.localUrl + $"GetbyId?Id={Id}");// обращаемся к контроллеру 
                if (json.IsSuccessStatusCode) 
                {
                    var result = await json.Content.ReadAsStringAsync();
                    var employees = JsonConvert.DeserializeObject<List<ProjectMember>>(result);
                    if(employees.Count > 0) 
                    {
                        Members.AddRange(employees);
                        foreach (var employee in employees) 
                        {
                            ProjectMembers.Add(employee);
                            //var res = App.client.GetStringAsync(App.localUrl + $"api/Account/Getmailbyid?id={employee.UserId}");
                        }
                    }
                }
                
                
            }
            
        }
        public ObservableRangeCollection<ProjectMember> Members { get; set; }
        public AsyncCommand invite { get; set; }
        public static List<ProjectMember> ProjectMembers { get; set; }
        public ObservableRangeCollection<Problem> Problems { get; set; }
        public AsyncCommand GetProblems { get; set; }
        public AsyncCommand<Problem> SelectedProblem { get; set; }
        async Task getProblems() 
        {
            
            using (App.client = new System.Net.Http.HttpClient()) 
            {
                if (Problems.Count > 0) { Problems.Clear();}
                
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var res = await App.client.GetStringAsync(App.localUrl + $"GetProblemsbyid?id={Id}");
                var problems = JsonConvert.DeserializeObject<List<Problem>>(res);
                if (problems.Count > 0) 
                {
                    Problems.AddRange(problems);
                }
            }
        
        }
        async Task selectedProblem(Problem problem) 
        {
            await Shell.Current.GoToAsync(nameof(DetailTaskPage)+$"?Name={problem.Name}" +
                $"&Id={Id}&ProblemId={problem.Id}&ProblemDescription={problem.Description}&" +
                $"ProblemCreationtime={problem.Creationtime}" +
                $"&ProblemDeadline={problem.Deadline}");
        }

    }
}
