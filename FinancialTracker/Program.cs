using FinancialTracker.Domain;

namespace FinancialTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var c = new Category(2, "магазин", TransactionType.Expense);
            var sum1 = new Money(1500m, "BYN");
            var sum2 = new Money(2000m, "USD");
            var t1 = new Transaction(1, new DateTime(2026, 11, 9), TransactionType.Expense, sum1);
            var t2 = new Transaction(2, new DateTime(2026, 11, 9), TransactionType.Income, sum2);
            var sum3 = sum1 + sum2;
            Console.WriteLine(sum1);
            Console.WriteLine(sum2);
            Console.WriteLine(sum3);

        }
    }
}
