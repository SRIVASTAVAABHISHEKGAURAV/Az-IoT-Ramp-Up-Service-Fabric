#region Copyright
//=======================================================================================
// Microsoft Azure Customer Advisory Team  
//
// This sample is supplemental to the technical guidance published on the community
// blog at http://blogs.msdn.com/b/paolos/. 
// 
// Author: Paolo Salvatori
//=======================================================================================
// Copyright © 2016 Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
// EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. YOU BEAR THE RISK OF USING IT.
//=======================================================================================
#endregion

#region Using Directives

using System;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
//using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

#endregion

namespace EventProcessorHostService
{
    public class EventProcessorHostListener : ICommunicationListener
    {
        private string eventHubName;
        private EventProcessorHost eventProcessorHost;
        private readonly StatelessServiceContext context;

        public EventProcessorHostListener(StatelessServiceContext context)
        {
            this.context = context;
        }

        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            try
            {
                await StartEventProcessorAsync();
                return eventHubName;
            }
            catch (Exception ex)
            {
                // Trace Error
                ServiceEventSource.Current.Message(ex.Message);
                throw;
            }
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                // Trace Error
                ServiceEventSource.Current.Message(ex.Message);
                throw;
            }
        }

        public void Abort()
        {
            try
            {
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.Message(ex.Message);
                throw;
            }
        }

        private async Task StartEventProcessorAsync()
        {
            try
            {
                string hubName = "{hub_Name}";
                string iotHubConnectionString = "{iot_Hub_ConnectionString}";
                string storageConnectionString = "{storageConnectionString}";
                string storageContainerName = "{storageContainerName}";
                string consumerGroupName = PartitionReceiver.DefaultConsumerGroupName;

                eventProcessorHost = new EventProcessorHost(hubName, consumerGroupName, iotHubConnectionString, storageConnectionString, storageContainerName);

                ServiceEventSource.Current.Message("Registering Event Processor [EventProcessor]... ");

                await eventProcessorHost.RegisterEventProcessorAsync<EventProcessor>();

                ServiceEventSource.Current.Message("Event Processor [EventProcessor] successfully registered. ");
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.Message(ex.Message);
                throw ex;
            }
        }

        private static void EventProcessorOptions_ExceptionReceived(object sender, ExceptionReceivedEventArgs e)
        {
            if (e?.Exception == null)
            {
                return;
            }
            ServiceEventSource.Current.Message(e.Exception.Message);
        }
    }
}
