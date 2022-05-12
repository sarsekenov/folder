using Microcharts;
using MvvmHelpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Diplomapp.ViewModels
{
    public class StatisticsPageViewModel : BaseViewModel
    {
        public StatisticsPageViewModel() 
        {
            command = new Command(getStatistics);
            ChartEntries = new List<ChartEntry>();
            command.Execute(null);
            BarChart = new LineChart {Entries = ChartEntries };
            
        }
        public List<ChartEntry> ChartEntries { get; set; }
        public LineChart BarChart { get; set; } 
        public Command command { get; set; }
        public void getStatistics() 
        {
            ChartEntry[] entry = new[]
            {
                new  ChartEntry(100)
                {
                    Label = "Task1",
                    ValueLabel = " 100"
                },
                new  ChartEntry(85)
                {
                    Label = "Task2",
                    ValueLabel = " 85"
                },
                new  ChartEntry(70)
                {
                    Label = "Task3",
                    ValueLabel = " 70"
                }
            };
             ChartEntries.AddRange(entry);
        }
    }
}
