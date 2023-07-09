// Libs
using Npgsql;
using System.IO;

// Classes
namespace Models {
  public class CatModel {
    public string? Id { get; set; }
    public string Name { set; get; }
    public string Color { set; get; }

    public CatModel(string id, string name, string color) {
      Id = id;
      Name = name;
      Color = color;
    }

    /// <summary>
    /// A method to get some cat using his Id.
    /// </summary>
    /// <returns>
    /// Return the founded cat. Otherwise throw Exception.
    /// </returns>
    public static CatModel Get(string uuid) {
      // Get the connection and create the sql command.
      NpgsqlConnection conn = DatabaseModel.GetConnection();
      NpgsqlCommand command = conn.CreateCommand();
      command.CommandText = $"SELECT * FROM cat WHERE id='{uuid}'";

      // Execute the command and read the results.
      using var reader = command.ExecuteReader();
      CatModel? cat = null;
      while (reader.Read()) {
        cat = new(reader.GetGuid(0).ToString(), reader.GetString(1), reader.GetString(2));
      }
      conn.Close();

      // Return the result to the client.
      if (cat == null) throw new Exception("The cat id was not found.");

      return cat;
    }

    /// <summary>
    /// A method to create a new cat.
    /// </summary>
    /// <returns>
    /// Returns the new cat.
    /// </returns>
    public static CatModel Create(CatRequest cat) {
      // Create uuids until find a valid one.
      string? id = null;
      while (id == null) {
        Guid uuid = Guid.NewGuid();
        try {
          Get(uuid.ToString());
        } catch {
          id = uuid.ToString();
        }
      }


      // Get the connection and create the sql command.
      NpgsqlConnection conn = DatabaseModel.GetConnection();
      NpgsqlCommand command = conn.CreateCommand();
      command.CommandText = (
        "INSERT INTO cat (id, name, color) "
        + $"VALUES('{id}', '{cat.Name}', '{cat.Color}')"
      );

      // Execute the command and return the result.
      int affectedRows = command.ExecuteNonQuery();
      conn.Close();

      if (affectedRows == -1) throw new Exception("Cat not created.");
      return Get(id);
    }

    /// <summary>
    /// A method to update a cat.
    /// </summary>
    /// <returns>
    /// Returns the new cat.
    /// </returns>
    public static CatModel Update(string id, CatRequest reqCat) {
      // Get the connection and create the sql command.
      NpgsqlConnection conn = DatabaseModel.GetConnection();
      NpgsqlCommand command = conn.CreateCommand();
      command.CommandText = (
        $"UPDATE cat SET name='{reqCat.Name}', color='{reqCat.Color}' "
        + $"WHERE id='{id}'"
      );

      // Execute the command and return the result.
      int affectedRows = command.ExecuteNonQuery();
      conn.Close();

      if (affectedRows == -1) throw new Exception("The cat id was not found.");
      return Get(id);
    }

    /// <summary>
    /// A method to delete some cat using his Id.
    /// </summary>
    public static void Delete(string uuid) {
      // // Try to get the cat.
      // _ = Get(uuid);

      // Get the connection and create the sql command.
      NpgsqlConnection conn = DatabaseModel.GetConnection();
      NpgsqlCommand command = conn.CreateCommand();
      command.CommandText = $"DELETE FROM cat WHERE id='{uuid}'";

      // Execute the command and read the results.
      int affectedRows = command.ExecuteNonQuery();
      if (affectedRows == 0) throw new Exception("The cat id was not found.");
      conn.Close();
    }
  }
}