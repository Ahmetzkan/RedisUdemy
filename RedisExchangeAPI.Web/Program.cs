using RedisExchangeAPI.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<RedisService>();
builder.Services.AddControllersWithViews();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

var redisService = app.Services.GetRequiredService<RedisService>();
redisService.Connect();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HashType}/{action=Index}/{id?}");

app.Run();
