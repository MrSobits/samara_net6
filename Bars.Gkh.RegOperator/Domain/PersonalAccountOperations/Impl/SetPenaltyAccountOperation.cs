namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using B4;

    using Bars.Gkh.RegOperator.DomainModelServices;

    public class SetPenaltyAccountOperation : PersonalAccountOperationBase
    {

        private IPersonalAccountChangeService _accountChangeService { get; set; }

        public SetPenaltyAccountOperation(IPersonalAccountChangeService accountChangeService)
        {
            _accountChangeService = accountChangeService;
        }

        public static string Key
        {
            get { return "SetPenaltyAccountOperation"; }
        }

        public override string Code
        {
            get { return Key; }
        }

        public override string Name
        {
            get { return "Установка и изменение пени"; }
        }

        public override IDataResult Execute(BaseParams baseParams)
        {
            return _accountChangeService.ChangePenalty(baseParams);
        }
    }
}