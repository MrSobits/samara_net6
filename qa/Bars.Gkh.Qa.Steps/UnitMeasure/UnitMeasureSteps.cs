namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;
    using FluentAssertions;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;

    [Binding]
    class UnitMeasureSteps : BindingBase
    {
        private DomainServiceCashe<UnitMeasure> _cashe = new DomainServiceCashe<UnitMeasure>();
        
        [Given(@"пользователь добавляет новую единицу измерения")]
        public void ДопустимПользовательДобавляетНовуюЕдиницуИзмерения()
        {
            UnitMeasureHelper.Current = new UnitMeasure();
        }

        [Given(@"пользователь у этой единицы измерения заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойЕдиницыИзмеренияЗаполняетПолеНаименование(string name)
        {
            UnitMeasureHelper.Current.Name = name;
        }

        [Given(@"пользователь у этой единицы измерения заполняет поле Краткое наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойЕдиницыИзмеренияЗаполняетПолеКраткоеНаименование(string shortName)
        {
            UnitMeasureHelper.Current.ShortName = shortName;
        }

        [Given(@"пользователь у этой единицы измерения заполняет поле Описание ""(.*)""")]
        public void ДопустимПользовательУЭтойЕдиницыИзмеренияЗаполняетПолеОписание(string description)
        {
            UnitMeasureHelper.Current.Description = description;
        }

        [Given(@"пользователь у этой единицы измерения заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойЕдиницыИзмеренияЗаполняетПолеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            UnitMeasureHelper.Current.Name =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой единицы измерения заполняет поле Краткое наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойЕдиницыИзмеренияЗаполняетПолеКраткоеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            UnitMeasureHelper.Current.ShortName =
               CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой единицы измерения заполняет поле Описание (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойЕдиницыИзмеренияЗаполняетПолеОписаниеСимволов(int countOfSymbols, string symbol)
        {
            UnitMeasureHelper.Current.Description =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"добавлена единица измерения")]
        public void ДопустимДобавленаЕдиницаИзмерения(Table unitMeasureTable)
        {
            var unitMeasure = unitMeasureTable.CreateInstance<UnitMeasure>();

            UnitMeasureHelper.Current = unitMeasure;

            this._cashe.Current.SaveOrUpdate(unitMeasure);
        }

        [When(@"пользователь сохраняет эту единицу измерения")]
        public void ЕслиПользовательСохраняетЭтуЕдиницыИзмерения()
        {
            try
            {
                _cashe.Current.SaveOrUpdate(UnitMeasureHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет эту единицу измерения")]
        public void ЕслиПользовательУдаляетЭтуЕдиницуИзмерения()
        {
            try
            {
                _cashe.Current.Delete(UnitMeasureHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этой единице измерения присутствует в справочнике единиц измерения")]
        public void ТоЗаписьПоЭтойЕдиницеИзмеренияПрисутствуетВСправочникеЕдиницИзмерения()
        {
            var unitMeasure = this._cashe.Current.Get(UnitMeasureHelper.Current.Id);

            unitMeasure.Should().NotBeNull(string.Format("единица измерения должна присутствовать в справочнике единиц измерения.{0}", ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этой единице измерения отсутствует в справочнике единиц измерения")]
        public void ТоЗаписьПоЭтойЕдиницеИзмеренияОтсутствуетВСправочникеЕдиницИзмерения()
        {
            var unitMeasure = this._cashe.Current.Get(UnitMeasureHelper.Current.Id);

            unitMeasure.Should().BeNull(string.Format("единица измерения должна отсутствовать в справочнике единиц измерения.{0}", ExceptionHelper.GetExceptions()));
        }

    }
}
