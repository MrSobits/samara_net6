namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public interface IResolutionService
    {
        IDataResult GetInfo(long? documentId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams">
        /// dateStart - Необходимо получить документы больше даты начала
        /// dateEnd - Необходимо получить документы меньше даты окончания
        /// realityObjectId - Необходимо получить документы по дому
        /// </param>
        /// <param name="paging">Постраничный вывод</param>
        IDataResult ListView(BaseParams baseParams, bool paging = true);

        IQueryable<ViewResolution> GetViewList();

        /// <summary>
        /// Получение распоряжений для раскрытия
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetResolutionInfo(BaseParams baseParams);
        
        
        string GetTakingDecisionAuthorityName();
    }
}