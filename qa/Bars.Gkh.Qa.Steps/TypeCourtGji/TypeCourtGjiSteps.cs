using System;
using Bars.B4;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps.TypeCourtGji
{
    [Binding]
    public class TypeCourtGjiSteps : BindingBase
    {
        private IDomainService DomainService
        {
            get
            {
                Type entityType = TypeCourtGjiHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        [Given(@"пользователь добавляет новый вид суда")]
        public void ДопустимПользовательДобавляетНовыйВидСуда()
        {
            TypeCourtGjiHelper.Current = Activator
                .CreateInstance("Bars.GkhGji", "Bars.GkhGji.Entities.TypeCourtGji").Unwrap();
        }
        
        [Given(@"пользователь у этого вида суда заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаСудаЗаполняетПолеНаименование(string name)
        {
            TypeCourtGjiHelper.ChangeCurrent("Name", name);
        }
        
        [Given(@"пользователь у этого вида суда заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаСудаЗаполняетПолеКод(string code)
        {
            TypeCourtGjiHelper.ChangeCurrent("Name", code);
        }
        
        [When(@"пользователь сохраняет этот вид суда")]
        public void ЕслиПользовательСохраняетЭтотВидСуда()
        {
            var id = (long)TypeCourtGjiHelper.GetPropertyValue("Id");

            try
            {
                if (id == 0)
                {
                    this.DomainService.Save(TypeCourtGjiHelper.Current);
                }
                else
                {
                    this.DomainService.Update(TypeCourtGjiHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этот вид суда")]
        public void ЕслиПользовательУдаляетЭтотВидСуда()
        {
            var id = (long)TypeCourtGjiHelper.GetPropertyValue("Id");

            try
            {
                this.DomainService.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому виду суда присутствует в справочнике видов суда")]
        public void ТоЗаписьПоЭтомуВидуСудаПрисутствуетВСправочникеВидовСуда()
        {
            var id = (long)TypeCourtGjiHelper.GetPropertyValue("Id");

            var current = this.DomainService.Get(id);

            current.Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому виду суда должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому виду суда отсутствует в справочнике видов суда")]
        public void ТоЗаписьПоЭтомуВидуСудаОтсутствуетВСправочникеВидовСуда()
        {
            var id = (long)TypeCourtGjiHelper.GetPropertyValue("Id");

            var current = this.DomainService.Get(id);

            current.Should()
                .BeNull(
                    string.Format(
                        "запись по этому виду суда должна отсутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь у этого вида суда заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаСудаЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            TypeCourtGjiHelper.ChangeCurrent("Name", new string(ch, count));
        }

        [Given(@"пользователь у этого вида суда заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаСудаЗаполняетПолеКодСимволов(int count, char ch)
        {
            TypeCourtGjiHelper.ChangeCurrent("Code", new string(ch, count));
        }

    }
}
