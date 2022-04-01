using GameLobbySignalRTemplate.Server.Models.Collections;
using GameLobbySignalRTemplate.Server.Models.Database;
using GameLobbySignalRTemplate.Server.Models.Redis;
using GameLobbySignalRTemplate.Server.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Database Services
builder.Services.Configure<GameDatabaseSettings>(
    builder.Configuration.GetSection("GameDatabase"));
builder.Services.Configure<RedisCacheSettings>(
    builder.Configuration.GetSection("RedisCache"));
builder.Services.Configure<CollectionsSettings>(
    builder.Configuration.GetSection("Collections"));
builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddSingleton<RedisCacheService>();
builder.Services.AddSingleton<CollectionService>();

// Other Services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<AliasService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
