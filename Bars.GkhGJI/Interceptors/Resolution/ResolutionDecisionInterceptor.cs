namespace Bars.GkhGji.Interceptors
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using PdfSharp.Pdf.Content.Objects;
    using System.Collections.Generic;
    using System.Linq;

    public class ResolutionDecisionInterceptor : EmptyDomainInterceptor<ResolutionDecision>
    {
		public override IDataResult BeforeDeleteAction(IDomainService<ResolutionDecision> service, ResolutionDecision entity)
		{		
			var longTextService = this.Container.Resolve<IDomainService<ResolutionDecisionLongText>>();

			try
			{

				longTextService.GetAll().Where(x => x.ResolutionDecision.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => longTextService.Delete(x));

				return this.Success();
			}
			finally
			{
				this.Container.Release(longTextService);				
			}
		}

        public override IDataResult AfterCreateAction(IDomainService<ResolutionDecision> service, ResolutionDecision entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ResolutionDecision, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.Id.ToString() + " " + entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ResolutionDecision> service, ResolutionDecision entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ResolutionDecision, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.AppealNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ResolutionDecision> service, ResolutionDecision entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.ResolutionDecision, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.AppealNumber);
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
                { "Resolution", "Постановление" },
                { "AppealNumber", "Номер документа" },
                { "DocumentName", "Имя документа" },
                { "AppealDate", "Дата документа" },
                { "Established", "" },
                { "Decided", "" },
                { "ConsideringBy", "Рассмотрено" },
                { "SignedBy", "Подписано" },
                { "Apellant", "Заявитель" },
                { "ApellantPosition", "Должность заявителя" },
                { "ApellantPlaceWork", "Место работы заявителя" },
                { "TypeDecisionAnswer", "Тип заявления" },
                { "RepresentativeFio", "ФИО представителя" },
            };
            return result;
        }
    }

  
}
