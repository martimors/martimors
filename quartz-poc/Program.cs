using Microsoft.Extensions.Hosting;
using Quartz;
using quartz_poc;

Console.WriteLine("Starting the application...");

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddQuartz(q =>
            {
                q.SchedulerName = "MyScheduler";
                q.SchedulerId = "MyScheduler";
                q.UseDefaultThreadPool(10);
                q.UsePersistentStore(s =>
                    {
                        s.UseProperties = true;
                        s.UseNewtonsoftJsonSerializer();
                        s.UsePostgres(p =>
                        {
                            p.ConnectionString =
                                "Server=db;Port=5432;User Id=postgres;Database=postgres;Password=postgres;";
                            p.TablePrefix = "scheduler_";
                        });
                        s.PerformSchemaValidation = true;
                        s.UseClustering(c =>
                        {
                            c.CheckinInterval = TimeSpan.FromSeconds(10);
                            c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
                        });
                        s.RetryInterval = TimeSpan.FromSeconds(15);
                    }
                );
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