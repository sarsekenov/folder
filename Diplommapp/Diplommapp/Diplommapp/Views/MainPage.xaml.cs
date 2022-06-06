using Diplomapp.Models;
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
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"/{nameof(AddCompany)}");
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = ((ListView)sender).SelectedItem as Project;
            if (item == null)
                return;
            Shell.Current.GoToAsync($"{nameof(ProjectPage)}?name={item.Name}&OwnerId={item.OwnerId}&Description={item.Description}&Id={item.Id}");

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var company = ((Button) sender).BindingContext as string;


        }
    }
}