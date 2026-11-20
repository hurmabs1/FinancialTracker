using System.Text;

namespace FinancialTracker.Storage
{
    public class SettingsRepository
    {
        private readonly string _filePath;

        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        public SettingsRepository(string filePath)
        {
            _filePath = filePath;
            Load();
        }

        private void Load()
        {
            var info = new FileInfo(_filePath);

            if (!info.Exists || info.Length == 0)
            {
                return;
            }

            using var reader = new StreamReader(_filePath);
            string? line;
            bool isFirstLine = true;

            while ((line = reader.ReadLine()) != null)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;

                    if (line.Trim() == "Key;Value")
                    {
                        continue;
                    }
                }

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var parts = line.Split(';');
                if (parts.Length == 2)
                {
                    _settings[parts[0]] = parts[1];
                }
            }
        }


        private void Save()
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using var writer = new StreamWriter(_filePath,false,new UTF8Encoding(true));
            writer.WriteLine("Key;Value");
            foreach (var part in _settings)
            {
                writer.WriteLine($"{part.Key};{part.Value}");
            }
        }

        public int GetInt(string key, int defaultValue)
        {
            if (_settings.TryGetValue(key, out string? value) && int.TryParse(value, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        public void SetInt(string key, int value)
        {
            _settings[key] = value.ToString();
            Save(); 
        }
    }
}
