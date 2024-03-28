using System;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;
using System.IO;

namespace Autotests.WebPages.Root
{
    public class Menu : PageBase
    {
        #region Menu
        string fileName = "тестовый файл.txt";
        string waitload = "//div[starts-with(@id,'loadmask')]/div[text()='Загрузка...']";
        private WebElement mainMenu { get { return new WebElement().ByXPath(string.Format("//button[@data-qtip='Главное меню']/span/span")); } }
        #endregion

        #region Menu Left (First Level item)
        private WebElement administration { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menufirstlevelitem')]/span[text()='Администрирование']")); } }
        private WebElement HandBook { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mainmenu')]//span[text()='Справочники']"));} }
        private WebElement housingFund { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mainmenu')]//div[starts-with(@id,'menufirstlevelitem')]/span[text()='Жилищный фонд']"));} }
        private WebElement kr { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menufirstlevelitem')]/span[text()='Капитальный ремонт']"));} }
        private WebElement regionalFund { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menufirstlevelitem')]/div/span[text()='Региональный фонд']")); } }
        private WebElement Report { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menufirstlevelitem')]/div/span[text()='Отчеты']")); } }
        private WebElement task { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mainmenu')]//div[starts-with(@id,'menufirstlevelitem')]/span[text()='Задачи']"));} }
        #endregion

        #region Menu Right
        private WebElement appealToRequestLicense { get { return new WebElement().ByXPath(string.Format("//*[text()='Обращения за выдачей лицензии']")); } }
        private WebElement Banking { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mainmenu')]//div[starts-with(@id,'menuright')]//div[text()='Банковские операции']")); } }
        private WebElement BaseStatement { get { return new WebElement().ByXPath(string.Format("//div[text()='Проверки по обращениям граждан']")); } }
        private WebElement calcMonth { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//span[text()='Расчетный месяц']")); } }
        private WebElement calculatedMonth { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mainmenu')]//div[starts-with(@id,'menuright')]//span[text()='Расчетный месяц']")); } }
        private WebElement checkAndCloseTheMonth { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Проверка и закрытие месяца']")); } }
        private WebElement closeMonth { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Проверка и закрытие месяца']")); } }
        private WebElement compaundingPeriods { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Периоды начислений']")); } }
        private WebElement documents { get { return new WebElement().ByXPath(string.Format("//span[text()='Документы']")); } }
        private WebElement houses { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mainmenu')]//div[starts-with(@id,'menuright')]//div[text()='Реестр жилых домов']")); } }
        private WebElement housingInspection { get { return new WebElement().ByXPath(string.Format("//span[text()='Жилищная инспекция']")); } }
        private WebElement import { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mainmenu')]//div[starts-with(@id,'menuright')]//span[text()='Импорты']")); } }
        private WebElement importPayments { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Реестр настроек импорта оплат (txt,csv)']")); } }
        private WebElement invoice { get { return new WebElement().ByXPath(string.Format("//div[@role='presentation']/div/div/span[text()='Счета']")); } }
        private WebElement longtermProgram { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Долгосрочная программа']")); } }
        private WebElement msgIconSuccess { get { return new WebElement().ByXPath(string.Format("//span[@class='b4-msg-icon-success']")); } }
        private WebElement objectcr { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Объекты капитального ремонта']")); } }
        private WebElement panelReports { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Панель отчетов']")); } }
        private WebElement paramKr { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mainmenu')]//div[starts-with(@id,'menuright')]//span[text()='Параметры программы капитального ремонта']")); } }
        private WebElement priceWork { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Расценки по работам']")); } }
        private WebElement ProgramKR { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Программы капитального ремонта']")); } }
        private WebElement ReasonsForReviewGrid { get { return new WebElement().ByXPath(string.Format("//span[text()='Основания проверок']")); } }
        private WebElement registerOfAplications { get { return new WebElement().ByXPath(string.Format("//div[text()='Реестр обращений']")); } }
        private WebElement registerOfAplicationsGrid { get { return new WebElement().ByXPath(string.Format("//span[text()='Реестр обращений']")); } }
        private WebElement registryFees { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'mainmenu')]//div[starts-with(@id,'menuright')]//div[text()='Реестр оплат платежных агентов']")); } }
        private WebElement registryPersAcc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Реестр лицевых счетов']")); } }
        private WebElement registryUnsubstantiatedCharges { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Реестр неподтвержденных начислений']")); } }
        private WebElement regprogram { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//span[text()='Региональная программа']")); } }
        private WebElement resolutionOfProsecutors { get { return new WebElement().ByXPath(string.Format("//div[text()='Постановления прокуратуры']")); } }
        private WebElement subsides { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Субсидирование']")); } }
        private WebElement taskList { get { return new WebElement().ByXPath((string.Format("//div[starts-with(@id,'mainmenu')]//div[starts-with(@id,'menuright')]//div[text()='Список задач']"))); } }
        private WebElement versionProgram { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menuright')]//div[text()='Версии программы']")); } }
        #endregion

        public void DeleteFilesFolder()
        {
            var path1 = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\";
            var pathDirectory = Path.GetFullPath(string.Format(@"{0}\DownloadFiles", path1));
            Directory.Delete(pathDirectory, true);
            Directory.CreateDirectory(pathDirectory);
            File.Create(pathDirectory + "\\" + fileName);
        }

        public void GoToAppealToRequestLicense()
        {
            mainMenu.Click(false);
            housingInspection.Click(false);
            appealToRequestLicense.Click(false);
        }
        
        public void GoToBanking()
        {
            mainMenu.Click(false);
            regionalFund.Click(false);
            invoice.Click(false);
            Banking.Click(false);
        }

        public void GoToBaseStatement()
        {
            mainMenu.Click(false);
            housingInspection.Click(false);
            ReasonsForReviewGrid.Click(false);
            BaseStatement.Click(false);
        }
        
        public void GotoCalculatedMonth()
        {
            mainMenu.Click(false);
            regionalFund.Click(false);
            calculatedMonth.Click(false);
            checkAndCloseTheMonth.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void GotoCloseMonth()
        {
            mainMenu.Click(false);
            regionalFund.WaitIsClickable();
            regionalFund.Click(false);
            calcMonth.Click(false);
            closeMonth.Click(false);
        }

        public void GotoCompaundingPeriod()
        {
            mainMenu.Click(false);
            regionalFund.Click(false);
            compaundingPeriods.Click(false);
        }

        public void GotoInvoice()
        {
            mainMenu.Click(false);
            regionalFund.Click(false);
            invoice.Click(false);
        }

        public void GotoLongTermProgram()
        {
            mainMenu.Click(false);
            kr.Click(false);
            regprogram.Click(false);
            longtermProgram.Click(false);
        }

        public void GotoObjectKr()
        {
            mainMenu.Click(false);
            objectcr.Click(false);
        }

        public void GotoPanelReport()
        {
            mainMenu.Click(false);
            Report.Click(false);
            panelReports.Click(false);
        }

        public void GotoPriceOnWork()
        {
            mainMenu.Click(false);
            HandBook.Click(false);
            priceWork.Click(false);
        }

        public void GotoProgramKR()
        {
            mainMenu.Click(false);
            kr.Click(false);
            ProgramKR.Click(false);
        }

        public void GoToRegisterOfAplications()
        {
            mainMenu.Click(false);
            housingInspection.Click(false);
            registerOfAplicationsGrid.Click(false);
            registerOfAplications.Click(false);
        }
        
        public void GotoRegistryFeesPaymentsAgents()
        {
            mainMenu.Click(false);
            regionalFund.Click(false);
            invoice.Click(false);
            registryFees.Click(false);
        }

        public void GotoRegistryOfHouses()
        {
            mainMenu.Click(false);
            housingFund.Click(false);
            houses.Click(false);
        }

        public void GotoRegistryOfUnsubstantiatedCharges()
        {
            GotoInvoice();
            registryUnsubstantiatedCharges.Click();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
        }

        public void GotoRegistryPersAcc()
        {
            registryPersAcc.Click(false);
        }
        
        public void GotoRegistrySettingImportPayments()
        {
            mainMenu.Click(false);
            administration.Click(false);
            import.Click(false);
            importPayments.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
        }
        
        public void GotoReport()
        {
            mainMenu.Click(false);
            panelReports.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
        }

        public void GoToResolutionOfProsecutorse()
        {
            mainMenu.Click(false);
            housingInspection.Click(false);
            documents.Click(false);
            resolutionOfProsecutors.Click(false);
        }

        public void GotoSubsides()
        {
            mainMenu.Click(false);
            kr.Click(false);
            regprogram.Click(false);
            subsides.Click(false);
        }

        public void GotoTaskList()
        {
            mainMenu.Click(false);
            task.Click(false);
            taskList.Click(false);
        }

        public void GotoVersionProgram()
        {
            mainMenu.Click(false);
            kr.Click(false);
            paramKr.Click(false);
            versionProgram.Click(false);
        }

        public bool MessageCompleteSuccess()
        {
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            return Browser.WaitUntil(msgIconSuccess.Displayed);
        }

        public void RegLS()
        {
            mainMenu.Click(false);
            regionalFund.Click(false);
        }
    }
}