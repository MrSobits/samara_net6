namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using Bars.B4;
    using Entities;

    public class TaskCalendarViewModel : BaseViewModel<TaskCalendar>
    {
        public override IDataResult Get(IDomainService<TaskCalendar> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var day = domainService.Get(id);

            return new BaseDataResult(new
            {
                day.Id,
                DayDate = day.DayDate.ToString("d MMMM yyyy"),
                day.DayType
            });
        }
    }
}