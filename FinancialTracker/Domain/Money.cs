using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FinancialTracker.Domain.Exceptions;

namespace FinancialTracker.Domain
   
{

    public readonly struct Money : IEquatable<Money>
    {
        public readonly decimal Amount;
        public readonly string Currency;

        public Money(decimal amount, string currency)
        {
            
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("Валюта не может быть пустой.", nameof(currency));
            }
            
                Currency = currency;
                Amount = amount;
            
        }

        public bool Equals(Money other)
        {
            return Amount == other.Amount && Currency == other.Currency;
        }

        public static bool operator ==(Money left, Money right)
        {
           
            return left.Equals(right);
        }

        public static bool operator !=(Money left, Money right)
        {

            return !(left.Equals(right));
        }

        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw new CurrencyMismatchException($"Нельзя складывать разные валюты: {left.Currency} и {right.Currency}");
            }
            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw new CurrencyMismatchException($"Нельзя вычетать разные валюты: {left.Currency} и {right.Currency}");
            }
            return new Money(left.Amount - right.Amount, left.Currency);
        }
        public override string ToString()
        {
            return $"{Amount.ToString("F2")} {Currency}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Money other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
    }
    
}
