namespace Bars.GkhCalendar.ViewModel
{
    using Bars.B4;
    using Bars.GkhCalendar.Entities;
    using System.Linq;

    public class AppointmentDiffDayViewModel : BaseViewModel<AppointmentDiffDay>
    {
        public override IDataResult Get(IDomainService<AppointmentDiffDay> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var id = loadParams.Filter.GetAs("Id", 0L);
            var data = domainService.GetAll()
                .Where(x => x.AppointmentQueue != null && x.AppointmentQueue.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.AppointmentQueue,
                    Day = x.Day.DayDate,
                    x.StartTime,
                    x.EndTime,
                    x.StarPauseTime,
                    x.EndPauseTime,
                    x.ChangeAppointmentDay
        })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}