namespace Bars.Gkh.SignalR;

using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Прокси-контекст для хаба
/// </summary>
public interface IHubContextProxy<THub>
    where THub : Hub
{
}

/// <summary>
/// Прокси-контекст для хаба
/// </summary>
/// <typeparam name="THub">Тип хаба</typeparam>
public class HubContextProxy<THub> : IHubContextProxy<THub>, IHubContext<THub>
    where THub : Hub
{
    /// <inheritdoc cref="IHubContext.Clients"/>
    public IHubClients Clients => this.Context.Clients;

    /// <inheritdoc cref="IHubContext.Groups"/>
    public IGroupManager Groups => this.Context.Groups;

    /// <summary>
    /// Оригинальный контекст
    /// </summary>
    protected IHubContext<THub> Context { get; }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="context">Оригинальный контекст</param>
    public HubContextProxy(IHubContext<THub> context)
    {
        this.Context = context;
    }
}

/// <summary>
/// Прокси-контекст для строго типизированного хаба
/// </summary>
/// <typeparam name="THub">Тип хаба</typeparam>
/// <typeparam name="TClient">Тип клиентского интерфейса</typeparam>
public class HubContextProxy<THub, TClient> : IHubContextProxy<THub>, IHubContext<THub, TClient>
    where THub : Hub<TClient>
    where TClient : class
{
    /// <inheritdoc cref="IHubContext{THub, TClient}.Groups"/>
    public IGroupManager Groups => this.Context.Groups;

    /// <inheritdoc cref="IHubContext{THub, TClient}.Clients"/>
    public IHubClients<TClient> Clients => this.Context.Clients;

    /// <summary>
    /// Оригинальный контекст
    /// </summary>
    protected IHubContext<THub, TClient> Context { get; }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="context">Оригинальный контекст</param>
    public HubContextProxy(IHubContext<THub, TClient> context)
    {
        this.Context = context;
    }
}