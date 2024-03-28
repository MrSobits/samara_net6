namespace Bars.Gkh.Regions.Tatarstan.Reports
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;

    public class NormConsumptionDataExportReport : DataExportReport
    {
        public NormConsumptionDataExportReport()
            : base(new ReportTemplateBinary(Properties.Resources.PrintForm))
        {
        }

        private const int countLists = 1;

        /// <summary>
        /// Выполнить сборку отчета
        /// </summary>
        /// <param name="reportParams">Параметры отчета
        ///             </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var headers = this.BaseParams.Params.ContainsKey("headers")
                              ? this.BaseParams.Params.GetAs<string[]>("headers")
                              : new string[0];

            var dataIndexes = this.BaseParams.Params.ContainsKey("dataIndexes")
                              ? this.BaseParams.Params.GetAs<string[]>("dataIndexes")
                              : new string[0];


            if (headers.Length > 0 && dataIndexes.Length > 0)
            {

                for (var i = 1; i <= NormConsumptionDataExportReport.countLists; i++)
                {
                    var sectionColumn = reportParams.ComplexReportParams.ДобавитьСекцию(string.Format("Column{0}", i.ToString()));
                    var columnNumber = 0;

                    foreach (var column in headers)
                    {
                        sectionColumn.ДобавитьСтроку();
                        sectionColumn["Value"] = "$" + (columnNumber < dataIndexes.Length ? dataIndexes[columnNumber] : string.Empty) + "$";
                        sectionColumn["Header"] = column;
                        columnNumber++;
                    }
                }

                var element = this.Data.Cast<object>().FirstOrDefault();
                if (element != null)
                {
                    this.FillSection(reportParams, element.GetType());
                }
            }
        }

        /// <summary>
        /// Имя отчета
        /// </summary>
        public override string Name => "Выгрузка норматива потребления";

        private void FillSection(ReportParams reportParams, Type type)
        {

            var sections = new List<Section>();

            //Количество строк на лист
            var rowsPerPage = 500000;

            //Получаем количество листов на которых может уместится по rowsPerPage записей
            int lists = Convert.ToInt16(Math.Ceiling((double) this.Data.Count / rowsPerPage));

            for (var i = 1; i <= lists; i++)
            {
                var section = reportParams.ComplexReportParams.ДобавитьСекцию(string.Format("Row{0}", i.ToString()));
                sections.Add(section);
            }

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var rowNumber = 0;
            var lestNumber = 0;
            var rowNumberInList = 0;
            foreach (var row in this.Data.Cast<object>())
            {
                rowNumber++;
                rowNumberInList++;

                if (rowNumberInList > rowsPerPage)
                {
                    rowNumberInList = 1;
                    lestNumber++;
                }

                var sectionRow = sections[lestNumber];

                sectionRow.ДобавитьСтроку();
                sectionRow["RowNum"] = rowNumber;

                foreach (var propertyInfo in properties)
                {

                    if (propertyInfo.PropertyType.IsGenericType
                        && (propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(IList)
                        || propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)))
                    {
                        continue;
                    }

                    var propValue = propertyInfo.GetValue(row, null);
                    if (propValue == null)
                    {
                        continue;
                    }

                    object fieldVal;
                    if (propValue is bool)
                    {
                        fieldVal = (bool)propValue ? "Да" : "Нет";
                    }
                    else if (propValue is PersistentObject)
                    {
                        var typeObject = propValue.GetType();
                        var propertyName = typeObject.GetProperty("Name");
                        fieldVal = propertyName != null ? propertyName.GetValue(propValue, new object[0]) : propValue;
                    }
                    else if (propValue is Enum)
                    {
                        var memInfo = propertyInfo.PropertyType.GetMember(propValue.ToString());

                        if (memInfo.Count() == 0)
                        {
                            continue;
                        }

                        var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

                        fieldVal = ((DisplayAttribute)attributes[0]).Value.ToString();
                    }
                    else
                    {
                        fieldVal = propValue;
                    }

                    sectionRow[propertyInfo.Name] = fieldVal;
                }
            }
        }
    }
}