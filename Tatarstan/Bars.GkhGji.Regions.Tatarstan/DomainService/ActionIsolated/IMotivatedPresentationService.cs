namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated
{
    using Bars.B4;

    public interface IMotivatedPresentationService
    {
        /// <summary>
        /// Получить сведения о проверках
        /// </summary>
        IDataResult GetInspectionInfoList(BaseParams baseParams);

        /// <summary>
        /// Получить перечень нарушений
        /// </summary>
        IDataResult GetViolationInfoList(BaseParams baseParams);
        
        /// <summary>
        /// Получить сведения-основание для новой проверки
        /// </summary>
        IDataResult GetNewInspectionBasementInfo(BaseParams baseParams);

        /// <summary>
        /// Получить список мотивированных представлений для реестра документов ГЖИ
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список документов</returns>
        /// <returns></returns>
        IDataResult ListForRegistry(BaseParams baseParams);
    }
}