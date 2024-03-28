namespace Bars.Gkh.RegOperator.DomainService
{
    using Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount;

    public interface IPersonalAccountInfoService
    {
        PersonalAccountInfoOut GetPersonalAccountInfo(PersonalAccountInfoIn personalAccountInfo);
    }
}