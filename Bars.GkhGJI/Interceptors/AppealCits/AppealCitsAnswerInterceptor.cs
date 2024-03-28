namespace Bars.GkhGji.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AppealCitsAnswerInterceptor : EmptyDomainInterceptor<AppealCitsAnswer>
    {
        public override IDataResult BeforeCreateAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {


            var servStateProvider = Container.Resolve<IStateProvider>();
            try
            {
                servStateProvider.SetDefaultState(entity);
            }
            finally
            {
                Container.Release(servStateProvider);
            }

            return this.Success();

        }

        public override IDataResult AfterCreateAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            var appCitizensContainer = this.Container.Resolve<IDomainService<AppealCits>>();
            if (entity.TypeAppealAnswer == null)
            {
                entity.TypeAppealAnswer = Enums.TypeAppealAnswer.NotSet;
            }
            if (entity.TypeAppealFinalAnswer != Enums.TypeAppealFinalAnswer.NotSet)
            {
                if (entity.File != null)
                {
                    var appealCitsAnswerAttDomain = this.Container.Resolve<IDomainService<AppealCitsAnswerAttachment>>();
                    try
                    {
                        appealCitsAnswerAttDomain.Save(new AppealCitsAnswerAttachment
                        {
                            AppealCitsAnswer = entity,
                            Name = string.IsNullOrEmpty(entity.DocumentName) ? entity.File.Name : entity.DocumentName,
                            Description = "Приложение к ответу",
                            FileInfo = entity.File
                        });
                    }
                    catch
                    { }
                    finally
                    {
                        Container.Release(appealCitsAnswerAttDomain);
                    }
                }
                var appealCitsAnswerStatSubjectDomain = this.Container.Resolve<IDomainService<AppealCitsAnswerStatSubject>>();
                var appealCitsStatSubjectDomain = this.Container.Resolve<IDomainService<AppealCitsStatSubject>>();
                try
                {
                    var statsubjList = appealCitsStatSubjectDomain.GetAll().Where(x => x.AppealCits != null && x.AppealCits.Id == entity.AppealCits.Id).ToList();
                    if (statsubjList.Count == 1)
                    {
                        appealCitsAnswerStatSubjectDomain.Save(new AppealCitsAnswerStatSubject
                        {
                            AppealCitsAnswer = entity,
                            StatSubject = statsubjList.FirstOrDefault()
                        });
                    }
                }
                catch (Exception e)
                {
                    string str = e.Message;
                }
                finally
                {
                    Container.Release(appealCitsAnswerStatSubjectDomain);
                    Container.Release(appealCitsStatSubjectDomain);
                }
            }

            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsAnswer, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.DocumentNumber);
            }
            catch
            {

            }

            if (entity.Addressee != null && entity.TypeAppealAnswer != Enums.TypeAppealAnswer.Note)
            {
                AppealCits app = appCitizensContainer.Get(entity.AppealCits.Id);
                app.AnswerDate = entity.DocumentDate;
                app.SSTUExportState = Enums.SSTUExportState.NotExported;
                appCitizensContainer.Update(app);
            }

            return this.Success();

        }
        public override IDataResult AfterUpdateAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
            var appCitizensContainer = this.Container.Resolve<IDomainService<AppealCits>>();
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();

            if (entity.Addressee != null && entity.TypeAppealAnswer != Enums.TypeAppealAnswer.Note)
            {
                AppealCits app = appCitizensContainer.Get(entity.AppealCits.Id);
                app.AnswerDate = entity.DocumentDate;
                app.SSTUExportState = Enums.SSTUExportState.NotExported;
                appCitizensContainer.Update(app);
            }

            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsAnswer, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.DocumentNumber);
            }
            catch
            {

            }

            return this.Success();

        }

        public override IDataResult AfterDeleteAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.AppealCitsAnswer, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.DocumentNumber);
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
                { "AppealCits", "Проверка" },
                { "DocumentName", "Документ" },
                { "DocumentNumber", "Номер документа" },
                { "DocumentDate", "Дата документа" },
                { "Executor", "Исполнитель" },
                { "Signer", "Исполнитель" },
                { "Addressee", "Адресат" },
                { "AnswerContent", "Содержание ответа" },
                { "Description", "Описание" },
                { "File", "Файл" },
                { "FileDoc", "Файл" },
                { "State", "Статус" },
                { "ExecDate", "Дата исполнения (направления ответа)" },
                { "ExtendDate", "Дата продления срока исполнения" },
                { "ConcederationResult", "Результат рассмотрения" },
                { "FactCheckingType", "Вид проверки факта" },
                { "RedirectContragent", "Контрагент для перенаправления обращения" },
                { "Address", "Контрагент для перенаправления обращения" },
                { "TypeAppealAnswer", "Тип ответа" },
                { "TypeAppealFinalAnswer", "Тип ответа" }
            };
            return result;
        }
    }
}
