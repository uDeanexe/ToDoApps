using Microsoft.Maui.Controls;
using ToDoApps.ViewModels;

namespace ToDoApps.Views
{
    public partial class ToDoDetailPage : ContentPage
    {
        public ToDoDetailPage()
        {
            InitializeComponent();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..", true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ToDoViewModel vm)
            {
                vm.PropertyChanged += Vm_PropertyChanged;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is ToDoViewModel vm)
            {
                vm.PropertyChanged -= Vm_PropertyChanged;
            }
        }

        private void Vm_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ToDoViewModel.SelectedItem))
            {
                Shell.Current.GoToAsync("..", true);
            }
        }
    }
}