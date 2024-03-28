using Bars.B4.Modules.DataExport.Domain;

namespace Bars.Gkh.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;

    public class PersonRequestToExamController : FileStorageDataController<PersonRequestToExam>
    {
        public IDomainService<QExamQuestion> QExamQuestionDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        public IDomainService<QualifyTestSettings> QualifyTestSettingsDomain { get; set; }

        public IDomainService<PersonRequestToExam> PersonRequestToExamDomain { get; set; }

        public IDomainService<QExamQuestionAnswer> QExamQuestionAnswerDomain { get; set; }

        public IDomainService<QualifyTestQuestions> QualifyTestQuestionsDomain { get; set; }

        public IDomainService<QualifyTestQuestionsAnswers> QualifyTestQuestionsAnswersDomain { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("PersonRequestToExamDataExport");

            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally
            {
                Container.Release(export);
            }
        }

        public ActionResult ListQuestionsForExam(BaseParams baseParams)
        {
           
            var loadParams = baseParams.GetLoadParam();

            var parentId = baseParams.Params.ContainsKey("requestToExamId")
            ? baseParams.Params.GetAs<long>("requestToExamId")
            : 0;
            if (parentId > 0)
            {
                try
                {
                    var qExamSetings = QualifyTestSettingsDomain.GetAll()
                        .Where(x => x.DateFrom <= DateTime.Now)
                        .Where(x => !x.DateTo.HasValue || x.DateTo.Value > DateTime.Now)
                        .OrderByDescending(x=> x.Id)
                        .FirstOrDefault();

                    var answersList = QExamQuestionAnswerDomain.GetAll()
                        .Where(x => x.QExamQuestion.PersonRequestToExam.Id == parentId)
                        .Select(x => x.Id).ToList();

                    foreach (long id in answersList)
                    {
                        QExamQuestionAnswerDomain.Delete(id);
                    }

                    var questionList = QExamQuestionDomain.GetAll()
                        .Where(x => x.PersonRequestToExam.Id == parentId)
                        .Select(x => x.Id).ToList();

                    foreach (long id in questionList)
                    {
                        QExamQuestionDomain.Delete(id);
                    }

                    Random rand = new Random();
                    Int64[] allQuestions = QualifyTestQuestionsDomain.GetAll()
                        .Where(x=> x.IsActual == Enums.YesNoNotSet.Yes).Select(x=> x.Id).ToArray();
                    List<Int64> lst = new List<Int64>();
                    int quantity = qExamSetings!= null? qExamSetings.QuestionsCount:100;
                    if (allQuestions.Count() <= quantity)
                    {
                        quantity = allQuestions.Count();
                    }

                    Int64[] selected = new Int64[quantity]; // здесь будут храниться 3 случаные неповторяющиеся строки из Allcards
                    int k;

                    for (int i = 0; i < selected.Length; i++)
                    {
                        while (true)
                        {
                            k = rand.Next(allQuestions.Length);
                            if (!lst.Any(x => x.Equals(allQuestions[k])))
                            {
                                lst.Add(allQuestions[k]);
                                selected[i] = allQuestions[k];
                                break;
                            }
                        }
                    }

                        QualifyTestQuestionsDomain.GetAll()
                            .Where(x=> selected.ToList().Contains(x.Id))
                            .ToList().ForEach
                        (x =>
                        {
                            var newExamQuest = new QExamQuestion
                            {
                                Number = x.Code,
                                ObjectVersion = 1,
                                ObjectCreateDate = DateTime.Now,
                                ObjectEditDate = DateTime.Now,
                                PersonRequestToExam = PersonRequestToExamDomain.Get(parentId),
                                QualifyTestQuestions = x,
                                QuestionText = x.Question
                            };
                            QExamQuestionDomain.Save(newExamQuest);
                            QualifyTestQuestionsAnswersDomain.GetAll()
                            .Where(y => y.QualifyTestQuestions == x).ToList().ForEach(
                                y =>
                                {
                                    QExamQuestionAnswer newAnswer = new QExamQuestionAnswer
                                    {
                                        AnswerText = y.Answer,
                                        ObjectVersion = 2,
                                        ObjectCreateDate = DateTime.Now,
                                        ObjectEditDate = DateTime.Now,
                                        QExamQuestion = newExamQuest,
                                        QualifyTestQuestionsAnswers = y
                                    };
                                    QExamQuestionAnswerDomain.Save(newAnswer);
                                });
                        }
                        );

                    var questionListNew = QExamQuestionDomain.GetAll()
                     .Where(x => x.PersonRequestToExam.Id == parentId)
                     .Select(x => x.Id).ToList();

                    return new JsonNetResult(new BaseDataResult(new
                    {
                        questionCount = questionListNew.Count()
                    }));

                                
                }
                catch (Exception e)
                {
                    return JsFailure(e.Message);
                }
                
            }
            else
            {
                return null;
            }
        }

        public ActionResult GetNextQuestion(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var parentId = baseParams.Params.ContainsKey("requestToExamId")
            ? baseParams.Params.GetAs<long>("requestToExamId")
            : 0;
            if (parentId > 0)
            {
                try
                {
                    var examQuestion = QExamQuestionDomain.GetAll()
                        .Where(x => x.PersonRequestToExam.Id == parentId)
                        .Where(x => x.QualifyTestQuestionsAnswers == null)
                        .FirstOrDefault();
                    var answersList = QExamQuestionAnswerDomain.GetAll()
                        .Where(x => x.QExamQuestion == examQuestion).ToList();
                    return new JsonNetResult(new BaseDataResult(new
                    {
                        question = examQuestion,
                        answers = answersList
                    }));
                }
                catch (Exception e)
                {
                    return JsFailure(e.Message);
                }

            }
            else
            {
                return null;
            }
        }
      

        public ActionResult SaveAndGetNextQuestion(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var questionId = baseParams.Params.ContainsKey("questionId")
           ? baseParams.Params.GetAs<long>("questionId")
           : 0;
            var answerId = baseParams.Params.ContainsKey("answerId")
           ? baseParams.Params.GetAs<long>("answerId")
           : 0;
            if (questionId > 0 && answerId > 0)
            {
                var qExamQuestion = this.QExamQuestionDomain.Get(questionId);

                try
                {
                    var qanswer = this.QExamQuestionAnswerDomain.Get(answerId);
                    qExamQuestion.QualifyTestQuestionsAnswers = qanswer.QualifyTestQuestionsAnswers;
                    this.QExamQuestionDomain.Update(qExamQuestion);


                  
                }
                catch (Exception e)
                {
                    return JsFailure(e.Message);
                }

            }          

            var parentId = baseParams.Params.ContainsKey("requestToExamId")
            ? baseParams.Params.GetAs<long>("requestToExamId")
            : 0;
            if (parentId > 0)
            {
                try
                {
                    var examQuestion = QExamQuestionDomain.GetAll()
                        .Where(x => x.PersonRequestToExam.Id == parentId)
                        .Where(x => x.QualifyTestQuestionsAnswers == null)
                        .FirstOrDefault();
                    var answersList = QExamQuestionAnswerDomain.GetAll()
                        .Where(x => x.QExamQuestion == examQuestion).ToList();
                    return new JsonNetResult(new BaseDataResult(new
                    {
                        question = examQuestion,
                        answers = answersList
                    }));
                }
                catch (Exception e)
                {
                    return JsFailure(e.Message);
                }

            }
            else
            {
                return null;
            }
        }

        public ActionResult SaveAnswer(BaseParams baseParams)
        {
            // var rosRegExtractDomain = Container.Resolve<IDomainService<RosRegExtract>>();
            // var listRRE = rosRegExtractDomain.GetAll().Take(10);
            

            var loadParams = baseParams.GetLoadParam();

            var questionId = baseParams.Params.ContainsKey("questionId")
            ? baseParams.Params.GetAs<long>("questionId")
            : 0;
            var answerId = baseParams.Params.ContainsKey("answerId")
           ? baseParams.Params.GetAs<long>("answerId")
           : 0;
            if (questionId > 0 && answerId>0)
            {
                var qExamQuestion = this.QExamQuestionDomain.Get(questionId);

                try
                {
                    var qanswer = this.QExamQuestionAnswerDomain.Get(answerId);
                    qExamQuestion.QualifyTestQuestionsAnswers = qanswer.QualifyTestQuestionsAnswers;
                    this.QExamQuestionDomain.Update(qExamQuestion);


                    return JsSuccess();
                }
                catch (Exception e)
                {
                    return JsFailure(e.Message);
                }

            }
            else
            {
                return null;
            }
        }

    }
}
