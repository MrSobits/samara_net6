namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using B4;

    public interface INsoRealEstateTypeService
    {
        /// <summary>
        /// Метод возвращает муниципальные образования по настройке уровня МО для типа дома
        /// </summary>
        IDataResult GetMuList(BaseParams baseParams);
    }
}