using System;
using System.ComponentModel;
using System.ServiceModel.Channels;
using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Bars.Gkh.Qa.Steps
{
    [Binding]
    public class ConstructiveElementsSteps : BindingBase
    {

        private IDomainService<ConstructiveElement> ds = Container.Resolve<IDomainService<ConstructiveElement>>();

        [Given(@"добавлена группа конструктивных элементов")]
        public void ДопустимДобавленаГруппаКонструктивныхЭлементов(Table table)
        {
            ConstructiveElementGroupHelper.Current = table.CreateInstance<ConstructiveElementGroup>();
            Container.Resolve<IDomainService<ConstructiveElementGroup>>().SaveOrUpdate(ConstructiveElementGroupHelper.Current);
        }
        
        [Given(@"добавлен нормативный документ")]
        public void ДопустимДобавленНормативныйДокумент(Table table)
        {
            NormativeDocHelper.Current = table.CreateInstance<NormativeDoc>();
            Container.Resolve<IDomainService<NormativeDoc>>().SaveOrUpdate(NormativeDocHelper.Current);
        }
        
        [Given(@"пользователь добавляет новый конструктивный элемент")]
        public void ДопустимПользовательДобавляетНовыйКонструктивныйЭлемент()
        {
            ConstructiveElementHelper.Current = new ConstructiveElement();
        }
        
        [Given(@"пользователь у этого конструктивного элемента заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоКонструктивногоЭлементаЗаполняетПолеНаименование(string name)
        {
            ConstructiveElementHelper.Current.Name = name;
        }
        
        [Given(@"пользователь у этого конструктивного элемента заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоКонструктивногоЭлементаЗаполняетПолеКод(string code)
        {
            ConstructiveElementHelper.Current.Code = code;
        }
        
        [Given(@"пользователь у этого конструктивного элемента заполняет поле Группа")]
        public void ДопустимПользовательУЭтогоКонструктивногоЭлементаЗаполняетПолеГруппа()
        {
            ConstructiveElementHelper.Current.Group = ConstructiveElementGroupHelper.Current;
        }
        
        [Given(@"пользователь у этого конструктивного элемента заполняет поле Срок эксплуатации ""(.*)""")]
        public void ДопустимПользовательУЭтогоКонструктивногоЭлементаЗаполняетПолеСрокЭксплуатации(int lifetime)
        {
            ConstructiveElementHelper.Current.Lifetime = lifetime;
        }
        
        [Given(@"пользователь у этого конструктивного элемента заполняет поле Нормативный документ")]
        public void ДопустимПользовательУЭтогоКонструктивногоЭлементаЗаполняетПолеНормативныйДокумент()
        {
            ConstructiveElementHelper.Current.NormativeDoc = NormativeDocHelper.Current;
        }
        
        [Given(@"пользователь у этого конструктивного элемента заполняет поле Единица измерения")]
        public void ДопустимПользовательУЭтогоКонструктивногоЭлементаЗаполняетПолеЕдиницаИзмерения()
        {
            ConstructiveElementHelper.Current.UnitMeasure = UnitMeasureHelper.Current;
        }
        
        [When(@"пользователь сохраняет этот конструктивный элемент")]
        public void ЕслиПользовательСохраняетЭтотКонструктивныйЭлемент()
        {
            try
            {
                ds.SaveOrUpdate(ConstructiveElementHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этот конструктивный элемент")]
        public void ЕслиПользовательУдаляетЭтотКонструктивныйЭлемент()
        {
            try
            {
                ds.Delete(ConstructiveElementHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому конструктивному элементу присутствует в реестре конструктивных элементов")]
        public void ТоЗаписьПоЭтомуКонструктивномуЭлементуПрисутствуетВРеестреКонструктивныхЭлементов()
        {
            ds.Get(ConstructiveElementHelper.Current.Id).Should().NotBeNull();
        }
        
        [Then(@"запись по этому конструктивному элементу отсутствует в реестре конструктивных элементов")]
        public void ТоЗаписьПоЭтомуКонструктивномуЭлементуОтсутствуетВРеестреКонструктивныхЭлементов()
        {
            ds.Get(ConstructiveElementHelper.Current.Id).Should().BeNull();
        }

        [Then(@"запись по этому конструктивному элементу не сохраняется и падает ошибка с текстом ""(.*)""")]
        public void ТоЗаписьПоЭтомуКонструктивномуЭлементуНеСохраняетсяИПадаетОшибкаСТекстом(string exceptionMessage)
        {
            ds.Get(ConstructiveElementHelper.Current.Id).Should().BeNull();

            if (!ExceptionHelper.TestExceptions.ContainsKey("ЕслиПользовательСохраняетЭтотКонструктивныйЭлемент"))
            {
                throw new Exception("Во время сохранения не выпало ошибок");
            }

            ExceptionHelper.TestExceptions["ЕслиПользовательСохраняетЭтотКонструктивныйЭлемент"]
                .Should().Be(exceptionMessage,
                string.Format("ошибка должна быть {0}. {1}",
                exceptionMessage, ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь у этого конструктивного элемента заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоКонструктивногоЭлементаЗаполняетПолеНаименованиеСимволов(int length, char ch)
        {
            ConstructiveElementHelper.Current.Name = new string(ch, length);
        }

        [Given(@"пользователь у этого конструктивного элемента заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоКонструктивногоЭлементаЗаполняетПолеКодСимволов(int length, char ch)
        {
            ConstructiveElementHelper.Current.Code = new string(ch, length);
        }
    }
}
