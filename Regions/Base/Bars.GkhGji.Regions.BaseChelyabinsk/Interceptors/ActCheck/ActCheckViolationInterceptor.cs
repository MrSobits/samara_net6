namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.ActCheck
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

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
            var inspectionViolDomain = this.Container.ResolveDomain<InspectionGjiViol>();

            try
            {
                entity.InspectionViolation.DatePlanRemoval = entity.DatePlanRemoval;

                inspectionViolDomain.Update(entity.InspectionViolation);
            }
            finally
            {
                this.Container.Release(inspectionViolDomain);
            }

            return this.Success();
        }
    }
}