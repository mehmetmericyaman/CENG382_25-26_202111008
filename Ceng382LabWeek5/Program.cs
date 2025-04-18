var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// **Session hizmetini ekleyelim**
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Oturum süresi
    options.Cookie.HttpOnly = true;  // Çerez sadece HTTP üzerinden erişilebilir olacak
    options.Cookie.IsEssential = true;  // Çerezin zorunlu olduğunu belirtiriz
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// **Session kullanımı için middleware ekleyelim**
app.UseSession(); // Bu satır önemli, session middleware'ini burada ekliyoruz

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
