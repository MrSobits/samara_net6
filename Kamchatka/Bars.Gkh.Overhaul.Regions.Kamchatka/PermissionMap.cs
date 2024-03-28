namespace Bars.Gkh.Overhaul.Regions.Kamchatka
{
    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            Permission("Reports.GkhOverhaul.ProgramCrByDpkrForm1", "Форма 1. Перечень многоквартирных домов, включенных в краткосрочный план КР (Камчатка)");
            Permission("Reports.GkhOverhaul.ProgramCrByDpkrForm2", "Форма 2. Реестр многоквартирных домов по видам ремонта (Камчатка)");
            Permission("Reports.GkhRegOp.TurnoverBalanceSheet", "Оборотно-сальдовая ведомость (Камчатка)");
        }
    }
}