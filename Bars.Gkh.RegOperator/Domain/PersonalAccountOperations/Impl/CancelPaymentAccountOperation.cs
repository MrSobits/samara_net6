namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    public class CancelPaymentAccountOperation : PersonalAccountOperationBase
    {
        public static string Key
        {
            get { return "CancelPaymentAccountOperation"; }
        }

        public override string Code
        {
            get { return Key; }
        }

        public override string Name
        {
            get { return "Отмена оплаты"; }
        }
    }
}