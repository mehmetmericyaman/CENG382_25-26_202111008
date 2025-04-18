using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Ceng382LabWeek5.Models;

namespace Ceng382LabWeek5.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public required string Username { get; set; }
        [BindProperty]
        public required string Password { get; set; }
        public required string ErrorMessage { get; set; }

        public IActionResult OnPost()
{
    // 1. Admin kontrolü
    if (Username == "admin" && Password == "admin")
    {
        var token = Guid.NewGuid().ToString();
        var sessionId = Guid.NewGuid().ToString();

        HttpContext.Session.SetString("username", Username);
        HttpContext.Session.SetString("token", token);
        HttpContext.Session.SetString("session_id", sessionId);

        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddMinutes(30),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };

        Response.Cookies.Append("username", Username, cookieOptions);
        Response.Cookies.Append("token", token, cookieOptions);
        Response.Cookies.Append("session_id", sessionId, cookieOptions);

        return RedirectToPage("/Index");
    }

    // 2. Diğer kullanıcılar için JSON'dan kontrol
    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");
    var jsonData = System.IO.File.ReadAllText(filePath);
    var users = JsonSerializer.Deserialize<List<User>>(jsonData);

    var user = users.FirstOrDefault(u => u.Username == Username && u.Password == Password && u.IsActive);

    if (user != null)
    {
        var token = Guid.NewGuid().ToString();
        var sessionId = Guid.NewGuid().ToString();

        HttpContext.Session.SetString("username", Username);
        HttpContext.Session.SetString("token", token);
        HttpContext.Session.SetString("session_id", sessionId);

        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddMinutes(30),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };

        Response.Cookies.Append("username", Username, cookieOptions);
        Response.Cookies.Append("token", token, cookieOptions);
        Response.Cookies.Append("session_id", sessionId, cookieOptions);

        return RedirectToPage("/Index");
    }

    // 3. Hatalı giriş
    ErrorMessage = "Geçersiz kullanıcı adı veya şifre.";
    return Page();
}

    }
}
