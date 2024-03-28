namespace Bars.GkhCalendar.ViewModel
{
    using Bars.B4;
    using Bars.GkhCalendar.Entities;
    using System.Linq;

    public class AppointmentQueueViewModel : BaseViewModel<AppointmentQueue>
    {
        public override IDataResult Get(IDomainService<AppointmentQueue> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.TypeOrganisation,
                    x.RecordTo,
                    x.Description,
                    x.TimeSlot

                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}