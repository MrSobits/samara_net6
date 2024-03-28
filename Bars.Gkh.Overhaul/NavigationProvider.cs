namespace Bars.Gkh.Overhaul
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
            root.Add("Справочники")
                .Add("Капитальный ремонт")
                .Add("Расценки по работам", "workprice")
                .WithIcon("work")
                .AddRequiredPermission("Gkh.Dictionaries.WorkPrice.View");

            root.Add("Справочники")
                .Add("Капитальный ремонт")
                .Add("Работы", "job")
                .WithIcon("work")
                .AddRequiredPermission("Ovrhl.Dictionaries.Job.View");

            root.Add("Справочники")
                .Add("Капитальный ремонт")
                .Add("Вид документа основания ДПКР", "basisoverhauldockind")
                .AddRequiredPermission("Ovrhl.Dictionaries.BasisOverhaulDocKind.View");

            root.Add("Справочники")
                .Add("Капитальный ремонт")
                .Add("Тип группы ООИ", "grouptype")
                .WithIcon("work")
                .AddRequiredPermission("Ovrhl.Dictionaries.GroupType.View");

            root.Add("Справочники")
                .Add("Капитальный ремонт")
                .Add("ООИ", "commonestateobject")
                .WithIcon("work")
                .AddRequiredPermission("Ovrhl.Dictionaries.CommonEstateObject.View");

            root.Add("Администрирование")
                .Add("Импорт жилых домов")
                .Add("Импорт жилых домов (универсальный)", "commonrealityobjimport")
                .AddRequiredPermission("Import.CommonRealtyObjectImport.View");
        }
    }
}
