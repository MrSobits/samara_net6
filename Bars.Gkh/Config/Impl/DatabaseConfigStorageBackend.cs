namespace Bars.Gkh.Config.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Config.Impl.Internal;
    using Bars.Gkh.Config.Impl.Internal.Serialization;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.Config;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.SignalR;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    /// <summary>
    ///     Бэкенд хранения настроек в базе данных
    /// </summary>
    public class DatabaseConfigStorageBackend : IGkhConfigStorageBackend
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<GkhConfigParam> DomainService { get; set; }
        public ILogger LogManager { get; set; }

        /// <inheritdoc />
        public IDictionary<string, ValueHolder> GetConfig()
        {
            using var scope = this.Container.BeginScope();
            try
            {
                return this.DomainService.GetAll().ToDictionary(x => x.Key, x => new ValueHolder(x.Value));
            }
            catch (Exception e)
            {
                this.LogManager.LogError(e, "Не удалось загрузить конфигурацию приложения из БД. Будут использованы стандартные значения");
                return new Dictionary<string, ValueHolder>();
            }
        }

        /// <inheritdoc />
        public void UpdateConfig(IDictionary<string, ValueHolder> map)
        {
            using (this.Container.BeginScope())
            {
                this.Container.InTransaction(() =>
                {
                    var keys = map.Keys.ToArray();
                    var values = this.DomainService.GetAll()
                        .WhereContainsBulked(x => x.Key, keys)
                        .ToDictionary(x => x.Key, x => x);

                    foreach (var pair in map)
                    {
                        GkhConfigParam entity;
                        if (!values.TryGetValue(pair.Key, out entity))
                        {
                            entity = new GkhConfigParam { Key = pair.Key };
                        }

                        entity.Value = ConfigSerializer.Serialize(pair.Value);

                        this.DomainService.SaveOrUpdate(entity);
                    }
                });
                if (map.Count > 0)
                {
                    var service = this.Container.Resolve<IGkhConfigService>();
                    var hubContext = this.Container.Resolve<IHubContext<GkhConfigHub, IGkhConfigHubClient>>();

                    using (this.Container.Using(service, hubContext))
                    {
                        var content = service.GetSerializedConfig(map);

                        hubContext
                            .Clients.All
                            .UpdateParams(content)
                            .GetResultWithoutContext();
                    }
                }
            }
        }
    }
}