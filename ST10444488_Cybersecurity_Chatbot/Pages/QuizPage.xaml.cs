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
    /// Interaction logic for QuizPage.xaml
    /// </summary>
    public partial class QuizPage : Window
    {
        private readonly List<(string Question, string[] Options, int CorrectIndex, string Explanation)> questions;
        private int current = 0, score = 0;
        private readonly List<string> log;

        public QuizPage(List<string> activityLog)
        {
            InitializeComponent();
            log = activityLog;

            questions = new List<(string, string[], int, string)>
            {
                ("What should you do if you receive an email asking for your password?",
                    new[] { "Reply", "Delete", "Report it", "Ignore" }, 2, "You should report phishing emails to your IT department or email provider."),

                ("True or False: Reusing passwords is safe.",
                    new[] { "True", "False" }, 1, "Using unique passwords keeps your accounts more secure."),

                ("Which is a strong password?",
                    new[] { "123456", "P@ssw0rd!", "password1" }, 1, "Strong passwords use a mix of letters, numbers, and symbols."),

                ("What does VPN stand for?",
                    new[] { "Very Protected Net", "Virtual Private Network", "Verified Person Number" }, 1, "A VPN encrypts your internet connection for privacy and security."),

                ("True or False: Public Wi-Fi is secure for banking.",
                    new[] { "True", "False" }, 1, "Avoid doing sensitive tasks like banking over public Wi-Fi."),

                ("Why are software updates important?",
                    new[] { "They fix bugs", "They slow systems", "They patch security holes" }, 2, "Updates often fix vulnerabilities hackers could exploit."),

                ("Two-factor authentication includes…?",
                    new[] { "Two passwords", "A code + password", "Double username" }, 1, "It adds an extra verification step beyond your password."),

                ("Which is a phishing attempt?",
                    new[] { "HR email asking login", "Friend sends meme", "App update" }, 0, "Phishing pretends to be a trusted source to get info from you."),

                ("True or False: Antivirus is 100% protection.",
                    new[] { "True", "False" }, 1, "Good habits are just as important as tools."),

                ("Safe link handling means…",
                    new[] { "Check sender & URL", "Click fast", "Forward to others" }, 0, "Always inspect links before clicking — they could be fake.")
            };
        }

        

        private void LoadQuestion()
        {
            if (current < questions.Count)
            {
                var q = questions[current];
                QuestionText.Text = $"Question {current + 1} of {questions.Count}:\n{q.Question}";
                OptionList.Items.Clear();
                foreach (var option in q.Options)
                    OptionList.Items.Add(option);

                FeedbackText.Text = "";
                ScoreText.Text = $"Score: {score}/{questions.Count}";
                OptionList.Visibility = Visibility.Visible;
                CloseButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                QuestionText.Text = $"Quiz Complete! You scored {score} out of {questions.Count}.";
                FeedbackText.Text = score >= 7
                    ? "Excellent! You're a cybersecurity star!"
                    : "Keep practicing — your awareness is growing!";
                ScoreText.Text = $"Score: {score}/{questions.Count}";
                OptionList.Visibility = Visibility.Collapsed;
                SubmitButton.Visibility = Visibility.Collapsed;
                CloseButton.Visibility = Visibility.Visible;

                log.Add($"Quiz completed: {score}/{questions.Count} correct.");
            }
        }
        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            current = 0;
            score = 0;
            WelcomePanel.Visibility = Visibility.Collapsed;
            QuizContent.Visibility = Visibility.Visible;
            SubmitButton.Visibility = Visibility.Visible;
            LoadQuestion();
        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (OptionList.SelectedIndex == -1)
            {
                FeedbackText.Text = "Please select an answer before continuing.";
                return;
            }

            var q = questions[current];
            bool isCorrect = OptionList.SelectedIndex == q.CorrectIndex;

            if (isCorrect) score++;
            FeedbackText.Text = isCorrect ? "Correct! " : "Incorrect. ";
            FeedbackText.Text += q.Explanation;
            
            OptionList.IsEnabled = false;
            SubmitButton.IsEnabled = false;

            NextButton.Visibility = Visibility.Visible;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            current++;
            OptionList.IsEnabled = true;
            SubmitButton.IsEnabled = true;
            NextButton.Visibility = Visibility.Collapsed;

            LoadQuestion();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
