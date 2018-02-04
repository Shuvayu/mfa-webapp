using MFA.Entities.Configurations;
using MFA.IInfrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MFA.Infrastructure
{
    public class WaitCall : IWaitCall
    {
        private static Queue<DateTime> _timeStampQueue;
        private readonly IOptions<AzureConfiguration> _azureSettings;

        public WaitCall(IOptions<AzureConfiguration> azureSettings)
        {
            _azureSettings = azureSettings;
            _timeStampQueue = new Queue<DateTime>(_azureSettings.Value.CognitiveServicesApiCallsPerMin);
        }

        public void AddCallTimeToQueue()
        {
            _timeStampQueue.Enqueue(DateTime.UtcNow);
        }

        public async Task WaitCallLimitPerMinAsync()
        {
            Monitor.Enter(_timeStampQueue);
            try
            {
                if (_timeStampQueue.Count >= _azureSettings.Value.CognitiveServicesApiCallsPerMin)
                {
                    var timeInterval = DateTime.UtcNow - _timeStampQueue.Peek();
                    if (timeInterval < TimeSpan.FromSeconds(60))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(60) - timeInterval);
                    }
                    _timeStampQueue.Dequeue();
                }
                _timeStampQueue.Enqueue(DateTime.UtcNow);
            }
            finally
            {
                Monitor.Exit(_timeStampQueue);
            }
        }
    }
}
