namespace Bars.GkhGji.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Entities;
    using System.Collections.Generic;

    public class RelatedAppealCitsInterceptor : EmptyDomainInterceptor<RelatedAppealCits>
    {
        public override IDataResult AfterCreateAction(IDomainService<RelatedAppealCits> service, RelatedAppealCits entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.RelatedAppealCits, entity.Id, entity.GetType(), GetPropertyValues(), entity.Parent.Id.ToString() + " " + entity.Children.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<RelatedAppealCits> service, RelatedAppealCits entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.RelatedAppealCits, entity.Id, entity.GetType(), GetPropertyValues(), entity.Parent.Id.ToString() + " " + entity.Children.DocumentNumber);
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
                { "Parent", "Родительское обращение" },
                { "Children", "Дочернее обращение" }
            };
            return result;
        }

    }
}
