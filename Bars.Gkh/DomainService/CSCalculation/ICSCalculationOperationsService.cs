namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Entities;
    /// <summary>
    /// Сервис расчета платы ЖКУ
    /// </summary>
    public interface ICSCalculationOperationsService
    {
        object CalculateCS(BaseParams baseParams);

        IDataResult GetListRoom(BaseParams baseParams);

        IDataResult AddCategoryes(BaseParams baseParams);

        IDataResult AddTarifs(BaseParams baseParams);

        IDataResult AddCoefficient(BaseParams baseParams);

    }
}
