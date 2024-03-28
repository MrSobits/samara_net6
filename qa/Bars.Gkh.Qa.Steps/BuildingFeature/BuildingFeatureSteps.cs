namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class BuildingFeatureSteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<BuildingFeature> _cashe = new BindingBase.DomainServiceCashe<BuildingFeature>();

        [Given(@"пользователь добавляет новый особый признак строения")]
        public void ДопустимПользовательДобавляетНовыйОсобыйПризнакСтроения()
        {
            BuildingFeatureHelper.Current = new BuildingFeature();
        }

        [Given(@"пользователь у этого особого признака строения заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоОсобогоПризнакаСтроенияЗаполняетПолеНаименование(string name)
        {
            BuildingFeatureHelper.Current.Name = name;
        }

        [Given(@"пользователь у этого особого признака строения заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоОсобогоПризнакаСтроенияЗаполняетПолеКод(string code)
        {
            BuildingFeatureHelper.Current.Code = code;
        }

        [When(@"пользователь сохраняет этот особый признак строения")]
        public void ЕслиПользовательСохраняетЭтотОсобыйПризнакСтроения()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(BuildingFeatureHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет этот особый признак строения")]
        public void ЕслиПользовательУдаляетЭтотОсобыйПризнакСтроения()
        {
            try
            {
                this._cashe.Current.Delete(BuildingFeatureHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Given(@"пользователь у этого особого признака строения заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоОсобогоПризнакаСтроенияЗаполняетПолеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            BuildingFeatureHelper.Current.Name =
               CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого особого признака строения заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоОсобогоПризнакаСтроенияЗаполняетПолеКодСимволов(int countOfSymbols, string symbol)
        {
            BuildingFeatureHelper.Current.Code =
              CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Then(@"запись по этому особому признаку строения присутствует в справочнике особых признаков строения")]
        public void ТоЗаписьПоЭтомуОсобомуПризнакуСтроенияПрисутствуетВСправочникеОсобыхПризнаковСтроения()
        {
            var buildingFeature = this._cashe.Current.Get(BuildingFeatureHelper.Current.Id);

            buildingFeature.Should().NotBeNull(string.Format("Запись по этому особому признаку строения должна присутствовать в справочнике  особых признаков строения.{0}", ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому особому признаку строения отсутствует в справочнике особых признаков строения")]
        public void ТоЗаписьПоЭтомуОсобомуПризнакуСтроенияОтсутствуетВСправочникеОсобыхПризнаковСтроения()
        {
            var buildingFeature = this._cashe.Current.Get(BuildingFeatureHelper.Current.Id);

            buildingFeature.Should().BeNull(string.Format("Запись по этому особому признаку строения должна отсутствовать в справочнике  особых признаков строения.{0}", ExceptionHelper.GetExceptions()));
        }

    }
}
