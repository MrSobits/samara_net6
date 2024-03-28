namespace Bars.GkhGji.Regions.Tatarstan.StateChange
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;

    public class ActRemovalValidationRule : Bars.GkhGji.StateChange.ActRemovalValidationRule
    {
        public override ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var result = new ValidateResult();

            if (statefulEntity is ActRemoval)
            {
                var actRemoval = statefulEntity as ActRemoval;

                if (Container.Resolve<IDomainService<ActRemovalViolation>>().GetAll()
                    .Count(x => x.Document.Id == actRemoval.Id && x.DateFactRemoval != null && x.SumAmountWorkRemoval == null) > 0)
                {
                    result.Message = "Укажите сумму работ по устранению нарушения";
                    result.Success = false;
                    return result;
                }
            }

            return base.Validate(statefulEntity, oldState, newState);
        }
    }
}