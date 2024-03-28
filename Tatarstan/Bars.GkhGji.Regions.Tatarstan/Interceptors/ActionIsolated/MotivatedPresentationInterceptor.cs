namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    using NHibernate.Util;

    public class MotivatedPresentationInterceptor : DocumentGjiInterceptor<MotivatedPresentation>
    {
        public override IDataResult BeforeCreateAction(IDomainService<MotivatedPresentation> service, MotivatedPresentation entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.CreationPlace);
            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<MotivatedPresentation> service, MotivatedPresentation entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.CreationPlace);
            return base.BeforeUpdateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<MotivatedPresentation> service, MotivatedPresentation entity)
        {
            var violationDomain = this.Container.ResolveDomain<MotivatedPresentationViolation>();
            var roDomain = this.Container.ResolveDomain<MotivatedPresentationRealityObject>();
            var annexDomain = this.Container.ResolveDomain<MotivatedPresentationAnnex>();

            using (this.Container.Using(violationDomain, roDomain, annexDomain))
            {
                var roIdsToDelete = roDomain.GetAll()
                    .Where(x => x.MotivatedPresentation.Id == entity.Id)
                    .Select(x => x.Id);
                var violationIdsToDelete = violationDomain.GetAll()
                    .Where(x => roIdsToDelete.Any(y => x.MotivatedPresentationRealityObject.Id == y))
                    .Select(x => x.Id)
                    .ToList();
                var annexIdsToDelete = annexDomain.GetAll()
                    .Where(x => x.MotivatedPresentation.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();
                
                violationIdsToDelete.ForEach(x => violationDomain.Delete(x));
                roIdsToDelete.ToList().ForEach(x => roDomain.Delete(x));
                annexIdsToDelete.ForEach(x => annexDomain.Delete(x));
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}