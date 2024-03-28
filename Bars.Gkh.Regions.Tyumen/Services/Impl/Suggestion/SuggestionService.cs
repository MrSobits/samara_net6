namespace Bars.Gkh.Regions.Tyumen.Services.Impl.Suggestion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ServiceModel;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Suggestion;
    using Bars.Gkh.Services.DataContracts.Suggestion;
    using Bars.Gkh.Services.ServiceContracts.Suggestion;
    using Castle.Windsor;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils.EntityExtensions;
    using DomainService;
    using ProblemPlace = Bars.Gkh.Services.DataContracts.Suggestion.ProblemPlace;
    using Rubric = Bars.Gkh.Services.DataContracts.Suggestion.Rubric;
    using CategoryPosts = Bars.Gkh.Services.DataContracts.Suggestion.CategoryPosts;
    using MessageSubject = Bars.Gkh.Services.DataContracts.Suggestion.MessageSubject;
    using Bars.Gkh.DomainService;

    // TODO: wcf
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SuggestionServiceTyumen : ISuggestionService
    {

        public IWindsorContainer Container { get; set; }

        public IFileManager FileManager { get; set; }

        public ProblemPlace[] GetProblemPlaces()
        {
            return
                Container.Resolve<IDomainService<Gkh.Entities.Suggestion.ProblemPlace>>().GetAll()
                    .Select(x => new ProblemPlace { Id = x.Id, Name = x.Name })
                    .ToArray();
        }

        public TypeProblem[] GetTypeProblems(long id)
        {
            var tpService = Container.ResolveDomain<Entities.Suggestion.SugTypeProblem>();

            try
            {

                return tpService.GetAll().Where(x => x.Rubric.Id == id)
                        .Select(
                            x => new TypeProblem { Id = x.Id, Name = x.Name, RequestTemplate = x.RequestTemplate })
                        .ToArray();
            }

            finally
            {
                Container.Release(tpService);
            }
        }



        public Rubric[] GetRubrics()
        {
            return
                Container.Resolve<IDomainService<Bars.Gkh.Entities.Suggestion.Rubric>>().GetAll()
                    .Select(x => new Rubric { Id = x.Id, Name = x.Name })
                    .ToArray();
        }

        public CategoryPosts[] GetCategoryPosts()
        {
            return
                Container.Resolve<IDomainService<Gkh.Entities.Suggestion.CategoryPosts>>().GetAll()
                    .Select(x => new CategoryPosts { Id = x.Id, Name = x.Name, Code = x.Code })
                    .ToArray();
        }

        public MessageSubject[] GetMessageSubjects(long id)
        {
            var categoryService = Container.ResolveDomain<Gkh.Entities.Suggestion.CategoryPosts>();
            var messageService = Container.ResolveDomain<Gkh.Entities.Suggestion.MessageSubject>();

            try
            {
                var categoryId = categoryService.Get(id).ReturnSafe(x => x.Id);

                return messageService.GetAll().Where(x => x.CategoryPosts.Id == categoryId)
                        .Select(
                            x => new MessageSubject { Id = x.Id, Name = x.Name, Code = x.Code })
                        .ToArray();
            }

            finally
            {
                Container.Release(categoryService);
                Container.Release(messageService);
            }
        }

        private StringBuilder CheckInParams(Suggestion suggestion)
        {
            StringBuilder message = new StringBuilder("");

            if (suggestion.Id == null)
            {
                message.Append("\n\t Не указан параметр ID suggestion");
            }
            if (suggestion.ProblemPlace.Id == null)
            {
                message.Append("\n\t Не указан параметр ID ProblemPlace");
            }
            else
            {
                if (suggestion.ProblemPlace.Id == null)
                {
                    message.Append("\n\t Не указан параметр ID ProblemPlace");
                }
            }
            if (suggestion.RealityObjectId == null)
            {
                message.Append("\n\t Не указан параметр RealityObjectId");
            }
            if (suggestion.Rubric.Id == null)
            {
                message.Append("\n\t Не указан параметр ID Rubric");
            }
            if (suggestion.StateId == null)
            {
                message.Append("\n\t Не указан параметр StateId suggestion");
            }
            if (suggestion.ApplicantFio == null)
            {
                message.Append("\n\t Не указан параметр ApplicantFio suggestion");
            }
            if (suggestion.Number == null)
            {
                message.Append("\n\t Не указан параметр Number suggestion");
            }
            return message;
        }

        public long CreateSuggestion(Suggestion suggestion)
        {
            StringBuilder message = CheckInParams(suggestion);
            if (message.Length > 0)
            {
                throw new FaultException("\n\tНе успешно: " + message);
            }

            var historyDomain = Container.ResolveDomain<CitizenSuggestionHistory>();
            var suggestionService = Container.ResolveDomain<CitizenSuggestion>();
            var commentDomain = Container.ResolveDomain<SuggestionComment>();
            var commentFilesDomain = Container.ResolveDomain<SuggestionCommentFiles>();
            var realObj = Container.ResolveDomain<RealityObject>().Load(suggestion.RealityObjectId);
            var rubric = Container.ResolveDomain<Gkh.Entities.Suggestion.Rubric>().Load(suggestion.Rubric.Id);
            var messageSubject = Container.ResolveDomain<Gkh.Entities.Suggestion.MessageSubject>().Load(suggestion.MessageId.ToLong());
            var room = Container.ResolveDomain<Room>().Load(suggestion.FlatId.ToLong());
            var handler = Container.Resolve<ISuggestionChangeHandler>();
            try
            {
                Gkh.Entities.Suggestion.ProblemPlace problemPlace;
                if (suggestion.ProblemPlace.Id > 0)
                {
                    problemPlace =
                        Container.Resolve<IDomainService<Gkh.Entities.Suggestion.ProblemPlace>>()
                            .Load(suggestion.ProblemPlace.Id);
                }
                else
                {
                    problemPlace = new Gkh.Entities.Suggestion.ProblemPlace { Name = suggestion.ProblemPlace.Name };
                    Container.Resolve<IDomainService<Gkh.Entities.Suggestion.ProblemPlace>>().Save(problemPlace);
                }

                var dt = DateTime.Now;
                var year = dt.Year;
                var num = suggestionService.GetAll().Where(x => x.Year == year).SafeMax(x => x.Num) + 1;

                var sug = new CitizenSuggestion(rubric)
                {
                    Number = string.Format("{0}-{1}", dt.ToString("yy"), num),
                    CreationDate = dt,
                    RealityObject = realObj,
                    ApplicantFio = suggestion.ApplicantFio,
                    ApplicantAddress = suggestion.ApplicantAddress,
                    ApplicantPhone = suggestion.ApplicantPhone,
                    ApplicantEmail = suggestion.ApplicantEmail,
                    MessageSubject = messageSubject,
                    Flat = room,
                    Year = year,
                    Num = num
                };

                var comment = new SuggestionComment
                {
                    CitizenSuggestion = sug,
                    Description = suggestion.Description,
                    ProblemPlace = problemPlace,
                    IsFirst = true,
                };

                if (suggestion.Test == true)
                {
                    sug.TestSuggestion = true;
                    comment.CreationDate = dt;
                    comment.AnswerDate = DateTime.Now;
                    comment.Answer = "Тестовый ответ от Управляющей компании";
                    comment.HasAnswer = true;
                }

                suggestionService.Save(sug);
                commentDomain.Save(comment);

                if (sug.TestSuggestion == false)
                {
                    handler.SendEmailApplicant(comment);
                    var history = new CitizenSuggestionHistory(comment, ExecutorType.None);
                    historyDomain.Save(history);
                }
                else
                {
                    comment.ApplyTransition(rubric.GetTestTransition());
                }

                if (suggestion.Files != null)
                {
                    foreach (var f in suggestion.Files.Where(x => x.Id == 0 || !x.Id.HasValue))
                    {
                        var fromBase64 = Convert.FromBase64String(f.Base64);
                        var fileExt = this.GetNameAndExtention(f.FileName);
                        var sugFile = new SuggestionCommentFiles
                        {
                            SuggestionComment = comment,
                            DocumentFile =
                                this.FileManager.SaveFile(
                                    new FileData(fileExt[0], fileExt[1], fromBase64)),
                            DocumentNumber = sug.Number
                        };

                        commentFilesDomain.Save(sugFile);
                    }
                }
                

                return sug.Id;
            }
            catch (Exception e)
            {
                throw new FaultException("\n\tНе успешно: \n" + e.Message);
            }
        }

        private string[] GetNameAndExtention(string fullFileName)
        {
            var result = new string[2];

            var splittedName = fullFileName.Split('.');

            result[1] = splittedName[splittedName.Length - 1];

            var resultName = new StringBuilder();

            for (var i = 0; i < splittedName.Length - 1; i++)
            {
                resultName.Append(string.Format("{0}.", splittedName[i]));
            }

            resultName.Remove(resultName.Length - 1, 1);

            result[0] = resultName.ToString();

            return result;
        }

        public Suggestion[] GetSuggestionList(long[] ids)
        {
            if (ids.Length == 0)
            {
                throw new FaultException<Exception>(new Exception("\n\tНе успешно: не указан параметр ids"));
            }

            var commentService = Container.Resolve<IDomainService<SuggestionComment>>();
            var filesService = Container.Resolve<IDomainService<SuggestionCommentFiles>>();
            var fileManager = Container.Resolve<IFileManager>();

            var suggestions = Container.Resolve<IDomainService<CitizenSuggestion>>()
                .GetAll()
                .Where(x => ids.Contains(x.Id));

            var firstComments = commentService
                .GetAll()
                .Where(x => ids.Contains(x.CitizenSuggestion.Id) && x.IsFirst)
                .Select(x => new
                {
                    x.CitizenSuggestion.Id,
                    x.Description,
                    x.ProblemPlace,
                    x.HasAnswer,
                    x.Answer,
                    x.AnswerDate
                })
                .ToDictionary(x => x.Id);

            var comment = commentService.GetAll()
                   .ToArray()
                   .Select(x => new
                   {
                       sugId = x.CitizenSuggestion.Id,
                       x.Id,
                       x.CreationDate,
                       Executor = x.GetCurrentExecutorType() != ExecutorType.None ? x.GetExecutor(x.GetCurrentExecutorType()).Name : string.Empty
                   });

            var lastComments = comment.AsEnumerable()
                .GroupBy(x => x.sugId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Id)
                    .Select(y => new
                    {
                        y.Id,
                        y.Executor,
                        y.CreationDate
                    }).FirstOrDefault());

            var resultList = new List<Suggestion>();

            foreach (var citizenSuggestion in suggestions)
            {
                var firstComm = firstComments.Get(citizenSuggestion.Id);

                var lastComm = lastComments.Get(citizenSuggestion.Id);

                var suggestion = citizenSuggestion;
                var sug = new Suggestion
                {
                    Id = citizenSuggestion.Id,
                    StateId = citizenSuggestion.State != null ? (long?)citizenSuggestion.State.Id : null,
                    StateName = citizenSuggestion.State != null ? citizenSuggestion.State.Name : string.Empty,
                    Number = citizenSuggestion.Number,
                    RealityObjectId =
                        citizenSuggestion.RealityObject.Return(x => x.Id),
                    Rubric =
                        citizenSuggestion.Rubric != null
                            ? new Rubric
                            {
                                Id = citizenSuggestion.Rubric.Id,
                                Name = citizenSuggestion.Rubric.Name
                            }
                            : null,
                    ApplicantFio = citizenSuggestion.ApplicantFio,
                    ApplicantAddress = citizenSuggestion.ApplicantAddress,
                    ApplicantPhone = citizenSuggestion.ApplicantPhone,
                    ApplicantEmail = citizenSuggestion.ApplicantEmail,
                    ProblemPlace =
                        firstComm.ProblemPlace != null
                            ? new ProblemPlace
                            {
                                Id = firstComm.ProblemPlace.Id,
                                Name = firstComm.ProblemPlace.Name
                            }
                            : null,
                    Description = firstComm.Description,
                    HasAnswer = (lastComm.Id == firstComm.Id || citizenSuggestion.State.FinalState) ? firstComm.HasAnswer : false,
                    AnswerText = (lastComm.Id == firstComm.Id || citizenSuggestion.State.FinalState) ? firstComm.Answer : null,
                    Executor =  lastComm.Executor,
                    Deadline = citizenSuggestion.Deadline,
                    DeadLineDays = citizenSuggestion.Deadline != null && lastComm.CreationDate != null ? (int)(citizenSuggestion.Deadline.Value.Date - lastComm.CreationDate.Value).TotalDays + 1 : 0,
                    Comments =
                        commentService.GetAll()
                            .Where(x => x.CitizenSuggestion.Id == suggestion.Id && !x.IsFirst)
                            .Select(x => new Comment
                            {
                                SuggestionId = suggestion.Id,
                                Question = x.Question,
                                CreationDate = (lastComm.Id != x.Id && x.CitizenSuggestion.State.FinalState) ? x.CreationDate : null,
                                Answer = (lastComm.Id != x.Id && x.CitizenSuggestion.State.FinalState) ? x.Answer : null,
                                Executor = x.GetCurrentExecutorType() != ExecutorType.None ? x.GetExecutor(x.GetCurrentExecutorType()).Name : string.Empty
                            })
                            .ToArray(),
                    Files = filesService.GetAll()
                        .Where(x => x.SuggestionComment.CitizenSuggestion.Id == suggestion.Id && x.SuggestionComment.IsFirst)
                        .ToList()
                        .Select(x => new File
                        {
                            Base64 = fileManager.GetBase64String(x.DocumentFile),
                            FileName = x.DocumentFile.FullName
                        }).ToArray()
                };

#warning добавить файлы

                resultList.Add(sug);
            }

            return resultList.ToArray();
        }

        public Suggestion GetSuggestion(long id)
        {
            if (id == 0)
            {
                throw new FaultException<Exception>(new Exception("\n\tНе успешно: не указан параметр ID"));
            }

            var commentService = Container.ResolveDomain<SuggestionComment>();
            var citizenSuggestionService = Container.ResolveDomain<CitizenSuggestion>();
            var suggestionCommentFilesService = Container.ResolveDomain<SuggestionCommentFiles>();
            var fileManager = Container.Resolve<IFileManager>();

            try
            {
                var citizenSuggestion = citizenSuggestionService.Get(id);

                var commentFiles = suggestionCommentFilesService.GetAll()
                    .Where(x => x.SuggestionComment.CitizenSuggestion.Id == id && !x.SuggestionComment.IsFirst)
                    .Select(x => new
                    {
                        x.Id,
                        CommentId = x.SuggestionComment.Id,
                        x.DocumentFile,
                        x.isAnswer
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.CommentId)
                    .ToDictionary(x => x.Key, y => y.Select(x => new File
                    {
                        Id = x.Id,
                        Base64 = fileManager.GetBase64String(x.DocumentFile),
                        FileName = x.DocumentFile.FullName,
                        IsAnswer = x.isAnswer
                    }).ToArray());

                var firstComment = commentService.GetAll()
                    .Where(x => x.CitizenSuggestion.Id == id && x.IsFirst)
                    .Select(x => new 
                    {
                        x.Description,
                        x.ProblemPlace,
                        x.HasAnswer,
                        x.Answer,
                        x.AnswerDate,
                        FirstExecutor = x.GetCurrentExecutorType() != ExecutorType.None ? x.GetExecutor(x.GetCurrentExecutorType()).Name : string.Empty
                    }).FirstOrDefault();

                var lastComments = commentService.GetAll()
                    .Where(x => x.CitizenSuggestion.Id == id)
                  .ToArray()
                  .Select(x => new
                  {
                      sugId = x.CitizenSuggestion.Id,
                      x.Id,
                      x.CreationDate,
                      x.Answer,
                      x.AnswerDate,
                      Executor = x.GetCurrentExecutorType() != ExecutorType.None ? x.GetExecutor(x.GetCurrentExecutorType()).Name : string.Empty
                  }).OrderByDescending(x => x.Id).FirstOrDefault();

                var comments = commentService.GetAll()
                    .Where(x => x.CitizenSuggestion.Id == id && !x.IsFirst)
                    .Select(
                        x => new
                        {
                            x.Id,
                            SuggestionId = id,
                            x.Question,
                            CreationDate = (lastComments.Id != x.Id || x.CitizenSuggestion.State.FinalState) ? x.AnswerDate : null,
                            Answer = (lastComments.Id != x.Id || x.CitizenSuggestion.State.FinalState) ? x.Answer : null,
                            QuestionDate = x.CreationDate,
                            x.CitizenSuggestion.State.FinalState,
                            Executor = x.GetCurrentExecutorType() != ExecutorType.None ? x.GetExecutor(x.GetCurrentExecutorType()).Name : string.Empty
                        })
                    .ToArray()
                    .Select(
                        x => new Comment
                        {
                            SuggestionId = x.SuggestionId,
                            Question = x.Question,
                            Answer = x.Answer,
                            QuestionDate = x.QuestionDate,
                            CreationDate = x.CreationDate,
                            Files =
                                (lastComments.Id != x.Id || x.FinalState)
                                    ? commentFiles.Get(x.Id)
                                    : (commentFiles.ContainsKey(x.Id) ? commentFiles.Get(x.Id).Where(y => !y.IsAnswer ?? true).ToArray() : null),
                            Executor = x.Executor
                        })
                    .ToArray();

                return new Suggestion
                {
                    Id = citizenSuggestion.Id,
                    StateId = citizenSuggestion.State != null ? (long?) citizenSuggestion.State.Id : null,
                    StateName = citizenSuggestion.State != null ? citizenSuggestion.State.Name : string.Empty,
                    Number = citizenSuggestion.Number,
                    RealityObjectId = citizenSuggestion.RealityObject != null ? citizenSuggestion.RealityObject.Id : 0,
                    Rubric = new Rubric
                    {
                        Id = citizenSuggestion.Rubric.Id,
                        Name = citizenSuggestion.Rubric.Name
                    },
                    ApplicantFio = citizenSuggestion.ApplicantFio,
                    ApplicantAddress = citizenSuggestion.ApplicantAddress,
                    ApplicantPhone = citizenSuggestion.ApplicantPhone,
                    ApplicantEmail = citizenSuggestion.ApplicantEmail,
                    ProblemPlace = firstComment.ProblemPlace != null
                    ? new ProblemPlace
                    {
                        Id = firstComment.ProblemPlace.Id,
                        Name = firstComment.ProblemPlace.Name
                    } : null,
                    Description = firstComment.Description,
                    FirstExecutor = firstComment.FirstExecutor,
                    HasAnswer = comments.Any() || citizenSuggestion.State.FinalState ? firstComment.HasAnswer : false,
                    AnswerText = comments.Any() || citizenSuggestion.State.FinalState ? firstComment.Answer : null,
                    AnswerDate = comments.Any() || citizenSuggestion.State.FinalState ? firstComment.AnswerDate : null,
                    Executor = lastComments != null ? lastComments.Executor : null,
                    Comments = comments,
                    Deadline = citizenSuggestion.Deadline,
                    DeadLineDays = citizenSuggestion.Deadline != null && lastComments.CreationDate != null ? (int)(citizenSuggestion.Deadline.Value.Date - lastComments.CreationDate.Value).TotalDays + 1 : 0,
                    Files = suggestionCommentFilesService.GetAll()
                        .Where(
                            x =>
                                x.SuggestionComment.CitizenSuggestion.Id == citizenSuggestion.Id &&
                                x.SuggestionComment.IsFirst &&
                                (comments.Any() || citizenSuggestion.State.FinalState || !x.isAnswer))
                        .ToList()
                        .Select(x => new File
                        {
                            Id = x.Id,
                            Base64 = fileManager.GetBase64String(x.DocumentFile),
                            FileName = x.DocumentFile.FullName
                        }).ToArray()
                };
            }
            finally
            {
                Container.Release(commentService);
                Container.Release(citizenSuggestionService);
                Container.Release(suggestionCommentFilesService);
                Container.Release(fileManager);
            }
        }

        public long AddComment(Comment comment)
        {

            var commentService = Container.Resolve<IDomainService<SuggestionComment>>();
            var suggestionService = Container.Resolve<IDomainService<CitizenSuggestion>>();
            var commentFilesDomain = Container.ResolveDomain<SuggestionCommentFiles>();

            var suggestion = suggestionService.Load(comment.SuggestionId);

            if (comment.IsAcceptance)
            {
                suggestion.Close();
                suggestionService.Update(suggestion);
                return 0;
            }

            var sugComment = new SuggestionComment
            {
                CitizenSuggestion = suggestion,
                CreationDate = DateTime.Now,
                Question = comment.Question,
                Answer = comment.Answer,
                AnswerDate = comment.CreationDate
            };

            var prevComment =
                commentService.GetAll()
                    .Where(x => x.CitizenSuggestion.Id == comment.SuggestionId)
                    .OrderByDescending(x => x.Id)
                    .First();

            var execType = prevComment.GetCurrentExecutorType();
            var transitions = sugComment.CitizenSuggestion.Rubric.Transitions.ToArray();

            foreach (var transition in transitions)
            {
                if (transition.InitialExecutorType == execType)
                {
                    sugComment.ApplyTransition(transition);
                    if (sugComment.GetExecutor(sugComment.GetCurrentExecutorType()) == null)
                    {
                        throw new FaultException<Exception>(new Exception("\n\tКоментарий не может быть добавлен, т.к. неопределен следующий исполнитель"));
                    }
                }
            }

            commentService.Save(sugComment);

            if (comment.Files != null)
            {
                foreach (var f in comment.Files.Where(x => x.Id == 0 || !x.Id.HasValue))
                {
                    var fromBase64 = Convert.FromBase64String(f.Base64);
                    var fileExt = this.GetNameAndExtention(f.FileName);
                    var sugFile = new SuggestionCommentFiles
                    {
                        SuggestionComment = sugComment,
                        DocumentFile =
                            this.FileManager.SaveFile(
                                new FileData(fileExt[0], fileExt[1], fromBase64)),
                        DocumentNumber = sugComment.CitizenSuggestion.Number
                    };

                    commentFilesDomain.Save(sugFile);
                }
            }

            suggestionService.Update(suggestion);
            return sugComment.Id;
        }
    }

}