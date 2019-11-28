using System;
using System.Collections.Generic;
using System.Text;

namespace EventProcessorHostService
{
    public class Message
    {
        public string messageId { get; set; }
        public string deviceId { get; set; }
    }
}
