namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk
{
    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            this.Namespace("Import.AgentPIRDebtorCreditImport", "Импорт зачислений агента ПИР");
            this.Permission("Import.AgentPIRDebtorCreditImport.View", "Просмотр");

            this.Namespace("Import.AgentPIRDebtorOrderingImport", "Импорт выписок должников агента ПИР");
            this.Permission("Import.AgentPIRDebtorOrderingImport.View", "Просмотр");
        }
    }
}