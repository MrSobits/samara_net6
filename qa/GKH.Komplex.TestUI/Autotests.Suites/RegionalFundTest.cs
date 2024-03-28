using Autotests.Utilities;
using Autotests.Utilities.Tags;
using Autotests.WebPages;
using Autotests.WebPages.Root;
using NUnit.Framework;

namespace Autotests.Suites
{
    [TestFixture]
    public class RegionalFundTests
    {
        #region SetUp And TearDown
        [SetUp]
        public void Init()
        {
            Pages.Login.Open();
            Pages.Login.DoLogin("admin_chelyabinsk", "admin_chelyabinsk");
        }
        [TearDown]
        public void TearDown()
        {
            Browser.Quit();
        }
        #endregion

        #region Data
        string accnum = "18092016";
        string beginDate = "01.09.2014";
        string date = "15.09.2016";
        string datebegin = "18.09.2016";
        string dateend = "19.09.2016";
        string home = "ж/д_ст. Буранная, ул. Дорожная, д. 1";
        string importbank = "Импорт банковских выписок";
        string month1 = "Сент";
        string month2 = "Окт";
        string nameUser1 = "КОЧУРА Н И";
        string nameUser2 = "КОЧУРА Д В";
        string numberBanRecalc = "250040893";
        string numberCalc = "250040888";
        string numberClosePersAcc = "250028156";
        string numberOfPersAcc = "250040895";
        string numberOpenClosePersAcc = "201160286";
        string numberOpenPersAcc = "251129342";
        string numdoc = "18";
        string printF = "print_f";
        string regFund = "Региональный фонд";
        string sum = "1000";
        string summ = "500";
        string surname = "КОЧУРА";
        string surname1 = "Кочура";
        string surnamebanrecalc = "ЕГОРОВА";
        string surnameCalc = "ФЕДОТОВА";
        string surnameN = "Ч";
        string valuechange = "Изменение перерасчета";
        string valuedata = "17.09.2016";
        string valuenull = "0";
        string valueone = "1";
        string valuePenalty1 = "29,32";
        string valuePenalty2 = "29,34";
        #endregion

        #region Operations
        string banReCalc = "Запрет перерасчета";
        string cancelCharges = "Отмена начислений за периоды";
        string CancelCharges = "Отмена начислений";
        string CancelChargingPenalties = "Отмена начисления пени";
        string changeSubscriber = "Смена абонента";
        string closed = "Закрытие";
        string manualCalc = "Ручной перерасчет";
        string offset = "Зачет средств за ранее выполненные работы";
        string reopen = "Повторное открытие";
        string setandchange = "Установка и изменение пени";
        string setAndChangeSaldo = "Установка и изменение сальдо";
        #endregion

        /// <summary>
        /// 1 Модуль «Региональный фонд»
        /// <para>
        /// Операции, выполняемые при закрытии периода
        /// Расчет начислений
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_01_CalculationOfCharges()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectCrFoundType();
            Pages.RegionalFund.ClickCalculation();
            Pages.RegionalFund.VerifyCalculation();
            Assert.IsTrue(Pages.RegionalFund.datetoday == Pages.RegionalFund.currentdate);
        }
        /// <summary>
        /// <para>
        /// Подтверждение начислений
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_02_ConfirmationOfCharges()
        {
            Pages.Menu.GotoRegistryOfUnsubstantiatedCharges();
            Pages.UnacceptingCharges.CheckFormedSection();
            Pages.UnacceptingCharges.ClickOnConfirm();
            Assert.IsTrue(Pages.UnacceptingCharges.messageSuccessfulConfirm.Enabled);
        }
        /// <summary>
        /// <para>
        /// Контрольные проверки
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_03_ControlChecks()
        {
            Pages.Menu.GotoCalculatedMonth();
            Pages.CheckingAndClosingTheMonth.ClickValidate();
            Assert.AreEqual(Pages.CheckingAndClosingTheMonth.datetoday, Pages.CheckingAndClosingTheMonth.currentdate, "Даты не совпадают");
        }
        /// <summary>
        /// <para>
        /// Контрольные отчеты
        /// </para>
        /// </summary>
        [Test]
        [TestCase("Контрольный отчет по суммам банковских выписок", "Контрольный отчет по суммам реестр оплат платежных агентов", "Контрольный отчет по дебатам")]
        public void TestRegFund_04_ControlReports_Error(string bankstatement, string payingagent, string debate)
        {
            Pages.ReportPanel.BankStatement(bankstatement);
            Pages.ReportPanel.PayingAgent(payingagent);
            Assert.True(Pages.ReportPanel.Debate(debate));
        }
        /// <summary>
        /// <para>
        /// Закрытие периода
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_05_PeriodEndClosing_Error()
        {
            Pages.Menu.GotoCalculatedMonth();
            Pages.CheckingAndClosingTheMonth.CloseCurrentPeriod();
            Assert.IsFalse(Pages.CheckingAndClosingTheMonth.messageError.Exists());
        }
        /// <summary>
        /// <para>
        /// Тестирование функциональности по перерасчетам
        /// Доля собственности абонента
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_06_TheShareOwned()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.EditPersonalAccount(numberOfPersAcc, surname);
            Pages.RegionalFund.TransitionSubscribes(surname);
            Pages.RegionalFund.InformationAboutTheRooms();
            Pages.RegionalFund.Ownership();
            Pages.RegionalFund.ChangeTheValueOfTheShare(date, valuenull);
            Pages.RegionalFund.GoToHistory();
            Assert.AreEqual(Pages.RegionalFund.verifyValue.Text, "0", "Доля собственности не изменилась", new string[] { });
        }
        /// <summary>
        /// <para>
        /// Площадь помещения
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_07_AreaRoom()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.EditPersonalAccount(numberOfPersAcc, surname);
            Pages.RegionalFund.TransitionRoom();
            Pages.RegionalFund.ChangeValue();
            Pages.RegionalFund.ChangeValueShare(date, valueone);
            Pages.RegionalFund.GoToHistory();
            Assert.AreEqual(Pages.RegionalFund.verifyValue.Text, "1", "Доля собственности не изменилась", new string[] { });
        }
        /// <summary>
        /// <para>
        /// Дата открытия ЛС
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_08_DateOpenLS()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.EditPersonalAccount(numberOfPersAcc, surname);
            Pages.RegionalFund.ChangeDateOpen();
            Pages.RegionalFund.ChangeDate(date, valuedata);
            Pages.RegionalFund.GoToHistory();
            Assert.AreEqual(Pages.RegionalFund.verifyValue.Text, "17.09.2016", "Дата собственности не изменилась", new string[] { });
        }
        /// <summary>
        /// <para>
        /// Проведение перерасчета
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_09_CarryingRecalculation()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectAccChange();
            Pages.RegionalFund.ClickCalculation();
            Assert.IsTrue(Pages.RegionalFund.datetoday == Pages.RegionalFund.currentdate);
        }
        /// <summary>
        /// <para>
        /// Формирование квитанций по ЛС+печать по нескольким тысячам ЛС
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_10_FormationPrintLS()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.EditPersonalAccount(numberOfPersAcc, surnameN);
            Pages.RegionalFund.UnloadAPaymentDoc();
            Pages.RegionalFund.ClickChoose();
            Pages.RegionalFund.WindowDownloading();
            Assert.IsTrue(Regop_Personal_Account.size.CompareTo(0) != 0);
            Pages.Menu.DeleteFilesFolder();
        }
        /// <summary>
        /// <para>
        /// Слияние абонентов
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_11_MergeSubscribers()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectOpenState();
            Pages.RegionalFund.Merger();
            Pages.RegionalFund.ChangeStatysClose();
            Assert.IsTrue(Regop_Personal_Account.verify == "0");
        }
        /// <summary>
        /// <para>
        /// Ручной перерасчет
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_12_ManualCounting_Error()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectOpenState();
            Pages.RegionalFund.EditPersonalAccount(numberOpenPersAcc, surnameN);
            Pages.RegionalFund.TransitionSubscribes(surnameN);
            Pages.RegionalFund.InformationAboutTheRooms();
            Pages.RegionalFund.ShareOfProperty(date, valuenull);
            Pages.RegionalFund.BackRegistryLS();
            Pages.RegionalFund.OtherOperation(manualCalc);
            Pages.RegionalFund.ManualRecalculation(date, valuechange);
            Pages.RegionalFund.ClickCalculation();
            Pages.RegionalFund.RegisterUnsubstantiatedCharges();
            Pages.RegionalFund.GotoPersAcc();
            Assert.AreEqual(Pages.RegionalFund.record.Text, "Записи с 1 по 1, всего 1", "Нет данных для отображения", new string[] { });
        }
        /// <summary>
        /// <para>
        /// Отмена начислений
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_13_CancelAccrual_Error()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectPersonalAccount(numberClosePersAcc, surnameN);
            Pages.RegionalFund.SelectTheClosedPeriod();
            Pages.RegionalFund.OtherOperation(CancelCharges);
        }
        /// <summary>
        /// <para>
        /// Отмена начисления пени
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_14_CancelFinesAccrual_Error()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectPersonalAccount(numberClosePersAcc, surnameN);
            Pages.RegionalFund.SelectTheClosedPeriod();
            Pages.RegionalFund.OtherOperation(CancelChargingPenalties);
        }
        /// <summary>
        /// <para>
        /// Смена абонента
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_15_ChangeUser()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectPersonalAccount(numberOfPersAcc, surname1);
            Pages.RegionalFund.OtherOperation(changeSubscriber);
            Pages.RegionalFund.NewUser(nameUser1, nameUser2, date);
            Pages.RegionalFund.ClickSaveNewOwner();
            Assert.IsTrue(Regop_Personal_Account.element1.CompareTo(Regop_Personal_Account.verify) != 0);
        }
        /// <summary>
        /// <para>
        /// Установка и изменение пени
        /// </para>
        /// </summary>
        [Test]
        [TestCase("Изменение пени")]
        public void TestRegFund_16_SetAndChangeFines(string changePenalty)
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectPersonalAccount(numberOfPersAcc, surname1);
            Pages.RegionalFund.OtherOperation(setandchange);
            Pages.RegionalFund.NewValue(valuePenalty1, valuePenalty2);
            Pages.RegionalFund.ReasonChangePenalty(changePenalty);
            Pages.RegionalFund.SaveChangePenalty(numberOfPersAcc, surname);
            Pages.RegionalFund.GoToHistory();
            Assert.IsTrue(Pages.RegionalFund.VerifyValue.Text == "Изменение пени");
        }
        /// <summary>
        /// <para>
        /// Установка и изменение сальдо
        /// </para>
        /// </summary>
        [Test]
        [TestCase("Изменение сальдо", "2005.99", "2005.98")]
        public void TestRegFund_17_SetAndChangeBalance(string changeSaldo, string valueSaldo1, string valueSaldo2)
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectPersonalAccount(numberOfPersAcc, surname1);
            Pages.RegionalFund.OtherOperation(setAndChangeSaldo);
            Pages.RegionalFund.NewValueSaldo(valueSaldo1, valueSaldo2);
            Pages.RegionalFund.ReasonChangeSaldo(changeSaldo);
            Pages.RegionalFund.SaveChangeSaldo(numberOfPersAcc, surname);
            Pages.RegionalFund.GoToHistory();
            Assert.IsTrue(Pages.RegionalFund.verifyValuePenaltySaldo.Text == "Изменение сальдо");
        }
        /// <summary>
        /// <para>
        /// Закрытие лицевого счета
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_18_ClosedLS()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectPersonalAccount(numberOpenClosePersAcc, surname1);
            Pages.RegionalFund.OtherOperation(closed);
            Pages.RegionalFund.ClosingDate(date);
            Pages.RegionalFund.SaveClosingDate(numberOfPersAcc, surname);
            Assert.IsTrue(Pages.RegionalFund.verifyCloseState.Text == "Закрыт" || Pages.RegionalFund.verifyCloseState.Text == "Закрыт с долгом");
        }
        /// <summary>
        /// <para>
        /// Повторное открытие лицевого счета
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_19_ReOpenTheAccount()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectPersonalAccount(numberOpenClosePersAcc, surname1);
            Pages.RegionalFund.OtherOperation(reopen);
            Pages.RegionalFund.reOpeningDate(date);
            Pages.RegionalFund.SaveOpenDate();
            Assert.IsTrue(Pages.RegionalFund.verifyOpenState.Text == "Открыт");
        }
        /// <summary>
        /// <para>
        /// Запись даты слияния
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_20_RecordTheDateOfMerger()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectOpenState();
            Pages.RegionalFund.Merger();
            Pages.RegionalFund.ChangeStatysClose();
            Pages.RegionalFund.CheckDateClosedPeriod(numberOfPersAcc, surname);
            Assert.IsTrue(Regop_Personal_Account.result == 0);
        }
        [Ignore("Требуется доработка")]
        /// <summary>
        /// <para>
        /// Массовая отмена начислений за периоды
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_21_MassCancelChargesForThePeriods()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectPersonalAccount(numberClosePersAcc, surnameN);
            Pages.RegionalFund.OtherOperation(cancelCharges);
            Pages.RegionalFund.DocumentBase();
            Pages.RegionalFund.SpecifyValue(summ);
            Pages.RegionalFund.SaveMassCancel();
            Pages.RegionalFund.GoToHistory();
            Assert.IsTrue(Pages.RegionalFund.verifyMassCancel.Text == "Отмена начислений за периоды");
        }
        [Ignore("Требуется доработка")]
        /// <summary>
        /// <para>
        /// Ручной перерасчет
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_22_ManualCounting_Error()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectAccChange();
            Pages.RegionalFund.OtherOperation(manualCalc);
            Pages.RegionalFund.ManualRecalculation(date, valuechange);
            Pages.RegionalFund.ClickCalculation();
            Pages.RegionalFund.RegisterUnsubstantiatedCharges();
            Pages.RegionalFund.GotoPersAcc();
            Assert.AreEqual(Pages.RegionalFund.record.Text, "Записи с 1 по 1, всего 1", "Нет данных для отображения", new string[] { });
        }
        /// <summary>
        /// <para>
        /// Зачет средств на ранее выполненные работы
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_23_OffsetFunds()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectPersonalAccount(numberOpenPersAcc, surname1);
            Pages.RegionalFund.OtherOperation(offset);
            Pages.RegionalFund.DateOperation(date);
            Pages.RegionalFund.SelectTheTypeOfDistribution();
            Pages.RegionalFund.SpecifyACertainSum(summ);
            Pages.RegionalFund.SaveDistribution();
            Pages.RegionalFund.GoToHistory();
            Assert.IsTrue(Pages.RegionalFund.MessageSuccessOffset.Text == "Зачет средств за ранее выполненные работы");
        }
        /// <summary>
        /// <para>
        /// Запрет перерасчета
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_24_BanReCalculate()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectPersonalAccount(numberBanRecalc, surnamebanrecalc);
            Pages.RegionalFund.OtherOperation(banReCalc);
            Pages.RegionalFund.DocumentBaseReCalc();
            Pages.RegionalFund.SetTheStartAndEndOfthePeriod(month1, month2);
            Pages.RegionalFund.ClickSave();
            Pages.RegionalFund.ClickCalculation();
            Pages.RegionalFund.EditPersonalAccount(numberBanRecalc,surnamebanrecalc);
            Pages.RegionalFund.GoToHistory();
            Assert.IsTrue(Pages.RegionalFund.MessageSuccessBanReCalc.Enabled);
        }
        /// <summary>
        /// <para>
        /// Расчет начислений
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_25_CalculationOfCharges()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectPersonalAccount(numberCalc, surnameCalc);
            Pages.RegionalFund.SelectAccChange();
            Pages.RegionalFund.ClickCalculation();
            Pages.RegionalFund.VerifyCalculation();
            Assert.IsTrue(Pages.RegionalFund.datetoday == Pages.RegionalFund.currentdate);
        }
        /// <summary>
        /// <para>
        /// Перерасчет начислений
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_26_ReCalculationOfCharges()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectPersonalAccount(numberCalc, surnameCalc);
            Pages.RegionalFund.EditPersonalAccount(numberCalc, surnameCalc);
            Pages.RegionalFund.TransitionRoom();
            Pages.RegionalFund.ChangeValue();
            Pages.RegionalFund.ChangeValueShare(date, valueone);
            Pages.RegionalFund.GoToTabbarRegFund();
            Pages.RegionalFund.SelectAccChange();
            Pages.RegionalFund.ClickCalculation();
            Pages.RegionalFund.VerifyCalculation();
            Assert.IsTrue(Pages.RegionalFund.datetoday == Pages.RegionalFund.currentdate);
        }
        /// <summary>
        /// <para>
        /// Изменение внешнего номера лицевого счета
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_27_ChangingTheNumberOfAccount()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectOpenState();
            Pages.RegionalFund.SelectPersonalAccount(numberOpenPersAcc, surnameN);
            Pages.RegionalFund.EditPersonalAccount(numberOpenPersAcc, surnameN);
            Pages.RegionalFund.ExternalNumberLS(date, printF);
            Assert.IsTrue(Browser.WaitUntil(Pages.RegionalFund.MessageSuccessChange.Exists()));
        }
        /// <summary>
        /// <para>
        /// Изменение даты открытия лицевого счета
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_28_ChangeDateOpenLS()
        {
            Pages.RegionalFund.GotoRegistryPersonalAccount();
            Pages.RegionalFund.SelectOpenState();
            Pages.RegionalFund.SelectPersonalAccount(numberOpenPersAcc, surnameN);
            Pages.RegionalFund.EditPersonalAccount(numberOpenPersAcc, surnameN);
            Pages.RegionalFund.ClickOpenDate(date, beginDate);
            Assert.IsTrue(Browser.WaitUntil(Pages.RegionalFund.MessageSuccessChange.Exists()));
        }
        /// <summary>
        /// <para>
        /// Выполнение контрольных проверок
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_29_PerformingControlChecks()
        {
            Pages.Menu.GotoCompaundingPeriod();
            Pages.ChargesPeriod.CloseTheCurrentPeriod();
            Pages.Menu.GotoCloseMonth();
            Pages.ChargesPeriod.CloseMonth();
            Assert.IsFalse(Charges_Period.verify);
        }
        [Ignore("Требуется доработка")]
        /// <summary>
        /// <para>
        /// Изменение Типа дома в карточке Жилого дома
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_30_ChangeApartmentBuilding()
        {
            Pages.Menu.GotoReport();
            Pages.ReportPanel.RunReport(regFund);
            Assert.IsTrue(Pages.ReportPanel.verify);
        }
        [Ignore("Требуется доработка")]
        /// <summary>
        /// <para>
        /// Тестирование работы с оплатами в системе
        /// Загрузка реестра платежных агентов
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_31_DownloadRegistryPayingAgents()
        {
            Pages.Menu.GotoRegistrySettingImportPayments();
            Pages.Menu.GotoRegistryFeesPaymentsAgents();
            Pages.BankDocImport.Import();
            Pages.BankDocImport.DisplayedLine();
            Pages.BankDocImport.ChooseOneTickLineAndConfirm();
            Pages.BankDocImport.RegisterLS();
            Pages.BankDocImport.DisplayedLoadedPayment();
            Assert.IsTrue(Bank_Doc_Import.resultsum == 0);
        }
        [Ignore("Требуется доработка")]
        /// <summary>
        /// <para>
        /// Загрузка банковской выписки
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_32_DownloadBankStatement()
        {
            Pages.Menu.GoToBanking();
            Pages.BankStatement.ClickImportBankStatement();
            Pages.Menu.GotoTaskList();
            Pages.BankStatement.TaskAfterImport(importbank);
            Assert.IsTrue(Pages.BankStatement.waitMessageSuccess.Enabled);
        }
        /// <summary>
        /// <para>
        /// Создание банковской операции
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_33_CreatingABankingTransaction()
        {
            Pages.Menu.GoToBanking();
            Pages.BankStatement.ClickAdd();
            Pages.BankStatement.FillRequiredFields(datebegin, dateend, numdoc, sum, accnum);
            Assert.IsTrue(Browser.WaitUntil(Pages.BankStatement.messageSuccessSave.Displayed));
        }
        /// <summary>
        /// <para>
        /// Распределение приходной банковской операции на Дом
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_34_PayBankingOperationsInTheHouse()
        {
            Pages.Menu.GoToBanking();
            Pages.BankStatement.Spread(accnum);
            Pages.BankStatement.ChooseHouse(home);
            Pages.BankStatement.ClickButtons();
            Pages.Menu.GotoRegistryOfHouses();
            Pages.BankStatement.AccountPayment(home);
            Assert.IsTrue(Pages.BankStatement.debtTotal.GetAttribute(TagAttributes.Value) != null);
        }
        [Ignore("Требуется доработка")]
        /// <summary>
        /// <para>
        /// Распределение приходной банковской операции на Лицевой счет
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_35_DestributionPayBankinkOperations()
        {
            Pages.Menu.GoToBanking();
            Pages.BankStatement.Spread(accnum);
            Pages.BankStatement.SelectTickLS(home);
            Pages.BankStatement.DestributeApplyDistribution();
            Pages.Menu.RegLS();
            Assert.IsTrue(Pages.BankStatement.result == 0);
        }
        [Ignore("Требуется доработка")]
        /// <summary>
        /// <para>
        /// Распределение расходной банковской операции на оплату подрядчику
        /// </para>
        /// </summary>
        [Test]
        public void TestRegFund_36_DistributionBankingTransaction()
        {
            Pages.Menu.GoToBanking();
            Pages.BankStatement.SpreadReceiptOfPaymentOfRent(accnum);
            Pages.BankStatement.ChooseAddress(home);
            Pages.BankStatement.ClickButton();
            Pages.Menu.RegLS();
            Pages.Menu.GotoRegistryPersAcc();
            Pages.BankStatement.GotoRegLs(home);
            Assert.IsTrue(Pages.BankStatement.result == 0);
        }
    }
}