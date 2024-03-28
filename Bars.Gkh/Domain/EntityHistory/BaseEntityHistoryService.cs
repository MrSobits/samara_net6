namespace Bars.Gkh.Domain.EntityHistory
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Базовый сервис логирования изменений сущности <see cref="T"/>
    /// </summary>
    /// <typeparam name="T">Тип логируемой сущности</typeparam>
    public abstract class BaseEntityHistoryService<T> : IEntityHistoryService<T>
        where T : PersistentObject
    {
        private T oldEntity;
        private T newEntity;
        private string parentName;
        protected long ParentEntityId;
        protected abstract EntityHistoryType GroupType { get; }

        private readonly Dictionary<string, Func<T, string>> logProperties = new Dictionary<string, Func<T, string>>();
        private readonly Dictionary<string, string> oldValues = new Dictionary<string, string>();
        private readonly Dictionary<string, string> newValues = new Dictionary<string, string>();

        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Инициализировать маппинг логируемых свойств
        /// </summary>
        protected abstract void Init();

        protected BaseEntityHistoryService()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            this.Init();
        }

        /// <inheritdoc />
        public void StoreEntity(long entityId)
        {
            this.oldEntity = this.GetExistsEntity(entityId);

            this.LogFields(this.oldEntity, this.oldValues);
        }

        /// <inheritdoc />
        public void LogCreate(T entity, IHaveId parentEntity)
        {
            this.ParentEntityId = parentEntity?.Id ?? 0;
            this.newEntity = entity;
            this.parentName = parentEntity.GetType().FullName;

            this.LogFields(this.newEntity, this.newValues);

            this.SaveLog(ActionKind.Insert);
        }

        /// <inheritdoc />
        public void LogUpdate(T entity, IHaveId parentEntity)
        {
            this.ParentEntityId = parentEntity?.Id ?? 0;
            this.newEntity = entity;
            this.parentName = parentEntity.GetType().FullName;

            this.LogFields(this.newEntity, this.newValues);

            this.SaveLog(ActionKind.Update);
        }

        /// <inheritdoc />
        public void LogDelete(IHaveId parentEntity)
        {
            this.ParentEntityId = parentEntity?.Id ?? 0;
            this.newEntity = null;
            this.parentName = parentEntity.GetType().FullName;

            this.LogFields(this.newEntity, this.newValues);

            this.SaveLog(ActionKind.Delete);
        }

        /// <summary>
        /// Маппинг лигируемого поля
        /// </summary>
        /// <param name="propertySelector">Селектор поля</param>
        /// <param name="fieldName">Наименование поля</param>
        protected void Map(Func<T, string> propertySelector, string fieldName)
        {
            this.logProperties.Add(fieldName, propertySelector);
        }

        private T GetExistsEntity(long entityId)
        {
            var provider =this.Container.Resolve<ISessionProvider>();
            using (var session = provider.OpenStatelessSession())
            {
                return session.Get<T>(entityId);
            }
        }

        private void LogFields(T entity, Dictionary<string, string> fieldValues)
        {
            fieldValues.Clear();
            if (entity != null)
            {
                foreach (var property in this.logProperties)
                {
                    fieldValues.Add(property.Key, property.Value(entity));
                }
            }
        }

        private void SaveLog(ActionKind action)
        {
            var entityHistoryInfoDomain = this.Container.Resolve<IDomainService<EntityHistoryInfo>>();
            var entityHistoryFieldDomain = this.Container.Resolve<IDomainService<EntityHistoryField>>();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var requestInfo = this.Container.Resolve<RequestingUserInformation>();

            using (this.Container.Using(entityHistoryInfoDomain, entityHistoryFieldDomain, userManager, requestInfo))
            {
                var fields = new List<EntityHistoryField>();
                var info = new EntityHistoryInfo
                {
                    GroupType = this.GroupType,
                    EntityId = this.oldEntity?.Id ?? this.newEntity.Id,
                    EntityName = typeof(T).FullName,
                    ActionKind = action,
                    User = userManager.GetActiveUser(),
                    IpAddress = requestInfo.RequestIpAddress,
                    EditDate = DateTime.Now,
                    ParentEntityName = this.parentName
                };

                info.Username = info.User?.Name;
                info.ParentEntityId = this.ParentEntityId == 0 ? info.EntityId : this.ParentEntityId;

                foreach (var fieldName in this.logProperties.Keys)
                {
                    var oldFieldValue = this.oldValues.Get(fieldName);
                    var newFieldValue = this.newValues.Get(fieldName);

                    if (oldFieldValue != newFieldValue)
                    {
                        fields.Add(new EntityHistoryField
                        {
                            EntityHistoryInfo = info,
                            FieldName = fieldName,
                            OldValue = oldFieldValue,
                            NewValue = newFieldValue
                        });
                    }
                }

                if (fields.Count > 0)
                {
                    this.Container.InTransaction(() =>
                    {
                        entityHistoryInfoDomain.Save(info);
                        fields.ForEach(entityHistoryFieldDomain.Save);
                    });
                }
            }
        }
    }
}