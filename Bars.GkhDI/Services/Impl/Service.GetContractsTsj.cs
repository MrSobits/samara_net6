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
        public GetContractsTsjResponse GetContractsTsj(string houseId, string periodId)
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberformat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                numberformat = ci.NumberFormat;
            }

            var idHouse = houseId.ToLong();
            var idPeriod = periodId.ToLong();

            if (idHouse != 0 && idPeriod != 0)
            {
                var contracts =
                    Container.Resolve<IDomainService<InformationOnContracts>>()
                             .GetAll()
                             .Where(x => x.RealityObject.Id == idHouse && x.DisclosureInfo.PeriodDi.Id == idPeriod)
                             .Select(
                                 x =>
                                 new Contract
                                     {
                                         Id = x.Id,
                                         NameManOrg = x.DisclosureInfo.ManagingOrganization.Contragent.Name,
                                         Name = x.Name,
                                         Number = x.Number,
                                         StartDate = x.DateStart.HasValue ? x.DateStart.Value.ToShortDateString() : null,
                                         FinishDate = x.DateEnd.HasValue ? x.DateEnd.Value.ToShortDateString() : null,
                                         Member = x.PartiesContract,
                                         Sum = x.Cost.HasValue ? x.Cost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                         Comments = x.Comments
                                     })
                             .ToArray();

                return new GetContractsTsjResponse { Contracts = contracts, Result = Result.NoErrors };
            }

            return new GetContractsTsjResponse { Result = Result.DataNotFound };
        }
    }
}
