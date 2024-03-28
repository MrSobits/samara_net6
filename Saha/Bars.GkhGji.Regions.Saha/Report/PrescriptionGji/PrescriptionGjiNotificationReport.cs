namespace Bars.GkhGji.Regions.Saha.Report.PrescriptionGji
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