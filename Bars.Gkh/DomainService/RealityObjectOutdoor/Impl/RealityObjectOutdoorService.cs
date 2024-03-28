namespace Bars.Gkh.DomainService.RealityObjectOutdoor.Impl
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class RealityObjectOutdoorService : IRealityObjectOutdoorService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Обновляет ссылку на двор у дома.
        /// </summary>
        public ActionResult UpdateOutdoorInRealityObjects(BaseParams baseParams)
        {
            var realityObjectIdsStr = baseParams.Params.GetAs<string>("realityObjectIds");
            var outdoorId = baseParams.Params.GetAsId("outdoorId");

            if (outdoorId == default(long) || string.IsNullOrWhiteSpace(realityObjectIdsStr))
            {
                return JsonNetResult.Failure("Некорректный идентификатор дома");
            }

            var realityObjectIdsHash = Array.ConvertAll(realityObjectIdsStr.Split(','),
                x => long.TryParse(x, out var result) ? result : 0)
                .Where(x => x != 0)
                .ToHashSet();

            if (!realityObjectIdsHash.Any())
            {
                return JsonNetResult.Failure("");
            }

            var realityObjectOutdoorDomain = this.Container.ResolveDomain<RealityObjectOutdoor>();
            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();

            using (this.Container.Using(realityObjectOutdoorDomain, realityObjectDomain))
            {
                var outdoor = realityObjectOutdoorDomain.Get(outdoorId);
                var realityObjects = realityObjectDomain.GetAll()
                    .Where(x => realityObjectIdsHash.Contains(x.Id))
                    .ToList();

                realityObjects.ForEach(x =>
                {
                    if (x.Outdoor?.Id == outdoorId)
                    {
                        return;
                    }
                    x.Outdoor = outdoor;
                    realityObjectDomain.Update(x);
                });

                return JsonNetResult.Success;
            }
        }

        /// <summary>
        /// Удаляет ссылку на двор у дома.
        /// </summary>
        public ActionResult DeleteOutdoorFromRealityObject(BaseParams baseParams)
        {
            var realityObjectId = baseParams.Params.GetAsId("realityObjectId");

            if (realityObjectId == default(long))
            {
                return JsonNetResult.Failure("Некорректный идентификатор дома");
            }

            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();
            using (this.Container.Using(realityObjectDomain))
            {
                if (!(realityObjectDomain.Get(realityObjectId) is RealityObject realityObject))
                {
                    return JsonNetResult.Failure("Некорректный идентификатор дома");
                }

                realityObject.Outdoor = null;
                realityObjectDomain.Update(realityObject);
            }

            return JsonNetResult.Success;
        }

        /// <inheritdoc />
        public IDataResult GetList(IDomainService<RealityObjectOutdoor> domainService, BaseParams baseParams)
        {
            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();
            var fiasDomainService = this.Container.ResolveDomain<Fias>();
            var userManager = this.Container.Resolve<IGkhUserManager>();

            using (this.Container.Using(realityObjectDomain, fiasDomainService, userManager))
            {
                var realityObjectsDict = realityObjectDomain.GetAll()
                    .Where(x => x.Outdoor != null)
                    .GroupBy(x => x.Outdoor.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Address).ToList());

                var fiasOffNameDict = fiasDomainService.GetAll()
                    .Where(x => domainService.GetAll().Any(y => y.MunicipalityFiasOktmo.FiasGuid == x.AOGuid))
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .Select(
                        x => new
                        {
                            x.AOGuid,
                            x.OffName
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.AOGuid)
                    .ToDictionary(x => x.Key, x => x.First().OffName);

                //отображаем дворы, относящиеся к МО оператора.
                var municipalityIds = userManager.GetMunicipalityIds();

                var loadParam = baseParams.GetLoadParam();

                return domainService.GetAll()
                    .WhereIf(municipalityIds.Count > 0,
                        x => municipalityIds.Contains(x.MunicipalityFiasOktmo.Municipality.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.MunicipalityFiasOktmo,
                        x.Name,
                        x.Code,
                        x.Area,
                        x.AsphaltArea,
                        x.RepairPlanYear
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.MunicipalityFiasOktmo,
                        Municipality = x.MunicipalityFiasOktmo.Municipality != null
                            ? x.MunicipalityFiasOktmo.Municipality.Name
                            : string.Empty,
                        Locality = fiasOffNameDict.ContainsKey(x.MunicipalityFiasOktmo.FiasGuid)
                            ? fiasOffNameDict[x.MunicipalityFiasOktmo.FiasGuid]
                            : string.Empty,
                        x.Name,
                        x.Code,
                        RealityObjects = realityObjectsDict.ContainsKey(x.Id)
                            ? string.Join(", ", realityObjectsDict[x.Id])
                            : string.Empty,
                        x.Area,
                        x.AsphaltArea,
                        x.RepairPlanYear
                    }).ToListDataResult(loadParam, this.Container);
            }
        }
    }
}
