using Lab2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Lab2
{
    public class TodoList
    {
        private List<TodoItem> _items = new List<TodoItem>();

        public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();

        public void Add(TodoItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _items.Count)
                _items.RemoveAt(index);
        }

        public void Save(string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_items, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new IOException($"Ошибка сохранения: {ex.Message}");
            }
        }

        public void Load(string filePath)
        {
            if (!File.Exists(filePath)) return;

            try
            {
                string json = File.ReadAllText(filePath);
                var loaded = JsonSerializer.Deserialize<List<TodoItem>>(json);
                if (loaded != null) _items = loaded;
            }
            catch
            {
                _items = new List<TodoItem>();
            }
        }
    }
}
