namespace Bars.Gkh.Regions.Perm
{
    using Bars.B4;
    using Bars.Gkh.TextValues;

    /// <summary>
    /// Меню навигации
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        /// <summary>
        /// Интерфейс для описания тектовых значений пунктов меню
        /// </summary>
        public IMenuItemText MenuItemText { get; set; }

        /// <inheritdoc />
        public void Init(MenuItem root)
        {
            var gji = root.Add("Жилищная инспекция");
            var menuLicense = root.Add("Жилищная инспекция").Add("Лицензирование");

            var item = menuLicense.Get(this.MenuItemText.GetText("Обращения за выдачей лицензии"));
            if (item != null)
            {
                item.Caption = "Реестр заявлений";
            }

            gji.Add("Лицензирование")
                .Add("Форма 1-ГУ", "governmenservice")
                .AddRequiredPermission("GkhGji.Licensing.FormGovService.View");

            gji.Add("Подготовка к отопительному сезону")
               .Add("Форма-2 ЖКХ (Зима)", "gkh2winterlist").AddRequiredPermission("GkhGji.HeatSeason.AvailabilityAndConsumptionFuel");

            var appeal = root.Add("Обращения").AddRequiredPermission("Ris.Appeal");
            var appealSet = appeal.Add("Обращения").AddRequiredPermission("Ris.Appeal.Appeal");
            appealSet.Add("Управление обращениями", "appeallist").AddRequiredPermission("Ris.Appeal.Appeal.AppealList");

            var externalIntegration = root.Add("Интеграция с внешними системами");
            var import = externalIntegration.Add("Импорт");

            import.Add("Загрузка данных", "importdatakernel").AddRequiredPermission("Ris.ExternalIntegration.ImportData");
            import.Add("Сопоставление адресов", "addressmatching").AddRequiredPermission("Ris.ExternalIntegration.CompareAddress");

            var outsideSystemIntegration = root.Add("Интеграция с ГИС ЖКХ");
            var gisIntegration = outsideSystemIntegration.Add("Интеграция с ГИС ЖКХ");

            gisIntegration.Add("Интеграция с ГИС ЖКХ", "gisintegration").AddRequiredPermission("Ris.Integration.Gis");
            gisIntegration.Add("Делегирование ГИС", "delegacy").AddRequiredPermission("Ris.Integration.Delegate");
            gisIntegration.Add("Роли ГИС", "gisrole").AddRequiredPermission("Ris.Integration.Role");

            var gisGji = root.Add("Жилищная инспекция");
            var licensing = gisGji.Add("Лицензирование");

            licensing.Add("Дисквалифицированные лица", "disqualifiedpersons").AddRequiredPermission("Ris.Gji.Licensing.DisqualifiedPersons");
            licensing.Add("Квалификационные аттестаты", "qualificationcertificate").AddRequiredPermission("Ris.Gji.Licensing.QualificationCertificate");

            var gisInforming = root.Add("Информирование");
            gisInforming
                .Add("Информационные сообщения")
                .Add("Информационные сообщения", "informationmessagelist").AddRequiredPermission("Ris.Informing.InformationMessageList");

            var housing = root.Add("Жилищный фонд РГИС").AddRequiredPermission("Ris.Housing");
            var housingOki = housing.Add("Объекты коммунальной инфраструктуры").AddRequiredPermission("Ris.Housing.Oki");
            housingOki.Add("Программы модернизации СКИ", "skimodernizationlist").AddRequiredPermission("Ris.Housing.Oki.SkiModernizationList");
            housingOki.Add("Плановые показатели", "housingtargerslist").AddRequiredPermission("Ris.Housing.Oki.HousingTargersList");

            root.Add("Жилищный фонд")
                .Add("Объекты жилищного фонда")
                .Add("Отчётность по форме 22-ЖКХ", "form22gkh")
                .AddRequiredPermission("Ris.Housing.Form22Gkh");
        }

        /// <inheritdoc />
        public string Key => MainNavigationInfo.MenuName;

        /// <inheritdoc />
        public string Description => MainNavigationInfo.MenuDescription;
    }
}