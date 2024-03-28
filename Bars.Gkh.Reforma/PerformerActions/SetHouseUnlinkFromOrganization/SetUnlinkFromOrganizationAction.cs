namespace Bars.Gkh.Reforma.PerformerActions.SetHouseUnlinkFromOrganization
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.ReformaService;

    using Castle.Windsor;

    /// <summary>
    ///     Действие отвязывает жилой дом от УО.
    /// </summary>
    public class SetUnlinkFromOrganizationAction : LoggableSyncActionBase<SetUnlinkFromOrganizationParams, object>
    {
        #region Constants

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "SetUnlinkFromOrganization";

        #endregion

        #region Public Properties

        public override string Id
        {
            get
            {
                return SetUnlinkFromOrganizationAction.ActionId;
            }
        }

        #endregion

        #region Methods

        protected override object Execute()
        {
            var service = this.Container.ResolveDomain<RefRealityObject>();
            try
            {
                this.Logger.SetActionDetails(string.Format("Внешний идентификатор = {0}", this.Parameters.ExternalId));
                var entity = service.GetAll().FirstOrDefault(x => x.ExternalId == this.Parameters.ExternalId);
                if (entity == null)
                {
                    throw new Exception("Не найден дом с указанным внешним идентификатором");
                }

                this.Logger.SetActionDetails($"Адрес = {entity.RealityObject.Address} Внешний идентификатор = {this.Parameters.ExternalId}");

                var unlinkParams = new UnlinkHouseFromOrganizationData
                {
                    date_stop = this.Parameters.ReasonType == ContractStopReasonEnum.added_by_error ? (DateTime?)null : this.Parameters.DateEnd,
                    house_id = this.Parameters.ExternalId,
                    stop_reason = this.Parameters.ReasonType == ContractStopReasonEnum.added_by_error
                        ? null
                        : (string.IsNullOrEmpty(this.Parameters.Reason) ? this.Parameters.ReasonType.GetDisplayName() : this.Parameters.Reason),
                    stop_reason_type = (int)this.Parameters.ReasonType
                };

                this.Client.SetUnlinkFromOrganization(unlinkParams);

                entity.RefManagingOrganization = null;
                service.Update(entity);

                return null;
            }
            finally
            {
                this.Container.Release(service);
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
        public SetUnlinkFromOrganizationAction(IWindsorContainer container, ISyncProvider syncProvider, SetUnlinkFromOrganizationParams parameters)
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
        public SetUnlinkFromOrganizationAction(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        #endregion
    }
}