namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.DomainService.Impl;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Сервис расчета очередности
    /// </summary>
    public interface IPriorityService
    {
        /// <summary>
        /// Рассчитать очередность
        /// </summary>
        IDataResult SetPriority(BaseParams baseParams);

        /// <summary>
        /// Рассчитать очередность по всем МО
        /// </summary>
        IDataResult SetPriorityAll(BaseParams baseParams);

        /// <summary>
        /// Получить баллы
        /// </summary>
        Dictionary<long, List<StoredPointParam>> GetPoints(long muId, IQueryable<IStage3Entity> stage3RecsQuery,
            IEnumerable<Stage2Proxy> stage2Query, IEnumerable<Stage1Proxy> stage1Query);

        /// <summary>
        /// Рассчитать порядок
        /// </summary>
        void CalculateOrder(Stage3Order st3Oreder, IEnumerable<string> keys, object injections);

        /// <summary>
        /// Заполнить критерии 3 этапа
        /// </summary>
        void FillStage3Criteria(IStage3Entity st3Item, Dictionary<string, object> orderDict);
    }
}