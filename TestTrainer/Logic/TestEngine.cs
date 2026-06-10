using System;
using TestTrainer.Models;
using TestTrainer.Data;

namespace TestTrainer.Logic
{
    public delegate void TestFinishedHandler(int score, int total);

    public class TestEngine
    {
        private TestRepository _repository;
        private List<OpenQuestion> _questions;

        public event TestFinishedHandler OnTestFinished;

        public TestEngine(string filePath)
        {
            _repository = new TestRepository(filePath);
            _questions = new List<OpenQuestion>();
        }

        public void LoadAndShuffleQuestions()
        {
            _questions = _repository.LoadQuestions();

            Random rng = new Random();
            int n = _questions.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                OpenQuestion value = _questions[k];
                _questions[k] = _questions[n];
                _questions[n] = value;
            }
        }

        public void RunSession()
        {
            int score = 0;
            int total = _questions.Count;

            if (total == 0)
            {
                Console.WriteLine("No questions found in file.");
                return;
            }

            for (int i = 0; i < total; i++)
            {
                OpenQuestion currentQ = _questions[i];
                Console.WriteLine("Question " + (i + 1) + ": " + currentQ.Text);
                Console.Write("Your answer: ");
                string answer = Console.ReadLine();

                if (currentQ.CheckAnswer(answer) == true)
                {
                    Console.WriteLine("Correct!");
                    score++;
                }
                else
                {
                    Console.WriteLine("Wrong! Correct answer was: " + currentQ.CorrectAnswer);
                }
            }

            if (OnTestFinished != null)
            {
                OnTestFinished(score, total);
            }
        }
    }
}