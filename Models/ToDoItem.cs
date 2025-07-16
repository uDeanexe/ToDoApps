using System;

namespace ToDoApps.Models
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public string Judul { get; set; } = string.Empty;
        public string Deskripsi { get; set; } = string.Empty;
        public DateTime Tanggal { get; set; }
        public bool Selesai { get; set; }
        public string Prioritas { get; set; } = string.Empty;
        public string Kategori { get; set; } = string.Empty;
    }
} 