namespace Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction
{
    using Bars.B4;

    public interface IPreventiveActionTaskRegulationService
    {
        /// <summary>
        /// Создать НПА
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult AddNormativeDocs(BaseParams baseParams);
    }
}