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
            try
            {
                var settingsRepo = new SettingsRepository("data/settings.csv");
                var categoryRepo = new CsvFileRepository<Category>(
                    filePath: "data/categories.csv",
                    serializer: CsvSerializer.SerializeCategory,
                    deserializer: CsvSerializer.DeserializeCategory,
                    header: "Id;Name;Type");

                var transactionRepo = new CsvFileRepository<Transaction>(
                    filePath: "data/transactions.csv",
                    serializer: CsvSerializer.SerializeTransaction,
                    deserializer: CsvSerializer.DeserializeTransaction,
                    header: "Id;CategoryId;Date;Type;Amount;Currency;Comment");

                DataSeeder.SeedCategories(categoryRepo);

                var categoryService = new CategoryService(categoryRepo, transactionRepo, settingsRepo);
                var transactionService = new TransactionService(transactionRepo, categoryRepo, settingsRepo);

                var menu = new MainMenu(categoryService, transactionService);
                menu.Run();
            }
            catch (IOException ex)
            {
                Console.WriteLine("ОШИБКА ДОСТУПА К ФАЙЛАМ ДАННЫХ!");
                Console.WriteLine($"Причина: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine("Возможные решения:");
                Console.WriteLine("  - Закройте файлы данных в Excel или других программах.");
                Console.WriteLine("  - Дождитесь окончания синхронизации OneDrive.");
                Console.WriteLine("  - Проверьте, что папка data не открыта в другой программе.");
                Console.WriteLine();
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}

