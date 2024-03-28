namespace Bars.Gkh.Overhaul.Hmao
{
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Права доступа
    /// </summary>
    public class PermissionMap : B4.PermissionMap
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PermissionMap()
        {
            #region Жилой дом
            this.Namespace("Gkh.RealityObject.Register.OwnerProtocol", "Протоколы и решения собственников");
            this.CRUDandViewPermissions("Gkh.RealityObject.Register.OwnerProtocol");
            #endregion Жилой дом

            this.Namespace("Ovrhl", "Капитальный ремонт");
            
            #region вкладки и кнопки версии программы
            Namespace<ProgramVersion>("Ovrhl.ProgramVersion", "Версия программы");
            Permission("Ovrhl.ProgramVersion.Copy", "Копирование");
            Namespace("Ovrhl.ProgramVersion.ActualizeProgram", "Актуализация ДПКР");

            Permission("Ovrhl.ProgramVersion.ActualizeProgram.View", "Доступность кнопок");

            Namespace("Ovrhl.ProgramVersion.ActualizeProgram.Actions", "Действия");

            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeNewRecords", "Добавить новые записи");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.CalculateSequence", "Рассчитать очередность");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeSum", "Актуализировать стоимость");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeYear", "Актуализировать год");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeDel", "Удалить лишние записи");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeYearChange", "Актуализация изменения года");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeMainVersion", "Актуализировать основную версию");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeFromShortCr", "Актуализировать из КПКР");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeSubProgram", "Актуализировать из КПКР");

            Namespace("Ovrhl.ProgramVersion.ActualizeProgram.ActionColumns", "Действия по строке");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.ActionColumns.Delete", "Удалить");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.ActionColumns.Undo", "Вернуть");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.ActionColumns.ToSubProgram", "Перенести в подпрограмму");
            Permission("Ovrhl.ProgramVersion.ActualizeProgram.ActionColumns.UndoSubProgram", "Вернуть из подпрограммы");

            Namespace("Ovrhl.ProgramCorrection", "Результат корректировки");
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

            Namespace("Ovrhl.Subcidy", "Субсидирование");
            Permission("Ovrhl.Subcidy.View", "Просмотр");
            Permission("Ovrhl.Subcidy.Edit", "Редактирование");
            Permission("Ovrhl.Subcidy.CalcValues", "Расчитать показатели");
            Permission("Ovrhl.Subcidy.CorrectDpkr", "Корректировка ДПКР");
            Namespace("Ovrhl.Subcidy.ColumnEditor", "Редактирование полей бюджетов и средств");
            Permission("Ovrhl.Subcidy.ColumnEditor.BudgetFcrEdit", "Редактирование полей \"Средства ГК ФСР ЖКХ\"");
            Permission("Ovrhl.Subcidy.ColumnEditor.BudgetRegionEdit", "Редактирование полей \"Региональный бюджет\"");
            Permission("Ovrhl.Subcidy.ColumnEditor.BudgetMunicipalityEdit", "Редактирование полей \"Бюджет муниципального образования\"");
            Permission("Ovrhl.Subcidy.ColumnEditor.OwnerSourcEdit", "Редактирование полей \"Средства собственников\"");
            #endregion
            
            this.Namespace("Ovrhl.LongProgram", "Долгосрочная программа");
            this.Permission("Ovrhl.LongProgram.View", "Просмотр");
            this.Permission("Ovrhl.LongProgram.Delete", "Удаление");
            this.Permission("Ovrhl.LongProgram.MakeLongProgram", "Расчет ДПКР");
            this.Permission("Ovrhl.LongProgram.MakeLongProgram_View", "Расчет ДПКР - Просмотр");
            this.Permission("Ovrhl.LongProgram.Import_View", "Импорт - Просмотр");
            this.Permission("Ovrhl.LongProgram.SaveVersion", "Сохранение версии");
            this.Permission("Ovrhl.LongProgram.SaveVersion_View", "Сохранение версии - Просмотр");
            this.Permission("Ovrhl.LongProgram.WorkView", "Объект КР-Виды работ");

            this.Namespace("Ovrhl.LongProgram.PriorParams", "Очередность");
            this.Permission("Ovrhl.LongProgram.PriorParams.Create", "Добавление");
            this.Permission("Ovrhl.LongProgram.PriorParams.Edit", "Редактирование");
            this.Permission("Ovrhl.LongProgram.PriorParams.Delete", "Удаление");
            
            this.Namespace("Ovrhl.LongProgram.CalcFecsability", "Расчет Целесообразности");
            this.Permission("Ovrhl.LongProgram.CalcFecsability.Calc", "Расчет");

            this.Namespace("Ovrhl.MassCalcLongProgram", "Массовый расчет ДПКР");
            this.Permission("Ovrhl.MassCalcLongProgram.View", "Просмотр");

            this.Namespace("Ovrhl.DpkrDocument", "Документы ДПКР");
            this.CRUDandViewPermissions("Ovrhl.DpkrDocument");

            this.Namespace("Ovrhl.DpkrDocument.ProgramVersion", "Версии ДПКР");
            this.Permission("Ovrhl.DpkrDocument.ProgramVersion.Create", "Добавление записей");
            this.Permission("Ovrhl.DpkrDocument.ProgramVersion.Delete", "Удаление записей");
            this.Permission("Ovrhl.DpkrDocument.ProgramVersion.AddRealityObjects", "Формирование перечня домов");

            this.Namespace("Ovrhl.Program4Stage", "Результат корректировки");
            this.Permission("Ovrhl.Program4Stage.View", "Просмотр");

            this.Namespace("Ovrhl.PublicationProgs", "Опубликованные программы");
            this.Permission("Ovrhl.PublicationProgs.View", "Просмотр");

            this.Namespace<PublishedProgram>("Ovrhl.PublicationProgs.PublishDate", "Поле дата опубликования");
            this.Permission("Ovrhl.PublicationProgs.PublishDate.View", "Просмотр");

            this.Namespace<PublishedProgram>("Ovrhl.PublicationProgs.Summary", "Поле Количество домов в программе");
            this.Permission("Ovrhl.PublicationProgs.Summary.View", "Просмотр");

            this.Namespace<PublishedProgram>("Ovrhl.PublicationProgsDelete", "Удаление опубликованной программы");
            this.Permission("Ovrhl.PublicationProgsDelete.Delete", "Удаление");

            this.Namespace("Ovrhl.LongTermProgram", "Долгосрочная программа");
            this.Permission("Ovrhl.LongTermProgram.View", "Просмотр");

            this.Namespace("Ovrhl.LongTermSubProgram", "Долгосрочная подпрограмма");
            this.Permission("Ovrhl.LongTermSubProgram.View", "Просмотр");

            this.Namespace("Ovrhl.RealEstateTypeRate", "Тариф по типам домов");
            this.CRUDandViewPermissions("Ovrhl.RealEstateTypeRate");

            this.Namespace("Ovrhl.Subcidy", "Субсидирование");
            this.Permission("Ovrhl.Subcidy.View", "Просмотр");
            this.Permission("Ovrhl.Subcidy.Edit", "Редактирование");
            this.Permission("Ovrhl.Subcidy.CalcOwnerCollection", "Расчитать собираемость");
            this.Permission("Ovrhl.Subcidy.CalcValues", "Расчитать показатели");

            this.Namespace("Ovrhl.ActualiseDPKR", "Параметры актуализации ДПКР");
            this.CRUDandViewPermissions("Ovrhl.ActualiseDPKR");

            this.Namespace("Ovrhl.ActualiseSubProgram", "Критерии отбора в подпрограмму");
            this.CRUDandViewPermissions("Ovrhl.ActualiseSubProgram");

            this.Namespace("Ovrhl.EconFeasibilityCalc", "Рассчет средней стоимости");
            this.CRUDandViewPermissions("Ovrhl.EconFeasibilityCalc");

            this.Namespace("Ovrhl.MaxSumByYear", "Предельные стоимости в разрезе МО");
            this.CRUDandViewPermissions("Ovrhl.MaxSumByYear");

            this.Namespace("Ovrhl.PriorityParam", "Параметры очередности");
            this.CRUDandViewPermissions("Ovrhl.PriorityParam");

            this.Namespace("Ovrhl.CreditOrg", "Кредитные организации");
            this.CRUDandViewPermissions("Ovrhl.CreditOrg");
            this.Namespace("Ovrhl.CreditOrg.Fields", "Поля");
            this.Permission("Ovrhl.CreditOrg.Fields.Name", "Наименование");
            this.Permission("Ovrhl.CreditOrg.Fields.Parent", "Головная организация");
            this.Permission("Ovrhl.CreditOrg.Fields.Inn", "ИНН");
            this.Permission("Ovrhl.CreditOrg.Fields.Kpp", "КПП");
            this.Permission("Ovrhl.CreditOrg.Fields.Bik", "БИК");
            this.Permission("Ovrhl.CreditOrg.Fields.Okpo", "ОКПО");
            this.Permission("Ovrhl.CreditOrg.Fields.CorrAccount", "Корреспондентский счет");
            this.Permission("Ovrhl.CreditOrg.Fields.Address", "Юридический адрес");
            this.Permission("Ovrhl.CreditOrg.Fields.MailingAddress", "Почтовый адрес");

            this.Namespace("Ovrhl.LongTermProgramObject", "Объекты долгосрочной программы капитального ремонта");
            this.Permission("Ovrhl.LongTermProgramObject.View", "Просмотр");

            this.Namespace("Ovrhl.LongTermProgramObject.PropertyOwnerProtocols", "Протоколы собственников помещений");
            this.CRUDandViewPermissions("Ovrhl.LongTermProgramObject.PropertyOwnerProtocols");
            this.Namespace("Ovrhl.LongTermProgramObject.PropertyOwnerDecision", "Решения собственников");
            this.CRUDandViewPermissions("Ovrhl.LongTermProgramObject.PropertyOwnerDecision");

            this.Namespace<SpecialAccountDecisionNotice>("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice", "Уведомления");
            this.Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Save", "Сохранить");
            this.Namespace("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field", "Поля");
            this.Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.NoticeNumber", "Дата уведомления");
            this.Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.File", "Документ уведомления");
            this.Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.RegDate", "Дата регистрации уведомления в ГЖИ");
            this.Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.GjiNumber", "Входящий номер в ГЖИ");
            this.Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.HasOriginal", "Оригинал уведомления поступил");
            this.Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.HasCopyCertificate", "Копия справки поступила");
            this.Permission("Ovrhl.LongTermProgramObject.PropertyOwnerDecision.Notice.Field.HasCopyProtocol", "Копия протокола поступила");

            this.Namespace("Ovrhl.LongTermProgramObject.Loan", "Учет займов");
            this.CRUDandViewPermissions("Ovrhl.LongTermProgramObject.Loan");

            #region Отчеты
            this.Permission("Reports.GKH.SummaryCharacteristicsOfHouse", "Сводная характеристика домов на ДПКР");

            this.Permission("Ovrhl.ControlCertificationOfBuild", "Контроль паспортизации домов");
            this.Permission("Ovrhl.PublishedDpkr", "Отчет по опубликованию ДПКР");
            this.Permission("Ovrhl.CountCeoByMuInPeriod", "(Долгосрочная программа) Количество КЭ по муниципальным образованиям за период");
            this.Permission("Ovrhl.CountRoByMuInPeriod", "(Долгосрочная программа) Количество МКД по муниципальным образованиям за период");

            this.Permission("Ovrhl.PublishedDpkrExtended", "Отчет по ДПКР");
            this.Permission("Ovrhl.SpecialAccountDecisionReport", "Реестр специальных счетов");
            this.Permission(
                "Ovrhl.FormFundNotSetMkdInfoReport",
                "Сведения о МКД, собственники помещений в которых не выбрали способ формирования фонда капитального ремонта");
            this.Permission("Ovrhl.CtrlCertOfBuildConsiderMissingCeo", "Контроль паспортизации домов с учетом отсутствующих элементов");
            this.Permission("Ovrhl.LongtermProgOverhaulInfo", "Информация о долгосрочной программе капитального ремонта");
            this.Permission("Ovrhl.MarginalCostKr1LivingSpace", "Предельная стоимость проведения комплексного КР на 1 кв.м. жилой площади");

            this.Namespace("Reports.GkhOverhaul", "Модуль капитальный ремонт (ДПКР)");
            this.Permission("Reports.GkhOverhaul.CertificationControlValues", "Контроль паспортизации домов (значения)");
            this.Permission(
                "Reports.GkhOverhaul.CertificationControlValuesWithQuality",
                "Контроль паспортизации домов (значения с качеством заполнения данных)");
            this.Permission("Reports.GkhOverhaul.CountRoByCeoInPublProgram", "(Долгосрочная программа) Количество объектов в опубликованной программе");
            this.Permission("Reports.GkhOverhaul.PublishedDpkrByWorkReport", "(Долгосрочная программа) Отчет по опубликованию ДПКР (по видам работ)");
            this.Permission("Reports.GkhOverhaul.PublishedDpkrByWorkAndAddressReport", "(Долгосрочная программа) Отчет по опубликованию (в разрезе видов работ и уровней адреса)");
            this.Permission(
                "Reports.GkhOverhaul.PublishedDpkrPeriodByWorkReport",
                "(Долгосрочная программа) Отчет по опубликованию ДПКР (по видам работ с группировкой по годам)");
            this.Permission("Reports.GkhOverhaul.ProgramVersionReport", "(Долгосрочная программа) Версии ДПКР");
            this.Permission("Reports.GkhOverhaul.HousesExcessMargSumReport", "Дома с  превышением предельной стоимости работ, не включенные в ДПКР");
            this.Permission("Reports.GkhOverhaul.HousesHaveNotCollectSum", "Отчет о собираемости средств граждан на КР за период ДПКР");
            this.Permission("Reports.GkhOverhaul.LongProgInfoByStructEl", "Расчет долгосрочной программы КР");
            this.Permission("Reports.GkhOverhaul.CtrlFillDataForFormLongProg", "Контроль расчета долгосрочной программы");
            this.Permission("Reports.GkhOverhaul.FinanceModelDpkr", "Финансовая модель формирования региональной программы по МО");
            this.Permission("Reports.GkhOverhaul.ImportedDpkrReport", "Отчет по ДПКР для импорта");
            this.Permission("Reports.GkhOverhaul.LongProgramReport", "Региональная адресная программа кап.ремонта МКД");
            this.Permission("Reports.GkhOverhaul.SubsidyInfo", "Информация о субсидировании по МО");
            this.Permission("Reports.GkhOverhaul.ReasonableRate", "Экономически обоснованный тариф взносов на КР");
            this.Permission("Reports.GkhOverhaul.LongProgramByTypeWork", "Долгосрочная  программа по видам работ");
            this.Permission("Reports.GkhOverhaul.NotIncludedWorksInProgram", "Работы, исключенные из Долгосрочной программы капитального ремонта");
            this.Permission("Reports.GkhOverhaul.PlanOwnerCollectionReport", "Планируемая собираемость по домам");
            this.Permission("Reports.GkhOverhaul.HouseInformationCeReport", "Информация по домам");
            this.Permission("Reports.GkhOverhaul.CrShortTermPlanReport", "Краткосрочный план проведения капитальных ремонтов");
            this.Permission("Reports.GkhOverhaul.OverhaulMkdIndicators", "Форма 3. Планируемые показатели выполнения работ по КР МКД");
            this.Permission("Reports.GkhOverhaul.DpkrDataAnalysisReport", "Анализ данных по ДПКР (по основной и опубликованной версии программы)");
            this.Permission("Reports.GkhOverhaul.PublishProgramByStructEl", "Расчет опубликованной программы КР");
            this.Permission("Reports.GkhOverhaul.DpkrStructuralElements", "Опубликованная программа (по конструктивным элементам)");

            this.Permission("Ovrhl.DpkrGroupedByPeriod", "Региональная программа капитального ремонта с группировкой по периодам");

            this.Permission("Ovrhl.DpkrGroupedByPeriodPublish", "Региональная программа КР с группировкой по периодам (опубликованная программа)");

            this.Permission("Ovrhl.SubsidyBudget", "Итоговый бюджет на КР");
            this.Permission("Ovrhl.SubsidyBudgetSrcFinanc", "Итоговый бюджет на КР по резервам финансирвоания");
            #endregion

            #region Импорт
            this.Namespace("Import.HmaoRealtyObjectImport", "Импорт жилых домов для Новосибирска");
            this.Permission("Import.HmaoRealtyObjectImport.View", "Просмотр");

            this.Namespace("Import.DpkrLoad", "Импорт ДПКР");
            this.Permission("Import.DpkrLoad.View", "Просмотр");

            this.Namespace("Import.RoImportFromFundPart3", "Импорт жилых домов из фонда (часть 3)");
            this.Permission("Import.RoImportFromFundPart3.View", "Просмотр");

            this.Namespace("Import.RoImportFromFundPart5", "Импорт жилых домов из фонда (часть 5)");
            this.Permission("Import.RoImportFromFundPart5.View", "Просмотр");

            this.Namespace("Import.WorksImportByStructElements", "Импорт работ по конструктивным элементам");
            this.Permission("Import.WorksImportByStructElements.View", "Просмотр");

            this.Namespace("Import.Dpkr", "Импорт ДПКР");
            this.Permission("Import.Dpkr.View", "Просмотр");

            this.Namespace("Import.Dpkr1C", "Импорт ДПКР из 1С");
            this.Permission("Import.Dpkr1C.View", "Просмотр");
            #endregion

            this.Namespace("Ovrhl.ShortProgram", "Краткосрочная программа");
            this.Permission("Ovrhl.ShortProgram.View", "Просмотр");

            this.Namespace("Ovrhl.ShortProgramDeficit", "Дефицит по МО");
            this.Permission("Ovrhl.ShortProgramDeficit.View", "Просмотр");

            this.Namespace("Ovrhl.ProgramVersions", "Версии программы");
            this.Permission("Ovrhl.ProgramVersions.View", "Просмотр");
            this.Permission("Ovrhl.ProgramVersions.Actualize", "Актуализация ДПКР");
            this.Permission("Ovrhl.ProgramVersions.StavropolActualize", "Актуализация ДПКР СК");
            this.Permission("Ovrhl.ProgramVersions.ActualizeYearForStavropol", "Актуализация изменения года");
            this.Permission("Ovrhl.ProgramVersions.Copy", "Копирование");
            this.Permission("Ovrhl.ProgramVersions.MassYearChange", "Массовое изменение года");
            this.Permission("Ovrhl.ProgramVersions.RedirectToParent", "Переход");
            this.Permission("Ovrhl.ProgramVersions.SplitWork", "Разделить работу");

            this.Namespace("Ovrhl.ProgramVersions.Tab", "Вкладки");
            this.Permission("Ovrhl.ProgramVersions.Tab.Subsidy", "Субсидирование");
            this.Permission("Ovrhl.ProgramVersions.Tab.Correction", "Результат корректировки");
            this.Permission("Ovrhl.ProgramVersions.Tab.Publication", "Опубликованная программа");
            this.Permission("Ovrhl.ProgramVersions.Tab.ActualizationLog", "Логи актуализации");
            this.Permission("Ovrhl.ProgramVersions.Tab.ActualizationFileLogging", "Файловое логирование актуализации");

            this.Namespace("Ovrhl.ProgramVersions.OwnerDecision", "Решение собственников");
            this.Permission("Ovrhl.ProgramVersions.OwnerDecision.Create", "Создание");
            this.Permission("Ovrhl.ProgramVersions.OwnerDecision.View", "Просмотр");

            this.Namespace("Ovrhl.LoadProgram", "Загрузка программы");
            this.Permission("Ovrhl.LoadProgram.View", "Просмотр");

            this.Permission("Ovrhl.Billing", "Модуль начисления");

            this.Namespace("Ovrhl.LoanRegister", "Реестр займов");
            this.Permission("Ovrhl.LoanRegister.View", "Просмотр");

            this.Namespace("Ovrhl.DecisionNoticeRegister", "Реестр уведомлений о способе формирования фонда КР");
            this.Permission("Ovrhl.DecisionNoticeRegister.View", "Просмотр");
            
            this.Namespace("Widget.CrStatisticData", "Статистические данные КР");
            this.Permission("Widget.CrStatisticData.View", "Отображение виджета");
            
            this.Namespace("Widget.CrStatisticData.NotIncludedInCrHouses", "Дома, не попавшие в версии ДПКР");
            this.Permission("Widget.CrStatisticData.NotIncludedInCrHouses.View", "Отображение вкладки");

            this.Namespace("Widget.CrStatisticData.WorksNotIncludedPublishProgram", "Работы из основной версии ДПКР, не попавшие в опубликованную программу");
            this.Permission("Widget.CrStatisticData.WorksNotIncludedPublishProgram.View", "Отображение вкладки");

            this.Namespace("Widget.CrStatisticData.HousesWithNotFilledFias", "Дома, у которых в Реестре жилых домов не заполнен код ФИАС");
            this.Permission("Widget.CrStatisticData.HousesWithNotFilledFias.View", "Отображение вкладки");

            this.Namespace("Widget.CrStatisticData.IncludedInCrHousesByYears", "Дома, включенные в ДПКР в разрезе годов");
            this.Permission("Widget.CrStatisticData.IncludedInCrHousesByYears.View", "Отображение вкладки");
            
            this.Namespace("Widget.CrStatisticData.CrWorksInCeoContext", "Количество работ ДПКР в разрезе ООИ");
            this.Permission("Widget.CrStatisticData.CrWorksInCeoContext.View", "Отображение вкладки");

            this.Namespace("Widget.CrStatisticData.HousesWithMissingDpkrParameters", "Дома, с отсутсвующими параметрами для расчета ДПКР");
            this.Permission("Widget.CrStatisticData.HousesWithMissingDpkrParameters.View", "Отображение вкладки");
            
            this.Namespace("Widget.CrStatisticData.CrBudgeting", "Бюджетирование");
            this.Permission("Widget.CrStatisticData.CrBudgeting.View", "Отображение вкладки");

            this.Namespace("Widget.CrStatisticData.CostOfWorksInStructuralElementContext", "Стоимость работ в разрезе КЭ");
            this.Permission("Widget.CrStatisticData.CostOfWorksInStructuralElementContext.View", "Отображение вкладки");

            this.Dictionaries();
        }

        private void Dictionaries()
        {
            this.Namespace("Ovrhl.Dictionaries.AccountOperation", "Операции по счету");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.AccountOperation");

            this.Namespace("Ovrhl.Dictionaries.CrPeriod", "Периоды программ КР");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.CrPeriod");

            this.Namespace("Ovrhl.Dictionaries.ShareFinancingCeo", "Доли финансирвоания по работам");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.ShareFinancingCeo");

            this.Namespace("Ovrhl.Dictionaries.CostLimit", "Предельные стоимости услуг и работ");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.CostLimit");

            this.Namespace("Ovrhl.Dictionaries.CostLimitOOI", "Предельные стоимости работ и услуг в разрезе ООИ");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.CostLimitOOI");

            this.Namespace("Ovrhl.Dictionaries.OwnerProtocolType", "Виды протоколов собрания собственников");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.OwnerProtocolType");

            this.Namespace("Ovrhl.Dictionaries.CriteriaForActualizeVersion", "Критерии для актуализации регпрограммы");
            this.CRUDandViewPermissions("Ovrhl.Dictionaries.CriteriaForActualizeVersion");
        }
    }
}