namespace Bars.GkhGji.Regions.Nso
{
    using B4;
    using Gkh.TextValues;

    public class NavigationProvider : INavigationProvider
    {
        public IMenuItemText MenuItemText { get; set; }

        public void Init(MenuItem root)
        {
            var dicts = root.Add("Справочники").Add("ГЖИ");

            var item = dicts.Get(this.MenuItemText.GetText("Зональные жилищные инспекции"));
            if (item != null)
            {
                item.Caption = "Отделы";
            }

            root.Add("Жилищная инспекция")
                .Add("Управление задачами")
                .Add("Задачи по обращениям", "reminderappealcits")
                .AddRequiredPermission("GkhGji.ManagementTask.ReminderAppealCits.View")
                .WithIcon("reminderHead");

            root.Add("Жилищная инспекция")
                .Add("Реестр уведомлений")
                .Add("Реестр уведомлений о смене способа управления МКД", "mkdchangenotification")
                .AddRequiredPermission("GkhGji.MkdChangeNotification.View")
                .WithIcon("mkdChangeNotificationHead");

            root.Add("Жилищная инспекция")
                .Add("Документы")
                .Add("Протоколы по ст.19.7 КоАП РФ", "protocol197")
                .AddRequiredPermission("GkhGji.DocumentsGji.Protocol197.View");
        }

        public string Key => MainNavigationInfo.MenuName;

        public string Description => MainNavigationInfo.MenuDescription;
    }
}