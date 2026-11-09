using FinancialTracker.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialTracker.Domain
{
    public record class Category
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public TransactionType Type { get; init; }

        public Category(int id, string name, TransactionType type)
        {
           if (id <= 0)
            {
                throw new DomainValidationException("Id - натуральное число!");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainValidationException("Имя категории не может быть пустой.");
            }
            Id = id;
            Name = name;
            Type = type;
        }
    }
}
