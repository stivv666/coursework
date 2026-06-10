namespace TestTrainer.Models
{
    public class AppConfig
    {
        public int MaxQuestionsPerSession { get; set; }
        public bool ShowCorrectAnswerImmediately { get; set; }

        public AppConfig()
        {
            MaxQuestionsPerSession = 10;
            ShowCorrectAnswerImmediately = true;
        }
    }
}