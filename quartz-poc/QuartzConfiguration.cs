using Quartz;

namespace quartz_poc;

public static class QuartzConfiguration
{
    public static void ConfigureQuartz(this IServiceCollectionQuartzConfigurator q, string name,
        string psqlConnectionString)
    {
        q.SchedulerName = name;
        q.SchedulerId = name;
        q.UseDefaultThreadPool(10);
        q.UsePersistentStore(s =>
            {
                s.UseProperties = true;
                s.UseNewtonsoftJsonSerializer();
                s.UsePostgres(p =>
                {
                    p.ConnectionString = psqlConnectionString;
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
    }
}