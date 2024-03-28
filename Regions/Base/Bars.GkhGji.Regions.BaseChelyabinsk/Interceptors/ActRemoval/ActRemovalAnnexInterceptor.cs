namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    public class ActRemovalAnnexInterceptor : EmptyDomainInterceptor<ActRemovalAnnex>
    {
        public override IDataResult AfterCreateAction(IDomainService<ActRemovalAnnex> service, ActRemovalAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.Id.ToString() + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActRemovalAnnex> service, ActRemovalAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.DocumentNumber + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActRemovalAnnex> service, ActRemovalAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.DocumentNumber + " " + entity.Name);
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
                { "DocumentDate", "Дата документа" },
                { "Name", "Наименование" },
                { "Description", "Описание" },
                { "File", "Файл" },
                { "SignedFile", "Подписанный файл" }
            };
            return result;
        }
    }
}