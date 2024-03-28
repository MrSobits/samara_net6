namespace Bars.GkhGji.Regions.BaseChelyabinsk
{
    using Bars.B4;
    using Bars.Gkh.TextValues;

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
                .Add("Лицензирование")
                .Add("Аннулирование и предоставление сведений о лицензиях", "licenseaction")
                .AddRequiredPermission("GkhGji.License.LicenseAction.View")
                .WithIcon("mkdChangeNotificationHead");

            root.Add("Жилищная инспекция")
                .Add("Межведомственное взаимодействие")
                .Add("Прием информации о сертификате ключа проверки электронной подписи", "certinfo")
                .AddRequiredPermission("GkhGji.SMEV.CertInfo.View")
                .WithIcon("mkdChangeNotificationHead");

            root.Add("Жилищная инспекция")
             .Add("Межведомственное взаимодействие")
             .Add("Сведения о паспортах гражданина РФ", "mvdpassport")
             .AddRequiredPermission("GkhGji.SMEV.MVDPassport.View")
             .WithIcon("activityTsj");

            root.Add("Жилищная инспекция")
           .Add("Межведомственное взаимодействие")
           .Add("Сведения о регистрации по месту жительства граждан РФ", "mvdlivingplacereg")
           .AddRequiredPermission("GkhGji.SMEV.MVDPassport.View")
           .WithIcon("shortProgram");

            root.Add("Жилищная инспекция")
       .Add("Межведомственное взаимодействие")
       .Add("Сведения о регистрации по месту пребывания граждан РФ", "mvdstayingplacereg")
       .AddRequiredPermission("GkhGji.SMEV.MVDPassport.View")
       .WithIcon("manOrg");

            root.Add("Жилищная инспекция")
          .Add("Управление задачами")
          .Add("Календарь инспектора", "taskcalendar")
          .AddRequiredPermission("GkhGji.TaskCalendar.TaskCalendarPanel.View")
          .WithIcon("clnd1");

            root.Add("Жилищная инспекция").Add("Документы").Add("Реестр СЭД", "eds").AddRequiredPermission("GkhGji.EDS.EDSRegistry.View").WithIcon("baseInsCheck");
            root.Add("Жилищная инспекция").Add("Документы").Add("Запросы ГЖИ", "requestregistry").AddRequiredPermission("GkhGji.EDS.EDSRegistry.View").WithIcon("baseInsCheck");
            root.Add("Жилищная инспекция").Add("Документы").Add("Реестр документов для подписи", "edssign").AddRequiredPermission("GkhGji.EDS.EDSRegistrySign.View").WithIcon("competition");
            root.Add("Жилищная инспекция").Add("Документы").Add("Досудебное обжалование", "complaints").AddRequiredPermission("GkhGji.SMEV.SMEVComplaints.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Запросы по досудебному обжалованию", "complaintsrequest").AddRequiredPermission("GkhGji.SMEV.SMEVComplaints.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Запрос номера лицензии в ЕРУЛ", "smeverul").AddRequiredPermission("GkhGji.License.LicenseAction.View");
            root.Add("Справочники").Add("ГЖИ").Add("Решения по жалобам", "complaintsdecdict").AddRequiredPermission("GkhGji.SMEV.SMEVComplaints.View");
            root.Add("Жилищная инспекция").Add("Реестр обращений").Add("Реестр ответов для регистрации", "answerregistration").AddRequiredPermission("GkhGji.AppealCitizens.AppealCitsAnswerRegistration.View").WithIcon("menuManorgRequestLicense");
        }

        public string Key => MainNavigationInfo.MenuName;

        public string Description => MainNavigationInfo.MenuDescription;
    }
}