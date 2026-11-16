using FinancialTracker.Domain;
using FinancialTracker.Domain.Exceptions;
using System.Globalization;

namespace FinancialTracker.Storage
{
    public static class CsvSerializer
    {

        public static string SerializeCategory(Category category)
        {
            return $"{category.Id};{category.Name};{category.Type}";
        }

        public static Category? DeserializeCategory(string line)
        {
            var parts = line.Split(';');
            if (parts.Length != 3) return null;
            if (!int.TryParse(parts[0], out int id)) return null;
            if (!Enum.TryParse<TransactionType>(parts[2], out var type)) return null;
            string name = parts[1];

            try
            {
                return new Category(id, name, type);
            }
            catch(DomainValidationException)
            {
                return null;
            }
        }

        public static string SerializeTransaction(Transaction transaction)
        {
            string dateStr = transaction.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string amountStr = transaction.Amount.Amount.ToString("F2", CultureInfo.InvariantCulture);
            string safeComment = transaction.Comment?.Replace(';', ',').Replace("\n", " ").Replace("\r", "") ?? "";


            return $"{transaction.Id};{transaction.CategoryId};{dateStr};{transaction.Type};" +
                $"{amountStr};{transaction.Amount.Currency};{safeComment}";
        }

        public static Transaction? DeserializeTransaction(string line)
        {
            var parts = line.Split(';');
            if (parts.Length != 7) return null;

            if (!int.TryParse(parts[0], out int id)) return null;
            if (!int.TryParse(parts[1], out int categoryId)) return null;
            if (!DateTime.TryParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)) return null;
            if (!Enum.TryParse<TransactionType>(parts[3], out var type)) return null;
            if (!decimal.TryParse(parts[4], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount)) return null;
            string currency = parts[5];
            string comment = parts[6];
            try
            {
                var money = new Money(amount, currency);
                return new Transaction(id, categoryId, date, type, money, comment);
            }
            catch (DomainValidationException)
            {
                return null;
            }
        }
    }


}
