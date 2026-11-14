using FinancialTracker.Domain;
using FinancialTracker.Domain.Exceptions;
using FinancialTracker.Services;

namespace FinancialTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var CategoryRepo = new InMemoryRepository<Category>();
            var TransactionRepo = new InMemoryRepository<Transaction>();
            DataSeeder.SeedCategories(CategoryRepo);
            var CategoryService = new CategoryService(CategoryRepo, TransactionRepo);
            var TransactionService = new TransactionService(TransactionRepo, CategoryRepo);



            foreach (var cat in CategoryService.GetAll())
            {
                Console.WriteLine($"{cat.Id}. {cat.Name} ({cat.Type})");
            }
                var productsCategory = CategoryService.GetAll().First(x => x.Name == "Продукты");

                Console.WriteLine("\n=== Добавляем расход ===");
                var t1 = TransactionService.Add(
                    date: DateTime.Now,
                    type: TransactionType.Expense,
                    amount: 150.50m,
                    currency: "BYN",
                    categoryId: productsCategory.Id,
                    comment: "Покупка в магазине"
                );

                Console.WriteLine($"Добавлено: {t1.Amount} на {t1.Date.ToShortDateString()}");
                // 6. ТЕСТ ОШИБКИ 1: Пытаемся добавить ДОХОД в категорию РАСХОДА ("Продукты")
                Console.WriteLine("\n=== Тест ошибки: Доход в категорию расхода ===");
                try
                {
                    TransactionService.Add(DateTime.Now, TransactionType.Income, 1000m, "BYN", productsCategory.Id, "Ошибка");
                }
                catch (DomainValidationException ex)
                {
                    Console.WriteLine($"Поймана ожидаемая ошибка: {ex.Message}");
                }

                // 7. ТЕСТ ОШИБКИ 2: Пытаемся удалить категорию "Продукты" (по ней есть операция t1)
                Console.WriteLine("\n=== Тест ошибки: Удаление занятой категории ===");
                try
                {
                    CategoryService.Delete(productsCategory.Id);
                }
                catch (DomainValidationException ex)
                {
                    Console.WriteLine($"Поймана ожидаемая ошибка: {ex.Message}");
                }

                // 8. ТЕСТ УСПЕХА: Добавляем и удаляем пустую категорию
                Console.WriteLine("\n=== Тест успеха: Добавление и удаление новой категории ===");
                var newCat = CategoryService.Add("Тестовая категория", TransactionType.Expense);
                Console.WriteLine($"Добавлена категория: {newCat.Name} с Id {newCat.Id}");
                try
                {
                    CategoryService.Delete(newCat.Id);
                    Console.WriteLine("Категория успешно удалена.");
                }
                catch (DomainValidationException ex)
                {
                    Console.WriteLine($"Ошибка при удалении (не ожидали этого!): {ex.Message}");
                }
            }

        }
    }

