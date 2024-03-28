using System;
using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps.ProblemPlace
{
    [Binding]
    public class ProblemPlaceSteps : BindingBase
    {

        private IDomainService<Entities.Suggestion.ProblemPlace> ds =
            Container.Resolve<IDomainService<Entities.Suggestion.ProblemPlace>>();
        
        [Given(@"пользователь добавляет новое Место проблемы")]
        public void ДопустимПользовательДобавляетНовоеМестоПроблемы()
        {
            ProblemPlaceHelper.Current = new Entities.Suggestion.ProblemPlace();
        }
        
        [Given(@"у этой места проблемы заполняет поле Наименование ""(.*)""")]
        public void ДопустимУЭтойМестаПроблемыЗаполняетПолеНаименование(string name)
        {
            ProblemPlaceHelper.Current.Name = name;
        }

        [Given(@"у этой места проблемы заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимУЭтойМестаПроблемыЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            ProblemPlaceHelper.Current.Name = new string(ch, count);
        }


        [When(@"пользователь сохраняет новое Место проблемы")]
        public void ЕслиПользовательСохраняетНовоеМестоПроблемы()
        {
            try
            {
                ds.SaveOrUpdate(ProblemPlaceHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет новое Место проблемы")]
        public void ЕслиПользовательУдаляетНовоеМестоПроблемы()
        {
            try
            {
                ds.Delete(ProblemPlaceHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"Место проблемы присутствует в списке")]
        public void ТоМестоПроблемыПрисутствуетВСписке()
        {
            ds.Get(ProblemPlaceHelper.Current.Id)
                .Should()
                .NotBeNull(string.Format("Место проблемы должно присутствовать в списке.{0}", ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"Место проблемы отсутствует в списке")]
        public void ТоМестоПроблемыОтсутствуетВСписке()
        {
            ds.Get(ProblemPlaceHelper.Current.Id)
                .Should()
                .BeNull(string.Format("Место проблемы должно отсутствовать в списке.{0}", ExceptionHelper.GetExceptions()));
        }
    }
}
