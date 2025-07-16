using Microsoft.Maui.Controls;

namespace ToDoApps.Views
{
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private async void OnLihatDaftarTugasClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ToDoListPage");
        }
    }
} 