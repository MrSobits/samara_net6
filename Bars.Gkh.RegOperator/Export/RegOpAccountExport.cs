namespace Bars.Gkh.RegOperator.Export
{
    using System.Collections;
    using System.Linq;
    using B4.Utils;
    using B4;
    using B4.Modules.DataExport.Domain;

    using Bars.Gkh.Modules.RegOperator.DomainService;

    using Entities;

    public class RegOpAccountExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var regOperatorId = loadParams.Filter.GetAs<long>("regOpId");

            var regopService = this.Container.Resolve<IRegopService>();
            var regopCalcAccountDomain = this.Container.Resolve<IDomainService<RegopCalcAccount>>();
            try
            {
                var contragentId = regopService.GetRegopContragent(regOperatorId).Return(x => x.Id);

                return regopCalcAccountDomain.GetAll()
                        .Where(x => x.AccountOwner.Id == contragentId)
                        .Select(x => new
                        {
                            x.Id,
                            ContragentBank = x.ContragentCreditOrg.CreditOrg.Name,
                            CreditOrg = "CreditOrg",
                            x.DateOpen,
                            x.DateClose,
                            x.TotalIn,
                            x.TotalOut,
                            x.BalanceIn
                        })
                        .Filter(loadParams, Container)
                        .Order(loadParams)
                        .ToList();
            }
            finally
            {
                this.Container.Release(regopCalcAccountDomain);
            }
        }
    }
}
