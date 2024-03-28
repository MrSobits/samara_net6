namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public interface IStage2Service
    {
        IDataResult MakeStage2(BaseParams baseParams);

        IDataResult CopyFromVersion(BaseParams baseParams);

        /// <summary>
        /// Расчет записей первого этапа
        /// </summary>
        IEnumerable<RealityObjectStructuralElementInProgramm> GetStage1(
            int startYear, int endYear, long municipalityId,
            IQueryable<RealityObjectStructuralElement> roStructElQuery = null,
            IDictionary<long, int> structElementPlanYearsDict = null);

        /// <summary>
        /// Расчет записей второго и третьего этапов
        /// </summary>
        void GetStage2And3(
            IEnumerable<RealityObjectStructuralElementInProgramm> stage1Records,
            out IList<RealityObjectStructuralElementInProgrammStage2> stage2Records,
            out IList<RealityObjectStructuralElementInProgrammStage3> stage3Records);
    }
}
