using NUnit.Framework;
using TestTrainer.Models;

namespace TestTrainer.Tests
{
    public class OpenQuestionTests
    {
        [Test]
        public void CheckAnswer_ExactMatch_ReturnsTrue()
        {
            OpenQuestion q = new OpenQuestion();
            q.CorrectAnswer = "Kyiv";
            Assert.That(q.CheckAnswer("Kyiv"), Is.True);
        }

        [Test]
        public void CheckAnswer_LowerCase_ReturnsTrue()
        {
            OpenQuestion q = new OpenQuestion();
            q.CorrectAnswer = "Kyiv";
            Assert.That(q.CheckAnswer("kyiv"), Is.True);
        }

        [Test]
        public void CheckAnswer_UpperCase_ReturnsTrue()
        {
            OpenQuestion q = new OpenQuestion();
            q.CorrectAnswer = "kyiv";
            Assert.That(q.CheckAnswer("KYIV"), Is.True);
        }

        [Test]
        public void CheckAnswer_LeadingAndTrailingSpaces_ReturnsTrue()
        {
            OpenQuestion q = new OpenQuestion();
            q.CorrectAnswer = "Kyiv";
            Assert.That(q.CheckAnswer("   Kyiv   "), Is.True);
        }

        [Test]
        public void CheckAnswer_WrongAnswer_ReturnsFalse()
        {
            OpenQuestion q = new OpenQuestion();
            q.CorrectAnswer = "Kyiv";
            Assert.That(q.CheckAnswer("Lviv"), Is.False);
        }

        [Test]
        public void CheckAnswer_EmptyString_ReturnsFalse()
        {
            OpenQuestion q = new OpenQuestion();
            q.CorrectAnswer = "Kyiv";
            Assert.That(q.CheckAnswer(""), Is.False);
        }

        [Test]
        public void CheckAnswer_NullString_ReturnsFalse()
        {
            OpenQuestion q = new OpenQuestion();
            q.CorrectAnswer = "Kyiv";
            Assert.That(q.CheckAnswer(null), Is.False);
        }

        [Test]
        public void CheckAnswer_NumbersInString_ReturnsTrue()
        {
            OpenQuestion q = new OpenQuestion();
            q.CorrectAnswer = "2024";
            Assert.That(q.CheckAnswer("2024"), Is.True);
        }

        [Test]
        public void CheckAnswer_SpecialCharacters_ReturnsTrue()
        {
            OpenQuestion q = new OpenQuestion();
            q.CorrectAnswer = "C#";
            Assert.That(q.CheckAnswer("c#"), Is.True);
        }
    }
}