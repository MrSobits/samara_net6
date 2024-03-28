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
        public GetFundsInfoResponse GetFundsInfo(string manOrgId, string periodId)
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
                    return new GetFundsInfoResponse { Result = Result.DataNotFound };
                }

                var data  = Container.Resolve<IDomainService<FundsInfo>>()
                         .GetAll()
                         .Where(x => x.DisclosureInfo.Id == disclosureInfo.Id)
                         .Select(
                             x =>
                             new Fund
                                 {
                                     Id = x.Id,
                                     Name = x.DocumentName,
                                     Date = x.DocumentDate.HasValue ? x.DocumentDate.Value.ToShortDateString() : null,
                                     Size = x.Size.HasValue ? x.Size.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                 })
                         .ToArray();

                return new GetFundsInfoResponse { Funds = data, Result = Result.NoErrors };
            }

            return new GetFundsInfoResponse { Result = Result.DataNotFound };
        }
    }
}
