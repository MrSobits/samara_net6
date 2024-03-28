using System;
using Bars.B4;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps.PeriodicityTemplateService
{
    [Binding]
    public class PeriodicityTemplateServiceSteps : BindingBase
    {
        private IDomainService DomainService
        {
            get
            {
                Type entityType = PeriodicityTemplateServiceHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        [Given(@"пользователь добавляет новую периодичность услуг")]
        public void ДопустимПользовательДобавляетНовуюПериодичностьУслуг()
        {
            PeriodicityTemplateServiceHelper.Current = Activator
                .CreateInstance("Bars.GkhDI", "Bars.GkhDi.Entities.PeriodicityTemplateService").Unwrap();
        }
        
        [Given(@"пользователь у этой периодичности услуг заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтойПериодичностиУслугЗаполняетПолеКод(string code)
        {
            PeriodicityTemplateServiceHelper.ChangeCurrent("Code", code);
        }
        
        [Given(@"пользователь у этой периодичности услуг заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойПериодичностиУслугЗаполняетПолеНаименование(string name)
        {
            PeriodicityTemplateServiceHelper.ChangeCurrent("Name", name);
        }
        
        [When(@"пользователь сохраняет эту периодичности услуг")]
        public void ЕслиПользовательСохраняетЭтуПериодичностиУслуг()
        {
            var id = (long)PeriodicityTemplateServiceHelper.GetPropertyValue("Id");

            try
            {
                if (id == 0)
                {
                    this.DomainService.Save(PeriodicityTemplateServiceHelper.Current);
                }
                else
                {
                    this.DomainService.Update(PeriodicityTemplateServiceHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет эту периодичности услуг")]
        public void ЕслиПользовательУдаляетЭтуПериодичностиУслуг()
        {
            var id = (long)PeriodicityTemplateServiceHelper.GetPropertyValue("Id");

            try
            {
                this.DomainService.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этой периодичности услуг присутствует в разделе периодичности услуг")]
        public void ТоЗаписьПоЭтойПериодичностиУслугПрисутствуетВРазделеПериодичностиУслуг()
        {
            var id = (long)PeriodicityTemplateServiceHelper.GetPropertyValue("Id");

            var periodicityTemplateService = this.DomainService.Get(id);

            periodicityTemplateService.Should()
                .NotBeNull(
                    string.Format(
                        "периодичностm услуг должна присутствовать в справочнике.{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этой периодичности услуг отсутствует в разделе периодичности услуг")]
        public void ТоЗаписьПоЭтойПериодичностиУслугОтсутствуетВРазделеПериодичностиУслуг()
        {
            var id = (long)PeriodicityTemplateServiceHelper.GetPropertyValue("Id");

            var periodicityTemplateService = this.DomainService.Get(id);

            periodicityTemplateService.Should()
                .BeNull(
                    string.Format(
                        "периодичностm услуг должна отсутствовать в справочнике.{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь у этой периодичности услуг заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойПериодичностиУслугЗаполняетПолеКодСимволов(int count, char ch)
        {
            PeriodicityTemplateServiceHelper.ChangeCurrent("Code", new string(ch, count));
        }

        [Given(@"пользователь у этой периодичности услуг заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойПериодичностиУслугЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            PeriodicityTemplateServiceHelper.ChangeCurrent("Name", new string(ch, count));
        }

    }
}
