namespace Bars.Gkh.Overhaul.Nso
{
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            #region Жилой дом

            Namespace("Gkh.RealityObject.Register.OwnerProtocol", "Протоколы и решения собственников");
            CRUDandViewPermissions("Gkh.RealityObject.Register.OwnerProtocol");

            #endregion Жилой дом

            Namespace("Ovrhl", "Капитальный ремонт");

            Namespace("Ovrhl.Program1Stage", "Долгосрочная программа (1 этап)");
            Permission("Ovrhl.Program1Stage.View", "Просмотр");
            Permission("Ovrhl.Program1Stage.MakeStage1", "Сформировать план");
            Permission("Ovrhl.Program1Stage.MakeStage2", "Расчет ДПКР (2 этап)");

            Namespace("Ovrhl.Program2Stage", "Долгосрочная программа (2 этап)");
            Permission("Ovrhl.Program2Stage.View", "Просмотр");
            Permission("Ovrhl.Program2Stage.MakeStage3", "Формирование ДПКР");

            Namespace("Ovrhl.LongProgram", "Долгосрочная программа (3 этап)");
            Permission("Ovrhl.LongProgram.View", "Просмотр");
            Permission("Ovrhl.LongProgram.Delete", "Удаление");
            Namespace("Ovrhl.LongProgram.PriorParams", "Очередность");
            Permission("Ovrhl.LongProgram.PriorParams.Create", "Добавление");
            Permission("Ovrhl.LongProgram.PriorParams.Edit", "Редактирование");
            Permission("Ovrhl.LongProgram.PriorParams.Delete", "Удаление");

            Namespace("Ovrhl.Program4Stage", "Результат корректировки");
            Permission("Ovrhl.Program4Stage.View", "Просмотр");

            Namespace("Ovrhl.PublicationProgs", "Опубликованные программы");
            Permission("Ovrhl.PublicationProgs.View", "Просмотр");

            Namespace("Ovrhl.LongTermProgram", "Долгосрочная программа");
            Permission("Ovrhl.LongTermProgram.View", "Просмотр");

            Namespace("Ovrhl.RealEstateTypeRate", "Тариф по типам домов");
            CRUDandViewPermissions("Ovrhl.RealEstateTypeRate");

            Namespace("Ovrhl.Subcidy", "Субсидирование");
            Permission("Ovrhl.Subcidy.View", "Просмотр");
            Permission("Ovrhl.Subcidy.Edit", "Редактирование");
            Permission("Ovrhl.Subcidy.FinanceNeedBefore", "Потребность до финансирования");
            Permission("Ovrhl.Subcidy.CalcValues", "Расчитать показатели");
            Permission("Ovrhl.Subcidy.CorrectDpkr", "Корректировка ДПКР");
            Permission("Ovrhl.Subcidy.CorrectResult", "Результат корректировки");

            Namespace("Ovrhl.PriorityParam", "Параметры очередности");
            CRUDandViewPermissions("Ovrhl.PriorityParam");

            Namespace("Ovrhl.RegOperator", "Региональные операторы");
            CRUDandViewPermissions("Ovrhl.RegOperator");
            Namespace("Ovrhl.RegOperator.Municipality", "Муниципальные образования");
            CRUDandViewPermissions("Ovrhl.RegOperator.Municipality");
            Namespace("Ovrhl.RegOperator.Accounts", "Счета");
            Permission("Ovrhl.RegOperator.Accounts.View", "Просмотр");

            Namespace("Ovrhl.RegOperator.CalcAccounts", "Расчетные счета");

            #region Банковская выписка
            Namespace<AccBankStatement>("Ovrhl.AccBankStatement", "Банковская выписка");
            CRUDandViewPermissions("Ovrhl.AccBankStatement");

            Namespace<AccBankStatement>("Ovrhl.AccBankStatement.Operation", "Операция");
            CRUDandViewPermissions("Ovrhl.AccBankStatement.Operation");

            #region Поля
            Namespace<AccBankStatement>("Ovrhl.AccBankStatement.Field", "Поля");
            Permission("Ovrhl.AccBankStatement.Field.Number_Edit", "Номер счета");
            Permission("Ovrhl.AccBankStatement.Field.DocumentDate_Edit", "Дата выписки");
            #endregion

            #endregion

            Namespace("Ovrhl.CreditOrg", "Кредитные организации");
            CRUDandViewPermissions("Ovrhl.CreditOrg");
            Namespace("Ovrhl.CreditOrg.Fields", "Поля");
            Permission("Ovrhl.CreditOrg.Fields.Name", "Наименование");
            Permission("Ovrhl.CreditOrg.Fields.Parent", "Головная организация");
            Permission("Ovrhl.CreditOrg.Fields.Inn", "ИНН");
            Permission("Ovrhl.CreditOrg.Fields.Kpp", "КПП");
            Permission("Ovrhl.CreditOrg.Fields.Bik", "БИК");
            Permission("Ovrhl.CreditOrg.Fields.Okpo", "ОКПО");
            Permission("Ovrhl.CreditOrg.Fields.CorrAccount", "Корреспондентский счет");
            Permission("Ovrhl.CreditOrg.Fields.Address", "Юридический адрес");
            Permission("Ovrhl.CreditOrg.Fields.MailingAddress", "Почтовый адрес");

            Namespace("Ovrhl.LongTermProgramObject", "Объекты долгосрочной программы капитального ремонта");
            Permission("Ovrhl.LongTermProgramObject.View", "Просмотр");

            Namespace("Ovrhl.LongTermProgramObject.PropertyOwnerProtocols", "Протоколы собственников помещений");
            CRUDandViewPermissions("Ovrhl.LongTermProgramObject.PropertyOwnerProtocols");
            Namespace("Ovrhl.LongTermProgramObject.PropertyOwnerDecision", "Решения собственников");
            CRUDandViewPermissions("Ovrhl.LongTermProgramObject.PropertyOwnerDecision");

            Namespace<SpecialAccountDecisionNotice>("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice", "Уведомления");
            Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Save", "Сохранить");
            Namespace("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field", "Поля");
            Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.NoticeNumber", "Дата уведомления");
            Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.File", "Документ уведомления");
            Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.RegDate", "Дата регистрации уведомления в ГЖИ");
            Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.GjiNumber", "Входящий номер в ГЖИ");
            Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.HasOriginal", "Оригинал уведомления поступил");
            Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.HasCopyCertificate", "Копия справки поступила");
            Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.HasCopyProtocol", "Копия протокола поступила");

            Namespace("Ovrhl.LongTermProgramObject.InfoCr", "Сведения о кап. ремонте");
            Permission("Ovrhl.LongTermProgramObject.InfoCr.LongTermProgram", "Долгосрочная программа кап. ремонта");
            Permission("Ovrhl.LongTermProgramObject.InfoCr.ExecutionLongTermProgram", "Выполнение кап. ремонта");

            Namespace("Ovrhl.LongTermProgramObject.Loan", "Учет займов");
            CRUDandViewPermissions("Ovrhl.LongTermProgramObject.Loan");

            #region Отчеты

            Permission("Ovrhl.ControlCertificationOfBuild", "Контроль паспортизации домов");
            Permission("Ovrhl.PublishedDpkr", "Отчет по опубликованию ДПКР");

            Permission("Ovrhl.PublishedDpkrExtended", "Отчет по ДПКР");
            Permission("Ovrhl.SpecialAccountDecisionReport", "Реестр специальных счетов");
            Permission("Ovrhl.FormFundNotSetMkdInfoReport", "Сведения о МКД, собственники помещений в которых не выбрали способ формирования фонда капитального ремонта");
            Permission("Ovrhl.CtrlCertOfBuildConsiderMissingCeo", "Контроль паспортизации домов с учетом отсутствующих элементов");
            Permission("Ovrhl.LongtermProgOverhaulInfo", "Информация о долгосрочной программе капитального ремонта");
            Permission("Ovrhl.TurnoverBalanceByMkd", "Оборотно-сальдовая ведомость по счетм МКД");

            Namespace("Reports.GkhOverhaul", "Модуль капитальный ремонт (ДПКР)");
            Permission("Reports.GkhOverhaul.CertificationControlValues", "Контроль паспортизации домов (значения)");
            Permission("Reports.GkhOverhaul.CertificationControlValuesWithQuality", "Контроль паспортизации домов (значения с качеством заполнения данных)");
            Permission("Reports.GkhOverhaul.PublishedDpkrByWorkReport", "(Долгосрочная программа) Отчет по опубликованию ДПКР (по видам работ)");
            Permission("Reports.GkhOverhaul.ProgramVersionReport", "(Долгосрочная программа) Версии ДПКР");
            Permission("Reports.GkhOverhaul.HousesExcessMargSumReport", "Дома с  превышением предельной стоимости работ, не включенные в ДПКР");
            Permission("Reports.GkhOverhaul.LongProgInfoByStructEl", "Расчет долгосрочной программы КР");
            Permission("Reports.GkhOverhaul.ReasonableRate", "Экономически обоснованный тариф взносов на КР");
            Permission("Reports.GkhOverhaul.NotIncludedWorksInProgram", "Работы, исключенные из Долгосрочной программы капитального ремонта");
            Permission("Reports.GkhOverhaul.PlanOwnerCollectionReport", "Планируемая собираемость по домам");
            Permission("Reports.GkhOverhaul.HouseInformationCeReport", "Информация по домам");
            Permission("Reports.GkhOverhaul.HouseInformationCeFiasReport", "Информация по домам (ФИАС)");
           

            #endregion

            #region Импорт

            Namespace("Import.NsoRealtyObjectImport", "Импорт жилых домов для Новосибирска");
            Permission("Import.NsoRealtyObjectImport.View", "Просмотр");
            
            Namespace("Import.DpkrLoad", "Импорт ДПКР");
            Permission("Import.DpkrLoad.View", "Просмотр");

            Namespace("Import.RoImportFromFundPart3", "Импорт жилых домов из фонда (часть 3)");
            Permission("Import.RoImportFromFundPart3.View", "Просмотр");

            Namespace("Import.RoImportFromFundPart5", "Импорт жилых домов из фонда (часть 5)");
            Permission("Import.RoImportFromFundPart5.View", "Просмотр");

            Namespace("Import.WorksImportByStructElements", "Импорт работ по конструктивным элементам");
            Permission("Import.WorksImportByStructElements.View", "Просмотр");

            #endregion

            Namespace("Ovrhl.ShortProgram", "Краткосрочная программа");
            Permission("Ovrhl.ShortProgram.View", "Просмотр");

            Namespace("Ovrhl.ShortProgramDeficit", "Дефицит по МО");
            Permission("Ovrhl.ShortProgramDeficit.View", "Просмотр");

            Namespace("Ovrhl.ProgramVersions", "Версии программы");
            Permission("Ovrhl.ProgramVersions.View", "Просмотр");
            Permission("Ovrhl.ProgramVersions.Copy", "Копирование");
            Namespace("Ovrhl.ProgramVersions.Actualize", "Актуализация ДПКР");

            Permission("Ovrhl.ProgramVersions.Actualize.View", "Доступность кнопки");

            Namespace("Ovrhl.ProgramVersions.Actualize.Actions", "Действия");

            Permission("Ovrhl.ProgramVersions.Actualize.Actions.ActualizeNewRecords", "Добавить новые записи");
            Permission("Ovrhl.ProgramVersions.Actualize.Actions.ActualizeSum", "Актуализировать стоимость");
            Permission("Ovrhl.ProgramVersions.Actualize.Actions.ActualizeYear", "Актуализировать год");
            Permission("Ovrhl.ProgramVersions.Actualize.Actions.ActualizeDel", "Удалить лишние записи");
            Permission("Ovrhl.ProgramVersions.Actualize.Actions.ActualizeOrder", "Рассчитать очередность");

            Namespace("Ovrhl.RealEstateType.Municipality", "Муниципальные образования");
            Permission("Ovrhl.RealEstateType.Municipality.View", "Просмотр");
            Permission("Ovrhl.RealEstateType.Municipality.Create", "Добавление");
            Permission("Ovrhl.RealEstateType.Municipality.Delete", "Удаление");

            Namespace("Ovrhl.LoadProgram", "Загрузка программы");
            Permission("Ovrhl.LoadProgram.View", "Просмотр");

            Namespace("Ovrhl.FundFormationContract", "Реестр договоров на формирование фонда капитального ремонта");
            CRUDandViewPermissions("Ovrhl.FundFormationContract");

            Namespace("Ovrhl.LoanRegister", "Реестр займов");
            Permission("Ovrhl.LoanRegister.View", "Просмотр");

            Namespace("Ovrhl.DecisionNoticeRegister", "Сводный реестр уведомлений о решениях общего собрания");
            Permission("Ovrhl.DecisionNoticeRegister.View", "Просмотр");

            Namespace("Ovrhl.SuspenseAccount", "Cчета невыявленных сумм");
            Permission("Ovrhl.SuspenseAccount.View", "Просмотр");

            Namespace("Administration.ExportTariff", "Экспорт тарифов");
            Permission("Administration.ExportTariff.View", "Просмотр раздела");

            Dictionaries();
        }

        private void Dictionaries()
        {
            Namespace("Ovrhl.Dictionaries.AccountOperation", "Операции по счету");
            CRUDandViewPermissions("Ovrhl.Dictionaries.AccountOperation");
        }
    }
}