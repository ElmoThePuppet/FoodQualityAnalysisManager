using FoodQualityAnalysis.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using QualityManager.Application.Interfaces;
using QualityManager.Application.Services;
using QualityManager.Domain.DTOs;
using QualityManager.Infrastructure.Repository;
using QualityManager.Middleware;
using RabbitMQ.Client;
using Contract.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

builder.Services.AddDbContext<FoodQualityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AnalysisResultConsumer>();
    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq://localhost");
        cfg.ReceiveEndpoint("analysis-response-queue", e =>
        {
            e.ConfigureConsumer<AnalysisResultConsumer>(context);
            e.Bind("FoodBatchExchange", x =>
            {
                x.RoutingKey = "analysis.response";
            });
        });

        cfg.ReceiveEndpoint("analysis-request-queue", e =>
        {
            e.Bind("FoodBatchExchange", x =>
            {
                x.RoutingKey = "foodbatch.request";
            });
        });
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFoodBatchRepository, FoodBatchRepository>();
builder.Services.AddScoped<IAnalysisRepository, AnalysisRepository>();
builder.Services.AddScoped<IFoodBatchService, FoodBatchService>();
builder.Services.AddScoped<IAnalysisResultService, AnalysisResultService>();
builder.Services.AddScoped<FoodBatchPublisher>();
builder.Services.AddScoped<AnalysisResultConsumer>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/foodbatch", async (FoodBatchRequestDto request, IFoodBatchService foodBatchService) =>
{
    var response = await foodBatchService.ProcessFoodBatchAsync(new FoodBatchRequestDto
    {
        FoodName = request.FoodName,
        DateSubmitted = request.DateSubmitted,
        SerialNumber = request.SerialNumber,
        AnalysisType = request.AnalysisType,
    });

    return Results.Ok(response);
})
.WithName("SubmitFoodBatch");

app.MapGet("/api/foodbatch/{serialNumber}/status", async (string serialNumber, IFoodBatchService foodBatchService) =>
{
    var status = await foodBatchService.GetFoodBatchStatusBySerialNumberAsync(serialNumber);
    return status is not null ? Results.Ok(status) : Results.NotFound("Food batch not found.");
})
.WithName("GetFoodBatchStatus");

app.MapGet("/api/foodbatch/{serialNumber}/analysis", async (string serialNumber, IAnalysisRepository analysisRepository) =>
{
    var analysis = await analysisRepository.GetAnalysisAsync(serialNumber);
    return analysis is not null ? Results.Ok(analysis) : Results.NotFound("No analysis found for this batch.");
})
.WithName("GetFoodBatchAnalysis");

app.Run();
