namespace TestTrainer.Models
{
    public interface IQuestion
    {
        bool CheckAnswer(string userAnswer);
    }
}