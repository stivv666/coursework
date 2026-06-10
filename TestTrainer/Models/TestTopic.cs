
namespace TestTrainer.Models
{
    public class TestTopic
    {
        public string TopicName { get; set; }
        public List<OpenQuestion> OpenQuestions { get; set; }
        public List<SingleChoiceQuestion> SingleChoiceQuestions { get; set; }

        public TestTopic()
        {
            OpenQuestions = new List<OpenQuestion>();
            SingleChoiceQuestions = new List<SingleChoiceQuestion>();
        }
    }
}