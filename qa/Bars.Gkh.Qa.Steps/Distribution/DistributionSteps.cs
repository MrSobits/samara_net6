namespace Bars.Gkh.Qa.Steps.Distribution
{
    using System.Runtime.CompilerServices;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Distribution;

    using TechTalk.SpecFlow;

    using ExceptionHelper = Bars.Gkh.Qa.Steps.ExceptionHelper;

    [Binding]
    internal class DistributionSteps : BindingBase
    {
        [Given(@"пользователь у этой операции для распределения выбирает Тип распределения ""(.*)""")]
        public void ДопустимПользовательУЭтойОперацииДляРаспределенияВыбираетТипРаспределения(string distributionType)
        {
            string code;

            var baseParams = ScenarioContext.Current.Get<BaseParams>("DistributionBaseParams");

            switch (distributionType)
            {
                case "Субсидия фонда":
                    {
                        code = "FundSubsidyDistribution";

                        break;
                    }

                case "Региональная субсидия":
                    {
                        code = "RegionalSubsidyDistribution";

                        break;
                    }

                case "Стимулирующая субсидия":
                    {
                        code = "StimulateSubsidyDistribution";

                        break;
                    }

                case "Целевая субсидия":
                    {
                        code = "TargetSubsidyDistribution";

                        break;
                    }

                case "Иные источники поступления":
                    {
                        code = "OtherSourcesDistribution";

                        break;
                    }

                case "Поступление процентов банка":
                    {
                        code = "BankPercentDistribution";

                        break;
                    }

                case "Платеж КР":
                    {
                        code = "TransferCrDistribution";

                        break;
                    }

                case "Ранее накопленные средства":
                    {
                        code = "AccumulatedFundsDistribution";

                        break;
                    }

                case "Средства за ранее выполненные работы":
                    {
                        code = "PreviousWorkPaymentDistribution";

                        break;
                    }

                case "Поступление оплат аренды":
                    {
                        code = "RentPaymentDistribution";

                        break;
                    }

                case "Оплата по мировому соглашению":
                    {
                        code = "RestructAmicableAgreementDistribution";

                        break;
                    }

                case "Возврат субсидии фонда":
                    {
                        code = "RefundFundSubsidyDistribution";

                        break;
                    }
                case "Возврат региональной субсидии":
                    {
                        code = "RefundRegionalSubsidyDistribution";

                        break;
                    }
                case "Возврат стимулирующей субсидии":
                    {
                        code = "RefundStimulateSubsidyDistribution";

                        break;
                    }

                case "Возврат целевой субсидии":
                    {
                        code = "RefundTargetSubsidyDistribution";

                        break;
                    }
                case "Оплата акта":
                    {
                        code = "PerformedWorkActsDistribution";

                        break;
                    }

                case "Возврат средств":
                    {
                        code = "RefundDistribution";

                        break;
                    }

                case "Возврат МСП":
                    {
                        code = "RefundMspDistribution";

                        break;
                    }

                case "Возврат пени":
                    {
                        code = "RefundPenaltyDistribution";

                        break;
                    }

                case "Комиссия за ведение счета кредитной организацией":
                    {
                        code = "ComissionForAccountServiceDistribution";

                        break;
                    }

                case "Оплата заявки":
                    {
                        code = "TransferContractorDistribution";

                        break;
                    }

                default:
                    {
                        throw new SpecFlowException("нет такого типа распределения");
                    }
            }

            baseParams.Params["code"] = code;
        }

        [Given(@"пользователь в этом распределении выбирает ЛС ""(.*)""")]
        public void ДопустимПользовательВЭтомРаспределенииВыбираетЛС(string persAccNumber)
        {
            var baseParams = ScenarioContext.Current.Get<BaseParams>("DistributionBaseParams");

            var recordsList = new[]
                                  {
                                      new DynamicDictionary
                                          {
                                              { "AccountId", BasePersonalAccountHelper.Current.Id },
                                              { "Sum", ScenarioContext.Current["DistributionSum"] },
                                              { "SumPenalty", 0 }
                                          }
                                  };

            baseParams.Params["records"] = recordsList;

            ScenarioContext.Current["DistributionSum"] = 0;
        }

        [When(@"пользователь применяет это распределение")]
        public void ЕслиПользовательПрименяетЭтоРаспределение()
        {
            var baseParams = ScenarioContext.Current.Get<BaseParams>("DistributionBaseParams");
            dynamic provider = Container.Resolve<IDistributionProvider>();
            var result = (BaseDataResult)provider.Apply(baseParams);

            if (!result.Success)
            {
                ExceptionHelper.AddException("IDistributionProvider.Apply", result.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }
    }
}
