namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.ActCheck
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    public class ActCheckServiceInterceptor : ActCheckServiceInterceptor<ActCheck>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActCheck> service, ActCheck entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DocumentPlaceFias);

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ActCheck> service, ActCheck entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DocumentPlaceFias);

            return base.BeforeUpdateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<ActCheck> service, ActCheck entity)
        {
            var actCheckActionDomain = this.Container.ResolveDomain<ActCheckAction>();
            
            using (this.Container.Using(actCheckActionDomain))
            {
                actCheckActionDomain.GetAll()
                    .Where(x => x.ActCheck.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => actCheckActionDomain.Delete(x));
                
                return base.BeforeDeleteAction(service, entity);
            }
        }
    }
}