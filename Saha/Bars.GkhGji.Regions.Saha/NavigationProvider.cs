namespace Bars.GkhGji.Regions.Saha
{
    using Bars.B4;
    using Bars.Gkh.TextValues;

    /// <summary>
    /// Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        public IMenuItemText MenuItemText { get; set; }

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
            root.Add("Справочники").Add("ГЖИ").Add("Правовые основания", "legalreason").AddRequiredPermission("GkhGji.Dict.LegalReason.View");
            root.Add("Справочники").Add("ГЖИ").Add("Предметы проверки", "surveysubject").AddRequiredPermission("GkhGji.Dict.SurveySubject.View");

            var contragent = root.Add("Контрагенты РГИС").AddRequiredPermission("Ris.Contragent");

            var contragentSet = contragent.Add("Контрагенты").AddRequiredPermission("Ris.Contragent");
            contragentSet.Add("Контрагенты", "contragentlist").AddRequiredPermission("Ris.Contragent.Contragent").WithIcon("contragent");

            var contragentRolesSet = contragent.Add("Роли контрагентов").AddRequiredPermission("Ris.Contragent");
            contragentRolesSet.Add("ОГВ", "stategovlist").AddRequiredPermission("Ris.Contragent.StateGov");
            contragentRolesSet.Add("Управляющие организации", "managementorganizationlist").AddRequiredPermission("Ris.Contragent.ManagementOrganization").WithIcon("manOrg");
            contragentRolesSet.Add("ОМС", "localgovlist").AddRequiredPermission("Ris.Contragent.LocalGov").WithIcon("localGovernment");
            contragentRolesSet.Add("РСО", "rsocompanylist").AddRequiredPermission("Ris.Contragent.RsoCompany");
            contragentRolesSet.Add("РЭК", "rekcompanylist").AddRequiredPermission("Ris.Contragent.RekCompany");

            //Жилищный фонд
            var housing = root.Add("Жилищный фонд РГИС").AddRequiredPermission("Ris.Housing");
            var housingOki = housing.Add("Объекты коммунальной инфраструктуры").AddRequiredPermission("Ris.Housing.Oki");
            housingOki.Add("Реестр домов", "houselist").AddRequiredPermission("Ris.Housing.House.HouseList.View").WithIcon("realObj");
            housingOki.Add("Реестр лицевых счетов", "personalaccountlist").AddRequiredPermission("Ris.Housing.PersonalAccount.PersonalAccountList.View");

            //Справочники
            var dict = root.Add("Справочники РГИС").AddRequiredPermission("Ris.Dict").WithIcon("instruction");
            var commonDict = dict.Add("Справочники - Общие").AddRequiredPermission("Ris.Dict.Common");
            commonDict.Add("Должности", "dictnsipostlist").AddRequiredPermission("Ris.Dict.Common.Post");
            commonDict.Add("ОКЕИ", "dictokeilist").AddRequiredPermission("Ris.Dict.Common.Okei").WithIcon("unitMeasure");
            commonDict.Add("Область и тематика НПА", "dictnpaactthemelist").AddRequiredPermission("Ris.Dict.Common.NpaActTheme");
            commonDict.Add("Муниципальные образования", "municipality")
                .AddRequiredPermission("Ris.Dict.Common.Municipality.View").WithIcon("municipality");
            commonDict.Add("Дерево муниципальных образований", "municipalityTree").
                AddRequiredPermission("Ris.Dict.Common.MunicipalityTree.View").WithIcon("municipality");

            var okiDict = dict.Add("Справочники ОКИ/СКИ").AddRequiredPermission("Ris.Dict.Oki");
            okiDict.Add("Вид электростанции", "dictelectrostationtypelist").AddRequiredPermission("Ris.Dict.Oki.ElectroStationType");
            okiDict.Add("Уровень давления газопровода", "dictgaspressurelist").AddRequiredPermission("Ris.Dict.Oki.GasPressure");
            okiDict.Add("Вид коммунальной услуги", "dictcommunalservicelist").AddRequiredPermission("Ris.Dict.Oki.CommunalService");
            okiDict.Add("Вид водозаборного сооружения", "dictwaterintaketypelist").AddRequiredPermission("Ris.Dict.Oki.WaterIntakeType");
            okiDict.Add("Тип газораспред. сети", "dictgasnettypelist").AddRequiredPermission("Ris.Dict.Oki.GasNetType");
            okiDict.Add("Тип электрической подстанции", "dictelectrosubstantiontypelist").AddRequiredPermission("Ris.Dict.Oki.ElectroSubstantionType");
            okiDict.Add("Основание эксплуатации объекта инфр.", "dictrunbaselist").AddRequiredPermission("Ris.Dict.Oki.RunBase");
            okiDict.Add("Вид топлива", "dictfueltypelist").AddRequiredPermission("Ris.Dict.Oki.FuelType");
            okiDict.Add("Вид теплоносителя", "dictheattypelist").AddRequiredPermission("Ris.Dict.Oki.HeatType");
            okiDict.Add("Уровень напряжения", "dictvoltagelist").AddRequiredPermission("Ris.Dict.Oki.Voltage");
            okiDict.Add("Вид коммунальной инфраструктуры", "dictskitypelist").AddRequiredPermission("Ris.Dict.Oki.SkiType");
            okiDict.Add("Вид ОКИ", "dictokitypelist").AddRequiredPermission("Ris.Dict.Oki.OkiType");
            okiDict.Add("Вид коммунального ресурса", "dictcommunalsourcelist").AddRequiredPermission("Ris.Dict.Oki.CommunalSource");

            var houseDict = dict.Add("Справочники Жилищного фонда").AddRequiredPermission("Ris.Dict.House");
            houseDict.Add("Основание заключения договора", "dictcontractconclusionlist").AddRequiredPermission("Ris.Dict.House.ContractConclusion");
            houseDict.Add("Код жилищной услуги", "dicthouseservicelist").AddRequiredPermission("Ris.Dict.House.HouseService");
            houseDict.Add("Категория граждан", "dictbenefitcategorylist").AddRequiredPermission("Ris.Dict.House.BenefitCategory");
            houseDict.Add("Классификатор видов работ (услуг)", "dictworktypelist").AddRequiredPermission("Ris.Dict.House.WorkType");
            houseDict.Add("Категория помещения", "dictpremisecategorylist").AddRequiredPermission("Ris.Dict.House.PremiseCategory");
            houseDict.Add("Тип дома", "dicthousetypelist").AddRequiredPermission("Ris.Dict.House.HouseType");

            var documents = root.Add("Документы").Add("Документы").AddRequiredPermission("Ris.Documents");
            documents.Add("Нормативно правовые акты", "documentsnpaact").AddRequiredPermission("Ris.Documents.NpaAct");

            var appeal = root.Add("Обращения").AddRequiredPermission("Ris.Appeal");

            var appealSet = appeal.Add("Обращения").AddRequiredPermission("Ris.Appeal.Appeal");
            appealSet.Add("Управление обращениями", "appeallist").AddRequiredPermission("Ris.Appeal.Appeal.AppealList");

            var outsideSystemIntegration = root.Add("Интеграция с ГИС ЖКХ");
            var gisIntegration = outsideSystemIntegration.Add("Интеграция с ГИС ЖКХ");

            gisIntegration.Add("Интеграция с ГИС ЖКХ", "gisintegration").AddRequiredPermission("Ris.Integration.Gis");
            gisIntegration.Add("Делегирование ГИС", "delegacy").AddRequiredPermission("Ris.Integration.Delegate");
            gisIntegration.Add("Роли ГИС", "gisrole").AddRequiredPermission("Ris.Integration.Role");

            var externalIntegration = root.Add("Интеграция с внешними системами");
            var import = externalIntegration.Add("Импорт");

            import.Add("Загрузка данных", "importdatakernel").AddRequiredPermission("Ris.ExternalIntegration.ImportData");
            import.Add("Сопоставление адресов", "addressmatching").AddRequiredPermission("Ris.ExternalIntegration.CompareAddress");
            import.Add("Сопоставление услуг", "servicematching").AddRequiredPermission("Ris.ExternalIntegration.ServiceMatching");
            import.Add("Поставщики Формат 4.0", "datasupplier").AddRequiredPermission("Ris.ExternalIntegration.DataSupplier");

            var export = externalIntegration.Add("Экспорт");
            export.Add("Выгрузка данных", "exportdatakernel").AddRequiredPermission("Ris.ExternalIntegration.ExportDataKernel");
        }
    }
}