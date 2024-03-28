namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities.Hcs;

    public class HouseMeterReadingViewModel : BaseViewModel<HouseMeterReading>
    {
        public override IDataResult List(IDomainService<HouseMeterReading> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var realtyObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var month = baseParams.Params.GetAs<long>("month");
            var year = baseParams.Params.GetAs<long>("year");

            //Если дата не задана - берем текущий год/месяц
            if (year == 0)
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
            }

            var data = domain.GetAll()
                .WhereIf(month > 0, x => x.CurrentReadingDate.Month == month)
                .Where(x => x.RealityObject.Id == realtyObjectId)
                .Where(x => x.CurrentReadingDate.Year == year)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}