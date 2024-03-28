namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public interface IRealObjectStructElementService
    {

        /// <summary>
        /// Получить износ конструктивного элемента дома
        /// </summary>
        /// <param name="roQuery"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IDictionary<long, decimal> GetRealityObjectWearoutDictionary(IQueryable<RealityObject> roQuery, string code);
    }
}