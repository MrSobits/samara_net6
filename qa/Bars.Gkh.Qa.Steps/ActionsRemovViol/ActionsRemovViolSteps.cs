using System;
using Bars.B4;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps.ActionsRemovViol
{
    [Binding]
    public class ActionsRemovViolSteps : BindingBase
    {
        private IDomainService DomainService
        {
            get
            {
                Type entityType = ActionsRemovViolHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        [Given(@"пользователь добавляет новое мероприятие по устранению нарушений")]
        public void ДопустимПользовательДобавляетНовоеМероприятиеПоУстранениюНарушений()
        {
            ActionsRemovViolHelper.Current = Activator
                .CreateInstance("Bars.GkhGji", "Bars.GkhGji.Entities.ActionsRemovViol").Unwrap();
        }
        
        [Given(@"пользователь у этого мероприятия по устранению нарушений заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоМероприятияПоУстранениюНарушенийЗаполняетПолеНаименование(string name)
        {
            ActionsRemovViolHelper.Current.Name = name;
        }
        
        [Given(@"пользователь у этого мероприятия по устранению нарушений заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоМероприятияПоУстранениюНарушенийЗаполняетПолеКод(string code)
        {
            ActionsRemovViolHelper.Current.Code = code;
        }
        
        [When(@"пользователь сохраняет это мероприятие по устранению нарушений")]
        public void ЕслиПользовательСохраняетЭтоМероприятиеПоУстранениюНарушений()
        {
            var id = (long)ActionsRemovViolHelper.Current.Id;

            try
            {
                if (id == 0)
                {
                    this.DomainService.Save(ActionsRemovViolHelper.Current);
                }
                else
                {
                    this.DomainService.Update(ActionsRemovViolHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет это мероприятие по устранению нарушений")]
        public void ЕслиПользовательУдаляетЭтоМероприятиеПоУстранениюНарушений()
        {
            var id = (long)ActionsRemovViolHelper.GetPropertyValue("Id");

            try
            {
                this.DomainService.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому мероприятию по устранению нарушений присутствует в справочнике мероприятий по устранению нарушений")]
        public void ТоЗаписьПоЭтомуМероприятиюПоУстранениюНарушенийПрисутствуетВСправочникеМероприятийПоУстранениюНарушений()
        {
            var id = (long)ActionsRemovViolHelper.GetPropertyValue("Id");

            var current = this.DomainService.Get(id);

            current.Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому мероприятию должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому мероприятию по устранению нарушений отсутствует в справочнике мероприятий по устранению нарушений")]
        public void ТоЗаписьПоЭтомуМероприятиюПоУстранениюНарушенийОтсутствуетВСправочникеМероприятийПоУстранениюНарушений()
        {
            var id = (long)ActionsRemovViolHelper.GetPropertyValue("Id");

            var current = this.DomainService.Get(id);

            current.Should()
                .BeNull(
                    string.Format(
                        "запись по этому мероприятию должна отсутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь у этого мероприятия по устранению нарушений заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоМероприятияПоУстранениюНарушенийЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            ActionsRemovViolHelper.ChangeCurrent("Name", new string(ch, count));
        }

        [Given(@"пользователь у этого мероприятия по устранению нарушений заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоМероприятияПоУстранениюНарушенийЗаполняетПолеКодСимволов(int count, char ch)
        {
            ActionsRemovViolHelper.ChangeCurrent("Code", new string(ch, count));
        }

    }
}
