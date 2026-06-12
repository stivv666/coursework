using System;
using System.Text.Json;
using TestTrainer.Exceptions;
using TestTrainer.Models;

namespace TestTrainer.Data
{
    public class TestRepository : ITestRepository
    {
        private readonly string _dataFolder;

        public TestRepository(string dataFolder)
        {
            _dataFolder = dataFolder;
        }

        public TestTopic LoadTopic(string topicName)
        {
            string filePath = Path.Combine(_dataFolder, $"{topicName}.json");

            try
            {
                if (!File.Exists(filePath))
                {
                    return new TestTopic { TopicName = topicName };
                }

                string jsonString = File.ReadAllText(filePath);
                var topic = JsonSerializer.Deserialize<TestTopic>(jsonString);

                if (topic == null || string.IsNullOrWhiteSpace(topic.TopicName))
                {
                    throw new InvalidTestFormatException($"Тема '{topicName}' завантажилася некоректно або не має назви.");
                }

                return topic;
            }
            catch (InvalidTestFormatException ex)
            {
                Console.WriteLine($"\n[Помилка Формату] {ex.Message}");
                return new TestTopic { TopicName = topicName };
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"\n[Помилка JSON] Файл теми '{topicName}' пошкоджений! Системне повідомлення: {ex.Message}");
                return new TestTopic { TopicName = topicName };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Критична Помилка] Збій при завантаженні: {ex.Message}");
                return new TestTopic { TopicName = topicName };
            }
        }

        public void SaveTopic(TestTopic topic)
        {
            try
            {
                if (!Directory.Exists(_dataFolder))
                {
                    Directory.CreateDirectory(_dataFolder);
                }

                string filePath = Path.Combine(_dataFolder, $"{topic.TopicName}.json");

                string jsonString = JsonSerializer.Serialize(topic, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(filePath, jsonString);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"\n[Помилка Доступу] Немає прав для збереження файлу! Деталі: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Критична Помилка] Не вдалося зберегти тему: {ex.Message}");
            }
        }


        public void AppendHistory(TestResult result)
        {
            try
            {
                string path = Path.Combine(_dataFolder, "history.txt");

                double percent = result.TotalQuestions > 0 ? (double)result.Score / result.TotalQuestions * 100 : 0;

                string historyLine = $"[{DateTime.Now:dd.MM.yyyy HH:mm}] Бали: {result.Score} з {result.TotalQuestions} ({percent:F1}%)";

                File.AppendAllText(path, historyLine + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Помилка] Не вдалося зберегти історію: {ex.Message}");
            }
        }

        public AppConfig LoadConfig()
        {
            try
            {
                string path = Path.Combine(_dataFolder, "config.json");
                if (File.Exists(path))
                {
                    string jsonString = File.ReadAllText(path);
                    var config = JsonSerializer.Deserialize<AppConfig>(jsonString);
                    return config ?? new AppConfig();
                }
            }
            catch (Exception)
            {

            }

            return new AppConfig();
        }
    }
}