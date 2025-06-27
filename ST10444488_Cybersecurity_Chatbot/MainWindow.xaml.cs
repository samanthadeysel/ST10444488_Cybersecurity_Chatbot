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
        private readonly Dictionary<string, string> responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"help", "You can ask about: 'Passwords', '2 Factor Authentication', 'Updates', 'Phishing', 'Virtual Private Networks', 'Clicking'."},
            {"password", "Use strong passwords (longer than 8 characters) and never reuse old ones."},
            {"2 factor authentication", "2FA adds a second layer of login security — like a code, fingerprint, or app prompt."},
            {"updates", "Install updates regularly. They patch security flaws hackers could exploit."},
            {"phishing", "Don’t click suspicious links or open attachments from unknown senders."},
            {"virtual private networks", "VPNs protect your data on public Wi-Fi by encrypting your traffic."},
            {"clicking", "Be cautious with unknown ads or links. Hover before you click."},
            {"worried", "It's perfectly okay to feel that way. Let's tackle one topic at a time."},
            {"frustrated", "Cybersecurity can be frustrating, but small steps make a big impact."},
            {"how are you", "I'm here and ready to help keep you cyber-safe!"}
        };


        public MainWindow()
        {
            InitializeComponent();
            AppendMessage("Chatbot: Hello! I'm your Cybersecurity Assistant.");
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            AppendMessage($"You: {input}");

            string tip = DetectCyberTip(input);
            if (tip != null)
            {
                AppendMessage("Chatbot: " + tip);
                UserInput.Clear();
                return;
            }

            string intent = DetectUserIntent(input);
            switch (intent)
            {
                case "add_task":
                    string cleanedTask = char.ToUpper(input[0]) + input.Substring(1);
                    knownTasks.Add(cleanedTask);
                    activityLog.Add($"Auto-created task: \"{cleanedTask}\"");
                    new TaskPage(knownTasks, activityLog).Show();
                    break;

                case "start_quiz":
                    activityLog.Add("Cybersecurity quiz started via chat.");
                    new QuizPage(activityLog).Show();
                    break;

                case "open_log":
                    activityLog.Add("Opened activity log from chat.");
                    new ActivityLogPage(activityLog, knownTasks).Show();
                    break;

                default:
                    AppendMessage("Chatbot: Ask me about reminders, quizzes, or activity logs — or try topics like passwords, phishing, or VPNs.");
                    break;
            }

            UserInput.Clear();
        }

        private string DetectCyberTip(string input)
        {
            input = input.ToLower();
            foreach (var keyword in responses.Keys)
            {
                if (input.Contains(keyword.ToLower()))
                    return responses[keyword];
            }
            return null;
        }

        private string DetectUserIntent(string input)
        {
            input = input.ToLower();

            var keywordMap = new Dictionary<string[], string>
            {
                { new[] { "remind", "create a task", "add task", "set a reminder" }, "add_task" },
                { new[] { "quiz", "take quiz", "start test", "cybersecurity questions" }, "start_quiz" },
                { new[] { "history", "activity", "log", "chat history" }, "open_log" }
            };

            foreach (var keywords in keywordMap.Keys)
            {
                foreach (var keyword in keywords)
                {
                    if (input.Contains(keyword))
                        return keywordMap[keywords];
                }
            }

            return "unknown";
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
