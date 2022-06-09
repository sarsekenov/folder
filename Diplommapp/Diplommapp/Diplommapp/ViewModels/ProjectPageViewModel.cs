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
using System.IO;
using Diplommapp.Models;
using Microcharts;
using SkiaSharp;

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
            Salaries = new ObservableRangeCollection<SalaryWithTaxes>();
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
            ProjectFiles = new ObservableRangeCollection<ProjectFile>();
            Getfilelist = new AsyncCommand(getfilelist);
            Selectfiletodownload = new AsyncCommand<ProjectFile>(selectedfiletodownload);
            selectedemployee = new AsyncCommand<ProjectMember>(selectedempl);
            changeSalary = new AsyncCommand(changesalary);
            changeOborudovanie = new AsyncCommand(changeoborudovanie);
            ChangeSal = true;
            ChangeObor = true;
            putSal = new AsyncCommand<SalaryWithTaxes>(putsal);
            putObor = new AsyncCommand<oborudovanie>(putobor);
            Owner = true;
            //donut = new DonutChart();
            //point = new RadialGaugeChart();
            donutentries = new List<ChartEntry>();
            pointentries = new List<ChartEntry>();
            donutCommand = new AsyncCommand(DonCom);
            pointCommand = new AsyncCommand(PoiCom);
            charts_in = new AsyncCommand(Charts_in);
        }
        #region Statistic
        public AsyncCommand charts_in { get; set; }
        async Task  Charts_in() 
        {
            await donutCommand.ExecuteAsync();
            await pointCommand.ExecuteAsync();
        }
        DonutChart chart;

        public DonutChart donut { get=>chart; set=>SetProperty(ref chart,value); }
        RadialGaugeChart chart1;
        public RadialGaugeChart point { get=>chart1; set=>SetProperty(ref chart1,value); }
        public List<ChartEntry> donutentries { get; set; }
        public AsyncCommand donutCommand { get; set; }
        public AsyncCommand pointCommand { get; set; }
        async Task DonCom()
        {
            donutentries.Clear();
            var m = Salaries[Salaries.Count - 1];
            donutentries.Add(new ChartEntry(m.total) { Label = " Траты на зарплату", ValueLabel = Convert.ToString(m.total), Color= SKColor.Parse("#d7fc03"), ValueLabelColor = SKColor.Parse("#d7fc03") });
            donutentries.Add(new ChartEntry(m.tax1) { Label = "налог 1", ValueLabel = Convert.ToString(m.tax1), Color = SKColor.Parse("#56fc03"), ValueLabelColor = SKColor.Parse("#56fc03") });
            donutentries.Add(new ChartEntry(m.tax2) { Label = "налог 2", ValueLabel = Convert.ToString(m.tax2), Color = SKColor.Parse("#03fc8c") ,ValueLabelColor = SKColor.Parse("#03fc8c") });
            donutentries.Add(new ChartEntry(m.tax3) { Label = " налог 3", ValueLabel = Convert.ToString(m.tax3), Color = SKColor.Parse("#037bfc") ,ValueLabelColor= SKColor.Parse("#037bfc") });
            donutentries.Add(new ChartEntry(m.tax4) { Label = " налог 4", ValueLabel = Convert.ToString(m.tax4), Color = SKColor.Parse("#b603fc") , ValueLabelColor = SKColor.Parse("#b603fc") });
            var o = oborudovanies[oborudovanies.Count - 1];
            donutentries.Add(new ChartEntry(o.Price) { Label = "Траты на оборудование", ValueLabel = Convert.ToString(o.Price), Color = SKColor.Parse("#e3fc03"), ValueLabelColor = SKColor.Parse("#e3fc03") });
            donut = new DonutChart() { Entries = donutentries };
        }
        async Task PoiCom()
        {
            pointentries.Clear();
            pointentries.Add(new ChartEntry(Members.Count) { Label = "Работники", ValueLabel = Convert.ToString(Members.Count) , Color = SKColor.Parse("#fcb603") , ValueLabelColor = SKColor.Parse("#fcb603") });
            pointentries.Add(new ChartEntry(oborudovanies.Count) { Label = "Оборудование", ValueLabel = Convert.ToString(oborudovanies.Count), Color = SKColor.Parse("#fc6203") , ValueLabelColor = SKColor.Parse("#fc6203") });
            pointentries.Add(new ChartEntry(Problems.Count) { Label = "Задачи", ValueLabel = Convert.ToString(Problems.Count), Color = SKColor.Parse("#075cba"), ValueLabelColor = SKColor.Parse("#075cba") });
            point = new RadialGaugeChart() { Entries = pointentries };
        }
        public List<ChartEntry> pointentries { get; set; }
        #endregion
        void inproject()
        {
            if (App.userId == OwnerId)
            { Owner = true; }
            else { Owner = false; }
        }
        bool owner;
        
        public bool Owner { get => owner; set => SetProperty(ref owner, value); }
        
        public AsyncCommand<SalaryWithTaxes> putSal { get; set; }
        async Task putsal(SalaryWithTaxes salary)
        {
            using (App.client = new HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                foreach (var m in Members) 
                {
                    if (m.UserName == salary.UserId){ salary.UserId = m.UserID; }
                }
                var sal = new Salary() { Id = salary.Id, ProjectId = salary.ProjectId, stavka = salary.stavka, UserId = salary.UserId, zp = salary.zp };
                var json = JsonConvert.SerializeObject(sal);
                var con = new StringContent(json);
                con.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var res = await App.client.PutAsync(App.localUrl + $"api/Salaries?id={salary.Id}",con);
                if (res.IsSuccessStatusCode)
                {

                }
                await GetSalaries.ExecuteAsync(); 
            }
        }
        public AsyncCommand<oborudovanie> putObor { get; set; }
        async Task putobor(oborudovanie oborudovanie) 
        {
            App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
            var json = JsonConvert.SerializeObject(oborudovanie);
            var con = new StringContent(json);
            con.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var res = await App.client.PutAsync(App.localUrl + $"api/oborudovanies?id={oborudovanie.Id}", con);
            if (res.IsSuccessStatusCode)
            {

            }
            await Getoborudovanies.ExecuteAsync(); 
        }
        async Task selectedempl(ProjectMember member)
        {
            await Shell.Current.GoToAsync(nameof(CreateUinfo) + $"?Id={member.UserID}");
        }
        public AsyncCommand<ProjectMember> selectedemployee { get; set; }
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
        public ObservableRangeCollection<SalaryWithTaxes> Salaries { get; set; }
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
                    var salarieswithtaxes = new List<SalaryWithTaxes>();
                    foreach (var sal in salaries)
                    {
                        foreach (var mem in Members)
                        {
                            if (mem.UserID == sal.UserId)
                            {
                                sal.UserId = mem.UserName;
                            }
                        }
                        salarieswithtaxes.Add(new SalaryWithTaxes() { Id = sal.Id, ProjectId = sal.ProjectId, stavka = sal.stavka,
                            UserId = sal.UserId, zp = sal.zp, tax1 = (float)(0.06 * sal.zp * sal.stavka), tax2 = (float)(0.035 * sal.zp * sal.stavka),
                            tax3 = (float)(0.02 * sal.zp * sal.stavka), tax4 = (float)(0.05 * sal.zp * sal.stavka), saltotal = sal.stavka * sal.zp
                        });
                        salarieswithtaxes[salarieswithtaxes.Count - 1].total = salarieswithtaxes[salarieswithtaxes.Count - 1].zp - (salarieswithtaxes[salarieswithtaxes.Count - 1].tax1
                            + salarieswithtaxes[salarieswithtaxes.Count - 1].tax2 + salarieswithtaxes[salarieswithtaxes.Count - 1].tax3 + salarieswithtaxes[salarieswithtaxes.Count - 1].tax4);
                    }
                    if (salarieswithtaxes.Count > 0)
                    {
                        Salaries.Clear();
                        Salaries.AddRange(salarieswithtaxes);
                    }
                    SalaryWithTaxes taxes = new SalaryWithTaxes();
                    taxes.UserId = "Итого";
                    foreach (var sala in Salaries)
                    {

                        taxes.saltotal += sala.zp * sala.stavka;
                        taxes.total += sala.total;
                        taxes.tax1 += sala.tax1;
                        taxes.tax2 += sala.tax2;
                        taxes.tax3 += sala.tax3;
                        taxes.tax4 += sala.tax4;
                        taxes.tax1 += sala.tax1;

                    }
                    Salaries.Add(taxes);
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
                    var total = 0.0;
                    foreach (var sal in salaries)
                    {
                        total += sal.Price * sal.Count;
                    }

                    oborudovanies.Add(new oborudovanie() { Name = "Итого", Price = (float)total });
                }
            }
        }

        public async Task Init()
        {
            inproject();
            await GetInfo.ExecuteAsync();
            await getempl.ExecuteAsync();
            await GetSalaries.ExecuteAsync();
            await Getoborudovanies.ExecuteAsync();
            await Getfilelist.ExecuteAsync();
        }
        public AsyncCommand changeSalary { get; set; }
        async Task changesalary() 
        { 
            if (ChangeSal == false)
            { ChangeSal = true; }
            else 
            { ChangeSal = false; }
            //if(Salbtn == false)
            //{ Salbtn = true; }
            //else 
            //{ Salbtn = false; }
        }
        public AsyncCommand changeOborudovanie { get; set; }
        async Task changeoborudovanie() 
        { 
            if (ChangeObor == false) 
            { ChangeObor = true; } 
            else 
            { ChangeObor = false; }
            //if(Obbtn == false)
            //{ Obbtn = true; } 
            //else
            //{ Obbtn = false; }
        }

        bool changeSal;
        public bool ChangeSal { get => changeSal; set => SetProperty(ref changeSal, value); }
        bool changeObor;
        public bool ChangeObor { get => changeObor; set => SetProperty(ref changeObor, value); }

        public AsyncCommand initialize { get; set; }
        public AsyncCommand getempl { get; set; }
        public async Task inviteuser()
        {
            await Shell.Current.GoToAsync(nameof(CreateInvitePage) + $"?name={Name}&Id={Id}");//передаем значения в форму 
        }
        public async Task createTask()
        {
            await Shell.Current.GoToAsync($"{nameof(TaskDetailPage)}?Id={Id}");
        }
        public AsyncCommand Getfilelist { get; set; }
        async Task getfilelist() 
        {
            using (App.client = new HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                ProjectFiles.Clear();
                var res2 = await App.client.GetAsync(App.localUrl + $"GetProjectFilesbyid?id={Id}");
                if (res2.IsSuccessStatusCode)
                {
                    var json = await res2.Content.ReadAsStringAsync();
                    var files = JsonConvert.DeserializeObject<List<ProjectFile>>(json);
                    if (files.Count > 0)
                    {
                        ProjectFiles.AddRange(files);
                    }
                }
            }
        }
        public AsyncCommand Pick { get; set; }
        public async Task pickFile() 
        {
            var file = await Xamarin.Essentials.FilePicker.PickAsync();
            if (file == null)
                return;
            using (App.client = new HttpClient()) 
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var stream = await file.OpenReadAsync();
                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, bytes.Length);
                var cont = new MultipartFormDataContent(); 
                var con = new ByteArrayContent(bytes);
                con.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    Name = file.FileName,
                    FileName = Name,
                    Size = Id
                };
                cont.Add(con);
                var res = await App.client.PostAsync(App.localUrl + "SendProjectFiles",cont);
                if (res.IsSuccessStatusCode)
                {
                    ProjectFiles.Clear();
                    var res2 = await App.client.GetAsync(App.localUrl + $"GetProjectFilesbyid?id={Id}");
                    if (res2.IsSuccessStatusCode) 
                    {
                        var json = await res2.Content.ReadAsStringAsync();
                        var files = JsonConvert.DeserializeObject<List<ProjectFile>>(json);
                        if (files.Count>0) 
                        {
                            ProjectFiles.AddRange(files);
                        }
                    }
                }
            }
        }

        public AsyncCommand<ProjectFile> Selectfiletodownload { get; set; }
        async Task selectedfiletodownload(ProjectFile file) 
        {
            using (App.client = new HttpClient())
            {
                App.client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.accessToken);
                var res = await App.client.GetAsync(App.localUrl + $"GetprFiles?path={file.Path + "/" + file.Name }");
                if (res.IsSuccessStatusCode)
                {
                    var bfile = await res.Content.ReadAsByteArrayAsync();
                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
                    string localFilename = file.Name;
                    File.WriteAllBytes(documentsPath + "/" + localFilename, bfile);
                }
            }
        }
        public ObservableRangeCollection<ProjectFile> ProjectFiles { get; set; }
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
                     var salarieswithtaxes = new SalaryWithTaxes()
                    {
                        Id = sal.Id,
                        ProjectId = sal.ProjectId,
                        stavka = sal.stavka,
                        UserId = salar.UserId,
                        zp = sal.zp,
                        tax1 = (float)(0.06 * sal.zp * sal.stavka),
                        tax2 = (float)(0.035 * sal.zp * sal.stavka),
                        tax3 = (float)(0.02 * sal.zp * sal.stavka),
                        tax4 = (float)(0.05 * sal.zp * sal.stavka),
                        
                    };
                    salarieswithtaxes.total = salarieswithtaxes.zp - (salarieswithtaxes.tax1
                        + salarieswithtaxes.tax2 + salarieswithtaxes.tax3 + salarieswithtaxes.tax4);
                    salarieswithtaxes.saltotal = sal.zp * sal.stavka;

                    Salaries.RemoveAt(Salaries.Count - 1);
                    Salaries.Add(salarieswithtaxes);
                    var taxes = new SalaryWithTaxes();
                    foreach (var sas in Salaries) 
                    {
                        taxes.saltotal += sas.zp * sas.stavka;
                        taxes.total += sas.total;
                        taxes.tax1 += sas.tax1;
                        taxes.tax2 += sas.tax2;
                        taxes.tax3 += sas.tax3;
                        taxes.tax4 += sas.tax4;
                        taxes.tax1 += sas.tax1;

                    }

                    Salaries.Add(taxes);
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
                    oborudovanies.RemoveAt(oborudovanies.Count - 1);
                    float total = 0;
                    oborudovanies.Add(salar);
                    foreach (var ob in oborudovanies)
                    {
                        total += ob.Count * ob.Price;
                    }
                    oborudovanies.Add(new oborudovanie() { Price = total , Name = "Итого" });
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
                $"&ProblemDeadline={problem.Deadline}" + $"&ProjectName={Name}");
        }

    }
}
