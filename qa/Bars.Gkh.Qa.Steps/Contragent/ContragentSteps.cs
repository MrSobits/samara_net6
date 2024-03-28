namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;

    using TechTalk.SpecFlow;
    using FluentAssertions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow.Assist;

    [Binding]
    class ContragentSteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<Contragent> _cashe = new BindingBase.DomainServiceCashe<Contragent>();

        [Given(@"пользователь добавляет нового контрагента")]
        public void ДопустимПользовательДобавляетНовогоКонтрагента()
        {
            ContragentHelper.CurrentContragent = new Contragent();
        }

        [Given(@"пользователь у этого контрагента заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоКонтрагентаЗаполняетПолеНаименование(string name)
        {
            ContragentHelper.CurrentContragent.Name = name;
        }


        [Given(@"пользователь у этого контрагента заполняет поле Организационно-правовая форма")]
        public void ДопустимПользовательУЭтогоКонтрагентаЗаполняетПолеОрганизационно_ПравоваяФорма()
        {
            ContragentHelper.CurrentContragent.OrganizationForm = OrganizationFormHelper.CurrentOrganizationForm;
        }

        [Given(@"пользователь у этого контрагента заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоКонтрагентаЗаполняетПолеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            ContragentHelper.CurrentContragent.Name =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"добавлен контрагент c текущей организационно-правовой формой")]
        public void ДопустимДобавленКонтрагентCЭтойОрганизационно_ПравовойФормой(Table table)
        {
            var data = table.Rows.First();

            var contragent = new Contragent
                                 {
                                     Name = data["Name"],
                                     OrganizationForm = OrganizationFormHelper.CurrentOrganizationForm
                                 };

            this._cashe.Current.Save(contragent);

            ContragentHelper.CurrentContragent = contragent;
        }

        [When(@"пользователь сохраняет этого контрагента")]
        public void ЕслиПользовательСохраняетЭтогоКонтрагента()
        {
            try
            {
                _cashe.Current.SaveOrUpdate(ContragentHelper.CurrentContragent);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому контрагенту присутствует в реестре контрагентов")]
        public void ТоЗаписьПоЭтойФормеПрисутствуетВРеестреКонтрагентов()
        {
            var currentContragent = this._cashe.Current.Get(ContragentHelper.CurrentContragent.Id);

            currentContragent.Should()
                .NotBeNull(string.Format("контрагент должен присутствовать в реестре контрагентов.{0}", ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому контрагенту отсутствует в реестре контрагентов")]
        public void ТоЗаписьПоЭтойФормеОтсутствуетВРеестреКонтрагентов()
        {
            var currentContragent = this._cashe.Current.Get(ContragentHelper.CurrentContragent.Id);

            currentContragent.Should()
                .BeNull(string.Format("контрагент должен отсутствовать в реестре контрагентов.{0}", ExceptionHelper.GetExceptions()));
        }

        [When(@"пользователь удаляет этого контрагента")]
        public void ЕслиПользовательУдаляетЭтогоКонтрагента()
        {
            try
            {
                _cashe.Current.Delete(ContragentHelper.CurrentContragent.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }


        [Then(@"запись по этому контрагенту не сохраняется и падает ошибка сохранения с текстом ""(.*)""")]
        public void ТоЗаписьПоЭтойФормеНеСохраняетсяИПадаетОшибкаСохраненияСТекстом(string exceptionMesage)
        {
            ТоЗаписьПоЭтойФормеОтсутствуетВРеестреКонтрагентов();

            if (!ExceptionHelper.TestExceptions.ContainsKey("ЕслиПользовательСохраняетЭтогоКонтрагента"))
            {
                throw new Exception("Во время сохранения контрагента не выпало ошибок");
            }

            ExceptionHelper.TestExceptions["ЕслиПользовательСохраняетЭтогоКонтрагента"]
                .Should().Be(exceptionMesage, 
                string.Format("ошибка должна быть {0}. {1}",
                exceptionMesage, ExceptionHelper.GetExceptions()));
        }
    }
}
