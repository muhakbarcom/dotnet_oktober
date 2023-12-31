global using dotnet_oktober.Models;
global using dotnet_oktober.Services.CharacterService;
global using dotnet_oktober.Services.UserService;
global using dotnet_oktober.Services.AuthService;
global using dotnet_oktober.Dtos.Character;
global using dotnet_oktober.Dtos.User;
global using Microsoft.EntityFrameworkCore;
global using dotnet_oktober.Data;
global using dotnet_oktober.Middleware;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContex>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<iCharacterService, CharacterService>();
builder.Services.AddScoped<iUserService, UserService>();
builder.Services.AddScoped<iAuthService, AuthService>();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtMiddleware>();
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
