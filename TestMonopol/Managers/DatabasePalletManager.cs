using Npgsql;
using TestMonopol.Managers;
using TestMonopol.Models;

public class DatabasePalletManager : LogicManager
{
    private const string ConnectionString = "Host=localhost;Username=postgres;Password=1234;Database=monopol_test";

    public override List<Pallet> ProcessPallets()
    {
        return LoadPalletsFromDatabase();
    }

    // Метод для загрузки паллет из базы данных
    private List<Pallet> LoadPalletsFromDatabase()
    {
        var pallets = new List<Pallet>();

        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("SELECT * FROM pallets", connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var palletId = reader.GetString(reader.GetOrdinal("pallet_id"));
                    var width = reader.GetDouble(reader.GetOrdinal("width"));
                    var height = reader.GetDouble(reader.GetOrdinal("height"));
                    var depth = reader.GetDouble(reader.GetOrdinal("depth"));

                    var pallet = new Pallet(palletId, width, height, depth);

                    var boxCommand = new NpgsqlCommand("SELECT * FROM boxes WHERE pallet_id = @pallet_id", connection);
                    boxCommand.Parameters.AddWithValue("pallet_id", palletId);

                    using (var boxReader = boxCommand.ExecuteReader())
                    {
                        while (boxReader.Read())
                        {
                            var boxId = boxReader.GetString(boxReader.GetOrdinal("box_id"));
                            var boxWidth = boxReader.GetDouble(boxReader.GetOrdinal("width"));
                            var boxHeight = boxReader.GetDouble(boxReader.GetOrdinal("height"));
                            var boxDepth = boxReader.GetDouble(boxReader.GetOrdinal("depth"));
                            var weight = boxReader.GetDouble(boxReader.GetOrdinal("weight"));
                            DateTime? productionDate = boxReader.IsDBNull(boxReader.GetOrdinal("production_date"))
                                ? (DateTime?)null
                                : boxReader.GetDateTime(boxReader.GetOrdinal("production_date"));
                            DateTime? expirationDate = boxReader.IsDBNull(boxReader.GetOrdinal("expiration_date"))
                                ? (DateTime?)null
                                : boxReader.GetDateTime(boxReader.GetOrdinal("expiration_date"));

                            Box box = productionDate.HasValue
                                ? new Box(boxId, boxWidth, boxHeight, boxDepth, weight, productionDate: productionDate.Value)
                                : new Box(boxId, boxWidth, boxHeight, boxDepth, weight, expirationDate: expirationDate.Value);

                            pallet.AddBox(box);
                        }
                    }

                    pallets.Add(pallet);
                }
            }
            connection.Close();
        }
        return pallets;
    }

    // Метод для добавления паллет и коробок в базу данных
    public void AddPalletsToDatabase(List<Pallet> pallets)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();

            foreach (var pallet in pallets)
            {
                // Добавление паллеты
                var palletCommand = new NpgsqlCommand(
                    "INSERT INTO pallets (pallet_id, width, height, depth) VALUES (@pallet_id, @width, @height, @depth) " +
                    "ON CONFLICT (pallet_id) DO NOTHING;", // Игнорировать дублирующиеся паллеты
                    connection
                );
                palletCommand.Parameters.AddWithValue("pallet_id", pallet.ID);
                palletCommand.Parameters.AddWithValue("width", pallet.Width);
                palletCommand.Parameters.AddWithValue("height", pallet.Height);
                palletCommand.Parameters.AddWithValue("depth", pallet.Depth);
                palletCommand.ExecuteNonQuery();

                // Добавление коробок, связанных с паллетой
                foreach (var box in pallet.Boxes)
                {
                    var boxCommand = new NpgsqlCommand(
                        "INSERT INTO boxes (box_id, pallet_id, width, height, depth, weight, production_date, expiration_date) " +
                        "VALUES (@box_id, @pallet_id, @width, @height, @depth, @weight, @production_date, @expiration_date) " +
                        "ON CONFLICT (box_id) DO NOTHING;", // Игнорировать дублирующиеся коробки
                        connection
                    );
                    boxCommand.Parameters.AddWithValue("box_id", box.ID);
                    boxCommand.Parameters.AddWithValue("pallet_id", pallet.ID);
                    boxCommand.Parameters.AddWithValue("width", box.Width);
                    boxCommand.Parameters.AddWithValue("height", box.Height);
                    boxCommand.Parameters.AddWithValue("depth", box.Depth);
                    boxCommand.Parameters.AddWithValue("weight", box.Weight);
                    boxCommand.Parameters.AddWithValue("production_date", (object?)box.ProductionDate ?? DBNull.Value);
                    boxCommand.Parameters.AddWithValue("expiration_date", (object?)box.ExpirationDate ?? DBNull.Value);
                    boxCommand.ExecuteNonQuery();
                }
            }
            connection.Close();

        }

        Console.WriteLine("Паллеты и коробки успешно добавлены в базу данных!");
    }
}
