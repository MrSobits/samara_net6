using System;
using Autotests.Utilities;
using Autotests.Utilities.Tags;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading;
using System.IO;
using System.Linq;

namespace Autotests.WebPages.Root
{
    public class Regop_Personal_Account : PageBase
    {
        #region DataMenu
        private WebElement mainMenu { get { return new WebElement().ByXPath(string.Format("//button[@data-qtip='Главное меню']/span/span")); } }
        private WebElement registryPersAcc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Реестр лицевых счетов']")); } }
        private WebElement tabbarRegFund { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'tabbar')]//span[text()='Реестр лицевых счетов']")); } }
        private WebElement task { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mainmenu')]//span[text()='Задачи']")); } }
        private WebElement taskList { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mainmenu')]//div[starts-with(@id,'menuright')]//div[text()='Список задач']")); } }
        private WebElement unsubstantiatedcharges { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Реестр неподтвержденных начислений']")); } }
        #endregion

        #region Data
        int day, month, year;
        private decimal count, sum, valueAreaShare1;
        private decimal valueelement1, valueelement2;
        private string param, numberOfPersAcc, date, changeValue, surnameLS, operation;
        public static int ls1 = 2, ls2 = 3;
        public static int size, result;
        public static string element1, element2;
        public static string elementint1 = "", elementint2 = "";
        public static string verify;
        public string datetoday, currentdate, period;
        private static string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"..\..\..\DownloadFiles";
        string fileload = @"14102016110416" + "." + "zip";
        private string waitBanReCalc = "//div[starts-with(@id,'loadmask')]/div[text()='Запрет перерасчета...']";
        private string waitForming = "//div[starts-with(@id,'loadmask')]/div[text()='Формирование начислений...']";
        private string waitload = "//div[starts-with(@id,'loadmask')]/div[text()='Загрузка...']";
        private string waitpls = "//div[starts-with(@id,'loadmask')]/div[text()='Пожалуйста подождите...']";
        private string waitsave = "//div[starts-with(@id,'loadmask')]/div[text()='Сохранение']";
        private WebElement actualFrom { get { return new WebElement().ByXPath(string.Format("//table[starts-with(@id,'changeownerwin')]//table[starts-with(@id,'datefield')]/tbody/tr/td/input[@name='ActualFrom']")); } }
        private WebElement areaShare { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]//td/input[@name='AreaShare']")); } }
        private WebElement baseRate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'masscancelchargegrid')]//div[5]//input[starts-with(@id,'gkhdecimalfield')]")); } }
        private WebElement buttomOkMonthPicker { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'monthpicker')]/div/div/em/button/span[contains(text(),'OK')]")); } }
        private WebElement buttonChoose { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]//span[text()='Выбрать']")); } }
        private WebElement buttonClose { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paownereditwin')]//span[text()='Закрыть']")); } }
        private WebElement buttonCloseAddWin { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paowneraccountaddwin')]//button[@data-qtip='Закрыть']/span[contains(@id,'btnIcon')]")); } }
        private WebElement buttonCloseCardRoom { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'roomeditwindow')]//button[@data-qtip='Закрыть']/span[contains(@id,'Icon')]")); } }
        private WebElement buttonCloseEditWin { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paownereditwin')]//button[@data-qtip='Закрыть']/span[contains(@id,'btnIcon')]")); } }
        private WebElement buttonOK { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//span[text()='OK']")); } }
        private WebElement buttonSave { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mergeaccountwin')]//div[starts-with(@id,'toolbar')]//button[@data-qtip='Сохранить']/span[text()='Применить']")); } }
        private WebElement buttonSaveBanReCalc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'banrecalcwin')]//button[@data-qtip='Сохранить']/span[text()='Сохранить']")); } }
        private WebElement buttonSaveCardRoom { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'roomeditwindow')]//button[@data-qtip='Сохранить']/span[contains(@id,'Icon')]")); } }
        private WebElement buttonSaveChangePenalty { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'setpenaltywin')]/div/div/div/div/em/button/span[text()='Сохранить']")); } }
        private WebElement buttonSaveClosing { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]/div[starts-with(@id,'window')]//span[text()='Подтвердить']")); } }
        private WebElement buttonSaveDistribution { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'distributefundsforperformedworkwindow')]//button[@data-qtip='Применить']/span[text()='Применить']")); } }
        private WebElement buttonSaveEditWin { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paownereditwin')]//button[@data-qtip='Сохранить']/span[contains(@id,'btnIcon')]")); } }
        private WebElement buttonSaveExternal { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]/div[starts-with(@id,'window')]//button[@data-qtip='Сохранить']/span[text()='Сохранить']")); } }
        private WebElement buttonSaveMassCancel { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'masscancelchargewin')]//button[@data-qtip='Сохранить']/span[text()='Сохранить']")); } }
        private WebElement buttonSaveNewOwner { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]/div[starts-with(@id,'changeownerwin')]//span[text()='Сохранить']")); } }
        private WebElement buttonSaveShare { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'window')]//div[starts-with(@id,'toolbar')]//span[text()='Сохранить']")); } }
        private WebElement buttonSaveShareOfProperty { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paowneraccountaddwin')]//span[text()='Сохранить']")); } }
        private WebElement buttonSaveShareProperty { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paownereditwin')]//span[text()='Сохранить']")); } }
        private WebElement ButtonUpdate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'unacceptedchargepacketgrid')]//button[@data-qtip='Обновить']/span[text()='Обновить']")); } }
        private WebElement buttonYes { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//span[text()='Да']")); } }
        private WebElement calculation { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//span[text()='Расчет']")); } }
        private WebElement changeArea { get { return new WebElement().ByXPath(string.Format("//input[@name='Area']/../../../../../../../../../div/em/button[@data-qtip='Сменить значение']/span[contains(@id,'Icon')]")); } }
        private WebElement changeroom { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paowneraccountaddwin')]//fieldset[2]//div[starts-with(@id,'changevalbtn')]/em/button[@data-qtip='Сменить значение']/span[contains(@id,'Icon')]")); } }
        private WebElement changetheValue { get { return new WebElement().ByXPath(string.Format("//input[@name='AreaShare']/../../../../../../../../../div/em[starts-with(@id,'changevalbtn')]/button[@data-qtip='Сменить значение']/span[contains(@id,'Icon')]")); } }
        private WebElement checkAllLS { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]/div/div/div/div[1]/div[1]/span[contains(@id,'textEl')]")); } }
        private WebElement checkcrFoundType { get { return new WebElement().ByXPath(string.Format("//div[text()='Специальный счет регионального оператора']/../../td[1]/div/div")); } }
        private WebElement checkLS { get { return new WebElement().ByXPath(string.Format("//div[@id='contentPanel-body']/div/div[starts-with(@id,'paccountgrid')]//tbody/tr[2]/td[1]/div/div")); } }
        private WebElement checkNewUser { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'window')]//tr[2]/td[1]/div/div")); } }
        private WebElement clickNewSaldo { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'masssaldochangegrid')]//tr[2]/td[5]/div")); } }
        private WebElement closeDate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]//input[@name='CloseDate']")); } }
        private WebElement closingDate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//div[starts-with(@id,'window')]//td[starts-with(@id,'datefield')]/input[@name='closeDate']")); } }
        private WebElement combobox { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'b4gridstatecolumn')]//tr[starts-with(@id,'b4combobox')]//tr/td[2]/div")); } }
        private WebElement comboboxSelectClose { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'boundlist')]/div/ul/li[text()='Закрыт']")); } }
        private WebElement comboboxSelectNone { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'boundlist')]/div/ul/li[text()='-']")); } }
        private WebElement comboboxSelectOpen { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'boundlist')]/div/ul/li[text()='Открыт']")); } }
        private WebElement compareName { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'unacceptedchargepacketgrid')]//tr[2]/td[3]/div")); } }
        private WebElement comparenull { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'unacceptedchargepacketgrid')]//tr[2]/td[3]/div[contains(text(),'Начисление по 0 лицевым счетам')]")); } }
        private WebElement Confirm { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]/div[starts-with(@id,'window')]//button[@data-qtip='Сохранить']/span[text()='Подтвердить']")); } }
        private WebElement count25 { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'boundlist')]/ul/li[text()='25']")); } }
        private WebElement countElement { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//table[starts-with(@id,'combobox')]//td[2]/div")); } }
        private WebElement crFoundType { get { return new WebElement().ByXPath(string.Format("//input[@name='crFoundType']/../../td[2]/div")); } }
        private WebElement currentOwner { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//table[starts-with(@id,'changeownerwin')]//input[@name='CurrentOwner']")); } }
        private WebElement CurrentSaldo { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'masssaldochangegrid')]//tr[2]/td[4]/div")); } }
        private WebElement dateEnd { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'banrecalcwin')]//td[starts-with(@id,'b4monthpicker')]/input[@name='DateEnd']/../../td[2]/div")); } }
        private WebElement dateFact { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'window')]/div[starts-with(@id,'window')]//input[@name='factDate']")); } }
        private WebElement dateInstallValue { get { return new WebElement().ByXPath(string.Format("//label[text()='Дата установки значения:']/../../td[2]//td[1]/input")); } }
        private WebElement dateOpen { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]//div[starts-with(@id,'container')][2]//button[@data-qtip='Сменить значение']/span[contains(@id,'Icon')]")); } }
        private WebElement daterun { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'taskentrygrid')]//tr[2]/td[4]/div")); } }
        private WebElement dateStart { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'banrecalcwin')]//td[starts-with(@id,'b4monthpicker')]/input[@name='DateStart']/../../td[2]/div")); } }
        private WebElement dateValue { get { return new WebElement().ByXPath(string.Format("//label[text()='Дата установки значения:']/../../td[2]//td[2]/div")); } }
        private WebElement debtPenalty { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//div[starts-with(@id,'setpenaltywin')]//input[@name='DebtPenalty']")); } }
        private WebElement distributionsum { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'distributefundsforperformedworkwindow')]//table[starts-with(@id,'numberfield')]//input[@name='DistributionSum']")); } }
        private WebElement docBase { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'masscancelchargewin')]//table[starts-with(@id,'b4filefield')]//td[2]/div")); } }
        private WebElement editNumber { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]//div[1]/div/div/div/em/button[@data-qtip='Сменить значение']/span[2]")); } }
        private WebElement editPersAcc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//tr[2]/td[2]/div/img[@data-qtip='Редактировать']")); } }
        private WebElement editUnaccepted { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'unacceptedchargepacketgrid')]//tr[2]/td[2]/div/img[@data-qtip='Редактировать']")); } }
        private WebElement evently { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'boundlist')]/div/ul/li[text()='Равномерное']")); } }
        private WebElement factdate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]/div[starts-with(@id,'window')]//table[starts-with(@id,'datefield')]//input[@name='factDate']")); } }
        private WebElement file { get { return new WebElement().ByXPath(string.Format("//input[@name='File']/../../td[1]/input")); } }
        private WebElement fines { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'masscancelchargegrid')]//div[9]//input[starts-with(@id,'gkhdecimalfield')]")); } }
        private WebElement formationOfCharges { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]/span[text()='Формирование начислений']")); } }
        private WebElement history { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccountcardpanel')]//span[text()='История изменений']")); } }
        private WebElement infoRoom { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paownereditwin')]//span[text()='Сведения о помещениях']")); } }
        private WebElement inputElement { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//div[starts-with(@id,'mergeaccountwin')]//tr[3]/td[2]/div")); } }
        private WebElement inputNewOwner { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'ext-comp')]/table[starts-with(@id,'numberfield')]//input[@name='NewShare']")); } }
        private WebElement inputnewOwner1 { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//div[starts-with(@id,'mergeaccountwin')]//tr[2]/td[1]/div")); } }
        private WebElement inputnewOwner2 { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//div[starts-with(@id,'mergeaccountwin')]//tr[3]/td[1]/div")); } }
        private WebElement inputSurname { get { return new WebElement().ByXPath(string.Format("//input[@name='Surname']")); } }
        private WebElement inputType { get { return new WebElement().ByXPath(string.Format("//input[@name='Type']")); } }
        private WebElement menuoperation { get { return new WebElement().ByXPath(string.Format("//a[starts-with(@id,'menuitem')]/span[text()='{0}']", param)); } }
        private WebElement messageError { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//span[text()='Ошибка']")); } }
        private WebElement MessageErrorSaldo { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//div[text()='Новое сальдо равно исходному сальдо']")); } }
        private WebElement MessageSuccessChangePenalty { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//div[text()='Выполнено успешно']")); } }
        private WebElement MessageSuccessDistribution { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//div[text()='Распределение выполнено успешно!']")); } }
        private WebElement MessageSuccessRecalc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//div[text()='Выполнено успешно']")); } }
        private WebElement monthPicker { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'monthpicker')]/div/div/div/a[text()='{0}']", param)); } }
        private WebElement name { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'window')]//label[text()='ФИО/Наименование:']/../../td[2]/input")); } }
        private WebElement newOwner { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//div[starts-with(@id,'mergeaccountwin')]//tr[2]/td[3]/div")); } }
        private WebElement newOwner1 { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//div[starts-with(@id,'mergeaccountwin')]//tr[3]/td[3]/div")); } }
        private WebElement newPenalty { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//div[starts-with(@id,'setpenaltywin')]//input[@name='NewPenalty']")); } }
        private WebElement NewSaldoByBaseTariff { get { return new WebElement().ByXPath(string.Format("//td[starts-with(@id,'gkhdecimalfield')]/input[@name='NewSaldoByBaseTariff']")); } }
        private WebElement newuser { get { return new WebElement().ByXPath(string.Format("//input[@name='NewOwner']/../../td[2]/div")); } }
        private WebElement nextPageGoto { get { return new WebElement().ByXPath(string.Format("//button[@data-qtip='Следующая страница']/span[contains(@id,'btnIcon')]")); } }
        private WebElement noDate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'unacceptedchargegrid')]//div[starts-with(@id,'b4pagingtoolbar')]/div[contains(text(),'Нет данных')]")); } }
        private WebElement numberTextfield { get { return new WebElement().ByXPath(string.Format("//label[text()='Номер:']/../../td[2]/input")); } }
        private WebElement onlineTemplate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'reportcustomgrid')]//div[text()='Счет (с адресом)']/../..//td[4]/div/a[text()='Скачать']")); } }
        private WebElement openDate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paowneraccountaddwin')]//input[@name='OpenDate']")); } }
        private WebElement openEditInfo { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paowneraccountgrid')]//tr[2]/td[1]/div/img[@data-qtip='Редактировать']")); } }
        private WebElement operationDate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'distributefundsforperformedworkwindow')]//table[starts-with(@id,'datefield')]//input[@name='OperationDate']")); } }
        private WebElement otherOperation { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//span[text()='Другие операции']")); } }
        private WebElement ownershipnull { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//div[contains(text(),'доля собственности равна 0')]")); } }
        private WebElement rateDecision { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'masscancelchargegrid')]//div[7]//input[starts-with(@id,'gkhdecimalfield')]")); } }
        private WebElement Reason { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'masssaldochangewin')]//input[@name='Reason']")); } }
        private WebElement reason { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mergeaccountwin')]//input[@name='Reason']")); } }
        private WebElement reasonPenalty { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'setpenaltywin')]//textarea[@name='Reason']")); } }
        private WebElement reasonRecalc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]/div[starts-with(@id,'window')]//input[@name='Reason']")); } }
        private WebElement recalcDateBegin { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]/div[starts-with(@id,'window')]//input[@name='recalcDateStart']")); } }
        private WebElement registryLS { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'desktoptopbar')]//span[text()='Реестр лицевых счетов']")); } }
        private WebElement reOpenDate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//div[starts-with(@id,'window')]//td[starts-with(@id,'datefield')]/input[@name='openDate']")); } }
        private WebElement replacementreports { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Замена шаблонов']")); } }
        private WebElement reports { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menufirstlevelitem')]/div/span[text()='Отчеты']")); } }
        private WebElement SaveButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Сохранить']")); } }
        private WebElement select { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'window')]//span[text()='Выбрать']")); } }
        private WebElement selectelement { get { return new WebElement().ByXPath(string.Format("//td[2]/div[text()='2016 Март']/../../td[1]/div/div")); } }
        private WebElement selectLS1 { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//tr[" + ls1 + "]/td[1]/div/div")); } }
        private WebElement selectLS2 { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//tr[" + ls2 + "]/td[1]/div/div")); } }
        private WebElement selectNewUser { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'window')]/div[starts-with(@id,'toolbar')]//span[text()='Выбрать']")); } }
        private WebElement sortdate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'taskentrygrid')]//div[starts-with(@id,'container')][2]//table[starts-with(@id,'datefield')]//td/input/../../td[2]/div")); } }
        private WebElement spread { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//div[starts-with(@id,'distributefundsforperformedworkwindow')]//span[text()='Распределить']")); } }
        private WebElement SuccessSetChangeSaldo { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]//div[text()='Установка и изменение сальдо выполнена успешно']")); } }
        private WebElement switchOver { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]//span[text()='Переходы']")); } }
        private WebElement switchOverRoom { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuitem')]/a/span[text()='Перейти к помещению']")); } }
        private WebElement switchOverSubscriber { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuitem')]/a/span[text()='Перейти к абоненту']")); } }
        private WebElement textLS1 { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//tr[" + ls1 + "]/td[4]/div[text()]")); } }
        private WebElement textLS2 { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//tr[" + ls2 + "]/td[4]/div[text()]")); } }
        private WebElement today { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'datepicker')]//span[text()='Сегодня']")); } }
        private WebElement typeDestribytion { get { return new WebElement().ByXPath(string.Format("//input[@name='DistributionType']/../../td[2]/div")); } }
        private WebElement unload { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]//div[starts-with(@id,'toolbar')]//td//td[2]/div")); } }
        private WebElement Value { get { return new WebElement().ByXPath(string.Format("//input[@name='value']")); } }
        private WebElement valueFactDate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'personalaccounteditpanel')]/div[starts-with(@id,'window')]//input[@name='value']")); } }
        private WebElement valueShare { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'window')]/div[starts-with(@id,'window')]//input[@name='value']")); } }
        private WebElement verifyNewUser { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//tr[2]/td[7]/div")); } }
        private WebElement waitOperationsLS { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paoperationgrid')]//tr[2]/td[2]/div")); } }
        private WebElement windowProtocol { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'formwindow')]/div/div[starts-with(@id,'unacceptedchargegrid')]//tr[2]/td[1]/div")); } }
        public WebElement MessageSuccessBanReCalc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccounthistorygrid')]//tr[2]/td[1]/div[text()='Запрет перерасчета']")); } }
        public WebElement MessageSuccessChange { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/div/p[text()='Значение параметра успешно изменено.']")); } }
        public WebElement MessageSuccessOffset { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccounthistorygrid')]//tr[2]/td[1]/div[text()='Зачет средств за ранее выполненные работы']")); } }
        public WebElement record { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'unacceptedchargegrid')]//div[starts-with(@id,'b4pagingtoolbar')]/div[13]")); } }
        public WebElement verifyCloseState { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//tr[2]/td[9]/div/div[contains(text(),'Закрыт')]")); } }
        public WebElement verifyMassCancel { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccounthistorygrid')]//tr[3]/td[1]/div[text()='Отмена начислений за периоды']")); } }
        public WebElement verifyOpenState { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccountgrid')]//tr[2]/td[9]/div/div[contains(text(),'Открыт')]")); } }
        public WebElement verifyValue { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccounthistorygrid')]//tr[2]/td[3]/div[text()]")); } }
        public WebElement VerifyValue { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccounthistorygrid')]//tr[2]/td[7]/div[text()]")); } }
        public WebElement verifyValuePenaltySaldo { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccounthistorygrid')]//tr[2]/td[7]/div[text()]")); } }
        #endregion

        #region DataSort
        string viewUnaccepted = "//div[starts-with(@id,'unacceptedchargepacketgrid')]//span[text()='Дата формирования']";
        string viewValue = "//div[starts-with(@id,'paccounthistorygrid')]//span[text()='Дата установки значения']";
        private WebElement sortdesc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menu')]//a[starts-with(@id,'menuitem')]/span[text()='Сортировать по убыванию']")); } }
        private WebElement sortUnaccepted { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'unacceptedchargepacketgrid')]//span[text()='Дата формирования']/../div")); } }
        private WebElement sortValue { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'paccounthistorygrid')]//span[text()='Дата установки значения']/../div")); } }
        #endregion

        public void GotoRegistryPersonalAccount()
        {
            AgainGoToInvoice:
            Pages.Menu.GotoInvoice();
            registryPersAcc.Click(false);
            try
            {
                Browser.WaitUntil(checkLS.Displayed);
                checkLS.WaitIsClickable();
                Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            }
            catch
            {
                Pages.Login.Open();
                Pages.Login.DoLogin("admin_chelyabinsk", "admin_chelyabinsk");
                goto AgainGoToInvoice;
            }
        }

        public void SelectPersonalAccount(string numberOfLS, string surname)
        {
            this.numberOfPersAcc = numberOfLS;
            this.surnameLS = surname;
            var WebElement = new WebElement();
            numberTextfield.Text = numberOfPersAcc;
            numberTextfield.SendKeys(Keys.Enter);
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(WebElement.ByXPath("//div[starts-with(@id,'paccountgrid')]//tr[2]/td[3]/div[text()='" + numberOfPersAcc + "']").Enabled);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void EditPersonalAccount(string numberOfLS, string surname)
        {
            SelectPersonalAccount(numberOfLS, surname);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            editPersAcc.WaitIsClickable();
            editPersAcc.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            Browser.WaitHideElement(By.XPath(waitpls));
            Browser.WaitUntil(waitOperationsLS.Displayed);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void SelectCrFoundType()
        {
            Browser.WaitUntil(checkLS.Exists());
            crFoundType.Click(false);
            checkcrFoundType.Click(false);
            select.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            checkAllLS.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromMilliseconds(500));
        }

        public void ClickCalculation()
        {
            calculation.Click(false);
            Browser.WaitUntil(formationOfCharges.Displayed);
            buttonOK.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitForming));
            buttonOK.Click(false);
        }

        public void VerifyCalculation()
        {
            mainMenu.Click(false);
            task.Click(false);
            taskList.Click(false);
            sortdate.Click(false);
            today.Click(false);
            day = DateTime.Now.Day;
            month = DateTime.Now.Month;
            year = DateTime.Now.Year;
            Browser.WaitHideElement(By.XPath(waitload));
            if (day < 10)
            {
                datetoday = "0" + day + "." + month + "." + year;
            }
            else
            {
                datetoday = day + "." + month + "." + year;
            }
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            try
            {
                currentdate = daterun.Text;
                currentdate = currentdate.Substring(0, 10);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "Нет задач на сегодня");
            }
        }

        public void TransitionSubscribes(string surname)
        {
            this.surnameLS = surname;
            Browser.WaitUntil(switchOver.Enabled);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(3));
            switchOver.Click(false);
            switchOverSubscriber.Click(false);
            Browser.WaitUntil(inputSurname.Displayed);
            verify = inputSurname.GetAttribute(TagAttributes.Value);
            var verifybool = verify.CompareTo(surnameLS);
            var count = 0;
            while (verifybool != 0 && count < 30)
            {
                Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
                verify = inputSurname.GetAttribute(TagAttributes.Value);
                verifybool = verify.CompareTo(surnameLS);
                count++;
            }
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void TransitionRoom()
        {
            Browser.WaitUntil(switchOver.Enabled);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(3));
            switchOver.Click(false);
            switchOverRoom.Click(false);
            Browser.WaitUntil(inputType.Displayed);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(3));
            verify = inputType.GetAttribute(TagAttributes.Value);
            var verifybool = verify.CompareTo("Жилое");
            var count = 0;
            while (verifybool != 0 && count < 30)
            {
                Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
                verify = inputType.GetAttribute(TagAttributes.Value);
                verifybool = verify.CompareTo("Жилое");
                count++;
            }
            if (count > 29)
            {
                throw new Exception(verifybool+" Невозможно сменить общую площадь в карточке помещения");
            }
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void InformationAboutTheRooms()
        {
            Browser.WaitUntil(infoRoom.Exists());
            infoRoom.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitUntil(openEditInfo.Exists());
            openEditInfo.WaitIsClickable();
            openEditInfo.Click(false);
            Browser.WaitUntil(openDate.Displayed);
            Browser.WaitUntil(openDate.GetAttribute(TagAttributes.Value) != null);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void Ownership()
        {
            changetheValue.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void ChangeTheValueOfTheShare(string datechange, string valuenull)
        {
            var WebElement = new WebElement();
            this.date = datechange;
            this.changeValue = valuenull;
            WebElement.ByName("factDate").Text = date;
            Value.Text = changeValue;
            buttonSaveShare.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            buttonCloseAddWin.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            if (buttonCloseAddWin.Exists())
            {
                buttonCloseAddWin.Click(false);
            }
            buttonSaveEditWin.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitsave));
            buttonCloseEditWin.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(history.Displayed);
        }

        public void ChangeValue()
        {
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            try
            {
                Browser.WaitUntil(changeArea.Enabled);
                changeArea.Click(true);
                Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            }
            catch 
            {
                throw new Exception(" Невозможно сменить общую площадь в карточке помещения");
            }
        }

        public void ChangeValueShare(string datechange, string valueone)
        {
            this.date = datechange;
            this.changeValue = valueone;
            dateFact.Text = date;
            valueShare.Text = changeValue;
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            buttonSaveShare.Click(false);
            buttonSaveCardRoom.Click(true);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            buttonCloseCardRoom.Click(true);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void ChangeDateOpen()
        {
            dateOpen.Click(false);
            var WebElement = new WebElement();
            Browser.WaitUntil(WebElement.ByName("factDate").Displayed);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void ChangeDate(string datefact, string valuechange)
        {
            this.date = datefact;
            this.changeValue = valuechange;
            dateFact.Text = date;
            valueShare.Text = changeValue;
            buttonSaveShare.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void SelectAccChange()
        {
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            checkLS.Click(false);
        }

        public void SelectTheClosedPeriod()
        {
            combobox.Click(false);
            comboboxSelectClose.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void OtherOperation(string operation)
        {
            this.param = operation;
            checkLS.Click(false);
            otherOperation.Click(false);
            try
            {
                menuoperation.Click(false);
                Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Отсутствует кнопка " + param);
            }
        }

        public void OtherOperation_Merge(string operation)
        {
            this.param = operation;
            otherOperation.Click(false);
            try
            {
                menuoperation.Click(false);
                Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Отсутствует кнопка " + param);
            }
        }

        public void SelectOpenState()
        {
            combobox.Click(false);
            comboboxSelectOpen.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            countElement.Click(false);
            count25.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void ShareOfProperty(string datefact, string valuenull)
        {
            this.date = datefact;
            this.changeValue = valuenull;
            changeroom.Click(false);
            dateFact.Text = date;
            valueShare.Text = changeValue;
            buttonSaveShare.Click(false);
            Browser.WaitUntil(MessageSuccessChange.Displayed);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void BackRegistryLS()
        {
            buttonSaveShareOfProperty.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            try
            {
                while (buttonSaveShareOfProperty.Displayed)
                {
                    Thread.Sleep(1500);
                }
            }
            catch
            {
            }
            buttonSaveShareProperty.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            buttonClose.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            registryLS.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void ManualRecalculation(string daterecalc, string valueReason)
        {
            var WebElement = new WebElement();
            this.date = daterecalc;
            this.changeValue = valueReason;
            recalcDateBegin.Text = date;
            reasonRecalc.Text = changeValue;
            Confirm.Click(false);
            Browser.WaitUntil(buttonOK.Displayed);
            try
            {
                Browser.WaitUntil(MessageSuccessRecalc.Displayed);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            buttonOK.Click(false);
        }
        
        public void RegisterUnsubstantiatedCharges()
        {
            mainMenu.Click(false);
            unsubstantiatedcharges.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void GotoPersAcc()
        {
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(viewUnaccepted))).Build().Perform();
            sortUnaccepted.Click(false);
            sortdesc.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            var comparename = compareName.Text;
            var count = 1;
            try
            {
                var compareLs = comparenull.Text;
                while (count < 15)
                {
                    if (comparename == compareLs)
                    {
                        ButtonUpdate.Click(false);
                        Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
                        count++;
                    }
                    else
                    {
                        goto Next;
                    }
                }
            }
            catch
            {
                ButtonUpdate.Click(false);
                Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            }
            Next:
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            editUnaccepted.Click(false);
            try
            {
                Browser.WaitUntil(windowProtocol.Displayed);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "Отсутствует ЛС в неподтвержденных начислениях");
            }
            try
            {
                if (noDate.Text != "Нет данных для отображения")
                {
                    throw new Exception("Отсутствует ЛС в неподтвержденных начислениях");
                }
            }
            catch
            {
            }
        }

        public void UnloadAPaymentDoc()
        {
            unload.Click(false);
            selectelement.Click(false);
        }

        public void ClickChoose()
        {
            buttonChoose.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            try
            {
                if (messageError.Enabled)
                {
                    throw new Exception("Ошибка при выгрузке платежного документа");
                }
            }
            catch
            {
            }
            String[] files = Directory.GetFiles(path).OrderBy(fileList => new FileInfo(fileList).CreationTime).ToArray(); //Сортирует
            size = files.Length;
            if (File.Exists(path))
            {
            }
            else
            {
                if (size == 0)
                {
                    throw new Exception("Файл не скачался");
                }
            }
        }

        public void WindowDownloading()
        {
            mainMenu.Click(false);
            reports.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            replacementreports.Click(false);
            onlineTemplate.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(3));
            String[] files = Directory.GetFiles(path).OrderBy(fileList => new FileInfo(fileList).CreationTime).ToArray(); //Сортирует
            size = files.Length;
            if (File.Exists(path))
            {
            }
            else
            {
                if (size == 0)
                {
                    throw new Exception("Файл не скачался");
                }
            }
        }

        public void NewUser(string kochurani, string kochuradv, string Date)
        {
            var WebElement = new WebElement();
            this.date = Date;
            this.param = kochurani;
            element1 = currentOwner.GetAttribute(TagAttributes.Value);
            newuser.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(checkNewUser.Displayed);
            if (element1 == "КОЧУРА Н И")
            {
                name.Text = "КОЧУРА Д В";
                Browser.WaitForReadyPage(TimeSpan.FromMilliseconds(500));
                name.SendKeys(Keys.Enter);
                Browser.WaitUntil(WebElement.ByXPath("//div[starts-with(@id,'window')]//tr[2]/td[3]/div[text()='Кочура Д В']").Displayed);
                Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            }
            else
            {
                this.param = kochuradv;
                name.Text = "КОЧУРА Н И";
                Browser.WaitForReadyPage(TimeSpan.FromMilliseconds(500));
                name.SendKeys(Keys.Enter);
                Browser.WaitUntil(WebElement.ByXPath("//div[starts-with(@id,'window')]//tr[2]/td[3]/div[text()='КОЧУРА Н И']").Displayed);
                Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            }
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            checkNewUser.Click(false);
            selectNewUser.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            actualFrom.Text = date;
        }

        public void ClickSaveNewOwner()
        {
            buttonSaveNewOwner.Click(false);
            Browser.WaitUntil(buttonOK.Displayed);
            buttonOK.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitUntil(verifyNewUser.Displayed);
            verify = verifyNewUser.Text;
        }
        
        public void NewValue(string valuePenalty1, string valuePenalty2)
        {
            this.param = valuePenalty1;
            element1 = debtPenalty.GetAttribute(TagAttributes.Value);
            while (element1 == "")
            {
                Thread.Sleep(1500);
                element1 = debtPenalty.GetAttribute(TagAttributes.Value);
            }
            newPenalty.Text = param;
            element1 = debtPenalty.GetAttribute(TagAttributes.Value);
            element2 = newPenalty.GetAttribute(TagAttributes.Value);
            valueelement1 = Decimal.Parse(element1.Replace('.', ','));
            valueelement2 = Decimal.Parse(element2.Replace('.', ','));
            result = valueelement1.CompareTo(valueelement2);
            if (result == 0)
            {
                this.param = valuePenalty2;
                newPenalty.Text = param;
            }
        }

        public void ReasonChangePenalty(string changePenalty)
        {
            this.param = changePenalty;
            reasonPenalty.Text = param;
        }

        public void SaveChangePenalty(string numberOfLS, string surname)
        {
            this.numberOfPersAcc = numberOfLS;
            buttonSaveChangePenalty.Click(false);
            Browser.WaitUntil(MessageSuccessChangePenalty.Displayed);
            try
            {
                Browser.WaitUntil(MessageSuccessChangePenalty.Displayed);
            }
            catch (Exception e)
            {
                if (MessageErrorSaldo.Displayed)
                {
                    throw new Exception(e.Message + element1 + " = " + element2);
                }
            }
            buttonOK.Click(false);
            EditPersonalAccount(numberOfLS, surname);
        }
        
        public void NewValueSaldo(string valueSaldo1, string valueSaldo2)
        {
            this.param = valueSaldo1;
            element1 = CurrentSaldo.Text;
            while (element1 == "")
            {
                Thread.Sleep(1500);
                element1 = CurrentSaldo.Text;
            }
            clickNewSaldo.Click(false);
            NewSaldoByBaseTariff.Text = param;
            element2 = clickNewSaldo.Text;
            Reason.Click(false);
            result = element1.CompareTo(element2);
            if (result == 0)
            {
                this.param = valueSaldo2;
                clickNewSaldo.Click(false);
                NewSaldoByBaseTariff.Text = param;
            }
        }

        public void ReasonChangeSaldo(string changeSaldo)
        {
            this.param = changeSaldo;
            Reason.Text = param;
        }

        public void SaveChangeSaldo(string numberOfLS, string surname)
        {
            SaveButton.Click(false);
            Browser.WaitUntil(SuccessSetChangeSaldo.Displayed);
            buttonOK.Click(false);
            EditPersonalAccount(numberOfLS, surname);
        }

        public void ClosingDate(string valuedate)
        {
            this.date = valuedate;
            closingDate.Text = date;
        }

        public void SaveClosingDate(string numberOfLS, string surname)
        {
            buttonSaveClosing.Click(false);
            buttonYes.Click(false);
            Browser.WaitUntil(MessageSuccessRecalc.Displayed);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            buttonOK.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void reOpeningDate(string valuedate)
        {
            this.date = valuedate;
            reOpenDate.Text = date;
        }

        public void SaveOpenDate()
        {
            buttonSaveClosing.Click(false);
            buttonOK.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void TextElements()
        {
            element1 = textLS1.Text;
            element2 = textLS2.Text;
        }

        public void Merger()
        {
            Found:
            var WebElement = new WebElement();
            TextElements();
            int result = element1.CompareTo(element2);
            if (result == 0)
            {
                selectLS1.Click(false);
                selectLS2.Click(false);
                OtherOperation_Merge("Слияние");
                goto Finish;
            }
            else
            {
                do
                {
                    if (ls1 < 25 || ls2 < 26)
                    {
                        if (result != 0)
                        {
                            ls1++;
                            ls2++;
                            TextElements();
                            result = element1.CompareTo(element2);
                        }
                    }
                    else
                    {
                        nextPageGoto.Click(false);
                        Browser.WaitForReadyPage(TimeSpan.FromSeconds(3));
                        ls1 = 2;
                        ls2 = 3;
                        TextElements();
                        result = element1.CompareTo(element2);
                    }
                }
                while (element1.CompareTo(element2) != 0);
                
                selectLS1.Click(false);
                selectLS2.Click(false);
                OtherOperation_Merge("Слияние");
                try
                {
                    if (ownershipnull.Enabled)
                    {
                        buttonOK.Click(false);
                        Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
                        selectLS1.Click(false);
                        selectLS2.Click(false);
                        ls1++;
                        ls2++;
                        goto Found;
                    }
                }
                catch
                {
                }
            }
            Finish:
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            elementint1 = newOwner.Text;
            count = Decimal.Parse(elementint1.Replace('.', ','));
            newOwner.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            inputNewOwner.Text = "0";
            elementint2 = newOwner1.Text;
            sum = count + Decimal.Parse(elementint2.Replace('.', ','));
            newOwner1.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            inputNewOwner.Text = Convert.ToString(sum);
            inputElement.Click(false);
            reason.Text = "Изменение доли собственности в связи со слиянием ЛС";
            element1 = inputnewOwner1.Text;
            element2 = inputnewOwner2.Text;
            buttonSave.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitUntil(buttonOK.Displayed);
            buttonOK.Click(false);
        }

        public void ChangeStatysClose()
        {
            var WebElement = new WebElement();
            combobox.Click(false);
            comboboxSelectNone.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            numberTextfield.Text = element1;
            numberTextfield.SendKeys(Keys.Enter);
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(WebElement.ByXPath("//div[starts-with(@id,'paccountgrid')]//tr[2]/td[3]/div[starts-with(text(),'" + element1 + "')]").Displayed);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            editPersAcc.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            Browser.WaitHideElement(By.XPath(waitpls));
            Browser.WaitUntil(waitOperationsLS.Displayed);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(3));
            var valueAreaShare = areaShare.GetAttribute(TagAttributes.Value);
            
            var result = valueAreaShare.CompareTo("0");
            try
            {
                if (result == 0)
                {
                    verify = "0";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "Доля собственности больше 0, у ЛС после слияния");
            }
        }
        
        public void CheckDateClosedPeriod(string numberOfPersAccaunt, string surname)
        {
            this.numberOfPersAcc = element1;
            this.surnameLS = surname;
            var dateclose = closeDate.GetAttribute(TagAttributes.Value);
            while (dateclose == "")
            {
                Thread.Sleep(1500);
                dateclose = closeDate.GetAttribute(TagAttributes.Value);
            }
            month = DateTime.Now.Month;
            year = DateTime.Now.Year;
            currentdate = "01." + month + "." + year;
            result = currentdate.CompareTo(dateclose);
            try
            {
                if (result == 0)
                {
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "Дата закрытия ЛС неверная");
            }
        }

        public void DocumentBase()
        {
            docBase.Click(false);
            System.Windows.Forms.SendKeys.SendWait(fileload);
            System.Windows.Forms.SendKeys.SendWait(@"{Enter}");
        }

        public void DocumentBaseReCalc()
        {
            Browser.WaitUntil(file.Displayed);
            file.Text = "Паспорт";
        }

        public void SpecifyValue(string sum)
        {
            this.param = sum;
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            baseRate.Text = param;
            rateDecision.Text = param;
            fines.Text = param;
        }

        public void SaveMassCancel()
        {
            buttonSaveMassCancel.Click(false);
            buttonOK.Click(false);
            editPersAcc.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            Browser.WaitHideElement(By.XPath(waitpls));
            Browser.WaitUntil(waitOperationsLS.Displayed);
        }

        public void DateOperation(string dateoperation)
        {
            this.date = dateoperation;
            operationDate.Text = date;
        }

        public void SelectTheTypeOfDistribution()
        {
            typeDestribytion.Click(false);
            evently.Click(false);
        }

        public void SpecifyACertainSum(string summ)
        {
            this.param = summ;
            distributionsum.Text = param;
            spread.Click(false);
            Browser.WaitHideElement(By.XPath("//div[starts-with(@id,'loadmask')]/div[text()='Распределение']"));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void SaveDistribution()
        {
            buttonSaveDistribution.WaitIsClickable();
            buttonSaveDistribution.Click(false);
            Browser.WaitHideElement(By.XPath("//div[starts-with(@id,'loadmask')]/div[text()='Распределение']"));
            Browser.WaitUntil(MessageSuccessDistribution.Displayed);
            buttonOK.Click(false);
            editPersAcc.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            Browser.WaitHideElement(By.XPath(waitpls));
            Browser.WaitUntil(waitOperationsLS.Displayed);
        }

        public void SetTheStartAndEndOfthePeriod(string monthStart, string monthEnd)
        {
            this.param = monthStart;
            dateStart.Click(false);
            monthPicker.Click(false);
            buttomOkMonthPicker.Click(false);
            this.param = monthEnd;
            dateEnd.Click(false);
            monthPicker.Click(false);
            buttomOkMonthPicker.Last().Click(false);
        }

        public void ClickSave()
        {
            buttonSaveBanReCalc.Click(false);
            Browser.WaitHideElement(By.XPath(waitBanReCalc));
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(buttonOK.Displayed);
            buttonOK.Click(false);
        }

        public void GoToHistory()
        {
            history.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            dateValue.Click(false);
            today.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            dateInstallValue.SendKeys(Keys.Enter);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitload));
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(viewValue))).Build().Perform();
            sortValue.Click(false);
            sortdesc.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void GoToTabbarRegFund()
        {
            tabbarRegFund.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitUntil(checkLS.Displayed);
            checkLS.WaitIsClickable();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void ExternalNumberLS(string Date, string ValueExternal)
        {
            this.date = Date;
            this.param = ValueExternal;
            editNumber.WaitIsClickable();
            editNumber.Click(false);
            factdate.Text = date;
            valueFactDate.Text = param;
            buttonSaveExternal.Click(false);
        }

        public void ClickOpenDate(string Date, string DateValue)
        {
            this.date = Date;
            this.param = DateValue;
            dateOpen.Click(false);
            factdate.Text = date;
            valueFactDate.Text = param;
            buttonSaveExternal.Click(false);
        }
    }
}