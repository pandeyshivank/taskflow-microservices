using Microsoft.EntityFrameworkCore;
using NotificationMicroService.AutoMapper;
using NotificationMicroService.Data;
using NotificationMicroService.Entities;
using NotificationMicroService.Repositories.Implementations;
using NotificationMicroService.Repositories.Interfaces;
using NotificationMicroService.Services.Implementations;
using NotificationMicroService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<NotificationDbContext>(

    option => option.UseSqlServer(builder.Configuration.GetConnectionString("NotificationDbContext"))
    );
builder.Services.AddAutoMapper(typeof(NotificationAutoMapper));
builder.Services.AddScoped<INotificationRepository,NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
