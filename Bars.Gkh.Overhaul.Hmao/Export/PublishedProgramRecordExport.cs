namespace Bars.Gkh.Overhaul.Hmao.Export
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Gkh.Utils;

    /// <summary>
    /// Экспорт опубликованной программы
    /// </summary>
    public class PublishedProgramRecordExport : BaseDataExportService
    {
        /// <summary>
        /// Версия
        /// </summary>
        public IDomainService<VersionRecord> VersionRecord { get; set; }

        /// <summary>
        /// Опубликованная программа
        /// </summary>
        public IDomainService<PublishedProgramRecord> DomainService { get; set; }

        /// <summary>
        /// Сервис модификации коллекции
        /// </summary>
        public IModifyEnumerableService ModifyEnumerableService { get; set; }

        /// <inheritdoc />
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);

            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;

            var moId = baseParams.Params.GetAs<long>("mo_id");

            List<PublishProgramRecordDto> data;

            if (groupByRoPeriod == 0)
            {
                // Поулчаем опубликованную программу по основной версии
                data = this.DomainService.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .Where(x => x.PublishedProgram.ProgramVersion.Municipality.Id == moId)
                    .Where(x => x.Stage2 != null)
                    .Select(x => new PublishProgramRecordDto
                    {
                        Id = x.Stage2.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        RealityObject = x.RealityObject.Address,
                        Sum = x.Stage2.Sum,
                        CommonEstateobject = x.Stage2.CommonEstateObject.Name,
                        PublishedYear = x.PublishedYear,
                        IndexNumber = x.Stage2.Stage3Version.IndexNumber
                    })
                    .OrderBy(x => x.IndexNumber)
                    .ThenBy(x => x.PublishedYear)
                    .Filter(loadParam, this.Container)
                    .Order(loadParam)
                    .ToList();
            }
            else
            {
                var dataPublished = this.DomainService.GetAll()
                    .Where(x => x.Stage2 != null)
                        .Where(x => x.PublishedProgram.ProgramVersion.IsMain
                                && x.PublishedProgram.ProgramVersion.Municipality.Id == moId)
                        .Select(x => new { x.Stage2.Stage3Version.Id, x.PublishedYear })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.PublishedYear).FirstOrDefault());

                data = this.VersionRecord.GetAll()
                    .Where(x => x.ProgramVersion.IsMain)
                    .Where(x => x.ProgramVersion.Municipality.Id == moId)
                    .Where(x => this.DomainService.GetAll()
                        .Any(y => y.Stage2.Stage3Version.Id == x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        RealityObject = x.RealityObject.Address,
                        PublishedYear = 0,
                        x.Sum,
                        CommonEstateobject = x.CommonEstateObjects,
                        x.IndexNumber
                    })
                    .AsEnumerable()
                    .Select(x => new PublishProgramRecordDto
                    {
                        Id = x.Id,
                        Municipality = x.Municipality,
                        RealityObject = x.RealityObject,
                        PublishedYear = dataPublished.ContainsKey(x.Id) ? dataPublished[x.Id] : 0,
                        Sum = x.Sum,
                        CommonEstateobject = x.CommonEstateobject,
                        IndexNumber = x.IndexNumber
                    })
                    .AsQueryable()
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.PublishedYear)
                    .Filter(loadParam, this.Container)
                    .Order(loadParam)
                    .ToList();
            }

            if (this.ModifyEnumerableService != null)
            {
                data = this.ModifyEnumerableService.ReplaceProperty(data, ".", x => x.RealityObject).ToList();
            }

            return data;
        }

        private class PublishProgramRecordDto
        {
            public long Id { get; set; }
            public string Municipality { get; set; }
            public string RealityObject { get; set; }
            public decimal Sum { get; set; }
            public string CommonEstateobject { get; set; }
            public int PublishedYear { get; set; }
            public int IndexNumber { get; set; }
        }
    }
}