namespace Bars.Gkh.Qa.Steps
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class SuspenseAccountSteps : BindingBase
    {
        [Given(@"пользователь выбирает счёт НВС с типом ""(.*)"" и Назначением платежа ""(.*)""")]
        public void ДопустимПользовательВыбираетСчётНВССТипомИНазначениемПлатежа(string moneyDirectionName, string details)
        {
            var moneyDirection = EnumHelper.GetFromDisplayValue<MoneyDirection>(moneyDirectionName);

            var saList = Container.Resolve<IDomainService<SuspenseAccount>>().GetAll();

            var sa = saList.FirstOrDefault(x => x.DetailsOfPayment == details && x.MoneyDirection == moneyDirection);

            if (sa == null)
            {
                throw new SpecFlowException(
                    string.Format(
                        "Отсутствует счёт НВС с типом {0} и назначением платежа {1}",
                        moneyDirectionName,
                        details));
            }

            SuspenseAccountHelper.Current = sa;
        }

        [Given(@"пользователь в реестре счетов НВС нажимает кнопку Зачислить")]
        public void ДопустимПользовательВРеестреСчетовНВСНажимаетКнопкуЗачислить()
        {
            ScenarioContext.Current["DistributionBaseParams"] = new BaseParams
                                                                    {
                                                                        Files =
                                                                            new Dictionary<string, FileData>(
                                                                            ),
                                                                        Params =
                                                                            new DynamicDictionary
                                                                                {
                                                                                    {
                                                                                        "distributionSource",
                                                                                        "20"
                                                                                    },
                                                                                    {
                                                                                        "distributionId",
                                                                                        SuspenseAccountHelper.Current.Id
                                                                                    }

                                                                                }
                                                                    };

            ScenarioContext.Current["DistributionSum"] = SuspenseAccountHelper.Current.Sum;
        }

        [Then(@"у этого счёта НВС заполнено поле Статус ""(.*)""")]
        public void ТоУЭтогоСчётаНВСЗаполненоПолеСтатус(string displayedDistributeState)
        {
            var distributionState = EnumHelper.GetFromDisplayValue<DistributionState>(displayedDistributeState);

            SuspenseAccountHelper.Current.DistributeState.Should()
                .Be(
                    distributionState,
                    string.Format("у этого счёта НВС статус должен быть {0}", displayedDistributeState));
        }
    }
}
