namespace Bars.Gkh.Overhaul.DomainService
{
    using System.IO;

    using Bars.B4;

    public interface ICommonEstateObjectService
    {
        /// <summary>
        /// Добавить работы
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult AddWorks(BaseParams baseParams);

        /// <summary>
        /// Печать
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        Stream PrintReport(BaseParams baseParams);

        /// <summary>
        /// Получить ООИ по дому
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListForRealObj(BaseParams baseParams);

        /// <summary>
        /// Добавить нарушения
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult AddFeatureViol(BaseParams baseParams);
    }
}
