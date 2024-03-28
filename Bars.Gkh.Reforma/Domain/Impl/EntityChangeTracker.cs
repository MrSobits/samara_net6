namespace Bars.Gkh.Reforma.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Entities.ChangeTracker;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    using NHibernate.Linq;

    /// <summary>
    ///     Трекер изменения сущностей, задействованных в синхронизации с Реформой
    /// </summary>
    public class EntityChangeTracker : IEntityChangeTracker
    {
        #region Fields

        private static readonly Dictionary<Type, Delegate> KnownHandlers = new Dictionary<Type, Delegate>();

        private readonly IWindsorContainer container;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">
        ///     IoC контейнер
        /// </param>
        public EntityChangeTracker(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Оповещение об изменении сущности
        /// </summary>
        /// <param name="type">
        ///     Тип сущности
        /// </param>
        /// <param name="entity">
        ///     Сущность
        /// </param>
        public void NotifyChanged(Type type, object entity)
        {
            Delegate handler;
            if (!KnownHandlers.TryGetValue(type, out handler))
            {
                var handlerMethod = this.GetType()
                                        .GetMethod(
                                            "HandleChange",
                                            BindingFlags.Static | BindingFlags.NonPublic,
                                            null,
                                            new[] { typeof(IWindsorContainer), type },
                                            null);
                if (handlerMethod != null)
                {
                    var containerParam = Expression.Parameter(typeof(IWindsorContainer), "container");
                    var entityParam = Expression.Parameter(type, "entity");
                    var methodInvocation = Expression.Call(handlerMethod, containerParam, entityParam);
                    handler = Expression.Lambda(methodInvocation, containerParam, entityParam).Compile();
                    KnownHandlers[type] = handler;
                }
            }

            if (handler != null)
            {
                handler.DynamicInvoke(this.container, entity);
            }
        }

        /// <summary>
        ///     Оповещение об изменении сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="entity">Сущность</param>
        public void NotifyChanged<TEntity>(TEntity entity)
        {
            this.NotifyChanged(typeof(TEntity), entity);
        }

        #endregion

        #region Methods

        private static ManagingOrganization[] GetManOrgsByContragent(IWindsorContainer container, Contragent contragent)
        {
            var service = container.ResolveDomain<ManagingOrganization>();
            try
            {
                return service.GetAll().Where(x => x.Contragent.Id == contragent.Id).ToArray();
            }
            finally
            {
                container.Release(service);
            }
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, Contragent entity)
        {
            var manOrgs = GetManOrgsByContragent(container, entity);
            if (manOrgs.Length == 0)
            {
                return;
            }

            foreach (var manOrg in manOrgs)
            {
                StoreManOrgChange(container, manOrg, null);
            }
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, ContragentContact entity)
        {
            var manOrgs = GetManOrgsByContragent(container, entity.Contragent);
            if (manOrgs.Length == 0)
            {
                return;
            }

            foreach (var manOrg in manOrgs)
            {
                StoreManOrgChange(container, manOrg, null);
            }
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, ManagingOrganization entity)
        {
            StoreManOrgChange(container, entity, null);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, ManOrgContractRealityObject entity)
        {
            StoreRobjectChange(container, entity.RealityObject, null);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, RealityObject entity)
        {
            StoreRobjectChange(container, entity, null);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, ManagingOrgWorkMode entity)
        {
            StoreManOrgChange(container, entity.ManagingOrganization, null);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, ManagingOrgMembership entity)
        {
            StoreManOrgChange(container, entity.ManagingOrganization, null);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, FinActivity entity)
        {
            StoreManOrgChange(container, entity.DisclosureInfo.ManagingOrganization, entity.DisclosureInfo.PeriodDi);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, FinActivityCommunalService entity)
        {
            StoreManOrgChange(container, entity.DisclosureInfo.ManagingOrganization, entity.DisclosureInfo.PeriodDi);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, FinActivityManagCategory entity)
        {
            StoreManOrgChange(container, entity.DisclosureInfo.ManagingOrganization, entity.DisclosureInfo.PeriodDi);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, AdminResp entity)
        {
            StoreManOrgChange(container, entity.DisclosureInfo.ManagingOrganization, entity.DisclosureInfo.PeriodDi);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, DisclosureInfo entity)
        {
            StoreManOrgChange(container, entity.ManagingOrganization, entity.PeriodDi);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, FinActivityRepairCategory entity)
        {
            StoreManOrgChange(container, entity.DisclosureInfo.ManagingOrganization, entity.DisclosureInfo.PeriodDi);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, FinActivityRepairSource entity)
        {
            StoreManOrgChange(container, entity.DisclosureInfo.ManagingOrganization, entity.DisclosureInfo.PeriodDi);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, DisclosureInfoRealityObj entity)
        {
            StoreRobjectChange(container, entity.RealityObject, entity.PeriodDi);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, CommunalService entity)
        {
            StoreRobjectChange(
                container,
                entity.DisclosureInfoRealityObj.RealityObject,
                entity.DisclosureInfoRealityObj.PeriodDi);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, FinActivityRealityObjCommunalService entity)
        {
            StoreRobjectChange(
                container,
                entity.DisclosureInfoRealityObj.RealityObject,
                entity.DisclosureInfoRealityObj.PeriodDi);
        }

        private static void HandleChange(IWindsorContainer container, TehPassportValue entity)
        {
            StoreRobjectChange(container, entity.TehPassport.RealityObject, null);
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, FileInfo entity)
        {
            var refFileDomain = container.ResolveDomain<RefFile>();

            using (container.Using(refFileDomain))
            using (var session = container.Resolve<ISessionProvider>().OpenStatelessSession())
            {
                var refFiles = session.Query<RefFile>().Where(x => x.FileInfo.Id == entity.Id).ToArray();

                if (refFiles.Length == 0)
                {
                    return;
                }

                foreach (var refFile in refFiles)
                {
                    refFileDomain.Delete(refFile.Id);
                }
            }
        }

        [UsedImplicitly]
        private static void HandleChange(IWindsorContainer container, FinActivityManagRealityObj entity)
        {
            StoreManOrgChange(container, entity.DisclosureInfo.ManagingOrganization, entity.DisclosureInfo.PeriodDi);
        }

        private static void StoreManOrgChange(IWindsorContainer container, ManagingOrganization manOrg, PeriodDi period)
        {
            var service = container.ResolveDomain<ChangedManOrg>();
            try
            {
                // синхронизируем только те, которые ведут деятельность
                if (manOrg.ActivityGroundsTermination != GroundsTermination.NotSet)
                {
                    return;
                }

                if (
                    service.GetAll()
                           .Any(
                               x =>
                               x.ManagingOrganization.Id == manOrg.Id && (x.PeriodDi == period || x.PeriodDi == null)))
                {
                    return;
                }

                service.Save(new ChangedManOrg { ManagingOrganization = manOrg, PeriodDi = period });
            }
            finally
            {
                container.Release(service);
            }
        }

        private static void StoreRobjectChange(IWindsorContainer container, RealityObject robject, PeriodDi period)
        {
            var service = container.ResolveDomain<ChangedRobject>();
            try
            {
                if (robject.ConditionHouse == ConditionHouse.Razed)
                {
                    return;
                }

                if (
                    service.GetAll()
                           .Any(x => x.RealityObject.Id == robject.Id && (x.PeriodDi == period || x.PeriodDi == null)))
                {
                    return;
                }

                service.Save(new ChangedRobject { RealityObject = robject, PeriodDi = period });
            }
            finally
            {
                container.Release(service);
            }
        }

        #endregion
    }
}