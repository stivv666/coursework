namespace TestTrainer.Models
{
    public abstract class BaseQuestion
    {
        public string Text { get; set; }
        public QuestionDifficulty Difficulty { get; set; }

        public abstract bool CheckAnswer(string userAnswer);
    }
}