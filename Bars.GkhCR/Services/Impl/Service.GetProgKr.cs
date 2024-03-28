namespace Bars.GkhCr.Services.Impl
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Services.DataContracts;

    public partial class Service
    {
        public GetProgKrResponse GetProgKr(string houseId, string usedInExport)
        {
            var useInExport = usedInExport.ToBool();
            var roId = houseId.ToLong();
            var programCrDomain = this.Container.ResolveDomain<ProgramCr>();
            var objectCrDomain = this.Container.ResolveDomain<ObjectCr>();

            try
            {
                var programIds = objectCrDomain.GetAll().Where(x => x.RealityObject.Id == roId).Select(x => x.ProgramCr.Id).ToArray();

                var programsCr = programCrDomain
                    .GetAll()
                    .WhereIf(useInExport, x => x.UsedInExport)
                    .WhereIf(roId != 0, x => programIds.Contains(x.Id))
                    .Select(
                        x => new ProgKr
                        {
                            Id = x.Id,
                            Name = x.Name,
                            StartDate = x.Period.DateStart.ToShortDateString(),
                            FinishDate =
                                x.Period.DateEnd.HasValue ? x.Period.DateEnd.Value.ToShortDateString() : string.Empty
                        })
                    .ToArray();

                return programsCr.Length > 0
                    ? new GetProgKrResponse {ProgKrs = programsCr, Result = Result.NoErrors}
                    : new GetProgKrResponse {Result = Result.DataNotFound};
            }
            finally
            {
                this.Container.Release(programCrDomain);
                this.Container.Release(objectCrDomain);
            }
        }
    }
}