namespace Bars.Gkh.Report.TechPassportSections
{
    public class Section2:BaseTechPassportSectionReport
    {
        protected override void PrepareComponentIds()
        {
            //2. Экспликация земельного участка
            GenerateCellCodes("Form_2", 1, 15, 3, 3);
        }
    }
}