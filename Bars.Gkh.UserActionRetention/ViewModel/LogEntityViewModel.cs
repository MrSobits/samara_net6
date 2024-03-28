namespace Bars.Gkh.UserActionRetention.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;

    public class LogEntityViewModel : BaseViewModel<LogEntity>
    {
        public override IDataResult List(IDomainService<LogEntity> domainService, BaseParams baseParams)
        {
            var dateFrom = baseParams.Params.GetAs("dateFrom", DateTime.MinValue);
            var dateTo = baseParams.Params.GetAs("dateTo", DateTime.MinValue);
            var entityTypes = baseParams.Params.GetAs("entityTypes", string.Empty);
            var listEntityTypes = !string.IsNullOrEmpty(entityTypes) ? entityTypes.Split(',').Select(x => x.Trim()).ToArray() : new string[0];

            var userIds = baseParams.Params.GetAs("userIds", string.Empty);
            var listUserIds = !string.IsNullOrEmpty(userIds) ? userIds.Split(',').Select(x => x.Trim()).ToArray() : new string[0];

            var loadParams = this.GetLoadParam(baseParams);

                var dictEntityNames = this.Container.Resolve<IChangeLogInfoProvider>()
                    .GetLoggedEntities()
                    .With(x => x.Distinct(y => y.EntityType).ToDictionary(y => y.EntityType, z => z.EntityName)) ??
                    new Dictionary<string, string>();

            // Нестандартное отслеживание сущности MonthlyFeeAmountDecision 
            dictEntityNames.Add("Bars.Gkh.Decisions.Nso.Entities.MonthlyFeeAmountDecision", "Размер ежемесячного взноса на КР (Протокол решений)");
            
                var data = domainService.GetAll()
                    .WhereIf(dateFrom != DateTime.MinValue, x => x.ChangeDate >= dateFrom.Date)
                    .WhereIf(dateTo != DateTime.MinValue, x => x.ChangeDate <= dateTo.Date.AddDays(1).AddMilliseconds(-1))
                    .WhereIf(listEntityTypes.Length > 0, x => listEntityTypes.Contains(x.EntityType))
                    .WhereIf(listUserIds.Length > 0, x => listUserIds.Contains(x.UserId))
                    .Select(
                        x => new
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
                        })
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
