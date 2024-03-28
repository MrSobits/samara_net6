namespace Bars.GkhGji.Regions.Tomsk.Report.DisposalGji
{
    /*
     Просто в томске данный отчет непонядобился и чтобы его выпилить просто убираю КодФормы чтобы нельзя было ег ониоткуда распечатать
     */
    public class DisposalGjiStateToProsecReport : GkhGji.Report.DisposalGjiStateToProsecReport
    {
        public override string CodeForm
        {
            get { return "Disposal_NotUsed"; }
        }
    }
}