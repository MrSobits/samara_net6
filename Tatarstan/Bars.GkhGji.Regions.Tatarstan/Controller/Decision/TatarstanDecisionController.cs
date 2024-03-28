namespace Bars.GkhGji.Regions.Tatarstan.Controller.Decision
{
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    public class TatarstanDecisionController : TatarstanDisposalController<TatarstanDecision>
    {
        /// <remarks>
        /// Подменен компонентом, зарегистрированным в контейнере под IDecisionService
        /// </remarks>
        public override IDisposalService DisposalService => this.DecisionService;

        public Bars.GkhGji.Regions.Tatarstan.DomainService.Decision.IDecisionService DecisionService { get; set; }
    }
}