namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    using NHibernate.Util;

    public class KnmTypesInterceptor : EmptyDomainInterceptor<KnmTypes>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<KnmTypes> service, KnmTypes entity)
        {
            var knmTypeKindCheckDomain = this.Container.ResolveDomain<KnmTypeKindCheck>();

            using (this.Container.Using(knmTypeKindCheckDomain))
            {
                knmTypeKindCheckDomain.GetAll().Where(x=> x.KnmTypes.Id == entity.Id)
                    .Select(x=>x.Id).ForEach(x=> knmTypeKindCheckDomain.Delete(x));
            }
            
            return base.BeforeDeleteAction(service, entity);
        }
    }
}