using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HNWebService", Version = "v1" });
});
// Add Distribution cache mechanism 
builder.Services.AddDistributedMemoryCache(); // Use appropriate distributed cache service

builder.Services.AddHttpClient<HNApiService>(client =>
{
    client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
});
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    {
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HNWebService v1");
        c.RoutePrefix = string.Empty;
    });
    }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
