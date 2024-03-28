namespace Bars.Gkh.Overhaul.Domain
{

    using Bars.B4;

    public interface IObjectCrIntegrationService
    {
        /// <summary>
        /// В объекте КР получаем список возможных на выбор объектов 
        /// </summary>
        IDataResult GetListWorksForObjectCr(BaseParams baseParams, ref int totalCount);

        /// <summary>
        /// В объекте КР получаем список возможных на выбор объектов 
        /// </summary>
        IDataResult AddWorks(BaseParams baseParams);
    }
}