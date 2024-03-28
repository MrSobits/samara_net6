namespace Bars.GkhGji.Regions.Smolensk.Report.Prescription
{
    using GkhGji.Report;

    public class PrescriptionCancelReport : PrescriptionGjiCancelReport
    {
        public override string Name
        {
            get { return "Решение об отмене"; }
        }

        public override string Description
        {
            get { return "Решение об отмене предписания"; }
        }
    }
}