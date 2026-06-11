using NUnit.Framework;
using TestTrainer.Models;

namespace TestTrainer.Tests
{
    public class SingleChoiceQuestionTests
    {
        [Test]
        public void CheckAnswer_CorrectValidIndex_ReturnsTrue()
        {
            SingleChoiceQuestion q = new SingleChoiceQuestion();
            q.CorrectOptionIndex = 1;
            Assert.That(q.CheckAnswer("2"), Is.True);
        }

        [Test]
        public void CheckAnswer_WrongValidIndex_ReturnsFalse()
        {
            SingleChoiceQuestion q = new SingleChoiceQuestion();
            q.CorrectOptionIndex = 1;
            Assert.That(q.CheckAnswer("1"), Is.False);
        }

        [Test]
        public void CheckAnswer_IndexTooHigh_ReturnsFalse()
        {
            SingleChoiceQuestion q = new SingleChoiceQuestion();
            q.CorrectOptionIndex = 0;
            Assert.That(q.CheckAnswer("99"), Is.False);
        }

        [Test]
        public void CheckAnswer_ZeroInput_ReturnsFalse()
        {
            SingleChoiceQuestion q = new SingleChoiceQuestion();
            q.CorrectOptionIndex = 0;
            Assert.That(q.CheckAnswer("0"), Is.False);
        }

        [Test]
        public void CheckAnswer_NegativeInput_ReturnsFalse()
        {
            SingleChoiceQuestion q = new SingleChoiceQuestion();
            q.CorrectOptionIndex = 0;
            Assert.That(q.CheckAnswer("-1"), Is.False);
        }

        [Test]
        public void CheckAnswer_TextInsteadOfNumber_ReturnsFalse()
        {
            SingleChoiceQuestion q = new SingleChoiceQuestion();
            q.CorrectOptionIndex = 0;
            Assert.That(q.CheckAnswer("Apple"), Is.False);
        }

        [Test]
        public void CheckAnswer_EmptyString_ReturnsFalse()
        {
            SingleChoiceQuestion q = new SingleChoiceQuestion();
            q.CorrectOptionIndex = 0;
            Assert.That(q.CheckAnswer(""), Is.False);
        }

        [Test]
        public void CheckAnswer_NullInput_ReturnsFalse()
        {
            SingleChoiceQuestion q = new SingleChoiceQuestion();
            q.CorrectOptionIndex = 0;
            Assert.That(q.CheckAnswer(null), Is.False);
        }
    }
}