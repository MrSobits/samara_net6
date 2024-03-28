namespace Bars.GkhCalendar.DomainService
{
    using Bars.B4;

    public interface IDayService
    {
        IDataResult GetDays(BaseParams baseParams);
    }
}