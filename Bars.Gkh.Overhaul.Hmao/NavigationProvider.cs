namespace Bars.Gkh.Overhaul.Hmao
{
    using Bars.B4;

    public class NavigationProvider : INavigationProvider
    {
        public string Key => MainNavigationInfo.MenuName;

        public string Description => MainNavigationInfo.MenuDescription;

        public void Init(MenuItem root)
        {
            root.Add("Жилищный фонд").Add("Объекты жилищного фонда").Add("Реестр протоколов ОСС в МКД", "protocolmkd").AddRequiredPermission("Gkh.RealityObject.Register.OwnerProtocol.View").WithIcon("bankStatement");
            //root.Add("Администрирование").Add("Импорт жилых домов").Add("Импорт жилых домов для Новосибирска", "nsorealityobjimport").AddRequiredPermission("Import.HmaoRealtyObjectImport.View");
            //root.Add("Администрирование").Add("Импорт жилых домов").Add("Импорт из фонда домов (часть 3)", "roimportfromfundpart3").AddRequiredPermission("Import.RoImportFromFundPart3.View");
            //root.Add("Администрирование").Add("Импорт жилых домов").Add("Импорт из фонда домов (часть 5)", "roimportfromfundpart5").AddRequiredPermission("Import.RoImportFromFundPart5.View");
            root.Add("Администрирование").Add("Импорт жилых домов").Add("Импорт работ по конструктивным элементам", "worksimportbystructelements").AddRequiredPermission("Import.WorksImportByStructElements.View");
            //root.Add("Администрирование").Add("Импорты").Add("Импорт ДПКР из 1С", "dpkr1cdataimport");

            root.Add("Участники процесса").Add("Контрагенты").Add("Кредитные организации", "creditorg").AddRequiredPermission("Ovrhl.CreditOrg.View");
            root.Add("Справочники").Add("Капитальный ремонт").Add("Операции по счету", "accountoperation").AddRequiredPermission("Ovrhl.Dictionaries.AccountOperation.View");
            root.Add("Справочники").Add("Капитальный ремонт").Add("Периоды программ КР", "crperiod").AddRequiredPermission("Ovrhl.Dictionaries.CrPeriod.View");
            var longProgram = root.Add("Капитальный ремонт").Add("Региональная программа");

            //longProgram.Add("Первый этап", "programfirststage").AddRequiredPermission("Ovrhl.Program1Stage.View");
            longProgram.Add("Долгосрочная программа", "dpkr").AddRequiredPermission("Ovrhl.LongTermProgram.View").WithIcon("longProgram");
            longProgram.Add("Подпрограмма", "subdpkr").AddRequiredPermission("Ovrhl.LongTermSubProgram.View").WithIcon("longProgram");

            longProgram.Add("Субсидирование", "subsidy").AddRequiredPermission("Ovrhl.Subcidy.View").WithIcon("subsidy");
            longProgram.Add("Опубликованные программы", "publicationprogs").AddRequiredPermission("Ovrhl.PublicationProgs.View").WithIcon("publishPrograms");
            longProgram.Add("Дефицит по МО", "shortprogramdef").AddRequiredPermission("Ovrhl.ShortProgramDeficit.View").WithIcon("deficitMo");
            longProgram.Add("Краткосрочная программа", "shortprogram").AddRequiredPermission("Ovrhl.ShortProgram.View").WithIcon("shortProgram");
            longProgram.Add("Загрузка программы", "loadprogram").AddRequiredPermission("Ovrhl.LoadProgram.View");
            longProgram.Add("Массовый расчет ДПКР", "masscalclongprogram").AddRequiredPermission("Ovrhl.MassCalcLongProgram.View");
            longProgram.Add("Документы ДПКР", "dpkrdocument").AddRequiredPermission("Ovrhl.DpkrDocument.View");
            
            var programParams = root.Add("Капитальный ремонт").Add("Параметры программы капитального ремонта");
            programParams.Add("Версии программы", "dpkr_versions").AddRequiredPermission("Ovrhl.ProgramVersions.View").WithIcon("programVersion");
            programParams.Add("Тариф по типам домов", "realestatetyperate").AddRequiredPermission("Ovrhl.RealEstateTypeRate.View");
            programParams.Add("Доли финансирования работ", "sharefinancingceo").AddRequiredPermission("Ovrhl.Dictionaries.ShareFinancingCeo.View").WithIcon("deficitMo");
            programParams.Add("Параметры очередности", "priorityparam").AddRequiredPermission("Ovrhl.PriorityParam.View").WithIcon("unitMeasure");
            programParams.Add("Параметры актуализации ДПКР", "actualisedpkr").AddRequiredPermission("Ovrhl.ActualiseDPKR.View").WithIcon("billing");
            programParams.Add("Критерии отбора в подпрограмму", "actualisesubprogram").AddRequiredPermission("Ovrhl.ActualiseSubProgram.View").WithIcon("subProgram");
            programParams.Add("Рассчет средней стоимости", "econfeasibilitycalc").AddRequiredPermission("Ovrhl.EconFeasibilityCalc.View");


            var longProgramObjects = root.Add("Капитальный ремонт").Add("Объекты региональной программы капитального ремонта");
            longProgramObjects.Add("Реестр объектов региональной программы", "longtermprobject").AddRequiredPermission("Ovrhl.LongTermProgramObject.View").WithIcon("longProgramRegistry");
            longProgramObjects.Add("Реестр уведомления о формировании фонда КР", "decisiomnoticeregister").AddRequiredPermission("Ovrhl.DecisionNoticeRegister.View");

            //root.Add("Региональный фонд").Add("Формирование регионального фонда").Add("Модуль начисления", "billing").AddRequiredPermission("Ovrhl.Billing").WithIcon("billing");
            //root.Add("Региональный фонд").Add("Использование регионального фонда").Add("Реестр займов", "loanregister").AddRequiredPermission("Ovrhl.LoanRegister.View");

            root.Add("Справочники").Add("Капитальный ремонт").Add("Предельные стоимости работ и услуг", "costlimit").AddRequiredPermission("Ovrhl.Dictionaries.CostLimit.View");
            root.Add("Справочники").Add("Капитальный ремонт").Add("Предельные стоимости работ и услуг в разрезе ООИ", "costlimitooi").AddRequiredPermission("Ovrhl.Dictionaries.CostLimitOOI.View");

            root.Add("Справочники").Add("Капитальный ремонт").Add("Виды протоколов собрания собственников", "ownerprottype").AddRequiredPermission("Ovrhl.Dictionaries.OwnerProtocolType.View");

            root.Add("Справочники").Add("Капитальный ремонт").Add("Критерии для актуализации регпрограммы", "criterias").AddRequiredPermission("Ovrhl.Dictionaries.CriteriaForActualizeVersion.View");
        }
    }
}