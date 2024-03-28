namespace Bars.Gkh.Qa.Steps
{
    using System;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;
    using FluentAssertions;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;

    [Binding]
    class OrganizationFormSteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<OrganizationForm> _cashe = new BindingBase.DomainServiceCashe<OrganizationForm>();

        [Given(@"пользователь добавляет новую организационно-правовую форму")]
        public void ДопустимПользовательДобавляетНовуюОрганизационно_ПравовуюФорму()
        {
            OrganizationFormHelper.CurrentOrganizationForm = new OrganizationForm();
        }

        [Given(@"пользователь у этой организационно-правовой формы заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойОрганизационно_ПравовойФормыЗаполняетПолеНаименование(string orgFormName)
        {
            OrganizationFormHelper.CurrentOrganizationForm.Name = orgFormName;
        }

        [Given(@"пользователь у этой организационно-правовой формы заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтойОрганизационно_ПравовойФормыЗаполняетПолеКод(string orgFormCode)
        {
            OrganizationFormHelper.CurrentOrganizationForm.Code = orgFormCode;
        }

        [Given(@"пользователь у этой организационно-правовой формы заполняет поле Код ОКОПФ ""(.*)""")]
        public void ДопустимПользовательУЭтойОрганизационно_ПравовойФормыЗаполняетПолеКодОКОПФ(string orgFormOkopfCode)
        {
            OrganizationFormHelper.CurrentOrganizationForm.OkopfCode = orgFormOkopfCode;
        }

        [Given(@"добавлена организационно-правовая форма")]
        public void ДопустимДобавленаОрганизационно_ПравоваяФорма(Table orgFormTable)
        {
            var orgForm = orgFormTable.CreateInstance<OrganizationForm>();

            this._cashe.Current.SaveOrUpdate(orgForm);

            OrganizationFormHelper.CurrentOrganizationForm = orgForm;
        }

        [Given(@"пользователь у этой организационно-правовой формы заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойОрганизационно_ПравовойФормыЗаполняетПолеНаименованиеЦифер(int countOfSymbols, string symbol)
        {
            OrganizationFormHelper.CurrentOrganizationForm.Name =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой организационно-правовой формы заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойОрганизационно_ПравовойФормыЗаполняетПолеКодСимволов(int countOfSymbols, string symbol)
        {
            OrganizationFormHelper.CurrentOrganizationForm.Code =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой организационно-правовой формы заполняет поле Код ОКОПФ (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойОрганизационно_ПравовойФормыЗаполняетПолеКодОКОПФСимволов(int countOfSymbols, string symbol)
        {
            OrganizationFormHelper.CurrentOrganizationForm.OkopfCode =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [When(@"пользователь сохраняет эту организационно-правовую форму")]
        public void ЕслиПользовательСохраняетЭтуОрганизационно_ПравовуюФорму()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(OrganizationFormHelper.CurrentOrganizationForm);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет эту организационно-правовую форму")]
        public void ЕслиПользовательУдаляетЭтуОрганизационно_ПравовуюФорму()
        {
            try
            {
                this._cashe.Current.Delete(OrganizationFormHelper.CurrentOrganizationForm.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }


        [Then(@"запись по этой форме присутствует в справочнике организационно-правовых форм")]
        public void ТоЗаписьПоЭтойФормеПрисутствуетВСправочникеОрганизационно_ПравовыхФорм()
        {
            var currentOrgForm = this._cashe.Current.Get(OrganizationFormHelper.CurrentOrganizationForm.Id);

            currentOrgForm.Should().NotBeNull(string.Format("форма должна присутствовать в справочнике организационно-правовых форм.{0}", ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этой форме отсутствует в справочнике организационно-правовых форм")]
        public void ТоЗаписьПоЭтойФормеОтсутствуетВСправочникеОрганизационно_ПравовыхФорм()
        {
            var currentOrgForm = this._cashe.Current.Get(OrganizationFormHelper.CurrentOrganizationForm.Id);

            currentOrgForm.Should().BeNull(string.Format("форма должна отсутствовать в справочнике организационно-правовых форм.{0}", ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этой организационно-правовой форме не сохраняется и падает ошибка сохранения с текстом ""(.*)""")]
        public void ТоЗаписьПоЭтойОрганизационно_ПравовойФормеНеСохраняетсяИПадаетОшибкаСохраненияСТекстом(string exceptionMesage)
        {
            ТоЗаписьПоЭтойФормеОтсутствуетВСправочникеОрганизационно_ПравовыхФорм();
            if (!ExceptionHelper.TestExceptions.ContainsKey("ЕслиПользовательСохраняетЭтуОрганизационно_ПравовуюФорму"))
            {
                throw new Exception("Во время сохранения организационно-правовой формы не выпало ошибок");
            }

            ExceptionHelper.TestExceptions["ЕслиПользовательСохраняетЭтуОрганизационно_ПравовуюФорму"].Should()
                .Be(exceptionMesage, 
                string.Format("Должно выйти сообщение {0}. {1}",
                exceptionMesage, ExceptionHelper.GetExceptions()));
        }
    }
}
