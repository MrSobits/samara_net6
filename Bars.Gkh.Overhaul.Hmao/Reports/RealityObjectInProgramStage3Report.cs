namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.Modules.DataExport;

    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.PriorityParams;
    using Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams;

    using Castle.Windsor;

    /// <summary>
    /// Экспорт ДПКР
    /// </summary>
    public class RealityObjectInProgramStage3Report : DataExportReport
    {
        /// <summary>
        /// Конструктивный элемент дома в ДПКР
        /// </summary>
        public IDomainService<RealityObjectStructuralElementInProgrammStage3> Domain { get; set; }

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сервис модификации коллекции
        /// </summary>
        public IModifyEnumerableService ModifyEnumerableService { get; set; }

        private readonly List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

        /// <summary>
        /// .ctor
        /// </summary>
        public RealityObjectInProgramStage3Report()
            : base(new ReportTemplateBinary(Properties.Resources.printform))
        {
        }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            this.PrepareData();

            // берем коды полей которые заполнены (в том числе используемые параметры очередности)
            var codes = this.data.First().Keys;

            // отсеиваем параметры по которым у нас не производился расчет
            var priorityParams = this.Container.ResolveAll<IProgrammPriorityParam>().Where(x => codes.Contains(x.Code)).ToList();
            var pointParams = this.Container.ResolveAll<IPriorityParams>().Where(x => codes.Contains(x.Id)).ToList(); 

            var headers = this.BaseParams.Params.ContainsKey("headers")
                ? this.BaseParams.Params["headers"].ToString()
                : string.Empty;

            var dataIndexes = this.BaseParams.Params.ContainsKey("dataIndexes")
                ? this.BaseParams.Params["dataIndexes"].ToString()
                : string.Empty;

            string[] massHeaders = headers.Split(',').Union(priorityParams.Select(x => x.Name))
                .Union(pointParams.Select(x => string.Format("Параметр балла \"{0}\"", x.Name))).ToArray();
            string[] massDataIndexes = dataIndexes.Split(',')
                .Union(priorityParams.Select(x => x.Code)).Union(pointParams.Select(x => x.Id)).ToArray();

            if (massHeaders.Length > 0 && massDataIndexes.Length > 0)
            {
                var sectionColumn = reportParams.ComplexReportParams.ДобавитьСекцию("Column1");
                var columnNumber = 0;

                foreach (var column in massHeaders)
                {
                    sectionColumn.ДобавитьСтроку();
                    sectionColumn["Value"] = "$"
                        + (columnNumber < massDataIndexes.Length
                            ? massDataIndexes[columnNumber]
                            : string.Empty) + "$";
                    sectionColumn["Header"] = column;
                    columnNumber++;
                }

                if (this.data.Any())
                {
                    this.FillSection(reportParams);
                }
            }
        }

        private void PrepareData()
        {
            var loadParam = this.BaseParams.GetLoadParam();

            var muId = this.BaseParams.Params.GetAs<long>("muId");

            var startData = this.Domain.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == muId
                    || x.RealityObject.MoSettlement.Id == muId)
                .Select(x => new VersionRecordDto
                {
                    Id = x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObject = x.RealityObject.Address,
                    CommonEstateObjects = x.CommonEstateObjects,
                    Year = x.Year,
                    IndexNumber = x.IndexNumber,
                    Sum = x.Sum,
                    StoredCriteria = x.StoredCriteria,
                    StoredPointParams = x.StoredPointParams
                })
                .Filter(loadParam, this.Container)
                .OrderIf(loadParam.Order.Length == 0, true, x => x.IndexNumber)
                .Order(loadParam)
                .ToList();

            if (this.ModifyEnumerableService != null)
            {
                startData = this.ModifyEnumerableService.ReplaceProperty(startData, ".", x => x.RealityObject).ToList();
            }

            foreach (var item in startData)
            {
                var tmpList = new Dictionary<string, string>
                {
                    {"Municipality", item.Municipality},
                    {"RealityObject", item.RealityObject},
                    {"Year", item.Year.ToStr()},
                    {"IndexNumber", item.IndexNumber.ToStr()},
                    {"Sum", item.Sum.ToStr()},
                    {"CommonEstateObjects", item.CommonEstateObjects}
                };

                if (item.StoredCriteria != null)
                {
                    foreach (var criterion in item.StoredCriteria.GroupBy(x => x.Criterion))
                    {
                        tmpList[criterion.Key] = criterion.First().Value;
                    }
                }

                if (item.StoredPointParams != null)
                {
                    foreach (var pointParam in item.StoredPointParams.GroupBy(x => x.Code))
                    {
                        tmpList[pointParam.Key] = pointParam.First().Value.ToStr();
                    }
                }

                this.data.Add(tmpList);
            }
        }

        /// <inheritdoc />
        public override string Name => "Выгрузка реестра ДПКР";

        private void FillSection(ReportParams reportParams)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Row1");

            var rowNumber = 0;
            foreach (var row in this.data)
            {
                section.ДобавитьСтроку();
                section["RowNum"] = ++rowNumber;

                foreach (var cell in row)
                {
                    section[cell.Key] = cell.Value;
                }
            }
        }
    }
}