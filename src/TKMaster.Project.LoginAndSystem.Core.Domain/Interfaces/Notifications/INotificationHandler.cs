using System.Threading.Tasks;

namespace TKMaster.Project.LoginAndSystem.Core.Domain.Interfaces.Notifications;

public interface INotificationHandler<T> where T : class
{
    Task Handler(T notification);
}
