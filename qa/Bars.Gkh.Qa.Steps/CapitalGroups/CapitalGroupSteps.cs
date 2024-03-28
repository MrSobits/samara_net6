namespace Bars.Gkh.Qa.Steps
{
    using System;

    using TechTalk.SpecFlow;
    using FluentAssertions;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;

    [Binding]
    class CapitalGroupSteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<CapitalGroup> _cashe = new BindingBase.DomainServiceCashe<CapitalGroup>();


        [Given(@"пользователь добавляет новую группу капитальности")]
        public void ДопустимПользовательДобавляетНовуюГруппуКапитальности()
        {
            CapitalGroupHelper.Current = new CapitalGroup();
        }

        [Given(@"пользователь у этой группы капитальности заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыКапитальностиЗаполняетПолеНаименование(string name)
        {
            CapitalGroupHelper.Current.Name = name;
        }

        [Given(@"пользователь у этой группы капитальности заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыКапитальностиЗаполняетПолеКод(string code)
        {
            CapitalGroupHelper.Current.Code = code;
        }

        [Given(@"пользователь у этой группы капитальности заполняет поле Описание ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыКапитальностиЗаполняетПолеОписание(string description)
        {
            CapitalGroupHelper.Current.Description = description;
        }

        [Given(@"пользователь у этой группы капитальности заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыКапитальностиЗаполняетПолеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            CapitalGroupHelper.Current.Name =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой группы капитальности заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыКапитальностиЗаполняетПолеКодСимволов(int countOfSymbols, string symbol)
        {
            CapitalGroupHelper.Current.Code =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой группы капитальности заполняет поле Описание (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыКапитальностиЗаполняетПолеОписаниеСимволов(int countOfSymbols, string symbol)
        {
            CapitalGroupHelper.Current.Description =
               CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [When(@"пользователь сохраняет эту группу капитальности")]
        public void ЕслиПользовательСохраняетЭтуГруппуКапитальности()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(CapitalGroupHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет эту группу капитальности")]
        public void ЕслиПользовательУдаляетЭтуГруппуКапитальности()
        {
            try
            {
                this._cashe.Current.Delete(CapitalGroupHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этой группе капитальности присутствует в справочнике групп капитальности")]
        public void ТоЗаписьПоЭтойГруппеКапитальностиПрисутствуетВСправочникеГруппКапитальности()
        {
            var capitalGroup = this._cashe.Current.Get(CapitalGroupHelper.Current.Id);

            capitalGroup.Should().NotBeNull(string.Format("Группа капитальности должно присутствовать в справочнике муниципальных образований.{0}", ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этой группе капитальности отсутствует в справочнике групп капитальности")]
        public void ТоЗаписьПоЭтойГруппеКапитальностиОтсутствуетВСправочникеГруппКапитальности()
        {
            var capitalGroup = this._cashe.Current.Get(CapitalGroupHelper.Current.Id);

            capitalGroup.Should().BeNull(string.Format("Группа капитальности должно отсутствовать в справочнике муниципальных образований.{0}", ExceptionHelper.GetExceptions()));
        }
    }
}
