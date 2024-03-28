namespace Bars.Gkh.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

using Bars.B4.IoC;
using Bars.B4.Utils;
using Bars.Gkh.SignalR;

using Castle.Windsor;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Методы расширения для SignalR
/// </summary>
public static class SignalRHubExtensions
{
    private const string HubPostfix = "Hub";
    private const string SignalRPath = "signalr";

    /// <summary>
    /// Регистрация хаба
    /// </summary>
    public static void RegisterHub<THub>(
        this IEndpointRouteBuilder builder,
        string name = null,
        Action<HttpConnectionDispatcherOptions> configureOptions = null)
        where THub : Hub
    {
        if (name.IsEmpty())
        {
            name = typeof(THub)
                .Name
                .Replace(SignalRHubExtensions.HubPostfix, string.Empty);
        }

        builder.MapHub<THub>(
            string.Format("{0}/{1}",
                SignalRHubExtensions.SignalRPath,
                name.Trim().ToLowerInvariant()),
            configureOptions);
    }


    /// <inheritdoc cref="IHubClients{TClient}.User"/>
    /// <param name="userId">Идентификатор пользователя</param>
    public static TClient User<TClient>(this IHubClients<TClient> clients, long userId)
        where TClient : class
    {
        return clients.User(userId.ToString());
    }

    /// <inheritdoc cref="IHubClients{TClient}.Users"/>
    /// <param name="userIds">Идентификаторы пользователей</param>
    public static TClient Users<TClient>(this IHubClients<TClient> clients, IEnumerable<long> userIds)
        where TClient : class
    {
        return clients.Users(
            userIds
                .Select(x => x.ToString())
                .ToList());
    }

    /// <summary>
    /// Получить идентификатор пользователя
    /// </summary>
    public static long GetUserId(this HubCallerContext context) => context.UserIdentifier.ToLong();
}