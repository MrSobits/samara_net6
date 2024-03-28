namespace Bars.Gkh.RegOperator.AccountNumberGenerator
{
    using System.Collections.Generic;
    using Entities;

    public interface IAccountNumberGenerator
    {
        void Generate(BasePersonalAccount account);

        void Generate(ICollection<BasePersonalAccount> accounts);
    }
}