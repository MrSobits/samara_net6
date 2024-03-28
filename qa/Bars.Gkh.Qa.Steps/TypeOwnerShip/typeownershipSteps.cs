namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;
    using FluentAssertions;
    using TechTalk.SpecFlow;

    [Binding]
    public class TypeOwnershipSteps : BindingBase
    {
        public IDomainService<TypeOwnership> ds = Container.Resolve<IDomainService<TypeOwnership>>();

        [Given(@"пользователь добавляет новую форму собственности")]
        public void ДопустимПользовательДобавляетНовуюФормуСобственности()
        {
            TypeOwnershipHelper.Current = new TypeOwnership();
        }
        
        [Given(@"у этой формы собственности устанавливает поле Наименование ""(.*)""")]
        public void ДопустимУЭтойФормыСобственностиУстанавливаетПолеНаименование(string name)
        {
            TypeOwnershipHelper.Current.Name = name;
        }
        
        [When(@"пользователь сохраняет эту форму собственности")]
        public void ЕслиПользовательСохраняетЭтуФормуСобственности()
        {
            try
            {
                ds.SaveOrUpdate(TypeOwnershipHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет эту форму собственности")]
        public void ЕслиПользовательУдаляетЭтуФормуСобственности()
        {
            try
            {
                ds.Delete(TypeOwnershipHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"созданная форма собственности появляется в справочнике")]
        public void ТоСозданнаяФормаСобственностиПоявляетсяВСправочнике()
        {
            var typeOwnershipList = ds.GetAll().ToList();
            typeOwnershipList.Contains(TypeOwnershipHelper.Current).Should().BeTrue();
        }

        [Then(@"созданная форма собственности отсутствует в справочнике")]
        public void ТоСозданнаяФормаСобственностиОтсутствуетВСправочнике()
        {
            var typeOwnershipList = ds.GetAll().ToList();
            typeOwnershipList.Contains(TypeOwnershipHelper.Current).Should().BeFalse();
        }

        [Given(@"пользователь у этой формы собственности заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойФормыСобственностиЗаполняетПолеНаименованиеСимволов(int length, char symbol)
        {
            TypeOwnershipHelper.Current.Name = new string(symbol, length);
        }
    }
}