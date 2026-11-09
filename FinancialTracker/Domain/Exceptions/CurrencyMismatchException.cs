using System;

namespace FinancialTracker.Domain.Exceptions
{
    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException() : base("Невозможно выполнить операцию с разными валютами.")
        {
        }

        public CurrencyMismatchException(string message) : base(message)
        {
        }

        public CurrencyMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
