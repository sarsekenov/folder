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
using Xamarin.Forms;


namespace Diplomapp.ViewModels
{
    [QueryProperty(nameof(Name), "Name")] //Problem Name
    [QueryProperty(nameof(Id), "Id")] //ProjectID
    [QueryProperty(nameof(ProblemId),"ProblemId")] //
    [QueryProperty(nameof(Description),"ProblemDescription")] //
    [QueryProperty(nameof(Creationtime),"ProblemCreationtime")] //
    [QueryProperty(nameof(Deadline),"ProblemDeadline")] //
    

    public class TaskDetailViewModel : BaseViewModel
    {
        string name;
        int problemid;
        public int ProblemId { get => problemid; set => SetProperty(ref problemid, value); }
        public string Name { get => name; set => SetProperty(ref name, value); }
        int id;
        public int Id { get => id; set => SetProperty(ref id, value); }
        string description,creationtime,deadline;
        public string Description { get=>description; set=>SetProperty(ref description,value); }
        public string Creationtime { get=>creationtime; set=>SetProperty(ref creationtime,value); }
        public string Deadline { get=>deadline; set=>SetProperty(ref deadline,value); }

        public TaskDetailViewModel()
        {
            Problem = new Problem();
            ProblemMember = new ProblemMember();
            command1 = new AsyncCommand(putChanges);
            command = new AsyncCommand(saveChanges);
            Checklists = new ObservableRangeCollection<ProblemChecklist>();
            Comments = new ObservableRangeCollection<ProblemComment>();
            Members = new List<ProjectMember>();
            SelectedMember = new ProjectMember();
            GetMember = new AsyncCommand(getMember);
            if (ProjectPageViewModel.ProjectMembers.Count >0)
            {
                foreach (var member in ProjectPageViewModel.ProjectMembers) 
                { 
                   Members.Add(member);
                }
            }
            //Selectedempl = new AsyncCommand<ProjectMember>(selectedempl);
            

        }
        public List<ProjectMember> Members { get; set; }
        public async Task putChanges() 
        {
            using (App.client = new HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                Problem.Creationtime = DateTime.Now;
                Problem.ProjectId = Id;
                Problem.Description = Description;
                Problem.Name = Name;
                Problem.Id = ProblemId;
                var json = JsonConvert.SerializeObject(Problem);
                var con = new StringContent(json);
                con.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var res = await App.client.PutAsync(App.localUrl + $"api/Problems?id={ProblemId}", con);

                var result = await res.Content.ReadAsStringAsync();
                var dson = JsonConvert.DeserializeObject<Problem>(result);
                ProblemMember.UserId = TasksUserName.UserID;
                ProblemMember.ProblemId = dson.Id;
                var json2 = JsonConvert.SerializeObject(ProblemMember);
                
                var con2 = new StringContent(json2);
                con2.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var res2 = await App.client.PutAsync(App.localUrl + $"api/ProblemMembers", con2);

            }
        }
        public async Task saveChanges()
        {
            using (App.client = new HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                Problem.Creationtime = DateTime.Now;
                Problem.ProjectId = Id;
                var json  = JsonConvert.SerializeObject(Problem);
                var con = new StringContent(json);
                con.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var res = await App.client.PostAsync(App.localUrl + "api/Problems", con);

                var result = await res.Content.ReadAsStringAsync();
                var dson = JsonConvert.DeserializeObject<Problem>(result);
                ProblemMember.UserId = SelectedMember.UserID;
                ProblemMember.ProblemId = dson.Id;
                
                var json2 = JsonConvert.SerializeObject(ProblemMember);
                var con2 = new StringContent(json2);
                con2.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var res2 = await App.client.PostAsync(App.localUrl + "api/ProblemMembers", con2);

            }
        }
        ProjectMember tasksusername;
        public ProjectMember TasksUserName { get=>tasksusername; set=>SetProperty(ref tasksusername,value); }
        public async Task getMember()
        {
            using (App.client = new HttpClient()) 
            {
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                var json = await App.client.GetStringAsync(App.localUrl + $"api/ProlemMembers?id={ProblemId}");
                var res = JsonConvert.DeserializeObject<ProblemMember>(json);
                foreach (var member in Members) 
                {
                    if (res.UserId == member.UserID) 
                    {
                        TasksUserName = member;
                    }
                }
                
            }
        }
        public AsyncCommand command1 { get; set; }

        public AsyncCommand GetMember { get; set; }
        public ProjectMember SelectedMember { get; set; }
        //string taskempname;
        //public string Taskempname { get => taskempname; set=>SetProperty(ref taskempname,value); }
        /*public async Task selectedempl(ProjectMember member) 
        { 
            if (member == null)
                return;

        }
        public AsyncCommand<ProjectMember> Selectedempl { get; set; }*/
        public AsyncCommand command { get; set; }
        public Problem Problem { get; set; }
        public ProblemMember ProblemMember { get; set; }
        public ProblemChecklist ProblemChecklist{ get; set; }
        public ProblemComment ProblemComment{ get; set; }
        public ObservableRangeCollection<ProblemChecklist> Checklists { get; set; }
        public ObservableRangeCollection<ProblemComment> Comments{ get; set; }
    }
}
