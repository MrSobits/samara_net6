namespace Bars.Gkh.SignalR;

using System.Threading.Tasks;

/// <summary>
/// Интерфейс для клиента <see cref="ProsecutorsOfficeHub"/>
/// </summary>
public interface IProsecutorsOfficeHubClient
{
    Task RefreshGrid();
}