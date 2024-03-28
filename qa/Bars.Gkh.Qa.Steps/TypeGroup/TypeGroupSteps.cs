using System;
using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities.CommonEstateObject;
using Bars.Gkh.Enums;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps
{
    [Binding]
    public class TypeGroupSteps:BindingBase
    {
        private IDomainService<GroupType> ds = Container.Resolve<IDomainService<GroupType>>();
        
        [Given(@"пользователь добавляет новый тип группы ООИ")]
        public void ДопустимПользовательДобавляетНовыйТипГруппыООИ()
        {
            TypeGroupHelper.Current = new GroupType();
        }
        
        [Given(@"пользователь у этого типа группы ООИ заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаГруппыООИЗаполняетПолеКод(string code)
        {
            TypeGroupHelper.Current.Code = code;
        }
        
        [Given(@"пользователь у этого типа группы ООИ заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаГруппыООИЗаполняетПолеНаименование(string name)
        {
            TypeGroupHelper.Current.Name = name;
        }
        
        [When(@"пользователь сохраняет этот тип группы ООИ")]
        public void ЕслиПользовательСохраняетЭтотТипГруппыООИ()
        {
            try
            {
                ds.SaveOrUpdate(TypeGroupHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этот тип группы ООИ")]
        public void ЕслиПользовательУдаляетЭтотТипГруппыООИ()
        {
            try
            {
                ds.Delete(TypeGroupHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому типу группы ООИ присутствует в справочнике типов группы ООИ")]
        public void ТоЗаписьПоЭтомуТипуГруппыООИПрисутствуетВСправочникеТиповГруппыООИ()
        {
            ds.Get(TypeGroupHelper.Current.Id).Should().NotBeNull(string.Format("тип группы ООИ должен присутствовать в справочнике.{0}", ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому типу группы ООИ отсутствует в справочнике типов группы ООИ")]
        public void ТоЗаписьПоЭтомуТипуГруппыООИОтсутствуетВСправочникеТиповГруппыООИ()
        {
            ds.Get(TypeGroupHelper.Current.Id).Should().BeNull(string.Format("тип группы ООИ должен отсутствовать в справочнике.{0}", ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь у этого типа группы ООИ заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаГруппыООИЗаполняетПолеКодСимволов(int count, char ch)
        {
            TypeGroupHelper.Current.Code = new string(ch, count);
        }

        [Given(@"пользователь у этого типа группы ООИ заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоТипаГруппыООИЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            TypeGroupHelper.Current.Name = new string(ch, count);
        }

    }
}
