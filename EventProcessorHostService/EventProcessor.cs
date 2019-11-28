using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using EventProcessorHostService.Services;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Cosmos;

namespace EventProcessorHostService
{
    class EventProcessor : IEventProcessor
    {
        private readonly ICosmosDbService _cosmosDbService;
        private string account, key, databaseName, containerName;

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine("LoggingEventProcessor closed, processing partition: " + context.PartitionId + " Reason: " + reason);
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine("LoggingEventProcessor Opened, processing partition: " + context.PartitionId);
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine("LoggingEventProcessor error, processing partition: " + context.PartitionId + " Error Message: " + error.Message);
            return Task.CompletedTask;
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {

            Console.WriteLine($"Batch of events received on partition: ' {context.PartitionId}");
            foreach (var eventData in messages)
            {
                var payload = Encoding.ASCII.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                var deviceId = eventData.SystemProperties["iothub-connection-device-id"];

                Console.WriteLine($"Message received on partition: ' {context.PartitionId} ', " +
                    $"device Id: '{deviceId} '" +
                    $"payload: ' {payload}'");

                var telemetry = JsonConvert.DeserializeObject<MessagePayload>(payload);

                CosmosClientBuilder clientBuilder = new CosmosClientBuilder(account, key);
                CosmosClient client = clientBuilder.WithConnectionModeDirect().Build();

                var cosmosDbService = new CosmosDBService(client, databaseName, containerName);
                DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
                await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

                var item = new Item();
                item.Id = Guid.NewGuid().ToString();
                item.messageId = telemetry.messageId;
                item.deviceId = telemetry.deviceId;

                await cosmosDbService.AddItemAsync(item);

            }

            await context.CheckpointAsync();
        }

        private void SendFirstRespondersTo(decimal latitude, decimal longitude)
        {
            Console.WriteLine($"First responders sent to ({latitude},{longitude})!");
        }
    }
}
