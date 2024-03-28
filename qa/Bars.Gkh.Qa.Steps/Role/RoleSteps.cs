using System;
using System.ComponentModel;
using Bars.B4;
using Bars.B4.Modules.Security;
using Bars.Gkh.Domain;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps
{
    [Binding]
    public class RoleSteps : BindingBase
    {
        private IDomainService<Role> ds = Container.Resolve<IDomainService<Role>>();

        [Given(@"пользователь добавляет новую роль")]
        public void ДопустимПользовательДобавляетНовуюРоль()
        {
            RoleHelper.Current = new Role();
        }
        
        [Given(@"пользователь у этой роли заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойРолиЗаполняетПолеНаименование(string name)
        {
            RoleHelper.Current.Name = name;
        }
        
        [When(@"пользователь сохраняет эту роль")]
        public void ЕслиПользовательСохраняетЭтуРоль()
        {
            try
            {
                ds.SaveOrUpdate(RoleHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет эту роль")]
        public void ЕслиПользовательУдаляетЭтуРоль()
        {
            try
            {
                ds.Delete(RoleHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этой роли присутствует в разделе ролей")]
        public void ТоЗаписьПоЭтойРолиПрисутствуетВРазделеРолей()
        {
            ds.Get(RoleHelper.Current.Id).Should().NotBeNull(string.Format("роль должна присутствовать в списке.{0}", ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этой роли отсутствует в разделе ролей")]
        public void ТоЗаписьПоЭтойРолиОтсутствуетВРазделеРолей()
        {
            ds.Get(RoleHelper.Current.Id).Should().BeNull(string.Format("роль должна отсутствовать в списке.{0}", ExceptionHelper.GetExceptions()));

        }

        [Given(@"пользователь у этой роли заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойРолиЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            RoleHelper.Current.Name = new string(ch, count);
        }

    }
}
