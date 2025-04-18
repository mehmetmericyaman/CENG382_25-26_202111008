using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Reflection;

namespace Ceng382LabWeek5.Helpers
{
    public sealed class Utils
    {
        private static readonly Lazy<Utils> lazy = new(() => new Utils());
        public static Utils Instance => lazy.Value;

        private Utils() { }

        // Generic JSON export method
        public string ExportToJson<T>(List<T> data, List<string>? selectedColumns = null)
        {
            if (selectedColumns == null || selectedColumns.Count == 0)
            {
                return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            }

            // Sadece seçilen kolonları içeren dictionary listesi oluştur
            var filteredData = data.Select(item =>
            {
                var dict = new Dictionary<string, object>();
                foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (selectedColumns.Contains(prop.Name))
                    {
                        dict[prop.Name] = prop.GetValue(item);
                    }
                }
                return dict;
            }).ToList();

            return JsonSerializer.Serialize(filteredData, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
