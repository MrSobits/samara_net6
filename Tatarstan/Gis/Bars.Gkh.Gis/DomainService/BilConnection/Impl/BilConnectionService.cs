namespace Bars.Gkh.Gis.DomainService.BilConnection.Impl
{
    using System;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.Application;

    using Entities.Kp50;
    using Enum;
    using Castle.Windsor;

    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Сервис получения строк соединения к серверам БД биллинга
    /// </summary>
    public class BilConnectionService : IBilConnectionService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить строку соединения
        /// </summary>
        /// <param name="connectionType">Тип подключения</param>
        /// <returns></returns>
        public string GetConnection(ConnectionType connectionType)
        {
            var connectionDomain = this.Container.ResolveDomain<BilConnection>();
            try
            {
                var accessor = this.Container.Resolve<IHttpContextAccessor>();
                var appUrl = accessor.HttpContext.Request.Host.Value;
                var bilConnection = connectionDomain.GetAll().FirstOrDefault(x => x.AppUrl == appUrl && x.ConnectionType == connectionType);
                if (string.IsNullOrEmpty(bilConnection?.Connection))
                {
                    throw new Exception($"Не настроена строка соединения к БД  {connectionType.GetDisplayName()}");
                }
                return bilConnection.Connection;
            }
            finally
            {
                this.Container.Release(connectionDomain);
            }
        }
    }
}
