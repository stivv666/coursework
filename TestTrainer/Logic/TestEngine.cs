using System;
using System.Collections.Generic;
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

            for (int i = 0; i < questionsToAsk; i++)
            {
                BaseQuestion currentQ = sessionQuestions[i];
                Console.WriteLine("\nQuestion " + (i + 1) + ": " + currentQ.Text);

                if (currentQ is SingleChoiceQuestion)
                {
                    SingleChoiceQuestion scq = (SingleChoiceQuestion)currentQ;
                    for (int j = 0; j < scq.Options.Count; j++)
                    {
                        Console.WriteLine((j + 1) + ". " + scq.Options[j]);
                    }
                }

                Console.Write("Your answer: ");
                string answer = Console.ReadLine();

                if (currentQ.CheckAnswer(answer) == true)
                {
                    Console.WriteLine("Correct!");
                    score++;
                }
                else
                {
                    Console.WriteLine("Wrong!");

                    if (_config.ShowCorrectAnswerImmediately == true)
                    {
                        if (currentQ is OpenQuestion)
                        {
                            Console.WriteLine("Correct answer was: " + ((OpenQuestion)currentQ).CorrectAnswer);
                        }
                        else if (currentQ is SingleChoiceQuestion)
                        {
                            SingleChoiceQuestion scq = (SingleChoiceQuestion)currentQ;
                            Console.WriteLine("Correct option was: " + (scq.CorrectOptionIndex + 1));
                        }
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
        }
    }
}