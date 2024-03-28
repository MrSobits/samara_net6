namespace Bars.Gkh.SignalR;

using System.Threading.Tasks;

/// <summary>
/// Интерфейс клиента для <see cref="GkhConfigHub"/>
/// </summary>
public interface IGkhConfigHubClient
{
    Task UpdateParams(string content);
}