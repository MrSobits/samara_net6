namespace Bars.GkhGji.Regions.Tomsk.Report
{
    public class ProtocolGjiNotificationReport : GkhGji.Report.ProtocolGjiNotificationReport
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