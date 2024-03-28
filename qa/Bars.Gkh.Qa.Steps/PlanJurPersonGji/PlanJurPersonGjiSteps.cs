namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    internal class PlanJurPersonGjiSteps : BindingBase
    {
        private IDomainService DomainService
        {
            get
            {
                Type entityType = PlanJurPersonGjiHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        [Given(@"пользователь добавляет новый план проверки юридических лиц")]
        public void ДопустимПользовательДобавляетНовыйПланПроверкиЮридическихЛиц()
        {
            PlanJurPersonGjiHelper.Current = new PlanJurPersonGji();
        }

        [Given(@"пользователь у этого плана проверки юридических лиц заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоПланаПроверкиЮридическихЛицЗаполняетПолеНаименование(string name)
        {
            PlanJurPersonGjiHelper.ChangeCurrent("Name", name);
        }

        [Given(@"пользователь у этого плана проверки юридических лиц заполняет поле Дата начала ""(.*)""")]
        public void ДопустимПользовательУЭтогоПланаПроверкиЮридическихЛицЗаполняетПолеДатаНачала(string startDate)
        {
            var parsedStartDate = string.IsNullOrEmpty(startDate) ? (DateTime?)null : DateTime.Parse(startDate);

            PlanJurPersonGjiHelper.ChangeCurrent("DateStart", parsedStartDate);
        }

        [Given(@"пользователь у этого плана проверки юридических лиц заполняет поле Дата окончания ""(.*)""")]
        public void ДопустимПользовательУЭтогоПланаПроверкиЮридическихЛицЗаполняетПолеДатаОкончания(string endDate)
        {
            var parsedStartDate = string.IsNullOrEmpty(endDate) ? (DateTime?)null : DateTime.Parse(endDate);

            PlanJurPersonGjiHelper.ChangeCurrent("DateEnd", parsedStartDate);
        }

        [When(@"пользователь сохраняет этот план проверки юридических лиц")]
        public void ЕслиПользовательСохраняетЭтотПланПроверкиЮридическихЛиц()
        {
            var id = (long)PlanJurPersonGjiHelper.GetPropertyValue("Id");

            try
            {
                if (id == 0)
                {
                    this.DomainService.Save(PlanJurPersonGjiHelper.Current);
                }
                else
                {
                    this.DomainService.Update(PlanJurPersonGjiHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет этот план проверки юридических лиц")]
        public void ЕслиПользовательУдаляетЭтотПланПроверкиЮридическихЛиц()
        {
            var id = (long)PlanJurPersonGjiHelper.GetPropertyValue("Id");

            try
            {
                this.DomainService.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому плану проверки юридических лиц присутствует в справочнике планов проверок юридических лиц")]
        public void ТоЗаписьПоЭтомуПлануПроверкиЮридическихЛицПрисутствуетВСправочникеПлановПроверокЮридическихЛиц()
        {
            var id = (long)PlanJurPersonGjiHelper.GetPropertyValue("Id");

            var planJurPersonGji = this.DomainService.Get(id);

            planJurPersonGji.Should()
                .NotBeNull(
                    string.Format(
                        "план проверки юридических лиц должен присутствовать в справочнике планов проверок юридических лиц.{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому плану проверки юридических лиц отсутствует в справочнике планов проверок юридических лиц")]
        public void ТоЗаписьПоЭтомуПлануПроверкиЮридическихЛицОтсутствуетВСправочникеПлановПроверокЮридическихЛиц()
        {
            var id = (long)PlanJurPersonGjiHelper.GetPropertyValue("Id");

            var planJurPersonGji = this.DomainService.Get(id);

            planJurPersonGji.Should()
                .BeNull(
                    string.Format(
                        "план проверки юридических лиц должен отсутствовать в справочнике планов проверок юридических лиц.{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Given(@"добавлен план проверки юридических лиц")]
        public void ДопустимДобавленПланПроверкиЮридическихЛиц(Table table)
        {
            PlanJurPersonGjiHelper.Current = table.CreateInstance<PlanJurPersonGji>();

            try
            {
                Container.Resolve<IDomainService<PlanJurPersonGji>>().SaveOrUpdate((PlanJurPersonGji)PlanJurPersonGjiHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
