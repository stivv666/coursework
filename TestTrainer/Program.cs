using System;
using TestTrainer.Logic;
using TestTrainer.Models;
using TestTrainer.Data;

namespace TestTrainer
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataFolder = "TestData";
            TestEngine engine = new TestEngine(dataFolder);
            TestRepository repo = new TestRepository(dataFolder);

            engine.OnTestFinished += ShowFinalScore;

            bool isRunning = true;
            int selectedIndex = 0;

            string[] menuItems = new string[6];
            menuItems[0] = "Run Test Session";
            menuItems[1] = "Add New Open Question to Topic";
            menuItems[2] = "Delete Question from Topic";
            menuItems[3] = "View History";
            menuItems[4] = "View Statistics";
            menuItems[5] = "Exit";

            while (isRunning == true)
            {
                Console.Clear();
                Console.WriteLine("EXAM SIMULATOR\n");

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
                            Console.Write("Enter topic name to run: ");
                            string runTopicName = Console.ReadLine();
                            engine.RunSession(runTopicName);

                            Console.WriteLine("\nPress any key to return to menu...");
                            Console.ReadKey(true);
                            break;

                        case 1:
                            Console.Write("Enter topic name: ");
                            string addTopicName = Console.ReadLine();
                            TestTopic topicToEdit = repo.LoadTopic(addTopicName);

                            Console.Write("Enter question text: ");
                            string qText = Console.ReadLine();

                            Console.Write("Enter correct answer: ");
                            string qAnswer = Console.ReadLine();

                            OpenQuestion newQuestion = new OpenQuestion();
                            newQuestion.Text = qText;
                            newQuestion.CorrectAnswer = qAnswer;
                            newQuestion.Difficulty = QuestionDifficulty.Medium;

                            topicToEdit.OpenQuestions.Add(newQuestion);
                            repo.SaveTopic(topicToEdit);

                            Console.WriteLine("Question added successfully!");
                            Console.WriteLine("\nPress any key to return to menu...");
                            Console.ReadKey(true);
                            break;

                        case 2:
                            Console.Write("Enter topic name: ");
                            string delTopicName = Console.ReadLine();
                            TestTopic topicToModify = repo.LoadTopic(delTopicName);

                            if (topicToModify.OpenQuestions.Count == 0)
                            {
                                Console.WriteLine("No open questions found in this topic.");
                            }
                            else
                            {
                                Console.WriteLine("\nQuestions list:");
                                for (int i = 0; i < topicToModify.OpenQuestions.Count; i++)
                                {
                                    Console.WriteLine((i + 1) + ". " + topicToModify.OpenQuestions[i].Text);
                                }

                                Console.Write("\nEnter number of question to delete: ");
                                int delIndex = -1;
                                if (int.TryParse(Console.ReadLine(), out delIndex) && delIndex > 0 && delIndex <= topicToModify.OpenQuestions.Count)
                                {
                                    topicToModify.OpenQuestions.RemoveAt(delIndex - 1);
                                    repo.SaveTopic(topicToModify);
                                    Console.WriteLine("Question deleted successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid index.");
                                }
                            }
                            Console.WriteLine("\nPress any key to return to menu...");
                            Console.ReadKey(true);
                            break;

                        case 3:
                            string historyFile = dataFolder + "/history.txt";
                            if (File.Exists(historyFile))
                            {
                                Console.WriteLine("HISTORY");
                                string historyData = File.ReadAllText(historyFile);
                                Console.WriteLine(historyData);
                            }
                            else
                            {
                                Console.WriteLine("History is empty.");
                            }
                            Console.WriteLine("\nPress any key to return to menu...");
                            Console.ReadKey(true);
                            break;

                        case 4:
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

                                Console.WriteLine("SUCCESS STATISTICS");
                                Console.WriteLine("Total test sessions analyzed: " + lines.Length);
                                Console.WriteLine("Total correct answers given: " + totalCorrect);
                                Console.WriteLine("Total questions answered: " + totalAsked);

                                if (totalAsked > 0)
                                {
                                    double percentage = ((double)totalCorrect / totalAsked) * 100;
                                    Console.WriteLine("Global success rate: " + Math.Round(percentage, 2) + "%");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No statistics available. Take some tests first!");
                            }
                            Console.WriteLine("\nPress any key to return to menu...");
                            Console.ReadKey(true);
                            break;

                        case 5:
                            isRunning = false;
                            Console.WriteLine("Goodbye!");
                            break;
                    }
                }
            }
        }

        static void ShowFinalScore(int score, int total)
        {
            Console.WriteLine("\nTEST FINISHED");
            Console.WriteLine("Your final score is: " + score + " out of " + total);
        }
    }
}