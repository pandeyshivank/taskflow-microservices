using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(option=>
    {
        option.SaveToken = true;
        option.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtTokenKey"))),
            ValidateIssuer = false,
            ValidateAudience = false
        };
     
    });

    builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<UserDbContext>
    (
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("UserDBConnectionString")
    ));
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description= "Enter: Bearer {your JWT token}",
        Name="Authorization",
        In= ParameterLocation.Header,
        Scheme="Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement()

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
          In=ParameterLocation.Header
       },
       new List<string>()
    }

});


});


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
