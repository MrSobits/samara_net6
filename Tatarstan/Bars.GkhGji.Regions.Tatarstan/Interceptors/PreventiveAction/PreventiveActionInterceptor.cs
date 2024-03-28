namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    using Gkh.Utils;

    public class PreventiveActionInterceptor : DocumentGjiInterceptor<PreventiveAction>
    {/// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<PreventiveAction> service, PreventiveAction entity)
        {
            IDomainService<InspectionGji> domainInspection;
            IDomainService<InspectionGjiStage> domainStage;

            using (this.Container.Using(domainStage = this.Container.Resolve<IDomainService<InspectionGjiStage>>(),
                domainInspection = this.Container.Resolve<IDomainService<InspectionGji>>()))
            {
                var inspection = new InspectionGji
                {
                    TypeBase = TypeBase.PreventiveAction
                };

                domainInspection.Save(inspection);

                var stage = new InspectionGjiStage
                {
                    Inspection = inspection,
                    TypeStage = TypeStage.PreventiveAction
                };

                domainStage.Save(stage);

                entity.Inspection = inspection;
                entity.Stage = stage;

                if (entity.DocumentDate.HasValue)
                {
                    entity.DocumentYear = entity.DocumentDate.Value.Year;
                }

                return base.BeforeCreateAction(service, entity);
            }
        }
        
        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<PreventiveAction> service, PreventiveAction entity)
        {
            if (entity.ControlledPersonAddress != null && entity.ControlledPersonAddress.Id == 0)
            {
                Utils.SaveFiasAddress(this.Container, entity.ControlledPersonAddress);
            }
            
            if (entity.DocumentDate.HasValue)
            {
                entity.DocumentYear = entity.DocumentDate.Value.Year;
            }

            return base.BeforeUpdateAction(service, entity);
        }
        
        /// <inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<PreventiveAction> service, PreventiveAction entity)
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

                var addressId = entity.ControlledPersonAddress?.Id ?? 0;
                if (addressId > 0)
                {
                    fiasAddressDomain.Delete(addressId);
                }

                return result;
            }
        }
    }
}