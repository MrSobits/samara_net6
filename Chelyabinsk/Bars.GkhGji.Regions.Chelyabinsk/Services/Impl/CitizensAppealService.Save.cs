namespace Bars.GkhGji.Regions.Chelyabinsk.Services.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Services.DataContracts;

    using System.Collections.Generic;
    
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Utils;

    using Microsoft.Extensions.Logging;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис сведений об обращениях граждан - валидация
    /// </summary>
    public partial class CitizensAppealService
    {
        private readonly List<FileInfo> filesToDelete = new List<FileInfo>();

        public IFileManager FileManager { get; set; }
        public ILogger LogManager { get; set; }

        /// <summary>
        /// Сохранение
        /// </summary>
        /// <returns></returns>
        protected IDataResult SaveCitizenAppeal(CitizenAppeal appeal)
        {
            IDomainService<AppealCits> appealService;
            IDomainService<KindStatementGji> kindStatementService;
            IDomainService<Citizenship> citizenshipService;
            IDomainService<AppealCitsSource> appealCitsSourceService;
            IDomainService<RevenueSourceGji> revenueSourceGji;
            IDomainService<AppealCitsCategory> appealCitsCategoryService;
            IDomainService<AppealCitsAttachment> appealCitsAttachmentService;
            IDomainService<QuestionKind> questionKindService;
            IDomainService<AppealCitsQuestion> appealCitsQuestionService;
            IDomainService<StatSubjectSubsubjectGji> appealStatSubjectSubsubjectGjiService;
            IDomainService<AppealCitsStatSubject> appealCitsStatSubject;
            IDomainService<StatSubjectGji> statSubjectGjiService;
            IDomainService<StatSubsubjectGji> statSubsubjectGjiService;
            IDomainService<FeatureViolGji> featureViolGjiService;
            IDomainService<Inspector> inspectorService;
            IDomainService<AppealCitsHeadInspector> appealCitsInspectorService;
            IDomainService<AppealCitsExecutant> appealCitsExecutantService;
            IConfigProvider configProvider;
            IDomainService<State> stateDomain;

            using (this.Container.Using(appealService = this.Container.ResolveDomain<AppealCits>(),
                kindStatementService = this.Container.ResolveDomain<KindStatementGji>(),
                citizenshipService = this.Container.ResolveDomain<Citizenship>(),
                appealCitsSourceService = this.Container.ResolveDomain<AppealCitsSource>(),
                revenueSourceGji = this.Container.ResolveDomain<RevenueSourceGji>(),
                appealCitsCategoryService = this.Container.ResolveDomain<AppealCitsCategory>(),
                appealCitsAttachmentService = this.Container.ResolveDomain<AppealCitsAttachment>(),
                questionKindService = this.Container.ResolveDomain<QuestionKind>(),
                appealCitsQuestionService = this.Container.ResolveDomain<AppealCitsQuestion>(),
                appealCitsStatSubject = this.Container.ResolveDomain<AppealCitsStatSubject>(),
                statSubjectGjiService = this.Container.ResolveDomain<StatSubjectGji>(),
                statSubsubjectGjiService = this.Container.ResolveDomain<StatSubsubjectGji>(),
                appealStatSubjectSubsubjectGjiService = this.Container.ResolveDomain<StatSubjectSubsubjectGji>(),
                featureViolGjiService = this.Container.ResolveDomain<FeatureViolGji>(),
                inspectorService = this.Container.ResolveDomain<Inspector>(),
                appealCitsInspectorService = this.Container.ResolveDomain<AppealCitsHeadInspector>(),
                appealCitsExecutantService = this.Container.ResolveDomain<AppealCitsExecutant>(),
                configProvider = this.Container.Resolve<IConfigProvider>(),
                stateDomain = this.Container.ResolveDomain<State>()))
            {
                string statusStr = string.Empty;
                try
                {
                    appeal.AppealUid = appeal.AppealUid;
                    var gkhAppeal = appealService.GetAll()
                        .FirstOrDefault(x => x.AppealUid == appeal.AppealUid) ?? 
                            new AppealCits
                            {
                                AppealUid = appeal.AppealUid
                            };

                    gkhAppeal.KindStatement = kindStatementService.GetAll()
                        .FirstOrDefault(x => x.Code == appeal.AppealForm.ToString());
                    gkhAppeal.AmountPages = appeal.AmountOfPages;
                    gkhAppeal.Correspondent = appeal.FioOfDeclarant;
                    gkhAppeal.Citizenship = citizenshipService.GetAll()
                        .FirstOrDefault(x => x.OksmCode == Convert.ToInt32(appeal.CitizenshipOfDeclarant));
                    gkhAppeal.DeclarantMailingAddress = appeal.MailingAddressOfDeclarant;
                    gkhAppeal.DeclarantWorkPlace = appeal.PlaceOfWorkOfDeclarant;
                    gkhAppeal.Phone = appeal.PhoneNumberOfDeclarant;
                    gkhAppeal.Email = appeal.EmailOfDeclarant;
                    gkhAppeal.DeclarantSex = appeal.SexOfDeclarant.HasValue && new long[] {1, 2}.Contains(appeal.SexOfDeclarant.Value)
                        ? (Gender) (appeal.SexOfDeclarant * 10)
                        : default(Gender?);
                    gkhAppeal.Description = appeal.AppealContent;
                    gkhAppeal.AppealStatus = appeal.Status.HasValue ? (AppealStatus)appeal.Status.Value : default(AppealStatus?);
                    gkhAppeal.PlannedExecDate = appeal.PlannedDateExtension;

                    if (!string.IsNullOrWhiteSpace(appeal.MainResolve))
                    {
                        gkhAppeal.Comment = appeal.MainResolve;
                    }

                    Inspector registrator = null;
                    if (appeal.RegistrarOfAppeal != null)
                    {
                        registrator = inspectorService.GetAll()
                            .Where(i => i.Fio == appeal.RegistrarOfAppeal.Fio)
                            .FirstOrDefault(i => i.Position == appeal.RegistrarOfAppeal.Post);

                        if (registrator == null)
                        {
                            Inspector newregistrator = new Inspector
                            {
                                Description = "Создан автоматически",
                                Email = appeal.RegistrarOfAppeal.Email,
                                Fio = appeal.RegistrarOfAppeal.Fio,
                                IsHead = false,
                                Position = appeal.RegistrarOfAppeal.Post,
                                Phone = appeal.RegistrarOfAppeal.PhoneNumber,
                                Code = "inported"
                            };
                            inspectorService.Save(newregistrator);
                            registrator = inspectorService.GetAll()
                            .Where(i => i.Fio == appeal.RegistrarOfAppeal.Fio)
                            .FirstOrDefault(i => i.Position == appeal.RegistrarOfAppeal.Post);
                            //по требованию заказчика формируем регистратора автоматом
                            //return BaseDataResult.Error(
                            //    $"По обращению №{appeal.AppealNumber} от {appeal.AppealDateOfRegistration:dd.MM.yyyy} "
                            //    + $"не найден регистратор обращения(оператор): '{appeal.RegistrarOfAppeal.Fio}'");
                        }
                    }
                    gkhAppeal.AppealRegistrator = registrator;

                    if (gkhAppeal.Id != default(long))
                    {
                        appealService.Update(gkhAppeal);

                        appealCitsSourceService.GetAll()
                            .Where(x => x.AppealCits.Id == gkhAppeal.Id)
                            .Select(x => x.Id)
                            .ForEach(x => appealCitsSourceService.Delete(x));

                        appealCitsAttachmentService.GetAll()
                            .Where(x => x.AppealCits.Id == gkhAppeal.Id)
                            .ForEach(x =>
                            {
                                if (x.FileInfo != null)
                                {
                                    this.filesToDelete.Add(x.FileInfo);
                                }
                                appealCitsAttachmentService.Delete(x.Id);
                            });

                        appealCitsQuestionService.GetAll()
                            .Where(x => x.AppealCits.Id == gkhAppeal.Id)
                            .Select(x => x.Id)
                            .ForEach(x => appealCitsQuestionService.Delete(x));

                        appealCitsStatSubject.GetAll()
                            .Where(x => x.AppealCits.Id == gkhAppeal.Id)
                            .Select(x => x.Id)
                            .ForEach(x => appealCitsStatSubject.Delete(x));

                        appealCitsInspectorService.GetAll()
                            .Where(x => x.AppealCits.Id == gkhAppeal.Id)
                            .Select(x => x.Id)
                            .ForEach(x => appealCitsInspectorService.Delete(x));

                        appealCitsExecutantService.GetAll()
                            .Where(x => x.AppealCits.Id == gkhAppeal.Id)
                            .Select(x => x.Id)
                            .ForEach(x => appealCitsExecutantService.Delete(x));
                    }
                    else
                    {
                        gkhAppeal.State = stateDomain
                            .FirstOrDefault(x => x.Code == AppealCits.Pending && x.TypeId == AppealCits.TypeId);

                        appealService.Save(gkhAppeal);
                    }

                    if (appeal.SourceOfReceipts?.Any() ?? false)
                    {
                        appeal.SourceOfReceipts.Select(x => new AppealCitsSource
                            {
                                AppealCits = gkhAppeal,
                                RevenueSource = revenueSourceGji.GetAll().FirstOrDefault(i => i.Code == x.SourceOfReceiptName.ToString()),
                                RevenueSourceNumber = x.SourceOfReceiptNumber,
                                RevenueDate = x.SourceOfReceiptDate
                            })
                            .ForEach(appealCitsSourceService.Save);
                    }
                    var selfAppealSource = new AppealCitsSource
                    {
                        AppealCits = gkhAppeal,
                        RevenueSource = revenueSourceGji.GetAll().FirstOrDefault(i => i.Code == "79"),
                        RevenueSourceNumber = appeal.AppealNumber,
                        RevenueDate = appeal.AppealDateOfRegistration
                    };
                    appealCitsSourceService.Save(selfAppealSource);

                    if (appeal.CategoriesOfDeclarant?.Any() ?? false)
                    {
                        var appealCitsCategories = appealCitsCategoryService.GetAll()
                            .Where(x => x.AppealCits == gkhAppeal)
                            .ToDictionary(key => key.ApplicantCategory.Id, value => value.AppealCits.Id);

                        appeal.CategoriesOfDeclarant
                            .Where(x => !appealCitsCategories.ContainsKey(x))
                            .Select(x => new AppealCitsCategory
                            {
                                AppealCits = gkhAppeal,
                                ApplicantCategory = new ApplicantCategory { Id = x }
                            })
                            .ForEach(x =>
                            {
                                appealCitsCategoryService.Save(x);
                                appealCitsCategories.Remove(x.ApplicantCategory.Id);
                            });

                        appealCitsCategories.Where(x => !appeal.CategoriesOfDeclarant.Contains(x.Key))
                            .ForEach(x => appealCitsCategoryService.Delete(x.Value));
                    }

                    if (appeal.AppealAttachments?.Any() ?? false)
                    {
                        appeal.AppealAttachments.Select(
                                x => new AppealCitsAttachment
                                {
                                    AppealCits = gkhAppeal,
                                    FileInfo = this.GetFile(configProvider, x),
                                    Name = x.Name,
                                    Description = x.Description
                                })
                            .ForEach(appealCitsAttachmentService.Save);
                    }

                    if (appeal.KindOfQuestions?.Any() ?? false)
                    {
                        appeal.KindOfQuestions.Select(
                                x => new AppealCitsQuestion
                                {
                                    AppealCits = gkhAppeal,
                                    QuestionKind = questionKindService.GetAll().FirstOrDefault(q => q.Code == x.ToString())
                                })
                            .ForEach(appealCitsQuestionService.Save);
                    }

                    if (appeal.AppealQuestions?.Any() ?? false)
                    {
                        appeal.AppealQuestions.Select(x =>
                            {
                                var splitCode = x.Code;
                                statusStr = splitCode;
                                //if (splitCode[0] != "0005") // Жилищно-коммунальная сфера
                                //{
                                //    throw new Exception(
                                //        $"По обращению №{appeal.AppealNumber} от {appeal.AppealDateOfRegistration:dd.MM.yyyy} "
                                //        + "не все вопросы относятся к разделу 'Жилищно-коммунальная сфера'");
                                //}
                                long themeid = 10;
                                //тематика по умолчанию берется "Прочие", в том случае, если подтематика не надена, либо подтематика не прикреплена к тематике
                                statusStr += "; тематика " + splitCode.Split('.')[1];
                                var subject = appealStatSubjectSubsubjectGjiService.GetAll()
                                        .Where(s => s.Subsubject.SSTUCodeSub == splitCode).Select(s=> s.Subject).FirstOrDefault()
                                    ?? statSubjectGjiService.Get(themeid);

                                // старая реализация
                                //statusStr += "; тематика " + splitCode.Split('.')[1];
                                //var subject = appealStatSubjectSubsubjectGjiService.GetAll()
                                //        .Where(s => s.Subsubject.SSTUCodeSub == splitCode).Select(s => s.Subject).FirstOrDefault()
                                //    ?? new StatSubjectGji
                                //    {
                                //        QuestionCode = splitCode.Split('.')[1],
                                //        SSTUCode = splitCode,
                                //        Name = x.StatementSubjectName
                                //    };

                                statusStr += "; подтематика " + splitCode.Split('.')[2];
                                var subsubject = statSubsubjectGjiService.GetAll()
                                        .FirstOrDefault(s => s.SSTUCodeSub == splitCode)
                                    ?? new StatSubsubjectGji
                                    {
                                        QuestionCode = splitCode.Split('.')[2],
                                        SSTUCodeSub = splitCode,
                                        Name = x.SubsubjectName
                                    };
                                statusStr += "; характеристика " + splitCode.Split('.')[3];
                                var questionCode = splitCode.Split('.')[3];
                                var feature = featureViolGjiService.GetAll()
                                        .FirstOrDefault(s => s.QuestionCode == questionCode)
                                    ?? new FeatureViolGji
                                    {
                                        QuestionCode = splitCode.Split('.')[3],
                                        Name = x.QuestionName
                                    };
                                statusStr += "; импортировано";
                                if (subject.Id == 0)
                                    statSubjectGjiService.Save(subject);
                                if (subsubject.Id == 0)
                                    statSubsubjectGjiService.Save(subsubject);
                                if (feature.Id == 0)
                                    featureViolGjiService.Save(feature);

                                if (subject.Id == 0 && subsubject.Id == 0)
                                {
                                    var subsubsub = new StatSubjectSubsubjectGji
                                    {
                                        Subject = subject,
                                        Subsubject = subsubject,
                                        ObjectCreateDate = DateTime.Now,
                                        ObjectEditDate = DateTime.Now,
                                        ObjectVersion = 1
                                    };

                                    appealStatSubjectSubsubjectGjiService.Save(subsubsub);
                                }

                                return new AppealCitsStatSubject
                                {
                                    AppealCits = gkhAppeal,
                                    Subject = subject,
                                    Subsubject = subsubject,
                                    Feature = feature
                                };
                            })
                            .ForEach(appealCitsStatSubject.Save);
                    }

                    if (appeal.Executives?.Any() ?? false)
                    {
                        appeal.Executives.Select(x =>
                            {
                                var inspector = inspectorService.GetAll()
                                    .FirstOrDefault(i => i.Fio == x.Fio);

                                if (inspector == null)
                                {
                                    throw new Exception(
                                        $"По обращению №{appeal.AppealNumber} от {appeal.AppealDateOfRegistration:dd.MM.yyyy} не найден сотрудник: {x.Fio}");
                                }

                                if (inspector.Position.ToLower() != x.Post.ToLower())
                                {
                                    if (inspector.Description == "Правительство Челябинской области")
                                    {
                                        inspector.Position = x.Post;
                                        inspectorService.Update(inspector);
                                    }
                                    else
                                    {
                                        throw new Exception(
                                            $"Требуется сопоставить Должность ({inspector.Position}/{x.Post}) ответственного сотрудника {x.Fio} по обращению №{appeal.AppealNumber} от {appeal.AppealDateOfRegistration:dd.MM.yyyy}");
                                    }
                                }

                                var appCitsInspector = appealCitsInspectorService.GetAll()
                                        .FirstOrDefault(z => z.AppealCits == gkhAppeal && z.Inspector == inspector)
                                    ?? new AppealCitsHeadInspector
                                    {
                                        AppealCits = gkhAppeal,
                                        Inspector = inspector
                                    };
                                ;

                                return appCitsInspector;
                            })
                            .ForEach(appealCitsInspectorService.SaveOrUpdate);
                    }

                    if (appeal.Executants?.Any() ?? false)
                    {
                        appeal.Executants.Select(x =>
                            {
                                Inspector author = null;
                                if (!string.IsNullOrEmpty(x.FioOfAppointingPerson))
                                {
                                    author = inspectorService.GetAll()
                                        .Where(i => i.Fio == x.FioOfAppointingPerson)
                                        .Where(i => i.Position == x.PostOfAppointingPerson)
                                        .WhereIf(
                                            !string.IsNullOrWhiteSpace(x.PhoneNumberOfAppointingPerson),
                                            i => i.Phone == x.PhoneNumberOfAppointingPerson)
                                        .SingleOrDefault();
                                    if (author == null)
                                    {
                                        //по требованию заказчика создаем назначающее лицо автоматически
                                        Inspector newauthor = new Inspector
                                        {
                                            Description = "Создан автоматически",
                                            Fio = x.FioOfAppointingPerson,
                                            IsHead = false,
                                            Position = x.PostOfAppointingPerson,
                                            Phone = x.PhoneNumberOfAppointingPerson,
                                            Code = "inported"
                                        };
                                        inspectorService.Save(newauthor);
                                        author = inspectorService.GetAll()
                                         .Where(i => i.Fio == x.FioOfAppointingPerson)
                                         .Where(i => i.Position == x.PostOfAppointingPerson)
                                         .WhereIf(
                                             !string.IsNullOrWhiteSpace(x.PhoneNumberOfAppointingPerson),
                                             i => i.Phone == x.PhoneNumberOfAppointingPerson)
                                         .SingleOrDefault();

                                        //throw new Exception(
                                        //    $"По обращению №{appeal.AppealNumber} от {appeal.AppealDateOfRegistration:dd.MM.yyyy} не найдено назначающее лицо: {x.FioOfAppointingPerson} {x.PostOfAppointingPerson}");
                                    }
                                }

                                var executant = inspectorService.GetAll()
                                    .Where(i => i.Fio == x.ExecutantFio)
                                    .SingleOrDefault(i => i.Position == x.ExecutantPost);

                                var zone = new ZonalInspection { Id = x.SubdivisionName };

                                return new AppealCitsExecutant
                                {
                                    AppealCits = gkhAppeal,
                                    OrderDate = x.SendingDateToExecutant,
                                    Author = author,
                                    Executant = executant,
                                    ZonalInspection = zone
                                };
                            })
                            .ForEach(appealCitsExecutantService.Save);
                    }
                }
                catch (Exception exception)
                {
                    return BaseDataResult.Error((exception.InnerException ?? exception).Message + "");
                }
            }

            return new BaseDataResult();
        }

        private FileInfo GetFile(IConfigProvider configProvider, AppealAttachment appealAttachment)
        {
            var appSettings = configProvider.GetConfig().AppSettings;
            //var ftpPath = appSettings.GetAs<string>("FtpPath");
            //var fileName = appealAttachment.UniqueName.Trim().Trim('/', '\\');

            //var filePath = Path.GetFullPath(Path.Combine(ftpPath, fileName));

            //if (Path.IsPathRooted(fileName) || !filePath.StartsWith(ftpPath) || !File.Exists(filePath))
            //{
            //    throw new FileNotFoundException($"Не удалось найти указанный файл вложения на FTP-сервере ({appealAttachment.UniqueName})");
            //}
            //return this.FileManager.SaveFile(filePath);

            var ftpServer = appSettings.GetAs<string>("ftpServer");
            var ftpUser = appSettings.GetAs<string>("ftpUser");
            var ftpPassword = appSettings.GetAs<string>("ftpPassword");
            var ftpUtility = new FtpUtility(ftpServer, ftpUser, ftpPassword);
            if (!ftpUtility.FileExists(appealAttachment.UniqueName))
            {
                throw new Exception($"Не удалось найти указанный файл вложения на FTP-сервере ({appealAttachment.UniqueName})");
            }

            using (var content = ftpUtility.DownloadFile(appealAttachment.UniqueName))
            {
                return this.FileManager.SaveFile(content, Path.GetFileName(appealAttachment.UniqueName));
            }
        }

        private void TryClearFiles()
        {
            try
            {
                this.filesToDelete
                    .DistinctBy(x => x.Id)
                    .ForEach(this.FileManager.Delete);
                this.filesToDelete.Clear();
            }
            catch (Exception ex)
            {
                this.LogManager.LogError(ex, $"Не удалось удалить старые файлы вложений");
            }
        }
    }
}
