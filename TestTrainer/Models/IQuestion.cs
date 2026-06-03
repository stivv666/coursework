namespace ExamSimulator.Models
{
    public interface IQuestion
    {
        bool CheckAnswer(string userAnswer);
    }
}