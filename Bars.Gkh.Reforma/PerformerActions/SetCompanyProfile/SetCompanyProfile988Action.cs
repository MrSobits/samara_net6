namespace Bars.Gkh.Reforma.PerformerActions.SetCompanyProfile
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.DataCollectors;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    /// <summary>
    ///     Действие обновления профиля УО (988)
    /// </summary>
    public class SetCompanyProfile988Action : LoggableSyncActionBase<SetCompanyProfileParams, object>
    {
        #region Constants

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "SetCompanyProfile988";

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
        /// <summary>
        /// Запуск действия
        /// </summary>
        /// <returns></returns>
        protected override object Execute()
        {
            var collector = this.Container.Resolve<IManOrg988DataCollector>();
            var manOrgService = this.Container.ResolveDomain<ManagingOrganization>();
            var periodService = this.Container.ResolveDomain<PeriodDi>();
            var reportingPeriodService = this.Container.ResolveDomain<ReportingPeriodDict>();
            try
            {
                var manOrg = manOrgService.Get(this.Parameters.ManagingOrganizationId);
                if (manOrg == null)
                {
                    return new BaseDataResult(false, string.Format("Не найдена УО с Id = {0}", this.Parameters.ManagingOrganizationId));
                }

                var period = periodService.Get(this.Parameters.PeriodId);
                if (period == null)
                {
                    return new BaseDataResult(false, string.Format("Не найден период раскрытия информации с Id = {0}", this.Parameters.PeriodId));
                }

                var reportingPeriod = reportingPeriodService.GetAll().FirstOrDefault(x => x.PeriodDi == period);

                var inn = manOrg.Contragent.Inn;
                this.Logger.SetActionDetails(string.Format("УО = {0} ИНН = {1} Период = {2}", manOrg.Contragent.Name, inn, period.Name));

                CompanyProfileData988 currentProfile;
                try
                {
                    currentProfile = this.Client.GetCompanyProfile988(inn, reportingPeriod.ExternalId).company_profile_data;
                }
                catch
                {
                    currentProfile = new CompanyProfileData988();
                }

                var result = collector.CollectCompanyProfile988Data(currentProfile, manOrg, period);
                if (!result.Success)
                {
                    throw new Exception(result.Message);
                }

                var collectionResult = result.Data;
                if (collectionResult.CollectedFiles.Length > 0)
                {
                    foreach (var collectedFile in collectionResult.CollectedFiles)
                    {
                        collectedFile.Process(collectionResult.ProfileData, this.SyncProvider);
                    }
                }

                this.Client.SetCompanyProfile988(inn, this.Parameters.PeriodExternalId, collectionResult.ProfileData);

                return manOrg.Id;
            }
            finally
            {
                this.Container.Release(collector);
                this.Container.Release(manOrgService);
                this.Container.Release(periodService);
                this.Container.Release(reportingPeriodService);
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
        public SetCompanyProfile988Action(IWindsorContainer container, ISyncProvider syncProvider, SetCompanyProfileParams parameters)
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
        public SetCompanyProfile988Action(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        #endregion
    }
}