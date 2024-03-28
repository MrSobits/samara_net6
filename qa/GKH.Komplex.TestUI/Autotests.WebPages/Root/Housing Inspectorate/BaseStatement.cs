using System;
using System.IO;
using System.Linq;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Autotests.WebPages.Root.Housing_Inspectorate
{
    public class BaseStatement : PageBase
    {
        public static int size;
        private static string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"..\..\..\DownloadFiles";
        #region Data
        private WebElement AddButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Добавить']/..")); } }
        private WebElement ChooseButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Выбрать']")); } }
        private WebElement TodayButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Сегодня']")); } }
        private WebElement SaveButton { get { return new WebElement().ByXPath(string.Format("//button[@data-qtip='Сохранить']")).Last(); } }
        private WebElement editButton { get { return new WebElement().ByXPath(string.Format("//img[@src='/stable-chelyabinsk/content/img/icons/pencil.png']")); } }
        private WebElement generateInspection { get { return new WebElement().ByXPath(string.Format("//span[text()='Сформировать проверку']/..")); } }
        private WebElement checkBox { get { return new WebElement().ByXPath(string.Format("//div[@class='x-grid-row-checker']")); } }
        private WebElement acceptButton { get { return new WebElement().ByXPath(string.Format("//span[text()='Применить']/..")); } }

        #endregion

        public void AssignNumber()
        {
            var WebElement = new WebElement();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(0));
            WebElement.ByXPath("//*[text()='Номер']").Click(false);
            WebElement.ByXPath("//*[text()='Номер']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(3));
            editButton.First().Click(false);
            WebElement.ByXPath("//*[text()='Распоряжение']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(1));
            WebElement.ByXPath("//span[text()='Черновик']/..").Click(false);
            WebElement.ByXPath("//span[text()='Зарегистрирован']").Click(false);

        }

        public void Print()
        {
            var WebElement = new WebElement();
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(0));
            WebElement.ByXPath("//*[text()='Номер']").Click(false);
            WebElement.ByXPath("//*[text()='Номер']").Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(3));
            editButton.First().Click(false);
            Browser.WaitForReadyPage(TimeSpan.FromSeconds(3));
            WebElement.ByXPath("//*[contains(text(),'Распоряжение')]").Click(false);
            WebElement.ByXPath("//*[text()='Печать']/..").Click(false);
            WebElement.ByXPath("//*[text()='Распоряжение']").Last().Click(false);
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

    }
}