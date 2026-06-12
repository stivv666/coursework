using System;

namespace TestTrainer.Exceptions
{
    public class InvalidTestFormatException : Exception
    {
        public InvalidTestFormatException()
            : base("Файл тесту має невірний формат або пошкоджений.") { }

        public InvalidTestFormatException(string message)
            : base(message) { }

        public InvalidTestFormatException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}