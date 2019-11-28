using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventProcessorHostService
{
    public class MessagePayload
    {
        public string messageId { get; set; }
        public string deviceId { get; set; }
    }
}
