namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using Bars.Gkh.Entities;
    
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    public class RealityObjectConstructiveElementSteps : BindingBase
    {

        public IDomainService<RealityObjectConstructiveElement> ds = Container.Resolve<IDomainService<RealityObjectConstructiveElement>>();

        [Given(@"добавлен новый конструктивный элемент жилого дома")]
        public void ДопустимДобавленНовыйКонструктивныйЭлементЖилогоДома()
        {
            var realityObjectConstructiveElement = new RealityObjectConstructiveElement();
            realityObjectConstructiveElement.RealityObject = RealityObjectHelper.CurrentRealityObject;
            realityObjectConstructiveElement.ConstructiveElement = ConstructiveElementHelper.Current;
            RealityObjectConstructiveElementHelper.Current = realityObjectConstructiveElement;
        }

        [When(@"пользователь сохраняет конструктивный элемент жилого дома")]
        public void ЕслиПользовательСохраняетКонструктивныйЭлементЖилогоДома()
        {
            try
            {
                ds.Save(RealityObjectConstructiveElementHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
       
        [When(@"пользователь удаляет конструктивный элемент жилого дома")]
        public void ЕслиПользовательУдаляетКонструктивныйЭлементЖилогоДома()
        {
            try
            {
                ds.Delete(RealityObjectConstructiveElementHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"конструктивный элемент появляется в списке конструктивных элементов дома")]
        public void ТоКонструктивныйЭлементПоявляетсяВСпискеКонструктивныхЭлементовДома()
        {
            var constructiveElementGroupList = ds.GetAll().ToList();
            constructiveElementGroupList.Contains(RealityObjectConstructiveElementHelper.Current).Should().BeTrue();
        }

        [Then(@"конструктивный элемент отсутствует в списке конструктивных элементов дома")]
        public void ТоКонструктивныйЭлементОтсутствуетВСпискеКонструктивныхЭлементовДома()
        {
            var constructiveElementGroupList = ds.GetAll().ToList();
            constructiveElementGroupList.Contains(RealityObjectConstructiveElementHelper.Current).Should().BeFalse();
        }

    }
}
