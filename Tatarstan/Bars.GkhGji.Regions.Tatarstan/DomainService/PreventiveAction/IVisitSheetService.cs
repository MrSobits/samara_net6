namespace Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// Интерфейс сервиса для <see cref="VisitSheet"/>
    /// </summary>
    public interface IVisitSheetService
    {
        /// <summary>
        /// Получить МО для документа "Лист визита"
        /// </summary>
        IDataResult GetVisitSheetMunicipality(BaseParams baseParams);

        /// <summary>
        /// Получить список домов из Нарушений по документу "Лист визита"
        /// </summary>
        IDataResult GetViolationRealityObjectsList(BaseParams baseParams);

        /// <summary>
        /// Получить список для реестра документов ГЖИ
        /// </summary>
        IDataResult ListForRegistry(BaseParams baseParams);
    }
}