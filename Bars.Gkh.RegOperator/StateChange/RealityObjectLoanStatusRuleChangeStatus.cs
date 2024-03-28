namespace Bars.Gkh.RegOperator.StateChange
{
    using B4;
    using B4.Modules.States;
    using Entities;
    using DomainModelServices;

    public class RealityObjectLoanStatusRuleChangeStatus : IRuleChangeStatus
    {
        private readonly ITakeLoanService _takeLoanService;

        public RealityObjectLoanStatusRuleChangeStatus(ITakeLoanService takeLoanService)
        {
            _takeLoanService = takeLoanService;
        }

        public string Id
        {
            get { return "RealityObjectLoanStatusRuleChangeStatus"; }
        }

        public string Name
        {
            get { return "Производим перевод денег"; }
        }

        public string Description
        {
            get { return "Производим перевод денег"; }
        }

        public string TypeId
        {
            get { return "gkh_regop_reality_object_loan"; }
        }
        
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var loan = statefulEntity as RealityObjectLoan;
            if (loan != null)
            {
                try
                {
                    _takeLoanService.TakeLoan(loan);
                }
                catch (ValidationException exc)
                {
                    return ValidateResult.No(exc.Message);
                }
            }

            return ValidateResult.Yes();
        }
    }
}