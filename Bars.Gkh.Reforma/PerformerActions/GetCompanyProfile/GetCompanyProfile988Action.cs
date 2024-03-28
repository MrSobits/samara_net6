namespace Bars.Gkh.Reforma.PerformerActions.GetCompanyProfile
{
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.ReformaService;

    using Castle.Windsor;

    /// <summary>
    ///     Получение профиля УО
    /// </summary>
    public class GetCompanyProfile988Action : LoggableSyncActionBase<GetCompanyProfileParams, CompanyProfileData988>
    {
        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "GetCompanyProfile988";

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
        public GetCompanyProfile988Action(IWindsorContainer container, ISyncProvider syncProvider, GetCompanyProfileParams parameters)
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
        public GetCompanyProfile988Action(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        public override string Id
        {
            get
            {
                return ActionId;
            }
        }

        protected override CompanyProfileData988 Execute()
        {
            Logger.SetActionDetails(string.Format("ИНН = {0}, Период = {1}", Parameters.Inn, Parameters.PeriodExternalId));
            return Client.GetCompanyProfile988(Parameters.Inn, Parameters.PeriodExternalId).company_profile_data;
        }
    }
}