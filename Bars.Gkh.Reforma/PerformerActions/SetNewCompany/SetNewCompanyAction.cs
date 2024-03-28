namespace Bars.Gkh.Reforma.PerformerActions.SetNewCompany
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Enums;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.DataCollectors;
    using Bars.Gkh.Reforma.ReformaService;

    using Castle.Windsor;

    /// <summary>
    ///     Действие создания новой УО
    /// </summary>
    public class SetNewCompanyAction : LoggableSyncActionBase<string, object>
    {
        #region Constants

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "SetNewCompany";

        #endregion

        #region Public Properties

        public override string Id
        {
            get
            {
                return SetNewCompanyAction.ActionId;
            }
        }

        #endregion

        #region Methods

        protected override object Execute()
        {
            var service = this.Container.Resolve<IManOrgService>();
            var collector = this.Container.Resolve<IManOrgDataCollector>();
            var periodDomain = this.Container.ResolveDomain<ReportingPeriodDict>();

            try
            {
                this.Logger.SetActionDetails(string.Format("ИНН = {0}", this.Parameters));

                var entity = service.GetManOrgByInn(this.Parameters);
                if (entity == null)
                {
                    throw new ArgumentException("Не найдена УО с указанным ИНН");
                }

                var period =
                    periodDomain.GetAll()
                        .Where(x => x.Synchronizing && x.PeriodDi != null && x.State == ReportingPeriodStateEnum.current)
                        .OrderByDescending(x => x.DateStart)
                        .FirstOrDefault();

                var data = collector.CollectNewCompanyProfileData(entity, period?.PeriodDi);
                if (!data.Success)
                {
                    throw new Exception(data.Message);
                }

                this.Client.SetNewCompany(entity.Contragent.Inn, (NewCompanyProfileData)data.Data);
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(collector);
                this.Container.Release(periodDomain);
            }

            return null;
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
        public SetNewCompanyAction(IWindsorContainer container, ISyncProvider syncProvider, string parameters)
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
        public SetNewCompanyAction(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        #endregion
    }
}