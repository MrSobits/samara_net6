namespace Bars.Gkh.Overhaul.DomainService
{
    using System.Linq;
    using Gkh.Entities;

    public class RecordObject
    {
        public long Id;
        public decimal? TotalCost;
    }

    public interface IRealityObjectsInPrograms
    {
        IQueryable<RealityObject> GetInShortProgram(int year);

        IQueryable<RealityObject> GetInPublishedProgram();

        IQueryable<RealityObject> GetInPublishedProgramByMunicipality(long[] muIds);

        IQueryable<RecordObject> GetShortProgramRecordData();
    }
}