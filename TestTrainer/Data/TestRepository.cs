using System;
using System.Text.Json;
using TestTrainer.Models;

namespace TestTrainer.Data
{
    public class TestRepository
    {
        private string _dataDirectory;

        public TestRepository(string dataDirectory)
        {
            _dataDirectory = dataDirectory;

            if (Directory.Exists(_dataDirectory) == false)
            {
                Directory.CreateDirectory(_dataDirectory);
            }
        }

        public void SaveTopic(TestTopic topic)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;

                string jsonString = JsonSerializer.Serialize(topic, options);
                string filePath = _dataDirectory + "/" + topic.TopicName + ".json";
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving topic: " + ex.Message);
            }
        }

        public TestTopic LoadTopic(string topicName)
        {
            try
            {
                string filePath = _dataDirectory + "/" + topicName + ".json";
                if (File.Exists(filePath) == false)
                {
                    TestTopic newTopic = new TestTopic();
                    newTopic.TopicName = topicName;
                    return newTopic;
                }

                string jsonString = File.ReadAllText(filePath);
                TestTopic topic = JsonSerializer.Deserialize<TestTopic>(jsonString);

                if (topic != null)
                {
                    return topic;
                }
                else
                {
                    return new TestTopic();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading topic: " + ex.Message);
                return new TestTopic();
            }
        }
        public AppConfig LoadConfig()
        {
            try
            {
                string filePath = _dataDirectory + "/config.json";
                if (File.Exists(filePath) == false)
                {
                    AppConfig defaultConfig = new AppConfig();
                    JsonSerializerOptions options = new JsonSerializerOptions();
                    options.WriteIndented = true;
                    File.WriteAllText(filePath, JsonSerializer.Serialize(defaultConfig, options));
                    return defaultConfig;
                }

                string jsonString = File.ReadAllText(filePath);
                AppConfig config = JsonSerializer.Deserialize<AppConfig>(jsonString);

                if (config != null)
                {
                    return config;
                }
                else
                {
                    return new AppConfig();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading config: " + ex.Message);
                return new AppConfig();
            }
        }
        public void AppendHistory(TestResult result)
        {
            try
            {
                string filePath = _dataDirectory + "/history.txt";
                string line = result.DateTaken + " | Topic: " + result.TopicName + " | Score: " + result.Score + "/" + result.TotalQuestions + "\n";
                File.AppendAllText(filePath, line);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving history: " + ex.Message);
            }
        }
    }
}