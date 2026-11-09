using System;

namespace FinancialTracker.Domain.Exceptions
{ 
    public class DomainValidationException : Exception
    {
        public DomainValidationException() : base("Произошла ошибка валидации доменных данных.")
        {
        
        }

        public DomainValidationException(string message) : base(message)
        {

        }

        public DomainValidationException(string message, Exception innerExeption) : base(message, innerExeption)
        {

        }

    }


}