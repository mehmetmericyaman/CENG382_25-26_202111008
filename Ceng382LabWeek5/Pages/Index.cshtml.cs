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
        public required string ClassName { get; set; }
        public int StudentCount { get; set; }
        public required string Description { get; set; }
        public int Id { get; set; } // işlem butonları için kullanılacak
    }

    public class IndexModel : PageModel
    {
        private static List<ClassInformationModel> ClassList = new();

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public required string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public required List<ClassInformationTable> FilteredClasses { get; set; }

        public void OnGet()
{
    // Kullanıcı oturum kontrolü
    var username = HttpContext.Session.GetString("username");
    var token = HttpContext.Session.GetString("token");
    var sessionId = HttpContext.Session.GetString("session_id");

    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(sessionId))
    {
        Response.Redirect("/Login");
        return;
    }

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

    // Eğer FilteredClasses null ise boş liste başlatıyoruz
    if (FilteredClasses == null)
    {
        FilteredClasses = new List<ClassInformationTable>();
    }
}

        public IActionResult OnPostAdd()
{
    if (!ModelState.IsValid)
        return Page();

    // Yeni sınıf eklerken Id'yi kontrol et
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

      public IActionResult OnPostLogout()
{
    // Session'ı temizle
    HttpContext.Session.Clear();

    // Login ile ilgili cookie'leri sil
    Response.Cookies.Delete("username");
    Response.Cookies.Delete("token");
    Response.Cookies.Delete("session_id");

    // Giriş sayfasına yönlendir
    return RedirectToPage("/Login");
}


    }
    
    
}
