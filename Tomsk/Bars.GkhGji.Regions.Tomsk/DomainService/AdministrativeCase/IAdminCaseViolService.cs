namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IAdminCaseViolService
    {
        /// <summary>
        /// Метод добавления нарушений 
        /// </summary>
        IDataResult AddViolations(BaseParams baseParams);
    }
}
