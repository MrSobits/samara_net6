using Bars.B4;

namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Entities;

    public interface IHmaoRealEstateTypeService
    {
        /// <summary>
        /// Метод возвращает собираемость по домам за год (если нет тарифа берет тариф максимально близкого прошедшего года) 
        /// </summary>
        IDictionary<long, decimal> GetCollectionByYear(IQueryable<RealityObject> roQuery, int year);


        /// <summary>
        /// Метод возвращает собираемость по домам за период (если нет тарифа берет тариф максимально близкого прошедшего года) 
        /// </summary>
        IDictionary<long, decimal> GetCollectionByPeriod(IQueryable<RealityObject> roQuery, int startYear, int endYear);

        /// <summary>
        /// Метод возвращает муниципальные образования по настройке уровня МО для типа дома
        /// </summary>
        IDataResult GetMuList(BaseParams baseParams);
    }
}