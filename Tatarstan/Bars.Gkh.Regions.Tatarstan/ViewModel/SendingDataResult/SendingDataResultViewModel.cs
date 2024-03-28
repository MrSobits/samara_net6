namespace Bars.Gkh.Regions.Tatarstan.ViewModel.SendingDataResult
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Regions.Tatarstan.Entities.SendingDataResult;
    using Bars.Gkh.Regions.Tatarstan.Services.ServicesContracts.SendingDataResult;

    public class SendingDataResultViewModel : BaseViewModel<SendingDataResult>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<SendingDataResult> domainService, BaseParams baseParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            using (this.Container.Using(userManager))
            {
                var gkhOperator = userManager.GetActiveOperator();

                if (gkhOperator == null)
                {
                    return new BaseDataResult();
                }

                var sendingDataResultService = this.Container.Resolve<ISendingDataResultService>();
                using (this.Container.Using(sendingDataResultService))
                {
                    return sendingDataResultService.GetSendingDataResult(gkhOperator.RisToken);
                }
            }
        }
    }
}