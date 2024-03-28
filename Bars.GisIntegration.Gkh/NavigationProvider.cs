namespace Bars.GisIntegration.Gkh
{
    using Bars.B4;
    using Bars.B4.Navigation;

    public class NavigationProvider : BaseMainMenuProvider
    {
        public override void Init(MenuItem root)
        {
            var administration = root.Add("Администрирование");

            var outsideSystemIntegrations = administration.Add("Интеграция с внешними системами");

            //outsideSystemIntegrations.Add("Интеграция с ГИС", "gisintegration").AddRequiredPermission("Administration.OutsideSystemIntegrations.Gis.View");
            outsideSystemIntegrations.Add("Делегирование ГИС", "delegacy").AddRequiredPermission("Administration.OutsideSystemIntegrations.Delegacy.View");
            outsideSystemIntegrations.Add("Роли ГИС", "gisrole").AddRequiredPermission("Administration.OutsideSystemIntegrations.GisRole.View");
            outsideSystemIntegrations.Add("Роли контрагентов РИС", "riscontragentrole").AddRequiredPermission("Administration.OutsideSystemIntegrations.RisContragentRole.View");

            administration
              .Add("Импорт/экспорт данных системы")
              .Add("Импорт инкрементальных данных для ГИС ЖКХ", "importdatakernel")
              .AddRequiredPermission("Ris.Administration.Import.ImportData");

            administration
               .Add("Импорт/экспорт данных системы")
               .Add("Поставщики данных для ГИС ЖКХ", "datasupplier")
               .AddRequiredPermission("Ris.Administration.Import.DataSupplier");

            administration
               .Add("Импорт/экспорт данных системы")
               .Add("Сопоставление услуг для ГИС ЖКХ", "servicematching")
               .AddRequiredPermission("Ris.Administration.Import.ServiceMatching");

            administration
               .Add("Импорт/экспорт данных системы")
               .Add("Сопоставление адресов для ГИС ЖКХ", "addressmatching")
               .AddRequiredPermission("Ris.Administration.Import.ImportData");

            root.Add("Жилищный фонд").Add("Объекты коммунальной инфраструктуры").Add("СКИ", "housingskilist").AddRequiredPermission("Ris.Housing.Ski.SkiList");

            root.Add("Жилищный фонд").Add("Объекты коммунальной инфраструктуры").Add("ОКИ", "housingokilist").AddRequiredPermission("Ris.Housing.Oki.OkiList").WithIcon("okiPaspport");
            root.Add("Жилищный фонд").Add("Объекты коммунальной инфраструктуры").Add("СКИ", "housingskilist").AddRequiredPermission("Ris.Housing.Ski.SkiList");

            root.Add("Жилищный фонд").Add("Энергоэффективность и энергосбережение").Add("Реестр программ энергосбережения", "energysavingprogramlist").AddRequiredPermission("Ris.Housing.EnergySaving.ProgramList").WithIcon("resettlementProg");
            root.Add("Жилищный фонд").Add("Энергоэффективность и энергосбережение").Add("Перечни обязательных мероприятий", "energysavinghouseprogramlist").AddRequiredPermission("Ris.Housing.EnergySaving.HouseProgramList").WithIcon("resettlementProg");
            root.Add("Жилищный фонд").Add("Энергоэффективность и энергосбережение").Add("Энергосервисные договора (контракты)", "energysavingescontractlist").AddRequiredPermission("Ris.Housing.EnergySaving.EsContractList").WithIcon("resettlementProg");

            //root.Add("Участники процесса").Add("Контрагенты").Add("Физические лица", "personlist").AddRequiredPermission("Ris.Contragent.SocialSupport.PersonList").WithIcon("menuPerson");

            root.Add("Справочники").Add("Общие").Add("ОКЕИ", "dictokeilist").AddRequiredPermission("Ris.Dict.Common.Okei").WithIcon("unitMeasure");
            root.Add("Справочники").Add("Общие").Add("Область и тематика НПА", "dictnpaactthemelist").AddRequiredPermission("Ris.Dict.Common.NpaActTheme");

            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Вид электростанции", "dictelectrostationtypelist").AddRequiredPermission("Ris.Dict.Oki.ElectroStationType");
            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Уровень давления газопровода", "dictgaspressurelist").AddRequiredPermission("Ris.Dict.Oki.GasPressure");
            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Вид коммунальной услуги", "dictcommunalservicelist").AddRequiredPermission("Ris.Dict.Oki.CommunalService");
            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Вид водозаборного сооружения", "dictwaterintaketypelist").AddRequiredPermission("Ris.Dict.Oki.WaterIntakeType");
            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Тип газораспред. сети", "dictgasnettypelist").AddRequiredPermission("Ris.Dict.Oki.GasNetType");
            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Тип электрической подстанции", "dictelectrosubstantiontypelist").AddRequiredPermission("Ris.Dict.Oki.ElectroSubstantionType");
            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Основание эксплуатации объекта инфр.", "dictrunbaselist").AddRequiredPermission("Ris.Dict.Oki.RunBase");
            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Вид топлива", "dictfueltypelist").AddRequiredPermission("Ris.Dict.Oki.FuelType");
            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Вид теплоносителя", "dictheattypelist").AddRequiredPermission("Ris.Dict.Oki.HeatType");
            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Уровень напряжения", "dictvoltagelist").AddRequiredPermission("Ris.Dict.Oki.Voltage");
            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Вид коммунальной инфраструктуры", "dictskitypelist").AddRequiredPermission("Ris.Dict.Oki.SkiType");
            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Вид ОКИ", "dictokitypelist").AddRequiredPermission("Ris.Dict.Oki.OkiType");
            root.Add("Справочники").Add("Справочники ОКИ/СКИ").Add("Вид коммунального ресурса", "dictcommunalsourcelist").AddRequiredPermission("Ris.Dict.Oki.CommunalSource");

            root.Add("Справочники").Add("Справочники Жилищного фонда").Add("Основание заключения договора", "dictcontractconclusionlist").AddRequiredPermission("Ris.Dict.House.ContractConclusion");
            root.Add("Справочники").Add("Справочники Жилищного фонда").Add("Код жилищной услуги", "dicthouseservicelist").AddRequiredPermission("Ris.Dict.House.HouseService");
            root.Add("Справочники").Add("Справочники Жилищного фонда").Add("Категория льгот", "dictbenefitcategorylist").AddRequiredPermission("Ris.Dict.House.BenefitCategory");
            root.Add("Справочники").Add("Справочники Жилищного фонда").Add("Классификатор видов работ (услуг)", "dictworktypelist").AddRequiredPermission("Ris.Dict.House.WorkType");
            root.Add("Справочники").Add("Справочники Жилищного фонда").Add("Категория помещения", "dictpremisecategorylist").AddRequiredPermission("Ris.Dict.House.PremiseCategory");
            root.Add("Справочники").Add("Справочники Жилищного фонда").Add("Тип дома", "dicthousetypelist").AddRequiredPermission("Ris.Dict.House.HouseType");

            root.Add("Документы").Add("Документы").Add("Нормативно правовые акты", "documentsnpaact").AddRequiredPermission("Ris.Documents.NpaAct");
            root.Add("Социальная поддержка").Add("Социальная поддержка").Add("Реестр льгот", "benefitlist").AddRequiredPermission("Ris.SocialSupport.SocialSupport.BenefitList").WithIcon("debtorClaimWork");
        }
    }
}