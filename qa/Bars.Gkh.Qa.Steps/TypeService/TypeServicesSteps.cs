using System;
using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps
{
    [Binding]
    public class TypeServicesSteps : BindingBase
    {

        private IDomainService<TypeService> ds = Container.Resolve<IDomainService<TypeService>>();

        [Given(@"пользователь добавляет новый тип обслуживания")]
        public void ДопустимПользовательДобавляетНовыйТипОбслуживания()
        {
            TypeServicesHelper.Current = new TypeService();
        }
        
        [Given(@"пользователь у этого типа обслуживания заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаОбслуживанияЗаполняетПолеНаименование(string name)
        {
            TypeServicesHelper.Current.Name = name;
        }
        
        [Given(@"пользователь сохраняет этот тип обслуживания")]
        public void ДопустимПользовательСохраняетЭтотТипОбслуживания()
        {
            try
            {
                ds.SaveOrUpdate(TypeServicesHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь сохраняет этот тип обслуживания")]
        public void ЕслиПользовательСохраняетЭтотТипОбслуживания()
        {
            try
            {
                ds.SaveOrUpdate(TypeServicesHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этот тип обслуживания")]
        public void ЕслиПользовательУдаляетЭтотТипОбслуживания()
        {
            try
            {
                ds.Delete(TypeServicesHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому типу обслуживания присутствует в справочнике типов обслуживания")]
        public void ТоЗаписьПоЭтомуТипуОбслуживанияПрисутствуетВСправочникеТиповОбслуживания()
        {
            ds.Get(TypeServicesHelper.Current.Id).Should().NotBeNull();
        }
        
        [Then(@"запись по этому типу обслуживания отсутствует в справочнике типов обслуживания")]
        public void ТоЗаписьПоЭтомуТипуОбслуживанияОтсутствуетВСправочникеТиповОбслуживания()
        {
            ds.Get(TypeServicesHelper.Current.Id).Should().BeNull();
        }

        [Given(@"пользователь у этого типа обслуживания заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаОбслуживанияЗаполняетПолеНаименованиеСимволов(int length, char ch)
        {
            TypeServicesHelper.Current.Name = new string(ch, length);
        }

    }
}
