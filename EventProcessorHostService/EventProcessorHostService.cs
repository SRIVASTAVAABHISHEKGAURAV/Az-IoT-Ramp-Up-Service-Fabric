using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace EventProcessorHostService
{
    internal sealed class EventProcessorHostService : StatelessService
    {
        public EventProcessorHostService(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
                    {
                        new ServiceInstanceListener(s => new EventProcessorHostListener(s))
                    };
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // This service instance continues processing until the instance is terminated.
            while (!cancellationToken.IsCancellationRequested)
            {
                // Pause for 1 second before continue processing.
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }

            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            ////long iterations = 0;

            ////while (true)
            ////{
            ////    cancellationToken.ThrowIfCancellationRequested();

            ////    ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

            ////    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            ////}
        }
    }
}
