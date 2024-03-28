namespace Bars.Gkh.Overhaul.Hmao.DomainService.Version
{
    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Interface for <see cref="DpkrDocumentRealityObject"/>
    /// </summary>
    public interface IDpkrDocumentRealityObjectService
    {
        /// <summary>
        /// Добавление домов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult AddRealityObjects(BaseParams baseParams);
    }
}