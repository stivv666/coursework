using System;
using TestTrainer.Models;
using TestTrainer.Data;

namespace TestTrainer.Logic
{
    public delegate void TestFinishedHandler(int score, int total);

    public class TestEngine
    {
        private TestRepository _repository;
        private AppConfig _config;

        public event TestFinishedHandler OnTestFinished;

        public TestEngine(string dataDirectory)
        {
            _repository = new TestRepository(dataDirectory);
            _config = _repository.LoadConfig();
        }

        public void RunSession(string topicName)
        {
            TestTopic topic = _repository.LoadTopic(topicName);
            List<BaseQuestion> sessionQuestions = new List<BaseQuestion>();

            for (int i = 0; i < topic.OpenQuestions.Count; i++)
            {
                sessionQuestions.Add(topic.OpenQuestions[i]);
            }

            for (int i = 0; i < topic.SingleChoiceQuestions.Count; i++)
            {
                sessionQuestions.Add(topic.SingleChoiceQuestions[i]);
            }

            int totalAvailable = sessionQuestions.Count;
            if (totalAvailable == 0)
            {
                Console.WriteLine("No questions found for this topic.");
                return;
            }

            Random rng = new Random();

            int n = totalAvailable;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                BaseQuestion value = sessionQuestions[k];
                sessionQuestions[k] = sessionQuestions[n];
                sessionQuestions[n] = value;
            }

            int questionsToAsk = totalAvailable;
            if (_config.MaxQuestionsPerSession < totalAvailable)
            {
                questionsToAsk = _config.MaxQuestionsPerSession;
            }

            int score = 0;
            List<string> errorReport = new List<string>();

            for (int i = 0; i < questionsToAsk; i++)
            {
                BaseQuestion currentQ = sessionQuestions[i];
                Console.WriteLine("\nQuestion " + (i + 1) + ": " + currentQ.Text);

                if (currentQ is SingleChoiceQuestion)
                {
                    SingleChoiceQuestion scq = (SingleChoiceQuestion)currentQ;

                    string correctText = scq.Options[scq.CorrectOptionIndex];
                    int optCount = scq.Options.Count;
                    while (optCount > 1)
                    {
                        optCount--;
                        int k = rng.Next(optCount + 1);
                        string tmp = scq.Options[k];
                        scq.Options[k] = scq.Options[optCount];
                        scq.Options[optCount] = tmp;
                    }

                    for (int j = 0; j < scq.Options.Count; j++)
                    {
                        if (scq.Options[j] == correctText)
                        {
                            scq.CorrectOptionIndex = j;
                            break;
                        }
                    }

                    for (int j = 0; j < scq.Options.Count; j++)
                    {
                        Console.WriteLine((j + 1) + ". " + scq.Options[j]);
                    }
                }

                Console.Write("Your answer: ");
                string answer = Console.ReadLine();

                if (currentQ.CheckAnswer(answer) == true)
                {
                    Console.WriteLine("Accepted.");
                    score++;
                }
                else
                {
                    Console.WriteLine("Accepted.");
                    if (currentQ is OpenQuestion)
                    {
                        errorReport.Add("Question: " + currentQ.Text + " | Your answer was wrong. Correct: " + ((OpenQuestion)currentQ).CorrectAnswer);
                    }
                    else if (currentQ is SingleChoiceQuestion)
                    {
                        SingleChoiceQuestion scq = (SingleChoiceQuestion)currentQ;
                        errorReport.Add("Question: " + currentQ.Text + " | Your answer was wrong. Correct option: " + (scq.CorrectOptionIndex + 1));
                    }
                }
            }

            TestResult result = new TestResult();
            result.TopicName = topic.TopicName;
            result.Score = score;
            result.TotalQuestions = questionsToAsk;
            _repository.AppendHistory(result);

            if (OnTestFinished != null)
            {
                OnTestFinished(score, questionsToAsk);
            }

            if (errorReport.Count > 0)
            {
                Console.WriteLine("\nREVIEW YOUR MISTAKES");
                for (int i = 0; i < errorReport.Count; i++)
                {
                    Console.WriteLine(errorReport[i]);
                }
            }
            else
            {
                Console.WriteLine("\nPerfect! No mistakes made!");
            }
        }
    }
}