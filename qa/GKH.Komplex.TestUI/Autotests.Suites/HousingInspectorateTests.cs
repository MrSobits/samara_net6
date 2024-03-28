using Autotests.Utilities;
using Autotests.WebPages;
using Autotests.WebPages.Root;
using Autotests.WebPages.Root.Housing_Inspectorate;
using NUnit.Framework;

namespace Autotests.Suites
{
    [TestFixture]
    public class HouseInspectorateTests
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

        /// <summary>
        /// Формирование обращения за выдачей лицензии
        /// </summary>
        [Test]
        public void HouseInspectorateTests_01_FormationOfApplying_Create()
        {
           Pages.Menu.GoToAppealToRequestLicense();
           Pages.AppealToRequestLicense.AddRequest();
           Assert.True(Pages.Menu.MessageCompleteSuccess());
        }
        /// <summary>
        /// Формирование проверки по обращению за выдачей лицензии
        /// </summary>
        [Test]
        public void HouseInspectorateTests_02_FormationOfChecking_Create()
        {
            Pages.Menu.GoToAppealToRequestLicense();
            Pages.AppealToRequestLicense.AddInspection();
            Assert.True(Pages.Menu.MessageCompleteSuccess());
        }
        /// <summary>
        /// Создание постановления прокуратуры
        /// </summary>
        [Test]
        public void HouseInspectorateTests_03_ProsecutorsDecision_Create()
        {
            Pages.Menu.GoToResolutionOfProsecutorse();
            Pages.ResolutionOfProsecutors.AddDecision();
            Assert.True(Pages.Menu.MessageCompleteSuccess());
        }
        /// <summary>
        /// Формирование Постановления
        /// </summary>
        [Test]
        public void HouseInspectorateTests_04_FormationOfDecision_Create()
        {
            Pages.Menu.GoToResolutionOfProsecutorse();
            Pages.ResolutionOfProsecutors.FormationDecision();
            Assert.True(Pages.Menu.MessageCompleteSuccess());

        }
        /// <summary>
        /// Формирование представлений
        /// </summary>
        [Test]
        public void HouseInspectorateTests_05_FormationOfRepresentations_Create()
        {
            Pages.Menu.GoToResolutionOfProsecutorse();
            Pages.ResolutionOfProsecutors.FormationOfRepresentations();
            Assert.True(Pages.Menu.MessageCompleteSuccess());

        }
        /// <summary>
        /// Формирование протокола
        /// </summary>
        [Test]
        public void HouseInspectorateTests_06_FormationProtocol_Create()
        {
            Pages.Menu.GoToResolutionOfProsecutorse();
            Assert.True(Pages.ResolutionOfProsecutors.FormationProtocol());
        }
        /// <summary>
        /// Формирование обращение
        /// </summary>
        [Test]
        public void HouseInspectorateTests_07_FormationTreatment_Create()
        {
            Pages.Menu.GoToRegisterOfAplications();
            Pages.RegisterOfAplications.AddApplication();
        }
        /// <summary>
        /// Формирование проверки на основании заявки
        /// </summary>
        [Test]
        public void HouseInspectorateTests_08_FormationOfCheckingBasedOnRequests_Create()
        {
            Pages.Menu.GoToRegisterOfAplications();
            Pages.RegisterOfAplications.FormationOfChecking();
            Assert.True(Pages.Menu.MessageCompleteSuccess());
        }
        /// <summary>
        /// Присвоение номера по исполнительным документам
        /// </summary>
        [Test]
        public void HouseInspectorateTests_09_ExecutiveDocument_NumberAssignment()
        {
            Pages.Menu.GoToBaseStatement();
            Pages.BaseStatement.AssignNumber();
            Assert.True(Pages.Menu.MessageCompleteSuccess());
        }
        /// <summary>
        /// Печать исполнительных документов
        /// </summary>
        [Test]
        public void HouseInspectorateTests_10_ExecutiveDocument_Print()
        {
            Pages.Menu.GoToBaseStatement();
            Pages.BaseStatement.Print();
            Assert.IsTrue(BaseStatement.size.CompareTo(0) != 0);
            Pages.Menu.DeleteFilesFolder();
        }

    }
}