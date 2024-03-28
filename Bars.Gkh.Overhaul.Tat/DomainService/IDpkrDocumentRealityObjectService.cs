namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using Bars.B4;

    public interface IDpkrDocumentRealityObjectService
    {
        /// <summary>
        /// Получить список домов
        /// </summary>
        IDataResult GetRealityObjectsList(BaseParams baseParams);
        
        /// <summary>
        /// Добавить дома
        /// </summary>
        IDataResult AddRealityObjects(BaseParams baseParams);
    }
}