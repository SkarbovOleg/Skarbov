using Microsoft.Data.SqlClient;

namespace SkarbovConsoleApp;

/// <summary>
/// Репозиторий для работы с таблицей Employees через ADO.NET.
/// </summary>
public class EmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>Получить все записи.</summary>
    public List<Employee> GetAll()
    {
        var result = new List<Employee>();
        const string sql = "SELECT Id, FullName, Position, Salary FROM dbo.Employees ORDER BY Id;";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new Employee
            {
                Id = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Position = reader.GetString(2),
                Salary = reader.GetDecimal(3)
            });
        }
        return result;
    }

    /// <summary>Добавить запись.</summary>
    public void Insert(Employee employee)
    {
        const string sql = @"INSERT INTO dbo.Employees (FullName, Position, Salary)
                             VALUES (@FullName, @Position, @Salary);";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@FullName", employee.FullName);
        command.Parameters.AddWithValue("@Position", employee.Position);
        command.Parameters.AddWithValue("@Salary", employee.Salary);

        connection.Open();
        command.ExecuteNonQuery();
    }

    /// <summary>Обновить запись по Id.</summary>
    public bool Update(Employee employee)
    {
        const string sql = @"UPDATE dbo.Employees
                             SET FullName = @FullName,
                                 Position = @Position,
                                 Salary   = @Salary
                             WHERE Id = @Id;";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Id", employee.Id);
        command.Parameters.AddWithValue("@FullName", employee.FullName);
        command.Parameters.AddWithValue("@Position", employee.Position);
        command.Parameters.AddWithValue("@Salary", employee.Salary);

        connection.Open();
        return command.ExecuteNonQuery() > 0;
    }

    /// <summary>Удалить запись по Id.</summary>
    public bool Delete(int id)
    {
        const string sql = "DELETE FROM dbo.Employees WHERE Id = @Id;";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        return command.ExecuteNonQuery() > 0;
    }

    /// <summary>Проверить существование записи по Id.</summary>
    public bool Exists(int id)
    {
        const string sql = "SELECT COUNT(1) FROM dbo.Employees WHERE Id = @Id;";

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        return (int)command.ExecuteScalar() > 0;
    }
}
