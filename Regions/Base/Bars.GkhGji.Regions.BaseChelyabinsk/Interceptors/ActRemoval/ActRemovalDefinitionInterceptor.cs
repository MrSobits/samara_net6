namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    public class ActRemovalDefinitionInterceptor : EmptyDomainInterceptor<ActRemovalDefinition>
    {
        public override IDataResult AfterCreateAction(IDomainService<ActRemovalDefinition> service, ActRemovalDefinition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.Id.ToString() + " " + entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActRemovalDefinition> service, ActRemovalDefinition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.DocumentNumber + " " + entity.DocumentNum);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActRemovalDefinition> service, ActRemovalDefinition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiDefinition, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.DocumentNumber + " " + entity.DocumentNum);
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
                { "ExecutionDate", "Дата исполнения" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNum", "Номер документа" },
                { "Description", "Описание" },
                { "IssuedDefinition", "ДЛ, вынесшее определение" },
                { "TypeDefinition", "Тип определения" }
            };
            return result;
        }
    }
}