namespace Bars.Gkh.UserActionRetention.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;

    internal class LogEntityPropertyViewModel : BaseViewModel<LogEntityProperty>
    {
        private const string PropertyName = "Неизвестный атрибут";

        public override IDataResult List(IDomainService<LogEntityProperty> domainService, BaseParams baseParams)
        {
            var logEntityId = baseParams.Params.GetAs("logEntityId", 0);

            var loadParams = this.GetLoadParam(baseParams);

            var logEntity = this.Container.Resolve<IDomainService<LogEntity>>().GetAll().First(x => x.Id == logEntityId);

            var properties = new Dictionary<string, LoggedPropertyInfo>();
            var dictProperties = this.Container.Resolve<IChangeLogInfoProvider>()
                                      .GetLoggedEntities()
                                          .With(x => x.Distinct(y => y.EntityType).ToDictionary(y => y.EntityType, z => z.Properties));

            dictProperties.Add("Bars.Gkh.Decisions.Nso.Entities.MonthlyFeeAmountDecision", 
                new List<LoggedPropertyInfo>
            {
                new LoggedPropertyInfo { DisplayName = "Решение", PropertyCode = "Decision", Type = typeof(string)}
            });

            if (dictProperties != null && dictProperties.ContainsKey(logEntity.EntityType))
            {
                properties = dictProperties[logEntity.EntityType].ToDictionary(x => x.PropertyCode, y => y);
            }


            var data = domainService.GetAll()
                                    .Where(x => x.LogEntity == logEntity)
                                    .AsEnumerable()
                                    .Select(x => new
                                    {
                                        x.Id,
                                        PropertyName = properties.ContainsKey(x.PropertyCode) 
                                            ? properties[x.PropertyCode].DisplayName
                                            : LogEntityPropertyViewModel.PropertyName,
                                        x.NewValue,
                                        x.OldValue,
                                        Type = properties.ContainsKey(x.PropertyCode)
                                            ? this.GetNativeType(properties[x.PropertyCode].Type).Name
                                            : null,
                                    })
                                    .AsQueryable()
                                    .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        private Type GetNativeType(Type type)
        {
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return type.GetGenericArguments()[0];
                }
            }

            return type;
        }
    }
}
