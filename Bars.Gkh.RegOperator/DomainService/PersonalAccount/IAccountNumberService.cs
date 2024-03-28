namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System.Collections.Generic;
    using Entities;

    public interface IAccountNumberService
    {
        void Generate(BasePersonalAccount account);

        void Generate(ICollection<BasePersonalAccount> accounts);
    }
}