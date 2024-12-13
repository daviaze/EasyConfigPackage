using System.Text.Json;

namespace EasyConfig
{
    public static class Config<T> where T : class
    {
        /// <summary>
        /// Read a file config
        /// </summary>
        /// <param name="path"></param>
        /// <param name="validate"></param>
        /// <returns>Return a config or null</returns>
        public static T? Read(string path, Predicate<T>? validate = null)
        {
            try
            {
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    T? config = JsonSerializer.Deserialize<T>(json);

                    if (config is not null && validate is not null && !validate(config))
                    {
                        throw new ArgumentException("Invalid config");
                    }

                    return config;
                }

                return default;
            }catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save a config
        /// </summary>
        /// <param name="path"></param>
        /// <param name="config"></param>
        public static void Save(string path, T config)
        {
            try
            {
                var json = JsonSerializer.Serialize(config);
                File.WriteAllText(path, json);
            }catch
            {
                throw;
            }
        }
    }
}
