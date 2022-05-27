using Diplomapp.Models;
using Diplomapp.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Diplomapp.ViewModels
{
    class TasksViewModel : BaseViewModel
    {
        public TasksViewModel()
        {
            GetProblems = new AsyncCommand(getProblems);
            Problems = new List<Problem>();
            Empty = true;
            GroupedProblems = new ObservableRangeCollection<Grouping<string, Problem>>();
            Projects = new List<Project>();
            ProjectMembers = new List<ProjectMember>();
            Init = new AsyncCommand(init);
            SelectedProblem = new AsyncCommand<Problem>(selectedProblem);
        }
        bool empty;
        public AsyncCommand Init { get; set; }
        public bool Empty { get => empty; set => SetProperty(ref empty,value); }
        public AsyncCommand GetProblems { get; set; }

        public AsyncCommand<Problem> SelectedProblem { get; set; }
        async Task selectedProblem(Problem problem)
        {
            await Shell.Current.GoToAsync(nameof(DetailTaskPage) + $"?Name={problem.Name}" +
                $"&Id={problem.ProjectId}&ProblemId={problem.Id}&ProblemDescription={problem.Description}&" +
                $"ProblemCreationtime={problem.Creationtime}" +
                $"&ProblemDeadline={problem.Deadline}");
        }
        async Task init() 
        {
            await getProjectid();
            await GetProblems.ExecuteAsync();
        }
        async Task getProjectid()
        {
            using (App.client = new System.Net.Http.HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var res = await App.client.GetStringAsync(App.localUrl + $"api/ProjectMembers");
                var members = JsonConvert.DeserializeObject<List<ProjectMember>>(res);
                if (members.Count > 0)
                {
                    Projects.Clear();
                    ProjectMembers.Clear();
                    ProjectMembers.AddRange(members);
                    foreach (var mem in members) 
                    {
                        var json = await App.client.GetStringAsync(App.localUrl + $"api/Projects?id={mem.ProjectID}");
                        var project = JsonConvert.DeserializeObject<Project>(json);
                        Projects.Add(project);
                    }
                }
            }
        }
        public List<Project> Projects { get; set; }
        public List<ProjectMember> ProjectMembers { get; set; }
        async Task getProblems()
        {
            Problems.Clear();
            GroupedProblems.Clear();

            using (App.client = new System.Net.Http.HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var res = await App.client.GetStringAsync(App.localUrl + $"api/ProblemMembers");
                var members = JsonConvert.DeserializeObject<List<ProblemMember>>(res);
                if (members.Count > 0)
                {
                    Empty = false;
                    foreach (var member in members) 
                    {
                        var json = await App.client.GetStringAsync(App.localUrl + $"api/Problems?id={member.ProblemId}");
                        var problem = JsonConvert.DeserializeObject<Problem>(json);
                        Problems.Add(problem);
                    }
                    foreach (var mem in ProjectMembers) 
                    {
                        var name = Projects.Where(c=>c.Id == mem.ProjectID);
                        GroupedProblems.Add(new Grouping<string,Problem>(name.FirstOrDefault().Name,Problems.Where(c => c.ProjectId == mem.ProjectID)));
                    }
                }
                else { Empty = true; }
            }

        }
        List<Problem> Problems { get; set; }
        public ObservableRangeCollection<Grouping<string,Problem>> GroupedProblems { get; set; }

    }
}
