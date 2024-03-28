namespace Bars.Gkh.RegOperator.Wcf.Services
{
    using System.ServiceModel;
    using Contracts.PersonalAccount;

    [ServiceContract]
    public interface IPersonalAccountService
    {
        [OperationContract]
        PersonalAccountInfoOut GetPersonalAccountInfo(PersonalAccountInfoIn arg);

        [OperationContract]
        PersonalAccountPaymentInfoOut PayBill(PersonalAccountPaymentInfoIn arg);
    }
}