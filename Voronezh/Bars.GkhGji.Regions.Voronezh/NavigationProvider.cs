namespace Bars.GkhGji.Regions.Voronezh
{
    using Bars.B4;

    /// <summary>
    /// Меню, навигация
    /// </summary>
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

            // реестр рассылок
            root.Add("Жилищная инспекция").Add("Реестр обращений").Add("Реестр исходящих электронных писем", "emaillist").AddRequiredPermission("GkhGji.AppealCitizens.View").WithIcon("businessActivity");
            root.Add("Администрирование").Add("Импорты").Add("Импорт из АМИРС", "amirsimport").AddRequiredPermission("Administration.AMIRS.View");
            root.Add("Администрирование").Add("Импорты").Add("Импорт тарифов и нормативов", "tarifimport").AddRequiredPermission("Import.Appeal.View");
            //СМЭВ
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения о доходах (2-НДФЛ)", "ndfl").AddRequiredPermission("GkhGji.SMEV.SMEVNDFL.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Выгрузка сведений в ГАС Управление", "gasu").AddRequiredPermission("GkhGji.SMEV.SMEVGASU.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения из ЕГРЮЛ", "smevegrul").AddRequiredPermission("GkhGji.SMEV.SMEVEGRUL.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения из ЕГРИП", "smevegrip").AddRequiredPermission("GkhGji.SMEV.SMEVEGRIP.View"); //егрип
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Тестирование СМЭВ", "smevdo").AddRequiredPermission("GkhGji.SMEV.SMEVEGRIP.View"); //егрип
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения о наличии (отсутствии) судимости", "smevmvd").AddRequiredPermission("GkhGji.SMEV.SMEVMVD.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Выгрузка сведений в ФГИС ЕРП", "giserp").AddRequiredPermission("GkhGji.SMEV.GISERP.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Выгрузка сведений в ЕРКНМ", "erknm").AddRequiredPermission("GkhGji.SMEV.GISERP.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Обмен информацией с ГИС ГМП", "gisgmp").AddRequiredPermission("GkhGji.SMEV.GISGMP.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения о признании жилого помещения непригодным для проживания", "premises").AddRequiredPermission("GkhGji.SMEV.SMEVPremises.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Реестр платежей", "payreg").AddRequiredPermission("GkhGji.SMEV.PAYREG.View");
          //  root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Запросы в ГосИмущество", "smevpropertytype").AddRequiredPermission("GkhGji.SMEV.SMEVPropertyType.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Информация из Реестра дисквалифицированных лиц", "diskvlic").AddRequiredPermission("GkhGji.SMEV.SMEVDISKVLIC.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Предоставление СНИЛС", "smevsnils").AddRequiredPermission("GkhGji.SMEV.SMEVDISKVLIC.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения из ФГИС ЕГРН", "smevegrn").AddRequiredPermission("GkhGji.SMEV.SMEVEGRN.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения о разрешении на ввод объекта в эксплуатацию", "exploit").AddRequiredPermission("GkhGji.SMEV.SMEVExploitResolution.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения о переводе жилого (нежилого) помещения в нежилое (жилое) помещение", "changepremisesstate").AddRequiredPermission("GkhGji.SMEV.SMEVExploitResolution.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения о действительности (недействительности) паспорта", "validpassport").AddRequiredPermission("GkhGji.SMEV.SMEVValidPassport.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения о регистрации по месту жительства", "livingplace").AddRequiredPermission("GkhGji.SMEV.SMEVLivingPlace.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения о регистрации по месту пребывания", "stayingplace").AddRequiredPermission("GkhGji.SMEV.SMEVStayingPlace.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения по договорам социального найма жилого помещения", "socialhire").AddRequiredPermission("GkhGji.SMEV.SMEVSocialHire.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения о признании дома аварийным", "emergencyhouse").AddRequiredPermission("GkhGji.SMEV.SMEVSocialHire.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения о согласовании переустройства и (или) перепланировки помещения", "redevelopment").AddRequiredPermission("GkhGji.SMEV.SMEVSocialHire.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сведения о принадлежности имущества к государственной или муниципальной собственности", "owproperty").AddRequiredPermission("GkhGji.SMEV.SMEVSocialHire.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Сводный реестр межведомственных запросов", "interdepartmentalrequestsdtogrid").AddRequiredPermission("GkhGji.SMEV.DepartmentlRequestsDTOController.View");
            //root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Запросы в ЕГРН", "smevegrn").AddRequiredPermission("GkhGji.SMEV.SMEVEGRN.View");
            root.Add("Жилищная инспекция").Add("Межведомственное взаимодействие").Add("Выгрузка сведений о лицензиях в ФНС России", "smevfnslicrequest").AddRequiredPermission("GkhGji.SMEV.SMEVFNSLicRequest.View");
            root.Add("Справочники").Add("ГЖИ").Add("Коды регионов", "regioncode").AddRequiredPermission("GkhGji.Dict.RegionCode.View");
            root.Add("Справочники").Add("ГЖИ").Add("Отделы прокуратуры", "prosecutoroffice").AddRequiredPermission("GkhGji.Dict.ProsecutorOffice.View");
            root.Add("Справочники").Add("ГЖИ").Add("Тип исполнения обращения", "appealexecutiontype").AddRequiredPermission("GkhGji.Dict.AppealExecutionType.View");
            root.Add("Справочники").Add("ГЖИ").Add("Идентификаторы ССТУ", "sstutransferorg").AddRequiredPermission("GkhGji.Dict.SSTUTransferOrg.View");
            root.Add("Справочники").Add("ГЖИ").Add("Коды документов физических лиц", "fldoctype").AddRequiredPermission("GkhGji.Dict.FLDocType.View");
            root.Add("Справочники").Add("ГЖИ").Add("Статусы плательщиков", "gisgmppayerstatus").AddRequiredPermission("GkhGji.SMEV.GISGMP.View"); 
            root.Add("Справочники").Add("ГЖИ").Add("Категория заявителя ЕГРН", "egrnapplicanttype").AddRequiredPermission("GkhGji.Dict.EGRNApplicantType.View");
            root.Add("Справочники").Add("ГЖИ").Add("Объект запроса ЕГРН", "egrnobjecttype").AddRequiredPermission("GkhGji.Dict.EGRNObjectType.View");
            root.Add("Справочники").Add("ГЖИ").Add("Документы ЕГРН", "egrndoctype").AddRequiredPermission("GkhGji.Dict.EGRNDocType.View");
            root.Add("Жилищная инспекция").Add("Основания проверок").Add("Плановые проверки ОМСУ", "baseomsu").AddRequiredPermission("GkhGji.Inspection.BaseOMSU.View").WithIcon("baseJurPerson");

            root.Add("Жилищная инспекция").Add("Риск-ориентированный подход").Add("Виды КНД", "kindknddict").WithIcon("reminderHead").AddRequiredPermission("GkhGji.RiskOrientedMethod.KindKNDDict.View");
            root.Add("Жилищная инспекция").Add("Риск-ориентированный подход").Add("Расчет категории риска", "romcategory").WithIcon("heatSeasonMassStateChange").AddRequiredPermission("GkhGji.RiskOrientedMethod.ROMCategory.View");
            root.Add("Жилищная инспекция").Add("Риск-ориентированный подход").Add("Массовый расчет категорий риска", "romcalctask").WithIcon("estimate").AddRequiredPermission("GkhGji.RiskOrientedMethod.ROMCategory.View");
            root.Add("Жилищная инспекция").Add("Риск-ориентированный подход").Add("Субъекты проверок ЛК", "licensecontrolobj").AddRequiredPermission("GkhGji.RiskOrientedMethod.ROMCategory.View");
            root.Add("Жилищная инспекция").Add("Риск-ориентированный подход").Add("Субъекты проверок ЖН", "housingspvobj").AddRequiredPermission("GkhGji.RiskOrientedMethod.ROMCategory.View");
            root.Add("Жилищная инспекция").Add("Риск-ориентированный подход").Add("Показатели эффективности КНД", "effectivekndindex").WithIcon("baseProsClaim").AddRequiredPermission("GkhGji.RiskOrientedMethod.EffectiveKNDIndex.View");

            root.Add("Жилищная инспекция").Add("Судебная практика").Add("Реестр судебной практики", "courtpractice").WithIcon("reminderHead").AddRequiredPermission("GkhGji.CourtPractice.CourtPracticeRegystry.View");
            root.Add("Жилищная инспекция").Add("Реестр обращений").Add("Экспорт обращений в ССТУ", "sstuexporttask").AddRequiredPermission("GkhGji.DocumentsGji.View").WithIcon("appealCits");
            root.Add("Жилищная инспекция").Add("Реестр обращений").Add("Реестр СОПР", "appealorder").AddRequiredPermission("GkhGji.SOPR.Appeal.View").WithIcon("appealCits");


            root.Add("Жилищная инспекция").Add("Реестр обращений").Add("Реестр нарушителей ВДГО", "vdgoviolators").AddRequiredPermission("GkhGji.VDGOViolators.View").WithIcon("appealCits");


            root.Add("Жилищная инспекция").Add("Реестр обращений").Add("Заявки на доступ к протоколам ОСС", "protocolosprequest").AddRequiredPermission("GkhGji.ProtocolOSPRequest.View").WithIcon("appealCits");

            //root.Add("Жилищная инспекция").Add("Реестр обращений").Add("Реестр нарушителей ВДГО", "vdgoviolators").WithIcon("appealCits");


            // Переоформление лицензии
            var menuLicense = root.Add("Жилищная инспекция").Add("Лицензирование");
            menuLicense.Add("Обращения за переоформлением лицензии", "licensereissuance").AddRequiredPermission("Gkh.ManOrgLicense.Request.View").WithIcon("menuManorgRequestLicense");
            root.Add("Жилищная инспекция").Add("Основания проверок").Add("Проверки по переоформлению лицензий", "baselicensereissuance").AddRequiredPermission("GkhGji.Inspection.BaseLicApplicants.View");

            root.Add("Жилищная инспекция")
          .Add("Документы")
          .Add("Протоколы без проведения проверок", "protocol197")
          .AddRequiredPermission("GkhGji.DocumentsGji.Protocol197.View");

            root.Add("Жилищная инспекция").Add("Документы").Add("Реестр предостережений", "admonition").AddRequiredPermission("GkhGji.AppealCitizensState.AppealCitsAdmonition.View").WithIcon("resolPros");
            root.Add("Обращения").Add("Обращения граждан").Add("Реестр предписаний ФКР", "prescriptionfond").AddRequiredPermission("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.View").WithIcon("resolPros");

            root.Add("Жилищная инспекция")
          .Add("Документы")
          .Add("Архив документов ГЖИ", "fileregister")
          .AddRequiredPermission("GkhGji.FileRegister.View");
        }
    }
}
