using System.Globalization;

namespace FinancialTracker.ConsoleInterfaces
{
    public static class ConsoleHelper
    {
        public static int ReadInteger(string prompt)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out result))
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Ошибка: Введите корректное число!");
                }
            }
        }

        public static decimal ReadDecimal(string prompt)
        {
            decimal result;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                input = input?.Replace('.', ',');

                if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.GetCultureInfo("ru-RU"), out result))
                {
                    if (result <= 0)
                    {
                        Console.WriteLine("Ошибка: Сумма должна  больше нуля!");
                        continue;
                    }
                    return result;
                }
                else
                {
                    Console.WriteLine("Ошибка: Введите корректное число!");
                }
            }
        }

        public static DateTime ReadDate(string prompt)
        {
            DateTime result;
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {   if (result.Year < 2000 || result.Year > DateTime.Now.Year + 1)
                    {
                        Console.WriteLine($"Ошибка: Год должен быть между 2000 и {DateTime.Now.Year + 1}");
                        continue;
                    }
                    return result;
                }
                else
                {
                    Console.WriteLine("Ошибка: Введите дату в формате ДД.ММ.ГГГГ");
                }
            }
        }

        public static string ReadString(string prompt, bool allowEmpty = false, int maxLength = 200)
        {


            while (true)
            {
                Console.Write(prompt);

                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    if (allowEmpty) return string.Empty;

                    Console.WriteLine("Поле не должно быть пустым");
                    continue;
                }
                if (input.Length > maxLength)
                {
                    Console.WriteLine($"Ошибка: Текст не должен быть длиннее {maxLength} символов.");
                    continue;
                }
                return input.Trim();
            }
        }

        internal static string ReadString(string v, object value)
        {
            throw new NotImplementedException();
        }
    }
}
