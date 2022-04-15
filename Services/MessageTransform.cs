using Newtonsoft.Json;

namespace RecipientList.Services
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