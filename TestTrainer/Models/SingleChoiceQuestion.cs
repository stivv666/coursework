using System.Collections.Generic;

namespace TestTrainer.Models
{
    public class SingleChoiceQuestion : BaseQuestion
    {
        public List<string> Options { get; set; }
        public int CorrectOptionIndex { get; set; }

        public override bool CheckAnswer(string userAnswer)
        {
            if (int.TryParse(userAnswer.Trim(), out int choice))
            {
                return (choice - 1) == CorrectOptionIndex;
            }
            return false;
        }
    }
}