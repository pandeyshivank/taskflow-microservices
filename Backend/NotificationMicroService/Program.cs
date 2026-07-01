using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddAuthentication(option =>
{

    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtTokenKey")))
    };

});
builder.Services.AddSwaggerGen(op =>
{
    op.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
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
            new OpenApiSecurityScheme(){

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


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();

