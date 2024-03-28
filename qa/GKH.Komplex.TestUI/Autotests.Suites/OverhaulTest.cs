using Autotests.Utilities;
using Autotests.WebPages;
using Autotests.WebPages.Root;
using NUnit.Framework;

namespace Autotests.Suites
{
    [TestFixture]
    public class OverhaulTest
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
        string actual = "Актуализировать стоимость";
        string actualcost = "Актуализировать стоимость";
        string add = "Добавить новые записи";
        string address = "с. Агаповка, ул. Рабочая, д. 37";
        string CalcCollection = "Расчет собираемости произведен успешно";
        string CalcIndication = "Расчет показателей произведен успешно";
        string CalcProgram = "Программа успешно опубликована!";
        string delete = "Удалить лишние записи";
        string minnumber = "5";
        string number = "10";
        string regnumber = "50000000";
        string roof = "Крыша";
        string roofs = "крыш";
        string stringActual = "Записи актуализированы";
        string stringAdd = "Записи добавлены";
        string stringDelete = "Записи удалены";
        string summa = "1500";
        string test = "Тестовая программа";
        string VersionProgram = "Версия программы";
        string year = "2015";
        string yearend = "2030";
        string yearstarts = "2014";
        #endregion

        /// <summary>
        /// 2 Модуль «Капитальный ремонт»
        /// <para>
        /// Операции по созданию ДПКР
        /// Расчет долгосрочной программы капитального ремонта
        /// </para>
        /// </summary>
        [Test]
        public void TestKR_01_CalculationLongTermProgram()
        {
            Pages.Menu.GotoLongTermProgram();
            Pages.Dpkr.CalculateDPKR();
            Assert.IsTrue(Dpkr.verify == true);
        }
        /// <summary>
        /// <para>
        /// Расчет очередности
        /// </para>
        /// </summary>
        [Test]
        public void TestKR_02_CalculateOfPriority()
        {
            Pages.Menu.GotoLongTermProgram();
            Pages.Dpkr.ClickPriority();
            Assert.IsTrue(Dpkr.verify == true);
        }
        /// <summary>
        /// <para>
        /// Создание версии программы ДПКР
        /// </para>
        /// </summary>
        [Test]
        public void TestKR_03_CreateVersionProgramDPKR()
        {
            Pages.Menu.GotoLongTermProgram();
            Pages.Dpkr.ClickSaveVersion();
            Pages.Dpkr.SpecifyName(VersionProgram);
            Pages.Menu.GotoVersionProgram();
            Pages.Dpkr.NewBaseVersion();
            Assert.IsTrue(Dpkr.verify == true);
        }
        /// <summary>
        /// <para>
        /// Копирование версии ДПКР
        /// </para>
        /// </summary>
        [Test]
        public void TestKR_04_CopyVersionDPKr()
        {
            Pages.Menu.GotoVersionProgram();
            Pages.Dpkr.ClickButtonCopy();
            Pages.Dpkr.ClickCopy();
            Assert.IsTrue(Dpkr.verify == true);
        }
        /// <summary>
        /// <para>
        /// Операции по актуализации ДПКР
        /// Актуализация ДПКР (удаление лишних записей)
        /// </para>
        /// </summary>
        [Test]
        public void TestKR_05_ActualizationDPKR_RemoveRecords()
        {
            Pages.Menu.GotoVersionProgram();
            Pages.Dpkr.InitiateTheRemovalRecords(delete, yearstarts, yearend, stringDelete);
            Assert.IsTrue(Dpkr.verify == true);
        }
        /// <summary>
        /// <para>
        /// Актуализация ДПКР (добавление новых записей)
        /// </para>
        /// </summary>
        [Test]
        public void TestKR_06_ActualizationDPKR_AddingRecords()
        {
            Pages.Menu.GotoVersionProgram();
            Pages.Dpkr.InitiateTheAddRecords(add, yearstarts, stringAdd);
            Assert.IsTrue(Dpkr.verify == true);
        }
        /// <summary>
        /// <para>
        /// Актуализация ДПКР (актуализация стоимость)
        /// </para>
        /// </summary>
        [Test]
        public void TestKR_07_ActualizationDPKR_ActualizationRate()
        {
            Pages.Menu.GotoVersionProgram();
            Pages.Dpkr.InitializateRoof(roof, address, year, roofs, summa, actual, yearstarts, yearend, stringActual);
            Assert.IsTrue(Pages.Dpkr.SuccessActual.Displayed);
        }
        /// <summary>
        /// <para>
        /// Операции по расчету Субсидирования и Публикации программы ДПКР
        /// Расчет плановой собираемости
        /// </para>
        /// </summary>
        [Test]
        public void TestKR_08_CalcPlan()
        {
            Pages.Menu.GotoSubsides();
            Pages.Subsidy.CalculateTheCollection(CalcCollection);
            Assert.IsTrue(Subsidy.verify == true);
        }
        /// <summary>
        /// <para>
        /// Расчет показателей субсидирования
        /// </para>
        /// </summary>
        [Test]
        public void TestKR_09_TheCalculationOfSubsidyRates()
        {
            Pages.Menu.GotoSubsides();
            Pages.Subsidy.CompleteThePlannedCollection(number);
            Pages.Subsidy.MinimumSizeFundReg(minnumber);
            Pages.Subsidy.RegionalBudget(regnumber);
            Pages.Subsidy.CalculateIndicators(CalcIndication);
            Assert.IsTrue(Subsidy.verify == true);
        }
        /// <summary>
        /// <para>
        /// Публикация программы КР
        /// </para>
        /// </summary>
        [Test]
        public void TestKR_10_PublicationKRProgram()
        {
            Pages.Menu.GotoSubsides();
            Pages.Subsidy.versionforpublication(CalcProgram);
            Assert.IsTrue(Subsidy.verify == true);
        }
        /// <summary>
        /// <para>
        /// Операции по расчету и работе с КПКР
        /// Создание КПКР на основе ДПКР
        /// </para>
        /// </summary>
        [Test]
        public void TestKR_11_CreatingKPKRBasedDPKR()
        {
            Pages.Menu.GotoProgramKR();
            Pages.Programcr.Create();
            Pages.Programcr.FillingDate(test);
            Pages.Programcr.ClickSave();
            Pages.Programcr.ObjectCr(test);
            Assert.IsTrue(Programcr.verify == true);
        }
    }
}
