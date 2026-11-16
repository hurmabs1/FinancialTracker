using FinancialTracker.ConsoleInterfaces;
using FinancialTracker.Domain;
using FinancialTracker.Services;
using FinancialTracker.Storage;

namespace FinancialTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Временный тест сериализации
            var cat = new Category(1, "Продукты", TransactionType.Expense);
            string catLine = CsvSerializer.SerializeCategory(cat);
            Console.WriteLine($"Сериализовано: {catLine}");
            var catBack = CsvSerializer.DeserializeCategory(catLine);
            Console.WriteLine($"Десериализовано: {catBack?.Name}");

            var money = new Money(150.50m, "BYN");
            var tran = new Transaction(1, 1, new DateTime(2026, 11, 14), TransactionType.Expense, money, "Хлеб; молоко\nи кефир");
            string tranLine = CsvSerializer.SerializeTransaction(tran);
            Console.WriteLine($"Сериализовано: {tranLine}");
            var tranBack = CsvSerializer.DeserializeTransaction(tranLine);
            Console.WriteLine($"Десериализовано: {tranBack?.Amount} | Коммент: {tranBack?.Comment}");

            Console.ReadKey();
            return; // Не запускаем меню, чтобы посмотреть тест



        }
    }
}

