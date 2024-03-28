namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Extensions;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    using Castle.Windsor;

    public class TatarstanProtocolMvdService : ITatarstanProtocolMvdService
    {
        public IWindsorContainer Container { get; set; }

        public List<TatarstanProtocolMvdServiceDto> GetList(BaseParams baseParams, bool isExport)
        {
            var serviceProtocolMvdRo = this.Container.Resolve<IDomainService<ProtocolMvdRealityObject>>();
            var resolution = this.Container.Resolve<IDomainService<TatarstanResolution>>();
            var protocol = this.Container.Resolve<IDomainService<TatarstanProtocolMvd>>();

            using (this.Container.Using(serviceProtocolMvdRo, resolution, protocol))
            {
                var dateStart = baseParams.Params.ContainsKey("dateStart") ? baseParams.Params["dateStart"].ToDateTime() : DateTime.MinValue;
                var dateEnd = baseParams.Params.ContainsKey("dateEnd") ? baseParams.Params["dateEnd"].ToDateTime() : DateTime.MinValue;
                var realityObjectId = baseParams.Params.ContainsKey("realityObjectId") ? baseParams.Params["realityObjectId"].ToLong() : 0;
                var stageId = baseParams.Params.ContainsKey("stageId") ? baseParams.Params["stageId"].ToLong() : 0;

                List<long> ids = null;

                if (realityObjectId > 0)
                {
                    ids = serviceProtocolMvdRo.GetAll()
                        .Where(x => x.RealityObject.Id == realityObjectId)
                        .Select(x => x.ProtocolMvd.Id).ToList();
                }

                var data = protocol.GetAll()
                    .WhereIf(ids != null, x => ids.Contains(x.Id))
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .WhereIf(stageId > 0, x => x.Stage.Id == stageId)
                    .AsEnumerable()
                    .LeftJoin(resolution.GetAll(),
                        t1 => t1?.Inspection.Id,
                        t2 => t2?.Inspection.Id,
                        (t1, t2) => new { protocol = t1, resolution = t2 })
                    .Select(x => new TatarstanProtocolMvdServiceDto
                        {
                            Id = x.protocol.Id,
                            State = x.protocol.State,
                            DocumentNumber = x.protocol.DocumentNumber,
                            DocumentDate = x.protocol.DocumentDate,
                            Executant = x.protocol.TypeExecutant,
                            InspectionId = x.protocol.Inspection.Id,
                            PhysicalPerson = string.IsNullOrWhiteSpace($"{x.protocol.SurName} {x.protocol.Name} {x.protocol.Patronymic}")
                                ? x.protocol.PhysicalPerson
                                : $"{x.protocol.SurName} {x.protocol.Name} {x.protocol.Patronymic}",
                            Official = x.resolution?.Official?.Fio,
                            PenaltyAmount = x.resolution?.PenaltyAmount,
                            GisUin = isExport ? $"'{x.resolution?.GisUin}" : x.resolution?.GisUin
                    })
                    .ToList();

                return data;
            }
        }
    }
}
