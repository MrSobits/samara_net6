namespace Bars.Gkh.Regions.Msk
{
    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            Namespace("Import.MskDpkrImport", "Импорт ДПКР(Москва)");
            Permission("Import.MskDpkrImport.View", "Просмотр");

            Namespace("Import.MskCeoStateServiceImport", "Импорт состояний ООИ для сервиса (Москва)");
            Permission("Import.MskCeoStateServiceImport.View", "Просмотр");

            Namespace("Import.MskCeoStateImport", "Импорт состояний ООИ (Москва)");
            Permission("Import.MskCeoStateImport.View", "Просмотр");

            Namespace("Reports.Msk", "Отчеты для Москвы");
            Permission("Reports.Msk.TypeWorkReport", "Отчет по видам работ");
        }

    }
}