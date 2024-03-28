namespace Bars.Gkh.SignalR;

using System.Threading.Tasks;

/// <summary>
/// Интерфейс для клиента для <see cref="NotifyHub"/>
/// </summary>
public interface INotifyHubClient
{
    Task SendMessage(string message);

    Task CloseWindow(string messageId);
}