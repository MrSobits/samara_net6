using System;
using Autotests.Utilities;
using Autotests.Utilities.Tags;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading;

namespace Autotests.WebPages.Root
{
    public class Dpkr : PageBase
    {
        #region Data
        public static bool verify;
        string param, type, yearstart,yearend, adress, roof, summa, roofs,Years;
        string correct = "//div[starts-with(@id,'b4_state_menu')]//a[starts-with(@id,'menuitem')]/span[text()='Актуальный']";
        string create = "//div[starts-with(@id,'loadmask')]/div[text()='Создание новой версии программы...']";
        string formpr = "//div[starts-with(@id,'loadmask')]/div[text()='Формирование программы...']";
        string save = "//div[starts-with(@id,'loadmask')]/div[text()='Сохранение...']";
        string uncorrect = "//div[starts-with(@id,'b4_state_menu')]//a[starts-with(@id,'menuitem')]/span[text()='Некорректный']";
        string waitAcctual = "//div[starts-with(@id,'loadmask')]/div[text()='Пожалуйста, подождите. Идет актуализация стоимости']";
        string waitAdd = "//div[starts-with(@id,'loadmask')]/div[text()='Пожалуйста, подождите. Идет добавление записей']";
        string waitchange = "//div[starts-with(@id,'loadmask')]/div[text()='Смена статуса']";
        string waitcopy = "//div[starts-with(@id,'loadmask')]/div[text()='Копирование версии...']";
        string waitDelete = "//div[starts-with(@id,'loadmask')]/div[text()='Пожалуйста, подождите. Идет удаление записей']";
        string waitload = "//div[starts-with(@id,'loadmask')]/div[text()='Загрузка...']";
        string waitpls = "//div[starts-with(@id,'ext-gen')]/div[text()='Пожалуйста, подождите']";
        private WebElement actualDpKr { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversioneditpanel')]//div[starts-with(@id,'versionrecordsgrid')]//span[text()='Актуализировать ДПКР']")); } }
        private WebElement address { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realityobjGrid')]/div/div/div/div/div/table/tbody/tr/td[1]/label[text()='Адрес:']/../../td[2]/input")); } }
        private WebElement Agapovskiy { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'boundlist')]/div/ul/li[text()='Агаповский муниципальный район']")); } }
        private WebElement baseVersion { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programthirdstagepanel')]//div[starts-with(@id,'programversionwin')]//tr/td[starts-with(@id,'checkboxfield')]/input")); } }
        private WebElement buttonOk { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]/div/em/button/span[text()='OK']")); } }
        private WebElement buttonSave { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversionwin')]//span[text()='Сохранить']")); } }
        private WebElement buttonSaveCost { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'workpricewindow')]//button[@data-qtip='Сохранить']/span[text()='Сохранить']")); } }
        private WebElement buttonSaveStatys { get { return new WebElement().ByXPath(string.Format("//label[contains(text(),'Для смены статуса нажмите')]/../../div[starts-with(@id,'menu')]/div[starts-with(@id,'button')]/em/button/span[text()='Сохранить']")); } }
        private WebElement calcDPKR { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programthirdstagegrid')]//span[text()='Расчет ДПКР']")); } }
        private WebElement changeStatys { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'structelementgrid')]//tr[contains(@id,'Водоснабжение')]//tbody/tr[2]/td[2]/div/img[@data-qtip='Перевести статус']")); } }
        private WebElement check { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversioneditpanel')]//tr[2]/td[1]/div/div")); } }
        private WebElement clickApply { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversioneditpanel')]//span[text()='Применить']/../span[contains(@id,'Icon')]")); } }
        private WebElement clickCopy { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversionpanel')]/div[starts-with(@id,'versioncopywindow')]//button[@data-qtip='Копировать']/span[text()='Копировать']")); } }
        private WebElement constructCharacter { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realityobjNavigationPanel')]//div[starts-with(@id,'menutreepanel')]//div[text()='Конструктивные характеристики']")); } }
        private WebElement copy { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversiongrid')]//tr[2]/td[3]/div/../../td[2]/div/img")); } }
        private WebElement editRegistry { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realityobjGrid')]//tr[2]/td[1]/div/img[@data-qtip='Редактировать']")); } }
        private WebElement editVersion { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversiongrid')]//tr[2]/td[3]/div[text()='Копия Версия программы']/../../td[1]/div/img[@data-qtip='Редактировать']")); } }
        private WebElement menuItem { get { return new WebElement().ByXPath(string.Format("//a[starts-with(@id,'menuitem')]/span[text()='{0}']", param)); } }
        private WebElement messageType { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/div/p[text()='{0}']", type)); } }
        private WebElement municipalDistrict { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'workpricepanel')]//td[1]/label[text()='Муниципальный район:']/../../td[2]/table/tbody/tr/td[2]/div")); } }
        private WebElement municipality { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programthirdstagepanel')]//input[@name='Municipality']")); } }
        private WebElement name { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programthirdstagepanel')]//div[starts-with(@id,'programversionwin')]//input[@name='Name']")); } }
        private WebElement nameWork { get { return new WebElement().ByXPath(string.Format("//label[text()='Наименование работы:']/../../td[2]/input")); } }
        private WebElement normativeCost { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'workpricewindow')]//input[@name='SquareMeterCost']")); } }
        private WebElement note { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversioneditpanel')]//table/tbody/tr[2]/td[1]/div/div")); } }
        private WebElement ooi { get { return new WebElement().ByXPath(string.Format("//label[text()='Объекты общего имущества:']/../../td[2]/input")); } }
        private WebElement priority { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programthirdstagegrid')]//span[text()='Очередность']")); } }
        private WebElement saveActual { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'versionactualizeperiodwindow')]//button[@data-qtip='Сохранить']/span[text()='Продолжить']")); } }
        private WebElement saveApply { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversioneditpanel')]//button[@data-qtip='Сохранить']/span[text()='Применить']")); } }
        private WebElement saveVersion { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programthirdstagegrid')]//span[text()='Сохранить версию программы']")); } }
        private WebElement SuccessMessage { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'messagebox')]/span[text()='Успешно!']")); } }
        private WebElement SuccessMessageCopy { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/div/p[text()='Версия успешно скопирована']")); } }
        private WebElement SuccessMessageCreate { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/p[text()='Новая версия программы успешно создана.']")); } }
        private WebElement TypeRoof { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'workpricepanel')]//tr/td[2]/div[contains(text(),'Скат')]/../../td[1]/div/img")); } }
        private WebElement Update { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'workpricepanel')]//button[@data-qtip='Обновить']/span[text()='Обновить']")); } }
        private WebElement VersionProgram { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'desktoptopbar')]//span[text()='Версия программы']")); } }
        private WebElement wait { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programthirdstagegrid')]//tr[2]/td[3]/div")); } }
        private WebElement waitAddress { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realityobjGrid')]//tr[2]/td[8]/div[text()='{0}']", adress)); } }
        private WebElement waitCopyVersion { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversiongrid')]//tr[2]/td[3]/div[text()='Копия Версия программы']")); } }
        private WebElement waitPriority { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'currprioritygrid')]//tr[2]/td[1]/div")); } }
        private WebElement waitProgram { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversioneditpanel')]//div[starts-with(@id,'versionrecordsgrid')]//tr[2]/td[3]/div")); } }
        private WebElement waitReg { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realityobjEditPanel')]//input[@name='BuildYear']")); } }
        private WebElement waitstring { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'realityobjGrid')]//tr[2]/td[3]/div/div")); } }
        private WebElement waitVersion { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversiongrid')]//tr[2]/td[3]/div")); } }
        private WebElement waitVersionProgram { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversiongrid')]//tr[2]/td[3]/div[text()='Версия программы']")); } }
        private WebElement Year { get { return new WebElement().ByXPath(string.Format("//label[text()='Год:']/../../td[2]/table/tbody/tr/td/input")); } }
        private WebElement YearEnd { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'versionactualizeperiodwindow')]//tbody[starts-with(@id,'numberfield')]//input[@name='YearEnd']")); } }
        private WebElement YearStart { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'versionactualizeperiodwindow')]//tbody[starts-with(@id,'numberfield')]//input[@name='YearStart']")); } }
        public WebElement SuccessActual { get { return new WebElement().ByXPath(string.Format("//div[@id='msg-div']/div/div/p[text()='Записи актуализированы']")); } }
        #endregion

        #region DataSort
        string date = "//div[starts-with(@id,'programversiongrid')]//span[text()='Дата']";
        private WebElement sortDate { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'programversiongrid')]//span[text()='Дата']/../div")); } }
        private WebElement sortdesc { get { return new WebElement().ByXPath(string.Format("//div[starts-with(@id,'menu')]//a[starts-with(@id,'menuitem')]/span[text()='Сортировать по убыванию']")); } }
        #endregion

        public void WaitLoadLongTerm()
        {
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitUntil(wait.Displayed);
        }

        public void WaitLoadVersion()
        {
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitUntil(waitVersion.Displayed);
        }

        public void EditVersions()
        {
            WaitLoadVersion();
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(date))).Build().Perform();
            sortDate.Click(false);
            sortdesc.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            editVersion.Click(false);
            Browser.WaitUntil(waitProgram.Displayed);
        }

        public void CalculateDPKR()
        {
            WaitLoadLongTerm();
            while (municipality.GetAttribute(TagAttributes.Value) == "")
            {
                Thread.Sleep(2000);
            }
            calcDPKR.Click(false);
            Browser.WaitHideElement(By.XPath(formpr));
            try
            {
                verify = Browser.WaitUntil(SuccessMessage.Displayed);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Формирование программы закончилось не успешно");
            }
            Browser.WaitUntil(buttonOk.Enabled);
            buttonOk.Click(false);
        }

        public void ClickPriority()
        {
            WaitLoadLongTerm();
            priority.Click(false);
            try
            {
                verify = Browser.WaitUntil(waitPriority.Displayed);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Очередность не появилась");
            }
        }

        public void ClickSaveVersion()
        {
            WaitLoadLongTerm();
            while (municipality.GetAttribute(TagAttributes.Value) == "")
            {
                Thread.Sleep(2000);
            }
            saveVersion.Click(false);
        }

        public void SpecifyName(string VersionProgram)
        {
            this.param = VersionProgram;
            Browser.WaitUntil(name.Enabled);
            name.Text = param;
            Browser.WaitUntil(baseVersion.Enabled);
            baseVersion.Click(false);
            buttonSave.Click(false);
            Browser.WaitHideElement(By.XPath(create));
            try
            {
                Browser.WaitUntil(SuccessMessageCreate.Exists());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Новая версия для программы не была создана");
            }
        }

        public void NewBaseVersion()
        {
            WaitLoadVersion();
            Actions builder = new Actions(Browser.WebDriver);
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(date))).Build().Perform();
            sortDate.Click(false);
            sortdesc.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            try
            {
                verify = Browser.WaitUntil(waitVersionProgram.Exists());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Отсутсвует версия программы");
            }
        }

        public void ClickButtonCopy()
        {
            NewBaseVersion();
            copy.Click(false);
        }

        public void ClickCopy()
        {
            Browser.WaitUntil(clickCopy.Enabled);
            clickCopy.Click(false);
            Browser.WaitHideElement(By.XPath(waitcopy));
            Browser.WaitUntil(SuccessMessageCopy.Displayed);
            try
            {
                verify = Browser.WaitUntil(waitCopyVersion.Enabled);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Отсутствует копия версии программы");
            }
        }

        public void InitiateTheRemovalRecords(string delete, string startyear,string endyear, string stringDelete)
        {
            this.param = delete;
            this.yearstart = startyear;
            this.yearend = endyear;
            this.type = stringDelete;
            WaitLoadVersion();
            Actions builder = new Actions(Browser.WebDriver);
            EditVersions();
            Pages.Menu.GotoRegistryOfHouses();
            Browser.WaitHideElement(By.XPath(waitload));
            editRegistry.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(constructCharacter.Enabled);
            constructCharacter.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(changeStatys.Enabled);
            changeStatys.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(uncorrect))).Build().Perform();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            buttonSaveStatys.Click(false);
            Browser.WaitHideElement(By.XPath(waitchange));
            Browser.WaitUntil(buttonOk.Enabled);
            buttonOk.Click(false);
            VersionProgram.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(actualDpKr.Enabled);
            actualDpKr.Click(false);
            menuItem.Click(false);
            YearStart.Text = yearstart;
            YearEnd.Text = yearend;
            saveActual.Click(false);
            Browser.WaitHideElement(By.XPath(waitpls));
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(check.Enabled);
            check.Click(false);
            saveApply.Click(false);
            Browser.WaitHideElement(By.XPath(waitDelete));
            try
            {
                verify = Browser.WaitUntil(messageType.Displayed);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Записи не удалены");
            }
        }

        public void InitiateTheAddRecords(string add, string startyear, string stringAdd)
        {
            this.param = add;
            this.yearstart = startyear;
            this.type = stringAdd;
            WaitLoadVersion();
            Actions builder = new Actions(Browser.WebDriver);
            EditVersions();
            Pages.Menu.GotoRegistryOfHouses();
            Browser.WaitHideElement(By.XPath(waitload));
            editRegistry.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(constructCharacter.Enabled);
            constructCharacter.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(changeStatys.Enabled);
            changeStatys.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            builder.MoveToElement(Browser.WebDriver.FindElement(By.XPath(correct))).Build().Perform();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            buttonSaveStatys.Click(false);
            Browser.WaitHideElement(By.XPath(waitchange));
            Browser.WaitUntil(buttonOk.Enabled);
            buttonOk.Click(false);
            VersionProgram.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(actualDpKr.Enabled);
            actualDpKr.Click(false);
            menuItem.Click(false);
            YearStart.Text = yearstart;
            saveActual.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitpls));
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(check.Enabled);
            check.Click(false);
            saveApply.Click(false);
            Browser.WaitHideElement(By.XPath(waitAdd));
            try
            {
                verify = Browser.WaitUntil(messageType.Displayed);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Записи не были добавлены");
            }
        }

        public void InitializateRoof(string Roof, string Address, string year, string Roofs,string sum, string actual, string startyear, string endyear, string Actual)
        {
            this.roof = Roof;
            this.adress = Address;
            this.Years = year;
            this.roofs = Roofs;
            this.summa = sum;
            this.param = actual;
            this.yearstart = startyear;
            this.yearend = endyear;
            this.type = Actual;
            EditVersions();
            Actions builder = new Actions(Browser.WebDriver);
            ooi.Text = roof;
            ooi.SendKeys(Keys.Enter);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitHideElement(By.XPath(waitload));
            Pages.Menu.GotoRegistryOfHouses();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            Browser.WaitUntil(address.Displayed);
            Browser.WaitHideElement(By.XPath(waitload));
            address.Text = adress;
            address.SendKeys(Keys.Enter);
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(waitAddress.Displayed);
            Browser.WaitUntil(editRegistry.Displayed);
            editRegistry.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(waitReg.Displayed);
            Pages.Menu.GotoPriceOnWork();
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(municipalDistrict.Displayed);
            municipalDistrict.Click(false);
            Agapovskiy.Click(false);
            Year.Text = Years;
            Update.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            nameWork.Text = roofs;
            Update.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            TypeRoof.Click(false);
            Browser.WaitUntil(normativeCost.Enabled);
            normativeCost.Text = summa;
            buttonSaveCost.Click(false);
            Browser.WaitHideElement(By.XPath(save));
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            VersionProgram.Click(false);
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(actualDpKr.Enabled);
            actualDpKr.Click(false);
            Browser.WaitUntil(menuItem.Enabled);
            menuItem.Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(2));
            YearStart.Text = yearstart;
            YearEnd.Text = yearend;
            saveActual.Click(false);
            Browser.WaitHideElement(By.XPath(waitpls));
            Browser.WaitHideElement(By.XPath(waitload));
            Browser.WaitUntil(note.Enabled);
            note.Click(false);
            verify = Browser.WaitUntil(clickApply.Enabled);
            clickApply.Click(false);
            Browser.WaitHideElement(By.XPath(waitAcctual));
            Browser.WaitUntil(messageType.Displayed);
        }
    }
}