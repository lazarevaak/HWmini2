using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ZooApplication.Interfaces;
using ZooApplication.Services;
using ZooInfrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1) Infrastructure
builder.Services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
builder.Services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
builder.Services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();

// 2) Application
builder.Services.AddScoped<IAnimalTransferService, AnimalTransferService>();
builder.Services.AddScoped<IFeedingOrganizationService, FeedingOrganizationService>();
builder.Services.AddScoped<IZooStatisticsService, ZooStatisticsService>();

// 3) MVC + Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Zoo API",
        Version = "v1",
        Description = "REST API для управления зоопарком"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // 1) генерим JSON
    app.UseSwagger();

    // 2) монтируем UI на корень сайта
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zoo API V1");
        c.RoutePrefix = string.Empty;    // ← именно это позволяет открывать UI по "/"
    });
}

// Здесь **никаких** app.UseHttpsRedirection() и app.UseStaticFiles()

app.MapControllers();
app.Run();
