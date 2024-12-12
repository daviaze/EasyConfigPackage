# EasyConfig Library

## Descrição

A **EasyConfig** é uma biblioteca simples para leitura e salvamento de configurações localmente em arquivos JSON. Ela fornece dois métodos principais: `Read` e `Save`, que permitem gerenciar configurações de forma prática e eficiente.

---

## Funcionalidades

### 1. `Read<T>(string path, Predicate<T>? validate)`
- **Descrição**: Lê as configurações de um arquivo JSON localizado no caminho especificado.
- **Parâmetros**:
  - `path` (string): O caminho do arquivo que contém as configurações em formato JSON.
  - `validate` (opcional): Um predicado (`Predicate<T>`) que define as condições que o objeto desserializado deve atender. Se a validação falhar, uma exceção `ArgumentException` será lançada.
- **Retorno**:
  - Retorna um objeto do tipo genérico `T`, contendo os dados deserializados.
  - Caso o arquivo não exista ou o conteúdo esteja inválido, retorna `null`.
- **Restrições**:
  - O tipo `T` deve ser uma classe (`where T : class`).

### 2. `Save<T>(string path, T config)`
- **Descrição**: Salva as configurações fornecidas em um arquivo JSON no caminho especificado.
- **Parâmetros**:
  - `config` (T): O objeto que será serializado e salvo no arquivo.
  - `path` (string): O caminho onde o arquivo JSON será salvo.
- **Comportamento**:
  - Cria o arquivo JSON se ele não existir.
  - Sobrescreve o conteúdo do arquivo, se ele já existir.

---

## Exemplo de Uso

### Lendo Configurações

```csharp
// Definindo o modelo de configuração
public class AppConfig
{
    public string AppName { get; set; }
    public int Version { get; set; }
}

// Lendo configurações de um arquivo JSON
var config = ConfigManager.Read<AppConfig>("config.json");

if (config != null)
{
    Console.WriteLine($"App Name: {config.AppName}, Version: {config.Version}");
}
else
{
    Console.WriteLine("Configuração não encontrada ou inválida.");
}
