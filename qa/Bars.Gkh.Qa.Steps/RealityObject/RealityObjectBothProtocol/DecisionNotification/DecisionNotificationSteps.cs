namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    internal class DecisionNotificationSteps : BindingBase
    {
        private DomainServiceCashe<DecisionNotification> _cashe = new DomainServiceCashe<DecisionNotification>();
        
        [Given(@"пользователь у этого уведомления протокола собственников заполняет поле Исх. номер ""(.*)""")]
        public void ДопустимПользовательУЭтогоУведомленияПротоколаСобственниковЗаполняетПолеИсхНомер(string number)
        {
            DecisionNotificationHelper.Current.Number = number;
        }

        [Given(@"пользователь у этого уведомления протокола собственников заполняет поле Номер счета ""(.*)""")]
        public void ДопустимПользовательУЭтогоУведомленияПротоколаСобственниковЗаполняетПолеНомерСчета(string accountNum)
        {
            DecisionNotificationHelper.Current.AccountNum = accountNum;
        }

        [When(@"пользователь сохраняет это уведомление протокола собственников")]
        public void ЕслиПользовательСохраняетЭтоУведомлениеПротоколаСобственников()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(DecisionNotificationHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException("DomainService<DecisionNotification>.SaveOrUpdate", ex.Message);
            }
        }
    }
}
