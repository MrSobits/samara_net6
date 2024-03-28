using Bars.B4;

namespace Bars.Gkh.Gis.DomainService.Register.HouseServiceRegister
{
    public interface IHouseServiceRegisterService
    {
        /// <summary>
        /// Список поставщиков
        /// </summary>
        IDataResult SupplierList(BaseParams baseParams);

        /// <summary>
        /// Список услуг
        /// </summary>
        IDataResult ServiceList(BaseParams baseParams);

        IDataResult ServiceGroupList(BaseParams baseParams);

        IDataResult SupplierListWithoutPaging(BaseParams baseParams);

        /// <summary>
        /// Список муниципальных районов
        /// </summary>
        IDataResult MunicipalAreaList(BaseParams baseParams);

        /// <summary>
        /// Список населенных пунктов
        /// </summary>
        IDataResult SettlementList(BaseParams baseParams);

        /// <summary>
        /// Список улиц/домов
        /// </summary>
        IDataResult StreetList(BaseParams baseParams);

        /// <summary>
        /// Список расхождений объемов
        /// </summary>
        IDataResult DiscrepancyList(BaseParams baseParams);

        /// <summary>
        /// Пометить данные как "Опубликовано"
        /// </summary>
        IDataResult Publish(BaseParams baseParams);
    }
}
