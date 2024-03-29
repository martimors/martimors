﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;
using Quartz;
using quartz_poc;

Console.WriteLine("Starting the application...");

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddLogging();
        services.AddOpenTelemetry().WithTracing(builder =>
        {
            // builder.AddQuartzInstrumentation();
            builder.AddConsoleExporter();
        });
        services.AddQuartz(q =>
            {
                q.ConfigureQuartz("MyScheduler",
                    "Server=db;Port=5432;User Id=postgres;Database=postgres;Password=postgres;");
                q.ScheduleJob<SayHiJob>(t => t.WithIdentity("every_2_seconds", "console")
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())
                    .UsingJobData("name", "John Doe")
                    .StartNow()
                    .WithDescription("Say hi every 2 seconds!"));
                q.ScheduleJob<RandomNumberJob>(t => t.WithIdentity("every_minute_cron", "console")
                    .WithCronSchedule("* 0/1 * 1/1 * ? * ", x => x.WithMisfireHandlingInstructionDoNothing())
                    .StartNow()
                    .WithDescription("Print a random number every second!"));
                q.ScheduleJob<FailingJob>(t => t.WithIdentity("every_10_seconds", "console")
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                    .StartNow()
                    .WithDescription("Throw an exception every 10 seconds!"));
                q.ScheduleJob<CountToTenJob>(t => t.WithIdentity("count_to_ten", "console")
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())
                    .StartNow()
                    .WithDescription("Count to ten every 30 seconds!"));
            }
        );
        services.AddQuartzHostedService(opt => { opt.WaitForJobsToComplete = true; });
    }).Build();

Console.WriteLine("Application started!");
await builder.RunAsync();