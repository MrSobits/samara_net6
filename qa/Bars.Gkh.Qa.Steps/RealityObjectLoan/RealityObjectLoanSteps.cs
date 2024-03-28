namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Controller.Provider;
    using Bars.B4.DataAccess;
    
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;
    using Bars.GkhCr.Entities;

    using FluentAssertions;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;

    [Binding]
    internal class RealityObjectLoanSteps : BindingBase
    {
        private BaseParams takeLoanBaseParams = new BaseParams();

        private LoanParamProxy loanParamProxy;

        private ControllerProvider controllerProvider = Container.Resolve<ControllerProvider>();

        public RealityObjectLoanSteps()
        {
            this.loanParamProxy = new LoanParamProxy { Loans = new List<LoanProxy>() };
        }

        [Given(@"пользователь формирует займ для дома по адресу ""(.*)"" по текущей программе КР и текущему МО")]
        public void ДопустимПользовательФормируетЗаймДляДомаПоАдресуПоТекущейПрограммеКРИТекущемуМО(string roAddress)
        {
            var actDomain = Container.ResolveDomain<PerformedWorkAct>();
            var actPaymentDomain = Container.ResolveDomain<PerformedWorkActPayment>();

            var paymentAccountDomain = RealityObjectPaymentAccountHelper.DomainService;

            var ro = RealityObjectHelper.GetRoByAddress(roAddress);

            this.takeLoanBaseParams.Params.Add("loanSubject", ro.Id);

            this.takeLoanBaseParams.Params.Add("programCr", ProgramCrHelper.Current.Id);

            var filterActDomain = actDomain.GetAll().Where(x => x.ObjectCr.RealityObject.Id == ro.Id).ToList();

            var actDomainSum = filterActDomain.Sum(x => x.Sum);

            var actPaymentDomainSum =
                actPaymentDomain.GetAll()
                .ToList()
                .Where(x => filterActDomain.Any(y => y.Id == x.PerformedWorkAct.Id))
                .Sum(x => x.Sum);

            paymentAccountDomain.GetAll().Cast<dynamic>().ToList().Where(x => x.RealityObject.Id == ro.Id);

            decimal need = actDomainSum - actPaymentDomainSum ?? 0m;

            this.takeLoanBaseParams.Params.Add("need", need);
        }

        [Given(@"пользователь устанавливает способ формирования займа ""(.*)""")]
        public void ДопустимПользовательУстанавливаетСпособФормированияЗайма(string externalTypeLoanProcess)
        {
            var enumType = Type.GetType("Bars.Gkh.RegOperator.Enums.TypeLoanProcess, Bars.Gkh.RegOperator");

            var typeLoanProcess = EnumHelper.GetFromDisplayValue(enumType, externalTypeLoanProcess);

            if (this.takeLoanBaseParams.Params.Any(x => x.Key == "typeLoanProcess"))
            {
                this.takeLoanBaseParams.Params.Remove("typeLoanProcess");
            }

            this.takeLoanBaseParams.Params.Add("typeLoanProcess", typeLoanProcess);
        }

        [Given(@"пользователь выбирает источник займа ""(.*)""")]
        public void ДопустимПользовательВыбираетИсточникЗайма(string externalTypeSourceLoan)
        {
            var enumType = Type.GetType("Bars.Gkh.RegOperator.Enums.TypeSourceLoan, Bars.Gkh.RegOperator");

            var typeSourceLoan = EnumHelper.GetFromDisplayValue(enumType, externalTypeSourceLoan);

            if (this.takeLoanBaseParams.Params.Any(x => x.Key == "typeSourceLoan"))
            {
                this.takeLoanBaseParams.Params.Remove("typeSourceLoan");
            }

            this.takeLoanBaseParams.Params.Add("typeSourceLoan", typeSourceLoan);
        }

        [Given(@"пользователь у заимодателя c адресом ""(.*)"" устанавливает сумму заимствования ""(.*)""")]
        public void ДопустимПользовательУЗаимодателяCАдресомУстанавливаетСуммуЗаимствования(string roAddress, decimal sum)
        {
            var ro = RealityObjectHelper.GetRoByAddress(roAddress);

            this.loanParamProxy.Loans.Add(new LoanProxy { RealityObject = ro, LoanSum = sum });
        }

        [When(@"пользователь принимает изменения займа")]
        public void ЕслиПользовательПринимаетИзмененияЗайма()
        {
            this.takeLoanBaseParams.Params.WriteInstance(this.loanParamProxy);

            var controllerProvider = new ControllerProvider(Container);

            dynamic realityObjectLoanController =
                controllerProvider.GetController(Container, RealityObjectLoanHelper.Type.Name);

            var result = realityObjectLoanController.TakeLoan(this.takeLoanBaseParams) as JsonNetResult;

            dynamic data = result.Data;

            ScenarioContext.Current.Add("TakeLoanResult", ReflectionHelper.GetPropertyValue(data, "success"));

            this.takeLoanBaseParams = new BaseParams();
        }

        [Then(@"взятие займа прошло успешно")]
        public void ТоВзятиеЗаймаПрошлоУспешно()
        {
            var result = ScenarioContext.Current.Get<bool>("TakeLoanResult");

            result.Should().BeTrue("взятие займа должно пройти успешно");
        }

        private class LoanParamProxy
        {
            public List<LoanProxy> Loans { get; set; }
        }

        private class LoanProxy
        {
            public RealityObject RealityObject { get; set; }

            public decimal LoanSum { get; set; }
        }
    }
}
