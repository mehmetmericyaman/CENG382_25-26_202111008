using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ceng382LabWeek5.Models;
using System.Collections.Generic;
using System.Linq;

namespace Ceng382LabWeek5.Pages
{
    public class IndexModel : PageModel
    {
        private static List<ClassInformationModel> ClassList = new(); // In-memory liste

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        public List<ClassInformationModel> Classes => ClassList;

        public void OnGet() { }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Yeni sınıfa otomatik ID ver
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
    }
}
