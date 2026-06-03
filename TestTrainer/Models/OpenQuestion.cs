namespace TestTrainer.Models
{
    public class OpenQuestion : BaseQuestion
    {
        public string CorrectAnswer { get; set; }

        public override bool CheckAnswer(string userAnswer)
        {
            if (string.IsNullOrEmpty(userAnswer) || string.IsNullOrEmpty(CorrectAnswer))
                return false;

            return userAnswer.Trim().ToLower() == CorrectAnswer.Trim().ToLower();
        }
    }
}