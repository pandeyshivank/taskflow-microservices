using Microsoft.EntityFrameworkCore;
using UserMicroService.AutoMapper;
using UserMicroService.Data;
using UserMicroService.Services.Implementations;
using UserMicroService.Services.Interfaces;
using UserMicroService.UserRepositories;
using UserMicroService.UserRepositories.Implementations;
using UserMicroService.UserRepositories.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<UserDbContext>
    (
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("UserDBConnectionString")
    ));
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
