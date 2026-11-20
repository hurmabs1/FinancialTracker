using FinancialTracker.Domain.Interfaces;
using System.Text;

namespace FinancialTracker.Storage
{
    public class CsvFileRepository<T> : IRepository<T> where T : class, IEntity

    {
        private readonly string _filePath;
        private readonly Func<T, string> _serializer;
        private readonly Func<string, T?> _deserializer;
        private readonly string _header;

        private readonly List<T> _items = new List<T>();

        public CsvFileRepository(string filePath, Func<T, string> serializer, Func<string, T?> deserializer, string header)
        {
            _filePath = filePath;
            _serializer = serializer;
            _deserializer = deserializer;
            _header = header;

            LoadFromFile();

        }



        public IReadOnlyList<T> GetAll()
        {
            return _items.AsReadOnly();
        }

        public T? GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }
        public T Add(T entity)
        {
            _items.Add(entity);
            SaveToFile();
            return entity;
        }

        public bool Delete(int id)
        {
            var itemToDelete = GetById(id);
            if (itemToDelete == null)
            {
                return false;
            }

            _items.Remove(itemToDelete);
            SaveToFile();

            return true;
        }



        private void LoadFromFile()
        {
            if (!File.Exists(_filePath)) return;

            using var reader = new StreamReader(_filePath);
            string? line;
            bool isFirstLine = true;
            while ((line = reader.ReadLine()) != null)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }
                if (string.IsNullOrEmpty(line)) continue;
                var entity = _deserializer(line);
                if (entity != null) _items.Add(entity);
            }
        }

        private void SaveToFile()
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using var writer = new StreamWriter(_filePath, false, new UTF8Encoding(true));
            writer.WriteLine(_header);
            foreach(var item in _items)
            {
                writer.WriteLine(_serializer(item));
            }
        }
    }
}
