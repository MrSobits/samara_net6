namespace Bars.Gkh.Regions.Tyumen
{
    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            this.Namespace("Import.ProgramCrImport", "Импорт программы кап.ремонта");
            this.Permission("Import.ProgramCrImport.View", "Просмотр");
            this.Permission("Import.ProgramCrImport.FinanceCut", "Разрез финансирования");

            this.Namespace("Import.RealityObjectExaminationImport", "Импорт жилых домов (Обследование)");
            this.Permission("Import.RealityObjectExaminationImport.View", "Просмотр");
        }
    }
}