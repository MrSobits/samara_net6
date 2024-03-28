namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using System.Linq;
    public class DecisionAnnexInterceptor : EmptyDomainInterceptor<DecisionAnnex>
    {
        public override IDataResult AfterCreateAction(IDomainService<DecisionAnnex> service, DecisionAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.Id.ToString() + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<DecisionAnnex> service, DecisionAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DecisionAnnex> service, DecisionAnnex entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiAnnex, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<DecisionAnnex> service, DecisionAnnex entity)
        {
            var signedInfoDomain = Container.ResolveDomain<PdfSignInfo>();
            try
            {
                var signed = signedInfoDomain.GetAll().Where(x => x.OriginalPdf == entity.File).Select(x=> x.Id).ToList();
                foreach (long id in signed)
                {
                    signedInfoDomain.Delete(id);
                }
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
                { "Decision", "Распоряжение" },
                { "TypeAnnex", "Тип приложения" },
                { "DocumentDate", "Дата документа" },
                { "Name", "Наименование" },
                { "Description", "Описание" },
                { "File", "Файл" },
                { "SignedFile", "Подписанный файл" },
                { "MessageCheck", "Статус файла" }
            };
            return result;
        }
    }
}