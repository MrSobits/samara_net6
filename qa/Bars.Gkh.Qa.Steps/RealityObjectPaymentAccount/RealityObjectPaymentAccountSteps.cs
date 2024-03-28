namespace Bars.Gkh.Qa.Steps
{
    using System;

    using TechTalk.SpecFlow;

    [Binding]
    internal class RealityObjectPaymentAccountSteps
    {
        [Given(@"добавлен новый счёт оплат текущего дома")]
        public void ДопустимДобавленНовыйСчётОплатТекущегоДома()
        {
            dynamic realityObjectPaymentAccount = 
                Activator.CreateInstance(RealityObjectPaymentAccountHelper.Type, RealityObjectHelper.CurrentRealityObject);
            
            RealityObjectPaymentAccountHelper.DomainService.Save(realityObjectPaymentAccount);

            RealityObjectPaymentAccountHelper.Current = realityObjectPaymentAccount;
        }
    }
}
