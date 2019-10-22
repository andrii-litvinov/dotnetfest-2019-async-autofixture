using System.Threading.Tasks;

namespace Service.Handlers.Commands
{
    public interface ICommandHandler<T>
    {
        // Decorate for logging, metrics, retries, transactions
        Task Handle(T command);
    }
}