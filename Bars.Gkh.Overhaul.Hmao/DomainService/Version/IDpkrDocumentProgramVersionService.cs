namespace Bars.Gkh.Overhaul.Hmao.DomainService.Version
{
    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Interface for <see cref="DpkrDocumentProgramVersion"/>
    /// </summary>
    public interface IDpkrDocumentProgramVersionService
    {
        /// <summary>
        /// Список версий программ
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult GetProgramVersionList(BaseParams baseParams);

        /// <summary>
        /// Добавление ссылок на версии
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult AddProgramVersions(BaseParams baseParams);
    }
}