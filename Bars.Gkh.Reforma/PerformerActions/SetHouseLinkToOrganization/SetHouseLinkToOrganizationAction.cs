namespace Bars.Gkh.Reforma.PerformerActions.SetHouseLinkToOrganization
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;

    using Castle.Windsor;

    /// <summary>
    ///     Действие привязки жилого дома к УО
    /// </summary>
    public class SetHouseLinkToOrganizationAction : LoggableSyncActionBase<SetHouseLinkToOrganizationParams, object>
    {
        #region Constants

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "SetHouseLinkToOrganization";

        #endregion

        #region Public Properties

        public override string Id
        {
            get
            {
                return ActionId;
            }
        }

        #endregion

        #region Methods

        protected override object Execute()
        {
            var service = Container.ResolveDomain<RefRealityObject>();
            var moService = Container.Resolve<IManOrgService>();
            try
            {
                Logger.SetActionDetails(string.Format("Внешний идентификатор = {0} ИНН = {1}", Parameters.ExternalId, Parameters.Inn));
                var entity = service.GetAll().FirstOrDefault(x => x.ExternalId == Parameters.ExternalId);
                if (entity == null)
                {
                    throw new Exception("Не найден дом с указанным внешним идентификатором");
                }

                Logger.SetActionDetails(string.Format("Адрес = {0} (Внешний идентификатор = {1}) ИНН = {2}", entity.RealityObject.Address, Parameters.ExternalId, Parameters.Inn));

                Client.SetHouseLinkToOrganization(Parameters.ExternalId, Parameters.Inn, Parameters.DateStart, Parameters.management_reason);
                var moId = moService.GetRefManOrgIdByInn(Parameters.Inn);
                if (moId > 0)
                {
                    entity.RefManagingOrganization = new RefManagingOrganization { Id = moId };
                    service.Update(entity);
                }

                return null;
            }
            finally
            {
                Container.Release(service);
                Container.Release(moService);
            }
        }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="syncProvider">
        ///     Провайдер синхронизации
        /// </param>
        /// <param name="parameters">
        ///     Параметры действия
        /// </param>
        public SetHouseLinkToOrganizationAction(IWindsorContainer container, ISyncProvider syncProvider, SetHouseLinkToOrganizationParams parameters)
            : base(container, syncProvider, parameters)
        {
        }

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="syncProvider">
        ///     Провайдер синхронизации
        /// </param>
        public SetHouseLinkToOrganizationAction(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        #endregion
    }
}