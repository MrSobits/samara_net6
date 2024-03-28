namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.DomainService.Impl;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public interface IPriorityService
    {
        IDataResult SetPriority(BaseParams baseParams);

        IDataResult SetPriorityAll(BaseParams baseParams);

        Dictionary<long, List<StoredPointParam>> GetPoints( IQueryable<IStage3Entity> stage3RecsQuery,
            IEnumerable<Stage2Proxy> stage2Query, IEnumerable<Stage1Proxy> stage1Query, long? versionId = null);

        void CalculateOrder(Stage3Order st3Oreder, IEnumerable<string> keys, object injections);

        void FillStage3Criteria(IStage3Entity st3Item, Dictionary<string, object> orderDict);
    }
}