namespace Bars.Gkh.Overhaul.Nso.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.PriorityParams;
    using Bars.Gkh.Overhaul.Nso.ProgrammPriorityParams;

    using Castle.Windsor;

    public class RealityObjectInProgramStage3Report : DataExportReport
    {
        public IDomainService<RealityObjectStructuralElementInProgrammStage3> Domain { get; set; }

        public IWindsorContainer Container { get; set; }

        private readonly List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

        public RealityObjectInProgramStage3Report()
            : base(new ReportTemplateBinary(Properties.Resources.printform))
        {
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            PrepareData();

            // берем коды полей которые заполнены (в том числе используемые параметры очередности)
            var codes = data.SelectMany(x => x.Keys).Distinct();

            // отсеиваем параметры по которым у нас не производился расчет
            var priorityParams = Container.ResolveAll<IProgrammPriorityParam>().Where(x => codes.Contains(x.Code)).ToList();
            var pointParams = Container.ResolveAll<IPriorityParams>().Where(x => codes.Contains(x.Id)).ToList(); 

            var headers = BaseParams.Params.ContainsKey("headers")
                ? BaseParams.Params["headers"].ToString()
                : string.Empty;

            var dataIndexes = BaseParams.Params.ContainsKey("dataIndexes")
                ? BaseParams.Params["dataIndexes"].ToString()
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
                    FillSection(reportParams);
                }
            }
        }

        private void PrepareData()
        {
            var loadParam = BaseParams.GetLoadParam();
            var startData = Domain.GetAll()
                .Select(x =>
                    new
                    {
                        Municipality = x.RealityObject.Municipality.Name,
                        RealityObject = x.RealityObject.Address,
                        x.CommonEstateObjects,
                        x.Year,
                        x.IndexNumber,
                        x.Point,
                        x.Sum,
                        x.StoredCriteria,
                        x.StoredPointParams
                    })
                .Filter(loadParam, Container)
                .OrderIf(loadParam.Order.Length == 0, true, x => x.IndexNumber)
                .Order(loadParam)
                .ToList();
            
            foreach (var item in startData)
            {
                var tmpList = new Dictionary<string, string>
                {
                    {"Municipality", item.Municipality},
                    {"RealityObject", item.RealityObject},
                    {"Year", item.Year.ToStr()},
                    {"IndexNumber", item.IndexNumber.ToStr()},
                    {"Point", item.Point.ToStr()},
                    {"Sum", item.Sum.ToStr()},
                    {"CommonEstateObjects", item.CommonEstateObjects}
                };

                foreach (var criterion in item.StoredCriteria)
                {
                    tmpList.Add(criterion.Criterion, criterion.Value);
                }

                foreach (var pointParam in item.StoredPointParams)
                {
                    if (tmpList.ContainsKey(pointParam.Code))
                    {
                        tmpList[pointParam.Code] = pointParam.Value.ToStr();
                    }
                    else
                    {
                        tmpList.Add(pointParam.Code, pointParam.Value.ToStr());
                    }
                }

                this.data.Add(tmpList);
            }
        }

        public override string Name
        {
            get { return "Выгрузка реестра ДПКР"; }
        }

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