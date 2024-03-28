namespace Bars.Gkh.Overhaul.DomainService
{
    using System.Linq;
    using Gkh.Entities;

  public interface IRealityObjectsProgramVersion
    {
        IQueryable<RealityObject> GetMainVersionRealityObjects();
    }
}