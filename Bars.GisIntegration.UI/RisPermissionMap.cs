namespace Bars.GisIntegration.UI
{
    using Bars.B4;
    public class RisPermissionMap : PermissionMap
    {
        public RisPermissionMap()
        {
            //пункты редакитрования РИС
            this.Namespace("Ris", "Редактирование объектов РИС");

            this.Namespace("Ris.Administration", "Администрирование");
            this.Permission("Ris.Administration.Import", "Импорт");
            this.Permission("Ris.Administration.Import.ImportData", "Импорт данных");
            this.Permission("Ris.Administration.Import.DataSupplier", "Поставщики данных");
            this.Permission("Ris.Administration.Import.ServiceMatching", "Сопоставление услуг");
            this.Permission("Ris.Administration.Import.ProcessingData", "Обработка загруженных данных");

            this.Permission("Ris.Dict", "Справочники");
            this.Permission("Ris.Dict.Common", "Справочники общие");
            this.Permission("Ris.Dict.Common.Post", "Должности");
            this.Permission("Ris.Dict.Common.MoTerritory", "Муниципальные образования");
            this.Permission("Ris.Dict.Common.Okei", "ОКЕИ");
            this.Permission("Ris.Dict.Common.NpaActTheme", "Область и тематика НПА");
            this.Permission("Ris.Dict.Common.Municipality", "Муниципальные образования");
            this.Permission("Ris.Dict.Common.MunicipalityTree", "Дерево муниципальных образований");

            this.Permission("Ris.Dict.Oki", "Справочники ОКИ/СКИ");
            this.Permission("Ris.Dict.Oki.GasPressure", "Уровень давления газопровода");
            this.Permission("Ris.Dict.Oki.ElectroStationType", "Вид электростанции");
            this.Permission("Ris.Dict.Oki.CommunalService", "Вид коммунальной услуги");
            this.Permission("Ris.Dict.Oki.WaterIntakeType", "Вид водозаборного сооружения");
            this.Permission("Ris.Dict.Oki.GasNetType", "Тип газораспределительной сети");
            this.Permission("Ris.Dict.Oki.ElectroSubstantionType", "Тип электрической подстанции");
            this.Permission("Ris.Dict.Oki.RunBase", "Основание эксплуатации объекта инфраструктуры");
            this.Permission("Ris.Dict.Oki.FuelType", "Вид топлива");
            this.Permission("Ris.Dict.Oki.HeatType", "Вид теплоносителя");
            this.Permission("Ris.Dict.Oki.Voltage", "Уровень напряжения");
            this.Permission("Ris.Dict.Oki.SkiType", "Вид коммунальной инфраструктуры");
            this.Permission("Ris.Dict.Oki.CommunalSource", "Вид коммунального ресурса");

            this.Permission("Ris.Dict.Oki.OkiType", "Вид ОКИ");
            this.Permission("Ris.Dict.Oki.OkiType.OkiTypeDetails", "Общие сведения");
            this.Permission("Ris.Dict.Oki.OkiType.CommunalService", "Виды коммунальных услуг");
            this.Permission("Ris.Dict.Oki.OkiType.Section", "Доступность разделов");

            this.Permission("Ris.Dict.House", "Справочники Жилищного фонда");
            this.Permission("Ris.Dict.House.ContractConclusion", "Основание заключения договора");
            this.Permission("Ris.Dict.House.HouseService", "Код жилищной услуги");
            this.Permission("Ris.Dict.House.BenefitCategory", "Категория льгот");
            this.Permission("Ris.Dict.House.WorkType", "Классификатор видов работ (услуг)");
            this.Permission("Ris.Dict.House.PremiseCategory", "Категория помещения");
            this.Permission("Ris.Dict.House.HouseType", "Тип дома");

            this.Permission("Ris.Documents", "Документы");
            this.Permission("Ris.Documents.NpaAct", "Нормативно правовые акты");

            this.Permission("Ris.Appeal", "Обращения");
            this.Permission("Ris.Appeal.Appeal", "Обращения");
            this.Permission("Ris.Appeal.Appeal.AppealList", "Управление обращениями");
            this.Permission("Ris.Appeal.Appeal.Appeal", "Дерево обращений");
            this.Permission("Ris.Appeal.Appeal.Appeal.AppealDetails", "Обращение");
            this.Permission("Ris.Appeal.Appeal.Appeal.AppealAttachment", "Обращение (вложения)");
            this.Permission("Ris.Appeal.Appeal.Appeal.AppealAnswer", "Обращение (ответы)");

            this.Permission("Ris.Contragent", "Контрагенты");
            this.Permission("Ris.Contragent.Contragent", "Контрагент");
            this.Permission("Ris.Contragent.Contragent.ContragentDetails", "Общие сведения");
            this.Permission("Ris.Contragent.Contragent.ContragentTypeList", "Типы контрагентов");
            this.Permission("Ris.Contragent.Contragent.SroMember", "Участие в СРО");
            this.Permission("Ris.Contragent.Contragent.ContragentStaff", "Должностные лица");

            this.Permission("Ris.Contragent.ManagementOrganization", "Управляющая организация");
            this.Permission("Ris.Contragent.ManagementOrganization.ManagementOrganizationDetails", "Общие сведения");
            this.Permission("Ris.Contragent.ManagementOrganization.RegularStaffing", "Штатная численность");
            this.Permission("Ris.Contragent.ManagementOrganization.WorkMode", "Режим работы");
            this.Permission("Ris.Contragent.ManagementOrganization.TsgControlMember", "Правление/Ревизионная комиссия");
            this.Permission("Ris.Contragent.ManagementOrganization.DispatchingService", "Диспетчерская служба");

            this.Permission("Ris.Contragent.StateGov", "Органы Государственной власти");
            this.Permission("Ris.Contragent.StateGov.StateGovDetails", "Общие сведения");
            this.Permission("Ris.Contragent.StateGov.WorkMode", "Режим работы");
            this.Permission("Ris.Contragent.StateGov.NPA", "Основание полномочий");

            this.Permission("Ris.Contragent.LocalGov", "Орган местного самоуправления");
            this.Permission("Ris.Contragent.LocalGov.LocalGovDetails", "Общие сведения");
            this.Permission("Ris.Contragent.LocalGov.WorkMode", "Режим работы");

            this.Permission("Ris.Contragent.RsoCompany", "Ресурсоснабжающая организация");
            this.Permission("Ris.Contragent.RsoCompany.RsoCompanyDetails", "Общие сведения");
            this.Permission("Ris.Contragent.RsoCompany.WorkMode", "Режим работы");
            this.Permission("Ris.Contragent.RsoCompany.DispatchingService", "Диспетчерская служба");

            this.Permission("Ris.Contragent.RekCompany", "Региональная энергетическая компания");
            this.Permission("Ris.Contragent.RekCompany.RekCompanyDetails", "Общие сведения");
            this.Permission("Ris.Contragent.RekCompany.WorkMode", "Режим работы");

            this.Permission("Ris.Contragent.SroOrg", "Саморегулируемые организации");
            this.Permission("Ris.Contragent.SroOrg.SroOrgDetails", "Общие сведения");

            //Жилищный фонд
            this.Permission("Ris.Housing", "Жилищный фонд");

            this.Permission("Ris.Housing.Oki", "Объекты коммунальной инфраструктуры");
            this.Permission("Ris.Housing.Oki.OkiList", "Реестр ОКИ");
            this.Permission("Ris.Housing.Oki.HousingOkiDetails", "Общие сведения");
            this.Permission("Ris.Housing.Oki.ManagBase", "Основание управления");
            this.Permission("Ris.Housing.Oki.TransferSource", "Транспортировка коммунальных ресурсов");
            this.Permission("Ris.Housing.Oki.NetOkiObject", "Характеристика сетевого объекта");
            this.Permission("Ris.Housing.Oki.CommunalSourceProduction", "Производство коммунального ресурса");
            this.Permission("Ris.Housing.Oki.NetPart", "Участки сети");
            this.Permission("Ris.Housing.Oki.EnergyEffectDoc", "Энергетическая эффективность");
            this.Permission("Ris.Housing.Oki.HousingOkiSource", "Источники");
            this.Permission("Ris.Housing.Oki.HousingOkiReceiver", "Приемники");
            this.Permission("Ris.Housing.Oki.SkiModernizationList", "Программы модернизации СКИ");
            this.Permission("Ris.Housing.Oki.HousingTargersList", "Плановые показатели");

            // Отчётность по форме 22-ЖКХ
            this.Permission("Ris.Housing.Form22Gkh", "Отчётность по форме 22-ЖКХ");

            //СКИ
            this.Permission("Ris.Housing.Ski", "Cистемы коммунальной инфраструктуры");
            this.Permission("Ris.Housing.Ski.SkiList", "Реестр СКИ");
            this.Permission("Ris.Housing.Ski.HousingSkiDetails", "Общие сведения");
            this.Permission("Ris.Housing.Ski.SkiSource", "Источники");
            this.Permission("Ris.Housing.Ski.SkiNet", "Сети");

            //Дома
            this.Permission("Ris.Housing.House", "Дома");
            this.Permission("Ris.Housing.House.HouseList", "Реестр домов");
            this.Permission("Ris.Housing.House.HouseDetails", "Общие сведения");
            this.Permission("Ris.Housing.House.HousePorch", "Сведения о подъездах");
            this.Permission("Ris.Housing.House.HouseRoom", "Сведения о комнатах");
            this.Permission("Ris.Housing.House.HousePremise", "Сведения о помещениях");
            this.Permission("Ris.Housing.House.ProtocolOss", "Протоколы решений");
            this.Permission("Ris.Housing.House.HouseCommunalRoom", "Сведения о комнатах коммунального заселения");
            this.Permission("Ris.Housing.House.HouseEnergySurvey", "Энергетическое обследование");

            //Лицевые счета
            this.Permission("Ris.Housing.PersonalAccount", "Лицевые счета");
            this.Permission("Ris.Housing.PersonalAccount.PersonalAccountList", "Реестр лицевых счетов");
            this.Permission("Ris.Housing.PersonalAccount.PersonalAccountDetails", "Общие сведения");
            this.Permission("Ris.Housing.PersonalAccount.ServiceQuality", "Качество услуг");
            this.Permission("Ris.Housing.PersonalAccount.ServiceBreak", "Перерывы в поставке");
            this.Permission("Ris.Housing.PersonalAccount.AddressLink", "Связь с помещениями");

            //Энергоэффективность и энергосбережение
            this.Permission("Ris.Housing.EnergySaving", "Энергоэффективность и энергосбережение");
            this.Permission("Ris.Housing.EnergySaving.ProgramList", "Реестр программ энергосбережения и повышения энергетической эффективности");
            this.Permission("Ris.Housing.EnergySaving.ProgramList.ProgramDetails", "Сведения о программе");
            this.Permission("Ris.Housing.EnergySaving.ProgramList.ProgramStage", "Сведения о мероприятиях");
            this.Permission("Ris.Housing.EnergySaving.ProgramList.ProgramTargetPlan", "Сведения о целевых показателях");
            this.Permission("Ris.Housing.EnergySaving.ProgramList.ProgramTargetFact", "Сведения о фактических показателях");
            this.Permission("Ris.Housing.EnergySaving.ProgramList.ProgramReport", "Отчеты о ходе реализации");
            this.Permission("Ris.Housing.EnergySaving.ProgramList.ProgramMunicipality", "Территория действия");

            //Перечни обязательных мероприятий в отношении ОИИ
            this.Permission("Ris.Housing.EnergySaving.HouseProgramList", "Перечни обязательных мероприятий в отношении ОИИ");
            this.Permission("Ris.Housing.EnergySaving.HouseProgramList.HouseProgramDetails", "Сведения о перечне");
            this.Permission("Ris.Housing.EnergySaving.HouseProgramList.HouseProgramReport", "Отчеты о выполнении перечня мероприятий");
            this.Permission("Ris.Housing.EnergySaving.HouseProgramList.HouseProgramStage", "Сведения о мероприятиях");
            this.Permission("Ris.Housing.EnergySaving.HouseProgramList.HouseProgramTargetPlan", "Сведения о целевых показателях");
            this.Permission("Ris.Housing.EnergySaving.HouseProgramList.HouseProgramTargetFact", "Сведения о фактических показателях");
            this.Permission("Ris.Housing.EnergySaving.HouseProgramList.HouseList", "Список домов");

            //Энергосервисные договора (контракты)
            this.Permission("Ris.Housing.EnergySaving.EsContractList", "Энергосервисные договора (контракты)");
            this.Permission("Ris.Housing.EnergySaving.EsContractList.EsContractDetails", "Общие сведения");
            this.Permission("Ris.Housing.EnergySaving.EsContractList.EsContractActOfCompletedWork", "Акты выполненных работ");
            this.Permission("Ris.Housing.EnergySaving.EsContractList.EsContractEconomyPeriod", "Периоды долей экономии");
            this.Permission("Ris.Housing.EnergySaving.EsContractList.EsContractEconomy", "Сведения об экономии");
            this.Permission("Ris.Housing.EnergySaving.EsContractList.EsContractStage", "Сведения о мероприятиях");
            this.Permission("Ris.Housing.EnergySaving.EsContractList.EsContractParties", "Сведения о сторонах договора");

            //Информирование
            this.Permission("Ris.Informing", "Информирование");
            this.Permission("Ris.Informing.InformationMessage", "Информационные сообщения");
            this.Permission("Ris.Informing.InformationMessage.InformationMessageList", "Информационные сообщения - Список");
            this.Permission("Ris.Informing.InformationMessage.InformationMessageDetails", "Сообщение");
            this.Permission("Ris.Informing.InformationMessage.InformationMessageAddress", "Адресаты");
            this.Permission("Ris.Informing.InformationMessage.InformationMessageDoc", "Вложения");

            //Социальная поддержка
            this.Permission("Ris.SocialSupport", "Социальная поддержка");
            this.Permission("Ris.SocialSupport.SocialSupport", "Социальная поддержка");
            this.Permission("Ris.SocialSupport.SocialSupport.BenefitList", "Реестр льгот");
            this.Permission("Ris.SocialSupport.SocialSupport.Benefit", "Льготы");
            this.Permission("Ris.SocialSupport.SocialSupport.Benefit.BenefitDetails", "Общие сведения");
            this.Permission("Ris.SocialSupport.SocialSupport.Benefit.BenefitDecision", "Решение о предоставлении");

            //Реестр физических лиц
            this.Permission("Ris.Contragent.SocialSupport.PersonList", "Реестр физических лиц");

            this.Permission("Ris.Gji", "Жилищная инспекция");
            this.Permission("Ris.Gji.Licensing", "Лицензирование");
            this.Permission("Ris.Gji.Licensing.DisqualifiedPersons", "Дисквалифицированные лица");
            this.Permission("Ris.Gji.Licensing.QualificationCertificate", "Квалификационные аттестаты");

            this.Permission("Ris.Informing", "Информирование");
            this.Permission("Ris.Informing.InformationMessageList", "Информационные сообщения");

            //Реестр физических лиц
            this.Permission("Ris.Contragent.SocialSupport.PersonList", "Реестр физических лиц");
            this.Permission("Ris.Contragent", "Контрагенты");
            this.Permission("Ris.Contragent.Contragent", "Контрагент");
            this.Permission("Ris.Contragent.ManagementOrganization", "Управляющая организация");
            this.Permission("Ris.Contragent.StateGov", "Органы Государственной власти");
            this.Permission("Ris.Contragent.LocalGov", "Орган местного самоуправления");
            this.Permission("Ris.Contragent.RsoCompany", "Ресурсоснабжающая организация");
            this.Permission("Ris.Contragent.RekCompany", "Региональная энергетическая компания");
            this.Permission("Ris.Contragent.SroOrg", "Саморегулируемые организации");

            //Жилищный фонд
            this.Permission("Ris.Housing", "Жилищный фонд");
            //Дома
            this.Permission("Ris.Housing.House", "Дома");
            //Лицевые счета
            this.Permission("Ris.Housing.PersonalAccount", "Лицевые счета");
            //Перечни обязательных мероприятий в отношении ОИИ
            this.Permission("Ris.Housing.EnergySaving.HouseProgramList", "Перечни обязательных мероприятий в отношении ОИИ");
            //Энергосервисные договора (контракты)
            this.Permission("Ris.Housing.EnergySaving.EsContractList", "Энергосервисные договора (контракты)");

            //Информирование
            this.Permission("Ris.Informing", "Информирование");
            this.Permission("Ris.Informing.InformationMessage", "Информационные сообщения");
            this.Permission("Ris.Dict.Oki", "Справочники ОКИ/СКИ");
            this.Permission("Ris.Dict.Oki.OkiType", "Вид ОКИ");
            this.Permission("Ris.Dict.House", "Справочники Жилищного фонда");

            this.Namespace("Ris.Integration", "Интеграция с ГИС ЖКХ");
            this.Permission("Ris.Integration.Gis", "Интеграция с ГИС ЖКХ");
            this.Permission("Ris.Integration.Delegate", "Делегирование ГИС");
            this.Permission("Ris.Integration.Role", "Роли ГИС");

            this.Namespace("Ris.ExternalIntegration", "Интеграция с внешними системами");
            this.Permission("Ris.ExternalIntegration.ImportData", "Загрузка данных");
            this.Permission("Ris.ExternalIntegration.CompareAddress", "Сопоставление адресов");
            this.Permission("Ris.ExternalIntegration.ServiceMatching", "Сопоставление услуг");
            this.Permission("Ris.ExternalIntegration.DataSupplier", "Поставщики Формат 4.0");
            this.Permission("Ris.ExternalIntegration.ExportDataKernel", "Выгрузка данных");
        }
    }
}
