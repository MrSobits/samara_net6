namespace Bars.Gkh.Qa.Steps
{
    using System;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;
    using FluentAssertions;

    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;

    [Binding]
    class MunicipalitySteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<Municipality> _cashe = new BindingBase.DomainServiceCashe<Municipality>();

        private IMunicipalityService service;

        private IMunicipalityService Service
        {
            get { return service ?? (service = Container.Resolve<IMunicipalityService>()); }
        }

        [Given(@"пользователь добавляет новое муниципальное образование")]
        public void ДопустимПользовательДобавляетНовоеМуниципальноеОбразование()
        {
            MunicipalityHelper.Current = new Municipality();
        }

        [Given(@"пользователь у этого муниципального образования заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоМуниципальногоОбразованияЗаполняетПолеНаименование(string name)
        {
            MunicipalityHelper.Current.Name = name;
        }

        [Given(@"пользователь у этого муниципального образования заполняет поле Регион ""(.*)""")]
        public void ДопустимПользовательУЭтогоМуниципальногоОбразованияЗаполняетПолеРегион(string region)
        {
            MunicipalityHelper.Current.RegionName = region;
        }

        [Given(@"пользователь у этого муниципального образования заполняет поле Группа ""(.*)""")]
        public void ДопустимПользовательУЭтогоМуниципальногоОбразованияЗаполняетПолеГруппа(string group)
        {
            MunicipalityHelper.Current.Group = group;
        }

        [Given(@"пользователь у этого муниципального образования заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоМуниципальногоОбразованияЗаполняетПолеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            MunicipalityHelper.Current.Name =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого муниципального образования заполняет поле Группа (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоМуниципальногоОбразованияЗаполняетПолеГруппаСимволов(int countOfSymbols, string symbol)
        {
            MunicipalityHelper.Current.Group =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"добавлено муниципальное образование")]
        public void ДопустимДобавленоМуниципальноеОбразование(Table municipalityTable)
        {
            var municipality = municipalityTable.CreateInstance<Municipality>();

            _cashe.Current.Save(municipality);

            MunicipalityHelper.Current = municipality;
        }

        [When(@"пользователь сохраняет это муниципальное образование")]
        public void ЕслиПользовательСохраняетЭтоМуниципальноеОбразование()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(MunicipalityHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет это муниципальное образование")]
        public void ЕслиПользовательУдаляетЭтоМуниципальноеОбразование()
        {
            try
            {
                this._cashe.Current.Delete(MunicipalityHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому муниципальному образованию присутствует в справочнике муниципальных образований")]
        public void ТоЗаписьПоЭтомуМуниципальномуОбразованиюПрисутствуетВСправочникеМуниципальныхОбразований()
        {
            var municipality = this._cashe.Current.Get(MunicipalityHelper.Current.Id);

            municipality.Should().NotBeNull(string.Format("Муниципальное образование должно присутствовать в справочнике муниципальных образований.{0}", ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому муниципальному образованию отсутствует в справочнике муниципальных образований")]
        public void ТоЗаписьПоЭтомуМуниципальномуОбразованиюОтсутствуетВСправочникеМуниципальныхОбразований()
        {
            var municipality = this._cashe.Current.Get(MunicipalityHelper.Current.Id);

            municipality.Should().BeNull(string.Format("Муниципальное образование должно отсутствовать в справочнике муниципальных образований.{0}", ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому муниципальному образованию не сохраняется и падает ошибка с текстом ""(.*)""")]
        public void ТоЗаписьПоЭтомуМуниципальномуОбразованиюНеСохраняетсяИПадаетОшибкаСТекстом(string exceptionMesage)
        {
            ТоЗаписьПоЭтомуМуниципальномуОбразованиюОтсутствуетВСправочникеМуниципальныхОбразований();

            if (!ExceptionHelper.TestExceptions.ContainsKey("ЕслиПользовательСохраняетЭтоМуниципальноеОбразование"))
            {
                throw new Exception("Во время сохранения контрагента не выпало ошибок");
            }

            ExceptionHelper.TestExceptions["ЕслиПользовательСохраняетЭтоМуниципальноеОбразование"].Should()
                .Be(
                    exceptionMesage,
                    string.Format(
                        "ошибка должна быть {0}. {1}",
                        exceptionMesage,
                        ExceptionHelper.GetExceptions()));
        }
    }
}
