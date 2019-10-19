using System.Threading.Tasks;

namespace Service.CommandHandlers
{
    public interface ICommandHandler<T>
    {
        Task Handle(T command);
    }
}