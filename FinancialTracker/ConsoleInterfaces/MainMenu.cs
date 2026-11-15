using FinancialTracker.Domain.Exceptions;
using FinancialTracker.Services;

namespace FinancialTracker.ConsoleInterfaces
{
    public class MainMenu
    {

        private readonly CategoryService _categoryService;
        private readonly TransactionService _transactionService;

        public MainMenu(CategoryService categoryService, TransactionService transactionService)
        {
            _categoryService = categoryService;
            _transactionService = transactionService;
        }

        public void Run()
        {
            while (true)
            {
                PrintMenu();

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddTransaction();
                        break;
                    case "2":
                        ShowTransactions();
                        break;
                    case "3":
                        ManageCategories();
                        break;
                    case "4":
                        Console.WriteLine("Вы выбрали: Аналитика (пока не реализовано)");
                        break;
                    case "0":
                        Console.WriteLine("Выход из программы. До свидания!");
                        return;
                    default:
                        Console.WriteLine("Неверный ввод. Пожалуйста, выберите пункт из меню.");
                        break;

                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");

                Console.ReadKey();

                Console.Clear();


            }
        }

        private void PrintMenu()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("       ФИНАНСОВЫЙ ТРЕКЕР v1.0");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Добавить операцию");
            Console.WriteLine("2. Показать операции");
            Console.WriteLine("3. Управление категориями");
            Console.WriteLine("4. Аналитика");
            Console.WriteLine("0. Выход");
            Console.WriteLine("----------------------------------------");
            Console.Write("Выберите действие: ");
        }

        private void AddTransaction()

        {
            var _categoryServiceItems = _categoryService.GetAll();

            Console.WriteLine("Добавление новой операции ");

            foreach (var cat in _categoryServiceItems)
            {
                Console.WriteLine($"{cat.Id} , {cat.Name}, {cat.Type}");
            }

            int categoryId = ConsoleHelper.ReadInteger("Введите ID категории ");

            var selectedCategory = _categoryServiceItems.FirstOrDefault(x => x.Id == categoryId);

            if (selectedCategory == null)
            {
                Console.WriteLine($"Ошибка: Категория с Id {categoryId} не найдена! ");
                return;
            }
            decimal amount = ConsoleHelper.ReadDecimal("Введите сумму ");
            DateTime date = ConsoleHelper.ReadDate("Введите дату (ДД.ММ.ГГГГ) ");
            string comment = ConsoleHelper.ReadString("Введите комментарий (или ПРОБЕЛ для пропуска) ", allowEmpty: true);

            try
            {
                string currency = "BYN";

                var newTransaction = _transactionService.Add(date, selectedCategory.Type, amount, currency, categoryId, comment);

                Console.WriteLine($"\nУСПЕХ: Операция добавлена! ({newTransaction.Amount} на {newTransaction.Date.ToShortDateString()}) ");
            }
            catch (DomainValidationException ex)
            {
                Console.WriteLine($"\nОШИБКА ДОБАВЛЕНИЯ: {ex.Message}");
            }
        }

        private void ShowTransactions()
        {
            var transactions = _transactionService.GetAll();

            if (transactions.Count == 0)
            {
                Console.WriteLine("Операций пока нет. Добавьте первую!");
                return;
            }
            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"{"Id",-4} | {"Дата",-12} | {"Тип",-8} | {"Сумма",-15} | {"Категория",-15} | Комментарий");
            Console.WriteLine(new string('-', 80));
            var categories = _categoryService.GetAll();
            foreach (var tran in transactions)
            {
                string categoryName = categories.FirstOrDefault(c => c.Id == tran.CategoryId)?.Name ?? "Unknown";
                Console.WriteLine($"{tran.Id,-4} | {tran.Date.ToShortDateString(),-12} | {tran.Type,-8} | {tran.Amount,-15} | {categoryName,-15} | {tran.Comment}");
                Console.WriteLine(new string('-', 80));
            }
        }

        private void ManageCategories()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("========================================");
                Console.WriteLine("       УПРАВЛЕНИЕ КАТЕГОРИЯМИ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Показать все категории");
                Console.WriteLine("2. Добавить новую категорию");
                Console.WriteLine("3. Удалить категорию");
                Console.WriteLine("0. Назад в главное меню");
                Console.WriteLine("----------------------------------------");
                Console.Write("Выберите действие: ");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        foreach (var cat in _categoryService.GetAll())
                        {
                            Console.WriteLine($"{cat.Id} {cat.Name} {cat.Type}");
                        }
                        break;

                    case "2":
                        AddCategory();
                        break;

                    case "3":
                        DeleteCategory();
                        break;

                    case "0":
                        return;
                       

                    default:
                        Console.WriteLine("Неверный ввод ");
                        break;

                }

                if (input != "0")
                {
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private void AddCategory()
        {
            Console.WriteLine("----- Добавление категории -----");

            string name = ConsoleHelper.ReadString("Введите имя категории ");

            Console.WriteLine("Выберите тип: 1 - Доход (Income), 2 - Расход (Expense) ");
            int typeChoice = ConsoleHelper.ReadInteger("Ваш выбор (1 или 2) ");   
            TransactionType type = typeChoice == 1 ? TransactionType.Income : TransactionType.Expense;

            try
            {
                var newCat = _categoryService.Add(name, type);
                Console.WriteLine($"УСПЕХ: Категория '{newCat.Name}' добавлена с Id {newCat.Id}");
            }
            catch (DomainValidationException ex)
            {
                Console.WriteLine($"ОШИБКА: {ex.Message}");
            }

        }
        private void DeleteCategory()
        {
            Console.WriteLine("----- Удаление категории -----");
            foreach (var cat in _categoryService.GetAll())
            {
                Console.WriteLine($"{cat.Id} {cat.Name} {cat.Type}");
            }
                

                int idToDelete = ConsoleHelper.ReadInteger("Введите Id категории для удаления ");

                try
                {
                    _categoryService.Delete(idToDelete);
                    Console.WriteLine($"УСПЕХ: Категория с Id {idToDelete} успешно удалена.");
                }
                catch (DomainValidationException ex)
                {
                    Console.WriteLine($"ОШИБКА: {ex.Message}");
                }
            
        }
    }
}
