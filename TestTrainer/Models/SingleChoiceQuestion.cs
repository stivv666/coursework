
namespace TestTrainer.Models
{
    public class SingleChoiceQuestion : BaseQuestion
    {
        public List<string> Options { get; set; }
        public int CorrectOptionIndex { get; set; }

        public SingleChoiceQuestion()
        {
            Options = new List<string>();
        }

        public override bool CheckAnswer(string userAnswer)
        {
            int chosenIndex = -1;
            bool isNumber = int.TryParse(userAnswer, out chosenIndex);

            if (isNumber == true)
            {
                return (chosenIndex - 1) == CorrectOptionIndex;
            }
            return false;
        }
    }
}