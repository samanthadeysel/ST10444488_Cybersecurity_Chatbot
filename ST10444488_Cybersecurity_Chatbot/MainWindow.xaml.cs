using ST10444488_Cybersecurity_Chatbot.Pages;
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

namespace ST10444488_Cybersecurity_Chatbot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> activityLog = new List<string>();
        private List<string> knownTasks = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            AppendMessage("Chatbot: Hello! I'm your Cybersecurity Assistant.");
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInput.Text.ToLower();
            if (string.IsNullOrWhiteSpace(input)) return;

            AppendMessage($"You: {UserInput.Text}");

            if (input.Contains("task") || input.Contains("remind"))
            {
                new TaskPage(knownTasks, activityLog).Show();
            }
            else if (input.Contains("quiz") || input.Contains("questions"))
            {
                new QuizPage(activityLog).Show();
            }
            else if (input.Contains("log") || input.Contains("history"))
            {
                new ActivityLogPage(activityLog, knownTasks).Show();
            }
            else
            {
                AppendMessage("Chatbot: Ask me about passwords, phishing, VPNs, reminders, or quizzes");
            }

            UserInput.Clear();
        }

        private void AppendMessage(string msg)
        {
            ChatHistory.Items.Add(new TextBlock
            {
                Text = msg,
                TextWrapping = TextWrapping.Wrap,
                Foreground = Brushes.Black,
                Margin = new Thickness(5)
            });
        }

        private void OpenTasks(object sender, RoutedEventArgs e)
        {
            new TaskPage(knownTasks, activityLog).Show();
        }
        private void OpenQuiz(object sender, RoutedEventArgs e)
        {
            new QuizPage(activityLog).Show();
        }
        private void OpenActivity(object sender, RoutedEventArgs e)
        {
            new ActivityLogPage(activityLog, knownTasks).Show();
        }


    }
}
