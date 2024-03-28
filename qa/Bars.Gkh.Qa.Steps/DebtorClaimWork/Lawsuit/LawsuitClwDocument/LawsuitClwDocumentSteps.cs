namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    internal class LawsuitClwDocumentSteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<LawsuitClwDocument> _cashe = new BindingBase.DomainServiceCashe<LawsuitClwDocument>();

        [Given(@"у этой записи по документу Искового заявления заполняет поле Документ ""(.*)""")]
        public void ДопустимУЭтойЗаписиПоДокументуИсковогоЗаявленияЗаполняетПолеДокумент(string docName)
        {
            LawsuitClwDocumentHelper.Current.DocName = docName;
        }

        [Given(@"у этой записи по документу Искового заявления заполняет поле Дата ""(.*)""")]
        public void ДопустимУЭтойЗаписиПоДокументуИсковогоЗаявленияЗаполняетПолеДата(DateTime docDate)
        {
            LawsuitClwDocumentHelper.Current.DocDate = docDate;
        }

        [When(@"пользователь сохраняет эту запись по документу Искового заявления")]
        public void ЕслиПользовательСохраняетЭтуЗаписьПоДокументуИсковогоЗаявления()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(LawsuitClwDocumentHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
