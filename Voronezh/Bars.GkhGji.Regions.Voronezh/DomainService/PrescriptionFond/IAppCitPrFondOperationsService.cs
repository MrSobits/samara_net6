namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    using Bars.B4;
    using Entities;
    /// <summary>
    /// Сервис расчета платы ЖКУ
    /// </summary>
    public interface IAppCitPrFondOperationsService
    { 
        IDataResult GetListObjectCr(BaseParams baseParams);
    }
}
