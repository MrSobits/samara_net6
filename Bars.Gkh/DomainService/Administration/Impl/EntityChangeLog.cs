namespace Bars.Gkh.DomainService.Administration.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис получения информации об изменении полей сущности
    /// </summary>
    public class EntityChangeLog : IEntityChangeLog
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult List(BaseParams baseParams)
        {
            var changeLogInfoProvider = this.Container.Resolve<IChangeLogInfoProvider>();
            var logEntityPropertyDomain = this.Container.ResolveDomain<LogEntityProperty>();

            var entityId = baseParams.Params.GetAsId();
            var entityType = baseParams.Params.GetAs<string>("entityType");

            if (entityId == 0 || string.IsNullOrEmpty(entityType))
            {
                return BaseDataResult.Error("Ошибка при передаче параметров запроса");
            }

            using (this.Container.Using(changeLogInfoProvider, logEntityPropertyDomain))
            {
                var propertyNameDict = changeLogInfoProvider.GetLoggedEntities()
                    .Where(x => x.EntityType == entityType)
                    .SelectMany(x => x.Properties)
                    .ToDictionary(x => x.PropertyCode, x => x.DisplayName);

                return logEntityPropertyDomain.GetAll()
                    .Where(x => x.LogEntity.EntityId == entityId)
                    .Where(x => x.LogEntity.EntityType == entityType)
                    .Where(x => !(x.LogEntity.ActionKind == ActionKind.Insert && (x.NewValue == null || x.NewValue.Trim() == string.Empty)))
                    .Select(x => new
                    {
                        x.Id,
                        User = x.LogEntity.UserLogin,
                        x.LogEntity.ChangeDate,
                        x.LogEntity.ActionKind,
                        Entity = x.LogEntity.EntityDescription,
                        x.PropertyCode,
                        x.OldValue,
                        x.NewValue
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.User,
                        ChangeDate = x.ChangeDate.ToLocalTime(),
                        x.ActionKind,
                        x.Entity,
                        Property = propertyNameDict.Get(x.PropertyCode, x.PropertyCode),
                        x.OldValue,
                        x.NewValue
                    })
                    .ToListDataResult(baseParams.GetLoadParam());
            }
        }
    }
}