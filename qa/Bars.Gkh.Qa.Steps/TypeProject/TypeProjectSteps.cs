using System;
using System.ComponentModel;
using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities;
using Bars.Gkh.Qa.Utils;
using Castle.DynamicProxy.Generators.Emitters;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps
{
    [Binding]
    public class TypeProjectSteps: BindingBase
    {
        private IDomainService<TypeProject> ds = Container.Resolve<IDomainService<TypeProject>>();
        
        [Given(@"пользователь добавляет новый тип проекта")]
        public void ДопустимПользовательДобавляетНовыйТипПроекта()
        {
            TypeProjectHelper.Current = new TypeProject();
        }
        
        [Given(@"пользователь у этого типа проекта заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаПроектаЗаполняетПолеНаименование(string name)
        {
            TypeProjectHelper.Current.Name = name;
        }
        
        [Given(@"пользователь у этого типа проекта заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаПроектаЗаполняетПолеКод(string code)
        {
            TypeProjectHelper.Current.Code = code;
        }
        
        [When(@"пользователь сохраняет этот тип проекта")]
        public void ДопустимПользовательСохраняетЭтотТипПроекта()
        {
            try
            {
                ds.SaveOrUpdate(TypeProjectHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет этот тип проекта")]
        public void ЕслиПользовательУдаляетЭтотТипПроекта()
        {
            try
            {
                ds.Delete(TypeProjectHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому типу проекта присутствует в справочнике типов проекта")]
        public void ТоЗаписьПоЭтомуТипуПроектаПрисутствуетВСправочникеТиповПроекта()
        {
            ds.Get(TypeProjectHelper.Current.Id).Should().NotBeNull();
        }
        
        [Then(@"запись по этому типу проекта отсутствует в справочнике типов проекта")]
        public void ТоЗаписьПоЭтомуТипуПроектаОтсутствуетВСправочникеТиповПроекта()
        {
            ds.Get(TypeProjectHelper.Current.Id).Should().BeNull();
        }

        [Given(@"пользователь у этого типа проекта заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаПроектаЗаполняетПолеНаименованиеСимволов(int length, char ch)
        {
            TypeProjectHelper.Current.Name = new string(ch, length);
        }

        [Given(@"пользователь у этого типа проекта заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаПроектаЗаполняетПолеКодСимволов(int length, char ch)
        {
            TypeProjectHelper.Current.Code = new string(ch, length);
        }

    }
}
