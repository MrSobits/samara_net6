namespace Bars.Gkh.UserActionRetention.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Castle.Windsor;

    public class UserActionRetentionService: IUserActionRetentionService
    {
        private readonly IWindsorContainer _container;
        private readonly IDomainService<LogEntity> _logEntityDomain;
        public UserActionRetentionService(IWindsorContainer container, IDomainService<LogEntity> logEntityDomain)
        {
            _container = container;
            this._logEntityDomain = logEntityDomain;
        }

        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var dateFrom = baseParams.Params.GetAs("dateFrom", DateTime.MinValue);
            var dateTo = baseParams.Params.GetAs("dateTo", DateTime.MinValue);
            var entityTypes = baseParams.Params.GetAs("entityTypes", string.Empty);
            var listEntityTypes = !string.IsNullOrEmpty(entityTypes) ? entityTypes.Split(',').Select(x => x.Trim()).ToArray() : new string[0];

            var userIds = baseParams.Params.GetAs("userIds", string.Empty);
            var listUserIds = !string.IsNullOrEmpty(userIds) ? userIds.Split(',').Select(x => x.Trim()).ToArray() : new string[0];
            var dictEntityNames = _container.Resolve<IChangeLogInfoProvider>()
                                          .GetLoggedEntities()
                                          .With(x => x.ToDictionary(y => y.EntityType, z => z.EntityName)) ??
                                 new Dictionary<string, string>();

            var data = _logEntityDomain.GetAll()
                                    .WhereIf(dateFrom != DateTime.MinValue, x => x.ChangeDate >= dateFrom)
                                    .WhereIf(dateTo != DateTime.MinValue, x => x.ChangeDate <= dateTo)
                                    .WhereIf(listEntityTypes.Length > 0, x => listEntityTypes.Contains(x.EntityType))
                                    .WhereIf(listUserIds.Length > 0, x => listUserIds.Contains(x.UserId))
                                    .Select(x => new
                                    {
                                        x.Id,
                                        EntityDateChange = x.ChangeDate,
                                        EntityName = dictEntityNames.ContainsKey(x.EntityType) ? dictEntityNames[x.EntityType] : "Неизвестный объект",
                                        x.EntityId,
                                        EntityTypeChange = x.ActionKind,
                                        TypeAction = "Юридическое",
                                        x.EntityDescription,
                                        x.UserId,
                                        x.UserName,
                                        x.UserLogin,
                                        Ip = x.UserIpAddress
                                    });

            return new ListDataResult(data.ToList(), data.Count());
        }

    }
}
