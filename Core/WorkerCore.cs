using Invoices_Manager_API.Controllers.v01;

namespace Invoices_Manager_API.Core
{
    public class WorkerCore
    {
        private readonly ILogger<WorkerCore> _logger;
        private readonly DataBaseContext _dbContext;

        public WorkerCore(ILogger<WorkerCore> logger, DataBaseContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task InitAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Do something in a loop
                    // ...

                    await Task.Delay(5000, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Task was cancelled, exit gracefully
                _logger.LogInformation("Worker Task was cancelled.");
            }
        }
    }
}
