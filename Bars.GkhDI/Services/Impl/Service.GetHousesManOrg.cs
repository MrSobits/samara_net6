namespace Bars.GkhDi.Services.Impl
{
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo;

    using DataContracts.HousesManOrg;
    using Entities;
    using Gkh.Entities;
    using Gkh.Services.DataContracts;

    public partial class Service
    {
        public GetHousesManOrgResponse GetHousesManOrg(string manOrgId, string periodId)
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

            House[] housesManOrg = null, noHousesManOrg = null;
            if (idManOrg != 0 && idPeriod != 0)
            {
                var period = Container.Resolve<IDomainService<PeriodDi>>().GetAll().FirstOrDefault(x => x.Id == idPeriod);

                if (period != null)
                {
                    var houses =
                        Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                            .Where(x => x.ManOrgContract.ManagingOrganization.Id == idManOrg &&
                                (!x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate < period.DateEnd))
                            .Select(
                                x =>
                                    new House
                                    {
                                        Address = x.RealityObject.Address,
                                        Id = x.RealityObject.Id,
                                        _FinishDate = x.ManOrgContract.EndDate,
                                        _StartDate = x.ManOrgContract.StartDate,
                                        FinishDate = x.ManOrgContract.EndDate.HasValue ? x.ManOrgContract.EndDate.Value.ToShortDateString() : null,
                                        StartDate = x.ManOrgContract.StartDate.HasValue ? x.ManOrgContract.StartDate.Value.ToShortDateString() : null,
                                        AreaRooms = x.RealityObject.AreaMkd.HasValue
                                            ? x.RealityObject.AreaMkd.Value.RoundDecimal(2).ToString(numberformat)
                                            : string.Empty,
                                        DocumentDate = x.ManOrgContract.DocumentDate.HasValue
                                            ? x.ManOrgContract.DocumentDate.Value.ToShortDateString()
                                            : null,
                                        FileInfo = x.ManOrgContract.FileInfo != null
                                            ? new FileInfo
                                            {
                                                IdFile = x.ManOrgContract.FileInfo.Id.ToString(),
                                                Name = x.ManOrgContract.FileInfo.Name,
                                                Extention = x.ManOrgContract.FileInfo.Extention
                                            }
                                            : null
                                    })
                            .ToArray();

                    noHousesManOrg = houses.Where(x => x._FinishDate.HasValue && x._FinishDate < period.DateStart).ToArray();
                    housesManOrg = houses.Except(noHousesManOrg).ToArray();

                    housesManOrg = housesManOrg.Length == 0 ? null : housesManOrg;
                    noHousesManOrg = noHousesManOrg.Length == 0 ? null : noHousesManOrg;
                }
            }

            var result = housesManOrg == null && noHousesManOrg == null ? Result.DataNotFound : Result.NoErrors;
            return new GetHousesManOrgResponse
                       {
                           HousesManOrg = housesManOrg,
                           NoHousesManOrg = noHousesManOrg,
                           Result = result
                       };
        }
    }
}