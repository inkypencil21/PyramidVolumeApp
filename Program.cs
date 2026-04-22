var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ✅ Correct way to serve static files in .NET 8
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// ✅ Map Razor Pages (no WithStaticAssets)
app.MapRazorPages();

app.Run();
