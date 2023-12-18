using HNWebService.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configuration setup
builder.Configuration.AddJsonFile("appsettings.json", optional: true);

// Services registration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HNWebService", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// added singleton dependency injection
builder.Services.AddSingleton<IHNApiService, HNApiService>();

// Read BaseUrls from appsettings.json and configure as a singleton service
var baseUrls = new BaseUrls();
builder.Configuration.GetSection("BaseUrls").Bind(baseUrls);
// added singleton dependency injection
builder.Services.AddSingleton(baseUrls);

// added Distributed Memory Cache 
builder.Services.AddDistributedMemoryCache();
// added Distributed Memory Cache 
builder.Services.AddHttpClient();

// added cors and allowed angular application
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
    {
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HNWebService v1");
        c.RoutePrefix = string.Empty;
    });
    }
app.UseCors("AllowOrigin");
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
