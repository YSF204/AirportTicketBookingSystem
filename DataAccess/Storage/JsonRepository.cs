namespace AirportTicketBookingSystem.DataAccess.Storage;
    public class JsonRepository<T>
    {

        private readonly string filePath;
        public JsonRepository(string fileName)
        {
            filePath = Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory, "..", "..", "..", fileName));
        }
        public string ReadJson()
        {
            EnsureFileExists();
            return File.ReadAllText(filePath);
        }
        public List<T> GetItems()
        {
            string json = ReadJson();
            return System.Text.Json.JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
        public void SaveItems(List<T> items)
        {
            EnsureFileExists();

            string serializedJson = System.Text.Json.JsonSerializer.Serialize(items,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, serializedJson);
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
        }

    }