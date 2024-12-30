WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

const string corsPolicy = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policy =>
    {
        policy.WithOrigins("https://localhost:44428") // Updated to HTTPS
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment()) app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(corsPolicy);

app.MapControllers();
app.MapFallbackToFile("index.html");

await app.RunAsync();