namespace Bars.GkhDi.Services.Impl
{
    using System.Linq;
    using B4;
    using DataContracts.GetPeriods;
    using Entities;
    using Gkh.Services.DataContracts;

    public partial class Service
    {
        public GetPeriodsResponse GetPeriods()
        {
            var periods =
                Container.Resolve<IDomainService<PeriodDi>>()
                         .GetAll()
                         .Select(
                             x =>
                             new Period
                                 {
                                     Id = x.Id,
                                     Name = x.Name,
                                     StartDate =
                                         x.DateStart.HasValue ? x.DateStart.Value.ToShortDateString() : null,
                                     FinishDate = x.DateEnd.HasValue ? x.DateEnd.Value.ToShortDateString() : null
                                 })
                         .ToArray();

            var result = periods.Length == 0 ? Result.DataNotFound : Result.NoErrors;
            return new GetPeriodsResponse { Periods = periods, Result = result };
        }
    }
}