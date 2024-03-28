using Bars.Gkh.RegOperator.Enums;

namespace Bars.Gkh.Qa.Steps
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using System;
    using System.Collections;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.DomainService.Period;
    using Bars.Gkh.RegOperator.Entities.Period;
    using FluentAssertions;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using System.Linq;
    using B4;
    using B4.Controller.Provider;
    using Bars.B4.DataAccess;
    using RegOperator.DomainService.PersonalAccount;

    [Binding]
    public class ChargePeriodSteps : BindingBase
    {
        [When(@"пользователь заходит в периоды начислений")]
        public void ЕслиПользовательЗаходитВПериодыНачислений()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {
                        "sort", new[] {new DynamicDictionary {{"property", "Name"}, {"direction", "ASC"}}}
                    }
                }
            };
            
            var res = Container.Resolve<ISessionProvider>().GetCurrentSession().CreateSQLQuery("Truncate regop_period cascade").ExecuteUpdate();
            
            ScenarioContext.Current["ChargePeriodVMList"] =
                Container.Resolve<IViewModel<PeriodCloseCheckResult>>()
                    .List(Container.Resolve<IDomainService<PeriodCloseCheckResult>>(), baseParams);
        }
        
        [When(@"ни одного периода еще нет")]
        public void ЕслиНиОдногоПериодаЕщеНет()
        {
            IDataResult result = ScenarioContext.Current.Get<IDataResult>("ChargePeriodVMList");
            ((IList)(result.Data)).Should().BeNullOrEmpty("Список периодов начислений должен быть пустым");
        }

        [Given(@"пользователь выбирает Период ""(.*)""")]
        public void ДопустимПользовательВыбираетПериод(string periodName)
        {
            IQueryable<PeriodCloseCheckResult> periodQuery = ChargePeriodHelper.DomainService.GetAll();

            IList<PeriodCloseCheckResult> periodList = periodQuery.ToList();

            var currentPeriod = periodName == "текущий" 
                ? periodList.FirstOrDefault(x => x.CheckState != PeriodCloseCheckStateType.Pending) 
                : periodList.FirstOrDefault(x => x.Name == periodName);
            
            if (currentPeriod == null)
            {
                throw new SpecFlowException(string.Format("Отсутствует период с наименованием {0}", periodName));
            }

            ChargePeriodHelper.Current = currentPeriod;

            BasePersonalAccountHelper.FilterBaseParams.Params["periodId"] = currentPeriod.Id;
        }

        [Then(@"Период автоматически создается")]
        public void ТоПериодАвтоматическиСоздается()
        {
            try
            {
                var res = Container.Resolve<IChargePeriodService>().CreateFirstPeriod();
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {
                        "sort", new[] {new DynamicDictionary {{"property", "Name"}, {"direction", "ASC"}}}
                    }
                }
            };

            var result =
                Container.Resolve<IViewModel<PeriodCloseCheckResult>>()
                    .List(Container.Resolve<IDomainService<PeriodCloseCheckResult>>(), baseParams);

            ((IList)(result.Data)).Should().NotBeNullOrEmpty("В списке периодов должен появитсья период, но список пуст");
        }

        [When(@"пользователь закрывает текущий период")]
        public void ТоПользовательЗакрываетПериод()
        {
            IDataResult result = null;

            try
            {
                result = Container.Resolve<IChargePeriodCloseService>().CloseCurrentPeriod(new BaseParams());
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            if (!result.Success)
            {
                Assert.Fail(result.Message);
            }

            ScenarioContext.Current["TaskEntryParentTaskId"] = ((CreateTasksDataResult)result).Data.ParentTaskId;
        }

        [Then(@"в карточке этого лицевого счета, в операциях за текущий период, присутствует запись ""(.*)"" где изменение сальдо = (.*)")]
        public void ТоВКарточкеЭтогоЛицевогоСчетаВОперацияхЗаТекущийПериодПрисутствуетЗаписьГдеИзменениеСальдо(string operationName, Decimal saldoChange)
        {
            var controllerProvider = new ControllerProvider(Container);

            dynamic controllerChargePeriod = controllerProvider.GetController(Container, "ChargePeriod");

            long periodId = 0;
            var period = controllerChargePeriod.List(new BaseParams());
            foreach (dynamic x in ((IList)((ListDataResult)period.Data).Data))
            {
                if (x.GetType().GetProperty("IsClosed").GetValue(x) == false)
                {
                    periodId = x.GetType().GetProperty("Id").GetValue(x);
                    break;
                }
            }

            var bP = new BaseParams { Params = new DynamicDictionary { { "accountId", BasePersonalAccountHelper.Current.Id }, { "periodId", periodId.ToString() } } };

            var service = Container.Resolve<IPersonalAccountDetailService>();
            var result = service.GetPeriodDetail(bP);

            foreach (dynamic x in result.Data)
            {
                if (x.GetType().GetProperty("PeriodId").GetValue(x) == periodId)
                {
                    periodId = x.GetType().GetProperty("Id").GetValue(x);
                    break;
                }
            }

            var baseParamsforListOperDet = new BaseParams
            {
                Params =
                    new DynamicDictionary
                    {
                        {
                            "filter", new[]
                            {
                                new DynamicDictionary
                                {
                                    {"property", "periodSummaryId"},
                                    {"value", periodId}
                                }
                            }
                        }
                    }
            };

            var listOperationDetails = (IList)service.GetPeriodOperationDetail(baseParamsforListOperDet).Data;

            listOperationDetails.Should().NotBeNullOrEmpty("В данном периоде список операций  для текущего лицевого счета пуст");

            switch (operationName)
            {
                case "Платеж КР":
                    operationName = "Оплата по базовому тарифу";
                    break;

                case "Поступление оплат аренды":
                    operationName = "Поступление оплаты аренды";
                    break;
            }

            var operation = listOperationDetails.Cast<PeriodOperationDetail>()
                .FirstOrDefault(x => x.SaldoChange == saldoChange && x.Name == operationName);

            operation.Should()
                .NotBeNull("не найдена операция, где: Название " + operationName + ", сальдо = " + saldoChange);
        }
    }
}
