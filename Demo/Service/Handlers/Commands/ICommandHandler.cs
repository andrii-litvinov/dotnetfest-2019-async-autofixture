using System.Threading.Tasks;

namespace Service.Handlers.Commands
{
    public interface ICommandHandler<T>
    {
        Task Handle(T command);
    }
}