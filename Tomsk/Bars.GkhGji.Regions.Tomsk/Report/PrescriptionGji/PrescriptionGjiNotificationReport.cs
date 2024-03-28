namespace Bars.GkhGji.Regions.Tomsk.Report.PrescriptionGji
{
    /*
     Просто в томске данный отчет непонадобился и чтобы его выпилить просто убираю КодФормы чтобы нельзя было его ни откуда распечатать
     */
    public class PrescriptionGjiNotificationReport : GkhGji.Report.PrescriptionGjiNotificationReport
    {
        public override string CodeForm
        {
            get { return "Prescription_NotUsed"; }
        }
    }
}