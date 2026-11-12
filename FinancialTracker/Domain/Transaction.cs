using FinancialTracker.Domain.Exceptions;
using FinancialTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialTracker.Domain
{
     public record class Transaction : IEntity
    {
        public int Id { get; init; }
        public int CategoryId { get; init; }

        public DateTime Date { get; init; }
        public TransactionType Type { get; init; }
        public Money Amount { get; init; }

        public string? Comment { get; init; }

        public  Transaction(int id,int categoryId, DateTime date, TransactionType type,Money amount, string? comment = null)
        {
            if (id <= 0)
            {
                throw new DomainValidationException("Id - натуральное число!");
            }

            if (categoryId <= 0)
            {
                throw new DomainValidationException("CategoryId - натуральное число!");
            }

            if (amount.Amount  <= 0)
            {
                throw new DomainValidationException("Нельзя создать операцию с нулем или минусом.");
            }

            if (date == default(DateTime))
            {
                throw new DomainValidationException("Дата не должна быть дефолтной.");
            }
            if(!(comment is null) && comment.Length > 200)
            {
                throw new DomainValidationException("Длинна комментария более 200 символов");
            }

            Id = id;
            Date = date;
            Type = type;
            Amount = amount;
            CategoryId = categoryId;
            Comment = comment;
        }
    }
}
