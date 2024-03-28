namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits;
    
    /// <summary>
    /// Сервис для "Мотивированное представление по обращению гражданина"
    /// </summary>
    public interface IMotivatedPresentationAppealCitsService
    {
        /// <summary>
        /// Получить Мотивированное представление по обращениям граждан
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Мотивированное представление по обращениям граждан</returns>
        IDataResult Get(BaseParams baseParams);
        
        /// <summary>
        /// Получить список для <see cref="MotivatedPresentationAppealCits"/>
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список мотивированных представлений по обращениям граждан</returns>
        IDataResult List(BaseParams baseParams);
        
        /// <summary>
        /// Список мотивированных представлений по обращениям граждан для реестра документов ГЖИ
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список документов</returns>
        IDataResult ListForRegistry(BaseParams baseParams);
    }
}