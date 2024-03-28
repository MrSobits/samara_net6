namespace Bars.GkhDi.Services.Impl
{
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Services.DataContracts;

    public partial class Service
    {
        public GetFinEconInfoManOrgResponse GetFinEconInfoManOrg(string manOrgId, string periodId)
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberformat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                numberformat = ci.NumberFormat;
            }


            var idManOrg = manOrgId.ToLong();
            var idPeriod = periodId.ToLong();

            if (idManOrg != 0 && idPeriod != 0)
            {
                var disclosureInfoObj = Container.Resolve<IDomainService<DisclosureInfo>>()
                             .GetAll().FirstOrDefault(x => x.PeriodDi.Id == idPeriod && x.ManagingOrganization.Id == idManOrg);

                if (disclosureInfoObj != null)
                {
                    var financialActivities = Container.Resolve<IDomainService<FinActivityManagRealityObj>>()
                            .GetAll()
                            .Where(x => x.DisclosureInfo.Id == disclosureInfoObj.Id)
                           .Select(x => new FinancialActivity
                            {
                                Id = x.Id, 
                                Address = x.RealityObject.FiasAddress != null ? x.RealityObject.FiasAddress.AddressName : string.Empty,
                                TotalArea = x.RealityObject.AreaMkd.HasValue ? x.RealityObject.AreaMkd.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                Payment = x.PresentedToRepay.HasValue ? x.PresentedToRepay.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                GetForServices = x.ReceivedProvidedService.HasValue ? x.ReceivedProvidedService.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                SumDept = x.SumDebt.HasValue ? x.SumDebt.Value.RoundDecimal(2).ToString(numberformat) : string.Empty
                            })
                           .ToArray();

                    return new GetFinEconInfoManOrgResponse { FinancialActivities = financialActivities, Result = Result.NoErrors };
                }
            }

            return new GetFinEconInfoManOrgResponse { FinancialActivities = null, Result = Result.DataNotFound };
        }
    }
}
