namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Decision
{
    using Bars.B4;

    public interface IDecisionInspBaseService
    {
        /// <summary>
        /// Получаем список объектов InspectionBaseType
        /// </summary>
        /// <param name="baseParams">В параметрах принимаем kindCheckId и recordsId</param>
        IDataResult ListInspectionBaseType(BaseParams baseParams);
    }
}