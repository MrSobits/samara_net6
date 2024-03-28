namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.ProgrammPriorityParams;

    using Castle.Windsor;

    public class RealityObjectInProgramStage3Report : DataExportReport
    {
        public IDomainService<RealityObjectStructuralElementInProgrammStage3> Domain { get; set; }

        public IWindsorContainer Container { get; set; }

        private const int CountLists = 2;

        private readonly List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

        public RealityObjectInProgramStage3Report()
            : base(new ReportTemplateBinary(Properties.Resources.PrintForm))
        {
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            PrepareData();

            var priorityParams = Container.ResolveAll<IProgrammPriorityParam>();

            var headers = BaseParams.Params.ContainsKey("headers")
                ? BaseParams.Params["headers"].ToString()
                : string.Empty;

            var dataIndexes = BaseParams.Params.ContainsKey("dataIndexes")
                ? BaseParams.Params["dataIndexes"].ToString()
                : string.Empty;

            string[] massHeaders = headers.Split(',').Union(priorityParams.Select(x => x.Name)).ToArray();
            string[] massDataIndexes = dataIndexes.Split(',').Union(priorityParams.Select(x => x.Code)).ToArray();

            if (massHeaders.Length > 0 && massDataIndexes.Length > 0)
            {
                for (var i = 1; i <= CountLists; i++)
                {
                    var sectionColumn =
                        reportParams.ComplexReportParams.ДобавитьСекцию(string.Format("Column{0}", i));
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
                        x.StoredCriteria
                    })
                .Filter(loadParam, Container)
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

                this.data.Add(tmpList);
            }
        }

        public override string Name
        {
            get { return "Выгрузка реестра ДПКР"; }
        }

        private void FillSection(ReportParams reportParams)
        {
            var sections = new List<Section>();

            //Получаем количество листов на которых может уместится по 50 тыщ записей
            int lists = Convert.ToInt16(Math.Ceiling((double)data.Count / 50000));

            for (var i = 1; i <= lists; i++)
            {
                var section = reportParams.ComplexReportParams.ДобавитьСекцию(string.Format("Row{0}", i));
                sections.Add(section);
            }

            var rowNumber = 0;
            var lestNumber = 0;
            var rowNumberInList = 0;
            foreach (var row in this.data)
            {
                rowNumber++;
                rowNumberInList++;

                if (rowNumberInList > 50000)
                {
                    rowNumberInList = 1;
                    lestNumber++;
                }

                var sectionRow = sections[lestNumber];

                sectionRow.ДобавитьСтроку();
                sectionRow["RowNum"] = rowNumber;

                foreach (var cell in row)
                {
                    sectionRow[cell.Key] = cell.Value;
                }
            }
        }
    }
}