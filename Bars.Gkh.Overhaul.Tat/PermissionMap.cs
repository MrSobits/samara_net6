namespace Bars.Gkh.Overhaul.Tat
{
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;

    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            #region Жилой дом

            Namespace("Gkh.RealityObject.Register.OwnerProtocol", "Протоколы и решения собственников");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.View", "Просмотр");

            Namespace("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerProtocols", "Протоколы");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerProtocols.Create", "Создание записей");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerProtocols.Edit", "Изменение записей");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerProtocols.Delete", "Удаление записей");

            Namespace("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision", "Решения протокола");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Create", "Создание записей");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Edit", "Изменение записей");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Delete", "Удаление записей");

            Namespace("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.FundFormType", "Способ формирования фонда");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.FundFormType.SpecAcc", "Спец. счет");

            const string decisionTypeNamespace = "Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.PropertyOwnerDecisionType";

            Namespace(decisionTypeNamespace, "Типы решения собственников помещений МКД");
            foreach (PropertyOwnerDecisionType propertyOwnerDecisionType in System.Enum.GetValues(typeof(PropertyOwnerDecisionType)))
            {
                Permission(decisionTypeNamespace + "." + propertyOwnerDecisionType.ToString(), propertyOwnerDecisionType.GetEnumMeta().Display);
            }

            Namespace<SpecialAccountDecisionNotice>("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice", "Уведомления");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Save", "Сохранить");
            Namespace("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field", "Поля");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.NoticeNumber", "Дата уведомления");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.File", "Документ уведомления");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.RegDate", "Дата регистрации уведомления в ГЖИ");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.GjiNumber", "Входящий номер в ГЖИ");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.HasOriginal", "Оригинал уведомления поступил");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.HasCopyCertificate", "Копия справки поступила");
            Permission("Gkh.RealityObject.Register.OwnerProtocol.PropertyOwnerDecision.Notice.Field.HasCopyProtocol", "Копия протокола поступила");

            #endregion Жилой дом

            Namespace("Administration.MassDeleteRoSe", "Массовое удаление КЭ");
            Permission("Administration.MassDeleteRoSe.View", "Просмотр");

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
            Permission("Ovrhl.LongProgram.Edit", "Редактирование");
            Namespace("Ovrhl.LongProgram.PriorParams", "Очередность");
            Permission("Ovrhl.LongProgram.PriorParams.Create", "Добавление");
            Permission("Ovrhl.LongProgram.PriorParams.Edit", "Редактирование");
            Permission("Ovrhl.LongProgram.PriorParams.Delete", "Удаление");
            Namespace("Ovrhl.LongProgram.ProgramVersion", "Версии программы");
            Permission("Ovrhl.LongProgram.ProgramVersion.Create", "Создание");

            Permission("Ovrhl.LongProgram.ProgramVersion.View", "Просмотр");
            Permission("Ovrhl.LongProgram.ProgramVersion.Delete", "Удаление");

            Namespace<ProgramVersion>("Ovrhl.ProgramVersion", "Версия программы");
            Permission("Ovrhl.ProgramVersion.Copy", "Копирование");
            Namespace("Ovrhl.ProgramVersion.ActualizeProgram", "Актуализация ДПКР");

            Permission("Ovrhl.ProgramVersion.ActualizeProgram.View", "Доступность кнопки");

            Namespace("Ovrhl.ProgramVersion.ActualizeProgram.Actions", "Действия");

            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeNewRecords", "Добавить новые записи");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeLiftNewRecords", "Добавить новые записи по ООИ \"Лифты\"");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeSum", "Актуализировать стоимость");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeYear", "Актуализировать год");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeDel", "Удалить лишние записи");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeGroup", "Сгруппировать ООИ");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeOrder", "Рассчитать очередность");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeFromShortCr", "Актуализировать из КПКР");

            Namespace<ProgramVersion>("Ovrhl.ProgramCorrection", "Результат корректировки");
            Permission("Ovrhl.ProgramCorrection.View", "Просмотр");
            Permission("Ovrhl.ProgramCorrection.CreateShortProgram", "Сформировать краткосрочную программу");
            Permission("Ovrhl.ProgramCorrection.ActualizeProgram", "Актуализировать Версию");
            Permission("Ovrhl.ProgramCorrection.PublishDpkr", "Версия для публикации");
            Permission("Ovrhl.ProgramCorrection.MassYearChange", "Массовое изменение года");

            Namespace("Ovrhl.PublicationProgs", "Опубликованные программы");
            Permission("Ovrhl.PublicationProgs.View", "Просмотр");

            Namespace("Ovrhl.LongTermProgram", "Долгосрочная программа");
            Permission("Ovrhl.LongTermProgram.View", "Просмотр");

            Namespace("Ovrhl.RealEstateTypeRate", "Тариф по типам домов");
            CRUDandViewPermissions("Ovrhl.RealEstateTypeRate");

            Namespace<ProgramVersion>("Ovrhl.Subcidy", "Субсидирование");
            Permission("Ovrhl.Subcidy.View", "Просмотр");
            Permission("Ovrhl.Subcidy.Edit", "Редактирование");
            Permission("Ovrhl.Subcidy.CalcValues", "Расчитать показатели");
            Permission("Ovrhl.Subcidy.CorrectDpkr", "Корректировка ДПКР");
            Namespace("Ovrhl.Subcidy.ColumnEditor", "Редактирование полей бюджетов и средств");
            Permission("Ovrhl.Subcidy.ColumnEditor.BudgetFcrEdit", "Редактирование полей \"Средства ГК ФСР ЖКХ\"");
            Permission("Ovrhl.Subcidy.ColumnEditor.BudgetRegionEdit", "Редактирование полей \"Региональный бюджет\"");
            Permission("Ovrhl.Subcidy.ColumnEditor.BudgetMunicipalityEdit", "Редактирование полей \"Бюджет муниципального образования\"");
            Permission("Ovrhl.Subcidy.ColumnEditor.OwnerSourcEdit", "Редактирование полей \"Средства собственников\"");

            Namespace("Ovrhl.DpkrParams", "Настройки программы капитального ремонта");
            Permission("Ovrhl.DpkrParams.View", "Просмотр");
            Permission("Ovrhl.DpkrParams.Edit", "Редактирование");

            Namespace("Ovrhl.PriorityParam", "Параметры очередности");
            CRUDandViewPermissions("Ovrhl.PriorityParam");

            Namespace("Ovrhl.RegOperator", "Региональные операторы");
            CRUDandViewPermissions("Ovrhl.RegOperator");
            Namespace("Ovrhl.RegOperator.Municipality", "Муниципальные образования");
            CRUDandViewPermissions("Ovrhl.RegOperator.Municipality");
            Namespace("Ovrhl.RegOperator.Accounts", "Счета");
            Permission("Ovrhl.RegOperator.Accounts.View", "Просмотр");

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

            #region Банковская выписка
            Namespace<AccBankStatement>("Ovrhl.AccBankStatement", "Банковская выписка");
            CRUDandViewPermissions("Ovrhl.AccBankStatement");

            Namespace("Ovrhl.AccBankStatement.Operation", "Операция");
            CRUDandViewPermissions("Ovrhl.AccBankStatement.Operation");

            #region Поля
            Namespace("Ovrhl.AccBankStatement.Field", "Поля");
            Permission("Ovrhl.AccBankStatement.Field.Number_Edit", "Номер счета");
            Permission("Ovrhl.AccBankStatement.Field.DocumentDate_Edit", "Дата выписки");
            #endregion

            #endregion

            Namespace("Ovrhl.LongTermProgramObject", "Объекты долгосрочной программы капитального ремонта");
            Permission("Ovrhl.LongTermProgramObject.View", "Просмотр");
            Permission("Ovrhl.LongTermProgramObject.Edit", "Изменение записи");
            Permission("Ovrhl.LongTermProgramObject.Delete", "Удаление записи");

            Namespace("Ovrhl.LongTermProgramObject.InfoCr", "Сведения о кап. ремонте");
            Permission("Ovrhl.LongTermProgramObject.InfoCr.LongTermProgram", "Долгосрочная программа кап. ремонта");
            Permission("Ovrhl.LongTermProgramObject.InfoCr.ExecutionLongTermProgram", "Выполнение кап. ремонта");

            Permission("Ovrhl.LongTermProgramObject.AccountLoan", "Учет займов");
            Permission("Ovrhl.LongTermProgramObject.RealAccount", "Реальный счет");

            #region Отчеты

            Permission("Ovrhl.ControlCertificationOfBuild", "Контроль паспортизации домов");
            Permission("Ovrhl.CtrlCertOfBuildConsiderMissingCeo", "Контроль паспортизации домов с учетом отсутствующих элементов");
            Permission("Ovrhl.ConsolidatedCertificationReport", "Сводный отчет по паспортизации домов");
            Permission("Ovrhl.PublishedDpkr", "Отчет по опубликованию ДПКР");

            Permission("Ovrhl.CountRoByMuInPeriod", "(Долгосрочная программа) Количество МКД по муниципальным образованиям за период");

            Permission("Ovrhl.LongProgramReport", "Региональная адресная программа кап.ремонта МКД");
            Permission("Ovrhl.FillingControlRepairReport", "Контроль заполнения показателей для определения  очередности проведения ремонта");
            Permission("Ovrhl.SpecialAccountDecisionReport", "Реестр специальных счетов");
            Permission("Ovrhl.FormFundNotSetMkdInfoReport", "Сведения о МКД, собственники помещений в которых не выбрали способ формирования фонда капитального ремонта");

            Namespace("Reports.GkhOverhaul", "Модуль капитальный ремонт (ДПКР)");
            Permission("Reports.GkhOverhaul.CertificationControlValues", "Контроль паспортизации домов (значения)");
            Permission("Reports.GkhOverhaul.LongProgramByTypeWork", "Долгосрочная  программа по видам работ");
            Permission("Reports.GkhOverhaul.ShortProgramByTypeWork", "Краткосрочная программа по видам работ");
            #endregion

            #region Импорт
            Namespace("Import.TatRealtyObjectImport", "Импорт жилых домов для Новосибирска");
            Permission("Import.TatRealtyObjectImport.View", "Просмотр");

            Namespace("Ovrhl.DpkrDocument.DpkrDocumentImport", "Импорт домов");
            Permission("Ovrhl.DpkrDocument.DpkrDocumentImport.View", "Просмотр");

            #endregion

            Namespace<ProgramVersion>("Ovrhl.ShortProgram", "Краткосрочная программа");
            Permission("Ovrhl.ShortProgram.View", "Просмотр");
            Permission("Ovrhl.ShortProgram.ActualizeVersion", "Актуализировать ДПКР");
            Permission("Ovrhl.ShortProgram.MassStateChange", "Массовая смена статусов");

            Namespace<ShortProgramRealityObject>("Ovrhl.ShortProgram.RealityObject", "Жилые дома краткосрочной программы");
            Permission("Ovrhl.ShortProgram.RealityObject.Create", "Добавление");
            Permission("Ovrhl.ShortProgram.RealityObject.Edit", "Редактирование");
            Permission("Ovrhl.ShortProgram.RealityObject.Delete", "Удаление");

            Namespace("Ovrhl.ShortProgram.RealityObject.Protocol", "Протоколы");
            Permission("Ovrhl.ShortProgram.RealityObject.Protocol.Create", "Добавление");
            Permission("Ovrhl.ShortProgram.RealityObject.Protocol.Edit", "Редактирование");
            Permission("Ovrhl.ShortProgram.RealityObject.Protocol.Delete", "Удаление");

            Namespace("Ovrhl.ShortProgram.RealityObject.DefectList", "Дефектные ведомости");
            Permission("Ovrhl.ShortProgram.RealityObject.DefectList.Create", "Добавление");
            Permission("Ovrhl.ShortProgram.RealityObject.DefectList.Edit", "Редактирование");
            Permission("Ovrhl.ShortProgram.RealityObject.DefectList.Delete", "Удаление");

            Namespace("Ovrhl.FundFormationContract", "Реестр договоров на формирование фонда капитального ремонта");
            CRUDandViewPermissions("Ovrhl.FundFormationContract");

            Permission("Ovrhl.DecisionNoticeRegister.View", "Просмотр");
            Dictionaries();

            Namespace("Ovrhl.DpkrDocument", "Документы ДПКР");
            CRUDandViewPermissions("Ovrhl.DpkrDocument");

            Namespace("Ovrhl.DpkrDocument.RealityObject", "Дома");
            Namespace("Ovrhl.DpkrDocument.RealityObject.Included", "Включенные дома");
            Permission("Ovrhl.DpkrDocument.RealityObject.Included.Create", "Добавление");
            Permission("Ovrhl.DpkrDocument.RealityObject.Included.Delete", "Удаление");

            Namespace("Ovrhl.DpkrDocument.RealityObject.Excluded", "Исключенные дома");
            Permission("Ovrhl.DpkrDocument.RealityObject.Excluded.Create", "Добавление");
            Permission("Ovrhl.DpkrDocument.RealityObject.Excluded.Delete", "Удаление");
        }

        private void Dictionaries()
        {
            Namespace("Ovrhl.Dictionaries.AccountOperation", "Операции по счету");
            CRUDandViewPermissions("Ovrhl.Dictionaries.AccountOperation");

            Namespace("Ovrhl.DecisionNoticeRegister", "Реестр уведомлений о способе формирования фонда КР");

            Namespace("Ovrhl.Dictionaries.PaymentSizeCr.Fields", "Поля");
            Permission("Ovrhl.Dictionaries.PaymentSizeCr.Fields.TypeIndicator_View", "Поле \"Показатель\" - просмотр");
            Permission("Ovrhl.Dictionaries.PaymentSizeCr.Fields.TypeIndicator_Edit", "Поле \"Показатель\" - редактирование");
            Permission("Ovrhl.Dictionaries.PaymentSizeCr.Fields.PaymentSize_View", "Поле \"Значение показателя\" - просмотр");
            Permission("Ovrhl.Dictionaries.PaymentSizeCr.Fields.PaymentSize_Edit", "Поле \"Значение показателя\" - редактирование");
            Permission("Ovrhl.Dictionaries.PaymentSizeCr.Fields.DateStartPeriod_View", "Поле \"Период действия с\" - просмотр");
            Permission("Ovrhl.Dictionaries.PaymentSizeCr.Fields.DateStartPeriod_Edit", "Поле \"Период действия с\" - редактирование");
            Permission("Ovrhl.Dictionaries.PaymentSizeCr.Fields.DateEndPeriod_View", "Поле \"Период действия по\" - просмотр");
            Permission("Ovrhl.Dictionaries.PaymentSizeCr.Fields.DateEndPeriod_Edit", "Поле \"Период действия по\" - редактирование");

            Namespace("Ovrhl.Dictionaries.PaymentSizeCr.Registry", "Реестры");

            Namespace("Ovrhl.Dictionaries.PaymentSizeCr.Registry.Municipality", "Муниципальные образования");
            CRUDandViewPermissions("Ovrhl.Dictionaries.PaymentSizeCr.Registry.Municipality");
        }
    }
}