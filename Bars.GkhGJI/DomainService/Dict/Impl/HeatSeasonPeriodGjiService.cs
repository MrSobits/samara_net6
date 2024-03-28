using System;
using System.Linq;
using Bars.B4;
using Bars.GkhGji.Entities;
using Castle.Windsor;

namespace Bars.GkhGji.DomainService
{
    public class HeatSeasonPeriodGjiService : IHeatSeasonPeriodGjiService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetCurrentPeriod()
        {
            var period = Container.Resolve<IDomainService<HeatSeasonPeriodGji>>().GetAll()
                    .Where(x => x.DateStart.HasValue && x.DateStart.Value.Date <= DateTime.Now.Date && (!x.DateEnd.HasValue || x.DateEnd.Value.Date >= DateTime.Now.Date))
                    .OrderByDescending(x => x.ObjectCreateDate)
                    .FirstOrDefault();

            if (period != null)
            {
                return new BaseDataResult(new { periodId = period.Id, periodName = period.Name });
            }

            return new BaseDataResult();
        }
    }
}