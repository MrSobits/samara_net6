namespace Bars.GkhGji.DomainService.Impl
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.Gkh.StimulReport;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ActCheckProvidedDocService : IActCheckProvidedDocService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddProvidedDocs(BaseParams baseParams)
        {
            var serviceDocs = this.Container.Resolve<IDomainService<ActCheckProvidedDoc>>();

            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;
                var providedDocIds = baseParams.Params.ContainsKey("providedDocIds")
                                         ? baseParams.Params["providedDocIds"].ToString()
                                         : "";

                // в этом списке будут id gпредоставляемых документов, которые уже связаны с этим распоряжением
                // (чтобы недобавлять несколько одинаковых документов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var provIds = providedDocIds.Split(',').Select(id => ObjectParseExtention.ToLong(id)).ToList();

                listIds.AddRange(
                    serviceDocs.GetAll()
                               .Where(x => x.ActCheck.Id == documentId)
                               .Select(x => x.ProvidedDoc.Id)
                               .Distinct()
                               .ToList());

                var listToSave = new List<ActCheckProvidedDoc>();
                
                foreach (var newId in provIds)
                {

                    // Если среди существующих документов уже есть такой документ то пролетаем мимо
                    if (listIds.Contains(newId))
                    {
                        continue;
                    }

                    // Если такого эксперта еще нет то добалвяем
                    listToSave.Add(new ActCheckProvidedDoc
                        {
                            ActCheck = new ActCheck { Id = documentId },
                            ProvidedDoc = new ProvidedDocGji { Id = newId }
                        });
                }

                if (listToSave.Count > 0)
                {
                    using (var tr = this.Container.Resolve<IDataTransaction>())
                    {

                        try
                        {
                            listToSave.ForEach(serviceDocs.Save);

                            tr.Commit();
                        }
                        catch (Exception exc)
                        {
                            tr.Rollback();
                            throw exc;
                        }
                    }
                }
                
                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(serviceDocs);
            }
        }

        public IDataResult AddCTListAnswers(BaseParams baseParams)
        {
            var serviceAnswers = this.Container.Resolve<IDomainService<ActCheckControlListAnswer>>();
            var serviceCTLists = this.Container.Resolve<IDomainService<ControlListQuestion>>();

            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;
                var ctlistIds = baseParams.Params.ContainsKey("ctlistIds")
                                         ? baseParams.Params["ctlistIds"].ToString()
                                         : "";

                // в этом списке будут id gпредоставляемых документов, которые уже связаны с этим распоряжением
                // (чтобы недобавлять несколько одинаковых документов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var provIds = ctlistIds.Split(',').Select(id => ObjectParseExtention.ToLong(id)).ToList();

                //чистим проверочный лист
                serviceAnswers.GetAll()
                               .Where(x => x.ActCheck.Id == documentId)
                               .Select(x => x.Id).ToList()
                               .ForEach(x =>
                               {
                                   serviceAnswers.Delete(x);
                               });
                List<ActCheckControlListAnswer> listToSave = new List<ActCheckControlListAnswer>();

                foreach (var newId in provIds)
                {
                    serviceCTLists.GetAll()
                        .Where(x => x.ControlList.Id == newId)
                        .ToList()
                        .ForEach(x =>
                        {
                            listToSave.Add(new ActCheckControlListAnswer
                            {
                                ActCheck = new ActCheck { Id = documentId },
                                ControlListQuestion = x,
                                NpdName = x.NPDName,
                                Question = x.Name,
                                YesNoNotApplicable = Gkh.Enums.YesNoNotApplicable.NotSet
                            });
                        });

                }

                if (listToSave.Count > 0)
                {
                    using (var tr = this.Container.Resolve<IDataTransaction>())
                    {

                        try
                        {
                            listToSave.ForEach(serviceAnswers.Save);

                            tr.Commit();
                        }
                        catch (Exception exc)
                        {
                            tr.Rollback();
                            throw exc;
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(serviceAnswers);
                this.Container.Release(serviceCTLists);

            }
        }

        public IDataResult PrintReport(BaseParams baseParams)
        {
            var serviceActCheckAnnex = this.Container.Resolve<IDomainService<ActCheckAnnex>>();     

            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;
                var userParam = new UserParamsValues();
                userParam.AddValue("DocumentId", documentId);
                var codedReportDomain = this.Container.ResolveAll<IGkhBaseReport>();
                var report = codedReportDomain.FirstOrDefault(x => x.Id == "ActChectQListAnswer");
                report.SetUserParams(userParam);
                MemoryStream stream;
                var reportProvider = Container.Resolve<IGkhReportProvider>();
                if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
                {
                    stream = (report as StimulReport).GetGeneratedReport();
                }
                else
                {
                    var reportParams = new ReportParams();
                    report.PrepareReport(reportParams);

                    // получаем Генератор отчета
                    var generatorName = report.GetReportGenerator();

                    stream = new MemoryStream();
                    var generator = Container.Resolve<IReportGenerator>(generatorName);
                    reportProvider.GenerateReport(report, stream, generator, reportParams);
                }
                var fileManager = this.Container.Resolve<IFileManager>();
                var reportFile = fileManager.SaveFile(stream, "Проверочный лист.docx");
                if (reportFile != null)
                {
                    var existsReportAnnex = serviceActCheckAnnex.GetAll()
                        .Where(x => x.ActCheck.Id == documentId && x.TypeAnnex == Enums.TypeAnnex.ControlList).FirstOrDefault();
                    if (existsReportAnnex != null)
                    {
                        existsReportAnnex.File = reportFile;
                        serviceActCheckAnnex.Update(existsReportAnnex);
                    }
                    else
                    {
                        serviceActCheckAnnex.Save(new ActCheckAnnex
                        {
                            File = reportFile,
                            ActCheck = new ActCheck {Id = documentId },
                            MessageCheck = Enums.MessageCheck.NotSet,
                            Description = "Сформирован автоматически",
                            DocumentDate = DateTime.Now,
                            Name = "Проверочный лист",
                            TypeAnnex = Enums.TypeAnnex.ControlList

                        });
                    }
                }
                return new BaseDataResult(true);
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(serviceActCheckAnnex);

            }
        }
        public IDataResult SaveAndGetNextQuestion(BaseParams baseParams)
        {
            var serviceAnswers = this.Container.Resolve<IDomainService<ActCheckControlListAnswer>>();

            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;

                var answerId = baseParams.Params.ContainsKey("answerId")
                                    ? baseParams.Params.GetAs<long>("answerId")
                                    : 0;
                var descripiton = baseParams.Params.ContainsKey("descripiton")
                                    ? baseParams.Params.GetAs<string>("descripiton")
                                    : "";
                var choise = baseParams.Params.ContainsKey("choise")
                                   ? baseParams.Params.GetAs<Int32>("choise")
                                   : 0;
                YesNoNotApplicable enumChosen = GetEnumValue(choise);

                var currentQuestion = serviceAnswers.Get(answerId);
                if (currentQuestion != null)
                {
                    currentQuestion.YesNoNotApplicable = enumChosen;
                    currentQuestion.Description = descripiton;
                    serviceAnswers.Update(currentQuestion);
                }

                //чистим проверочный лист
                var question = serviceAnswers.GetAll()
                               .Where(x => x.ActCheck.Id == documentId && x.YesNoNotApplicable == Gkh.Enums.YesNoNotApplicable.NotSet)
                               .OrderBy(x => x.ControlListQuestion.ControlList.Id).ThenBy(x => x.ControlListQuestion.Id).ToList()
                               .FirstOrDefault();

                if (question == null)
                {
                    return new BaseDataResult(new
                    {
                        QlistName = "",
                        Question = "",
                        qid = 0
                    });
                }


                return new BaseDataResult(new
                {
                    QlistName = question.ControlListQuestion.ControlList.Name,
                    Question = question.Question,
                    qid = question.Id
                });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(serviceAnswers);

            }
        }

       

        public IDataResult GetNextQuestion(BaseParams baseParams)
        {
            var serviceAnswers = this.Container.Resolve<IDomainService<ActCheckControlListAnswer>>();

            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;


                //чистим проверочный лист
                var question = serviceAnswers.GetAll()
                               .Where(x => x.ActCheck.Id == documentId && x.YesNoNotApplicable == Gkh.Enums.YesNoNotApplicable.NotSet)
                               .OrderBy(x => x.ControlListQuestion.ControlList.Id).ThenBy(x => x.ControlListQuestion.Id).ToList()
                               .FirstOrDefault();

                if(question == null)
                {
                    return new BaseDataResult(new
                    {
                        QlistName = "",
                        Question = "",
                        qid = 0
                    });
                }
               

                return new BaseDataResult(new 
                {
                    QlistName = question.ControlListQuestion.ControlList.Name,
                    Question = question.Question,
                    qid = question.Id
                });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(serviceAnswers);

            }
        }

        private YesNoNotApplicable GetEnumValue(Int32 enumval)
        {
            switch (enumval)
            {
                case 0:
                    {
                        return YesNoNotApplicable.NotSet;                       
                    }
                case 10:
                    {
                        return YesNoNotApplicable.Yes;
                    }
                case 20:
                    {
                        return YesNoNotApplicable.No;
                    }
                case 30:
                    {
                        return YesNoNotApplicable.NotApplicable;
                    }
                default:

                    return YesNoNotApplicable.NotApplicable;
            }
        }
    }
}