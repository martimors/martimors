using Quartz;

namespace quartz_poc;

[DisallowConcurrentExecution]
public class SayHiJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            Console.WriteLine($"Hi {context.MergedJobDataMap["name"]}! I'm a job!");
            await Task.Delay(1000 * 5);
            Console.WriteLine($"Bye {context.MergedJobDataMap["name"]}! I'm a job!");
        }
        catch (Exception e)
        {
            throw new JobExecutionException(e);
        }
    }
}