using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("AnalysisEngine is starting...");

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<AnalysisServiceConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("rabbitmq", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("analysis-request-queue", ep =>
                {
                    ep.UseMessageRetry(r => r.Immediate(5));
                    ep.ConfigureConsumer<AnalysisServiceConsumer>(ctx);
                    ep.Bind("FoodBatchExchange", x =>
                    {
                        x.RoutingKey = "foodbatch.request";
                    });
                });
            });
        });

        services.AddScoped<AnalysisServicePublisher>();
    })
    .Build();

Console.WriteLine("AnalysisEngine is now listening for messages...");
await host.RunAsync();