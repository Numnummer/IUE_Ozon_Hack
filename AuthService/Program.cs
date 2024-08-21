using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthService.Data;
using AuthService.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthServiceDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthServiceDbContextConnection' not found.");

builder.Services.AddDbContext<AuthServiceDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<AuthServiceUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthServiceDbContext>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
