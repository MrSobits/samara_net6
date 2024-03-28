namespace Bars.GisIntegration.Base.Interceptors
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.GisRole;
    using System.Linq;
    using Bars.B4.Utils;

    /// <summary>
    /// Interceptor для GisOperator
    /// </summary>
    public class GisOperatorInterceptor : EmptyDomainInterceptor<GisOperator>
    {
        /// <summary>
        /// Событие перед удалением GisOperator
        /// </summary>
        /// <param name="service">Домен GisOperator</param>
        /// <param name="entity">Удаляемый оператор</param>
        /// <returns>Результат выполнения операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<GisOperator> service, GisOperator entity)
        {
            var risContragentRoleDomain = this.Container.ResolveDomain<RisContragentRole>();

            risContragentRoleDomain.GetAll()
                .Where(x => x.GisOperator.Id == entity.Id)
                .Select(x => x.Id).AsEnumerable()
                .ForEach(x => risContragentRoleDomain.Delete(x));

            return this.Success();
        }

        /// <summary>
        /// Событие перед созданием GisOperator
        /// </summary>
        /// <param name="service">Домен GisOperator</param>
        /// <param name="entity">Удаляемый оператор</param>
        /// <returns>Результат выполнения операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<GisOperator> service, GisOperator entity)
        {
            if (service.GetAll().Any(x => x.Id != entity.Id && x.Contragent == entity.Contragent))
            {
                return this.Failure("Запись с таким контрагентом уже существует");
            }

            return this.Success();
        }
    }
}
