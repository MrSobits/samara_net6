namespace Bars.Gkh.ClaimWork.Services
{
    using B4;

    public interface IJurInstitutionService
    {
        /// <summary>
        /// Добавить жилые дома
        /// </summary>
        IDataResult AddRealObjs(BaseParams baseParams);

        /// <summary>
        /// Получить жилые дома для добавления
        /// </summary>
        IDataResult ListRealObj(StoreLoadParams storeParams);

        /// <summary>
        /// Поулчить добавленные жилые дома
        /// </summary>
        IDataResult ListRealObjByMunicipality(BaseParams baseParams);
    }
}