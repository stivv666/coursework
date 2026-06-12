using TestTrainer.Models;

namespace TestTrainer.Data
{
    public interface ITestRepository
    {
        TestTopic LoadTopic(string topicName);
        void SaveTopic(TestTopic topic);

        void AppendHistory(TestResult result);

        AppConfig LoadConfig();
    }
}