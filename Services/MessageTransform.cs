using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompetingConsumer.Services
{
    public class MessageTransform : IMessageTransform
    {
        public static string _transformedMessage;

        public string MessageTransformation()
        {
            return _transformedMessage = JsonConvert.SerializeObject(OrderApp._order);
        }
    }
}