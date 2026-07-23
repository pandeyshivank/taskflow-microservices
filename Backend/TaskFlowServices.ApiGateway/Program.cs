var builder = WebApplication.CreateBuilder(args);




builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option=>option.AddPolicy("MyDefultpolicy", policy =>
{

    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
   
}

app.UseHttpsRedirection();
app.MapReverseProxy();
app.UseCors("MyDefultpolicy");
app.UseAuthorization();


app.Run();
