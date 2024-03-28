namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// Interceptor для <see cref="VisitSheetViolationInfo"/>
    /// </summary>
    public class VisitSheetViolationInfoInterceptor : EmptyDomainInterceptor<VisitSheetViolationInfo>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<VisitSheetViolationInfo> service, VisitSheetViolationInfo entity)
        {
            var visitSheetViolationDomain = this.Container.ResolveDomain<VisitSheetViolation>();

            using (this.Container.Using(visitSheetViolationDomain))
            {
                visitSheetViolationDomain.GetAll()
                    .Where(x => x.ViolationInfo.Id == entity.Id)
                    .ToList()
                    .ForEach(x => visitSheetViolationDomain.Delete(x.Id));
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}