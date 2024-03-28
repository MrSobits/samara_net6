namespace Bars.Gkh.RegOperator.Domain.ProxyEntity
{
    using Wcf.Contracts.PersonalAccount;

    public class PaymentInProxy : PersonalAccountPaymentInfoIn
    {
        public override AccountType OwnerType
        {
            get
            {
                return AccountType.Personal;
            }
            set { }
        }
    }
}