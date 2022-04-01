using System.Threading.Tasks;

namespace CompetingConsumer.Services
{
    public interface IMessageConstruction
    {
        Task CreateSendMessage(MessageTransform m);
    }
}