namespace Bars.GkhGji.Regions.Habarovsk.Interceptors
{
    using Entities;
    using System;
    using Bars.B4;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using System.Collections.Generic;
    using Bars.GkhGji.Entities;

    class AppealCitsAdmonitionInterceptor : EmptyDomainInterceptor<AppealCitsAdmonition>
    {

        public IDomainService<AppCitAdmonVoilation> AppCitAdmonVoilationDomain { get; set; }
        public IDomainService<AppealCitsAdmonitionLongText> AppealCitsAdmonitionLongTextDomain { get; set; }
        public IDomainService<AppCitAdmonAppeal> AppCitAdmonAppealDomain { get; set; }
        public IDomainService<ERKNM> ERKNMDomain { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<AppealCitsAdmonition> service, AppealCitsAdmonition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();

            try
            {
                if (entity.AppealCits != null)
                {
                    AppCitAdmonAppealDomain.Save(new AppCitAdmonAppeal
                    {
                        AppealCitsAdmonition = entity,
                        AppealCits = entity.AppealCits
                    });
                }

                var appealCitsId = AppCitAdmonAppealDomain.GetAll().Where(x => x.AppealCitsAdmonition.Id == entity.Id).Select(x => x.AppealCits.Id).FirstOrDefault();

                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsAdmonition, entity.Id, entity.GetType(), GetPropertyValues(), appealCitsId + " " + entity.DocumentNumber);

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось сохранить задачу");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<AppealCitsAdmonition> service, AppealCitsAdmonition entity)
        {
            try
            {
                var erknmTask = ERKNMDomain.GetAll().Where(x => x.AppealCitsAdmonition != null && x.AppealCitsAdmonition.Id == entity.Id).FirstOrDefault();
                var signedInfoDomain = Container.ResolveDomain<PdfSignInfo>();

                if (erknmTask != null)
                {
                    return Failure("Данное предостережение было направлено в ЕРКНМ через СМЭВ3, перед удалением Вам необходимо удалить запись в межведомственном взаимодействии");
                }
                AppCitAdmonVoilationDomain.GetAll()
                   .Where(x => x.AppealCitsAdmonition.Id == entity.Id)
                   .Select(x => x.Id)
                   .ToList()
                   .ForEach(x => AppCitAdmonVoilationDomain.Delete(x));
                AppCitAdmonAppealDomain.GetAll()
                  .Where(x => x.AppealCitsAdmonition.Id == entity.Id)
                  .Select(x => x.Id)
                  .ToList()
                  .ForEach(x => AppCitAdmonAppealDomain.Delete(x));
                AppealCitsAdmonitionLongTextDomain.GetAll()
               .Where(x => x.AppealCitsAdmonition.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => AppealCitsAdmonitionLongTextDomain.Delete(x));

                var signed = signedInfoDomain.GetAll().Where(x => x.OriginalPdf == entity.File).Select(x => x.Id).ToList();
                foreach (long id in signed)
                {
                    signedInfoDomain.Delete(id);
                }

                return Success();
            }
            catch (Exception e)
            {
                return Failure("При удалении произошла ошибка");
            }
        }

        public override IDataResult AfterUpdateAction(IDomainService<AppealCitsAdmonition> service, AppealCitsAdmonition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsAdmonition, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<AppealCitsAdmonition> service, AppealCitsAdmonition entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.AppealCitsAdmonition, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.DocumentNumber);
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
                { "RealityObject", "Дом" },
                { "AppealCits", "Обращение" },
                { "Contragent", "Контрагент" },
                { "DocumentName", "Документ" },
                { "DocumentNumber", "Номер документа" },
                { "DocumentDate", "Дата документа" },
                { "PerfomanceDate", "Дата исполнения" },
                { "PerfomanceFactDate", "Дата фактического исполнения" },
                { "File", "Файл" },
                { "AnswerFile", "Файл ответа" },
                { "PayerType", "Тип плательщика" },
                { "FIO", "ФИО" },
                { "FizAddress", "ФИО физ.лица" },
                { "FizINN", "ИНН физ.лица" },
                { "PhysicalPersonDocType", "Тип документа, удостоверяющего личность" },
                { "DocumentNumberFiz", "Номер документа физлица" },
                { "DocumentSerial", "Серия документа физлица" },
                { "RiskCategory", "Тип исполниителя" },
                { "SurveySubject", "Выгружено в еркнм" },
                { "InspectionReasonERKNM", "Основания проведения КНМ" }
            };
            return result;
        }

    }
}
