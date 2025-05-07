using Microsoft.EntityFrameworkCore;
using Ceng382LabWeek5.Data; // DbContext için gerekli

var builder = WebApplication.CreateBuilder(args);

// Bağlantı cümlesi üzerinden DbContext'i ekliyoruz
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDbConnection")));

// Razor Pages servisi
builder.Services.AddRazorPages();

// Session hizmetini ekliyoruz
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Hata ayıklama ve HSTS yapılandırması
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Static dosyalar (wwwroot) için
app.UseStaticFiles();

// Routing ve session middleware
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Razor Pages route tanımı
app.MapRazorPages();

app.Run();
