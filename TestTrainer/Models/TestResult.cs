using System;
namespace TestTrainer.Models
{
    public class TestResult
    {
        public string TopicName { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public string DateTaken { get; set; }

        public TestResult()
        {
            DateTaken = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
    }
}