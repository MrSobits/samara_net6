namespace Bars.GkhGji.StateChange
{
    using B4.Modules.States;
    using B4.Utils;
    using Bars.B4.IoC;
    using Castle.Windsor;
    using Contracts;
    using Entities;
    using Enums;

    public class DisposalProsClaimRule : IRuleChangeStatus
    {
        public string Id { get { return "gji_disposal_prosclaim_validation_rule"; } }
        
        public string Name { get { return "Проверка согласования проверки прокуратурой"; } }

        public string TypeId { get { return "gji_document_disp"; } }

        public string Description { get { return "Проверка согласования проверки прокуратурой"; } }

        public DisposalProsClaimRule(IWindsorContainer container)
        {
            Container = container;
        }

        protected readonly IWindsorContainer Container;

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var dispText = Container.Resolve<IDisposalText>();

            var disposal = statefulEntity as Disposal;

            using (Container.Using(dispText))
            {
                if (disposal == null)
                {
                    return ValidateResult.No("Документ не является " + dispText.InstrumentalCase);
                }
            }

            if (disposal.Inspection.Return(x => x.TypeBase) == TypeBase.DisposalHead
                && disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement)
            {
                return disposal.TypeAgreementResult == TypeAgreementResult.NotAgreed
                    ? ValidateResult.Yes()
                    : ValidateResult.No("Проверка согласована с прокуратурой. Требуется проведение проверки");
            }

            return ValidateResult.Yes();
        }
    }
}