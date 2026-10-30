using System;
using System.Text.Json.Serialization;

namespace Lab2
{
    public class TodoItem
    {

        public string Title { get; private set; }
        public bool IsCompleted { get; private set; }
        public string Priority { get; private set; }

        [JsonConstructor]
        public TodoItem(string title, bool isCompleted, string priority)
        {
            Title = title;
            IsCompleted = isCompleted;
            Priority = priority;
            Validate();
        }

        public TodoItem(string title, string priority = "Обычный")
        {
            Title = title;
            IsCompleted = false;
            Priority = string.IsNullOrWhiteSpace(priority) ? "Обычный" : priority;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw new ArgumentException("Название задачи не может быть пустым!");
        }

        public void Complete()
        {
            IsCompleted = true;
        }

        public void Rename(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Название задачи не может быть пустым!");
            Title = newTitle.Trim();
        }

        public override string ToString()
        {
            string status = IsCompleted ? "[✓]" : "[ ]";
            return $"{status} {Title}  (Приоритет: {Priority})";
        }
    }
}
