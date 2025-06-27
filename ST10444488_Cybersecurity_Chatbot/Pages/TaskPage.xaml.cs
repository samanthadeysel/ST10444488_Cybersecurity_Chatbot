using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ST10444488_Cybersecurity_Chatbot.Pages
{
    /// <summary>
    /// Interaction logic for TaskPage.xaml
    /// </summary>
    public partial class TaskPage : Window
    {
        private List<string> tasks;
        private List<string> log;
        public TaskPage(List<string> taskList, List<string> activityLog)
        {
            InitializeComponent();
            tasks = taskList;
            log = activityLog;
            Refresh();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string task = $"{TaskTitle.Text} - {TaskDesc.Text} {(ReminderDate.SelectedDate != null ? $"(Remind: {ReminderDate.SelectedDate.Value.ToShortDateString()})" : "")}";
            tasks.Add(task);
            log.Add($"Task added: {task}");
            Refresh();
        }

        private void Refresh()
        {
            TaskList.Items.Clear();
            foreach (string t in tasks)
                TaskList.Items.Add(t);
        }

        private void ClearText(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box != null && (box.Text == "Task Title" || box.Text == "Task Description"))
            {
                box.Text = "";
                box.Foreground = Brushes.Black;
            }
        }

        private void RestoreText(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box != null && string.IsNullOrWhiteSpace(box.Text))
            {
                if (box.Name == "TaskTitle") box.Text = "Task Title";
                if (box.Name == "TaskDesc") box.Text = "Task Description";
                box.Foreground = Brushes.Gray;
            }
        }
    }
}
