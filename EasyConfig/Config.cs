using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

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
            if (File.Exists(path))
            {
                var json = string.Empty;
                var stream  = File.OpenRead(path);

                json = await JsonSerializer.DeserializeAsync<string>(stream);

                if (!string.IsNullOrEmpty(encryptionKey))
                {
                    json = Encryption.Decrypt(json!, encryptionKey);
                }

                T? config = JsonSerializer.Deserialize<T>(json!);

                if (config is not null && validate is not null && !validate(config))
                {
                    throw new ArgumentException("Invalid config");
                }

                return config;
            }

            return default;
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
            var json = string.Empty;

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            if (!string.IsNullOrEmpty(encryptionKey))
            {
                json = Encryption.Encrypt(JsonSerializer.Serialize(config, options), encryptionKey);
            }
            else
            {
                json = JsonSerializer.Serialize(config, options);
            }

            File.WriteAllText(path, json);
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
            var json = string.Empty;

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            if (!string.IsNullOrEmpty(encryptionKey))
            {
                json = Encryption.Encrypt(JsonSerializer.Serialize(config, options), encryptionKey);
            }
            else
            {
                json = JsonSerializer.Serialize(config, options);
            }

            await File.WriteAllTextAsync(path, json);
        }
    }
}
