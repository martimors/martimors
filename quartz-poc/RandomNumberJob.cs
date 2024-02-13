using Quartz;

namespace quartz_poc;

[DisallowConcurrentExecution]
public class RandomNumberJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var random = new Random();
            var randomNumber = random.Next(1, 100);
            Console.WriteLine($"Random number: {randomNumber}");
            await Task.Delay(10);
            Console.WriteLine("Random number job finished!");
        }
        catch (Exception e)
        {
            throw new JobExecutionException(e);
        }
    }
}