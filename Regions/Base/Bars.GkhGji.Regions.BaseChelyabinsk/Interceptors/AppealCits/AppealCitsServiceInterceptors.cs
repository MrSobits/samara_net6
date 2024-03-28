namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.AppealCits
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Ionic.Zip;
    using Newtonsoft.Json;
    using Ionic.Zlib;
    using Newtonsoft.Json.Serialization;
    using Bars.GkhGji.DomainService;
    using System.Collections.Generic;


    // Пустышка на случай если от этог окласса наследовались
    public class AppealCitsServiceInterceptor : Bars.GkhGji.Interceptors.AppealCitsServiceInterceptor
    {
        public IFileManager FileManager { get; set; }
        public override IDataResult BeforeDeleteAction(IDomainService<AppealCits> service, AppealCits entity)
        {
            var servAppCitsExecutant = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();

            try
            {

                var executantIds = servAppCitsExecutant.GetAll()
                                        .Where(x => x.AppealCits.Id == entity.Id)
                                        .Select(x => x.Id)
                                        .AsEnumerable();

                foreach (var id in executantIds)
                {
                    servAppCitsExecutant.Delete(id);
                }

                return base.BeforeDeleteAction(service, entity);

            }
            finally
            {
                this.Container.Release(servAppCitsExecutant);
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<AppealCits> service, AppealCits entity)
        {
            var AppealCitsAttachmentContainer = Container.Resolve<IDomainService<AppealCitsAttachment>>();
            if (entity.File == null)
            {
                var attachmentsList = AppealCitsAttachmentContainer.GetAll()
                    .Where(x => x.AppealCits == entity).Select(x=> x.Id).ToList();
                if (attachmentsList.Count > 0)
                {
                    try
                    {
                        entity.File = GetArchive(entity.Id, entity.Number);
                    }
                    catch
                    { }
                }
            }
            if (entity.QuestionStatus == null || entity.QuestionStatus == QuestionStatus.NotSet)
            {
                entity.QuestionStatus = QuestionStatus.InWork;
            }
            var appealCitsStatSubjectContainer = Container.Resolve<IDomainService<AppealCitsStatSubject>>();
            var AppealCitsExecutantContainer = Container.Resolve<IDomainService<AppealCitsExecutant>>();
            var appealCitsRealityObjectDomain = Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var manOrgContractRealityObjectDomain = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            try
            {

                if (entity.ControlDateGisGkh == null)
                {
                    if (entity.DateFrom != null)
                    {
                        var date = (DateTime)entity.DateFrom;
                        var dayofweek = date.DayOfWeek;
                        if (dayofweek == DayOfWeek.Friday)
                        {
                            date = date.AddDays(31);
                            entity.ControlDateGisGkh = date;
                        }
                        else
                        {
                            date = date.AddDays(29);
                            entity.ControlDateGisGkh = date;
                        }
                    }
                }

                var statSubjectsList = appealCitsStatSubjectContainer.GetAll()
                    .Where(x => x.AppealCits.Id == entity.Id)
                    .Select(x => x.Subject.Name).ToList()
                    ;

                string statSubjects = "";
                foreach (string subject in statSubjectsList)
                {
                    if (statSubjects != "")
                        statSubjects += ", " + subject;
                    else statSubjects = subject;
                }
                entity.StatementSubjects = statSubjects;

                var executantList = AppealCitsExecutantContainer.GetAll()
                    .Where(x => entity.Id == x.AppealCits.Id)
                    .Where(x => x.Executant != null)
                    .Select(x => new
                    {
                        Fio = x.IsResponsible ? "<b>" + x.Executant.Fio + "</b>" : x.Executant.Fio
                    }).ToList();

                var testersList = AppealCitsExecutantContainer.GetAll()
                   .Where(x => entity.Id == x.AppealCits.Id)
                   .Where(x => x.Controller != null)
                   .Select(x => x.Controller.Fio).ToList()
                   ;
                string executants = "";
                foreach (var subject in executantList)
                {
                    if (executants != "")
                        executants += ", " + subject.Fio;
                    else executants = subject.Fio;
                }
                string controllers = "";
                foreach (string subject in testersList)
                {
                    if (controllers != "")
                        controllers += ", " + subject;
                    else controllers = subject;
                }
                entity.Executors = executants;
                entity.Testers = controllers;

                var sourcesCont = this.Container.Resolve<IDomainService<AppealCitsSource>>();
                var sources = sourcesCont.GetAll()
                    .Where(x => x.AppealCits.Id == entity.Id)
                    .Select(x => x.RevenueSourceNumber + (x.SSTUDate.HasValue ? " от " + x.SSTUDate.Value.ToString("dd.MM.yyyy") : "")).ToList();
                string answersNums = "";
                foreach (string thisNum in sources)
                {
                    if (!string.IsNullOrEmpty(thisNum))
                    {
                        if (answersNums != "")
                            answersNums += "; " + thisNum;
                        else
                            answersNums = thisNum;
                    }
                }
                entity.IncomingSources = answersNums;

                var sourceNamesList = sourcesCont.GetAll()
                    .Where(x => x.AppealCits.Id == entity.Id)
                    .Where(x => x.RevenueSource != null)
                    .Select(x => x.RevenueSource.Name).ToList();

                string sourceNames = "";
                foreach (string thisNum in sourceNamesList)
                {
                    if (!string.IsNullOrEmpty(thisNum))
                    {
                        if (sourceNames != "")
                            sourceNames += "; " + thisNum;
                        else
                            sourceNames = thisNum;
                    }
                }
                entity.IncomingSourcesName = sourceNames;

                var realityCont = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();

                var realities = realityCont.GetAll()
                    .Where(x => x.AppealCits.Id == entity.Id)
                    .Where(x => x.Id != entity.Id)
                    .Select(x => x.RealityObject.Address).ToList();
                string address = "";
                foreach (string thisreality in realities)
                {
                    if (!string.IsNullOrEmpty(thisreality))
                    {
                        if (address != "")
                            address += "; " + thisreality;
                        else
                            address = thisreality;
                    }
                }
                if (address.Length > 500)
                {
                    address = address.Substring(0, 450);
                }
                entity.RealityAddresses = address;

                var firstRO = realityCont.GetAll()
                   .FirstOrDefault(x => x.AppealCits.Id == entity.Id);
                if (firstRO != null)
                {
                    entity.Municipality = firstRO.RealityObject.Municipality.Name;
                    entity.MunicipalityId = firstRO.RealityObject.Municipality.Id;
                }

                if (entity.QuestionStatus != QuestionStatus.Transferred)
                {
                    entity.SSTUTransferOrg = null;
                }
                if (entity.QuestionStatus == QuestionStatus.NotSet)
                {
                    string message = "Укажите статус вопроса в ССТУ";
                    return Failure(message);
                }
                else
                {
                    if (entity.OrderContragent == null)
                    {
                        Contragent contragent = new Contragent();
                        var realityObject = appealCitsRealityObjectDomain.FirstOrDefault(x => x.AppealCits == entity);
                        if (realityObject == null)
                        {
                            return this.Success();
                        }

                        var contract = manOrgContractRealityObjectDomain.GetAll()
                            .Where(x => x.RealityObject == realityObject.RealityObject)
                            .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate.Value <= DateTime.Now)
                            .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value > DateTime.Now).FirstOrDefault();
                        if (contract != null)
                        {
                            if (contract.ManOrgContract.ManagingOrganization != null)
                            {
                                contragent = contract.ManOrgContract.ManagingOrganization.Contragent;
                                entity.OrderContragent = contragent;
                            }
                        }
                        return this.Success();

                    }
                    else
                    {
                        return this.Success();
                    }
                }
            }
            finally
            {
                Container.Release(AppealCitsAttachmentContainer);
                Container.Release(AppealCitsExecutantContainer);
                Container.Release(appealCitsStatSubjectContainer);
                Container.Release(appealCitsRealityObjectDomain);
                Container.Release(manOrgContractRealityObjectDomain);
            }
            
        }

        public override IDataResult AfterUpdateAction(IDomainService<AppealCits> service, AppealCits entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                this.CreateReminders(entity);
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCits, entity.Id, entity.GetType(), GetPropertyValues(), entity.NumberGji);
            }
            catch
            {
                
            }
            return this.Success();

        }

        public override IDataResult AfterCreateAction(IDomainService<AppealCits> service, AppealCits entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCits, entity.Id, entity.GetType(), GetPropertyValues(), entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<AppealCits> service, AppealCits entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.AppealCits, entity.Id, entity.GetType(), GetPropertyValues(), entity.NumberGji);
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
                { "Surety", "Поручитель" },
                { "SuretyResolve", "Резолюция" },
                { "SuretyDate", "Срок исполнения" },
                { "ZonalInspection", "Отдел ГЖИ" },
                { "DocumentNumber", "Номер обращения" },
                { "NumberGji", "Номер ГЖИ" },
                { "DateFrom", "Дата от" },
                { "CheckTime", "Контрольный срок" },
                { "KindStatement", "Вид обращения" },
                { "FlatNum", "Номер квартиры" },
                { "StatementSubjects", "Тематики обращения" },
                { "ManagingOrganization", "Управляющая Организация" },
                { "OrderContragent", "Управляющая Организация" },
                { "Correspondent", "Корреспондент" },
                { "CorrespondentAddress", "Адрес корреспондента" },
                { "Email", "Эл.почта" },
                { "Phone", "Контактный Телефон" },
                { "Description", "Описание" },
                { "File", "Файл" },
                { "TypeCorrespondent", "Тип корреспондента" },
                { "QuestionStatus", " Статус вопроса в ССТУ" }
            };
            return result;
        }

        private void CreateReminders(AppealCits entity)
        {
            // Получаем правила формирования Напоминаний и запускаем метод создания напоминаний
            var servReminderRule = this.Container.ResolveAll<IReminderRule>();

            try
            {
                var rule = servReminderRule.FirstOrDefault(x => x.Id == "AppealCitsReminderRule");
                rule?.Create(entity);
            }
            finally
            {
                this.Container.Release(servReminderRule);
            }
        }

        private B4.Modules.FileStorage.FileInfo GetArchive(long appcitId, string rsnumber)
        {
            var AppealCitsAttachmentContainer = Container.Resolve<IDomainService<AppealCitsAttachment>>();
            try
            {
                var archive = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level9,
                    AlternateEncoding = Encoding.GetEncoding("cp866"),
                    AlternateEncodingUsage = ZipOption.AsNecessary
                };

                var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));


                var attachments = AppealCitsAttachmentContainer.GetAll().Where(x => x.AppealCits.Id == appcitId).ToList();

                foreach (var file in attachments)
                {
                    System.IO.File.WriteAllBytes(
                        Path.Combine(tempDir.FullName, $"{file.FileInfo.Name}.{file.FileInfo.Extention}"),
                        FileManager.GetFile(file.FileInfo).ReadAllBytes());
                }

                archive.AddDirectory(tempDir.FullName);

                using (var ms = new MemoryStream())
                {
                    archive.Save(ms);

                    var file = FileManager.SaveFile(ms, $"{rsnumber}.zip");
                    return file;
                }
                /*
                var contentDisposition = new ContentDisposition();
                contentDisposition.Inline = false;
                this.Response.AddHeader("Content-Disposition", $@"attachment; filename={citizenSuggestion.Number} - {citizenSuggestion.ApplicantFio}.zip");
                var result = new FileStreamResult(ms, "application/zip");*/
            }
            finally
            {
                Container.Release(AppealCitsAttachmentContainer);
            }
        }
    }
}