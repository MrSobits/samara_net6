namespace Bars.GkhCalendar.ViewModel
{
    using Bars.B4;
    using Bars.GkhCalendar.Entities;

    public class DayViewModel : BaseViewModel<Day>
    {
        public override IDataResult Get(IDomainService<Day> domainService, BaseParams baseParams)
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