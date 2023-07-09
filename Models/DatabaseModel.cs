using Npgsql;

namespace Models {
  class DatabaseModel {
    private static readonly string? host = Environment.GetEnvironmentVariable("DATABASE_HOST");
    private static readonly string? user = Environment.GetEnvironmentVariable("POSTGRES_USER");
    private static readonly string? pass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

    private static readonly string connectionStr = (
      $"Host={host};Username={user};Password={pass};Database={user}");

    private static readonly NpgsqlDataSource DataSource = new NpgsqlDataSourceBuilder(connectionStr).Build();


    public static void CreateTable() {
      // Get the connection and create the SQL command.
      NpgsqlConnection conn = GetConnection();
      NpgsqlCommand command = conn.CreateCommand();
      command.CommandText = @"CREATE TABLE IF NOT EXISTS cat (
        id uuid NOT NULL,
        name varchar(20) NOT NULL,
        color varchar(20) NOT NULL
      );";

      // Execute the command.
      command.ExecuteNonQuery();
    }

    public static NpgsqlConnection GetConnection() {
      // Build and create the connection.
      NpgsqlConnection connection = DataSource.OpenConnection();
      return connection;
    }
  }
}
