using Quartz;

namespace quartz_poc;

public class CountToTenJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            for (var i = 1; i <= 10; i++)
            {
                Console.WriteLine("Counting to ten: " + i);
                await Task.Delay(1000);
            }

        }
        catch (Exception e)
        {
            throw new JobExecutionException(e);
        }
    }
}