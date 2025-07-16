using IntricomMVC.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IntricomMVC.Services
{
    public static class FileSystemRepository
    {
        public static void SaveEntityToFileSystem<T>(T entity, string basePath) where T : class
        {
            // Nombre de la carpeta
            string entityName = typeof(T).Name;
            // Ruta completa de la carpeta de la entidad
            string entityFolder = Path.Combine(basePath, entityName);

            // Existe?
            if (!Directory.Exists(entityFolder))
                Directory.CreateDirectory(entityFolder);

            // Ruta del archivo de metadatos
            string metadataPath = Path.Combine(entityFolder, "_metadata");
            Metadata metadata;
            if (File.Exists(metadataPath))
            {
                string metadataJson = File.ReadAllText(metadataPath);
                metadata = JsonSerializer.Deserialize<Metadata>(metadataJson);
            }
            else
            {
                metadata = new Metadata { TOTAL_REGISTRIES = 0, LAST_INDEX = 0 };
            }

            // Calcular nuevo ID
            int newId = metadata.LAST_INDEX + 1;
            var idProp = typeof(T).GetProperty("Id");
            if (idProp != null && idProp.CanWrite) idProp.SetValue(entity, newId);

            // Guardar el archivo del registro
            string filePath = Path.Combine(entityFolder, $"{newId}.json");
            string json = JsonSerializer.Serialize(entity, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);

            // Actualizar metadata
            metadata.LAST_INDEX = newId;
            metadata.TOTAL_REGISTRIES += 1;
            string updatedMetadata = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(metadataPath, updatedMetadata);

            //Console.WriteLine($"[{entityName}] Guardado con ID = {newId}");
        }
    }
}
