using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ceng382LabWeek5.Models;
using Ceng382LabWeek5.Helpers;
using Ceng382LabWeek5.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Ceng382LabWeek5.Pages
{
    public class ClassInformationTable
    {
        public required string ClassName { get; set; }
        public int StudentCount { get; set; }
        public required string Description { get; set; }
        public int Id { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly SchoolDbContext _context;

        public IndexModel(SchoolDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Class NewClass { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public required List<ClassInformationTable> FilteredClasses { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Oturum kontrolü
            var username = HttpContext.Session.GetString("username");
            var token = HttpContext.Session.GetString("token");
            var sessionId = HttpContext.Session.GetString("session_id");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(sessionId))
            {
                return RedirectToPage("/Login");
            }

            // Eğer veritabanı boşsa, 100 sahte kayıt ekle
            if (!_context.Classes.Any())
            {
                for (int i = 1; i <= 100; i++)
                {
                    _context.Classes.Add(new Class
                    {
                        Name = "Class " + i,
                        PersonCount = 20 + (i % 30),
                        Description = "Sample class description " + i,
                        IsActive = true
                    });
                }
                await _context.SaveChangesAsync();
            }


            // Tüm sınıfları çek
            var query = _context.Classes.AsQueryable();
            query = query.Where(c => c.IsActive == true); // sadece aktif kayıtları getir


            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(c => c.Name.ToLower().Contains(SearchName.ToLower()));
            }

            TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)PageSize);
            var paginated = await query.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();

            FilteredClasses = paginated.Select(c => new ClassInformationTable
            {
                Id = c.Id,
                ClassName = c.Name,
                StudentCount = c.PersonCount,
                Description = c.Description
            }).ToList();

            return Page();
        }

      public async Task<IActionResult> OnPostAddAsync()
        {
            Console.WriteLine("OnPostAddAsync() ÇALIŞTI!");
            Console.WriteLine($"Gelen veri => Name: {NewClass?.Name}, Count: {NewClass?.PersonCount}, Description: {NewClass?.Description}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model hatalı! Şu alanlar geçersiz:");

                foreach (var modelStateKey in ModelState.Keys)
                {
                    var value = ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($" - {modelStateKey}: {error.ErrorMessage}");
                    }
                }

                return await OnGetAsync();
            }

            NewClass.IsActive = true;
            _context.Classes.Add(NewClass);
            await _context.SaveChangesAsync();

            Console.WriteLine("Kayıt başarıyla eklendi!");
            return RedirectToPage();
        }



       public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var classToUpdate = await _context.Classes.FindAsync(id);
            if (classToUpdate != null)
            {
                classToUpdate.IsActive = false; // sadece pasif yap
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostExportJsonAsync(List<string> SelectedColumns, bool exportAll = false)
        {
            List<Class> exportData;

            var query = _context.Classes.AsQueryable();

            if (!exportAll && !string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(c => c.Name.ToLower().Contains(SearchName.ToLower()));
            }

            exportData = await query.ToListAsync();

            var json = Utils.Instance.ExportToJson(exportData, SelectedColumns);

            var fileBytes = Encoding.UTF8.GetBytes(json);
            return File(fileBytes, "application/json", "export.json");
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("username");
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("session_id");
            return RedirectToPage("/Login");
        }
    }
}
