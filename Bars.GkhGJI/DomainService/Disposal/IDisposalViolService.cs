namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IDisposalViolService
    {
        /// <summary>
        /// Метод получения списка домов по которым есть нарушения
        /// </summary>
        IDataResult ListRealityObject(BaseParams baseParams);

        /// <summary>
        /// Метод добавления нарушений 
        /// </summary>
        IDataResult AddViolations(BaseParams baseParams);
    }
}
