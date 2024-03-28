namespace Bars.Gkh.Overhaul.Domain
{
    using System.Linq;

    using Bars.Gkh.Entities;

    public interface IDpkrRealityObjectService
    {
        /// <summary>
        /// Возвращает дома, которые могут попасть в дпкр
        /// </summary>
        /// <returns></returns>
        IQueryable<RealityObject> GetObjectsInDpkr();
    }
}