-- ============================================================
-- Скрипт создания базы данных TestWpfDb и таблицы Employees
-- СУБД: MS SQL Server
-- Студент: Скарбов
-- Запускать в SSMS за один раз
-- ============================================================

-- 1. Создание базы данных (если не существует)
IF DB_ID(N'TestWpfDb') IS NULL
BEGIN
    CREATE DATABASE TestWpfDb;
END
GO

USE TestWpfDb;
GO

-- 2. Создание таблицы Employees
IF OBJECT_ID(N'dbo.Employees', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Employees
    (
        Id        INT             IDENTITY(1,1) NOT NULL,
        FullName  NVARCHAR(100)   NOT NULL,
        Position  NVARCHAR(100)   NOT NULL,
        Salary    DECIMAL(10, 2)  NOT NULL,
        CONSTRAINT PK_Employees PRIMARY KEY (Id)
    );
END
GO

-- 3. Заполнение тестовыми данными
IF NOT EXISTS (SELECT 1 FROM dbo.Employees)
BEGIN
    INSERT INTO dbo.Employees (FullName, Position, Salary) VALUES
        (N'Иванов Иван Иванович',      N'Программист',              120000.00),
        (N'Петрова Анна Сергеевна',    N'Менеджер',                  95000.50),
        (N'Сидоров Пётр Алексеевич',   N'Тестировщик',               80000.00),
        (N'Кузнецова Ольга Ивановна',  N'Бухгалтер',                 70000.75),
        (N'Смирнов Дмитрий Олегович',  N'Системный администратор',  105000.00);
END
GO
