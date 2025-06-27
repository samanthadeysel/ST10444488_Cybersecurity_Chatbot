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
                    new[] { "Reply", "Delete", "Report it", "Ignore" }, 2, "Report phishing emails."),

                ("True or False: Reusing passwords is safe.",
                    new[] { "True", "False" }, 1, "Unique passwords protect your accounts."),

                ("Which is a strong password?",
                    new[] { "123456", "P@ssw0rd!", "password1" }, 1, "Strong passwords include symbols and casing."),

                ("What does VPN stand for?",
                    new[] { "Very Protected Net", "Virtual Private Network", "Verified Person Number" }, 1, "VPN = secure encrypted connection."),

                ("True or False: Public Wi-Fi is secure for banking.",
                    new[] { "True", "False" }, 1, "Avoid sensitive tasks on public Wi-Fi."),

                ("Why are software updates important?",
                    new[] { "They fix bugs", "They slow systems", "They patch security holes" }, 2, "Updates fix known vulnerabilities."),

                ("Two-factor authentication includes…?",
                    new[] { "Two passwords", "A code + password", "Double username" }, 1, "It adds an extra verification step."),

                ("Which is a phishing attempt?",
                    new[] { "HR email asking login", "Friend sends meme", "App update" }, 0, "Phishing pretends to be a trusted source."),

                ("True or False: Antivirus is 100% protection.",
                    new[] { "True", "False" }, 1, "No tool is foolproof without smart habits."),

                ("Safe link handling means…",
                    new[] { "Check sender & URL", "Click fast", "Forward to others" }, 0, "Inspect all links before clicking.")
            };

            LoadQuestion();
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
                QuestionText.Text = $"🎉 Quiz finished! You scored {score} out of {questions.Count}.";
                FeedbackText.Text = score >= 7
                    ? "🌟 Excellent! You're a cybersecurity star!"
                    : "💡 Keep practicing — your awareness is growing!";
                ScoreText.Text = $"Score: {score}/{questions.Count}";
                OptionList.Visibility = Visibility.Collapsed;
                CloseButton.Visibility = Visibility.Visible;
                log.Add($"Quiz completed: {score}/{questions.Count} correct.");
            }
        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (OptionList.SelectedIndex == -1)
            {
                FeedbackText.Text = "⚠️ Please select an answer before continuing.";
                return;
            }

            var q = questions[current];
            bool isCorrect = OptionList.SelectedIndex == q.CorrectIndex;
            if (isCorrect) score++;

            FeedbackText.Text = isCorrect ? "✅ Correct! " : "❌ Incorrect. ";
            FeedbackText.Text += q.Explanation;

            current++;
            LoadQuestion();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
