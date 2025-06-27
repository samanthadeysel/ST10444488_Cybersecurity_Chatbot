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
        private int currentPage = 0;
        private int pageSize = 10;

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
            var pagedItems = activityLog
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .ToList();

            foreach (var entry in pagedItems)
            {
                var textBlock = new TextBlock
                {
                    Text = entry,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(5)
                };
                if (entry.Contains("✅"))
                {
                    textBlock.TextDecorations = TextDecorations.Strikethrough;
                    textBlock.Foreground = Brushes.Gray;
                }
                LogList.Items.Add(textBlock);
            }
            ShowMoreButton.Visibility = (activityLog.Count > (currentPage + 1) * pageSize)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void ShowMore_Click(object sender, RoutedEventArgs e)
        {
            currentPage++;
            RefreshLog();
        }

        private void MarkComplete_Click(object sender, RoutedEventArgs e)
        {
            if (LogList.SelectedItem is TextBlock selected && selected.Text.ToLower().Contains("task"))
            {
                string taskTitle = ExtractTaskTitle(selected.Text);

                int taskIndex = tasks.FindIndex(t =>
                    t.Equals(taskTitle, StringComparison.OrdinalIgnoreCase) &&
                    !t.StartsWith("✅"));
                if (taskIndex >= 0)
                {
                    tasks[taskIndex] = "✅ " + taskTitle;

                    int logIndex = activityLog.FindIndex(l =>
                        l.Contains(taskTitle) && !l.Contains("✅"));
                    if (logIndex >= 0)
                    {
                        activityLog[logIndex] = "✅ " + activityLog[logIndex];
                    }
                    RefreshLog();
                }
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (LogList.SelectedItem is TextBlock selected && selected.Text.ToLower().Contains("task"))
            {
                string taskTitle = ExtractTaskTitle(selected.Text);

                int taskIndex = tasks.FindIndex(t =>
                    t.Equals(taskTitle, StringComparison.OrdinalIgnoreCase) ||
                    t.Contains(taskTitle));
                if (taskIndex >= 0)
                {
                    tasks.RemoveAt(taskIndex);
                }
                int logIndex = activityLog.FindIndex(l => l == selected.Text || l.Contains(taskTitle));
                if (logIndex >= 0)
                {
                    activityLog.RemoveAt(logIndex);
                }
                RefreshLog();
            }
        }

        private string ExtractTaskTitle(string logEntry)
        {
            int firstQuote = logEntry.IndexOf('"');
            int lastQuote = logEntry.LastIndexOf('"');

            if (firstQuote >= 0 && lastQuote > firstQuote)
            {
                return logEntry.Substring(firstQuote + 1, lastQuote - firstQuote - 1).Trim();
            }
            int colon = logEntry.IndexOf(':');
            return colon >= 0 ? logEntry.Substring(colon + 1).Trim() : logEntry.Trim();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
