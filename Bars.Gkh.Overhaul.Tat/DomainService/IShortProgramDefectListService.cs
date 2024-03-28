namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using Bars.B4;

    public interface IShortProgramDefectListService
    {
        /// <summary>
        /// метод получения работ конкретно для объекта
        /// и тех которых еще несуществует
        /// </summary>
        IDataResult GetWorks(BaseParams baseParams);
    }
}
