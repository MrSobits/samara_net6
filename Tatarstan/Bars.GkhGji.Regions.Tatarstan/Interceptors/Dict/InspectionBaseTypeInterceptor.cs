namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class InspectionBaseTypeInterceptor : EmptyDomainInterceptor<InspectionBaseType>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<InspectionBaseType> service, InspectionBaseType entity)
        {
            var inspectionBaseTypeKindCheckDomain = this.Container.ResolveDomain<InspectionBaseTypeKindCheck>();

            using (this.Container.Using(inspectionBaseTypeKindCheckDomain))
            {
                var linkedKindChecks = inspectionBaseTypeKindCheckDomain.GetAll()
                    .Where(x => x.InspectionBaseType.Id == entity.Id)
                    .Select(x => x.Id);

                linkedKindChecks.ForEach(x => inspectionBaseTypeKindCheckDomain.Delete(x));
            }

            return this.Success();
        }
    }
}