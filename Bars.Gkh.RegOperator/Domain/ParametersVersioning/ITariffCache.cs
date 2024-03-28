namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Decisions.Nso.Entities;
    using Gkh.Entities;
    using Gkh.Entities.RealEstateType;
    using Overhaul.Entities;

    /// <summary>
    /// Кеш тарифов
    /// </summary>
    public interface ITariffCache : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roQuery"></param>
        void Init(IQueryable<RealityObject> roQuery);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roIds"></param>
        void Init(IEnumerable<long> roIds);

        /// <summary>
        /// Получение размеров взносов по городским поселениям
        /// </summary>
        PaysizeRecord[] GetSettlementPaysizes(RealityObject robject);

        /// <summary>
        /// Получение размеров взносов по муниципальным районам
        /// </summary>
        PaysizeRecord[] GetMunicipalityPaysizes(RealityObject robject);

        /// <summary>
        /// Получение размеров взносов по городскому поселению, сгруппированные по типу дома
        /// </summary>
        Dictionary<long, PaysizeRealEstateType[]> GetSettlementPaysizesByType(RealityObject robject);

        /// <summary>
        /// Получение размеров взносов по муниципальному району, сгруппированные по типу дома
        /// </summary>
        Dictionary<long, PaysizeRealEstateType[]> GetMunicipalityPaysizesByType(RealityObject robject);

        /// <summary>
        /// Получение типов домов
        /// </summary>
        RealEstateTypeRealityObject[] GetRoTypes(RealityObject robject);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Tuple<DateTime, PeriodMonthlyFee[]>[] GetDecisions(RealityObject robject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="robject"></param>
        /// <returns></returns>
        Dictionary<long, long> GetEntrances(RealityObject robject);
    }
}