using System.Threading.Tasks;

namespace RecipientList.Services
{
    public interface IMessageConstruction
    {
        Task CreateSendMessage(MessageTransform m);
    }
}