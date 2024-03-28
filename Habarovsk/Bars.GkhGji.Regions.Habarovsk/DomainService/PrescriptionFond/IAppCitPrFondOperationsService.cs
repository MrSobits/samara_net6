namespace Bars.GkhGji.Regions.Habarovsk.DomainService
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
