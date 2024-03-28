namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.Qa.Utils;
    using Bars.GkhGji.Entities;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class PlanInsCheckGjiSteps : BindingBase
    {
        private IDomainService DomainService
        {
            get
            {
                Type entityType = PlanInsCheckGjiHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        [Given(@"пользователь добавляет новый план инспекционной проверки")]
        public void ДопустимПользовательДобавляетНовыйПланИнспекционнойПроверки()
        {
            PlanInsCheckGjiHelper.Current = new PlanInsCheckGji();
        }

        [Given(@"пользователь у этого плана инспекционной проверки заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоПланаИнспекционнойПроверкиЗаполняетПолеНаименование(string name)
        {
            PlanInsCheckGjiHelper.ChangeCurrent("Name", name);
        }

        [Given(@"пользователь у этого плана инспекционной проверки заполняет поле Дата начала ""(.*)""")]
        public void ДопустимПользовательУЭтогоПланаИнспекционнойПроверкиЗаполняетПолеДатаНачала(string startDate)
        {
            var parsedStartDate = string.IsNullOrEmpty(startDate) ? (DateTime?)null : DateTime.Parse(startDate);

            PlanInsCheckGjiHelper.ChangeCurrent("DateStart", parsedStartDate);
        }

        [Given(@"пользователь у этого плана инспекционной проверки заполняет поле Дата окончания ""(.*)""")]
        public void ДопустимПользовательУЭтогоПланаИнспекционнойПроверкиЗаполняетПолеДатаОкончания(string endDate)
        {
            var parsedStartDate = string.IsNullOrEmpty(endDate) ? (DateTime?)null : DateTime.Parse(endDate);

            PlanInsCheckGjiHelper.ChangeCurrent("DateEnd", parsedStartDate);
        }

        //[Given(@"пользователь у этого плана инспекционной проверки заполняет поле Дата утверждения ""(.*)""")]
        //public void ДопустимПользовательУЭтогоПланаИнспекционнойПроверкиЗаполняетПолеДатаУтверждения(string dateApproval)
        //{
        //    PlanInsCheckGjiHelper.Current.DateApproval = dateApproval.DateParse();
        //}

        [Given(@"пользователь у этого плана инспекционной проверки заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоПланаИнспекционнойПроверкиЗаполняетПолеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            PlanInsCheckGjiHelper.ChangeCurrent(
                "Name",
                CommonHelper.DuplicateLine(symbol, countOfSymbols));
        }

        [When(@"пользователь сохраняет этот план инспекционной проверки")]
        public void ЕслиПользовательСохраняетЭтотПланИнспекционнойПроверки()
        {
            var id = (long)PlanInsCheckGjiHelper.GetPropertyValue("Id");

            try
            {
                if (id == 0)
                {
                    this.DomainService.Save(PlanInsCheckGjiHelper.Current);
                }
                else
                {
                    this.DomainService.Update(PlanInsCheckGjiHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет этот план инспекционной проверки")]
        public void ЕслиПользовательУдаляетЭтотПланИнспекционнойПроверки()
        {
            var id = (long)PlanInsCheckGjiHelper.GetPropertyValue("Id");

            try
            {
               this.DomainService.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому плану инспекционной проверки присутствует в справочнике планов инспекционных проверок")]
        public void ТоЗаписьПоЭтомуПлануИнспекционнойПроверкиПрисутствуетВСправочникеПлановИнспекционныхПроверок()
        {
            var id = (long)PlanInsCheckGjiHelper.GetPropertyValue("Id");

            var planInsCheckGjiHelper = this.DomainService.Get(id);

            planInsCheckGjiHelper.Should()
                .NotBeNull(
                    string.Format(
                        "план инспекционной проверки должен присутствовать в справочнике планов инспекционных проверок.{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому плану инспекционной проверки отсутствует в справочнике планов инспекционных проверок")]
        public void ТоЗаписьПоЭтомуПлануИнспекционнойПроверкиОтсутствуетВСправочникеПлановИнспекционныхПроверок()
        {
            var id = (long)PlanInsCheckGjiHelper.GetPropertyValue("Id");

            var planInsCheckGjiHelper = this.DomainService.Get(id);

            planInsCheckGjiHelper.Should()
                .BeNull(
                    string.Format(
                        "план инспекционной проверки должен отсутствовать в справочнике планов инспекционных проверок.{0}",
                        ExceptionHelper.GetExceptions()));
        }

    }
}
