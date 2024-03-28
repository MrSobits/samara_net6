namespace Bars.GkhDi.MetaValueConstructor.DataFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.EfficiencyRating;
    using Bars.Gkh.Enums;
    using Bars.Gkh.MetaValueConstructor;
    using Bars.Gkh.MetaValueConstructor.DataFillers;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Площадь управляемого жилья
    /// </summary>
    public class AreaManagedPropertyDataFiller : BaseManagingOrganizationDataFiller
    {
        private IDictionary<long, decimal> totalAreaDict;

        /// <summary>
        /// Домен-сервис <see cref="ManOrgContractRealityObject"/>
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomainService { get; set; }

        /// <inheritdoc />
        public override void PrepareCache(BaseParams baseParams)
        {
            base.PrepareCache(baseParams);

            this.totalAreaDict = this.ManOrgContractRealityObjectDomainService.GetAll()
                .Where(x => this.ManorgQuery.Any(y => y.Id == x.ManOrgContract.ManagingOrganization.Id))
                .Where(x => x.RealityObject.TypeHouse != TypeHouse.BlockedBuilding && x.RealityObject.TypeHouse != TypeHouse.Individual)
                .Where(x =>
                        x.RealityObject.ConditionHouse == ConditionHouse.Serviceable 
                        || x.RealityObject.ConditionHouse == ConditionHouse.Emergency
                        || x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated && !x.RealityObject.ResidentsEvicted)
                .Select(
                    x =>
                        new ManorgContractProxy
                        {
                            Id = x.ManOrgContract.ManagingOrganization.Id,
                            AreaMkd = x.RealityObject.AreaMkd,
                            StartDate = x.ManOrgContract.StartDate,
                            EndDate = x.ManOrgContract.EndDate
                        })
                .Where(this.period.CreateContainsExpression<ManorgContractProxy>(x => x.StartDate ?? DateTime.MinValue, x => x.EndDate ?? DateTime.MaxValue))
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.AreaMkd ?? 0M));
        }

        /// <summary>
        /// Метод заполняет значение объекта УО поля/атрибута из внешнего источника (системы)
        /// </summary>
        /// <param name="value">Объект</param>
        protected override void SetValue(ManagingOrganizationDataValue value)
        {
            value.Value = this.totalAreaDict.Get(value.EfManagingOrganization.ManagingOrganization.Id);
        }

        private class ManorgContractProxy
        {
            public long Id { get; set; }
            public decimal? AreaMkd { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }
    }
}