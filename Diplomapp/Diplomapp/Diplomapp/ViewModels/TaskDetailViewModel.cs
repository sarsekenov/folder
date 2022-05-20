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
            Checklists = new ObservableRangeCollection<ProblemChecklist>();
            Comments = new ObservableRangeCollection<ProblemComment>();
            Members = new List<ProjectMember>();
            #region Commands
            command1 = new AsyncCommand(putChanges);
            command = new AsyncCommand(saveChanges);
            GetComments = new AsyncCommand(getcomments);
            SendComment = new AsyncCommand(sendcomment);
            GetChecklist = new AsyncCommand(getchecklist);
            SendChecklist = new AsyncCommand(sendChecklist);
            GetMember = new AsyncCommand(getMember);
            Initial = new AsyncCommand(init);
            addCheck = new AsyncCommand(AddCheck);
            Selectedcheck = new AsyncCommand<ProblemChecklist>(CheckChecklist);
            #endregion
            SelectedMember = new ProjectMember();
            if (ProjectPageViewModel.ProjectMembers.Count >0)
            {
                foreach (var member in ProjectPageViewModel.ProjectMembers) 
                { 
                   Members.Add(member);
                }
            }
            //Selectedempl = new AsyncCommand<ProjectMember>(selectedempl);
            

        }


        public AsyncCommand addCheck { get; set; }
        async Task AddCheck()
        {
            Checklists.Add(new ProblemChecklist() { IsChecked = false, ProblemId = ProblemId, ProblemName= Title});
            await sendChecklist();
            Title = string.Empty;
        }
        public static AsyncCommand<ProblemChecklist> Selectedcheck { get; set; }
        async Task CheckChecklist(ProblemChecklist checklist) 
        {
            if (checklist.IsChecked) { checklist.IsChecked = false; }
            else { checklist.IsChecked = true;}

            using (App.client = new HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                var json = JsonConvert.SerializeObject(checklist);
                var con = new StringContent(json);
                con.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var res = await App.client.PutAsync(App.localUrl + $"api/ProblemChecklists?id={checklist.Id}", con);
                if (res.IsSuccessStatusCode) 
                {
                    //Checklists.Remove(checklist);
                    var newres= await res.Content.ReadAsStringAsync();
                    var newitem = JsonConvert.DeserializeObject<ProblemChecklist>(newres);
                    Checklists.Insert(Checklists.IndexOf(checklist), newitem);
                    Checklists.Remove(checklist);
                }
            }
            
        }
        public AsyncCommand Initial { get; set; }
        public async Task init()
        {
            await GetChecklist.ExecuteAsync();
            await GetComments.ExecuteAsync();
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
                await sendChecklist();
            }
        }
        ProjectMember tasksusername;
        public ProjectMember TasksUserName { get=>tasksusername; set=>SetProperty(ref tasksusername,value); }
        public async Task getMember()
        {
            using (App.client = new HttpClient()) 
            {
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                var json = await App.client.GetAsync(App.localUrl + $"api/ProlemMembers?id={ProblemId}");
                if (json.IsSuccessStatusCode) 
                {
                    var result = await json.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<ProblemMember>(result);

                    foreach (var member in Members) 
                    {
                        if (res.UserId == member.UserID) 
                        {
                            TasksUserName = member;
                        }
                    } 
                }
                
                
            }
        }
        public AsyncCommand command1 { get; set; }

        public AsyncCommand GetMember { get; set; }
        public ProjectMember SelectedMember { get; set; }
        
        public AsyncCommand command { get; set; }
        public Problem Problem { get; set; }
        public ProblemMember ProblemMember { get; set; }
        public ProblemChecklist ProblemChecklist{ get; set; }
        public ProblemComment ProblemComment{ get; set; }
        public ObservableRangeCollection<ProblemChecklist> Checklists { get; set; }
        public ObservableRangeCollection<ProblemComment> Comments{ get; set; }

        public AsyncCommand GetComments { get; set; }
        public async Task getcomments() 
        {
            using (App.client = new HttpClient())
            {
                Comments.Clear();
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                var json = await App.client.GetAsync(App.localUrl + $"GetProblemComments?id={ProblemId}");
                if (json.IsSuccessStatusCode) 
                {
                    var result = await json.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<List<ProblemComment>>(result);
                    if (res.Count != 0)
                    {
                        Comments.AddRange(res);
                    }
                }
                
            }
        }
        string comment;
        public string Comment { get => comment; set => SetProperty(ref comment, value); }
        public AsyncCommand SendComment { get; set; }
        public async Task sendcomment() 
        {
            using (App.client = new HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                var comment = new ProblemComment() {ProblemId = ProblemId, Author = App.email,Text = Comment};
                var json = JsonConvert.SerializeObject(comment);
                var con = new StringContent(json);
                
                con.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var req = await App.client.PostAsync(App.localUrl + $"api/ProblemComments",con);

                var dson = await req.Content.ReadAsStringAsync();
                var res2 = JsonConvert.DeserializeObject<ProblemComment>(dson);
                Comment = string.Empty;               
                if (req.IsSuccessStatusCode)
                {
                    Comments.Add(res2);
                }
            }

        }
        string check;
        public string Check { get => check; set => SetProperty(ref check, value); }
        public AsyncCommand GetChecklist { get; set; }
        public async Task getchecklist()
        {
            using (App.client = new HttpClient())
            {
                Checklists.Clear();
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                var json = await App.client.GetAsync(App.localUrl + $"GetProblemChecklists?id={ProblemId}");
                if (json.IsSuccessStatusCode)
                {
                    var result = await json.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<List<ProblemChecklist>>(result);
                    if (res.Count != 0)
                    {
                        Checklists.AddRange(res);
                    }
                }
                
            }
        }
        public AsyncCommand SendChecklist { get; set; }
        public async Task sendChecklist() 
        {
            using (App.client = new HttpClient())
            {
                
                App.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.accessToken);
                //var checklist = new ProblemChecklist() { ProblemId = ProblemId, ProblemName = Check, IsChecked = false };
                foreach (var check in Checklists)
                {
                    var json = JsonConvert.SerializeObject(check);
                    var con = new StringContent(json);

                    con.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var req = await App.client.PostAsync(App.localUrl + $"api/ProblemChecklists", con);

                    var dson = await req.Content.ReadAsStringAsync();
                    var res2 = JsonConvert.DeserializeObject<ProblemChecklist>(dson);
                    //Check = string.Empty;
                    if (req.IsSuccessStatusCode)
                    {
                        //Checklists.Add(res2);
                    }
                }
            }
        }

    }
}
