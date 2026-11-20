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

            // 3. Инициализация бизнес-логики (НИЧЕГО НЕ МЕНЯЕТСЯ!)
            var categoryService = new CategoryService(categoryRepo, transactionRepo,settingsRepo);
            var transactionService = new TransactionService(transactionRepo, categoryRepo, settingsRepo);

            // 4. Запуск пользовательского интерфейса (НИЧЕГО НЕ МЕНЯЕТСЯ!)
            var menu = new MainMenu(categoryService, transactionService);
            menu.Run();


        }
    }
}

