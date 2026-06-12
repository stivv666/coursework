namespace TestTrainer.Models
{
    public class OpenQuestion : BaseQuestion
    {
        public string CorrectAnswer { get; set; }

        public override bool CheckAnswer(string userAnswer)
        {
            if (string.IsNullOrWhiteSpace(userAnswer)) return false;
            return userAnswer.Trim().Equals(CorrectAnswer.Trim(), System.StringComparison.OrdinalIgnoreCase);
        }
    }
}