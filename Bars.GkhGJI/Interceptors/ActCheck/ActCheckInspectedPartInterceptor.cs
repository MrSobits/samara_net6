namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActCheckInspectedPartInterceptor : EmptyDomainInterceptor<ActCheckInspectedPart>
    {
        public override IDataResult AfterCreateAction(IDomainService<ActCheckInspectedPart> service, ActCheckInspectedPart entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiInspectedPart, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.Id.ToString() + " " + entity.InspectedPart.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActCheckInspectedPart> service, ActCheckInspectedPart entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiInspectedPart, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.DocumentNumber + " " + entity.InspectedPart.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActCheckInspectedPart> service, ActCheckInspectedPart entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiInspectedPart, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.DocumentNumber + " " + entity.InspectedPart.Name);
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
                { "ActCheck", "Акт обследования" },
                { "InspectedPart", "Инспектируемая часть" },
                { "Character", "Характер и местоположение" },
                { "Description", "Примечание" }
            };
            return result;
        }
    }
}