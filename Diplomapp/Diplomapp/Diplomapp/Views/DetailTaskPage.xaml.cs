using Diplomapp.Models;
using Diplomapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diplomapp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailTaskPage : ContentPage
	{
		public DetailTaskPage ()
		{
			InitializeComponent ();
		}

        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
			var item = ((CheckBox)sender).Parent.BindingContext as ProblemChecklist;
			if (item == null)
				return;
			await TaskDetailViewModel.Selectedcheck.ExecuteAsync(item);
		}
    }
}