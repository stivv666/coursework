using TestTrainer.Models;
using System;
using System.Text.Json;


namespace TestTrainer.Data
{
    public class TestRepository
    {
        private string _filePath;

        public TestRepository(string filePath)
        {
            _filePath = filePath;
        }

        public void SaveQuestions(List<OpenQuestion> questions)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;

                string jsonString = JsonSerializer.Serialize(questions, options);
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving file: " + ex.Message);
            }
        }

        public List<OpenQuestion> LoadQuestions()
        {
            try
            {
                if (File.Exists(_filePath) == false)
                {
                    return new List<OpenQuestion>();
                }

                string jsonString = File.ReadAllText(_filePath);
                List<OpenQuestion> questions = JsonSerializer.Deserialize<List<OpenQuestion>>(jsonString);

                if (questions != null)
                {
                    return questions;
                }
                else
                {
                    return new List<OpenQuestion>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
                return new List<OpenQuestion>();
            }
        }
    }
}