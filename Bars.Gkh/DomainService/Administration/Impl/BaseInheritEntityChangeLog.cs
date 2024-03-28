namespace Bars.Gkh.DomainService.Administration.Impl
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Базовый сервис получения информации об изменении полей сущности <see cref="T"/> зависимой от другой сущности
    /// </summary>
    public abstract class BaseInheritEntityChangeLog<T> : IInheritEntityChangeLog
        where T : PersistentObject
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<T> DomainService { get; set; }

        public static string Id => typeof(T).Name;

        /// <inheritdoc />
        public virtual string Code => BaseInheritEntityChangeLog<T>.Id;

        protected abstract Expression<Func<T, bool>> GetEntityIdSelector(long entityId);

        /// <inheritdoc />
        public virtual IDataResult List(BaseParams baseParams)
        {
            var changeLogInfoProvider = this.Container.Resolve<IChangeLogInfoProvider>();
            var logEntityPropertyDomain = this.Container.ResolveDomain<LogEntityProperty>();

            var entityId = baseParams.Params.GetAsId();
            var entityType = baseParams.Params.GetAs<string>("entityType");

            if (entityId == 0 || string.IsNullOrEmpty(entityType))
            {
                return BaseDataResult.Error("Ошибка при передаче параметров запроса");
            }

            var ids = this.DomainService.GetAll()
                .Where(this.GetEntityIdSelector(entityId))
                .Select(x => x.Id)
                .ToList();

            using (this.Container.Using(changeLogInfoProvider, logEntityPropertyDomain))
            {
                var propertyNameDict = changeLogInfoProvider.GetLoggedEntities()
                    .Where(x => x.EntityType == entityType)
                    .SelectMany(x => x.Properties)
                    .ToDictionary(x => x.PropertyCode, x => x.DisplayName);

                return logEntityPropertyDomain.GetAll()
                    .Where(x => x.LogEntity.EntityType == entityType)
                    .Where(x => !(x.LogEntity.ActionKind == ActionKind.Insert && (x.NewValue == null || x.NewValue.Trim() == string.Empty)))
                    .WhereContainsBulked(x => x.LogEntity.EntityId, ids)
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
                        x.ChangeDate,
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