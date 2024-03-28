namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    public class PreventiveActionTaskInterceptor : DocumentGjiInterceptor<PreventiveActionTask>
    {
        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<PreventiveActionTask> service, PreventiveActionTask entity)
        {
            if (entity.ActionLocation != null && entity.ActionLocation.Id == 0)
            {
                Utils.SaveFiasAddress(this.Container, entity.ActionLocation);
            }
            
            if (entity.DocumentDate.HasValue)
            {
                entity.DocumentYear = entity.DocumentDate.Value.Year;
            }

            return base.BeforeUpdateAction(service, entity);
        }
        
        /// <inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<PreventiveActionTask> service, PreventiveActionTask entity)
        {
            var inspectionDomain = this.Container.ResolveDomain<InspectionGji>();
            var documentDomain = this.Container.ResolveDomain<DocumentGji>();
            var fiasAddressDomain = this.Container.ResolveDomain<FiasAddress>();

            using (this.Container.Using(inspectionDomain, documentDomain, fiasAddressDomain))
            {
                var result = base.AfterDeleteAction(service, entity);
                
                var inspectionId = entity.Inspection.Id;
                if (inspectionId > 0)
                {
                    if (!documentDomain.GetAll().Any(x => x.Inspection.Id == inspectionId))
                    {
                        inspectionDomain.Delete(inspectionId);
                    }
                }

                var addressId = entity.ActionLocation?.Id ?? 0;
                if (addressId > 0)
                {
                    fiasAddressDomain.Delete(addressId);
                }

                return result;
            }
        }
    }
}