using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
builder.Services.AddSwaggerGen(op=>
{

    op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "Paste only the JWT token. Do NOT include the 'Bearer' prefix.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"

    });
    op.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
        new OpenApiSecurityScheme()
        {
             Reference= new OpenApiReference()
          {
              Id="Bearer",
              Type=ReferenceType.SecurityScheme,
          },
          Scheme="bearer",
          Name ="bearer",
          In=ParameterLocation.Header,
        },
        new List<string>()
        }

    });
    });
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(op => {
    op.SaveToken = true;
    op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
         ValidateIssuerSigningKey=true,
         ValidateAudience=false,
         ValidateIssuer=false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtTokenKey")))

    };
    });
  
builder.Services.AddDbContext<TaskDbContext>(
    option=> option.UseSqlServer(builder.Configuration.GetConnectionString("TaskDBConnectionString")
    ));

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();
