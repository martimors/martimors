using Microsoft.Extensions.Logging;
using Quartz;

namespace quartz_poc;

public class FailingJob(ILogger<FailingJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            throw new NotImplementedException("This job is supposed to fail!");
        }
        catch (Exception e)
        {
            logger.LogError("This is an error message!");
            throw new JobExecutionException("...but we should only throw this exception, so that Quartz can handle it!",
                e);
        }
    }
}