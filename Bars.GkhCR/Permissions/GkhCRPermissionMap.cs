namespace Bars.GkhCr.Permissions
{
    using B4;

    using Bars.Gkh.Utils;

    using Entities;

    public class GkhCrPermissionMap : PermissionMap
    {
        public GkhCrPermissionMap()
        {
            #region Импорт
            this.Namespace("Import.Estimate", "Импорт смет");
            this.Permission("Import.Estimate.View", "Просмотр");

            this.Namespace("Import.EstimateARPS", "Импорт смет в формате АРПС 1.10");
            this.Permission("Import.EstimateARPS.View", "Просмотр");

            this.Namespace("Import.ContractsImport", "Импорт договоров и актов");
            this.Permission("Import.ContractsImport.View", "Просмотр");

            this.Namespace("Import.FinanceSource", "Импорт средств источников финансирования");
            this.Permission("Import.FinanceSource.View", "Просмотр");

            this.Namespace("Import.PersonalAccount", "Импорт лицевых счетов");
            this.Permission("Import.PersonalAccount.View", "Просмотр");

            this.Namespace("Import.PerformedWorkAct", "Импорт актов выполненных работ");
            this.Permission("Import.PerformedWorkAct.View", "Просмотр");

            this.Namespace("Import.ResourceStatement", "Импорт ведомостей ресурсов");
            this.Permission("Import.ResourceStatement.View", "Просмотр");

            this.Namespace("Import.PaymentOrder", "Импорт платежные поручения");
            this.Permission("Import.PaymentOrder.View", "Просмотр");

            this.Namespace("Import.ProgramCrImport", "Импорт программы кап.ремонта");
            this.Permission("Import.ProgramCrImport.View", "Просмотр");

            this.Namespace("Import.PerformedWork", "Импорт выполненных работ");
            this.Permission("Import.PerformedWork.View", "Просмотр");

            #endregion Импорт

            #region Модуль капитальный ремонт
            this.Namespace("GkhCr", "Модуль капитальный ремонт");

            this.Namespace("GkhCr.ProgramCr", "Реестр программ КР");
            this.CRUDandViewPermissions("GkhCr.ProgramCr");

            this.Namespace("GkhCr.ProgramCr.ChangeJournal", "Журнал изменений");
            this.Permission("GkhCr.ProgramCr.ChangeJournal.View", "Просмотр");

            this.Namespace("GkhCr.ProgramCr.AddWorkFromLongProgram", "Добавление видов работ из ДПКР");
            this.Permission("GkhCr.ProgramCr.AddWorkFromLongProgram.View", "Просмотр");

            this.Namespace("GkhCr.ProgramCr.Field", "Поля");
            this.Permission("GkhCr.ProgramCr.Field.NormativeDoc_View", "Постановление об утверждении КП - Просмотр");
            this.Permission("GkhCr.ProgramCr.Field.NormativeDoc_Edit", "Постановление об утверждении КП - Редактирование");
            this.Permission("GkhCr.ProgramCr.Field.File_View", "Файл - Просмотр");
            this.Permission("GkhCr.ProgramCr.Field.File_Edit", "Файл - Редактирование");
            this.Permission("GkhCr.ProgramCr.Field.UseForReformaAndGisGkhReports", "Использовать для отчетов Реформы и ГИС ЖКХ");

            #endregion Модуль капитальный ремонт

            #region Контрольный срок
            this.Namespace("GkhCr.ControlDate", "Контрольный срок");
            this.CRUDandViewPermissions("GkhCr.ControlDate");

            this.Namespace("GkhCr.ControlDate.Field", "Поля");
            this.Permission("GkhCr.ControlDate.Field.Date", "Контрольный срок");

            this.Namespace("GkhCr.ControlDate.StageWork", "Этапы работ");
            this.Permission("GkhCr.ControlDate.StageWork.Create", "Создание");
            this.Permission("GkhCr.ControlDate.StageWork.Delete", "Удаление");

            this.Namespace("GkhCr.ControlDate.MunicipalityLimitDate", "Сроки по муниципальному образованию");
            this.CRUDandViewPermissions("GkhCr.ControlDate.MunicipalityLimitDate");
            #endregion Контрольный срок

            #region Объекты КР
            this.Namespace("GkhCr.ObjectCrViewCreate", "Реестр объектов КР");
            this.Permission("GkhCr.ObjectCrViewCreate.View", "Просмотр");
            this.Permission("GkhCr.ObjectCrViewCreate.Create", "Создание записей");
            this.Namespace("GkhCr.ObjectCrViewCreate.Columns", "Столбцы");
            this.Permission("GkhCr.ObjectCrViewCreate.Columns.DateAcceptCrGji", "Принят ГЖИ");

            this.Namespace<ObjectCr>("GkhCr.ObjectCr", "Объекты КР: Просмотр, изменение");
            this.Permission("GkhCr.ObjectCr.Edit", "Изменение записей");
            this.Permission("GkhCr.ObjectCr.Delete", "Удаление записей");

            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.Inspector_View", "Инспектор - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.DateInGjiRegister_View", "Договор внесен в реестр ГЖИ - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.DateCancelReg_View", "Отклонено от регистрации - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.DateAcceptOnReg_View", "Принято на регистрацию - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.Description_View", "Описание - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.TabResultQual_View", "Вкладка:«Результат квалификационного отбора» - Просмотр");

            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.TabResultQual_Edit", "Вкладка:«Результат квалификационного отбора» - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.Description_Edit", "Описание - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.Inspector_Edit", "Инспектор - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.DateInGjiRegister_Edit", "Договор внесен в реестр ГЖИ - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.DateCancelReg_Edit", "Отклонено от регистрации - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.DateAcceptOnReg_Edit", "Принято на регистрацию - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.TerminationContractTab_View", "Вкладка:«Расторжение договора» - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.TerminationDate_View", "Дата расторжения - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.TerminationDate_Edit", "Дата расторжения - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.TerminationDocumentFile_View", "Документ-основание - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.TerminationDocumentFile_Edit", "Документ-основание - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.TerminationReason_View", "Основание расторжения - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.TerminationReason_Edit", "Основание расторжения - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.GuaranteePeriod_View", "Гарантийный срок (лет) - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.GuaranteePeriod_Edit", "Гарантийный срок (лет) - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.UrlResultTrading_View", "Ссылка на результаты проведения торгов - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.UrlResultTrading_Edit", "Ссылка на результаты проведения торгов - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.FinanceSource_View", "Разрез финансирования - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.Description_View", "Описание - Просмотр");

            this.Namespace("GkhCr.ObjectCr.CreateContract", "Сформировать договора подряда на основе договора из раздела \"Конкурсы\"");
            this.Permission("GkhCr.ObjectCr.CreateContract.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.CreateContract.Edit", "Изменение");

            this.Namespace<ObjectCr>("GkhCr.ObjectCr.Field", "Поля");
            this.Permission("GkhCr.ObjectCr.Field.RealityObject_Edit", "Жилой дом");
            this.Permission("GkhCr.ObjectCr.Field.ProgramCr_Edit", "Программа");

            this.Permission("GkhCr.ObjectCr.Field.ProgramNum_View", "Номер по программе - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.ProgramNum_Edit", "Номер по программе - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.GjiNum_View", "Номер ГЖИ - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.GjiNum_Edit", "Номер ГЖИ - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.FederalNumber_View", "Федеральный номер - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.FederalNumber_Edit", "Федеральный номер - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.Description_Edit", "Примечание");

            this.Permission("GkhCr.ObjectCr.Field.SumSMR_View", "Сумма на СМР - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.SumSMR_Edit", "Сумма на СМР - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.SumDevolopmentPSD_View", "Сумма на разработку и экспертизу ПСД - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.SumDevolopmentPSD_Edit", "Сумма на разработку и экспертизу ПСД - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.SumTehInspection_View", "Сумма на технадзор - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.SumTehInspection_Edit", "Сумма на технадзор - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.SumSMRApproved_View", "Утвержденная сумма СМР - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.SumSMRApproved_Edit", "Утвержденная сумма СМР - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.DateGjiReg_View", "Дата регистрации ГЖИ - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.DateGjiReg_Edit", "Дата регистрации ГЖИ - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.DateEndBuilder_View", "Дата завершения работ подрядчиком - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.DateEndBuilder_Edit", "Дата завершения работ подрядчиком - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.DateStopWorkGji_View", "Дата остановки работ ГЖИ - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.DateStopWorkGji_Edit", "Дата остановки работ ГЖИ - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.DateAcceptCrGji_View", "Дата принятия капитального ремонта ГЖИ - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.DateAcceptCrGji_Edit", "Дата принятия капитального ремонта ГЖИ - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.DateStartWork_View", "Дата начала работ - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.DateStartWork_Edit", "Дата начала работ - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.DateEndWork_View", "Дата окончания работ - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.DateEndWork_Edit", "Дата окончания работ - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.DateAcceptReg_View", "Дата принятия на регистрацию - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.DateAcceptReg_Edit", "Дата принятия на регистрацию - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.DateCancelReg_View", "Дата отклонения от регистрации - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.DateCancelReg_Edit", "Дата отклонения от регистрации - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.MaxKpkrAmount_View", "Предельная сумма из КПКР - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.MaxKpkrAmount_Edit", "Предельная сумма из КПКР - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.FactAmountSpent_View", "Фактически освоенная сумма - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.FactAmountSpent_Edit", "Фактически освоенная сумма - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.FactStartDate_View", "Фактическая дата начала работ - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.FactStartDate_Edit", "Фактическая дата начала работ - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.FactEndDate_View", "Фактическая дата окончания работ - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.FactEndDate_Edit", "Фактическая дата окончания работ - Изменение");

            this.Permission("GkhCr.ObjectCr.Field.WarrantyEndDate_View", "Дата окончания гарантийных обязательств - Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.WarrantyEndDate_Edit", "Дата окончания гарантийных обязательств - Изменение");

            this.Namespace("GkhCr.ObjectCr.Field.AllowReneg", "Повторное согласование");
            this.Permission("GkhCr.ObjectCr.Field.AllowReneg.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.Field.AllowReneg.Edit", "Изменение");

            #region Реестры
            this.Namespace<ObjectCr>("GkhCr.ObjectCr.Register", "Реестры");

            this.Namespace("GkhCr.ObjectCr.Register.ContractCrViewCreate", "Договоры на услуги - Просмотр, Создание");
            this.Permission("GkhCr.ObjectCr.Register.ContractCrViewCreate.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.ContractCrViewCreate.Create", "Создание записей");
            this.Namespace<ContractCr>("GkhCr.ObjectCr.Register.ContractCr", "Договоры на услуги - Изменение, Удаление");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Edit", "Изменение записей");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Delete", "Удаление записей");
            this.Namespace("GkhCr.ObjectCr.Register.HousekeeperReport", "Отчет старшего по дому");
            this.Permission("GkhCr.ObjectCr.Register.HousekeeperReport.View", "Просмотр");

            #region Договоры
            #region Поля
            this.Namespace<ContractCr>("GkhCr.ObjectCr.Register.ContractCr.Field", "Поля");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.DocumentName_Edit", "Документ - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.DocumentNum_Edit", "Номер документа - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.DateFrom_Edit", "Дата от - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.TypeContractObject_Edit", "Тип договора объекта КР - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.SumContract_Edit", "Сумма договора - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.BudgetMo_Edit", "Бюджет МО - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.BudgetSubject_Edit", "Бюджет субъекта - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.OwnerMeans_Edit", "Средства собственников - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.FundMeans_Edit", "Средства фонда - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.File_Edit", "Файл - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.Contragent_Edit", "Подрядная организация - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.FinanceSource_Edit", "Разрез финансирования - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.Description_Edit", "Описание - Изменение - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.DateStartWork_Edit", "Дата начала работ - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.DateEndWork_Edit", "Дата окончания работ - Изменение");

            this.Namespace("GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType", "Типы договоров");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType.Expertise", "Экспертиза");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType.RoMoAggreement", "Договор о функции заказчика между РО и МО");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType.BuildingControl", "Строительный контроль");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType.TechSepervision", "Технический надзор");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType.Psd", "ПСД");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.ContractCrType.Insurance", "Страхование");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.Customer_View", "Заказчик - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.Field.Customer_Edit", "Заказчик - Изменение");
            this.Namespace("GkhCr.ObjectCr.Register.ContractCr.TypeWork", "Виды работ");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.TypeWork.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.TypeWork.Create", "Добавление записей");
            this.Permission("GkhCr.ObjectCr.Register.ContractCr.TypeWork.Delete", "Удаление записей");

            #endregion Поля
            #endregion Договоры

            #region Протоколы
            this.Namespace("GkhCr.ObjectCr.Register.Protocol", "Протоколы");
            this.CRUDandViewPermissions("GkhCr.ObjectCr.Register.Protocol");
            this.Permission("GkhCr.ObjectCr.Register.Protocol.SumActVerificationOfCosts", "Сумма Акта сверки данных о расходах");
            this.Permission("GkhCr.ObjectCr.Register.Protocol.TypeWork.View", "Виды работ - Просмотр");

            this.Namespace("GkhCr.ObjectCr.Register.Protocol.Field", "Поля");

            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.SumActVerificationOfCosts_View", "Сумма Акта сверки данных о расходах - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.SumActVerificationOfCosts_Edit", "Сумма Акта сверки данных о расходах - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.CountVote_View", "Количество голосов (кв.м) - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.CountVote_Edit", "Количество голосов (кв.м) - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.CountVoteGeneral_View", "Общее количество голосов (кв.м) - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.CountVoteGeneral_Edit", "Общее количество голосов (кв.м) - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.CountAccept_View", "Доля принявших участие (%) - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.CountAccept_Edit", "Доля принявших участие (%) - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.GradeClient_View", "Оценка заказчика - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.GradeClient_Edit", "Оценка заказчика - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.GradeOccupant_View", "Оценка жителей - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.GradeOccupant_Edit", "Оценка жителей - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.Protocol.Field.DecisionOms_View", "Решение ОМС - Просмотр");


            #endregion Список протоколов

            #region Дефектные ведомости
            this.Namespace("GkhCr.ObjectCr.Register.DefectListViewCreate", "Дефектные ведомости - Просмотр, Создание");
            this.Permission("GkhCr.ObjectCr.Register.DefectListViewCreate.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.DefectListViewCreate.Create", "Создание записей");
            this.Namespace<DefectList>("GkhCr.ObjectCr.Register.DefectList", "Дефектные ведомости - Изменение, Удаление");
            this.Permission("GkhCr.ObjectCr.Register.DefectList.Edit", "Изменение записей");
            this.Permission("GkhCr.ObjectCr.Register.DefectList.Delete", "Удаление записей");

            this.Namespace<DefectList>("GkhCr.ObjectCr.Register.DefectList.Field", "Поля");
            this.Permission("GkhCr.ObjectCr.Register.DefectList.Field.DocumentName_Edit", "Наименование");
            this.Permission("GkhCr.ObjectCr.Register.DefectList.Field.DocumentDate_Edit", "Дата");
            this.Permission("GkhCr.ObjectCr.Register.DefectList.Field.Work_Edit", "Вид работы");
            this.Permission("GkhCr.ObjectCr.Register.DefectList.Field.File_Edit", "Файл");
            this.Permission("GkhCr.ObjectCr.Register.DefectList.Field.Sum_Edit", "Сумма  по ведомости, руб");
            #endregion Дефектные ведомости

            #region Виды работ
            this.Namespace("GkhCr.ObjectCr.Register.TypeWork", "Виды работ");
            this.CRUDandViewPermissions("GkhCr.ObjectCr.Register.TypeWork");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.DeleteStruclEl", "Удаление отдельных КЭ");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.MoveToAnotherPeriod", "Перенос работ в другой период");
            #region Поля
            this.Namespace("GkhCr.ObjectCr.Register.TypeWork.Field", "Поля");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.FinanceSource_Edit", "Разрез финансирования");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.TypeWork_Edit", "Вид работы");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.SumMaterialsRequirement_Edit", "Потребность материалов");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.HasPsd_Edit", "Наличие ПСД");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.Volume_Edit", "Объем");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.Sum_Edit", "Сумма");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.Description_Edit", "Примечание");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.DateStartWork_Edit", "Начало выполнения работ - Редактирование");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.DateStartWork_View", "Начало выполнения работ - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.DateEndWork_Edit", "Окончание выполнения работ - Редактирование");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.DateEndWork_View", "Окончание выполнения работ - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.GroupDpkr_View", "ДПКР - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.TypeWork.Field.GroupCr_View", "КПКР - Просмотр");
            #endregion Поля
            #endregion Список видов работ

            #region Удаленные объекты КР
            this.Namespace("GkhCr.ObjectCr.Register.DeletedObject", "Удаленные объекты КР");
            this.Permission("GkhCr.ObjectCr.Register.DeletedObject.View", "Просмотр");
            #region Поля
            this.Namespace("GkhCr.ObjectCr.Register.DeletedObject.Field", "Поля");
            this.Permission("GkhCr.ObjectCr.Register.DeletedObject.Field.Recover", "Восстановление");
            #endregion Поля
            #endregion

            #region Журнал изменений
            this.Namespace("GkhCr.ObjectCr.Register.TypeWorkCrHistory", "Журнал изменений");
            this.Permission("GkhCr.ObjectCr.Register.TypeWorkCrHistory.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.TypeWorkCrHistory.Restore", "Восстановление");
            #endregion Журнал изменений

            #region Cредства источников финансирования
            this.Namespace("GkhCr.ObjectCr.Register.FinanceSourceRes", "Разрезы финансирования");
            this.CRUDandViewPermissions("GkhCr.ObjectCr.Register.FinanceSourceRes");

            this.Namespace("GkhCr.ObjectCr.Register.FinanceSourceRes.Column", "Столбцы");
            this.Permission("GkhCr.ObjectCr.Register.FinanceSourceRes.Column.BudgetMuIncome", "Поступило по Бюджету МО");
            this.Permission("GkhCr.ObjectCr.Register.FinanceSourceRes.Column.BudgetMuPercent", "Процент МО");
            this.Permission("GkhCr.ObjectCr.Register.FinanceSourceRes.Column.BudgetSubjectIncome", "Поступило по Бюджету субъекта");
            this.Permission("GkhCr.ObjectCr.Register.FinanceSourceRes.Column.BudgetSubjectPercent", "Процент БС");
            this.Permission("GkhCr.ObjectCr.Register.FinanceSourceRes.Column.FundResourceIncome", "Поступило по Средствам фонда");
            this.Permission("GkhCr.ObjectCr.Register.FinanceSourceRes.Column.FundResourcePercent", "Процент СФ");

            #endregion Средства источников финансирования

            #region Лицевые счета
            this.Namespace("GkhCr.ObjectCr.Register.PersonalAccount", "Лицевые счета");
            this.CRUDandViewPermissions("GkhCr.ObjectCr.Register.PersonalAccount");
            #endregion Список лицевых счетов

            #region Сметы
            this.Namespace<ObjectCr>("GkhCr.ObjectCr.Register.EstimateCalculationViewCreate", "Сметы: Просмотр, Создание");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculationViewCreate.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculationViewCreate.Create", "Создание записей");
            this.Namespace<EstimateCalculation>("GkhCr.ObjectCr.Register.EstimateCalculation", "Сметы: Изменение, Удаление");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Edit", "Изменение записей");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Delete", "Удаление записей");

            this.Namespace("GkhCr.ObjectCr.Register.EstimateCalculation.Field", "Поля");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Field.TypeWorkCr", "Вид работ");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Field.EstimateDocument", "Документ сметы");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Field.ResourceStatmentDocument", "Документ ведомости ресурсов");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Field.FileEstimateDocument", "Файл сметы");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Field.TotalDirectCost", "Прямые затраты");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Field.OverheadSum", "Накладные расходы");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Field.EstimateProfit", "Сметная прибыль");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Field.Nds", "НДС");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Field.OtherCost", "Другие затраты");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Field.TotalEstimate", "Итого по смете");

            this.Namespace("GkhCr.ObjectCr.Register.EstimateCalculation.Estimate", "Записи сметы");

            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Estimate.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Estimate.Edit", "Редактирование записей");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.Estimate.Delete", "Удаление записей");

            this.Namespace("GkhCr.ObjectCr.Register.EstimateCalculation.ResStat", "Записи ведомости ресурсов");

            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.ResStat.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.ResStat.Edit", "Редактирование записей");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.ResStat.Delete", "Удаление записей");
            this.Permission("GkhCr.ObjectCr.Register.EstimateCalculation.ResStat.IsSumWithoutNds", "Сумма по ресурсам/материалам указана без НДС");
            #endregion Сметы

            #region Квалификационный отбор
            this.Namespace("GkhCr.ObjectCr.Register.Qualification", "Квалификационный отбор");
            this.CRUDandViewPermissions("GkhCr.ObjectCr.Register.Qualification");
            #endregion Квалификационный отбор

            #region Квалификационный отбор
            this.Namespace("GkhCr.ObjectCr.Register.Competition", "Конкурсы");
            this.Permission("GkhCr.ObjectCr.Register.Competition.View", "Просмотр");
            #endregion Квалификационный отбор

            #region Задание на проектирование
            this.Namespace("GkhCr.ObjectCr.Register.DesignAssignment", "Задание на проектирование");
            this.Permission("GkhCr.ObjectCr.Register.DesignAssignment.View", "Просмотр");
            #endregion Задание на проектирование

            #region Договоры подряда
            this.Namespace("GkhCr.ObjectCr.Register.BuildContractViewCreate", "Договоры подряда - Просмотр, Создание");
            this.Permission("GkhCr.ObjectCr.Register.BuildContractViewCreate.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContractViewCreate.Create", "Создание записей");
            this.Namespace("GkhCr.ObjectCr.Register.BuildContractViewCreate.Column", "Колонки");
            this.Permission("GkhCr.ObjectCr.Register.BuildContractViewCreate.Column.Sum", "Сумма");

            this.Namespace<BuildContract>("GkhCr.ObjectCr.Register.BuildContract", "Договоры подряда - Изменение, Удаление");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Edit", "Изменение записей");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Delete", "Удаление записей");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Print", "Печать");

            this.Namespace("GkhCr.ObjectCr.Register.BuildContract.TypeWork", "Виды работ");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.TypeWork.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.TypeWork.Create", "Добавление записей");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.TypeWork.Delete", "Удаление записей");

            #region Поля
            this.Namespace<BuildContract>("GkhCr.ObjectCr.Register.BuildContract.Field", "Поля");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.DocumentName_Edit", "Название документа - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.DocumentNum_Edit", "Номер документа - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.DocumentDateFrom_Edit", "Дата документа - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.DocumentFile_Edit", "Файл документа - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.Sum_Edit", "Сумма договора подряда - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.DateStartWork_Edit", "Дата начала работ - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.DateEndWork_Edit", "Дата окончания работ - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.TypeContractBuild_Edit", "Тип договора - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.Builder_Edit", "Подрядчик - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.BudgetMo_Edit", "Бюджет МО - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.BudgetSubject_Edit", "Бюджет субъекта - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.OwnerMeans_Edit", "Средства собственников - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.FundMeans_Edit", "Средства фонда - Изменение");

            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.ProtocolName_Edit", "Название документа квалификационного отбора - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.ProtocolNum_Edit", "Номер документа квалификационного отбора - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.ProtocolDateFrom_Edit", "Дата документа квалификационного отбора - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.ProtocolFile_Edit", "Файл документа квалификационного отбора - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.IsLawProvided_Edit", "Проведение отбора предусмотрено законодательством - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.WebSite_Edit", "Адрес сайта с информацией об отборе - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractState_Edit", "Состояние договора - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.IsLawProvided_View", "Проведение отбора предусмотрено законодательством - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.WebSite_View", "Адрес сайта с информацией об отборе - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractState_View", "Состояние договора - Просмотр");

            this.Namespace("GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractType", "Типы договоров");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractType.Smr", "На СМР");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractType.Device", "На приборы");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractType.Lift", "На лифты");
            this.Permission("GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractType.EnergySurvey", "На энергообследование");
            #endregion Поля
            #endregion Договоры подряда

            #region Мониторинг СМР
            this.Namespace<ObjectCr>("GkhCr.ObjectCr.Register.MonitoringSmr", "Мониторинг СМР");

            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.View", "Просмотр");
            #region График выполнения работ
            this.Namespace<MonitoringSmr>("GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork", "График выполнения работ");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.View", "Чтение");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.Edit", "Изменение");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.AddDate", "Дополнительный срок");

            this.Namespace("GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.Column", "Колонки");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.Column.FinanceSource", "Разрез финансирования");
            #endregion График выполнения работ

            #region Ход выполнения работ
            this.Namespace<MonitoringSmr>("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork", "Ход выполнения работ");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.CalcPercentOfCompletion", "Расчет процента выполнения");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.View", "Чтение");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Edit", "Изменение");

            this.Namespace("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field", "Поля");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_View", "Этап работы - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_Edit", "Этап работы - Изменение");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_View", "Производитель - Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_Edit", "Производитель - Изменение");
            this.Namespace("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Column", "Колонки");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Column.StageWorkCr", "Этап работы");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Column.Manufacturer", "Производитель");
            #endregion Ход выполнения работ

            #region Численность рабочих
            this.Namespace<MonitoringSmr>("GkhCr.ObjectCr.Register.MonitoringSmr.WorkersCount", "Численность рабочих");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.WorkersCount.View", "Чтение");
            this.Permission("GkhCr.ObjectCr.Register.MonitoringSmr.WorkersCount.Edit", "Изменение");
            #endregion Численность рабочих

            #region Документы
            this.Namespace<MonitoringSmr>("GkhCr.ObjectCr.Register.MonitoringSmr.Document", "Документы");
            this.CRUDandViewPermissions("GkhCr.ObjectCr.Register.MonitoringSmr.Document");
            #endregion Документы

            #endregion Мониторинг СМР

            #region Акт выполненных работ
            this.Namespace<ObjectCr>("GkhCr.ObjectCr.Register.PerformedWorkActViewCreate", "Акт выполненных работ: Просмотр, создание");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkActViewCreate.View", "Просмотр");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkActViewCreate.Create", "Создание записей");
            this.Namespace<PerformedWorkAct>("GkhCr.ObjectCr.Register.PerformedWorkAct", "Акт выполненных работ: Изменение, удаление");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkAct.Edit", "Изменение записей");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkAct.Delete", "Удаление записей");

            this.Namespace<PerformedWorkAct>("GkhCr.ObjectCr.Register.PerformedWorkAct.Field", "Поля");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Work", "Вид работ");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.DocumentNum", "Номер");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Sum", "Сумма");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.DateFrom", "Дата");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Description", "Описание");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Volume", "Объем");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.CostFile", "Справка о стоимости выполненных работ и затрат");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.DocumentFile", "Документ акта");
            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.AdditionFile", "Приложение к акту");
            this.FieldPermission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.RepresentativeSigned", "Акт подписан представителем собственников");
            this.FieldPermission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.RepresentativeName", "ФИО представителя");
            this.FieldPermission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.ExploitationAccepted", "Принято в эксплуатацию");
            this.FieldPermission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.WarrantyStartDate", "Дата начала гарантийного срока");
            this.FieldPermission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.WarrantyEndDate", "Дата окончания гарантийного срока");
            this.Namespace("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Rec", "Записи акта");
            this.CRUDandViewPermissions("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Rec");
            this.Namespace("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Payment", "Оплата акта выполненных работ");
            this.CRUDandViewPermissions("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Payment");

            #endregion Акт выполненных работ

            #endregion Реестры

            #endregion Объекты КР

            #region Предложения КР
            this.Namespace<OverhaulProposal>("GkhCr.OverhaulProposal", "Предложения по капремонту");
            this.Permission("GkhCr.OverhaulProposal.View", "Просмотр");
            this.Permission("GkhCr.OverhaulProposal.Create", "Создание");
            this.Permission("GkhCr.OverhaulProposal.Edit", "Изменение записей");
            this.Permission("GkhCr.OverhaulProposal.Delete", "Удаление записей");

            this.Namespace<OverhaulProposal>("GkhCr.OverhaulProposal.Register", "Реестры");
            this.Permission("GkhCr.OverhaulProposal.Register.Works", "Работы КПКР");
            this.Permission("GkhCr.OverhaulProposal.Register.Works.Create", "Создание работ");
            this.Permission("GkhCr.OverhaulProposal.Register.Works.Delete", "Удаление работ");


            #endregion 

            #region Объекты КР (работы)
            this.Namespace<TypeWorkCr>("GkhCr.TypeWorkCr", "Объекты КР (работы)");

            this.CRUDandViewPermissions("GkhCr.TypeWorkCr");
            this.Permission("GkhCr.TypeWorkCr.Import", "Импорты");

            this.CRUDandViewPermissions("GkhCr.CrFileRegister");
            this.Permission("GkhCr.CrFileRegister", "Архив файлов");

            this.Namespace<TypeWorkCr>("GkhCr.TypeWorkCr.Field", "Поля");
            this.Permission("GkhCr.TypeWorkCr.Field.FinanceSource_Edit", "Разрез финансирования");
            this.Permission("GkhCr.TypeWorkCr.Field.Year_Edit", "Год ремонта");
            this.Permission("GkhCr.TypeWorkCr.Field.Volume_Edit", "Объем");
            this.Permission("GkhCr.TypeWorkCr.Field.Sum_Edit", "Сумма");

            #region Реестры
            this.Namespace<TypeWorkCr>("GkhCr.TypeWorkCr.Register", "Реестры");

            #region Обследование объекта
            this.Namespace<TypeWorkCr>("GkhCr.TypeWorkCr.Register.Inspection", "Обследование объекта");
            this.CRUDandViewPermissions("GkhCr.TypeWorkCr.Register.Inspection");

            #endregion Обследование объекта

            #region Договоры
            this.Namespace("GkhCr.TypeWorkCr.Register.ContractCrViewCreate", "Договоры - Просмотр, Создание");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCrViewCreate.View", "Просмотр");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCrViewCreate.Create", "Создание записей");
            this.Namespace<ContractCr>("GkhCr.TypeWorkCr.Register.ContractCr", "Договоры на услуги - Изменение, Удаление");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Edit", "Изменение записей");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Delete", "Удаление записей");

            #region Поля
            this.Namespace<ContractCr>("GkhCr.TypeWorkCr.Register.ContractCr.Field", "Поля");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.DocumentName_Edit", "Документ");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.DocumentNum_Edit", "Номер документа");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.DateFrom_Edit", "Дата от");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.TypeContractObject_Edit", "Тип договора объекта КР");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.SumContract_Edit", "Сумма договора");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.BudgetMo_Edit", "Бюджет МО");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.BudgetSubject_Edit", "Бюджет субъекта");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.OwnerMeans_Edit", "Средства собственников");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.FundMeans_Edit", "Средства фонда");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.File_Edit", "Файл");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.Contragent_Edit", "Участник");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.FinanceSource_Edit", "Разрез финансирования");
            this.Permission("GkhCr.TypeWorkCr.Register.ContractCr.Field.Description_Edit", "Описание");
            #endregion Поля

            #endregion Договоры

            #region Протоколы
            this.Namespace("GkhCr.TypeWorkCr.Register.Protocol", "Протоколы");
            this.CRUDandViewPermissions("GkhCr.TypeWorkCr.Register.Protocol");
            this.Permission("GkhCr.TypeWorkCr.Register.Protocol.SumActVerificationOfCosts", "Сумма Акта сверки данных о расходах");
            this.Permission("GkhCr.TypeWorkCr.Register.Protocol.TypeWork.View", "Виды работ - Просмотр");

            #endregion Список протоколов

            #region Дефектные ведомости
            this.Namespace("GkhCr.TypeWorkCr.Register.DefectListViewCreate", "Дефектные ведомости - Просмотр, Создание");
            this.Permission("GkhCr.TypeWorkCr.Register.DefectListViewCreate.View", "Просмотр");
            this.Permission("GkhCr.TypeWorkCr.Register.DefectListViewCreate.Create", "Создание записей");
            this.Namespace<DefectList>("GkhCr.TypeWorkCr.Register.DefectList", "Дефектные ведомости - Изменение, Удаление");
            this.Permission("GkhCr.TypeWorkCr.Register.DefectList.Edit", "Изменение записей");
            this.Permission("GkhCr.TypeWorkCr.Register.DefectList.Delete", "Удаление записей");

            this.Namespace<DefectList>("GkhCr.TypeWorkCr.Register.DefectList.Field", "Поля");
            this.Permission("GkhCr.TypeWorkCr.Register.DefectList.Field.DocumentName_Edit", "Наименование");
            this.Permission("GkhCr.TypeWorkCr.Register.DefectList.Field.DocumentDate_Edit", "Дата");
            this.Permission("GkhCr.TypeWorkCr.Register.DefectList.Field.Work_Edit", "Вид работы");
            this.Permission("GkhCr.TypeWorkCr.Register.DefectList.Field.File_Edit", "Файл");
            this.Permission("GkhCr.TypeWorkCr.Register.DefectList.Field.Sum_Edit", "Сумма  по ведомости, руб");
            #endregion Дефектные ведомости

            #region Cредства источников финансирования
            this.Namespace("GkhCr.TypeWorkCr.Register.FinanceSourceRes", "Разрезы финансирования");
            this.CRUDandViewPermissions("GkhCr.TypeWorkCr.Register.FinanceSourceRes");

            this.Namespace("GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column", "Столбцы");
            this.Permission("GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column.BudgetMuIncome", "Поступило по Бюджету МО");
            this.Permission("GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column.BudgetMuPercent", "Процент МО");
            this.Permission("GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column.BudgetSubjectIncome", "Поступило по Бюджету субъекта");
            this.Permission("GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column.BudgetSubjectPercent", "Процент БС");
            this.Permission("GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column.FundResourceIncome", "Поступило по Средствам фонда");
            this.Permission("GkhCr.TypeWorkCr.Register.FinanceSourceRes.Column.FundResourcePercent", "Процент СФ");

            #endregion Средства источников финансирования

            #region Сметы
            this.Namespace<TypeWorkCr>("GkhCr.TypeWorkCr.Register.EstimateCalculationViewCreate", "Сметы: Просмотр, Создание");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculationViewCreate.View", "Просмотр");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculationViewCreate.Create", "Создание записей");
            this.Namespace<EstimateCalculation>("GkhCr.TypeWorkCr.Register.EstimateCalculation", "Сметы: Изменение, Удаление");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Edit", "Изменение записей");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Delete", "Удаление записей");

            this.Namespace("GkhCr.TypeWorkCr.Register.EstimateCalculation.Field", "Поля");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.TypeWorkCr", "Вид работ");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.EstimateDocument", "Документ сметы");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.ResourceStatmentDocument", "Документ ведомости ресурсов");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.FileEstimateDocument", "Файл сметы");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.TotalDirectCost", "Прямые затраты");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.OverheadSum", "Накладные расходы");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.EstimateProfit", "Сметная прибыль");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.Nds", "НДС");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.OtherCost", "Другие затраты");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.TotalEstimate", "Итого по смете");

            this.Namespace("GkhCr.TypeWorkCr.Register.EstimateCalculation.Estimate", "Записи сметы");

            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Estimate.View", "Просмотр");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Estimate.Edit", "Редактирование записей");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.Estimate.Delete", "Удаление записей");

            this.Namespace("GkhCr.TypeWorkCr.Register.EstimateCalculation.ResStat", "Записи ведомости ресурсов");

            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.ResStat.View", "Просмотр");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.ResStat.Edit", "Редактирование записей");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.ResStat.Delete", "Удаление записей");
            this.Permission("GkhCr.TypeWorkCr.Register.EstimateCalculation.ResStat.IsSumWithoutNds", "Сумма по ресурсам/материалам указана без НДС");
            #endregion Сметы

            #region Договоры подряда
            this.Namespace("GkhCr.TypeWorkCr.Register.BuildContractViewCreate", "Договоры подряда - Просмотр, Создание");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContractViewCreate.View", "Просмотр");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContractViewCreate.Create", "Создание записей");
            this.Namespace("GkhCr.TypeWorkCr.Register.BuildContractViewCreate.Column", "Колонки");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContractViewCreate.Column.Sum", "Сумма");

            this.Namespace<BuildContract>("GkhCr.TypeWorkCr.Register.BuildContract", "Договоры подряда - Изменение, Удаление");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Edit", "Изменение записей");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Delete", "Удаление записей");

            this.Namespace("GkhCr.TypeWorkCr.Register.BuildContract.TypeWork", "Виды работ");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.TypeWork.View", "Просмотр");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.TypeWork.Create", "Добавление записей");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.TypeWork.Delete", "Удаление записей");

            #region Поля
            this.Namespace<BuildContract>("GkhCr.TypeWorkCr.Register.BuildContract.Field", "Поля");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.DocumentName_Edit", "Название документа");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.DocumentNum_Edit", "Номер документа");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.DocumentDateFrom_Edit", "Дата документа");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.DocumentFile_Edit", "Файл документа");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.Sum_Edit", "Сумма договора подряда");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.DateStartWork_Edit", "Дата начала работ");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.DateEndWork_Edit", "Дата окончания работ");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.DateCancelReg_Edit", "Отклонено от регистрации");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.DateAcceptOnReg_Edit", "Принято на регистрацию");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.DateInGjiRegister_Edit", "Договор внесен в реестр ГЖИ");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.TypeContractBuild_Edit", "Тип договора");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.Inspector_Edit", "Инспектор");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.Builder_Edit", "Подрядчик");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.Description_Edit", "Описание");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.BudgetMo_Edit", "Бюджет МО");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.BudgetSubject_Edit", "Бюджет субъекта");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.OwnerMeans_Edit", "Средства собственников");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.FundMeans_Edit", "Средства фонда");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.TabResultQual_Edit", "Вкладка:«Результат квалификационного отбора»");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.ProtocolName_Edit", "Название документа квалификационного отбора");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.ProtocolNum_Edit", "Номер документа квалификационного отбора");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.ProtocolDateFrom_Edit", "Дата документа квалификационного отбора");
            this.Permission("GkhCr.TypeWorkCr.Register.BuildContract.Field.ProtocolFile_Edit", "Файл документа квалификационного отбора");

            #endregion Поля
            #endregion Договоры подряда

            #region Мониторинг СМР
            this.Namespace<TypeWorkCr>("GkhCr.TypeWorkCr.Register.MonitoringSmr", "Мониторинг СМР");

            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.View", "Просмотр");
            #region График выполнения работ
            this.Namespace<MonitoringSmr>("GkhCr.TypeWorkCr.Register.MonitoringSmr.ScheduleExecutionWork", "График выполнения работ");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ScheduleExecutionWork.View", "Чтение");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ScheduleExecutionWork.Edit", "Изменение");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ScheduleExecutionWork.AddDate", "Дополнительный срок");
            #endregion График выполнения работ

            #region Ход выполнения работ
            this.Namespace<MonitoringSmr>("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork", "Ход выполнения работ");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.CalcPercentOfCompletion", "Расчет процента выполнения");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.View", "Чтение");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Edit", "Изменение");

            this.Namespace("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field", "Поля");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_View", "Этап работы - Просмотр");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_Edit", "Этап работы - Изменение");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_View", "Производитель - Просмотр");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_Edit", "Производитель - Изменение");
            this.Namespace("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Column", "Колонки");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Column.StageWorkCr", "Этап работы");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Column.Manufacturer", "Производитель");
            #endregion Ход выполнения работ

            #region Численность рабочих
            this.Namespace<MonitoringSmr>("GkhCr.TypeWorkCr.Register.MonitoringSmr.WorkersCount", "Численность рабочих");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.WorkersCount.View", "Чтение");
            this.Permission("GkhCr.TypeWorkCr.Register.MonitoringSmr.WorkersCount.Edit", "Изменение");
            #endregion Численность рабочих

            #region Документы
            this.Namespace<MonitoringSmr>("GkhCr.TypeWorkCr.Register.MonitoringSmr.Document", "Документы");
            this.CRUDandViewPermissions("GkhCr.TypeWorkCr.Register.MonitoringSmr.Document");
            #endregion Документы

            #endregion Мониторинг СМР

            #region Акт выполненных работ
            this.Namespace<TypeWorkCr>("GkhCr.TypeWorkCr.Register.PerformedWorkActViewCreate", "Акт выполненных работ: Просмотр, создание");
            this.Permission("GkhCr.TypeWorkCr.Register.PerformedWorkActViewCreate.View", "Просмотр");
            this.Permission("GkhCr.TypeWorkCr.Register.PerformedWorkActViewCreate.Create", "Создание записей");
            this.Namespace<PerformedWorkAct>("GkhCr.TypeWorkCr.Register.PerformedWorkAct", "Акт выполненных работ: Изменение, удаление");
            this.Permission("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Edit", "Изменение записей");
            this.Permission("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Delete", "Удаление записей");

            this.Namespace<PerformedWorkAct>("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field", "Поля");
            this.Permission("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field.DocumentNum", "Номер");
            this.Permission("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field.Sum", "Сумма");
            this.Permission("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field.DateFrom", "Дата");
            this.Permission("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field.Description", "Описание");
            this.Permission("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field.Volume", "Объем");
            this.Permission("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field.CostFile", "Справка о стоимости выполненных работ и затрат");
            this.Permission("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field.DocumentFile", "Документ акта");
            this.Permission("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field.AdditionFile", "Приложение к акту");
            this.Namespace("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field.Rec", "Записи акта");
            this.CRUDandViewPermissions("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field.Rec");
            this.Namespace("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field.Payment", "Оплата акта выполненных работ");
            this.CRUDandViewPermissions("GkhCr.TypeWorkCr.Register.PerformedWorkAct.Field.Payment");

            #endregion Акт выполненных работ

            #endregion Реестры

            #endregion Объекты КР (работы)

            #region Реестр актов выполненных работ
            this.Namespace("GkhCr.WorkAct", "Реестр актов выполненных работ");
            this.Permission("GkhCr.WorkAct.View", "Просмотр");
            #endregion Реестр актов выполненных работ

            #region Реестр смет
            this.Namespace("GkhCr.Estimate", "Реестр смет");
            this.Permission("GkhCr.Estimate.View", "Просмотр");
            #endregion Реестр смет

            #region Квалификационный отбор
            this.Namespace("GkhCr.QualificationMember", "Квалификационный отбор");
            this.Permission("GkhCr.QualificationMember.View", "Просмотр");
            #endregion 

            #region Подрядчики, нарушившие условия договора
            this.Namespace("GkhCr.BuilderViolator", "Подрядчики, нарушившие условия договора");
            this.Permission("GkhCr.BuilderViolator.View", "Просмотр");
            this.Permission("GkhCr.BuilderViolator.Add", "Добавление");
            #endregion

            #region Конкурсы
            this.Namespace<Competition>("GkhCr.Competition", "Конкурсы");
            this.CRUDandViewPermissions("GkhCr.Competition");

            this.Namespace("GkhCr.Competition.Lot", "Лоты");
            this.CRUDandViewPermissions("GkhCr.Competition.Lot");

            this.Namespace("GkhCr.Competition.Document", "Документы");
            this.CRUDandViewPermissions("GkhCr.Competition.Document");

            this.Namespace("GkhCr.Competition.Protocol", "Протокол");
            this.CRUDandViewPermissions("GkhCr.Competition.Protocol");
            #endregion Конкурсы

            #region Банковские выписки
            this.Namespace("GkhCr.BankStatement", "Банковские выписки");
            this.CRUDandViewPermissions("GkhCr.BankStatement");
            #endregion Банковские выписки

            #region Платежные поручения
            this.Namespace("GkhCr.PaymentOrder", "Платежные поручения");
            this.CRUDandViewPermissions("GkhCr.PaymentOrder");
            #region Поля
            this.Namespace("GkhCr.PaymentOrder.Field", "Поля");
            this.Permission("GkhCr.PaymentOrder.Field.BankStatement_Edit", "Банковская выписка");
            this.Permission("GkhCr.PaymentOrder.Field.FinanceSource_Edit", "Разрез финансирования");
            this.Permission("GkhCr.PaymentOrder.Field.PayerContragent_Edit", "Плательщик");
            this.Permission("GkhCr.PaymentOrder.Field.DocumentNum_Edit", "Номер");
            this.Permission("GkhCr.PaymentOrder.Field.BidNum_Edit", "Номер заявки");
            this.Permission("GkhCr.PaymentOrder.Field.Sum_Edit", "Сумма по документу");
            this.Permission("GkhCr.PaymentOrder.Field.ReceiverContragent_Edit", "Получатель");
            this.Permission("GkhCr.PaymentOrder.Field.PayPurpose_Edit", "Назначение платежа");
            this.Permission("GkhCr.PaymentOrder.Field.DocumentDate_Edit", "Дата п/п");
            this.Permission("GkhCr.PaymentOrder.Field.BidDate_Edit", "Дата заявки");
            this.Permission("GkhCr.PaymentOrder.Field.RedirectFunds_Edit", "Повторно направленные средства");
            #endregion Поля
            #endregion Платежные поручения

            #region Массовая смена статусов объектов капремонта
            this.Namespace("GkhCr.ObjectCrMassStateChange", "Массовая смена статусов объектов КР");
            this.Permission("GkhCr.ObjectCrMassStateChange.View", "Просмотр");

            #endregion

            #region Отчеты
            this.Namespace("Reports.CR", "Модуль капитальный ремонт");
            this.Permission("Reports.CR.BuilderRegister", "Реестр подрядчиков");
            this.Permission("Reports.CR.CountPhoto", "Отчет \"Количество фотографий\"");
            this.Permission("Reports.CR.CountSumActAccepted", "Количество и сумма принятых госжилинспекцикй актов по форме КС-2");
            this.Permission("Reports.CR.InfoActProtocolObjectCr", "Отчет \"Информация по домам, завершенным капитальным ремонтом по наличию загруженных документов в системе\"");
            this.Permission("Reports.CR.WeeklyAboutCrProgress", "Еженедельный отчет");
            this.Permission("Reports.CR.YearlyForFund", "Ежеквартальный и годовой отчеты для Фонда");
            this.Permission("Reports.CR.MkdByTypeRepairRegister", "Реестр по видам работ");
            this.Permission("Reports.CR.FormForMoscow3", "Форма для Москвы 3");
            this.Permission("Reports.CR.BuildContractsReestr", "Реестр договоров подряда ГЖИ");
            this.Permission("Reports.CR.DefectListReport", "Информация о загруженных дефектных ведомостях");
            this.Permission("Reports.CR.CrWorkTypeSum", "Сумма по видам работ объекта КР");
            this.Permission("Reports.CR.RegistryBySource", "Отчет Реестр по источникам");
            this.Permission("Reports.CR.BuildersInfo", "Информация о подрядчиках");
            this.Permission("Reports.CR.Financing_CheckRegister", "Реестр платежных документов (за период)");
            this.Permission("Reports.CR.WorksProgress", "Отчет МО о ходе работ (по домам)");
            this.Permission("Reports.CR.ObjectCrInfoService", "Отчет информация по объектам о ходе КР по услугам (ГЖИ)");
            this.Permission("Reports.CR.WorksGraph", "Графики работ");
            this.Permission("Reports.CR.JournalKr1", "Процент отставания и выполнения работ, информация по приборам учета (Журнал ч.1)");
            this.Permission("Reports.CR.JournalKr34", "Отчет Информация по начатым и завершенным капремонтом домам (Журнал ч.3,4)");
            this.Permission("Reports.CR.JournalCr6", "Полная информация по охваченному капремонту МКД (Журнал ч.6)");
            this.Permission("Reports.CR.DetectRepeatingProgram", "Выявление повторных объектов по программам капремонта");
            this.Permission("Reports.CR.ComparePrograms", "Отчет по сверке программ КР");
            this.Permission("Reports.CR.FinancingAnnex4F1", "Приложение 4 к отчету о расходовании средств Фонда (форма 1)");
            this.Permission("Reports.CR.FinancingAnnex4F4", "Приложение 4 к отчету о расходовании средств Фонда (форма 4)");
            this.Permission("Reports.CR.WorkKindsByMonitoringSmrReport", "Отчет по видам работ по мониторингу СМР (по 100% работам)");
            this.Permission("Reports.CR.RepairProgressByKindOfWork", "Отчет по видам работ (сравнение с программой КР по 100% работам)");
            this.Permission("Reports.CR.WorkKindsCompareWithBaseProgram", "Отчет по видам работ (сверка с основной программой)");
            this.Permission("Reports.CR.FulfilledWorkAmountReport", "Выполненные объемы работ по МО");
            this.Permission("Reports.CR.CompletionOfTheGraphicOnGivenDate", "Завершение работ по графику на заданную дату");
            this.Permission("Reports.CR.HousesCompletedWorkFact", "Отчет по домам, по которым работы завершены фактически");
            this.Permission("Reports.CR.StaffingWorkers", "05_Нормативная численность рабочих");
            this.Permission("Reports.CR.PlannedAllocationOfWorks", "Отчет Плановое распределение выполнения работ по месяцам года по МО РТ");
            this.Permission("Reports.CR.NormativeStaffingWorkers", "05_Численность рабочих на заданную дату");
            this.Permission("Reports.CR.NeedMaterialsExtendedReport", "Потребность в материалах (расширенная)");
            this.Permission("Reports.CR.StaffingWorkersByRealtyObjects", "Нормативная численность рабочих (по домам)");
            this.Permission("Reports.CR.ActAuditDataExpense", "Отчет по Актам сверок данных о расходах");
            this.Permission("Reports.CR.CountWorkCrReport", "Отчет по количеству работ в программе капремонта");
            this.Permission("Reports.CR.PhotoArchiveReport", "Фото архив");
            this.Permission("Reports.CR.ProtocolsActsGjiReestr", "Реестр протоколов и актов ГЖИ");
            this.Permission("Reports.CR.ActuallyStartedWorksByHouses", "Отчет по домам, по которым работы начаты фактически");
            this.Permission("Reports.CR.DataStartedFinishedWorkReport", "Сведения о начатых и завершенных работах");
            this.Permission("Reports.CR.InformationOnObjectsCr", "Отчет Информация по объектам о ходе КР по работам (ГЖИ)");
            this.Permission("Reports.CR.HomesWithoutGraphics", "Отчет по домам, у которых отсутствуют графики");
            this.Permission("Reports.CR.LaggingPerformanceOfWork", "Отставание выполнения по заданному виду работ");
            this.Permission("Reports.CR.CountRealObjByBacklogWork", "Отчет по домам, по которым присутствует отставание по графику выполнения работ");
            this.Permission("Reports.CR.WorkScheldudeInfo", "Информация по графикам производства работ (Журнал ч.2)");
            this.Permission("Reports.CR.DataDevicesStatementReport", "Сведения по приборам Учета");
            this.Permission("Reports.CR.AreaCrMkd", "Площадь охваченных капремонтом МКД (Журнал ч.5)");
            this.Permission("Reports.CR.StatusInformation", "Информация по статусам");
            this.Permission("Reports.CR.InformationByBuilders", "Информация по подрядчикам по выбранной программе и на заданную дату");
            this.Permission("Reports.CR.ListByManyApartmentsHouses", "Перечень многоквартирных домов");
            this.Permission("Reports.CR.ByProgramCrNew", "По программам КР (новый)");
            this.Permission("Reports.CR.ProgrammCrRealization", "Реализация программы капитального ремонта");
            this.Permission("Reports.CR.UploadingAtDataForFund", "Выгрузка сведений по аварийности для Фонда");
            this.Permission("Reports.CR.NeedMaterialsReport", "Потребность в материалах");
            this.Permission("Reports.CR.ProgramCrInformationForFund", "Сведения по программе КР для Фонда");
            this.Permission("Reports.CR.EconomyByTypeWork", "Экономия по видам работ (внутри дома)");
            this.Permission("Reports.CR.FillingPassportControl", "Контроль заполнения паспорта");
            this.Permission("Reports.CR.ProgramCrProgressOperativeReport", "Оперативный отчет о ходе реализации программ КР МКД");
            this.Permission("Reports.CR.ExtendedInformationAboutTransferFunds", "Информация о перечислении средств (расширенная)");
            this.Permission("Reports.CR.AnalysOfRealizByProgram", "Анализ реализации программы");
            this.Permission("Reports.CR.QuartAndAnnualRepByFundExt", "Ежеквартальный и годовой отчеты для Фонда (расширенный)");
            this.Permission("Reports.CR.DetectRepeatingProgramDistribServices", "Выявление повторных объектов по программам капремонта (распределение услуг)");
            this.Permission("Reports.CR.InformationAboutContractors", "Сведения по подрядчикам");
            this.Permission("Reports.CR.EconomyByTypeWorkToFund", "Экономия по видам работ (внутри дома) для Фонда");
            this.Permission("Reports.CR.RegisterMkdByTypeRepair", "Реестр многоквартирных домов по видам ремонта");
            this.Permission("Reports.CR.RegisterMkdToBeRepaired", "02_Реестр МКД, подлежащих ремонту");

            this.Permission("Reports.CR.CrAggregatedReport", "Свод_2");
            this.Permission("Reports.CR.ObjectCrInfo", "Отчет информация по объектам о ходе КР");
            this.Permission("Reports.CR.InformOnHousesIncludedProgramCr", "Информация по домам включенных в программу капитального ремонта");
            this.Permission("Reports.CR.ArchiveSmrReport", "Ход выполнения работ в Мониторинге СМР");
            this.Permission("Reports.CR.ListByManyApartmentsHousesTat", "Перечень многоквартирных домов (Татарстан)");
            this.Permission("Reports.CR.FormForMoscow3Tat", "Форма для Москвы 3 (Татарстан)");
            this.Permission("Reports.CR.YearlyForFundTat", "Ежеквартальный и годовой отчеты для Фонда (Татарстан)");
            this.Permission("Reports.CR.PlanedProgramIndicators", "Планирумые показатели выполнения программы КР");
            #endregion

            #region Справочники
            this.Namespace("GkhCr.Dict", "Справочники");
            this.Namespace("GkhCr.Dict.QualMember", "Участники квалификационного отбора");
            this.CRUDandViewPermissions("GkhCr.Dict.QualMember");

            this.Namespace("GkhCr.Dict.FinanceSource", "Разрезы финансирования");
            this.CRUDandViewPermissions("GkhCr.Dict.FinanceSource");
            this.Namespace("GkhCr.Dict.FinanceSource.Works", "Виды работ");
            this.Permission("GkhCr.Dict.FinanceSource.Works.Create", "Создание записей");
            this.Permission("GkhCr.Dict.FinanceSource.Works.Delete", "Удаление записей");

            this.Namespace("GkhCr.Dict.StageWorkCr", "Этапы работ");
            this.CRUDandViewPermissions("GkhCr.Dict.StageWorkCr");

            this.Namespace("GkhCr.Dict.Official", "Должностные лица");
            this.CRUDandViewPermissions("GkhCr.Dict.Official");

            this.Namespace("GkhCr.Dict.TerminationReason", "Причины расторжения договора");
            this.Permission("GkhCr.Dict.TerminationReason.View", "Просмотр");

            this.Namespace("GkhCr.Dict.BasisOverhaulDocKind", "Вид документа основания ДПКР");
            this.CRUDandViewPermissions("GkhCr.Dict.BasisOverhaulDocKind");
            #endregion

            #region Жилые дома
            this.Namespace("Gkh.RealityObject.Register.ProgramCr", "Программы КР");
            this.Permission("Gkh.RealityObject.Register.ProgramCr.View", "Просмотр");
            #endregion

            #region SpecialObjectCr
            #region Объекты КР
            this.Namespace("GkhCr.SpecialObjectCrViewCreate", "Реестр объектов КР для владельцев специальных счетов");
            this.Permission("GkhCr.SpecialObjectCrViewCreate.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCrViewCreate.Create", "Создание записей");
            this.Namespace("GkhCr.SpecialObjectCrViewCreate.Columns", "Столбцы");
            this.Permission("GkhCr.SpecialObjectCrViewCreate.Columns.DateAcceptCrGji", "Принят ГЖИ");

            this.Namespace<SpecialObjectCr>("GkhCr.SpecialObjectCr", "Объекты КР для владельцев специальных счетов: Просмотр, изменение");
            this.Permission("GkhCr.SpecialObjectCr.Edit", "Изменение записей");
            this.Permission("GkhCr.SpecialObjectCr.Delete", "Удаление записей");

            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.Inspector_View", "Инспектор - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateInGjiRegister_View", "Договор внесен в реестр ГЖИ - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateCancelReg_View", "Отклонено от регистрации - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateAcceptOnReg_View", "Принято на регистрацию - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.Description_View", "Описание - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.TabResultQual_View", "Вкладка:«Результат квалификационного отбора» - Просмотр");

            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.TabResultQual_Edit", "Вкладка:«Результат квалификационного отбора» - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.Description_Edit", "Описание - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.Inspector_Edit", "Инспектор - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateInGjiRegister_Edit", "Договор внесен в реестр ГЖИ - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateCancelReg_Edit", "Отклонено от регистрации - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateAcceptOnReg_Edit", "Принято на регистрацию - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationContractTab_View", "Вкладка:«Расторжение договора» - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationDate_View", "Дата расторжения - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationDate_Edit", "Дата расторжения - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationDocumentFile_View", "Документ-основание - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationDocumentFile_Edit", "Документ-основание - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationReason_View", "Основание расторжения - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.TerminationReason_Edit", "Основание расторжения - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.GuaranteePeriod_View", "Гарантийный срок (лет) - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.GuaranteePeriod_Edit", "Гарантийный срок (лет) - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.UrlResultTrading_View", "Ссылка на результаты проведения торгов - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.UrlResultTrading_Edit", "Ссылка на результаты проведения торгов - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.FinanceSource_View", "Разрез финансирования - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.Description_View", "Описание - Просмотр");

            this.Namespace("GkhCr.SpecialObjectCr.CreateContract", "Сформировать договора подряда на основе договора из раздела \"Конкурсы\"");
            this.Permission("GkhCr.SpecialObjectCr.CreateContract.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.CreateContract.Edit", "Изменение");

            this.Namespace<SpecialObjectCr>("GkhCr.SpecialObjectCr.Field", "Поля");
            this.Permission("GkhCr.SpecialObjectCr.Field.RealityObject_Edit", "Жилой дом");
            this.Permission("GkhCr.SpecialObjectCr.Field.ProgramCr_Edit", "Программа");

            this.Permission("GkhCr.SpecialObjectCr.Field.ProgramNum_View", "Номер по программе - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.ProgramNum_Edit", "Номер по программе - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.GjiNum_View", "Номер ГЖИ - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.GjiNum_Edit", "Номер ГЖИ - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.FederalNumber_View", "Федеральный номер - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.FederalNumber_Edit", "Федеральный номер - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.Description_Edit", "Примечание");

            this.Permission("GkhCr.SpecialObjectCr.Field.SumSMR_View", "Сумма на СМР - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.SumSMR_Edit", "Сумма на СМР - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.SumDevolopmentPSD_View", "Сумма на разработку и экспертизу ПСД - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.SumDevolopmentPSD_Edit", "Сумма на разработку и экспертизу ПСД - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.SumTehInspection_View", "Сумма на технадзор - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.SumTehInspection_Edit", "Сумма на технадзор - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.SumSMRApproved_View", "Утвержденная сумма СМР - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.SumSMRApproved_Edit", "Утвержденная сумма СМР - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.DateGjiReg_View", "Дата регистрации ГЖИ - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.DateGjiReg_Edit", "Дата регистрации ГЖИ - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.DateEndBuilder_View", "Дата завершения работ подрядчиком - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.DateEndBuilder_Edit", "Дата завершения работ подрядчиком - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.DateStopWorkGji_View", "Дата остановки работ ГЖИ - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.DateStopWorkGji_Edit", "Дата остановки работ ГЖИ - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.DateAcceptCrGji_View", "Дата принятия капитального ремонта ГЖИ - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.DateAcceptCrGji_Edit", "Дата принятия капитального ремонта ГЖИ - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.DateStartWork_View", "Дата начала работ - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.DateStartWork_Edit", "Дата начала работ - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.DateEndWork_View", "Дата окончания работ - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.DateEndWork_Edit", "Дата окончания работ - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.DateAcceptReg_View", "Дата принятия на регистрацию - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.DateAcceptReg_Edit", "Дата принятия на регистрацию - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.DateCancelReg_View", "Дата отклонения от регистрации - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.DateCancelReg_Edit", "Дата отклонения от регистрации - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.MaxKpkrAmount_View", "Предельная сумма из КПКР - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.MaxKpkrAmount_Edit", "Предельная сумма из КПКР - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.FactAmountSpent_View", "Фактически освоенная сумма - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.FactAmountSpent_Edit", "Фактически освоенная сумма - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.FactStartDate_View", "Фактическая дата начала работ - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.FactStartDate_Edit", "Фактическая дата начала работ - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.FactEndDate_View", "Фактическая дата окончания работ - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.FactEndDate_Edit", "Фактическая дата окончания работ - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Field.WarrantyEndDate_View", "Дата окончания гарантийных обязательств - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.WarrantyEndDate_Edit", "Дата окончания гарантийных обязательств - Изменение");

            this.Namespace("GkhCr.SpecialObjectCr.Field.AllowReneg", "Повторное согласование");
            this.Permission("GkhCr.SpecialObjectCr.Field.AllowReneg.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Field.AllowReneg.Edit", "Изменение");

            #region Реестры
            this.Namespace<SpecialObjectCr>("GkhCr.SpecialObjectCr.Register", "Реестры");

            this.Namespace("GkhCr.SpecialObjectCr.Register.ContractCrViewCreate", "Договоры на услуги - Просмотр, Создание");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCrViewCreate.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCrViewCreate.Create", "Создание записей");

            this.Namespace<SpecialContractCr>("GkhCr.SpecialObjectCr.Register.ContractCr", "Договоры на услуги - Изменение, Удаление");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Edit", "Изменение записей");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Delete", "Удаление записей");


            #region Договоры
            #region Поля
            this.Namespace<SpecialContractCr>("GkhCr.SpecialObjectCr.Register.ContractCr.Field", "Поля");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.DocumentName_Edit", "Документ - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.DocumentNum_Edit", "Номер документа - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.DateFrom_Edit", "Дата от - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.TypeContractObject_Edit", "Тип договора объекта КР - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.SumContract_Edit", "Сумма договора - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.BudgetMo_Edit", "Бюджет МО - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.BudgetSubject_Edit", "Бюджет субъекта - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.OwnerMeans_Edit", "Средства собственников - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.FundMeans_Edit", "Средства фонда - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.File_Edit", "Файл - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.Contragent_Edit", "Подрядная организация - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.FinanceSource_Edit", "Разрез финансирования - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.Description_Edit", "Описание - Изменение - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.DateStartWork_Edit", "Дата начала работ - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.DateEndWork_Edit", "Дата окончания работ - Изменение");

            this.Namespace("GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType", "Типы договоров");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType.Expertise", "Экспертиза");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType.RoMoAggreement", "Договор о функции заказчика между РО и МО");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType.BuildingControl", "Строительный контроль");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType.TechSepervision", "Технический надзор");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType.Psd", "ПСД");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.ContractCrType.Insurance", "Страхование");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.Customer_View", "Заказчик - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.Field.Customer_Edit", "Заказчик - Изменение");
            this.Namespace("GkhCr.SpecialObjectCr.Register.ContractCr.TypeWork", "Виды работ");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.TypeWork.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.TypeWork.Create", "Добавление записей");
            this.Permission("GkhCr.SpecialObjectCr.Register.ContractCr.TypeWork.Delete", "Удаление записей");

            #endregion Поля
            #endregion Договоры

            #region Протоколы
            this.Namespace("GkhCr.SpecialObjectCr.Register.Protocol", "Протоколы");
            this.CRUDandViewPermissions("GkhCr.SpecialObjectCr.Register.Protocol");
            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.SumActVerificationOfCosts", "Сумма Акта сверки данных о расходах");
            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.TypeWork.View", "Виды работ - Просмотр");

            this.Namespace("GkhCr.SpecialObjectCr.Register.Protocol.Field", "Поля");

            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.SumActVerificationOfCosts_View", "Сумма Акта сверки данных о расходах - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.SumActVerificationOfCosts_Edit", "Сумма Акта сверки данных о расходах - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.CountVote_View", "Количество голосов (кв.м) - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.CountVote_Edit", "Количество голосов (кв.м) - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.CountVoteGeneral_View", "Общее количество голосов (кв.м) - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.CountVoteGeneral_Edit", "Общее количество голосов (кв.м) - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.CountAccept_View", "Доля принявших участие (%) - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.CountAccept_Edit", "Доля принявших участие (%) - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.GradeClient_View", "Оценка заказчика - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.GradeClient_Edit", "Оценка заказчика - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.GradeOccupant_View", "Оценка жителей - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.GradeOccupant_Edit", "Оценка жителей - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.Protocol.Field.DecisionOms_View", "Решение ОМС - Просмотр");


            #endregion Список протоколов

            #region Дефектные ведомости
            this.Namespace("GkhCr.SpecialObjectCr.Register.DefectListViewCreate", "Дефектные ведомости - Просмотр, Создание");
            this.Permission("GkhCr.SpecialObjectCr.Register.DefectListViewCreate.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.DefectListViewCreate.Create", "Создание записей");
            this.Namespace<SpecialDefectList>("GkhCr.SpecialObjectCr.Register.DefectList", "Дефектные ведомости - Изменение, Удаление");
            this.Permission("GkhCr.SpecialObjectCr.Register.DefectList.Edit", "Изменение записей");
            this.Permission("GkhCr.SpecialObjectCr.Register.DefectList.Delete", "Удаление записей");

            this.Namespace<SpecialDefectList>("GkhCr.SpecialObjectCr.Register.DefectList.Field", "Поля");
            this.Permission("GkhCr.SpecialObjectCr.Register.DefectList.Field.DocumentName_Edit", "Наименование");
            this.Permission("GkhCr.SpecialObjectCr.Register.DefectList.Field.DocumentDate_Edit", "Дата");
            this.Permission("GkhCr.SpecialObjectCr.Register.DefectList.Field.Work_Edit", "Вид работы");
            this.Permission("GkhCr.SpecialObjectCr.Register.DefectList.Field.File_Edit", "Файл");
            this.Permission("GkhCr.SpecialObjectCr.Register.DefectList.Field.Sum_Edit", "Сумма  по ведомости, руб");
            #endregion Дефектные ведомости

            #region Виды работ
            this.Namespace("GkhCr.SpecialObjectCr.Register.TypeWork", "Виды работ");
            this.CRUDandViewPermissions("GkhCr.SpecialObjectCr.Register.TypeWork");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWork.DeleteStruclEl", "Удаление отдельных КЭ");
            #region Поля
            this.Namespace("GkhCr.SpecialObjectCr.Register.TypeWork.Field", "Поля");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWork.Field.FinanceSource_Edit", "Разрез финансирования");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWork.Field.TypeWork_Edit", "Вид работы");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWork.Field.SumMaterialsRequirement_Edit", "Потребность материалов");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWork.Field.HasPsd_Edit", "Наличие ПСД");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWork.Field.Volume_Edit", "Объем");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWork.Field.Sum_Edit", "Сумма");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWork.Field.Description_Edit", "Примечание");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWork.Field.DateStartWork_Edit", "Начало выполнения работ - Редактирование");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWork.Field.DateStartWork_View", "Начало выполнения работ - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWork.Field.DateEndWork_Edit", "Окончание выполнения работ - Редактирование");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWork.Field.DateEndWork_View", "Окончание выполнения работ - Просмотр");
            #endregion Поля
            #endregion Список видов работ

            #region Удаленные объекты КР
            this.Namespace("GkhCr.SpecialObjectCr.Register.DeletedObject", "Удаленные объекты КР");
            this.Permission("GkhCr.SpecialObjectCr.Register.DeletedObject.View", "Просмотр");
            #region Поля
            this.Namespace("GkhCr.SpecialObjectCr.Register.DeletedObject.Field", "Поля");
            this.Permission("GkhCr.SpecialObjectCr.Register.DeletedObject.Field.Recover", "Восстановление");
            #endregion Поля
            #endregion

            #region Журнал изменений
            this.Namespace("GkhCr.SpecialObjectCr.Register.TypeWorkCrHistory", "Журнал изменений");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWorkCrHistory.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.TypeWorkCrHistory.Restore", "Восстановление");
            #endregion Журнал изменений

            #region Cредства источников финансирования
            this.Namespace("GkhCr.SpecialObjectCr.Register.FinanceSourceRes", "Разрезы финансирования");
            this.CRUDandViewPermissions("GkhCr.SpecialObjectCr.Register.FinanceSourceRes");

            this.Namespace("GkhCr.SpecialObjectCr.Register.FinanceSourceRes.Column", "Столбцы");
            this.Permission("GkhCr.SpecialObjectCr.Register.FinanceSourceRes.Column.BudgetMuIncome", "Поступило по Бюджету МО");
            this.Permission("GkhCr.SpecialObjectCr.Register.FinanceSourceRes.Column.BudgetMuPercent", "Процент МО");
            this.Permission("GkhCr.SpecialObjectCr.Register.FinanceSourceRes.Column.BudgetSubjectIncome", "Поступило по Бюджету субъекта");
            this.Permission("GkhCr.SpecialObjectCr.Register.FinanceSourceRes.Column.BudgetSubjectPercent", "Процент БС");
            this.Permission("GkhCr.SpecialObjectCr.Register.FinanceSourceRes.Column.FundResourceIncome", "Поступило по Средствам фонда");
            this.Permission("GkhCr.SpecialObjectCr.Register.FinanceSourceRes.Column.FundResourcePercent", "Процент СФ");

            #endregion Средства источников финансирования

            #region Лицевые счета
            this.Namespace("GkhCr.SpecialObjectCr.Register.PersonalAccount", "Лицевые счета");
            this.CRUDandViewPermissions("GkhCr.SpecialObjectCr.Register.PersonalAccount");
            #endregion Список лицевых счетов

            #region Сметы
            this.Namespace<SpecialObjectCr>("GkhCr.SpecialObjectCr.Register.EstimateCalculationViewCreate", "Сметы: Просмотр, Создание");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculationViewCreate.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculationViewCreate.Create", "Создание записей");
            this.Namespace<SpecialEstimateCalculation>("GkhCr.SpecialObjectCr.Register.EstimateCalculation", "Сметы: Изменение, Удаление");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Edit", "Изменение записей");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Delete", "Удаление записей");

            this.Namespace("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field", "Поля");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.TypeWorkCr", "Вид работ");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.EstimateDocument", "Документ сметы");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.ResourceStatmentDocument", "Документ ведомости ресурсов");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.FileEstimateDocument", "Файл сметы");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.TotalDirectCost", "Прямые затраты");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.OverheadSum", "Накладные расходы");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.EstimateProfit", "Сметная прибыль");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.Nds", "НДС");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.OtherCost", "Другие затраты");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.TotalEstimate", "Итого по смете");

            this.Namespace("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Estimate", "Записи сметы");

            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Estimate.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Estimate.Edit", "Редактирование записей");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.Estimate.Delete", "Удаление записей");

            this.Namespace("GkhCr.SpecialObjectCr.Register.EstimateCalculation.ResStat", "Записи ведомости ресурсов");

            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.ResStat.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.ResStat.Edit", "Редактирование записей");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.ResStat.Delete", "Удаление записей");
            this.Permission("GkhCr.SpecialObjectCr.Register.EstimateCalculation.ResStat.IsSumWithoutNds", "Сумма по ресурсам/материалам указана без НДС");
            #endregion Сметы

            #region Квалификационный отбор
            this.Namespace("GkhCr.SpecialObjectCr.Register.Qualification", "Квалификационный отбор");
            this.CRUDandViewPermissions("GkhCr.SpecialObjectCr.Register.Qualification");
            #endregion Квалификационный отбор

            #region Квалификационный отбор
            this.Namespace("GkhCr.SpecialObjectCr.Register.Competition", "Конкурсы");
            this.Permission("GkhCr.SpecialObjectCr.Register.Competition.View", "Просмотр");
            #endregion Квалификационный отбор

            #region Задание на проектирование
            this.Namespace("GkhCr.SpecialObjectCr.Register.DesignAssignment", "Задание на проектирование");
            this.Permission("GkhCr.SpecialObjectCr.Register.DesignAssignment.View", "Просмотр");
            #endregion Задание на проектирование

            #region Договоры подряда
            this.Namespace("GkhCr.SpecialObjectCr.Register.BuildContractViewCreate", "Договоры подряда - Просмотр, Создание");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContractViewCreate.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContractViewCreate.Create", "Создание записей");
            this.Namespace("GkhCr.SpecialObjectCr.Register.BuildContractViewCreate.Column", "Колонки");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContractViewCreate.Column.Sum", "Сумма");

            this.Namespace<SpecialBuildContract>("GkhCr.SpecialObjectCr.Register.BuildContract", "Договоры подряда - Изменение, Удаление");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Edit", "Изменение записей");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Delete", "Удаление записей");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Print", "Печать");

            this.Namespace("GkhCr.SpecialObjectCr.Register.BuildContract.TypeWork", "Виды работ");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.TypeWork.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.TypeWork.Create", "Добавление записей");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.TypeWork.Delete", "Удаление записей");

            #region Поля
            this.Namespace<SpecialBuildContract>("GkhCr.SpecialObjectCr.Register.BuildContract.Field", "Поля");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.DocumentName_Edit", "Название документа - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.DocumentNum_Edit", "Номер документа - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.DocumentDateFrom_Edit", "Дата документа - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.DocumentFile_Edit", "Файл документа - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.Sum_Edit", "Сумма договора подряда - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateStartWork_Edit", "Дата начала работ - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.DateEndWork_Edit", "Дата окончания работ - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.TypeContractBuild_Edit", "Тип договора - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.Builder_Edit", "Подрядчик - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.BudgetMo_Edit", "Бюджет МО - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.BudgetSubject_Edit", "Бюджет субъекта - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.OwnerMeans_Edit", "Средства собственников - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.FundMeans_Edit", "Средства фонда - Изменение");

            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.ProtocolName_Edit", "Название документа квалификационного отбора - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.ProtocolNum_Edit", "Номер документа квалификационного отбора - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.ProtocolDateFrom_Edit", "Дата документа квалификационного отбора - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.ProtocolFile_Edit", "Файл документа квалификационного отбора - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.IsLawProvided_Edit", "Проведение отбора предусмотрено законодательством - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.WebSite_Edit", "Адрес сайта с информацией об отборе - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractState_Edit", "Состояние договора - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.IsLawProvided_View", "Проведение отбора предусмотрено законодательством - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.WebSite_View", "Адрес сайта с информацией об отборе - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractState_View", "Состояние договора - Просмотр");

            this.Namespace("GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractType", "Типы договоров");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractType.Smr", "На СМР");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractType.Device", "На приборы");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractType.Lift", "На лифты");
            this.Permission("GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractType.EnergySurvey", "На энергообследование");
            #endregion Поля
            #endregion Договоры подряда

            #region Мониторинг СМР
            this.Namespace<SpecialObjectCr>("GkhCr.SpecialObjectCr.Register.MonitoringSmr", "Мониторинг СМР");

            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.View", "Просмотр");
            #region График выполнения работ
            this.Namespace<SpecialMonitoringSmr>("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ScheduleExecutionWork", "График выполнения работ");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.View", "Чтение");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.Edit", "Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.AddDate", "Дополнительный срок");

            this.Namespace("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.Column", "Колонки");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.Column.FinanceSource", "Разрез финансирования");
            #endregion График выполнения работ

            #region Ход выполнения работ
            this.Namespace<SpecialMonitoringSmr>("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork", "Ход выполнения работ");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.CalcPercentOfCompletion", "Расчет процента выполнения");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.View", "Чтение");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Edit", "Изменение");

            this.Namespace("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field", "Поля");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_View", "Этап работы - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_Edit", "Этап работы - Изменение");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_View", "Производитель - Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_Edit", "Производитель - Изменение");
            this.Namespace("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Column", "Колонки");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Column.StageWorkCr", "Этап работы");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.Column.Manufacturer", "Производитель");
            #endregion Ход выполнения работ

            #region Численность рабочих
            this.Namespace<SpecialMonitoringSmr>("GkhCr.SpecialObjectCr.Register.MonitoringSmr.WorkersCount", "Численность рабочих");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.WorkersCount.View", "Чтение");
            this.Permission("GkhCr.SpecialObjectCr.Register.MonitoringSmr.WorkersCount.Edit", "Изменение");
            #endregion Численность рабочих

            #region Документы
            this.Namespace<SpecialMonitoringSmr>("GkhCr.SpecialObjectCr.Register.MonitoringSmr.Document", "Документы");
            this.CRUDandViewPermissions("GkhCr.SpecialObjectCr.Register.MonitoringSmr.Document");
            #endregion Документы

            #endregion Мониторинг СМР

            #region Акт выполненных работ
            this.Namespace<SpecialObjectCr>("GkhCr.SpecialObjectCr.Register.PerformedWorkActViewCreate", "Акт выполненных работ: Просмотр, создание");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkActViewCreate.View", "Просмотр");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkActViewCreate.Create", "Создание записей");
            this.Namespace<SpecialPerformedWorkAct>("GkhCr.SpecialObjectCr.Register.PerformedWorkAct", "Акт выполненных работ: Изменение, удаление");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Edit", "Изменение записей");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Delete", "Удаление записей");

            this.Namespace<SpecialPerformedWorkAct>("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field", "Поля");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Work", "Вид работ");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.DocumentNum", "Номер");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Sum", "Сумма");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.DateFrom", "Дата");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Description", "Описание");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Volume", "Объем");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.CostFile", "Справка о стоимости выполненных работ и затрат");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.DocumentFile", "Документ акта");
            this.Permission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.AdditionFile", "Приложение к акту");
            this.FieldPermission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.RepresentativeSigned", "Акт подписан представителем собственников");
            this.FieldPermission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.RepresentativeName", "ФИО представителя");
            this.FieldPermission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.ExploitationAccepted", "Принято в эксплуатацию");
            this.FieldPermission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.WarrantyStartDate", "Дата начала гарантийного срока");
            this.FieldPermission("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.WarrantyEndDate", "Дата окончания гарантийного срока");
            this.Namespace("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Rec", "Записи акта");
            this.CRUDandViewPermissions("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Rec");
            this.Namespace("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Payment", "Оплата акта выполненных работ");
            this.CRUDandViewPermissions("GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Field.Payment");

            #endregion Акт выполненных работ

            #endregion Реестры

            #endregion Объекты КР

            #region Объекты КР (работы)
            this.Namespace<SpecialTypeWorkCr>("GkhCr.SpecialTypeWorkCr", "Объекты КР для владельцев специальных счетов (работы)");
            this.CRUDandViewPermissions("GkhCr.SpecialTypeWorkCr");

            this.Namespace<SpecialTypeWorkCr>("GkhCr.SpecialTypeWorkCr.Field", "Поля");
            this.Permission("GkhCr.SpecialTypeWorkCr.Field.FinanceSource_Edit", "Разрез финансирования");
            this.Permission("GkhCr.SpecialTypeWorkCr.Field.Year_Edit", "Год ремонта");
            this.Permission("GkhCr.SpecialTypeWorkCr.Field.Volume_Edit", "Объем");
            this.Permission("GkhCr.SpecialTypeWorkCr.Field.Sum_Edit", "Сумма");

            #region Реестры
            this.Namespace<SpecialTypeWorkCr>("GkhCr.SpecialTypeWorkCr.Register", "Реестры");

            #region Обследование объекта
            this.Namespace<SpecialTypeWorkCr>("GkhCr.SpecialTypeWorkCr.Register.Inspection", "Обследование объекта");
            this.CRUDandViewPermissions("GkhCr.SpecialTypeWorkCr.Register.Inspection");

            #endregion Обследование объекта

            #region Договоры
            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.ContractCrViewCreate", "Договоры - Просмотр, Создание");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCrViewCreate.View", "Просмотр");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCrViewCreate.Create", "Создание записей");
            this.Namespace<SpecialContractCr>("GkhCr.SpecialTypeWorkCr.Register.ContractCr", "Договоры на услуги - Изменение, Удаление");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Edit", "Изменение записей");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Delete", "Удаление записей");

            #region Поля
            this.Namespace<SpecialContractCr>("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field", "Поля");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.DocumentName_Edit", "Документ");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.DocumentNum_Edit", "Номер документа");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.DateFrom_Edit", "Дата от");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.TypeContractObject_Edit", "Тип договора объекта КР");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.SumContract_Edit", "Сумма договора");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.BudgetMo_Edit", "Бюджет МО");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.BudgetSubject_Edit", "Бюджет субъекта");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.OwnerMeans_Edit", "Средства собственников");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.FundMeans_Edit", "Средства фонда");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.File_Edit", "Файл");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.Contragent_Edit", "Участник");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.FinanceSource_Edit", "Разрез финансирования");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.ContractCr.Field.Description_Edit", "Описание");
            #endregion Поля

            #endregion Договоры

            #region Протоколы
            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.Protocol", "Протоколы");
            this.CRUDandViewPermissions("GkhCr.SpecialTypeWorkCr.Register.Protocol");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.Protocol.SumActVerificationOfCosts", "Сумма Акта сверки данных о расходах");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.Protocol.TypeWork.View", "Виды работ - Просмотр");

            #endregion Список протоколов

            #region Дефектные ведомости
            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.DefectListViewCreate", "Дефектные ведомости - Просмотр, Создание");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.DefectListViewCreate.View", "Просмотр");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.DefectListViewCreate.Create", "Создание записей");
            this.Namespace<SpecialDefectList>("GkhCr.SpecialTypeWorkCr.Register.DefectList", "Дефектные ведомости - Изменение, Удаление");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.DefectList.Edit", "Изменение записей");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.DefectList.Delete", "Удаление записей");

            this.Namespace<SpecialDefectList>("GkhCr.SpecialTypeWorkCr.Register.DefectList.Field", "Поля");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.DefectList.Field.DocumentName_Edit", "Наименование");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.DefectList.Field.DocumentDate_Edit", "Дата");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.DefectList.Field.Work_Edit", "Вид работы");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.DefectList.Field.File_Edit", "Файл");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.DefectList.Field.Sum_Edit", "Сумма  по ведомости, руб");
            #endregion Дефектные ведомости

            #region Cредства источников финансирования
            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.FinanceSourceRes", "Разрезы финансирования");
            this.CRUDandViewPermissions("GkhCr.SpecialTypeWorkCr.Register.FinanceSourceRes");

            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.FinanceSourceRes.Column", "Столбцы");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.FinanceSourceRes.Column.BudgetMuIncome", "Поступило по Бюджету МО");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.FinanceSourceRes.Column.BudgetMuPercent", "Процент МО");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.FinanceSourceRes.Column.BudgetSubjectIncome", "Поступило по Бюджету субъекта");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.FinanceSourceRes.Column.BudgetSubjectPercent", "Процент БС");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.FinanceSourceRes.Column.FundResourceIncome", "Поступило по Средствам фонда");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.FinanceSourceRes.Column.FundResourcePercent", "Процент СФ");

            #endregion Средства источников финансирования

            #region Сметы
            this.Namespace<SpecialTypeWorkCr>("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculationViewCreate", "Сметы: Просмотр, Создание");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculationViewCreate.View", "Просмотр");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculationViewCreate.Create", "Создание записей");
            this.Namespace<SpecialEstimateCalculation>("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation", "Сметы: Изменение, Удаление");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Edit", "Изменение записей");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Delete", "Удаление записей");

            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Field", "Поля");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Field.TypeWorkCr", "Вид работ");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Field.EstimateDocument", "Документ сметы");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Field.ResourceStatmentDocument", "Документ ведомости ресурсов");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Field.FileEstimateDocument", "Файл сметы");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Field.TotalDirectCost", "Прямые затраты");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Field.OverheadSum", "Накладные расходы");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Field.EstimateProfit", "Сметная прибыль");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Field.Nds", "НДС");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Field.OtherCost", "Другие затраты");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Field.TotalEstimate", "Итого по смете");

            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Estimate", "Записи сметы");

            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Estimate.View", "Просмотр");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Estimate.Edit", "Редактирование записей");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.Estimate.Delete", "Удаление записей");

            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.ResStat", "Записи ведомости ресурсов");

            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.ResStat.View", "Просмотр");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.ResStat.Edit", "Редактирование записей");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.ResStat.Delete", "Удаление записей");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.EstimateCalculation.ResStat.IsSumWithoutNds", "Сумма по ресурсам/материалам указана без НДС");
            #endregion Сметы

            #region Договоры подряда
            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.BuildContractViewCreate", "Договоры подряда - Просмотр, Создание");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContractViewCreate.View", "Просмотр");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContractViewCreate.Create", "Создание записей");
            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.BuildContractViewCreate.Column", "Колонки");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContractViewCreate.Column.Sum", "Сумма");

            this.Namespace<SpecialBuildContract>("GkhCr.SpecialTypeWorkCr.Register.BuildContract", "Договоры подряда - Изменение, Удаление");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Edit", "Изменение записей");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Delete", "Удаление записей");

            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.BuildContract.TypeWork", "Виды работ");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.TypeWork.View", "Просмотр");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.TypeWork.Create", "Добавление записей");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.TypeWork.Delete", "Удаление записей");

            #region Поля
            this.Namespace<SpecialBuildContract>("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field", "Поля");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.DocumentName_Edit", "Название документа");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.DocumentNum_Edit", "Номер документа");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.DocumentDateFrom_Edit", "Дата документа");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.DocumentFile_Edit", "Файл документа");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.Sum_Edit", "Сумма договора подряда");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.DateStartWork_Edit", "Дата начала работ");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.DateEndWork_Edit", "Дата окончания работ");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.DateCancelReg_Edit", "Отклонено от регистрации");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.DateAcceptOnReg_Edit", "Принято на регистрацию");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.DateInGjiRegister_Edit", "Договор внесен в реестр ГЖИ");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.TypeContractBuild_Edit", "Тип договора");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.Inspector_Edit", "Инспектор");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.Builder_Edit", "Подрядчик");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.Description_Edit", "Описание");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.BudgetMo_Edit", "Бюджет МО");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.BudgetSubject_Edit", "Бюджет субъекта");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.OwnerMeans_Edit", "Средства собственников");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.FundMeans_Edit", "Средства фонда");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.TabResultQual_Edit", "Вкладка:«Результат квалификационного отбора»");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.ProtocolName_Edit", "Название документа квалификационного отбора");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.ProtocolNum_Edit", "Номер документа квалификационного отбора");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.ProtocolDateFrom_Edit", "Дата документа квалификационного отбора");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.BuildContract.Field.ProtocolFile_Edit", "Файл документа квалификационного отбора");

            #endregion Поля
            #endregion Договоры подряда

            #region Мониторинг СМР
            this.Namespace<SpecialTypeWorkCr>("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr", "Объекты КР для владельцев специальных счетов: Мониторинг СМР");

            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.View", "Просмотр");

            #region График выполнения работ
            this.Namespace<SpecialMonitoringSmr>("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ScheduleExecutionWork", "График выполнения работ");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ScheduleExecutionWork.View", "Чтение");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ScheduleExecutionWork.Edit", "Изменение");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ScheduleExecutionWork.AddDate", "Дополнительный срок");
            #endregion График выполнения работ

            #region Ход выполнения работ
            this.Namespace<SpecialMonitoringSmr>("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork", "Ход выполнения работ");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.CalcPercentOfCompletion", "Расчет процента выполнения");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.View", "Чтение");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Edit", "Изменение");

            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field", "Поля");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_View", "Этап работы - Просмотр");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field.StageWorkCr_Edit", "Этап работы - Изменение");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_View", "Производитель - Просмотр");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Field.Manufacturer_Edit", "Производитель - Изменение");
            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Column", "Колонки");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Column.StageWorkCr", "Этап работы");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.Column.Manufacturer", "Производитель");
            #endregion Ход выполнения работ

            #region Численность рабочих
            this.Namespace<SpecialMonitoringSmr>("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.WorkersCount", "Численность рабочих");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.WorkersCount.View", "Чтение");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.WorkersCount.Edit", "Изменение");
            #endregion Численность рабочих

            #region Документы
            this.Namespace<SpecialMonitoringSmr>("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.Document", "Документы");
            this.CRUDandViewPermissions("GkhCr.SpecialTypeWorkCr.Register.MonitoringSmr.Document");
            #endregion Документы

            #endregion Мониторинг СМР

            #region Акт выполненных работ
            this.Namespace<SpecialTypeWorkCr>("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkActViewCreate", "Акт выполненных работ: Просмотр, создание");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkActViewCreate.View", "Просмотр");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkActViewCreate.Create", "Создание записей");
            this.Namespace<SpecialPerformedWorkAct>("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct", "Акт выполненных работ: Изменение, удаление");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Edit", "Изменение записей");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Delete", "Удаление записей");

            this.Namespace<SpecialPerformedWorkAct>("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field", "Поля");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field.DocumentNum", "Номер");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field.Sum", "Сумма");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field.DateFrom", "Дата");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field.Description", "Описание");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field.Volume", "Объем");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field.CostFile", "Справка о стоимости выполненных работ и затрат");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field.DocumentFile", "Документ акта");
            this.Permission("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field.AdditionFile", "Приложение к акту");
            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field.Rec", "Записи акта");
            this.CRUDandViewPermissions("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field.Rec");
            this.Namespace("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field.Payment", "Оплата акта выполненных работ");
            this.CRUDandViewPermissions("GkhCr.SpecialTypeWorkCr.Register.PerformedWorkAct.Field.Payment");

            #endregion Акт выполненных работ

            #endregion Реестры

            #endregion Объекты КР (работы)

            #endregion SpecialObjectCr
        }
    }
}