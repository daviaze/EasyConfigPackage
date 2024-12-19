using System.Security.Cryptography;
using System.Text.Json;

namespace EasyConfig
{
    public static class Config<T> where T : class
    {
        /// <summary>
        /// Read a file config, if encryption key is not null, it will be decrypted.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="validate"></param>
        /// <param name="encryptionKey"></param>
        /// <returns>Return a config or null</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="JsonException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="CryptographicException"></exception>
        public static T? Read(string path, Predicate<T>? validate = null, string? encryptionKey = null)
        {
            try
            {
                if (File.Exists(path))
                {
                    var json = string.Empty;
                    if (!string.IsNullOrEmpty(encryptionKey))
                    {
                        json = Encryption.Decrypt(File.ReadAllText(path), encryptionKey);
                    }
                    else
                    {
                        json = File.ReadAllText(path);
                    }

                    T? config = JsonSerializer.Deserialize<T>(json);

                    if (config is not null && validate is not null && !validate(config))
                    {
                        throw new ArgumentException("Invalid config");
                    }

                    return config;
                }

                return default;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously read a file config, if encryption key is not null, it will be decrypted.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="validate"></param>
        /// <param name="encryptionKey"></param>
        /// <returns>Return a config or null</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="JsonException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="CryptographicException"></exception>
        public static async Task<T?> ReadAsync(string path, Predicate<T>? validate = null, string? encryptionKey = null)
        {
            try
            {
                if (File.Exists(path))
                {
                    var json = string.Empty;
                    if (!string.IsNullOrEmpty(encryptionKey))
                    {
                        json = Encryption.Decrypt(await File.ReadAllTextAsync(path), encryptionKey);
                    }
                    else
                    {
                        json = await File.ReadAllTextAsync(path);
                    }

                    T? config = JsonSerializer.Deserialize<T>(json);

                    if (config is not null && validate is not null && !validate(config))
                    {
                        throw new ArgumentException("Invalid config");
                    }

                    return config;
                }

                return default;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save a config, if encryption key is not null, it will be encrypted.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="config"></param>
        /// <param name="encryptionKey"></param>
        /// <returns>Return void</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="JsonException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="CryptographicException"></exception>
        public static void Save(string path, T config, string? encryptionKey = null)
        {
            try
            {
                var json = string.Empty;

                if (!string.IsNullOrEmpty(encryptionKey))
                {
                    json = Encryption.Encrypt(JsonSerializer.Serialize(config), encryptionKey);
                }
                else
                {
                    json = JsonSerializer.Serialize(config);
                }

                File.WriteAllText(path, json);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously save a config, if encryption key is not null, it will be encrypted.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="config"></param>
        /// <param name="encryptionKey"></param>
        /// <returns>Return void</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="JsonException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="CryptographicException"></exception>
        public static async Task SaveAsync(string path, T config, string? encryptionKey = null)
        {
            try
            {
                var json = string.Empty;

                if (!string.IsNullOrEmpty(encryptionKey))
                {
                    json = Encryption.Encrypt(JsonSerializer.Serialize(config), encryptionKey);
                }
                else
                {
                    json = JsonSerializer.Serialize(config);
                }

                await File.WriteAllTextAsync(path, json);
            }
            catch
            {
                throw;
            }
        }
    }
}
