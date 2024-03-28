using System;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.DomainService.Config;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Enums.Administration.FormatDataExport;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    public class FormatDataExportEntityViewModel : BaseViewModel<FormatDataExportEntity>
    {
        public override IDataResult List(IDomainService<FormatDataExportEntity> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var entityType = baseParams.Params.GetAs<EntityType>("EntityType");
            var id = baseParams.Params.GetAs<long>("Id");
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var dpkrDocument = this.Container.ResolveRepository<DpkrDocument>();
            var publishedProgramRecord = this.Container.ResolveRepository<PublishedProgramRecord>();
            var stage1 = this.Container.ResolveRepository<VersionRecordStage1>();

            var data = domainService.GetAll()
                .Where(x => (x.FormatDataExportInfo.Id == id) && (x.EntityType == entityType))
                .Select(x => new
                {
                    x.Id,
                    x.EntityId,
                    x.ErrorMessage,
                    x.ExportDate,
                    x.ExportEntityState,
                }).AsEnumerable()
                .Where(x => long.TryParse(x.EntityId, out _))
                .ToDictionary(x =>long.Parse(x.EntityId), y => y);

            IQueryable<FormatDataExportEntityDto> result = null;

            using (this.Container.Using(dpkrDocument, publishedProgramRecord, stage1))
            {
                switch (entityType)
                {
                    case EntityType.CrProgram:
                        result = data.Select(x => new FormatDataExportEntityDto
                        {
                            Id = x.Value.Id,
                            ErrorMessage = x.Value.ErrorMessage,
                            ExportDate = x.Value.ExportDate,
                            ExportEntityState = x.Value.ExportEntityState,
                            DpkrName = config.ProgrammName,
                            EntityId = x.Key
                        }).AsQueryable();
                        break;
                    case EntityType.СrProgramDoc:
                    {
                        result = dpkrDocument.GetAll()
                            .AsEnumerable()
                            .Where(x => data.ContainsKey(x.Id))
                            .Select(x => new FormatDataExportEntityDto
                            {
                                Id = data[x.Id].Id,
                                ErrorMessage = data[x.Id].ErrorMessage,
                                ExportDate = data[x.Id].ExportDate,
                                ExportEntityState = data[x.Id].ExportEntityState,
                                DocumentName = x.DocumentName,
                                DocumentNumber = x.DocumentNumber,
                                DocumentKind = x.DocumentKind.Name,
                                EntityId = x.Id
                            })
                            .AsQueryable();
                        break;
                    }
                    case EntityType.CrProgramHousePlanWork:
                    {
                        var entityIds = data.Select(x => x.Key.ToString()).ToList();
                        var list = new List<EntityIdElement>();

                        foreach (var item in entityIds)
                        {
                            if (item.Length > 5) // Для работ формат entityId A BBB CC, где A - id дома в Системе, B - код ОИИ (3 знака), С - порядковый номер (2 знака) 
                            {
                                list.Add(new EntityIdElement
                                {
                                    RoId = Convert.ToInt64(item.Substring(0, item.Length - 5)),
                                    ExportId = Convert.ToInt64(item.Substring(item.Length - 5, 3)),
                                    Index = Convert.ToInt64(item.Substring(item.Length - 2, 2))
                                });
                            }
                        }

                        var roList = list.Select(x => x.RoId);

                        var longTermTypeWorks = publishedProgramRecord.GetAll()
                            .Join(stage1.GetAll(),
                                x => x.Stage2,
                                y => y.Stage2Version,
                                (a, b) => new { pubRec = a, stage1 = b })
                            .Where(x => roList.Contains(x.pubRec.RealityObject.Id))
                            .Where(x => x.pubRec.Stage2.Stage3Version.ProgramVersion.IsMain)
                            .Select(x => new
                            {
                                RealityObjectId = x.pubRec.RealityObject.Id,
                                StructElExportId = x.stage1.StructuralElement.StructuralElement.ExportId,
                                x.pubRec.PublishedYear,
                                Locality = x.pubRec.RealityObject.Municipality.Name,
                                x.pubRec.RealityObject.Address,
                                StructElName = x.stage1.StructuralElement.StructuralElement.Name
                            })
                            .AsEnumerable()
                            .GroupBy(x => new { x.RealityObjectId, x.StructElExportId, x.PublishedYear, x.Locality, x.Address, x.StructElName })
                            .Select(x => new
                            {
                                x.Key.RealityObjectId,
                                x.Key.StructElExportId,
                                x.Key.PublishedYear,
                                x.Key.Locality,
                                x.Key.Address,
                                x.Key.StructElName
                            })
                            .GroupBy(x => new { x.RealityObjectId, x.StructElExportId })
                            .SelectMany(s =>
                            {
                                var index = 0;
                                var listPub = new List<FormatDataExportEntityDto>();

                                foreach (var f in s.OrderBy(o => o.PublishedYear))
                                {
                                    listPub.Add(new FormatDataExportEntityDto
                                    {
                                        EntityId = long.Parse(string.Format($"{f.RealityObjectId}{f.StructElExportId:D3}{++index:D2}")),
                                        RoId = f.RealityObjectId,
                                        Index = index,
                                        Address = f.Address,
                                        Locality = f.Locality,
                                        StructElName = f.StructElName,
                                        PublishedYear = f.PublishedYear,
                                        StructElExpId = f.StructElExportId
                                    });
                                }
                                return listPub;
                            })
                            .ToList();

                        result = longTermTypeWorks.Where(x => list.Any(y => y.ExportId == x.StructElExpId && y.Index == x.Index && y.RoId == x.RoId))
                            .Select(x => new FormatDataExportEntityDto
                            {
                                Id = data[x.EntityId].Id,
                                Address = x.Address,
                                Locality = x.Locality,
                                StructElName = x.StructElName,
                                PublishedYear = x.PublishedYear,
                                ErrorMessage = data[x.EntityId].ErrorMessage,
                                ExportDate = data[x.EntityId].ExportDate,
                                ExportEntityState = data[x.EntityId].ExportEntityState,
                                EntityId = x.EntityId
                            })
                            .AsQueryable();
                        break;
                    }
                }
            }

            return result.ToListDataResult(loadParams, this.Container);
        }

        private class EntityIdElement
        {
            public long RoId { get; set; }
            public long ExportId { get; set; }
            public long Index { get; set; }
        }

        private class FormatDataExportEntityDto
        {
            public long Id { get; set; }

            public long EntityId { get; set; }

            public long RoId { get; set; }

            public long Index { get; set; }

            public string Locality { get; set; }

            public string Address { get; set; }

            public int PublishedYear { get; set; }

            public DateTime? ExportDate { get; set; }

            public FormatDataExportEntityState ExportEntityState { get; set; }

            public string ErrorMessage { get; set; }

            public string DpkrName { get; set; }

            public string DocumentName { get; set; }

            public string DocumentKind { get; set; }

            public string DocumentNumber { get; set; }

            public string StructElName { get; set; }

            public long StructElExpId { get; set; }
        }

    }
}
