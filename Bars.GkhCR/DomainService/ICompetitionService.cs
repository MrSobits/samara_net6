namespace Bars.GkhCr.DomainService
{
    using System.Collections;
    using B4;

    /// <summary>
    /// Сервис конкурсов на проведение КР
    /// </summary>
    public interface ICompetitionService
    {
        /// <summary>
        /// Получить список конкурсов
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="isPaging"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount);
    }
}