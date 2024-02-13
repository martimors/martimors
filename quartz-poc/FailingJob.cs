using Quartz;

namespace quartz_poc;

public class FailingJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            throw new NotImplementedException("This job is supposed to fail!");
        }
        catch (Exception e)
        {
            throw new JobExecutionException("...but we should only throw this exception, so that Quartz can handle it!",
                e);
        }
    }
}