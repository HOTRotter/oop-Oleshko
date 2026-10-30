using Lab2;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Lab2
{
    public partial class MainForm : Form
    {
        private readonly TodoList _todoList = new TodoList();
        private const string FilePath = "tasks.json";

        public MainForm()
        {
            InitializeComponent();

            btnAdd.Click += BtnAdd_Click;
            btnComplete.Click += BtnComplete_Click;
            btnRename.Click += BtnRename_Click;
            btnDelete.Click += BtnDelete_Click;
            FormClosing += MainForm_FormClosing;

            _todoList.Load(FilePath);
            RefreshList();
        }

        private void RefreshList()
        {
            listBoxTasks.Items.Clear();
            foreach (var item in _todoList.Items)
                listBoxTasks.Items.Add(item);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var item = new TodoItem(txtTitle.Text, txtPriority.Text);
                _todoList.Add(item);
                RefreshList();
                txtTitle.Clear();
                txtPriority.Text = "Обычный";
                txtTitle.Focus();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnComplete_Click(object sender, EventArgs e)
        {
            if (listBoxTasks.SelectedIndex < 0)
            {
                MessageBox.Show("Сначала выберите задачу в списке.",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _todoList.Items[listBoxTasks.SelectedIndex].Complete();
            RefreshList();
        }

        private void BtnRename_Click(object sender, EventArgs e)
        {
            if (listBoxTasks.SelectedIndex < 0)
            {
                MessageBox.Show("Сначала выберите задачу в списке.",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                _todoList.Items[listBoxTasks.SelectedIndex].Rename(txtTitle.Text);
                RefreshList();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxTasks.SelectedIndex < 0)
            {
                MessageBox.Show("Сначала выберите задачу в списке.",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _todoList.RemoveAt(listBoxTasks.SelectedIndex);
            RefreshList();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _todoList.Save(FilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
