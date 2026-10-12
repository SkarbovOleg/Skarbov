# Учёт сотрудников — SkarbovConsoleApp

**Студент:** Скарбов

Информационная система для учёта сотрудников организации (ADO.NET + MS SQL Server).

## Состав проекта

```
Скарбов/
├── database.sql
├── SkarbovConsoleApp/
│   ├── SkarbovConsoleApp.csproj
│   ├── Program.cs
│   ├── Employee.cs
│   └── EmployeeRepository.cs
└── README.md
```

## Требования

- СУБД: Microsoft SQL Server 2019 (или выше)
- Сервер по умолчанию: localhost (при необходимости замените на localhost\SQLEXPRESS)
- .NET: 8.0
- Пакет NuGet: Microsoft.Data.SqlClient 5.2.2

## Развёртывание базы данных

1. Откройте SSMS и подключитесь к своему SQL Server.
2. Откройте файл database.sql.
3. Выполните скрипт (клавиша F5).
   Будут созданы база данных TestWpfDb, таблица dbo.Employees и 5 тестовых записей.

## Настройка строки подключения

Строка подключения находится в начале файла Program.cs:

```
Server=localhost;Database=TestWpfDb;Trusted_Connection=True;TrustServerCertificate=True;
```

Что нужно поменять на своём компьютере:

- Server — имя вашего SQL Server (например, localhost\SQLEXPRESS).
- Database — имя базы данных (TestWpfDb).
- Trusted_Connection=True — Windows-аутентификация.
- Для SQL-аутентификации используйте: User Id=sa;Password=ваш_пароль;

Пример:

```
Server=localhost;Database=TestWpfDb;User Id=sa;Password=ВашПароль;TrustServerCertificate=True;
```

## Запуск приложения

```
cd SkarbovConsoleApp
dotnet restore
dotnet run
```

## Функциональность

- 1 — показать все записи (выровненная таблица ID / ФИО / Должность / Зарплата)
- 2 — добавить запись
- 3 — обновить запись (с проверкой существования Id)
- 4 — удалить запись (с подтверждением y/n)
- 0 — выход

Все SQL-запросы параметризованные. Ошибки SqlException обрабатываются с выводом понятного сообщения.

## Скриншоты работы

Вставьте сюда свои скриншоты:

- screenshots/menu.png — главное меню
- screenshots/list.png — список сотрудников
- screenshots/add.png — добавление записи
