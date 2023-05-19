using Newtonsoft.Json;
using System.Text;

namespace TeleWoL.Settings
{
    internal class SettingsWrapper<T> : IDisposable, ISettingsSaver
        where T : class, new()
    {
        private readonly string _filePath;
        private readonly JsonSerializer _serializer;
        private Mutex _mutex;

        public SettingsWrapper(string filePath)
        {
            _filePath = filePath;
            Settings = new T();
            _mutex = new Mutex(false, _filePath);
            _serializer = new JsonSerializer
            {
                Culture = System.Globalization.CultureInfo.InvariantCulture,
                Formatting = Formatting.Indented,
            };

            if (!File.Exists(_filePath))
            {
                Save();
                return;
            }

            Update();
        }

        private void Update()
        {
            try
            {
                _mutex.WaitOne();
                using var fs = File.OpenRead(_filePath);
                using var sr = new StreamReader(fs, Encoding.UTF8);
                Settings = _serializer.Deserialize<T>(new JsonTextReader(sr)) ?? new T();
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public void Save()
        {
            using Mutex mutex = new Mutex(false, _filePath);
            try
            {
                _mutex.WaitOne();
                using var fs = File.OpenWrite(_filePath);
                using var sw = new StreamWriter(fs, Encoding.UTF8);
                _serializer.Serialize(new JsonTextWriter(sw), Settings);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public T Settings { get; private set; }

        public void Dispose()
        {
            _mutex.Dispose();
        }
    }
}
