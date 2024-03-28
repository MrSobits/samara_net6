namespace Bars.GkhDI.Regions.Samara.Report
{
    /// <summary>
    /// Отчет по раскрытию информации по ПП РФ №731 (Самара)
    /// </summary>
    public class PercentCalculationReportSamara : GkhDi.Report.PercentCalculationReport
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PercentCalculationReportSamara()
        {
            AverRealObjPerc = 0;//Если на период построения отчета у УО в управлении нет ни одного дома в столбце "Процент раскрываемости (вторые 50%)" отображать "0".
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по раскрытию информации по ПП РФ №731 (Самара)";
            }
        }

        public override string Name
        {
            get
            {
                return "Отчет по раскрытию информации по ПП РФ №731 (Самара)";
            }
        }

    }
}