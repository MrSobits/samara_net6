using System;
using Bars.B4;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps.StageWorkCrSteps
{
    [Binding]
    public class StageWorkCrStepsSteps : BindingBase
    {
        private IDomainService DomainService
        {
            get
            {
                Type entityType = StageWorkCrHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }
        
        [Given(@"пользователь добавляет новый этап работы")]
        public void ДопустимПользовательДобавляетНовыйЭтапРаботы()
        {
            StageWorkCrHelper.Current = Activator
                .CreateInstance("Bars.GkhCr", "Bars.GkhCr.Entities.StageWorkCr").Unwrap();
        }
        
        [Given(@"пользователь у этого этапа работы заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоЭтапаРаботыЗаполняетПолеНаименование(string name)
        {
            StageWorkCrHelper.ChangeCurrent("Name", name);
        }
        
        [Given(@"пользователь у этого этапа работы заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоЭтапаРаботыЗаполняетПолеКод(string code)
        {
            StageWorkCrHelper.ChangeCurrent("Code", code);
        }
        
        [When(@"пользователь сохраняет этот этап работы")]
        public void ЕслиПользовательСохраняетЭтотЭтапРаботы()
        {
            var id = (long)StageWorkCrHelper.GetPropertyValue("Id");

            try
            {
                if (id == 0)
                {
                    this.DomainService.Save(StageWorkCrHelper.Current);
                }
                else
                {
                    this.DomainService.Update(StageWorkCrHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этот этап работы")]
        public void ЕслиПользовательУдаляетЭтотЭтапРаботы()
        {
            var id = (long)StageWorkCrHelper.GetPropertyValue("Id");

            try
            {
                this.DomainService.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому этапу работы присутствует в разделе этапов работ")]
        public void ТоЗаписьПоЭтомуЭтапуРаботыПрисутствуетВРазделеЭтаповРабот()
        {
            var id = (long)StageWorkCrHelper.GetPropertyValue("Id");

            var current = this.DomainService.Get(id);

            current.Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому этапу работ должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому этапу работы отсутствует в разделе этапов работ")]
        public void ТоЗаписьПоЭтомуЭтапуРаботыОтсутствуетВРазделеЭтаповРабот()
        {
            var id = (long)StageWorkCrHelper.GetPropertyValue("Id");

            var current = this.DomainService.Get(id);

            current.Should()
                .BeNull(
                    string.Format(
                        "запись по этому этапу работ должна отсутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь у этого этапа работы заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоЭтапаРаботыЗаполняетПолеКодСимволов(int count, char ch)
        {
            StageWorkCrHelper.ChangeCurrent("Code", new string(ch, count));
        }

        [Given(@"пользователь у этого этапа работы заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоЭтапаРаботыЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            StageWorkCrHelper.ChangeCurrent("Name", new string(ch, count));
        }


    }
}
