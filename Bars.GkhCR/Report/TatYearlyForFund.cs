namespace Bars.GkhCr.Report
{
    using B4.Modules.Reports;

    public class TatYearlyForFund : GkhCr.Report.YearlyForFund
    {
        public TatYearlyForFund()
            : base(new ReportTemplateBinary(Properties.Resources.YearlyForFund))
        {
        }

        public override string Name
        {
            get
            {
                return "Ежеквартальный и годовой отчеты для Фонда (Татарстан)";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Ежеквартальный и годовой отчеты для Фонда (Татарстан)";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.YearlyForFundTat";
            }
        }

        protected override void InitCodes()
        {
            base.InitCodes();

            workGroups["7"] = "7";
            workGroups["8"] = "7";
            workGroups["9"] = "7";
            workGroups["10"] = "7";
            workGroups["11"] = "7";
            workGroups["29"] = "7";

            workGroups["30"] = "30";

            allVolsCodes.Add("7");
        }
    }
}