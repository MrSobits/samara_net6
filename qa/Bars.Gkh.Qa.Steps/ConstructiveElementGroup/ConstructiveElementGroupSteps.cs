namespace Bars.Gkh.Qa.Steps
{
    using System;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;
    using FluentAssertions;
    using TechTalk.SpecFlow;

    [Binding]
    public class ConstructiveElementGroupSteps : BindingBase
    {

        private IDomainService<ConstructiveElementGroup> ds = Container.Resolve<IDomainService<ConstructiveElementGroup>>();

        [Given(@"пользователь добавляет новую группу конструктивных элементов")]
        public void ДопустимПользовательДобавляетНовуюГруппуКонструктивныхЭлементов()
        {
            ConstructiveElementGroupHelper.Current = new ConstructiveElementGroup();
        }
        
        [Given(@"пользователь у этой группы конструктивных элементов заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыКонструктивныхЭлементовЗаполняетПолеНаименование(string name)
        {
            ConstructiveElementGroupHelper.Current.Name = name;
        }
        
        [Given(@"пользователь у этой группы конструктивных элементов заполняет поле Обязательность = true")]
        public void ДопустимПользовательУЭтойГруппыКонструктивныхЭлементовЗаполняетПолеОбязательностьTrue()
        {
            ConstructiveElementGroupHelper.Current.Necessarily = true;
        }
        
        [Given(@"пользователь у этой группы конструктивных элементов заполняет поле Обязательность = false")]
        public void ДопустимПользовательУЭтойГруппыКонструктивныхЭлементовЗаполняетПолеОбязательностьFalse()
        {
            ConstructiveElementGroupHelper.Current.Necessarily = false;
        }
        
        [When(@"пользователь сохраняет эту группу конструктивных элементов")]
        public void ЕслиПользовательСохраняетЭтуГруппуКонструктивныхЭлементов()
        {
            try
            {
                ds.SaveOrUpdate(ConstructiveElementGroupHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет эту группу конструктивных элементов")]
        public void ЕслиПользовательУдаляетЭтуГруппуКонструктивныхЭлементов()
        {
            try
            {
                ds.Delete(ConstructiveElementGroupHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этой форме присутствует в справочнике групп конструктивных элементов")]
        public void ТоЗаписьПоЭтойФормеПрисутствуетВСправочникеГруппКонструктивныхЭлементов()
        {
            ds.Get(ConstructiveElementGroupHelper.Current.Id).Should().NotBeNull();
        }
        
        [Then(@"запись по этой группе конструктивных элементов отсутствует в справочнике групп конструктивных элементов")]
        public void ТоЗаписьПоЭтойГруппеКонструктивныхЭлементовОтсутствуетВСправочникеГруппКонструктивныхЭлементов()
        {
            ds.Get(ConstructiveElementGroupHelper.Current.Id).Should().BeNull();
        }

        [Then(@"запись по этой группе конструктивных элементов не сохраняется и падает ошибка с текстом ""(.*)""")]
        public void ТоЗаписьПоЭтойГруппеКонструктивныхЭлементовНеСохраняетсяИПадаетОшибкаСТекстом(string exceptionMessage)
        {
            ds.Get(ConstructiveElementGroupHelper.Current.Id).Should().BeNull();

            if (!ExceptionHelper.TestExceptions.ContainsKey("ЕслиПользовательСохраняетЭтуГруппуКонструктивныхЭлементов"))
            {
                throw new Exception("Во время сохранения не выпало ошибок");
            }

            ExceptionHelper.TestExceptions["ЕслиПользовательСохраняетЭтуГруппуКонструктивныхЭлементов"]
                .Should().Be(exceptionMessage,
                string.Format("ошибка должна быть {0}. {1}",
                exceptionMessage, ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь у этой группы конструктивных элементов заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыКонструктивныхЭлементовЗаполняетПолеНаименованиеСимволов(int length, char ch)
        {
            ConstructiveElementGroupHelper.Current.Name = new string(ch, length);
        }
    }
}
