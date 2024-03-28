namespace Bars.GkhDi.Services.Impl
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Enums;
    using Gkh.Services.DataContracts;

    public partial class Service
    {
        public GetAdminRespManOrgResponse GetAdminRespManOrg(string manOrgId, string periodId)
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
                var disclosureInfo = Container.Resolve<IDomainService<DisclosureInfo>>()
                             .GetAll().FirstOrDefault(x => x.PeriodDi.Id == idPeriod && x.ManagingOrganization.Id == idManOrg);

                if (disclosureInfo == null)
                {
                    return new GetAdminRespManOrgResponse { Result = Result.DataNotFound };
                }

                var adminResps = new List<AdminRespons>();

                var hasAdminResp = disclosureInfo.AdminResponsibility.GetEnumMeta().Display;

                if (disclosureInfo.AdminResponsibility == YesNoNotSet.Yes)
                {
                    adminResps = Container.Resolve<IDomainService<AdminResp>>()
                                .GetAll()
                                .Where(x => x.DisclosureInfo.Id == disclosureInfo.Id)
                                .Select(x => new AdminRespons
                                {
                                    Id = x.Id,
                                    ControlOrg = x.SupervisoryOrg.Name,
                                    ViolCount = x.AmountViolation.ToInt(),
                                    DatePayFine = x.DatePaymentPenalty.HasValue ? x.DatePaymentPenalty.Value.ToShortDateString() : null,
                                    FineDate = x.DateImpositionPenalty.HasValue ? x.DateImpositionPenalty.Value.ToShortDateString() : null,
                                    SumFine = x.SumPenalty.HasValue ? x.SumPenalty.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                }).ToList();
                }

                return new GetAdminRespManOrgResponse { AdminResponses = adminResps.ToArray(), HasAdminResp = hasAdminResp, Result = Result.NoErrors };
            }

            return new GetAdminRespManOrgResponse { Result = Result.DataNotFound };
        }
    }
}
