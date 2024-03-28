namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Decision
{
    using Bars.B4;

    public interface IDecisionControlObjectInfoService
    {
        /// <summary>
        /// Получаем коллекцию объектов ControlObjectKind 
        /// </summary>
        /// <param name="baseParams">Принимаем параметр controlTypeId</param>
        /// <returns></returns>
        IDataResult ListControlObjectKind(BaseParams baseParams);
        
        /// <summary>
        /// Получаем коллекцию объектов InspectionGjiRealityObject 
        /// </summary>
        /// <param name="baseParams">Принимаем параметры recordIds и inspectionId</param>
        /// <returns></returns>
        IDataResult ListInspGjiRealityObject(BaseParams baseParams);
    }
}