var builder = WebApplication.CreateBuilder(args);




builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "https://localhost:7180/swagger/v1/swagger.json",
            "User API");

        options.SwaggerEndpoint(
            "https://localhost:7068/swagger/v1/swagger.json",
            "Task API");

        options.SwaggerEndpoint(
            "https://localhost:7019/swagger/v1/swagger.json",
            "Notification API");
    });
}

app.UseHttpsRedirection();
app.MapReverseProxy();

app.UseAuthorization();


app.Run();
