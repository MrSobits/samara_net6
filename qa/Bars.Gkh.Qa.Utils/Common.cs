namespace Bars.Gkh.Qa.Utils
{
    using System;
    using System.Linq;
    
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.TestToolkit.Fixtures;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    internal sealed class Common : BindingBase
    {
        private static IWindsorContainer parentContainer;

        private static IWindsorContainer childContainer;


        [BeforeTestRun]
        public static void CommonTestsSetup()
        {
            var fixture = new AppContextFixture<GkhContext>();
            BindingBase.Container = ApplicationContext.Current.Container;
        }

        [AfterTestRun]
        public static void CommonTestsTeardown()
        {
            ApplicationContext.Current.Stop();
        }

        [BeforeScenario]
        public static void CommonScenarioSetup()
        {
            parentContainer = BindingBase.Container;

            childContainer = new WindsorContainer();

            Container.AddChildContainer(childContainer);

            BindingBase.Container = childContainer;
            ExplicitSessionScope.EnterNewScope();

            Container.Resolve<ISessionProvider>().GetCurrentSession();
        }

        [AfterScenario]
        public static void CommonScenarioTearDown()
        {
            try
            {
                CommonHelper.CloseCurrentSession();
            }
            finally
            {
                BindingBase.Container = parentContainer;
                Container.RemoveChildContainer(childContainer);

                childContainer.Dispose();
            }
        }

        [BeforeScenario("kamchatka")]
        [BeforeScenario("kostroma")]
        [BeforeScenario("msk")]
        [BeforeScenario("nao")]
        [BeforeScenario("nso")]
        [BeforeScenario("rt")]
        [BeforeScenario("arhangelsk")]
        [BeforeScenario("chelyabinsk")]
        [BeforeScenario("dagestan")]
        [BeforeScenario("hmao")]
        [BeforeScenario("khakasia")]
        [BeforeScenario("nnovgorod")]
        [BeforeScenario("nsogji")]
        [BeforeScenario("saha")]
        [BeforeScenario("sahalin")]
        [BeforeScenario("samara")]
        [BeforeScenario("saratov")]
        [BeforeScenario("smolensk")]
        [BeforeScenario("stavropol")]
        [BeforeScenario("tambov")]
        [BeforeScenario("tomsk")]
        [BeforeScenario("tula")]
        [BeforeScenario("tver")]
        [BeforeScenario("tyumen")]
        [BeforeScenario("volgograd")]
        [BeforeScenario("voronezh")]
        [BeforeScenario("yanao")]
        [BeforeScenario("zabaykalye")]
        public void BeforeRegionScenario()
        {
            var context = (GkhContext)ApplicationContext.Current;

            if (!ScenarioContext.Current.ScenarioInfo.Tags.Contains(context.Region))
            {
                Assert.Ignore();
            }
        }

        [BeforeScenario("ScenarioInTransaction")]
        public void ScenarioInTransactionSetup()
        {
            var transaction = Container.Resolve<IDataTransaction>();
            
            ScenarioContext.Current.Add("transaction", transaction);
        }

        [BeforeScenario("MockUserIdentity")]
        public void MockUserIdentityBeforeScenario()
        {
            var realImpl = parentContainer.Resolve<IUserIdentity>();

            if (realImpl.GetType() != typeof (FakeUserIdentity))
            {
                parentContainer.ReplaceComponent(Component.For<IUserIdentity>().ImplementedBy<FakeUserIdentity>().LifestyleSingleton());
            }

            var fakeIdentity = (FakeUserIdentity)parentContainer.Resolve<IUserIdentity>();

            try
            {
                fakeIdentity.PrevUserIdentity = realImpl;
                fakeIdentity.Mock = true;
            }
            finally
            {
                Container.Release(realImpl);
                Container.Release(fakeIdentity);
            }
        }

        [AfterScenario("MockUserIdentity")]
        public void MockUserIdentityAfterScenario()
        {
            var fakeIdentity = (FakeUserIdentity)parentContainer.Resolve<IUserIdentity>();

            try
            {
                fakeIdentity.Mock = false;
            }
            finally
            {
                Container.Release(fakeIdentity);
            }
        }
    }
}
