namespace Bars.Gkh.SignalR;

using System.Threading.Tasks;

/// <summary>
/// Интерфейс для <see cref="GkhParamsHub"/>
/// </summary>
public interface IGkhParamsHubClient
{
    Task UpdateParams(string parameters);
}