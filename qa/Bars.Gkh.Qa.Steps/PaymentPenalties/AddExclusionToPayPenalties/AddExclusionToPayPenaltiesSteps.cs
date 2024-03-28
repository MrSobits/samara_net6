namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Controller.Provider;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Controllers;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    public class AddExclusionToPayPenaltiesSteps : BindingBase
    {

        public IDomainService<PaymentPenaltiesExcludePersAcc> DomainService
        {
            get
            {
                var ds = Container.Resolve<IDomainService<PaymentPenaltiesExcludePersAcc>>();

                return ds;
            }
        }

        [Given(@"добавлен расчет пеней")]
        public void ДопустимДобавленРасчетПеней(Table table)
        {
            PaymentPenaltiesListHelper.PaymentPenaltiesList = new List<PaymentPenalties>();

            foreach (var tableRow in table.Rows)
            {
                Type paymentPenaltiesType = Type.GetType("Bars.Gkh.RegOperator.Entities.Dict.PaymentPenalties, Bars.Gkh.RegOperator");

                var paymentPenalties = new PaymentPenalties();
                    

                var genericDomainServ = typeof(IDomainService<>).MakeGenericType(paymentPenaltiesType);

                var dsPrivilegedCategory = (IDomainService)Container.Resolve(genericDomainServ);

                paymentPenalties.DateStart = DateTime.Parse(tableRow["DateStart"]);

                var id = (long)paymentPenalties.Id;

                try
                {
                    if (id == 0)
                    {
                        dsPrivilegedCategory.Save(paymentPenalties);
                    }
                    else
                    {
                        dsPrivilegedCategory.Update(paymentPenalties);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                PaymentPenaltiesListHelper.PaymentPenaltiesList.Add(paymentPenalties);
            }
        }

        [When(@"пользователь к этим расчетам пеней добавляет исключение")]
        public void ЕслиПользовательКЭтимРасчетамПенейДобавляетИсключение()
        {
            PaymentPenaltiesExcludePersAccHelper.Current = new PaymentPenaltiesExcludePersAcc();

            Container.Resolve<ISessionProvider>().GetCurrentSession().Flush();
            

            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    { "persAccIds", new long[] { BasePersonalAccountHelper.Current.Id } },
                    { "payPenaltiesId", PaymentPenaltiesHelper.Current.Id }
                }
            };

            var service = Container.Resolve<IPaymentPenaltiesService>();
            
            try
            {
                service.AddExcludePersAccs(baseParams);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }


        [When(@"пользователь из этого расчета пеней с типом Специальный счет удаляет исключение")]
        public void ЕслиПользовательИзЭтогоРасчетаПенейСТипомСпециальныйСчетУдаляетИсключение()
        {
            try
            {
                DomainService.Delete(PaymentPenaltiesExcludePersAccHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь у этого расчета пеней добавляет исключение ЛС ""(.*)""")]
        public void ПользовательУЭтогоРасчетаПенейДобавляетИсключениеЛС(string basePersonalAccountNumber)
        {
            var paymentPenaltiesService = Container.Resolve<IPaymentPenaltiesService>();

            var requiredBasePersAcc = Container.Resolve<IDomainService<BasePersonalAccount>>().GetAll()
                .FirstOrDefault(x => x.PersonalAccountNum == basePersonalAccountNumber);

            if (requiredBasePersAcc == null)
            {
                throw new NullReferenceException(string.Format("Отсутствует ЛС с номером {0}", basePersonalAccountNumber));
            }

            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                                                  {
                                                      { "payPenaltiesId", PaymentPenaltiesHelper.Current.Id },
                                                      { "persAccIds", new long[] { requiredBasePersAcc.Id } }
                                                  }
            };

            var result = paymentPenaltiesService.AddExcludePersAccs(baseParams);

            if (!result.Success)
            {
                ExceptionHelper.AddException("IPaymentPenaltiesService.AddExcludePersAccs()", result.Message);

                return;
            }

            Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();

            var newExcludePersAcc = Container.Resolve<IDomainService<PaymentPenaltiesExcludePersAcc>>()
                .GetAll()
                .OrderByDescending(x => x.ObjectCreateDate).First(x => x.PersonalAccount.PersonalAccountNum == basePersonalAccountNumber);
     
            PaymentPenaltiesExcludePersAccHelper.Current = newExcludePersAcc;
        }

        [When(@"пользователь у этого расчета пеней удаляет это исключение")]
        public void ЕслиПользовательУЭтогоРасчетаПенейУдаляетЭтоИсключение()
        {
            try
            {
                Container.Resolve<IDomainService<PaymentPenaltiesExcludePersAcc>>()
                    .Delete(PaymentPenaltiesExcludePersAccHelper.Current.Id);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Flush();
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"в списке исключений у этого параметра расчета пеней отсутствует исключение для этого ЛС")]
        public void ТоВСпискеИсключенийУЭтогоПараметраРасчетаПенейОтсутствуетИсключениеДляЭтогоЛс()
        {
            this.DomainService.Get(PaymentPenaltiesExcludePersAccHelper.Current.Id).Should()
               .NotBeNull(
                   string.Format(
                       "в списке исключений у этого параметра расчета пеней должно отсутствовать исключение для ЛС {0}. {1}",
                       BasePersonalAccountHelper.Current.PersonalAccountNum,
                       ExceptionHelper.GetExceptions()));
        }


        [Then(@"запись по этому исключению присутствует у этого расчета пеней")]
        public void ТоЗаписьПоЭтомуИсключениюПрисутствуетУЭтогоРасчетаПеней()
        {
            this.DomainService.Get(PaymentPenaltiesExcludePersAccHelper.Current.Id).Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому исключению должна присутствовать в списке{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому исключению отсутствует у этого расчета пеней")]
        public void ТоЗаписьПоЭтомуИсключениюОтсутствуетУЭтогоРасчетаПеней()
        {
            this.DomainService.Get(PaymentPenaltiesExcludePersAccHelper.Current.Id).Should()
                .BeNull(
                    string.Format(
                        "запись по этому исключению должна отсутствовать в списке{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"количество записей по этому исключению = (.*)")]
        public void ТоКоличествоЗаписейПоЭтомуИсключению(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"в списке исключений у этого параметра расчета пеней присутствует исключение для ЛС ""(.*)""")]
        public void ТоВСпискеИсключенийУЭтогоПараметраРасчетаПенейПрисутствуетИсключениеДляЛС(string basePersonalAccountNumber)
        {
            var currentPenaltyExclusions = Container.Resolve<IDomainService<PaymentPenaltiesExcludePersAcc>>()
                .GetAll()
                .Where(x => x.PaymentPenalties.Id == PaymentPenaltiesHelper.Current.Id);

            currentPenaltyExclusions.Any(x => x.PersonalAccount.PersonalAccountNum == basePersonalAccountNumber)
                .Should()
                .BeTrue(
                    string.Format(
                        "в списке исключений у этого параметра расчета пеней должно присутствовать исключение для ЛС {0}",
                        basePersonalAccountNumber));
        }

        [Then(@"в списке исключений у этого параметра расчета пеней отсутствует исключение для ЛС ""(.*)""")]
        public void ТоВСпискеИсключенийУЭтогоПараметраРасчетаПенейОтсутствуетИсключениеДляЛС(string basePersonalAccountNumber)
        {
            var currentPenaltyExclusions = Container.Resolve<IDomainService<PaymentPenaltiesExcludePersAcc>>()
                .GetAll()
                .Where(x => x.PaymentPenalties.Id == PaymentPenaltiesHelper.Current.Id);

            currentPenaltyExclusions.Any(x => x.PersonalAccount.PersonalAccountNum == basePersonalAccountNumber)
                .Should()
                .BeFalse(
                    string.Format(
                        "в списке исключений у этого параметра расчета пеней должно отсутствовать исключение для ЛС {0}. {1}",
                        basePersonalAccountNumber, ExceptionHelper.GetExceptions()));
        }

        [Then(@"в списке добавления исключений у этого расчета пеней отсутствует ЛС ""(.*)""")]
        public void ТоВСпискеИсключенийУЭтогоРасчетаПенейОтсутствуетЛс(string basePersonalAccountNumber)
        {
            var controllerProvider = new ControllerProvider(Container);

            var paymentPenaltiesBasePersonalAccountController =
                (PaymentPenaltiesBasePersonalAccountController)controllerProvider.GetController(Container, "PaymentPenaltiesBasePersonalAccount");

            var baseParams = new BaseParams
                                 {
                                     Params =
                                         new DynamicDictionary
                                             {
                                                 {
                                                     "payPenaltiesId",
                                                     PaymentPenaltiesHelper.Current.Id
                                                 }
                                             }
                                 };

            var result = (JsonListResult)paymentPenaltiesBasePersonalAccountController.List(baseParams);

            dynamic data = result.Data;

            IEnumerable<object> list = ReflectionHelper.GetPropertyValue<IEnumerable<object>>(data, "data");

            list.Any(
                x => ReflectionHelper.GetPropertyValue<string>(x, "PersonalAccountNum") == basePersonalAccountNumber)
                .Should()
                .BeFalse(
                    string.Format(
                        "в списке добавления исключений у этого расчета пеней должен отсутствовать ЛС {0}. {1}",
                        basePersonalAccountNumber,
                        ExceptionHelper.GetExceptions()));
        }
    }
}
