using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventProcessorHostService
{
    public class Item
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "messageId")]
        public string messageId { get; set; }

        [JsonProperty(PropertyName = "deviceId")]
        public string deviceId { get; set; }

    }
}
