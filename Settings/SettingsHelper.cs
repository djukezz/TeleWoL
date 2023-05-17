using System.Text;
using TeleWoL.Common;

namespace TeleWoL.Settings
{
    internal class SettingsWrapper<T>
        where T : IReadable, IWriteable, new()
    {
        private readonly object _lock = new object();
        private readonly string _filePath;

        public SettingsWrapper(string filePath)
        {
            using Mutex mutex = new Mutex(true, _filePath);
            Settings = new T();
            try
            {
                using BinaryReader br = new BinaryReader(File.OpenRead(filePath), Encoding.UTF8, false);
                Settings.Read(br);
            }
            catch (Exception)
            {
                Settings = new T();
            }
            finally
            {
                mutex.ReleaseMutex();
            }
            _filePath = filePath;
        }

        public void Save()
        {
            using Mutex mutex = new Mutex(false, _filePath);
            try
            {
                mutex.WaitOne();
                using BinaryWriter bw = new BinaryWriter(File.OpenWrite(_filePath), Encoding.UTF8, false);
                Settings.Write(bw);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        public T Settings { get; }
    }
}
