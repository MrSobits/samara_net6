namespace Bars.GkhGji.Regions.Tula.Report.PrescriptionGji
{
    public class PrescriptionGjiNotificationReport : GkhGji.Report.PrescriptionGjiNotificationReport
    {
        public override bool PrintingAllowed
        {
            get
            {
                return false;
            }
        }
    }
}