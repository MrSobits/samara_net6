namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    public class ActRemovalWitnessInterceptor : EmptyDomainInterceptor<ActRemovalWitness>
    {
        public override IDataResult AfterCreateAction(IDomainService<ActRemovalWitness> service, ActRemovalWitness entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiWitness, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.Id.ToString() + " " + entity.Fio);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActRemovalWitness> service, ActRemovalWitness entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiWitness, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.DocumentNumber + " " + entity.Fio);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActRemovalWitness> service, ActRemovalWitness entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiWitness, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.DocumentNumber + " " + entity.Fio);
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
                { "ActRemoval", "Акт проверки предписания" },
                { "Position", "Должность" },
                { "Fio", "ФИО" }
            };
            return result;
        }
    }
}