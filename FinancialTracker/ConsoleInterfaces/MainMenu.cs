using FinancialTracker.Domain.Exceptions;
using FinancialTracker.Services;
namespace FinancialTracker.ConsoleInterfaces
{
    public class MainMenu
    {

        private readonly CategoryService _categoryService;
        private readonly TransactionService _transactionService;

        private readonly AnalyticsService _analyticsService;

        public MainMenu(CategoryService categoryService, TransactionService transactionService, AnalyticsService analyticsService)
        {
            _categoryService = categoryService;
            _transactionService = transactionService;
            _analyticsService = analyticsService;
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
                        DeleteTransaction();
                        break;
                    case "4":
                        ManageCategories();
                        break;
                    case "5":
                        ShowAnalyticsMenu();
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
            Console.WriteLine("3. Удалить операцию");
            Console.WriteLine("4. Управление категориями");
            Console.WriteLine("5. Аналитика");
            Console.WriteLine("0. Выход");
            Console.WriteLine("----------------------------------------");
            Console.Write("Выберите действие: ");
        }

        private void AddTransaction()

        {
            var categoryServiceItems = _categoryService.GetAll();

            Console.WriteLine("Добавление новой операции ");

            foreach (var cat in categoryServiceItems)
            {
                Console.WriteLine($"{cat.Id} , {cat.Name}, {cat.Type}");
            }

            int categoryId = ConsoleHelper.ReadInteger("Введите ID категории ");

            var selectedCategory = categoryServiceItems.FirstOrDefault(x => x.Id == categoryId);

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
            catch (IOException ex)   // <-- НОВЫЙ CATCH
            {
                Console.WriteLine("\nОШИБКА СОХРАНЕНИЯ НА ДИСК!");
                Console.WriteLine("Файл данных занят другой программой (Excel, OneDrive, антивирус).");
                Console.WriteLine("Закройте файл данных и попробуйте снова.");
                Console.WriteLine($"Подробности: {ex.Message}");
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
            catch (IOException ex) 
            {
                Console.WriteLine("\nОШИБКА СОХРАНЕНИЯ НА ДИСК!");
                Console.WriteLine("Файл данных занят другой программой (Excel, OneDrive, антивирус).");
                Console.WriteLine("Закройте файл данных и попробуйте снова.");
                Console.WriteLine($"Подробности: {ex.Message}");
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
            catch (IOException ex)   // <-- НОВЫЙ CATCH
            {
                Console.WriteLine("\nОШИБКА СОХРАНЕНИЯ НА ДИСК!");
                Console.WriteLine("Файл данных занят другой программой (Excel, OneDrive, антивирус).");
                Console.WriteLine("Закройте файл данных и попробуйте снова.");
                Console.WriteLine($"Подробности: {ex.Message}");
            }

        }

        private void DeleteTransaction()
        {
            Console.WriteLine("--- Удаление операции ---");

            if (_transactionService.GetAll().Count == 0)
            {
                Console.WriteLine("Операций пока нет. Удалять нечего.");
                return;
            }
            ShowTransactions();
            int id = ConsoleHelper.ReadInteger("Введите Id операции для удаления: ");
            string? agreement = ConsoleHelper.ReadString("Вы уверены? (да/нет): ");
            if(agreement.Equals("да",StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    _transactionService.Delete(id);
                    Console.WriteLine("Операция успешно удалена ");
                }
                catch (DomainValidationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (IOException ex)
                {
                    Console.WriteLine("\nОШИБКА СОХРАНЕНИЯ НА ДИСК!");
                    Console.WriteLine("Файл данных занят другой программой (Excel, OneDrive, антивирус).");
                    Console.WriteLine("Закройте файл данных и попробуйте снова.");
                    Console.WriteLine($"Подробности: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Отмена операции удаления ");
                return;
            }

           
        }

        private void ShowOverallAnalytics()
        {
            Console.WriteLine("--- Аналитика ---");
            Console.WriteLine($"Общий доход: {_analyticsService.GetTotalIncome():F2} BYN");
            Console.WriteLine($"Общий расход: {_analyticsService.GetTotalExpense():F2} BYN");
            decimal balance = _analyticsService.GetBalance();
            if (balance < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine($"Текущий баланс: {balance:F2}  BYN");
            Console.ResetColor();
            Console.WriteLine("--- Расходы по категориям ---");
            var report = _analyticsService.GetExpensesByCategory();

            if (report.Count == 0) Console.WriteLine("Расходов пока нет. ");
            else
            {
                foreach(var item in report)
                {
                    Console.WriteLine($"{item.CategoryName,-20} {item.Total:F2} BYN");
                }
            }



        }
        private void ShowAnalyticsMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("========================================");
                Console.WriteLine("              АНАЛИТИКА ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Общая статистика (за всё время) ");
                Console.WriteLine("2. Статистика за месяц");
                Console.WriteLine("3. Топ-5 расходов");
                Console.WriteLine("4. Средний расход в день");
                Console.WriteLine("0. Назад в главное меню");
                Console.WriteLine("----------------------------------------");
                Console.Write("Выберите действие: ");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowOverallAnalytics();
                        break;

                    case "2":
                        ShowMonthlyAnalytics();
                        break;

                    case "3":
                        ShowTopExpenses();
                        break;

                    case "4":
                        ShowAverageDailyExpense();
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

        private void ShowMonthlyAnalytics()
        {
            Console.WriteLine("\n--- Статистика за месяц ---");
            int year = ConsoleHelper.ReadInteger("Введите год (например, 2026): ");

            int month = ConsoleHelper.ReadInteger("Введите месяц (1-12): ");

            if(month < 1 || month > 12)
            {
                Console.WriteLine("Ошибка: месяц должен быть от 1 до 12.");
                return;
            }

            if (year < 2000 || year > DateTime.Now.Year + 1)
            {
                Console.WriteLine($"Ошибка: год должен быть между 2000 и {DateTime.Now.Year + 1}.");
                return;
            }

            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            var summary = _analyticsService.GetSummaryForPeriods(start, end);

            Console.WriteLine($"Период: {start:MMMM yyyy}");  
            Console.WriteLine($"Доход:   {summary.Income:F2} BYN");
            Console.WriteLine($"Расход:  {summary.Expense:F2} BYN");

            if (summary.Balance < 0) Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Баланс:  {summary.Balance:F2} BYN");
            Console.ResetColor();
        }

        private void ShowTopExpenses()
        {
            Console.WriteLine("\n--- Топ-5 самых крупных расходов ---");
            var top = _analyticsService.GetTopExpenses(5);

            if (top.Count == 0)
            {
                Console.WriteLine("Расходов пока нет.");
                return;
            }

            int rank = 1;
            foreach (var item in top)
            {
                Console.WriteLine($"{rank}. {item.Date.ToShortDateString()} | {item.CategoryName,-15} | {item.Amount:F2} BYN");
                rank++;
            }
        }

        private void ShowAverageDailyExpense()
        {
            Console.WriteLine("\n--- Средний расход в день ---");
            decimal average = _analyticsService.GetAverageDailyExpense();
            Console.WriteLine($"Средний расход: {average:F2} BYN/день");
            Console.WriteLine("(считается от первой операции до сегодня)");
        }
    }

    
}
