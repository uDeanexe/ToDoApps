using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDoApps.Models;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;

namespace ToDoApps.ViewModels
{
    public partial class ToDoViewModel : ObservableObject
    {
        public ObservableCollection<ToDoItem> ToDoList { get; set; } = new();
        public ObservableCollection<ToDoItem> FilteredToDoList { get; set; } = new();
        public string? NewJudul { get; set; }
        public string? NewDeskripsi { get; set; }
        public DateTime NewTanggal { get; set; } = DateTime.Now;
        public string? NewPrioritas { get; set; }
        public string? NewKategori { get; set; }
        public string? SearchText { get; set; }
        public string? FilterPrioritas { get; set; }
        public string? FilterKategori { get; set; }
        public DateTime? FilterTanggal { get; set; }
        public ToDoItem? SelectedItem { get; set; }
        public ObservableCollection<string> PrioritasList { get; } = new() { "Rendah", "Sedang", "Tinggi" };
        public ObservableCollection<string> KategoriList { get; } = new() { "Umum", "Pekerjaan", "Pribadi", "Belanja", "Lainnya" };
        const string ToDoListKey = "ToDoListData";

        public IAsyncRelayCommand<ToDoItem?> RemoveTaskCommand { get; set; }

        public ToDoViewModel()
        {
            LoadData();
            FilteredToDoList = new ObservableCollection<ToDoItem>(ToDoList);
            RemoveTaskCommand = new AsyncRelayCommand<ToDoItem?>(RemoveTaskAsync);
            ToDoList.CollectionChanged += (s, e) => SaveData();
        }

        [RelayCommand]
        public void AddTask()
        {
            if (!string.IsNullOrWhiteSpace(NewJudul))
            {
                var item = new ToDoItem
                {
                    Id = Guid.NewGuid(),
                    Judul = NewJudul!,
                    Deskripsi = NewDeskripsi ?? string.Empty,
                    Tanggal = NewTanggal,
                    Selesai = false,
                    Prioritas = NewPrioritas ?? string.Empty,
                    Kategori = NewKategori ?? string.Empty
                };
                ToDoList.Add(item);
                FilteredToDoList.Add(item);
                NewJudul = string.Empty;
                NewDeskripsi = string.Empty;
                NewTanggal = DateTime.Now;
                NewPrioritas = string.Empty;
                NewKategori = string.Empty;
                SaveData();
            }
        }

        private async System.Threading.Tasks.Task RemoveTaskAsync(ToDoItem? item)
        {
            if (item != null && ToDoList.Contains(item))
            {
                ToDoList.Remove(item);
                FilteredToDoList.Remove(item);
                SaveData();
            }
            await System.Threading.Tasks.Task.CompletedTask;
        }

        [RelayCommand]
        public void ToggleSelesai(ToDoItem item)
        {
            item.Selesai = !item.Selesai;
            SaveData();
        }

        [RelayCommand]
        public void FilterTasks()
        {
            var query = ToDoList.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(SearchText))
                query = query.Where(x => x.Judul.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || (x.Deskripsi != null && x.Deskripsi.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            if (!string.IsNullOrWhiteSpace(FilterPrioritas))
                query = query.Where(x => x.Prioritas == FilterPrioritas);
            if (!string.IsNullOrWhiteSpace(FilterKategori))
                query = query.Where(x => x.Kategori == FilterKategori);
            if (FilterTanggal.HasValue)
                query = query.Where(x => x.Tanggal.Date == FilterTanggal.Value.Date);
            FilteredToDoList.Clear();
            foreach (var item in query)
                FilteredToDoList.Add(item);
        }

        [RelayCommand]
        public void SaveEdit()
        {
            if (SelectedItem != null)
            {
                var item = ToDoList.FirstOrDefault(x => x.Id == SelectedItem.Id);
                if (item != null)
                {
                    item.Judul = SelectedItem.Judul;
                    item.Deskripsi = SelectedItem.Deskripsi;
                    item.Tanggal = SelectedItem.Tanggal;
                    item.Prioritas = SelectedItem.Prioritas;
                    item.Kategori = SelectedItem.Kategori;
                    item.Selesai = SelectedItem.Selesai;
                    SaveData();
                }
            }
        }

        private void SaveData()
        {
            var json = JsonConvert.SerializeObject(ToDoList);
            Preferences.Set(ToDoListKey, json);
        }

        private void LoadData()
        {
            var json = Preferences.Get(ToDoListKey, null);
            if (!string.IsNullOrEmpty(json))
            {
                var list = JsonConvert.DeserializeObject<ObservableCollection<ToDoItem>>(json);
                if (list != null)
                {
                    ToDoList = list;
                }
            }
        }
    }
}