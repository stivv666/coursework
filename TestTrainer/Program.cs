using System;
using System.IO;
using System.Collections.Generic;
using TestTrainer.Logic;
using TestTrainer.Models;
using TestTrainer.Data;

namespace TestTrainer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            string dataFolder = "TestData";
            TestEngine engine = new TestEngine(dataFolder);
            TestRepository repo = new TestRepository(dataFolder);

            engine.OnTestFinished += ShowFinalScore;

            bool isRunning = true;
            int selectedIndex = 0;

            string[] menuItems = new string[7];
            menuItems[0] = "Запустити тестування";
            menuItems[1] = "Додати нове відкрите питання до теми";
            menuItems[2] = "Видалити питання з теми";
            menuItems[3] = "Видалити тему повністю";
            menuItems[4] = "Переглянути історію";
            menuItems[5] = "Переглянути статистику";
            menuItems[6] = "Вихід";

            while (isRunning == true)
            {
                Console.Clear();
                Console.WriteLine("СИМУЛЯТОР ЕКЗАМЕНУ\n");

                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.WriteLine("> " + menuItems[i]);
                    Console.ResetColor();
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0)
                    {
                        selectedIndex = menuItems.Length - 1;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= menuItems.Length)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.Clear();

                    switch (selectedIndex)
                    {
                        case 0: 
                            if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);

                            string[] files = Directory.GetFiles(dataFolder, "*.json");
                            List<string> topics = new List<string>();

                            for (int i = 0; i < files.Length; i++)
                            {
                                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                                if (fileName != "config" && !string.IsNullOrWhiteSpace(fileName))
                                {
                                    topics.Add(fileName);
                                }
                            }

                            if (topics.Count == 0)
                            {
                                Console.WriteLine("Тем не знайдено. Будь ласка, додайте питання спочатку, щоб створити тему.");
                            }
                            else
                            {
                                Console.WriteLine("Доступні теми:");
                                for (int i = 0; i < topics.Count; i++)
                                {
                                    Console.WriteLine((i + 1) + ". " + topics[i]);
                                }

                                Console.Write("\nОберіть номер теми: ");
                                int topicChoice = -1;
                                if (int.TryParse(Console.ReadLine(), out topicChoice) && topicChoice > 0 && topicChoice <= topics.Count)
                                {
                                    string selectedTopic = topics[topicChoice - 1];
                                    engine.RunSession(selectedTopic);
                                }
                                else
                                {
                                    Console.WriteLine("Неправильний вибір.");
                                }
                            }

                            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення до меню...");
                            Console.ReadKey(true);
                            break;

                        case 1:
                            Console.Write("Введіть назву теми (нову або існуючу): ");
                            string addTopicName = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(addTopicName))
                            {
                                Console.WriteLine("Назва теми не може бути порожньою!");
                                Console.WriteLine("\nНатисніть будь-яку клавішу для повернення до меню...");
                                Console.ReadKey(true);
                                break;
                            }

                            TestTopic topicToEdit = repo.LoadTopic(addTopicName.Trim());

                            string qText = "";
                            while (string.IsNullOrWhiteSpace(qText))
                            {
                                Console.Write("Введіть текст питання: ");
                                qText = Console.ReadLine();
                                if (string.IsNullOrWhiteSpace(qText))
                                {
                                    Console.WriteLine("Помилка: Питання не може бути порожнім або містити лише пробіли!");
                                }
                            }

                            string qAnswer = "";
                            while (string.IsNullOrWhiteSpace(qAnswer))
                            {
                                Console.Write("Введіть правильну відповідь: ");
                                qAnswer = Console.ReadLine();
                                if (string.IsNullOrWhiteSpace(qAnswer))
                                {
                                    Console.WriteLine("Помилка: Відповідь не може бути порожньою або містити лише пробіли!");
                                }
                            }

                            OpenQuestion newQuestion = new OpenQuestion();
                            newQuestion.Text = qText.Trim();
                            newQuestion.CorrectAnswer = qAnswer.Trim();
                            newQuestion.Difficulty = QuestionDifficulty.Medium;

                            topicToEdit.OpenQuestions.Add(newQuestion);
                            repo.SaveTopic(topicToEdit);

                            Console.WriteLine("Питання успішно додано!");
                            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення до меню...");
                            Console.ReadKey(true);
                            break;

                        case 2:
                            Console.Write("Введіть назву теми: ");
                            string delTopicName = Console.ReadLine();
                            TestTopic topicToModify = repo.LoadTopic(delTopicName);

                            if (topicToModify.OpenQuestions.Count == 0)
                            {
                                Console.WriteLine("Відкритих питань у цій темі не знайдено.");
                            }
                            else
                            {
                                Console.WriteLine("\nСписок питань:");
                                for (int i = 0; i < topicToModify.OpenQuestions.Count; i++)
                                {
                                    Console.WriteLine((i + 1) + ". " + topicToModify.OpenQuestions[i].Text);
                                }

                                Console.Write("\nВведіть номер питання для видалення: ");
                                int delIndex = -1;
                                if (int.TryParse(Console.ReadLine(), out delIndex) && delIndex > 0 && delIndex <= topicToModify.OpenQuestions.Count)
                                {
                                    topicToModify.OpenQuestions.RemoveAt(delIndex - 1);
                                    repo.SaveTopic(topicToModify);
                                    Console.WriteLine("Питання успішно видалено!");
                                }
                                else
                                {
                                    Console.WriteLine("Неправильний індекс.");
                                }
                            }
                            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення до меню...");
                            Console.ReadKey(true);
                            break;

                        case 3:
                            Console.Write("Введіть назву теми для видалення: ");
                            string topicToDelete = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(topicToDelete))
                            {
                                string filePath = dataFolder + "/" + topicToDelete.Trim() + ".json";
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                    Console.WriteLine("Тему успішно видалено!");
                                }
                                else
                                {
                                    Console.WriteLine("Таку тему не знайдено.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Назва не може бути порожньою.");
                            }
                            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення до меню...");
                            Console.ReadKey(true);
                            break;

                        case 4:
                            string historyFile = dataFolder + "/history.txt";
                            if (File.Exists(historyFile))
                            {
                                Console.WriteLine("ІСТОРІЯ");
                                string historyData = File.ReadAllText(historyFile);
                                Console.WriteLine(historyData);
                            }
                            else
                            {
                                Console.WriteLine("Історія порожня.");
                            }
                            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення до меню...");
                            Console.ReadKey(true);
                            break;

                        case 5:
                            string statFile = dataFolder + "/history.txt";
                            if (File.Exists(statFile))
                            {
                                string[] lines = File.ReadAllLines(statFile);
                                int totalCorrect = 0;
                                int totalAsked = 0;

                                for (int i = 0; i < lines.Length; i++)
                                {
                                    string currentLine = lines[i];
                                    int scoreIdx = currentLine.IndexOf("Score: ");
                                    if (scoreIdx != -1)
                                    {
                                        string scorePart = currentLine.Substring(scoreIdx + 7).Trim();
                                        string[] slashSplit = scorePart.Split('/');
                                        if (slashSplit.Length == 2)
                                        {
                                            int correct = 0;
                                            int asked = 0;
                                            if (int.TryParse(slashSplit[0], out correct) && int.TryParse(slashSplit[1], out asked))
                                            {
                                                totalCorrect += correct;
                                                totalAsked += asked;
                                            }
                                        }
                                    }
                                }

                                Console.WriteLine("СТАТИСТИКА УСПІШНОСТІ");
                                Console.WriteLine("Всього проаналізовано тестових сесій: " + lines.Length);
                                Console.WriteLine("Всього дано правильних відповідей: " + totalCorrect);
                                Console.WriteLine("Всього пройдено питань: " + totalAsked);

                                if (totalAsked > 0)
                                {
                                    double percentage = ((double)totalCorrect / totalAsked) * 100;
                                    Console.WriteLine("Загальний відсоток успішності: " + Math.Round(percentage, 2) + "%");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Статистика недоступна. Пройдіть кілька тестів спочатку!");
                            }
                            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення до меню...");
                            Console.ReadKey(true);
                            break;

                        case 6:
                            isRunning = false;
                            Console.WriteLine("Бувай!");
                            break;
                    }
                }
            }
        }

        static void ShowFinalScore(int score, int total)
        {
            Console.WriteLine("\nТЕСТ ЗАВЕРШЕНО");
            Console.WriteLine("Ваш фінальний бал: " + score + " з " + total);
        }
    }
}