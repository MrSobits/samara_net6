namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;

    public class VprPrescriptionInterceptor : EmptyDomainInterceptor<VprPrescription>
    {
        public override IDataResult AfterCreateAction(IDomainService<VprPrescription> service, VprPrescription entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.VprPrescription, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.Id.ToString() + " " + entity.StateName);
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<VprPrescription> service, VprPrescription entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.VprPrescription, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.DocumentNumber + " " + entity.StateName);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<VprPrescription> service, VprPrescription entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.VprPrescription, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.DocumentNumber + " " + entity.StateName);
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "ROMCategory", "Расчет категории для контрагента" },
                { "Prescription", "Постановление" },
                { "StateName", "Статус предписания" }
            };
            return result;
        }
    }
}