namespace Bars.Gkh.Qa.Steps
{
    using Bars.B4;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using FluentAssertions;

    [Binding]
    internal class LoginSteps : BindingBase
    {
        [BeforeFeature("login")]
        public static void FeatureSetup()
        {
        }

        [AfterFeature("login")]
        public static void FeatureTeardown()
        {
        }

        [Given(@"Есть пользователь с логином ""(.*)"" и паролем ""(.*)""")]
        public void ДопустимЕстьПользовательСЛогиномИПаролем(string login, string password)
        {
            LoginHelper.CreateUserPassword(login, password);
        }


        [Given(@"пользователь зашёл под логином ""(.*)"" и паролем ""(.*)""")]
        public void ДопустимПользовательЗашёлПодЛогиномИПаролем(string login, string password)
        {
            var result = Container.Resolve<IAuthenticationService>().Authenticate(login, password);

            ScenarioContext.Current.Add("AuthenticationResult", result);
        }

        [Then(@"аутентификация успешна")]
        public void ТоАутентификацияУспешна()
        {
            var result = ScenarioContext.Current.Get<AuthenticationResult>("AuthenticationResult");
            result.Success.Should().BeTrue("Пользователь должен аутентифицироваться");
        }

        [Then(@"аутентификация не успешна")]
        public void ТоАутентификацияНеУспешна()
        {
            var result = ScenarioContext.Current.Get<AuthenticationResult>("AuthenticationResult");
            result.Success.Should().BeFalse("Пользователь не должен аутентифицироваться");
        }
    }
}
