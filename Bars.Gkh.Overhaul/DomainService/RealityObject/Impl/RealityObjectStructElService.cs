namespace Bars.Gkh.Overhaul.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Entities;

    using Castle.Windsor;

    public class RealityObjectStructElService : IRealityObjectStructElService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetHistory(BaseParams baseParams)
        {
            var roSeIdHistoryDomain = this.Container.ResolveDomain<RealObjStructElementIdHistory>();
            var roSeDomain = this.Container.ResolveDomain<RealityObjectStructuralElement>();
            var logEntityDomain = this.Container.ResolveDomain<LogEntity>();
            var valuesService = this.Container.Resolve<IDomainService<RealityObjectStructuralElementAttributeValue>>();
            
            using (this.Container.Using(logEntityDomain, roSeIdHistoryDomain, roSeDomain, valuesService))
            {
                var loadParam = baseParams.GetLoadParam();
                var realityObjectId = loadParam.Filter.GetAs<long>("realityObjectId");

                var structuralElementIdsForRoStructuralElementQuery = roSeIdHistoryDomain.GetAll()
                    .Where(x => x.RealityObject.Id == realityObjectId)
                    .Select(x => x.RealObjStructElId);

                var structuralElementIdsForRoStructuralElementSeQuery = roSeDomain.GetAll()
                    .Where(x => x.RealityObject.Id == realityObjectId)
                    .Select(x => x.Id);

                var attributeValueIdsForRoStructuralElementAttributeValueQuery = valuesService.GetAll()
                    .Where(x => x.Object.RealityObject.Id == realityObjectId)
                    .Select(x => x.Id);

                var data = logEntityDomain.GetAll()
                    .Where(x => x.ActionKind == ActionKind.Update || x.ActionKind == ActionKind.Insert)
                    .Where(x => (x.EntityType == typeof(RealityObjectStructuralElement).FullName
                        && (structuralElementIdsForRoStructuralElementQuery.Any(y => y == x.EntityId) || structuralElementIdsForRoStructuralElementSeQuery.Any(y => y == x.EntityId)))
                        || x.EntityType == typeof(RealityObjectStructuralElementAttributeValue).FullName
                        && attributeValueIdsForRoStructuralElementAttributeValueQuery.Contains(x.EntityId))
                    .Select(x => new RealityObjectStructuralElementHistoryProxy
                    {
                        Id = x.Id,
                        EntityDescription = x.EntityDescription,
                        EntityDateChange = x.ChangeDate,
                        Ip = x.UserIpAddress,
                        UserLogin = x.UserLogin,
                        EntityTypeChange = x.ActionKind
                    })
                    .Filter(loadParam, this.Container);

                var excludedLogEntities = this.GetExcludeLogEntity(data);

                data = data.Where(x => !excludedLogEntities.Contains(x.Id));

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
        }

        public IDataResult GetHistoryDetail(BaseParams baseParams)
        {
            var logEntityDomain = this.Container.ResolveDomain<LogEntity>();
            var logEntityPropertyDomain = this.Container.ResolveDomain<LogEntityProperty>();

            using (this.Container.Using(logEntityDomain, logEntityPropertyDomain))
            {
                var loadParams = baseParams.GetLoadParam();
                var logEntityId = loadParams.Filter.GetAs<long>("logEntityId");
                const string PropertyName = "Неизвестный атрибут";

                var logEntity = logEntityDomain.GetAll().First(x => x.Id == logEntityId);

                var properties = new Dictionary<string, LoggedPropertyInfo>();
                var dictProperties = this.Container.Resolve<IChangeLogInfoProvider>()
                    .GetLoggedEntities()
                    .With(x => x.ToDictionary(y => y.EntityType, z => z.Properties));
                
                if (dictProperties != null && dictProperties.ContainsKey(logEntity.EntityType))
                {
                    properties = dictProperties[logEntity.EntityType].ToDictionary(x => x.PropertyCode, y => y);

                    // пришлось делать так, потому что AuditLogMap.MapProperty не поддерживает передачу делегата
                    // для параметра loggedPropertyDisplayedName, поэтому приходиться задавать его статичным значением,
                    // а затем динамически менять
                    this.ChangeDisplayNameIfNeeded(logEntity, properties);
                }

                var data = logEntityPropertyDomain.GetAll()
                    .Where(x => x.LogEntity == logEntity)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        PropertyName = properties.ContainsKey(x.PropertyCode)
                            ? properties[x.PropertyCode].DisplayName
                            : GetPropertyName(x.PropertyCode),
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

        private string GetPropertyName(string propCode)
        {
            switch (propCode)
            {
                case "Address":
                    return "Адрес";
                case "Volume":
                    return "Объем КЭ";
                case "Wearout":
                    return "Износ";
                case "UnitMeasure":
                    return "Ед. измерения";
                case "State":
                    return "Статус";
                case "LastOverhaulYear":
                    return "Год установки или последнего ремонта";
                default:
                    return "Иное";
            }
        }

        private void ChangeDisplayNameIfNeeded(LogEntity logEntity, Dictionary<string, LoggedPropertyInfo> properties)
        {
            if (logEntity.EntityType == typeof(RealityObjectStructuralElementAttributeValue).FullName)
            {
                var valuesService = this.Container.Resolve<IDomainService<RealityObjectStructuralElementAttributeValue>>();
                using (this.Container.Using(valuesService))
                {
                    var attrValue = valuesService.Get(logEntity.EntityId);

                    if (attrValue != null)
                    {
                        properties.Where(x => x.Key == "Value").ForEach(x => x.Value.DisplayName = attrValue.Attribute.Name);
                    }
                }
            }
        }

        private List<long> GetExcludeLogEntity(IQueryable<RealityObjectStructuralElementHistoryProxy> data)
        {
            var logEntityPropertyDomain = this.Container.ResolveDomain<LogEntityProperty>();

            var insertedLogEntityIds = data.Where(x => x.EntityTypeChange == ActionKind.Insert).Select(x => x.Id).ToList();
            var filterLogEnityId = new List<long>();

            using (this.Container.Using(logEntityPropertyDomain))
            {
                var logEntityProperties = logEntityPropertyDomain.GetAll()
                    .Where(x => insertedLogEntityIds.Contains(x.LogEntity.Id))
                    .Select(x => new { x.Id, x.NewValue, x.LogEntity })
                    .AsEnumerable()
                    .GroupBy(x => x.LogEntity.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());

                foreach (var logEntityProperty in logEntityProperties)
                {
                    if (logEntityProperty.Value.All(x => x.NewValue.IsEmpty()))
                    {
                        filterLogEnityId.Add(logEntityProperty.Key);
                    }
                }
            }

            return filterLogEnityId;
        }

        private class RealityObjectStructuralElementHistoryProxy
        {
            public long Id { get; set; }

            public string EntityDescription { get; set; }

            public DateTime EntityDateChange { get; set; }

            public string Ip { get; set; }

            public string UserLogin { get; set; }

            public ActionKind EntityTypeChange { get; set; }
        }
    }
}