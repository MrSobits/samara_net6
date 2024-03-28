namespace Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    using Castle.Windsor;

    public class TatarstanProtocolGjiService : ITatarstanProtocolGjiService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public List<ResultTatarstanProtocolGji> GetListResult(IDomainService<TatarstanProtocolGji> domainService, BaseParams baseParams)
        {
            var realityObject = baseParams.Params.GetAsId("realityObjectId");
            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");

            var protocolsIds = new HashSet<long>();

            if (realityObject != default(long))
            {
                var realityObjectService = this.Container.ResolveDomain<TatarstanProtocolGjiRealityObject>();
                using (this.Container.Using(realityObjectService))
                {
                    protocolsIds = realityObjectService.GetAll()
                        .Where(x => x.RealityObject.Id == realityObject)
                        .Select(x => x.TatarstanProtocolGji.Id)
                        .Distinct()
                        .AsEnumerable()
                        .ToHashSet();
                }
            }

            var protocolIdsHash = domainService.GetAll()
                .WhereIf(protocolsIds.Any(), x => protocolsIds.Contains(x.Id))
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .Select(x => x.Id)
                .ToHashSet();

            var inspectorDict = this.GetInspectors(protocolIdsHash);

            return domainService.GetAll()
                .Where(x => protocolIdsHash.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.DocumentNumber,
                    x.DocumentDate,
                    x.GisUin,
                    x.PenaltyAmount,
                    x.Executant,
                    MunicipalityName = x.Municipality.Name,
                    x.SurName,
                    x.Name,
                    x.Patronymic,
                    x.Inspection
                })
                .AsEnumerable()
                .Select(x => new ResultTatarstanProtocolGji ()
                {
                    Id = x.Id,
                    State = x.State,
                    DocumentNumber = x.DocumentNumber,
                    DocumentDate = x.DocumentDate,
                    GisUin = x.GisUin,
                    PenaltyAmount = x.PenaltyAmount,
                    Executant = x.Executant,
                    MunicipalityName = x.MunicipalityName,
                    Inspectors = inspectorDict.TryGetValue(x.Id, out var result)
                        ? result.InspectorNames
                        : null,
                    Fio = $"{x.SurName} {x.Name} {x.Patronymic}",
                    InspectionId = x.Inspection?.Id
                }).ToList();
        }

        /// <inheritdoc />
        public IDataResult GetInfo(BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAsId("protocolId");
            if (protocolId == default(long))
            {
                return new BaseDataResult(false, "Некорректные данные");
            }
            
            return this.GetInspectors(new[] { protocolId }).TryGetValue(protocolId, out var inspectors) 
                ? new BaseDataResult(new
                {
                    inspectorNames = inspectors.InspectorNames,
                    inspectorIds = inspectors.InspectorIds
                }) 
                : new BaseDataResult();
        }

        /// <summary>
        /// Возвращает словарь id протокола - инспекторы
        /// </summary>
        private Dictionary<long, InpectorsInfo> GetInspectors(ICollection<long> protocolIdsCollection)
        {
            var service = this.Container.ResolveDomain<DocumentGjiInspector>();
            using (this.Container.Using(service))
            {
               return service.GetAll()
                    .Where(x => protocolIdsCollection.Contains(x.DocumentGji.Id))
                    .Select(x => new
                    {
                        ProtocolId = x.DocumentGji.Id,
                        x.Inspector.Id,
                        x.Inspector.Fio
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ProtocolId)
                    .ToDictionary(x => x.Key,
                        x => new InpectorsInfo(
                            string.Join(", ", x.Select(y => y.Fio)),
                                string.Join(", ", x.Select(y => y.Id))));
            }
        }

        private class InpectorsInfo
        {
            public string InspectorNames { get; }
            public string InspectorIds { get; }

            /// <inheritdoc />
            public InpectorsInfo(string inspectorNames, string inspectorIds)
            {
                this.InspectorNames = inspectorNames;
                this.InspectorIds = inspectorIds;
            }
        }
    }
}
