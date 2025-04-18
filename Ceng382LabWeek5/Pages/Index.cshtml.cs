// Pages/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ceng382LabWeek5.Models;
using System.Collections.Generic;
using System.Linq;
using Ceng382LabWeek5.Helpers; // Utils sınıfı için gerekli
using System.Text;             // FileContentResult için gerekli

namespace Ceng382LabWeek5.Pages
{
    public class ClassInformationTable
    {
        public string ClassName { get; set; }
        public int StudentCount { get; set; }
        public string Description { get; set; }
        public int Id { get; set; } // işlem butonları için kullanılacak
    }

    public class IndexModel : PageModel
    {
        private static List<ClassInformationModel> ClassList = new();

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public List<ClassInformationTable> FilteredClasses { get; set; }

        public void OnGet()
        {
            // SAHTE VERİ EKLE (tek seferlik istenirse yorum satırına al)
            if (ClassList.Count < 100)
            {
                for (int i = 1; i <= 100; i++)
                {
                    ClassList.Add(new ClassInformationModel
                    {
                        Id = i,
                        ClassName = "Class " + i,
                        StudentCount = 20 + (i % 30),
                        Description = "Sample class description " + i
                    });
                }
            }

            var query = ClassList.AsQueryable();

            // FİLTRELEME
            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(c => c.ClassName.ToLower().Contains(SearchName.ToLower()));
            }

            // SAYFALAMA
            TotalPages = (int)System.Math.Ceiling(query.Count() / (double)PageSize);
            query = query.Skip((PageNumber - 1) * PageSize).Take(PageSize);

            // VİEW MODEL'e dönüştür
            FilteredClasses = query
                .Select(c => new ClassInformationTable
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    StudentCount = c.StudentCount,
                    Description = c.Description
                }).ToList();
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
                return Page();

            NewClass.Id = ClassList.Count > 0 ? ClassList.Max(c => c.Id) + 1 : 1;
            ClassList.Add(NewClass);
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var classToRemove = ClassList.FirstOrDefault(c => c.Id == id);
            if (classToRemove != null)
            {
                ClassList.Remove(classToRemove);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostExportJson(List<string> SelectedColumns, bool exportAll = false)
        {
            List<ClassInformationModel> exportData;

            if (exportAll)
            {
                exportData = ClassList;
            }
            else
            {
                var query = ClassList.AsQueryable();

                if (!string.IsNullOrEmpty(SearchName))
                {
                    query = query.Where(c => c.ClassName.ToLower().Contains(SearchName.ToLower()));
                }

                exportData = query.ToList();
            }

            // JSON oluştur
            var json = Utils.Instance.ExportToJson(exportData, SelectedColumns);

            // Tarayıcıya indirme başlat (application/json)
            var fileBytes = Encoding.UTF8.GetBytes(json);
            return File(fileBytes, "application/json", "export.json");
        }
    }
}
