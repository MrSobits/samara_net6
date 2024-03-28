namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Disposal;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    using NHibernate.Linq;

    public class DecisionInterceptor : TatDisposalBaseInterceptor<TatarstanDecision>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<TatarstanDecision> service, TatarstanDecision entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DecisionPlace);

            return base.BeforeCreateAction(service, entity);
        }
        
        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<TatarstanDecision> service, TatarstanDecision entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DecisionPlace);

            return base.BeforeUpdateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<TatarstanDecision> service, TatarstanDecision entity)
        {
            var decisionKnmActionDomain = this.Container.ResolveDomain<DecisionKnmAction>();

            using (this.Container.Using(decisionKnmActionDomain))
            {
                decisionKnmActionDomain.GetAll()
                    .Where(x => x.MainEntity.Id == entity.Id)
                    .Delete();
            }
            
            return base.BeforeDeleteAction(service, entity);
        }
    }
}