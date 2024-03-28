namespace Bars.Gkh.Regions.Msk
{
    using Bars.B4;

    public class NavigationProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Администрирование")
                .Add("Импорты")
                .Add("Импорт ДПКР для сервиса(Москва)", "mskdpkrimport")
                .AddRequiredPermission("Import.MskDpkrImport.View");

            root.Add("Администрирование")
                .Add("Импорты")
                .Add("Импорт ДПКР(Москва)", "mskoverhaulimport")
                .AddRequiredPermission("Import.MskDpkrImport.View");

            root.Add("Администрирование")
                .Add("Импорты")
                .Add("Импорт состояний ООИ (Москва)", "mskceoimport")
                .AddRequiredPermission("Import.MskCeoStateImport.View");

            root.Add("Администрирование")
                .Add("Импорты")
                .Add("Импорт состояний ООИ Для сервиса (Москва)", "mskceoserviceimport")
                .AddRequiredPermission("Import.MskCeoStateServiceImport.View");

            root.Add("Администрирование")
                .Add("Импорты")
                .Add("Импорт баллов ДПКР (Москва)", "mskceopointimport")
                .AddRequiredPermission("Import.MskDpkrImport.View");
        }
    }
}
