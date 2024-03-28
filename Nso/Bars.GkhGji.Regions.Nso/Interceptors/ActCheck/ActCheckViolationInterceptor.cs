namespace Bars.GkhGji.Regions.Nso.Interceptors
{
    using B4;
    using B4.DataAccess;
    using GkhGji.Entities;

    public class ActCheckViolationInterceptor : EmptyDomainInterceptor<ActCheckViolation>
    {
        /// <summary>
        /// Метод вызывается после обновления объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterUpdateAction(IDomainService<ActCheckViolation> service, ActCheckViolation entity)
        {
            var inspectionViolDomain = Container.ResolveDomain<InspectionGjiViol>();

            try
            {
                entity.InspectionViolation.DatePlanRemoval = entity.DatePlanRemoval;

                inspectionViolDomain.Update(entity.InspectionViolation);
            }
            finally
            {
                Container.Release(inspectionViolDomain);
            }

            return Success();
        }
    }
}