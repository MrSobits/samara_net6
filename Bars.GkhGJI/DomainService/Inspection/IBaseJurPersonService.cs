namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IBaseJurPersonService
    {
        IDataResult GetInfo(BaseParams baseParams);

        IDataResult GetStartFilters();

        /// <summary>
        /// Заполнить порядковый номер в плане у всех записей
        /// </summary>
        IDataResult FillPlanNumber();
    }
}