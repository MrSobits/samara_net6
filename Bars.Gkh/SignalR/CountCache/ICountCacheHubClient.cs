namespace Bars.Gkh.SignalR;

using System.Threading.Tasks;

/// <summary>
/// Интерфейс клиента для <see cref="CountCacheHub"/>
/// </summary>
public interface ICountCacheHubClient
{
    /// <summary>
    /// Уведомить клиентов о невалидности кэша для текущего префикса
    /// </summary>
    /// <param name="key">Префикс ключа</param>
    Task ClearCache(string key);
}