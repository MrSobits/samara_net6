namespace Bars.Gkh.RegOperator.Interceptors.Decision
{
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.RegOperator.Entities.Decisions;
    using Bars.Gkh.RegOperator.Enums;

    public sealed class UltimateDecisionInterceptor<T> : EmptyDomainInterceptor<T> where T : UltimateDecision
    {
        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            var domainService = Container.ResolveDomain<CoreDecision>();
            domainService.Save(new CoreDecision
            {
                DecisionType = CoreDecisionType.Owners,
                UltimateDecision = entity
            });

            return base.AfterCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var coreDecisionDomain = Container.ResolveDomain<CoreDecision>();

            coreDecisionDomain.GetAll()
                .Where(x => x.UltimateDecision.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => coreDecisionDomain.Delete(x));

            return Success();
        }
    }
}