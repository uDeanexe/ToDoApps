using Microsoft.Maui.Controls;
using ToDoApps.Models;
using ToDoApps.ViewModels;

namespace ToDoApps.Views
{
    public partial class ToDoListPage : ContentPage
    {
        public ToDoListPage()
        {
            InitializeComponent();
            if (BindingContext is ToDoViewModel vm)
            {
                vm.RemoveTaskCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand<ToDoItem>(OnRemoveTaskAsync);
            }
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is ToDoItem item)
            {
                if (BindingContext is ToDoViewModel vm)
                {
                    vm.SelectedItem = new ToDoItem
                    {
                        Id = item.Id,
                        Judul = item.Judul,
                        Deskripsi = item.Deskripsi,
                        Tanggal = item.Tanggal,
                        Selesai = item.Selesai,
                        Prioritas = item.Prioritas
                    };
                }
                await Shell.Current.GoToAsync("//ToDoDetailPage");
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private async System.Threading.Tasks.Task OnRemoveTaskAsync(ToDoItem? item)
        {
            if (item != null)
            {
                bool confirm = await DisplayAlert("Konfirmasi", $"Hapus tugas '{item.Judul}'?", "Ya", "Batal");
                if (confirm && BindingContext is ToDoViewModel vm)
                {
                    vm.RemoveTaskCommand.Execute(item);
                }
            }
        }
    }
}