namespace Bars.Gkh.Regions.Tatarstan.ViewModel.RisDebtInfo
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities.RisDebtInfo;
    using Bars.Gkh.Regions.Tatarstan.Services.ServicesContracts.RisDebtInfo;

    public class DebtInfoViewModel : BaseViewModel<DebtInfo>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<DebtInfo> domainService, BaseParams baseParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            using (this.Container.Using(userManager))
            {
                var gkhOperator = userManager.GetActiveOperator();

                if (gkhOperator == null)
                {
                    return new BaseDataResult();
                }

                var debtInfoService = this.Container.Resolve<IDebtSubRequestInformationService>();
                using (this.Container.Using(debtInfoService))
                {
                    return debtInfoService.GetDebtInfo(gkhOperator.RisToken);
                }
            }
        }
    }
}