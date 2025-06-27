using System;
using System.Collections.Generic;
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
    /// Interaction logic for ActivityLogPage.xaml
    /// </summary>
    public partial class ActivityLogPage : Window
    {
        private readonly List<string> activityLog;
        private readonly List<string> tasks;

        public ActivityLogPage(List<string> activityLog, List<string> tasks)
        {
            InitializeComponent();
            this.activityLog = activityLog;
            this.tasks = tasks;
            RefreshLog();
        }

        private void RefreshLog()
        {
            LogList.Items.Clear();

            var recentItems = activityLog.Skip(Math.Max(0, activityLog.Count - 10)).ToList();

            if (recentItems.Count == 0)
            {
                LogList.Items.Add("No recent activity.");
                return;
            }

            foreach (var entry in recentItems)
                LogList.Items.Add(entry);
        }

        private void MarkComplete_Click(object sender, RoutedEventArgs e)
        {
            if (LogList.SelectedItem is string selected && selected.StartsWith("Task added: "))
            {
                string taskTitle = ExtractTaskTitle(selected);
                int index = tasks.FindIndex(t => t.StartsWith(taskTitle) && !t.Contains("✅"));

                if (index >= 0)
                {
                    tasks[index] += "Completed";
                    activityLog.Add($"Marked complete: {taskTitle}");
                    MessageBox.Show($"Marked '{taskTitle}' as complete.");
                }
                else
                {
                    MessageBox.Show("That task is already marked complete or no longer exists.");
                }
            }
            RefreshLog();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (LogList.SelectedItem is string selected && selected.StartsWith("Task added: "))
            {
                string taskTitle = ExtractTaskTitle(selected);
                int index = tasks.FindIndex(t => t.StartsWith(taskTitle));

                if (index >= 0)
                {
                    tasks.RemoveAt(index);
                    activityLog.Add($"Deleted task: {taskTitle}");
                    MessageBox.Show($"🗑️ Deleted '{taskTitle}' from your tasks.");
                }
                else
                {
                    MessageBox.Show("Could not find the task to delete.");
                }
            }
            RefreshLog();
        }

        private string ExtractTaskTitle(string logEntry)
        {
            string withoutPrefix = logEntry.Replace("Task added: ", "");
            string titleOnly = withoutPrefix.Split('(')[0].Trim(); // in case of "(Reminder...)" or "(Completed)"
            return titleOnly;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
