namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities.Hcs;
    
    public class MeterReadingViewModel : BaseViewModel<MeterReading>
    {
        public override IDataResult List(IDomainService<MeterReading> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var accountId = baseParams.Params.GetAs<long>("accountId");
            var month = baseParams.Params.GetAs<long>("month");
            var year = baseParams.Params.GetAs<long>("year");

            //Если дата не задана - берем текущий год/месяц
            if (year == 0)
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
            }

            var data = domain.GetAll()
                .Where(x => x.Account.Id == accountId 
                    && (x.CurrentReadingDate.Month == month || month == 0) 
                    && x.CurrentReadingDate.Year == year)
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}