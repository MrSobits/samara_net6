using System;
using Bars.B4;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps.SanctionGji
{
    [Binding]
    public class SanctionGjiSteps : BindingBase
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

        [Given(@"пользователь добавляет новый вид санкций")]
        public void ДопустимПользовательДобавляетНовыйВидСанкций()
        {
            SanctionGjiHelper.Current = Activator
                .CreateInstance("Bars.GkhGji", "Bars.GkhGji.Entities.SanctionGji").Unwrap();
        }
        
        [Given(@"пользователь у этого вида санкций заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаСанкцийЗаполняетПолеНаименование(string name)
        {
            SanctionGjiHelper.ChangeCurrent("Name", name);
        }
        
        [Given(@"пользователь у этого вида санкций заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаСанкцийЗаполняетПолеКод(string code)
        {
            SanctionGjiHelper.ChangeCurrent("Code", code);
        }
        
        [When(@"пользователь сохраняет этот вид санкций")]
        public void ЕслиПользовательСохраняетЭтотВидСанкций()
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
        
        [When(@"пользователь удаляет этот вид санкций")]
        public void ЕслиПользовательУдаляетЭтотВидСанкций()
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
        
        [Then(@"запись по этому виду санкций присутствует в справочнике видов санкций")]
        public void ТоЗаписьПоЭтомуВидуСанкцийПрисутствуетВСправочникеВидовСанкций()
        {
            var id = (long)SanctionGjiHelper.GetPropertyValue("Id");

            var current = this.DomainService.Get(id);

            current.Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому виду санкций должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому виду санкций отсутствует в справочнике видов санкций")]
        public void ТоЗаписьПоЭтомуВидуСанкцийОтсутствуетВСправочникеВидовСанкций()
        {
            var id = (long)SanctionGjiHelper.GetPropertyValue("Id");

            var current = this.DomainService.Get(id);

            current.Should()
                .BeNull(
                    string.Format(
                        "запись по этому виду санкций должна отсутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь у этого вида санкций заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаСанкцийЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            SanctionGjiHelper.ChangeCurrent("Name", new string(ch, count));
        }

        [Given(@"пользователь у этого вида санкций заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаСанкцийЗаполняетПолеКодСимволов(int count, char ch)
        {
            SanctionGjiHelper.ChangeCurrent("Name", new string(ch, count));
        }


    }
}
