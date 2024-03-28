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
    ///     Действие обновления профиля УО
    /// </summary>
    public class SetCompanyProfileAction : LoggableSyncActionBase<SetCompanyProfileParams, object>
    {
        #region Constants

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "SetCompanyProfile";

        #endregion

        #region Public Properties

        /// <summary>
        /// Идентификатор действия
        /// </summary>
        public override string Id
        {
            get
            {
                return SetCompanyProfileAction.ActionId;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Выполнить действие
        /// </summary>
        /// <returns></returns>
        protected override object Execute()
        {
            var collector = this.Container.Resolve<IManOrgDataCollector>();
            var manOrgService = this.Container.ResolveDomain<ManagingOrganization>();
            var periodService = this.Container.ResolveDomain<PeriodDi>();

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

                var inn = manOrg.Contragent.Inn;
                this.Logger.SetActionDetails(string.Format("УО = {0} ИНН = {1} Период = {2}", manOrg.Contragent.Name, inn, period.Name));

                CompanyProfileData currentProfile;
                try
                {
                    currentProfile = this.Client.GetCompanyProfile(inn, this.Parameters.PeriodExternalId).company_profile_data;
                }
                catch
                {
                    currentProfile = new CompanyProfileData();
                }

                var result = collector.CollectCompanyProfileData(currentProfile, manOrg, period);
                if (!result.Success)
                {
                    throw new Exception(result.Message);
                }

                var profile = result.Data.ProfileData;
                var collectedFiles = result.Data.CollectedFiles;

                if (collectedFiles.Length > 0)
                {
                    foreach (var collectedFile in collectedFiles)
                    {
                        collectedFile.Process(profile, this.SyncProvider);
                    }
                }

                this.Client.SetCompanyProfile(inn, this.Parameters.PeriodExternalId, profile);

                return manOrg.Id;
            }
            finally
            {
                this.Container.Release(collector);
                this.Container.Release(manOrgService);
                this.Container.Release(periodService);
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
        public SetCompanyProfileAction(IWindsorContainer container, ISyncProvider syncProvider, SetCompanyProfileParams parameters)
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
        public SetCompanyProfileAction(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        #endregion
    }
}