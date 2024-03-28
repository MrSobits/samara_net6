namespace Bars.Gkh.RegOperator
{
    using Bars.B4;

    public class NavigationProvider: INavigationProvider
    {
        public string Key
        {
            get { return MainNavigationInfo.MenuName; }
        }

        public string Description
        {
            get { return MainNavigationInfo.MenuDescription; }
        }

        public void Init(MenuItem root)
        {
            root.Add("Администрирование").Add("Импорты").Add("Реестр настроек импорта оплат (txt,csv)", "fstownimportsettings").AddRequiredPermission("Import.FsGorodImportInfoSettings.View");
            root.Add("Администрирование").Add("Импорты").Add("Импорт сведений от биллинга", "chesimport").AddRequiredPermission("Import.ChesImport.View");

            root.Add("Задачи").Add("Список задач").Add("Прогресс документов на оплату", "paymentdocumentlogs").AddRequiredPermission("Tasks.PaymentDocumentLogsView");

            root.Add("Участники процесса").Add("Роли контрагента").Add("Агенты доставки", "deliveryagent").WithIcon("deliveryAgent").AddRequiredPermission("Gkh.Orgs.DeliveryAgent.View");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Расчетно-кассовые центры", "cashpaymentcenter").AddRequiredPermission("Gkh.Orgs.CashPaymentCenter.View");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Региональные операторы", "regoperator").AddRequiredPermission("GkhRegOp.FormationRegionalFund.RegOperator.View");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Условия обслуживания кредитными организациями", "creditorgservicecondition").AddRequiredPermission("Gkh.Orgs.CreditorgServiceCondition.View");

            var rfAccounts = root.Add("Региональный фонд").Add("Счета");

            rfAccounts.Add("Реестр лицевых счетов", "regop_personal_account").AddRequiredPermission("GkhRegOp.Accounts.BasePersonalAccount.View");
            rfAccounts.Add("Журнал расчета начислений", "unaccepted_charges").AddRequiredPermission("GkhRegOp.Accounts.UnacceptedCharge.View");
            //rfAccounts.Add("Реестр неподтвержденных оплат", "unaccepted_payment").AddRequiredPermission("GkhRegOp.Accounts.UnacceptedPayment.View");
            rfAccounts.Add("Реестр домов регионального оператора", "regop_registry_account").AddRequiredPermission("GkhRegOp.Accounts.RegistryAccount.View");
            //rfAccounts.Add("Обновленный счет НВС", "regop_suspense_account").AddRequiredPermission("GkhRegOp.Accounts.SuspenseAccount.View");
            rfAccounts.Add("Реестр специальных счетов", "regop_special_account").AddRequiredPermission("GkhRegOp.Accounts.SpecialAccount.View");
            //rfAccounts.Add("Протоколы расчетов", "calculationprotocol").AddRequiredPermission("GkhRegOp.Accounts.CalculationProtocol.View");
            //rfAccounts.Add("Контроль транзитного счета", "controltransitaccount").AddRequiredPermission("GkhRegOp.Accounts.ControlTransitAccount.View");
            rfAccounts.Add("Документы на оплату за периоды", "paymentdocuments").AddRequiredPermission("GkhRegOp.Accounts.PaymentDocuments.View");
            //rfAccounts.Add("Взносы на КР", "crpayments").AddRequiredPermission("GkhRegOp.Accounts.PaymentCrSpecAccNotRegop.View");
            rfAccounts.Add("Банковские операции", "bank_statement").AddRequiredPermission("GkhRegOp.Accounts.BankOperations.View");
            rfAccounts.Add("Реестр документов на оплату", "paymentdoc_snapshot").AddRequiredPermission("GkhRegOp.Accounts.PaymentDocumentSnapshot.View");
            //rfAccounts.Add("Реестр субсидий", "subsidyincome").AddRequiredPermission("GkhRegOp.Settings.SubsidyIncome.View");
            //rfAccounts.Add("Реестр субсидий", "subsidyincome").AddRequiredPermission("GkhRegOp.Settings.SubsidyIncome.View");
            rfAccounts.Add("Реестр оплат платежных агентов", "bank_doc_import").AddRequiredPermission("GkhRegOp.Settings.BankDocumentImport.View");
            rfAccounts.Add("Реестр неопределенных оплат", "notdefined_payment").AddRequiredPermission("GkhRegOp.Settings.BankDocumentImport.View");
            rfAccounts.Add("Состояние выгрузки квитанций в ГИС ЖКХ", "statuspaymentdocumenthouses").AddRequiredPermission("GkhRegOp.Accounts.StatusPaymentDocumentHouses.View");
            rfAccounts.Add("Обновление сальдо", "saldo_refresh").AddRequiredPermission("GkhRegOp.Accounts.SaldoRefresh.View");
            rfAccounts.Add("Неподтвержденные оплаты", "unconfirmed_payments").AddRequiredPermission("GkhRegOp.Accounts.UnconfirmedPayments.View");
            rfAccounts.Add("Реестр ЛС, не сопоставленных с мобильным приложением", "mobileappaccountcomparsion").AddRequiredPermission("GkhRegOp.Accounts.MobileAppAccountComparsion.View");

            root.Add("Региональный фонд").Add("Абоненты").Add("Реестр абонентов", "regop_personal_acc_owner").AddRequiredPermission("GkhRegOp.PersonalAccountOwner.PersonalAccountOwner.View");
            root.Add("Региональный фонд").Add("Абоненты").Add("Реестр должников", "regop_debtor").AddRequiredPermission("GkhRegOp.PersonalAccountOwner.Debtor.View");
            root.Add("Региональный фонд").Add("Абоненты").Add("Информация по начисленным льготам", "persaccbenefits").AddRequiredPermission("GkhRegOp.PersonalAccountOwner.Benefits.View");
            root.Add("Региональный фонд").Add("Абоненты").Add("Реестр рассылок на электронную почту", "emailnewsletter").AddRequiredPermission("GkhRegOp.PersonalAccountOwner.EmailNewsletter.View");

            root.Add("Справочники").Add("Общие").Add("Справочник кодов населенных пунктов", "regop_dict_location_code").AddRequiredPermission("GkhRegOp.Dictionaries.RegopDictLocationCode");

            root.Add("Справочники").Add("Капитальный ремонт").Add("Типы программы КР", "programcrtype").AddRequiredPermission("GkhRegOp.Dictionaries.ProgramCrType.View");

            var rtRegFond = root.Add("Справочники").Add("Региональный фонд");
            rtRegFond.Add("Группы льготных категорий граждан", "privilegedcategory").AddRequiredPermission("GkhRegOp.Dictionaries.PrivilegedCategory.View");
            rtRegFond.Add("Периоды начислений", "charges_period").AddRequiredPermission("GkhRegOp.Settings.ChargePeriod.View");
            rtRegFond.Add("Группы лицевых счетов", "persaccgroup").AddRequiredPermission("GkhRegOp.Settings.PersAccGroup.View");
            rtRegFond.Add("Тарифы для эталонных начислений", "tariffbyperiodforclaimwork").AddRequiredPermission("GkhRegOp.Dictionaries.TariffByPeriodForClaimWork.View");

            var loanAndCredits = root.Add("Региональный фонд").Add("Займы и кредиты");

            loanAndCredits.Add("Управление займами", "loan_manage").AddRequiredPermission("GkhRegOp.Loans.Manage.View");
            loanAndCredits.Add("Реестр займов", "loans").AddRequiredPermission("GkhRegOp.Loans.Loan.View");
            loanAndCredits.Add("Реестр кредитов", "calcaccountcredit").AddRequiredPermission("GkhRegOp.Loans.AccountCredit.View");
            //loanAndCredits.Add("Овердрафт", "calcaccountoverdraft").AddRequiredPermission("GkhRegOp.Loans.Overdraft.View");

            root.Add("Региональный фонд").Add("Счета").Add("Квитанции на оплату Сбербанку", "sberbankpaymentdoc").AddRequiredPermission("GkhRegOp.Accounts.SberbankPaymentDoc.View");

            root.Add("Региональный фонд").Add("Настройки").Add("Информации для физических лиц", "paymentdocinfo").AddRequiredPermission("GkhRegOp.Settings.PaymentDocInfo.View");
            //root.Add("Региональный фонд").Add("Настройки").Add("Реестр банковских выписок", "bank_acc_statement").AddRequiredPermission("GkhRegOp.Settings.BankAccountStatement.View");

            root.Add("Региональный фонд").Add("Настройки").Add("Проверки перед закрытием месяца", "cp_checks").AddRequiredPermission("GkhRegOp.Settings.PeriodChecks.View");
            root.Add("Региональный фонд").Add("Расчетный месяц").Add("Проверка и закрытие месяца", "cp_checking").AddRequiredPermission("GkhRegOp.AccountingMonth.PeriodChecking.View");

            //var rfOptions = root.Add("Региональный фонд").Add("Настройки");

            //rfOptions.Add("Размеры взносов на КР", "paymentsizecr").AddRequiredPermission("GkhRegOp.Settings.PaymentSizeCr.View");
            //rfOptions.Add("Федеральный стандарт взноса на КР", "federalstandardfeecr").AddRequiredPermission("GkhRegOp.Settings.FederalStandardFeeCr.View");
            //rfOptions.Add("Сопоставление платежных агентов", "comparison_paying_agents").AddRequiredPermission("GkhRegOp.Settings.ComparisonPayingAgents.View");

            //root.Add("Региональный фонд").Add("Использование регионального фонда").Add("Реестр займов", "loanregister").AddRequiredPermission("GkhRegOp.RegionalFundUse.Loan.View");
            root.Add("Региональный фонд").Add("Формирование регионального фонда").Add("Реестр договоров на формирование фонда капитального ремонта", "fundformationcontract").AddRequiredPermission("GkhRegOp.FormationRegionalFund.FundFormationContract.View"); 

            var admImportNode = root.Add("Администрирование").Add("Импорты");

            //admImportNode.Add("Импорт собственников", "ownersimport").AddRequiredPermission("Import.OwnersImport");
            admImportNode.Add("Импорт абонентов", "roomimport").AddRequiredPermission("Import.RoomImport.View");
            admImportNode.Add("Импорт абонентов (замена данных)", "ownerroomimport").AddRequiredPermission("Import.OwnerRoomImport");

            admImportNode.Add("Импорт протоколов решений", "decisionprotocolimport")
                .AddRequiredPermission("Import.DecisionProtocolImport");
            /*admImportNode.Add("Импорт протоколов собственников", "ownerdecisionprotocolimport")
                .AddRequiredPermission("Import.OwnerDecisionProtocolImport");*/

            //admImportNode.Add("Импорт социальной поддержки", "socialsupportimport").AddRequiredPermission("Import.SocialSupportImport");
            admImportNode.Add("Импорт начислений и оплат (оффлайн версия)", "offlinepaymentchargeimport").AddRequiredPermission("Import.PersonalAccountChargePaymentImportXml");

            admImportNode.Add("Импорт начислений (в закрытые периоды)", "chargestoclosedperiodimport").AddRequiredPermission("Import.ChargesToClosedPeriods.View");
            admImportNode.Add("Импорт оплат (в закрытые периоды)", "paymentstoclosedperiodimport").AddRequiredPermission("Import.PaymentsToClosedPeriods.View");
            admImportNode.Add("Импорт данных в ПИР", "debtorclaimworkimport").AddRequiredPermission("Import.DebtorClaimWorkImport.View");
            admImportNode.Add("Импорт признака выдачи электронных квитанций", "personalaccountdigitalreceiptimport").AddRequiredPermission("Import.PersonalAccountDigitalReceiptImport.View");

            root.Add("Региональный фонд").Add("Использование регионального фонда").Add("Заявки на перечисление средств подрядчикам", "transferctr").AddRequiredPermission("GkhRf.TransferCtr.View").WithIcon("requestTransferRf");

            var settingsMenu = root.Add("Региональный фонд").Add("Настройки");
            
            settingsMenu.Add("Логи обращений к сервисам", "regopservicelog").AddRequiredPermission("GkhRegOp.Settings.RegopServiceLog.View");
            settingsMenu.Add("Шаблоны квитанций по периодам", "paydoctemplate").AddRequiredPermission("GkhRegOp.Settings.PayDocTemplate.View");
        }
    }
}