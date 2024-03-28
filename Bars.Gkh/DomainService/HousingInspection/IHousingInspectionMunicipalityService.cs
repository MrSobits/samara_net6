namespace Bars.Gkh.DomainService
{
    using B4;

    /// <summary>
    /// Сервис для работы с мунципальными образованиями жилищной инспекции
    /// </summary>
    public interface IHousingInspectionMunicipalityService
    {
        IDataResult AddMunicipalities(BaseParams baseParams);
    }
}
