namespace Bars.Gkh.RegOperator.Exceptions
{
    public class MoneyUnlockException : WalletException
    {
        public MoneyUnlockException()
        {

        }

        public MoneyUnlockException(string message)
            : base(message)
        {

        }
    }
}
