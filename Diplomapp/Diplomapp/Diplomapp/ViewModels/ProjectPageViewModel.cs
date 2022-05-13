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

namespace Diplomapp.ViewModels
{

[QueryProperty(nameof(Name), "name")]// Project Name
[QueryProperty(nameof(Id), "Id")] // Project Id
[QueryProperty(nameof(OwnerId), "OwnerId")]
[QueryProperty(nameof(Description), "Description")]
    public class ProjectPageViewModel:BaseViewModel
    {
        string name;
        int id;
        public string Name { get=>name; set=>SetProperty(ref name,value); }
        public int Id { get=>id; set=>SetProperty(ref id,value); }
        string description;
        string ownerId;
        public string OwnerId { get=> ownerId; set=>SetProperty(ref ownerId,value); }
        public string Description { get=>description; set=>SetProperty(ref description,value); }
        
        //Project project;
        //public Project Project { get=>project; set=> SetProperty(ref project,value); }
        public ProjectPageViewModel() 
        {
            initialize = new AsyncCommand(Init);
            GetProblems = new AsyncCommand(getProblems);
            CreateTask = new AsyncCommand(createTask);
            invite = new AsyncCommand(inviteuser);
            getempl = new AsyncCommand(GetEmployees);
            Members = new ObservableRangeCollection<ProjectMember>();
            Members.Clear();
            Problems = new ObservableRangeCollection<Problem>();
            SelectedProblem = new AsyncCommand<Problem>(selectedProblem);
        }
        public async Task Init() 
        { 
            await getempl.ExecuteAsync();

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
        public AsyncCommand CreateTask { get; set; }
        public async Task GetEmployees() // Получаем всех работников этого проекта 
        {
            Members.Clear();
            ProjectMembers.Clear();
            using (App.client = new System.Net.Http.HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var json = await App.client.GetStringAsync(App.localUrl + $"GetbyId?Id={Id}");// обращаемся к контроллеру 
                var employees = JsonConvert.DeserializeObject<List<ProjectMember>>(json);
                if(employees.Count > 0) 
                {
                    Members.AddRange(employees);
                    foreach (var employee in employees) 
                    {
                        ProjectMembers.Add(employee);
                    }
                }
                
            }
            
        }
        public ObservableRangeCollection<ProjectMember> Members { get; set; }
        public AsyncCommand invite { get; set; }
        public static List<ProjectMember> ProjectMembers = new List<ProjectMember>();
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
