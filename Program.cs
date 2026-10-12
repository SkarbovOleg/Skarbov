using System.Text;
using Microsoft.Data.SqlClient;

namespace SkarbovConsoleApp;

internal class Program
{
    // ============================================================
    // СТРОКА ПОДКЛЮЧЕНИЯ
    // Пример: "Server=localhost;Database=TestWpfDb;Trusted_Connection=True;TrustServerCertificate=True;"
    // Что поменять на своём компьютере:
    //   Server   — имя вашего SQL Server (например, localhost\SQLEXPRESS)
    //   Database — имя базы (TestWpfDb)
    //   Trusted_Connection=True — Windows-аутентификация
    //   Либо: User Id=sa;Password=ваш_пароль;
    // ============================================================
    private const string ConnectionString =
        "Server=localhost;Database=TestWpfDb;Trusted_Connection=True;TrustServerCertificate=True;";

    private static EmployeeRepository _repository = null!;

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        _repository = new EmployeeRepository(ConnectionString);

        while (true)
        {
            PrintMenu();
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1": ShowAll(); break;
                case "2": Add(); break;
                case "3": Update(); break;
                case "4": Delete(); break;
                case "0": return;
                default:
                    Console.WriteLine("Неверный пункт меню. Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("=== Учёт сотрудников (Скарбов) ===");
        Console.WriteLine("1 — Показать все записи");
        Console.WriteLine("2 — Добавить запись");
        Console.WriteLine("3 — Обновить запись");
        Console.WriteLine("4 — Удалить запись");
        Console.WriteLine("0 — Выход");
        Console.Write("Выберите пункт: ");
    }

    // ---------------- Показать все ----------------
    private static void ShowAll()
    {
        try
        {
            var list = _repository.GetAll();
            if (list.Count == 0)
            {
                Console.WriteLine("Записей нет.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"{"ID",-5} {"ФИО",-30} {"Должность",-30} {"Зарплата",12}");
            Console.WriteLine(new string('-', 80));
            foreach (var e in list)
            {
                Console.WriteLine($"{e.Id,-5} {e.FullName,-30} {e.Position,-30} {e.Salary,12:N2}");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Ошибка SQL: {ex.Message}");
        }
    }

    // ---------------- Добавить ----------------
    private static void Add()
    {
        try
        {
            string fullName = ReadNonEmptyString("Введите ФИО: ");
            string position = ReadNonEmptyString("Введите должность: ");
            decimal salary = ReadDecimal("Введите зарплату: ");

            _repository.Insert(new Employee
            {
                FullName = fullName,
                Position = position,
                Salary = salary
            });

            Console.WriteLine("Запись добавлена.");
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Ошибка SQL: {ex.Message}");
        }
    }

    // ---------------- Обновить ----------------
    private static void Update()
    {
        try
        {
            int id = ReadInt("Введите Id записи для обновления: ");

            if (!_repository.Exists(id))
            {
                Console.WriteLine($"Запись с Id={id} не найдена.");
                return;
            }

            string fullName = ReadNonEmptyString("Новое ФИО: ");
            string position = ReadNonEmptyString("Новая должность: ");
            decimal salary = ReadDecimal("Новая зарплата: ");

            _repository.Update(new Employee
            {
                Id = id,
                FullName = fullName,
                Position = position,
                Salary = salary
            });

            Console.WriteLine("Запись обновлена.");
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Ошибка SQL: {ex.Message}");
        }
    }

    // ---------------- Удалить ----------------
    private static void Delete()
    {
        try
        {
            int id = ReadInt("Введите Id записи для удаления: ");

            if (!_repository.Exists(id))
            {
                Console.WriteLine($"Запись с Id={id} не найдена.");
                return;
            }

            Console.Write($"Удалить запись с Id={id}? (y/n): ");
            string? answer = Console.ReadLine()?.Trim().ToLower();
            if (answer != "y")
            {
                Console.WriteLine("Удаление отменено.");
                return;
            }

            _repository.Delete(id);
            Console.WriteLine("Запись удалена.");
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Ошибка SQL: {ex.Message}");
        }
    }

    // ---------------- Вспомогательные методы ввода ----------------
    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int value))
                return value;

            Console.WriteLine("Некорректный ввод. Введите целое число.");
        }
    }

    private static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (decimal.TryParse(input, out decimal value))
                return value;

            Console.WriteLine("Некорректный ввод. Введите число.");
        }
    }

    private static string ReadNonEmptyString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.WriteLine("Строка не может быть пустой.");
        }
    }
}
