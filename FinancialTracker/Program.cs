using FinancialTracker.ConsoleInterfaces;
using FinancialTracker.Domain;
using FinancialTracker.Services;

namespace FinancialTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var categoryRepo = new InMemoryRepository<Category>();
            var transactionRepo = new InMemoryRepository<Transaction>();

            DataSeeder.SeedCategories(categoryRepo);

            var categoryService = new CategoryService(categoryRepo, transactionRepo);
            var transactionService = new TransactionService(transactionRepo, categoryRepo);

            var menu = new MainMenu(categoryService, transactionService);

           
            menu.Run();



        }
    }
}

