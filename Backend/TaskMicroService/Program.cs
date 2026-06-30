using Microsoft.EntityFrameworkCore;
using TaskMicroService.AutoMapper;
using TaskMicroService.Data;
using TaskMicroService.Services.Implementations;
using TaskMicroService.Services.Interfaces;
using TaskMicroService.TaskRepositories.Implementations;
using TaskMicroService.TaskRepositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(TaskMappingConfig));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TaskDbContext>(
    option=> option.UseSqlServer(builder.Configuration.GetConnectionString("TaskDBConnectionString")
    ));

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
