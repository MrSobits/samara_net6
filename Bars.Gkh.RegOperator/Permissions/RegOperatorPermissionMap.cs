namespace Bars.Gkh.RegOperator.Permissions
{
    using B4;
    using B4.Application;
    using B4.IoC;
    using Entities;
    using Distribution;
    using Utils;

    /// <summary>
    /// Маппинг прав доступа RegOperator
    /// </summary>
    public class RegOperatorPermissionMap : PermissionMap
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RegOperatorPermissionMap()
        {
            this.Namespace("GkhRegOp", "Модуль РегОператор");

            this.Namespace("GkhRegOp.Settings", "Настройки");

            this.Namespace("GkhRegOp.Settings.PaymentSizeCr", "Размер взноса на КР");
            this.Permission("GkhRegOp.Settings.PaymentSizeCr.View", "Просмотр");

            this.Namespace("GkhRegOp.Settings.ChargePeriod", "Периоды начислений");
            this.Permission("GkhRegOp.Settings.ChargePeriod.View", "Просмотр");

            this.Namespace("GkhRegOp.Settings.PeriodChecks", "Проверки перед закрытием месяца");
            this.Permission("GkhRegOp.Settings.PeriodChecks.View", "Просмотр");
            this.Permission("GkhRegOp.Settings.PeriodChecks.Edit", "Изменение");

            this.Namespace("Import.ChargesToClosedPeriods", "Импорт начислений (в закрытые периоды)");
            this.Permission("Import.ChargesToClosedPeriods.View", "Просмотр");
            this.Namespace("Import.ChargesToClosedPeriods.Warnings", "Предупреждения");
            this.Permission("Import.ChargesToClosedPeriods.Warnings.View", "Просмотр");

            this.Namespace("Import.PaymentsToClosedPeriods", "Импорт оплат (в закрытые периоды)");
            this.Permission("Import.PaymentsToClosedPeriods.View", "Просмотр");
            this.Namespace("Import.PaymentsToClosedPeriods.Warnings", "Предупреждения");
            this.Permission("Import.PaymentsToClosedPeriods.Warnings.View", "Просмотр");

            this.Namespace("Import.RoomImport", "Импорт абонентов");
            this.Permission("Import.RoomImport.ReplaceExistRooms", "Заменять существующие сведения");
            this.Permission("Import.RoomImport.View", "Импорт абонентов - Просмотр");

            this.Namespace("Import.DebtorClaimWorkImport", "Импорт данных в ПИР");
            this.Permission("Import.DebtorClaimWorkImport.View", "Просмотр");

            this.Namespace("Import.PersonalAccountDigitalReceiptImport", "Импорт признака выдачи электронных квитанций");
            this.Permission("Import.PersonalAccountDigitalReceiptImport.View", "Просмотр");

            this.Namespace("GkhRegOp.Settings.BankDocumentImport", "Реестр оплат платежных агентов");
            this.Permission("GkhRegOp.Settings.BankDocumentImport.View", "Просмотр");

            this.Namespace("GkhRegOp.Settings.BankDocumentImport.Comparing", "Сопоставить ЛС");
            this.Permission("GkhRegOp.Settings.BankDocumentImport.Comparing.CompareAndAccept", "Сопоставить и подтвердить");

            this.Namespace("GkhRegOp.Settings.BankDocumentImport.Formats", "Допустимые Форматы");
            this.Permission("GkhRegOp.Settings.BankDocumentImport.Formats.Dbf", "Dbf");
            this.Permission("GkhRegOp.Settings.BankDocumentImport.Formats.Xml", "Xml");
            this.Permission("GkhRegOp.Settings.BankDocumentImport.Formats.Json", "Json");
            this.Permission("GkhRegOp.Settings.BankDocumentImport.Formats.Txt", "Txt");
            this.Permission("GkhRegOp.Settings.BankDocumentImport.Formats.Dbf2", "Dbf2");
            this.Permission("GkhRegOp.Settings.BankDocumentImport.Formats.DbfSpb", "DbfSpb");
            this.Permission("GkhRegOp.Settings.BankDocumentImport.Formats.Vtb24", "Vtb24xml");

            this.Namespace("GkhRegOp.Settings.PaymentDocInfo", "Информации для физических лиц");
            this.CRUDandViewPermissions("GkhRegOp.Settings.PaymentDocInfo");

            this.Namespace("GkhRegOp.Settings.FederalStandardFeeCr", "Федеральный стандарт взноса на КР");
            this.CRUDandViewPermissions("GkhRegOp.Settings.FederalStandardFeeCr");

            this.Namespace("GkhRegOp.Settings.ComparisonPayingAgents", "Сопоставление платежных агентов");
            this.Permission("GkhRegOp.Settings.ComparisonPayingAgents.View", "Просмотр");

            this.Namespace("GkhRegOp.Settings.BankAccountStatement", "Реестр банковских выписок");
            this.Permission("GkhRegOp.Settings.BankAccountStatement.View", "Просмотр");

            this.Namespace("GkhRegOp.Settings.SubsidyIncome", "Реестр субсидий");
            this.Permission("GkhRegOp.Settings.SubsidyIncome.View", "Просмотр");

            this.Namespace("GkhRegOp.Settings.RegopServiceLog", "Логи обращений к сервисам");
            this.Permission("GkhRegOp.Settings.RegopServiceLog.View", "Просмотр");

            this.Namespace("GkhRegOp.AccountingMonth", "Расчетный месяц");
            this.Namespace("GkhRegOp.AccountingMonth.PeriodChecking", "Проверка и закрытие месяца");
            this.Permission("GkhRegOp.AccountingMonth.PeriodChecking.View", "Просмотр");
            this.Permission("GkhRegOp.AccountingMonth.PeriodChecking.Check", "Выполнить проверки");
            this.Permission("GkhRegOp.AccountingMonth.PeriodChecking.Close", "Закрыть месяц");
            this.Permission("GkhRegOp.AccountingMonth.PeriodChecking.RollbackClosedPeriod", "Откатить закрытый период - Просмотр");

            this.Namespace("GkhRegOp.FormationRegionalFund", "Формирование регионального фонда");

            this.Namespace("GkhRegOp.FormationRegionalFund.RegOperator", "Региональные операторы");
            this.CRUDandViewPermissions("GkhRegOp.FormationRegionalFund.RegOperator");
            this.Namespace("GkhRegOp.FormationRegionalFund.RegOperator.Municipality", "Муниципальные образования");
            this.CRUDandViewPermissions("GkhRegOp.FormationRegionalFund.RegOperator.Municipality");
            this.Namespace("GkhRegOp.FormationRegionalFund.RegOperator.Accounts", "Счета");
            this.Permission("GkhRegOp.FormationRegionalFund.RegOperator.Accounts.View", "Просмотр");
            this.Namespace("GkhRegOp.FormationRegionalFund.RegOperator.AccountHistory", "История ведения расчетных счетов");
            this.Permission("GkhRegOp.FormationRegionalFund.RegOperator.AccountHistory.View", "Просмотр");

            this.Namespace("GkhRegOp.FormationRegionalFund.FundFormationContract", "Реестр договоров на формирование фонда капитального ремонта");
            this.Permission("GkhRegOp.FormationRegionalFund.FundFormationContract.View", "Просмотр");

            this.Namespace("GkhRegOp.RegionalFundUse", "Использование регионального фонда");

            this.Namespace("GkhRegOp.RegionalFundUse.Loan", "Реестр займов");
            this.Permission("GkhRegOp.RegionalFundUse.Loan.View", "Просмотр");
            this.Permission("GkhRegOp.RegionalFundUse.Loan.RepaymentAll", "Возврат займов");

            this.Namespace("GkhRegOp.Accounts", "Счета");
            this.Namespace("GkhRegOp.Accounts.BasePersonalAccount", "Реестр лицевых счетов");
            this.Permission("GkhRegOp.Accounts.BasePersonalAccount.View", "Просмотр");

            this.Namespace("GkhRegOp.Accounts.UnacceptedCharge", "Журнал расчета начислений");
            this.Permission("GkhRegOp.Accounts.UnacceptedCharge.View", "Просмотр");

            this.Namespace("GkhRegOp.Accounts.SberbankPaymentDoc", "Квитанции на оплату Сбербанку");
            this.Permission("GkhRegOp.Accounts.SberbankPaymentDoc.View", "Просмотр");

            this.Namespace("GkhRegOp.Accounts.StatusPaymentDocumentHouses", "Реестр домов с невыгруженными платежными документам в ГИС ЖКХ");
            this.Permission("GkhRegOp.Accounts.StatusPaymentDocumentHouses.View", "Просмотр");

            this.Namespace("GkhRegOp.Accounts.UnacceptedPayment", "Реестр неподтвержденных оплат");
            this.Permission("GkhRegOp.Accounts.UnacceptedPayment.View", "Просмотр");
            this.Permission("GkhRegOp.Accounts.UnacceptedPayment.Accept", "Подтвердить");

            this.Namespace("GkhRegOp.Accounts.PaymentDocumentSnapshot", "Реестр документов на оплату");
            this.Permission("GkhRegOp.Accounts.PaymentDocumentSnapshot.Delete", "Удаление записей");
            this.Permission("GkhRegOp.Accounts.PaymentDocumentSnapshot.View", "Просмотр");
            this.Permission("GkhRegOp.Accounts.PaymentDocumentSnapshot.SendEmail", "Отправить на эл.почту");

            this.Namespace("GkhRegOp.Accounts.RegistryAccount", "Реестр домов регионального оператора");
            this.Permission("GkhRegOp.Accounts.RegistryAccount.View", "Просмотр");

            this.Namespace("GkhRegOp.Accounts.ControlTransitAccount", "Контроль транзитного счета");
            this.Permission("GkhRegOp.Accounts.ControlTransitAccount.View", "Просмотр");

            this.Namespace("GkhRegOp.Accounts.PaymentCrSpecAccNotRegop", "Взносы на КР");
            this.Permission("GkhRegOp.Accounts.PaymentCrSpecAccNotRegop.View", "Просмотр");

            this.Namespace("GkhRegOp.Accounts.SuspenseAccount", "Счет невыясненных сумм");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.View", "Просмотр");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.Add", "Добавить");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.Enroll", "Зачислить");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.CancelDistribution", "Отменить распределение");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.CancelEnrollment", "Отменить зачисление");

            this.Namespace("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes", "Типы зачисления");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.FundSubsidy", "Субсидия фонда");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RefundFundSubsidy", "Возврат субсидии фонда");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RegionalSubsidy", "Региональная субсидия");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RefundRegionalSubsidy", "Возврат региональной субсидии");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.StimulatingSubsidy", "Стимулирующая субсидия");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RefundStimulatingSubsidy", "Возврат стимулирующей субсидии");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.TargetSubsidy", "Целевая субсидия");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RefundTargetSubsidy", "Возврат целевой субсидии");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.TransferCr", "Платеж КР");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.BudgetMu", "Бюджет МО");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.BudgetSubject", "Бюджет субъекта");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RentPaymentIn", "Поступление оплаты аренды");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.AccumulatedFunds", "Ранее накопленные средства");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.Refund", "Возврат взносов на КР");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RefundMsp", "Возврат МСП");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RefundPenaltyDistribution", "Возврат пени");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.OtherSources", "Иные поступления");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.BankPercent", "Поступление процентов банка");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.PreviousWorkPayments", "Средства за ранее выполненные работы");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.PerformedWorkAct", "Оплата акта выполненных работ");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.ComissionForAccountService", "Комиссия за ведение счета кредитной организацией");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.TransferContractor", "Распределение средств подрядчику");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.BuildControlPayment", "Оплата Стройконтроль");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.DEDPayment", "Оплата ПСД");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RestructAmicableAgreement", "Оплата задолженности по Мировому соглашению");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RefundBuilderDistribution", "Возврат от подрядчика");
            this.Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RefundTransferFundsDistribution", "Возврат перечисленных средств");
            /*Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.CreditRepayment", "Оплата кредита");
            Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.CreditPercentRepayment", "Оплата процентов по кредиту");
            Permission("GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.CreditGuarantee", "Расход на получение гарантий и поручительства по кредитам");*/

            this.Namespace("GkhRegOp.Accounts.SpecialAccount", "Реестр специальных счетов");
            this.Permission("GkhRegOp.Accounts.SpecialAccount.View", "Просмотр");

            this.Namespace("GkhRegOp.Accounts.CalculationProtocol", "Протоколы расчетов");
            this.Permission("GkhRegOp.Accounts.CalculationProtocol.View", "Просмотр");

            this.Namespace("GkhRegOp.Accounts.PaymentDocuments", "Документы на оплату за периоды");
            this.Permission("GkhRegOp.Accounts.PaymentDocuments.View", "Просмотр");

            this.Namespace("GkhRegOp.Accounts.SaldoRefresh", "Обновление сальдо");
            this.CRUDandViewPermissions("GkhRegOp.Accounts.SaldoRefresh");

            this.Namespace("GkhRegOp.Accounts.MobileAppAccountComparsion", "Реестр ЛС, не сопоставленных с мобильным приложенем");
            this.CRUDandViewPermissions("GkhRegOp.Accounts.MobileAppAccountComparsion");

            this.Namespace("GkhRegOp.Accounts.UnconfirmedPayments", "Неподтвержденные оплаты");
            this.CRUDandViewPermissions("GkhRegOp.Accounts.UnconfirmedPayments");

            this.Namespace("Tasks", "Задачи");
            this.Permission("Tasks.PaymentDocumentLogsView", "Просмотр прогресса документов на оплату");

            this.Namespace("Gkh.RealityObject.Register.Loan", "Займы");
            this.Permission("Gkh.RealityObject.Register.Loan.View", "Просмотр");

            this.Namespace("GkhRegOp.Loans", "Займы");
            this.Namespace("GkhRegOp.Loans.Manage", "Управление займами");
            this.Permission("GkhRegOp.Loans.Manage.View", "Просмотр");

            this.Namespace("GkhRegOp.Loans.Overdraft", "Овердрафт");
            this.CRUDandViewPermissions("GkhRegOp.Loans.Overdraft");

            this.Namespace("GkhRegOp.Loans.AccountCredit", "Реестр кредитов");
            this.Permission("GkhRegOp.Loans.AccountCredit.View", "Просмотр");
            this.Permission("GkhRegOp.Loans.AccountCredit.Create", "Создание записей");

            this.Namespace("GkhRegOp.Loans.Loan", "Реестр займов");
            this.Permission("GkhRegOp.Loans.Loan.View", "Просмотр");

            this.Namespace("Gkh.Orgs.DeliveryAgent", "Агент доставки");
            this.CRUDandViewPermissions("Gkh.Orgs.DeliveryAgent");

            this.Namespace("Gkh.Orgs.DeliveryAgent.GoToContragent", "Переход к контрагенту");
            this.Permission("Gkh.Orgs.DeliveryAgent.GoToContragent.View", "Просмотр");

            this.Namespace("Gkh.Orgs.DeliveryAgent.Municipality", "Муниципальные образования");
            this.Permission("Gkh.Orgs.DeliveryAgent.Municipality.View", "Просмотр");
            this.Permission("Gkh.Orgs.DeliveryAgent.Municipality.Create", "Добавить");
            this.Permission("Gkh.Orgs.DeliveryAgent.Municipality.Delete", "Удалить");

            this.Namespace("Gkh.Orgs.DeliveryAgent.RealityObject", "Жилые дома");
            this.Permission("Gkh.Orgs.DeliveryAgent.RealityObject.View", "Просмотр");
            this.Permission("Gkh.Orgs.DeliveryAgent.RealityObject.Create", "Добавить");
            this.Permission("Gkh.Orgs.DeliveryAgent.RealityObject.Delete", "Удалить");

            this.Namespace("Gkh.Orgs.CashPaymentCenter", "Расчетно-кассовые центры");
            this.CRUDandViewPermissions("Gkh.Orgs.CashPaymentCenter");
            
            this.Namespace("Gkh.Orgs.CashPaymentCenter.GoToContragent", "Переход к контрагенту");
            this.Permission("Gkh.Orgs.CashPaymentCenter.GoToContragent.View", "Просмотр");

            this.Namespace("Gkh.Orgs.CashPaymentCenter.Municipality", "Муниципальные образования");
            this.CRUDandViewPermissions("Gkh.Orgs.CashPaymentCenter.Municipality");

            this.Namespace("Gkh.Orgs.CashPaymentCenter.RealityObject", "Жилые дома");
            this.CRUDandViewPermissions("Gkh.Orgs.CashPaymentCenter.RealityObject");

            this.Namespace("Gkh.Orgs.CashPaymentCenter.ManOrg", "Обслуживаемые УК");
            this.CRUDandViewPermissions("Gkh.Orgs.CashPaymentCenter.ManOrg");

            this.Namespace("GkhRegOp.Processes", "Процессы");

            this.Namespace("GkhRegOp.Processes.ComputingProcess", "Мои процессы");
            this.Permission("GkhRegOp.Processes.ComputingProcess.View", "Просмотр");

            this.Namespace("Gkh.RealityObject.Register.Accounts", "Счета");
            this.Permission("Gkh.RealityObject.Register.Accounts.View", "Просмотр");


            #region Счет начислений
            this.Namespace("Gkh.RealityObject.Register.Accounts.RealtyChargeAccount", "Счет начислений");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtyChargeAccount.View", "Просмотр");

            #region Поля
            this.Namespace("Gkh.RealityObject.Register.Accounts.RealtyChargeAccount.Field", "Поля");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtyChargeAccount.Field.AccountNum_View", "Номер счета - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtyChargeAccount.Field.DateOpen_View", "Дата открытия счета - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtyChargeAccount.Field.DateClose_View", "Дата закрытия счета - Просмотр");
            #endregion
            #endregion

            #region Счет оплат
            this.Namespace("Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount", "Счет оплат");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.View", "Просмотр");

            #region Поля
            this.Namespace("Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.Field", "Поля");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.Field.AccountNum_View", "Номер счета - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.Field.DateOpen_View", "Дата открытия счета - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.Field.DateClose_View", "Дата закрытия счета - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.Field.BankAccountNum_View", "№ р/с - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.Field.Limit_View", "Лимит по - Просмотр");
            #endregion
            #endregion

            #region Счет расчета с поставщиками
            this.Namespace("Gkh.RealityObject.Register.Accounts.RealtySupplierAccount", "Счет расчета с поставщиками");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtySupplierAccount.View", "Просмотр");

            #region Поля
            this.Namespace("Gkh.RealityObject.Register.Accounts.RealtySupplierAccount.Field", "Поля");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtySupplierAccount.Field.AccountNum_View", "Номер счета - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtySupplierAccount.Field.DateOpen_View", "Дата открытия счета - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtySupplierAccount.Field.DateClose_View", "Дата закрытия счета - Просмотр");
            this.Permission("Gkh.RealityObject.Register.Accounts.RealtySupplierAccount.Field.BankAccountNum_View", "№ р/с - Просмотр");
            #endregion
            #endregion

            #region Счет субсидий
            this.Namespace("Gkh.RealityObject.Register.Accounts.SubsidyAccount", "Счет субсидий");
            this.Permission("Gkh.RealityObject.Register.Accounts.SubsidyAccount.View", "Просмотр");
            #endregion

            this.Namespace("Gkh.RealityObject.Register.DeliveryAgent", "Агенты доставки");
            this.Permission("Gkh.RealityObject.Register.DeliveryAgent.View", "Просмотр");

            this.RegPersAcc();

            #region Абоненты
            this.Namespace("GkhRegOp.PersonalAccountOwner", "Абоненты");
            this.Namespace("GkhRegOp.PersonalAccountOwner.PersonalAccountOwner", "Реестр абонентов");
            this.Permission("GkhRegOp.PersonalAccountOwner.PersonalAccountOwner.View", "Просмотр");

            this.Namespace("GkhRegOp.PersonalAccountOwner.Debtor", "Реестр должников");
            this.Permission("GkhRegOp.PersonalAccountOwner.Debtor.View", "Просмотр");

            this.Namespace("GkhRegOp.PersonalAccountOwner.Benefits", "Информация по начисленным льготам");
            this.Permission("GkhRegOp.PersonalAccountOwner.Benefits.View", "Просмотр");

            this.Namespace("GkhRegOp.PersonalAccountOwner.EmailNewsletter", "Рассылка на электронную почту");
            this.Permission("GkhRegOp.PersonalAccountOwner.EmailNewsletter.View", "Просмотр");

            #region Абоненты - поля
            this.Namespace("GkhRegOp.PersonalAccountOwner.Field", "Поля");

            this.Permission("GkhRegOp.PersonalAccountOwner.Field.PrivilegedCategory_View", "Категория льгот - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.PrivilegedCategory_Edit", "Категория льгот - изменение");

            this.Namespace("GkhRegOp.PersonalAccountOwner.Field.Individ", "Физическое лицо");

            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.Surname_View", "Фамилия - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.Surname_Edit", "Фамилия - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.FirstName_View", "Имя - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.FirstName_Edit", "Имя - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.SecondName_View", "Отчество - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.SecondName_Edit", "Отчество - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.BirthDate_View", "Дата рождения - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.BirthDate_Edit", "Дата рождения - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.BirthPlace_View", "Место рождения - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.BirthPlace_Edit", "Место рождения - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.IdentityType_View", "Тип документа - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.IdentityType_Edit", "Тип документа - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.IdentitySerial_View", "Серия документа - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.IdentitySerial_Edit", "Серия документа - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.IdentityNumber_View", "Номер документа - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.IdentityNumber_Edit", "Номер документа - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.BillingAddress_View", "Адрес для отправки корреспонденции - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.BillingAddress_Edit", "Адрес для отправки корреспонденции - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.Gender_View", "Пол - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.Gender_Edit", "Пол - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.DateDocumentIssuance_View", "Дата выдачи документа - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.DateDocumentIssuance_Edit", "Дата выдачи документа - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.ActivityStage_View", "Финансовоее состояние - Просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Individ.ActivityStage_Edit", "Финансовоее состояние - Изменение");

            this.Namespace("GkhRegOp.PersonalAccountOwner.Field.Legal", "Юридическое лицо");

            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Legal.Contragent_View", "Контрагент - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Legal.Contragent_Edit", "Контрагент - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Legal.Inn_View", "ИНН - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Legal.Inn_Edit", "ИНН - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Legal.Kpp_View", "КПП - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Legal.Kpp_Edit", "КПП - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Legal.PrintAct_View", "Печатать акт при печати документов на оплату - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Legal.PrintAct_Edit", "Печатать акт при печати документов на оплату - изменение");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Legal.Address_View", "Адрес для корреспонденции - просмотр");
            this.Permission("GkhRegOp.PersonalAccountOwner.Field.Legal.Address_Edit", "Адрес для корреспонденции - изменение");

            #endregion Абоненты - поля

            #region Абоненты - Сведения о помещении

            this.Namespace("GkhRegOp.PersonalAccountOwner.Account", "Абоненты - Сведения о помещении");
            this.Permission("GkhRegOp.PersonalAccountOwner.Account.PersonalAccount_Edit", "Редактирование");

            #endregion Абоненты - Сведения о помещении

            #endregion Абоненты


            #region Отчеты
            this.Namespace("Reports.GkhRegOp", "Модуль региональный оператор");
            this.Permission("Reports.GkhRegOp.OwnersRoNotInLongTermPr", "Собственники МКД, отсутствующих в ДПКР");
            this.Permission("Reports.GkhRegOp.OwnersRoInLongTermPr", "Наличие собственников МКД по ДПКР");
            this.Permission("Reports.GkhRegOp.CheckMkdAndOwnersPremises", "Проверка по МКД и собственникам помещений");
            this.Permission("Reports.GkhRegOp.TurnoverBalance", "Оборотно-сальдовая ведомость");
            this.Permission("Reports.GkhRegOp.ChargeReport", "Отчет по начислениям");
            this.Permission("Reports.GkhRegOp.PaymentAndBalanceReport", "Сумма начислений и остаток средств");
            this.Permission("Reports.GkhRegOp.RepairPaymentAmountReport", "Размер средств, направленныx на КР");
            this.Permission("Reports.GkhRegOp.AccountOperationsReport", "Операции по счету");
            this.Permission("Reports.GkhRegOp.MkdRoomAbonentReport", "Отчет по адресам МКД и помещениям для импорта абонентов");
            this.Permission("Reports.GkhRegOp.TransferFundReport", "Отчет по перечислениям в фонд");
            this.Permission("Reports.GkhRegOp.MkdChargePaymentReport", "Сведения о поступлении взносов на капитальный ремонт от собственников МКД");
            this.Permission("Reports.GkhRegOp.MkdLoanReport", "Отчет по займам в разрезе МКД");
            this.Permission("Reports.GkhRegOp.AccrualAccountStateReport", "Отчет о состоянии счета начислений");
            this.Permission("Reports.GkhRegOp.NotificationFormFundReport", "Реестр уведомлений о выбранном способе формирования фонда КР");
            this.Permission("Reports.GkhRegOp.RepairContributionInfoReport", "Сведения о поступлениях и остатках взносов на капитальный ремонт для гос. инспекции");
            this.Permission("Reports.GkhRegOp.RepairPaymentByOwnerReport", "Размер начисленных и уплаченных взносов на кр каждым собственником");
            this.Permission("Reports.GkhRegOp.RequestsGisuReport", "Отчет по заявкам в ГИСУ");
            this.Permission("Reports.GkhRegOp.SocialSupportReport", "Отчет поступлений соц. поддержки");

            this.Permission("Reports.GkhRegOp.PersonalAccountChargeReport", "Отчет о начислениях");
            this.Permission("Reports.GkhRegOp.OwnerChargeReport", "Отчет о начислениях в разрезе собственников");
            this.Permission("Reports.GkhRegOp.MunicipalityChargeReport", "Отчет о начислениях в разрезе МО");
            this.Permission("Reports.GkhRegOp.RaionChargeReport", "Отчет о начислениях в разрезе МР");
            this.Permission("Reports.GkhRegOp.RegionChargeReport", "Отчет о начислениях в разрезе Края");

            this.Permission("Reports.GkhRegOp.CollectionPercentReport", "Отчет определения процента собираемости");
            this.Permission("Reports.GkhRegOp.RealtiesOutOfOverhaul", "Дома, не включенные в ДПКР");
            this.Permission("Reports.GkhRegOp.RealtiesOutOfDpkr", "Дома, не включенные в ДПКР (%)");
            this.Permission("Reports.GkhRegOp.BalanceReport", "Отчет об остатках");
            this.Permission("Reports.GkhRegOp.CalendarCostPlaning", "Календарь планирования расходов");
            this.Permission("Reports.GkhRegOp.OwnerAndGovernmentDecisionReport", "Отчет о решениях собственников и ОГВ");
            this.Permission("Reports.GkhRegOp.RoomAndAccOwnersReport", "Сводный отчет о помещениях и абонентах");

            this.Permission("Reports.DecisionsNso.FundFormationStimul", "Отчет по разделу уведомлений о способе формирования фонда (на Стимуле)");

            #endregion Отчеты

            #region Условия обслуживания кредитными организациями
            this.Namespace("Gkh.Orgs.CreditorgServiceCondition", "Условия обслуживания");
            this.CRUDandViewPermissions("Gkh.Orgs.CreditorgServiceCondition");
            #endregion

            this.Namespace("GkhRf.TransferFunds", "Перечисления средств в фонд");
            this.Permission("GkhRf.TransferFunds.View", "Просмотр");

            this.Permission("Import.BankDocument", "Импорт реестров оплат");
            this.Permission("Import.OwnersImport", "Импорт собственников");
            this.Namespace("Import.RoomImport", "Импорт абонентов");
            this.Permission("Import.RoomImport.ReplaceExistRooms", "Заменять существующие сведения");
            this.Permission("Import.RoomImport.View", "Импорт абонентов - Просмотр");
            this.Permission("Import.OwnerRoomImport", "Импорт абонентов (замена данных)");
            this.Permission("Import.PersonalAccountImport", "Импорт лицевых счетов");
            this.Permission("Import.SocialSupportImport", "Импорт социальной поддержки");
            this.Permission("Import.PersonalAccountClosedImport", "Импорт лицевых счетов (в закрытые периоды)");
            this.Permission("Import.RkcImport", "Импорт ЛС из XML");
            this.Permission("Import.DecisionProtocolImport", "Импорт протоколов решений");
            this.Permission("Import.OwnerDecisionProtocolImport", "Импорт протоколов решений собственников");
            this.Permission("Import.BenefitsCategoryImport", "Импорт сведений по льготным категориям граждан");
            this.Permission("Import.BenefitsCategoryImportVersion2", "Импорт сведений по льготным категориям граждан 2");

            this.Permission("Import.BankAccountStatementImport", "Импорт банковских выписок");
            this.Permission("Import.PersonalAccountChargePaymentImportXml", "Импорт начислений и оплат (оффлайн версия)");
            this.Permission("Import.SubsidyIncome", "Импорт реестра субсидий");
            this.Permission("Import.BenefitsCategoryPersAccSum", "Импорт начисленных льгот");

            #region Реестр настроек импорта оплат (txt,csv)
            this.Namespace("Import.FsGorodImportInfoSettings", "Реестр настроек импорта оплат (txt,csv)");
            this.Permission("Import.FsGorodImportInfoSettings.View", "Просмотр");
            this.Permission("Import.FsGorodImportInfoSettings.Import", "Импорт элемента");
            this.Permission("Import.FsGorodImportInfoSettings.Export", "Экспорт элемента");
            #endregion

            this.Namespace("Import.ChesImport", "Импорт сведений от билинга");
            this.Permission("Import.ChesImport.View", "Просмотр");

            this.Namespace("GkhRegOp.Dictionaries", "Справочники");
            this.Permission("GkhRegOp.Dictionaries.RegopDictLocationCode", "Справочник кодов населенных пунктов");
            this.Namespace("GkhRegOp.Dictionaries.PrivilegedCategory", "Группы льготных категорий граждан");
            this.Permission("GkhRegOp.Dictionaries.PrivilegedCategory.View", "Просмотр");
            this.Permission("GkhRegOp.Dictionaries.PrivilegedCategory.Create", "Добавить");
            this.Permission("GkhRegOp.Dictionaries.PrivilegedCategory.Edit", "Редактировать");
            this.Permission("GkhRegOp.Dictionaries.PrivilegedCategory.Delete", "Удалить");

            this.Namespace("GkhRegOp.Dictionaries.TariffByPeriodForClaimWork", "Тарифы для эталонных начислений");
            this.CRUDandViewPermissions("GkhRegOp.Dictionaries.TariffByPeriodForClaimWork");



            this.Namespace("GkhRegOp.Dictionaries.ProgramCrType", "Типы программы КР");
            this.CRUDandViewPermissions("GkhRegOp.Dictionaries.ProgramCrType");

            this.Permission("GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Payment.Export", "Выгрузка 1С");

            this.Namespace<TransferCtr>("GkhRf.TransferCtr", "Реестр заявок на перечисление денежных средств подрядчикам");
            this.Permission("GkhRf.TransferCtr.View", "Просмотр");
            this.Permission("GkhRf.TransferCtr.Create", "Создание записей");
            this.Permission("GkhRf.TransferCtr.Edit", "Изменение записей");
            this.Permission("GkhRf.TransferCtr.Delete", "Удаление записей");
            this.Permission("GkhRf.TransferCtr.ExportToTxt", "Сформировать документ");

            this.Namespace("GkhRf.TransferCtr.Field", "Поля");
            this.Permission("GkhRf.TransferCtr.Field.DocumentNum_View", "Номер заявки - Просмотр");
            this.Permission("GkhRf.TransferCtr.Field.DocumentNum_Edit", "Номер заявки - Редактирование");
            this.Permission("GkhRf.TransferCtr.Field.DateFrom_Edit", "Дата заявки");
            this.Permission("GkhRf.TransferCtr.Field.TypeProgramRequest_Edit", "Тип программы");
            this.Permission("GkhRf.TransferCtr.Field.ProgramCr_Edit", "Программа капремонта");
            this.Permission("GkhRf.TransferCtr.Field.ObjectCr_Edit", "Объект КР");
            this.Permission("GkhRf.TransferCtr.Field.TypeWorkCr_Edit", "Вид работы - Редактирование");
            this.Permission("GkhRf.TransferCtr.Field.TypeWorkCr_View", "Вид работы - Просмотр");

            this.Permission("GkhRf.TransferCtr.Field.Builder_Edit", "Подрядная организация");
            this.Permission("GkhRf.TransferCtr.Field.ContragentBank_Edit", "Наименование банка");
            this.Permission("GkhRf.TransferCtr.Field.Contract_Edit", "Договор подряда");

            this.Permission("GkhRf.TransferCtr.Field.RegOperator_Edit", "Региональный оператор - Редактирование");
            this.Permission("GkhRf.TransferCtr.Field.RegOperator_View", "Региональный оператор - Просмотр");
            this.Permission("GkhRf.TransferCtr.Field.RegopCalcAccount_Edit", "Счет - Редактирование");
            this.Permission("GkhRf.TransferCtr.Field.RegopCalcAccount_View", "Счет - Просмотр");

            this.Permission("GkhRf.TransferCtr.Field.Perfomer_Edit", "Исполнитель");
            this.Permission("GkhRf.TransferCtr.Field.File_Edit", "Файл");
            this.Permission("GkhRf.TransferCtr.Field.Comment_Edit", "Комментарий - Редактирование");
            this.Permission("GkhRf.TransferCtr.Field.Comment_View", "Комментарий - Просмотр");

            this.Permission("GkhRf.TransferCtr.Field.PaymentType_Edit", "Тип платежа - Редактирование");
            this.Permission("GkhRf.TransferCtr.Field.PaymentType_View", "Тип платежа - Просмотр");
            this.Permission("GkhRf.TransferCtr.Field.KindPayment_Edit", "Вид платежа - Редактирование");
            this.Permission("GkhRf.TransferCtr.Field.KindPayment_View", "Вид платежа - Просмотр");
            this.Permission("GkhRf.TransferCtr.Field.PaymentPurposeDescription_Edit", "Назначение платежа - Редактирование");
            this.Permission("GkhRf.TransferCtr.Field.PaymentPurposeDescription_View", "Назначение платежа - Просмотр");

            this.Permission("GkhRf.TransferCtr.Field.PaidSum_View", "Сумма оплаты - Просмотр");
            this.Permission("GkhRf.TransferCtr.Field.PaymentDate_View", "Дата оплаты - Просмотр");

            this.Namespace("GkhRf.TransferCtr.Column", "Столбцы");
            this.Permission("GkhRf.TransferCtr.Column.IsExport", "Сформирован документ");
            this.Permission("GkhRf.TransferCtr.Column.PaidSum", "Оплачено");
            this.Permission("GkhRf.TransferCtr.Column.CalcAccNumber", "Счет плательщика");

            this.Namespace("GkhRf.TransferCtr.Field.PayDetail", "Источник финансирования");
            this.Permission("GkhRf.TransferCtr.Field.PayDetail.Amount_View", "Оплата - Просмотр");
            this.Permission("GkhRf.TransferCtr.Field.PayDetail.Amount_Edit", "Оплата - Редактирование");

            #region Банковские операции
            this.Namespace("GkhRegOp.Accounts.BankOperations", "Банковские операции");
            this.Permission("GkhRegOp.Accounts.BankOperations.View", "Просмотр");
            this.Permission("GkhRegOp.Accounts.BankOperations.SetDistributable", "Указание возможности распределения");

            this.Namespace("GkhRegOp.Accounts.BankOperations.Field", "Поля");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.ProgramCr_View", "Программа КР - Просмотр");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.RealityObjectOriginator_View", "Адрес - Просмотр");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.SelectContragent_View", "Выбрать из контрагентов - Просмотр");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.SelectPayerAccountNum_View", "Выбрать р/с плательщика - Просмотр");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.SelectRecipientContragent_View", "Выбрать из контрагентов (Расход) - Просмотр");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.SelectRecipientAccountNumOutcome_View", "Выбрать р/с получателя (Расход) - Просмотр");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.SelectRecipientAccountNumIncome_View", "Выбрать р/с получателя (Приход) - Просмотр");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.SelectPayerAccountNumOutcome_View", "Выбрать р/с плательщика (Расход) - Просмотр");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.SelectPayerAccountNumIncome_View", "Выбрать р/с плательщика (Приход) - Просмотр");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.InternalCancel", "Частичная отмена распределения");

            this.Permission("GkhRegOp.Accounts.BankOperations.Field.PayerName_Edit", "Наименование контрагента (Плательщик) - Редактирование");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.PayerAccountNum_Edit", "Р/С (Плательщик) - Редактирование");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.PayerInn_Edit", "ИНН (Плательщик) - Редактирование");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.PayerBik_Edit", "БИК (Плательщик) - Редактирование");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.PayerKpp_Edit", "КПП (Плательщик) - Редактирование");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.PayerCorrAccount_Edit", "Корр.счет (Плательщик) - Редактирование");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.PayerBank_Edit", "Банк (Плательщик) - Редактирование");

            this.Permission("GkhRegOp.Accounts.BankOperations.Field.RecipientName_Edit", "Наименование контрагента (Получатель) - Редактирование");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.RecipientAccountNum_Edit", "Р/С (Получатель) - Редактирование");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.RecipientInn_Edit", "ИНН (Получатель) - Редактирование");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.RecipientBik_Edit", "БИК (Получатель) - Редактирование");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.RecipientKpp_Edit", "КПП (Получатель) - Редактирование");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.RecipientCorr_Edit", "Корр.счет (Получатель) - Редактирование");
            this.Permission("GkhRegOp.Accounts.BankOperations.Field.RecipientBank_Edit", "Банк (Получатель) - Редактирование");

            this.Namespace("GkhRegOp.Accounts.BankOperations.Distributions", "Выбор распределения");
            this.GenerateDistributionPermissions("GkhRegOp.Accounts.BankOperations.Distributions");

            #endregion

            this.Namespace("GkhRegOp.Accounts.BankDocumentImport", "Реестр оплат платежных агентов");
            this.Permission("GkhRegOp.Accounts.BankDocumentImport.Accept", "Подтвердить");
            this.Namespace("GkhRegOp.Accounts.BankDocumentImport.Field", "Поля");
            this.Permission("GkhRegOp.Accounts.BankDocumentImport.Field.PersonalAccountNumber", "Номер ЛС");
            this.Permission("GkhRegOp.Accounts.BankDocumentImport.Field.PaymentDate", "Дата оплаты");
            this.Permission("GkhRegOp.Accounts.BankDocumentImport.Field.InternalCancel", "Частичная отмена подтверждения");
        }

        private void RegPersAcc()
        {
            this.Namespace("GkhRegOp.PersonalAccount", "Лицевые счета");

            this.Namespace("GkhRegOp.PersonalAccount.Tab", "Вкладки");
            this.Permission("GkhRegOp.PersonalAccount.Tab.EntityLogLightGrid_View", "История изменений - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Tab.OwnerInformation_View", "Документы о собственности - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Tab.BanRecalc_View", "Запрет перерасчета - Просмотр");

            this.Namespace("GkhRegOp.PersonalAccount.Tab.PrivilegedCategory", "Категория льготы");
            this.CRUDandViewPermissions("GkhRegOp.PersonalAccount.Tab.PrivilegedCategory");
            this.Permission("GkhRegOp.PersonalAccount.Tab.PaymentsInfo_View", "Оплаты - Просмотр");

            this.Namespace("GkhRegOp.PersonalAccount.Tab.PaymentsInfo", "Оплаты");
            this.Permission("GkhRegOp.PersonalAccount.Tab.PaymentsInfo.UserLogin_View", "Пользователь - Просмотр");

            this.Namespace("GkhRegOp.PersonalAccount.Tab.Groups", "Группы");
            this.Permission("GkhRegOp.PersonalAccount.Tab.Groups.Create", "Создание записей");
            this.Permission("GkhRegOp.PersonalAccount.Tab.Groups.Delete", "Удаление записей");
            this.Permission("GkhRegOp.PersonalAccount.Tab.Groups.View", "Просмотр записей");

            this.Namespace("GkhRegOp.PersonalAccount.Field", "Поля");
            this.Permission("GkhRegOp.PersonalAccount.Field.ReportPersonalAccount_View", "Отчет по ЛС - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.PaymentDoc_View", "Платежный документ - Просмотр");

            this.Permission("GkhRegOp.PersonalAccount.Field.Fio_View", "ФИО/Наименование абонента - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.ChargedTariff_View", "Начислено взносов по минимальному тарифу - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.PaymentTariff_View", "Уплачено взносов по минимальному тарифу всего - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.PaymentDebt_View", "Задолженность по взносам всего - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.ChargedOwnerDecision_View", "Начислено взносов по тарифу решения всего - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.PaymentOwnerDecision_View", "Уплачено взносов по тарифу решения всего - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.PenaltyDebt_View", "Задолженность пени всего - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.ChargedPenalty_View", "Начислено пени всего - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.PaymentPenalty_View", "Уплачено пени всего - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.TotalDebt_View", "Итого задолженность - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.TotalCharge_View", "Итого начислено - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.PaymentTotal_View", "Итого уплачено - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.ContributionsInArrearsTariff_View", "Задолженность по взносам тарифа решения, всего: - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.AreaShare_View", "Доля собственности - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.PersAccNumExternalSystems_View", "Изменение внешнего номера ЛС - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.ChangeDate_View", "Изменение даты открытия и закрытия - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.ServiceType_View", "Тип услуги - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.ServiceType_Edit", "Тип услуги - Изменение");
            this.Permission("GkhRegOp.PersonalAccount.Field.CashPayCenter_View", "Расчетно-кассовый центр - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.Restructuring_View", "Реструктуризация - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.Amicable_Agreement_View", "Мировое соглашение - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.Pir_View", "ПИР - Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Field.PerfWorkChargeBalance_View", "Зачет средств за ранее выполненные работы - Просмотр");

            this.Namespace("GkhRegOp.PersonalAccount.Import", "Импорт");

            this.Permission("GkhRegOp.PersonalAccount.Import.PersonalAccountPaymentImport", "Импорт начислений dbf");
            this.Permission("GkhRegOp.PersonalAccount.Import.PersonalAccountPaymentImportDbf2", "Импорт начислений dbf 2");
            this.Permission("GkhRegOp.PersonalAccount.Import.PersonalAccountChargeImportCsv", "Импорт начислений csv");
            this.Permission("GkhRegOp.PersonalAccount.Import.PersonalSaldoAccountImport", "Импорт сальдо");

            this.Namespace("GkhRegOp.Settings.PersAccGroup", "Группы лицевых счетов");
            this.CRUDandViewPermissions("GkhRegOp.Settings.PersAccGroup");

            this.Namespace("GkhRegOp.Settings.PayDocTemplate", "Шаблоны квитанций по периодам");
            this.CRUDandViewPermissions("GkhRegOp.Settings.PayDocTemplate");

            #region Лицевые счета - реестр
            this.Namespace("GkhRegOp.PersonalAccount.Registry", "Реестр");

            this.Permission("GkhRegOp.PersonalAccount.Registry.UpdateCache_View", "Обновить реестр");
            this.Permission("GkhRegOp.PersonalAccount.Registry.ChargePeriod_View", "Период");
            this.Permission("GkhRegOp.PersonalAccount.Registry.ChargePeriodAdvanced_View", "Расширенный просмотр периодов");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action_View", "Просмотр действий");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Protocol_View", "Протокол");
            this.Permission("GkhRegOp.PersonalAccount.Registry.ExportCalculation_View", "Выгрузка начислений");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Saldo_Change", "Изменение Сальдо");

            this.Permission("GkhRegOp.PersonalAccount.Registry.CurrTariffDebt_View", "Текущая задолженность");
            this.Permission("GkhRegOp.PersonalAccount.Registry.OverdueTariffDebt_View", "Просроченная задолженность");

            this.Permission("GkhRegOp.PersonalAccount.Registry.SaldoInFromServ_View", "Входящее cальдо из файла");
            this.Permission("GkhRegOp.PersonalAccount.Registry.SaldoChangeFromServ_View", "Изменение cальдо из файла");
            this.Permission("GkhRegOp.PersonalAccount.Registry.SaldoOutFromServ_View", "Исходящее cальдо из файла");
            this.Permission("GkhRegOp.PersonalAccount.Registry.PerformedWorkCharged_View", "Зачет средств за работы");

            this.Namespace("GkhRegOp.PersonalAccount.Registry.Action", "Действия");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.Charge", "Расчет");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.Others", "Другие операции");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.PaymentDoc", "Документы на оплату");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.PartiallyPaymentDoc", "Документы на оплату (по частичному реестру)");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.ZeroPaymentDocs", "Выгрузка документов");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.ExportToVtscp", "Выгрузка информации для ВЦКП");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.ExportPenalty", "Выгрузка начислений пени");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.DeleteAccount", "Удаление личных счетов");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.Merge", "Слияние");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.ManuallyRecalc", "Ручной перерасчет");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.PerformedWorkFundsDistribution", "Зачет средств за ранее выполненные работы");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.BanRecalcOperation", "Запрет перерасчета");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.TurnOffLock", "Снять блокировку расчета");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.RepaymentOperation", "Перераспределение оплаты");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.PersonalAccountSplitOperation", "Разделение лицевого счета");
            this.Namespace("GkhRegOp.PersonalAccount.Registry.Action.ReOpenAccountOperation", "Повторное открытие");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.ReOpenAccountOperation.View", "Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.ReOpenAccountOperation.OpenBeforeClose_View", "Открыть ранее даты закрытия - Просмотр");

            this.Namespace("GkhRegOp.PersonalAccount.Registry.Action.MassPersAccGroupOperation", "Включению/исключению в/из группы лицевых счетов");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.MassPersAccGroupOperation.View", "Просмотр");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.MassPersAccGroupOperation.Create", "Создание записей – добавление списка лицевых счетов в группы");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.MassPersAccGroupOperation.Delete", "Удаление записей – исключение списка лицевых счетов из групп");

            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.CorrectPaymentsOperation", "Корректировка оплат");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.MassSaldoChangeOperation", "Установка и изменение сальдо");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.ExportSaldo", "Выгрузка сальдо");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Action.CalcDebtOperation", "Расчет долга");

            this.Namespace("GkhRegOp.PersonalAccount.Registry.Mode", "Режимы работы");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Mode.View", "Просмотр");

            this.Namespace("GkhRegOp.PersonalAccount.Registry.Field", "Поля");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Field.AccountOwner_View", "ФИО/Наименование");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Field.OwnerType_View", "Тип абонента");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Field.HasCharges_View", "Наличие начислений за текущий месяц");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Field.PersAccNumExternalSystems_View", "Номер ЛС во внешних системах");

            this.Permission("GkhRegOp.PersonalAccount.Registry.Field.PrivilegedCategoryPercent_View", "Процент льготы");

            this.Permission("GkhRegOp.PersonalAccount.Registry.Field.PrivilegedCategory_View", "Льготная категория");
            this.Permission("GkhRegOp.PersonalAccount.Registry.Field.CashPaymentCenter_View", "Расчетно-кассовый центр");

            #endregion Лицевые счета - реестр
            #region Модуль ЖКХ/Жилые дома/Поля

            this.Permission("Gkh.RealityObject.Field.FillPercentDebt", "Заполнить собираемость платежей - Просмотр");
            this.Permission("Gkh.RealityObject.Field.FillAreaLivingOwned", "Заполнить площадь, находящихся в собственности граждан - Просмотр");
            #endregion
        }

        private void GenerateDistributionPermissions(string ns)
        {
            var container = ApplicationContext.Current.Container;

            var distributions = container.ResolveAll<IDistribution>();
            using (container.Using(distributions))
            {
                foreach (var distribution in distributions)
                {
                    this.Permission(distribution.GetPermissionId(ns), distribution.Name);
                }
            }
        }
    }
}