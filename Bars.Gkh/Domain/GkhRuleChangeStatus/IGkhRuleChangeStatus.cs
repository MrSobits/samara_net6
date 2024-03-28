namespace Bars.Gkh.Domain
{
    using Bars.B4;
    using Bars.B4.Modules.States;

    public interface IGkhRuleChangeStatus : IRuleChangeStatus
    {
        void HandleUserParams(BaseParams baseParams, IStatefulEntity entity);
    }
}