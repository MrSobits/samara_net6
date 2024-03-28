using System;
using Bars.B4;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps.CourtVerdictGji
{
    [Binding]
    public class CourtVerdictGjiSteps : BindingBase
    {
        private IDomainService DomainService
        {
            get
            {
                Type entityType = SanctionGjiHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }
        [Given(@"пользователь добавляет новое решение суда")]
        public void ДопустимПользовательДобавляетНовоеРешениеСуда()
        {
            SanctionGjiHelper.Current = Activator
                .CreateInstance("Bars.GkhGji", "Bars.GkhGji.Entities.CourtVerdictGji").Unwrap();
        }
        
        [Given(@"пользователь у этого решения суда заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоРешенияСудаЗаполняетПолеНаименование(string name)
        {
            SanctionGjiHelper.ChangeCurrent("Name", name);
        }
        
        [Given(@"пользователь у этого решения суда заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоРешенияСудаЗаполняетПолеКод(string code)
        {
            SanctionGjiHelper.ChangeCurrent("Code", code);
        }
        
        [When(@"пользователь сохраняет это решение суда")]
        public void ЕслиПользовательСохраняетЭтоРешениеСуда()
        {
            var id = (long)SanctionGjiHelper.GetPropertyValue("Id");

            try
            {
                if (id == 0)
                {
                    this.DomainService.Save(SanctionGjiHelper.Current);
                }
                else
                {
                    this.DomainService.Update(SanctionGjiHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет это решение суда")]
        public void ЕслиПользовательУдаляетЭтоРешениеСуда()
        {
            var id = (long)SanctionGjiHelper.GetPropertyValue("Id");

            try
            {
                this.DomainService.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому решению суда присутствует в справочнике решений суда")]
        public void ТоЗаписьПоЭтомуРешениюСудаПрисутствуетВСправочникеРешенийСуда()
        {
            var id = (long)SanctionGjiHelper.GetPropertyValue("Id");

            var current = this.DomainService.Get(id);

            current.Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому решению суда должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому решению отсутствует в справочнике решений суда")]
        public void ТоЗаписьПоЭтомуРешениюОтсутствуетВСправочникеРешенийСуда()
        {
            var id = (long)SanctionGjiHelper.GetPropertyValue("Id");

            var current = this.DomainService.Get(id);

            current.Should()
                .BeNull(
                    string.Format(
                        "запись по этому решению суда должна отсутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Given(@"пользователь у этого решения суда заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоРешенияСудаЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            SanctionGjiHelper.ChangeCurrent("Name", new string(ch, count));
        }

        [Given(@"пользователь у этого решения суда заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоРешенияСудаЗаполняетПолеКодСимволов(int count, char ch)
        {
            SanctionGjiHelper.ChangeCurrent("Code", new string(ch, count));
        }


    }
}
