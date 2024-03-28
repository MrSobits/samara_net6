namespace Bars.Gkh.Overhaul.ViewModel
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.DomainService;

    using Bars.B4;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;
    using B4.Utils;

    using Bars.Gkh.Domain;

    /// <summary>
    /// View model конструктивного элемента дома
    /// </summary>
    public class RealityObjectStructuralElementViewModel : BaseViewModel<RealityObjectStructuralElement>
    {
        #region Dependency injection members

        /// <summary>
        /// Сервис дома в ДПКР
        /// </summary>
        public IRealityObjectDpkrDataService RoDpkrDataService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="OvrhlRealityObjectLift"/>
        /// </summary>
        public IDomainService<OvrhlRealityObjectLift> OvrhlRealityObjectLiftDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="TehPassportValue"/>
        /// </summary>
        public IDomainService<TehPassportValue> TehPassportValueDomain { get; set; }

        #endregion

        /// <inheritdoc />
        public override IDataResult List(IDomainService<RealityObjectStructuralElement> domainService, BaseParams baseParams)
        {
            var valuesService = this.Container.ResolveDomain<RealityObjectStructuralElementAttributeValue>();
            var attributesService = this.Container.ResolveDomain<StructuralElementGroupAttribute>();
            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();

            try
            {
                var loadParams = this.GetLoadParam(baseParams);

                var objectId = baseParams.Params.GetAs<long>("objectId");
                var showLift = baseParams.Params.GetAs<bool>("showLift");
                var showLiftTp = baseParams.Params.GetAs<bool>("showLiftTp");
                var liftId = baseParams.Params.GetAsId("liftId");

                var liftQuery = this.OvrhlRealityObjectLiftDomain.GetAll().WhereIf(liftId != 0, y => y.Id != liftId);
                var liftTpQuery = this.TehPassportValueDomain.GetAll()
                    .Where(y => y.FormCode == "Form_4_2_1" && y.CellCode.EndsWith(":20") && y.Value != null);

                var roElements = domainService.GetAll()
                    .Where(x => x.RealityObject.Id == objectId)
                    .WhereIf(
                        showLift, 
                        x => x.StructuralElement.Name.ToLower().Contains("лифт") 
                            && !liftQuery.Any(y => y.RealityObjectStructuralElement == x))
                    .WhereIf(
                        showLiftTp,
                        x => x.StructuralElement.Name.ToLower().Contains("лифт")
                            && !liftTpQuery.Any(y => y.Value == x.Id.ToString()));

                var elemIds = roElements.Select(x => x.Id).ToList();
                var groupIds = roElements.Select(x => x.StructuralElement.Group.Id).Distinct().ToList();

                var attributes = attributesService.GetAll().Where(x => groupIds.Contains(x.Group.Id)).ToList();
                var values = valuesService.GetAll().Where(x => elemIds.Contains(x.Object.Id)).ToList();

                var publishYearsByCeo = new Dictionary<long, int>();
                var adjustedYears = new Dictionary<long, int>();

                if (this.RoDpkrDataService != null)
                {
                    publishYearsByCeo = this.RoDpkrDataService.GetPublishYear(objectId, DateTime.Now.Year);
                    adjustedYears = this.RoDpkrDataService.GetAdjustedYear(objectId);
                }

                var data = roElements
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.RealityObject.Address,
                        RealityObject = new RealityObject {Id = x.RealityObject.Id, Address = x.RealityObject.Address },
                        CeoId = x.StructuralElement.Group.CommonEstateObject.Id,
                        Object = x.StructuralElement.Group.CommonEstateObject.Name,
                        GroupId = x.StructuralElement.Group.Id,
                        Group = x.StructuralElement.Group.Name,
                        ElementName = x.Name != "" && x.Name != null? x.Name: x.StructuralElement.Name,
                        UnitMeasure = x.StructuralElement.UnitMeasure.Name,
                        x.Volume,
                        x.Repaired,
                        x.LastOverhaulYear,
                        x.StructuralElement,
                        x.Wearout,
                        x.WearoutActual,
                        Name = x.Name != "" && x.Name != null ? x.Name : x.StructuralElement.Name,
                        Multiple = x.StructuralElement.Group.CommonEstateObject.MultipleObject,
                        x.Condition,
                        PlanRepairYear =
                        publishYearsByCeo.ContainsKey(x.StructuralElement.Group.CommonEstateObject.Id)
                            ? (int?)publishYearsByCeo[x.StructuralElement.Group.CommonEstateObject.Id]
                            : null,
                        RequiredFieldsFilled =
                        !attributes.Any(y =>
                            y.Group.Id == x.StructuralElement.Group.Id &&
                            y.IsNeeded &&
                            !values.Any(v =>
                                v.Object.Id == x.Id &&
                                v.Attribute.Id == y.Id &&
                                v.Value != null)),
                        x.SystemType,
                        x.NetworkLength,
                        x.NetworkPower,
                        AdjustedYear = adjustedYears.Get(x.Id) != 0 ? adjustedYears.Get(x.Id) : (int?)null,
                        x.FileInfo
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(valuesService);
                this.Container.Release(attributesService);
                this.Container.Release(realityObjectDomain);
            }
        }
    }
}