namespace Bars.GkhGji.Regions.Tomsk.Report.DisposalGji
{
    /*
     Просто в томске данный отчет не понадобился и чтобы его выпилить просто убираю КодФормы, чтобы нельзя было его ни откуда распечатать
     */
    public class DisposalGjiNotificationReport : GkhGji.Report.DisposalGjiNotificationReport
    {
        public override string CodeForm
        {
            get { return "Disposal_NotUsed"; }
        }
    }
}