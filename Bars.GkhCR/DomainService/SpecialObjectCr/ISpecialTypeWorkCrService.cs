namespace Bars.GkhCr.DomainService
{
    using System.Collections;
    using Bars.B4;

    public interface ISpecialTypeWorkCrService
    {
        IDataResult ListRealityObjectWorksByPeriod(BaseParams baseParams);

        IDataResult CalcPercentOfCompletion(BaseParams baseParams);

        IList ListByProgramCr(BaseParams baseParams, bool isPaging, ref int totalCount);

        IDataResult CreateTypeWork(BaseParams baseParams);
        
        IDataResult ListWorksCr(BaseParams baseParams);

        /// <summary>
        /// Получение в видах работ объекта КР только тех источников финансирования которые есть у программы данного объекта КР
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListFinanceSources(BaseParams baseParams);
    }
}
