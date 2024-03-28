namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.Disposal
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    // интерцептор необходимо тоже перекрыть поскольку сущность ChelyabinskDisposal должна выполнить базовый код интерцептора Disposal
    public class ChelyabinskDisposalServiceInterceptor : DisposalServiceInterceptor<ChelyabinskDisposal>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ChelyabinskDisposal> service, ChelyabinskDisposal entity)
        {
            entity.NcObtained = YesNo.No;
            entity.NcSent = YesNo.No;

            return base.BeforeCreateAction(service, entity);
        }
        
        public override IDataResult BeforeDeleteAction(IDomainService<ChelyabinskDisposal> service, ChelyabinskDisposal entity)
        {
            // Получаем предметы проверки и удаляем их
            var serviceSubj = this.Container.Resolve<IDomainService<DisposalVerificationSubject>>();
            var serviceDocs = this.Container.Resolve<IDomainService<DisposalDocConfirm>>();
            var serviceLongText = this.Container.Resolve<IDomainService<DisposalLongText>>();
            var factViols = this.Container.ResolveDomain<DisposalFactViolation>();
            var surveyObjectivesDomain = this.Container.ResolveDomain<DisposalSurveyObjective>();
            var surveyPurposeDomain = this.Container.ResolveDomain<DisposalSurveyPurpose>();

            try
            {
                // удаляем субтаблицы добавленные в регионе НСО

                serviceSubj.GetAll()
                .Where(x => x.Disposal.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => serviceSubj.Delete(x));

                serviceDocs.GetAll()
                .Where(x => x.Disposal.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => serviceDocs.Delete(x));

                serviceLongText.GetAll()
                .Where(x => x.Disposal.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => serviceLongText.Delete(x));

                factViols.GetAll()
                    .Where(x => x.Disposal.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => factViols.Delete(x));

                surveyObjectivesDomain.GetAll()
                    .Where(x => x.Disposal.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => surveyObjectivesDomain.Delete(x));

                surveyPurposeDomain.GetAll()
                    .Where(x => x.Disposal.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => surveyPurposeDomain.Delete(x));

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                this.Container.Release(serviceSubj);
                this.Container.Release(serviceDocs);
                this.Container.Release(serviceLongText);
                this.Container.Release(factViols);
                this.Container.Release(surveyObjectivesDomain);
                this.Container.Release(surveyPurposeDomain);
            }
            
        }

        public override IDataResult AfterCreateAction(IDomainService<ChelyabinskDisposal> service, ChelyabinskDisposal entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ChelyabinskDisposal> service, ChelyabinskDisposal entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ChelyabinskDisposal> service, ChelyabinskDisposal entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
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
                { "Inspection", "Проверка ГЖИ" },
                { "Stage", "Этап проверки" },
                { "TypeDocumentGji", "Тип документа ГЖИ" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNumber", "Номер документа" },
                { "DocumentSubNum", "Дополнительный номер документа (порядковый номер если документов одного типа несколько)" },
                { "State", "Статус" },
                { "TypeDisposal", "Тип распоряжения" },
                { "DateStart", "Дата начала обследования" },
                { "DateEnd", "Дата окончания обследования" },
                { "TypeAgreementProsecutor", "Согласование с прокуротурой" },
                { "DocumentNumberWithResultAgreement", "Номер документа с результатом согласования" },
                { "TypeAgreementResult", "Результат согласования" },
                { "DocumentDateWithResultAgreement", "Дата документа с результатом согласования" },
                { "IssuedDisposal", "Должностное лицо (ДЛ) вынесшее распоряжение" },
                { "ResponsibleExecution", "Ответственный за исполнение" },
                { "KindCheck", "Вид проверки" },
                { "Description", "Описание" },
                { "ObjectVisitStart", "Выезд на объект с" },
                { "ObjectVisitEnd", "Выезд на объект по" },
                { "TimeVisitStart", "Время начала визита (Время с)" },
                { "TimeVisitEnd", "Время окончания визита (Время по)" },
                { "NcNum", "Номер документа (Уведомление о проверке)" },
                { "NcDate", "Дата документа (Уведомление о проверке)" },
                { "PoliticAuthority", "Орган гос власти" },
                { "DateStatement", "Дата формирования заявления" },
                { "MotivatedRequestNumber", "Номер мотивированного запроса" },
                { "MotivatedRequestDate", "Дата мотивированного запроса" },
                { "NoticeDateProtocol", "Дата составления протокола" },
                { "NoticePlaceCreation", "Место составления" },
                { "NoticeDescription", "Комментарии к составлению" },
                { "ProsecutorDecNumber", "Номер решения прокурора" },
                { "ProsecutorDecDate", "Дата решения прокурора" }
            };
            return result;
        }
    }
}

