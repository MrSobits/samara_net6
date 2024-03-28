namespace Bars.Gkh.Services.Impl.Suggestion
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
    using Bars.Gkh.Utils;

    using ProblemPlace = DataContracts.Suggestion.ProblemPlace;
    using Rubric = Bars.Gkh.Services.DataContracts.Suggestion.Rubric;

    // TODO wcf
    // [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SuggestionService : ISuggestionService
    {

        public IWindsorContainer Container { get; set; }

        public IFileManager FileManager { get; set; }

        public ProblemPlace[] GetProblemPlaces()
        {
            return
                Container.Resolve<IDomainService<Entities.Suggestion.ProblemPlace>>().GetAll()
                    .Select(x => new ProblemPlace { Id = x.Id, Name = x.Name })
                    .ToArray();
        }
        
        public DataContracts.Suggestion.TypeProblem[] GetTypeProblems(long id)
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
                Container.Resolve<IDomainService<Entities.Suggestion.Rubric>>().GetAll()
                    .Select(x => new Rubric { Id = x.Id, Name = x.Name })
                    .ToArray();
        }

        public DataContracts.Suggestion.CategoryPosts[] GetCategoryPosts()
        {
            return
                Container.Resolve<IDomainService<Entities.Suggestion.CategoryPosts>>().GetAll()
                    .Select(x => new DataContracts.Suggestion.CategoryPosts { Id = x.Id, Name = x.Name, Code = x.Code })
                    .ToArray();
        }

        public DataContracts.Suggestion.MessageSubject[] GetMessageSubjects(long id)
        {
            var categoryService = Container.ResolveDomain<Entities.Suggestion.CategoryPosts>();
            var messageService = Container.ResolveDomain<Entities.Suggestion.MessageSubject>();

            try
            {
                var categoryId = categoryService.Get(id).ReturnSafe(x => x.Id);

                return messageService.GetAll().Where(x => x.CategoryPosts.Id == categoryId)
                        .Select(
                            x => new DataContracts.Suggestion.MessageSubject {Id = x.Id, Name = x.Name, Code = x.Code})
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
                var suggestionFileService = Container.ResolveDomain<CitizenSuggestionFiles>();
                var realObj = Container.ResolveDomain<RealityObject>().Load(suggestion.RealityObjectId);
                var rubric = Container.ResolveDomain<Entities.Suggestion.Rubric>().Load(suggestion.Rubric.Id);

            try
            {
                Entities.Suggestion.ProblemPlace problemPlace;
                if (suggestion.ProblemPlace.Id > 0)
                {
                    problemPlace =
                        Container.Resolve<IDomainService<Entities.Suggestion.ProblemPlace>>()
                            .Load(suggestion.ProblemPlace.Id);
                }
                else
                {
                    problemPlace = new Entities.Suggestion.ProblemPlace {Name = suggestion.ProblemPlace.Name};
                    Container.Resolve<IDomainService<Entities.Suggestion.ProblemPlace>>().Save(problemPlace);
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
                    ProblemPlace = problemPlace,
                    Description = suggestion.Description,
                    Year = year,
                    Num = num
                };

                sug.ApplyTransition(rubric.GetZeroTransition(), rubric);

                suggestionService.Save(sug);
                var history = new CitizenSuggestionHistory(sug, sug.GetCurrentExecutorType());
                historyDomain.Save(history);


                foreach (var f in suggestion.Files.Where(x => x.Id == 0 || !x.Id.HasValue))
                {
                    var fromBase64 = Convert.FromBase64String(f.Base64);
                    var fileExt = GetNameAndExtention(f.FileName);
                    var sugFile = new CitizenSuggestionFiles
                    {
                        CitizenSuggestion = sug,
                        DocumentFile =
                            FileManager.SaveFile(
                                new FileData(fileExt[0], fileExt[1], fromBase64)),
                        DocumentNumber = sug.Number
                    };

                    suggestionFileService.Save(sugFile);
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
            var filesService = Container.Resolve<IDomainService<CitizenSuggestionFiles>>();
            var fileManager = Container.Resolve<IFileManager>();

            var suggestions = Container.Resolve<IDomainService<CitizenSuggestion>>()
                .GetAll()
                .Where(x => ids.Contains(x.Id));

            var resultList = new List<Suggestion>();

            foreach (var citizenSuggestion in suggestions)
            {
                var suggestion = citizenSuggestion;
                var sug = new Suggestion
                {
                    Id = citizenSuggestion.Id,
                    Number = citizenSuggestion.Number,
                    StateId = citizenSuggestion.State != null ? (long?)citizenSuggestion.State.Id : null,
                    StateName = citizenSuggestion.State != null ? citizenSuggestion.State.Name : string.Empty,
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
                        citizenSuggestion.ProblemPlace != null
                            ? new ProblemPlace
                            {
                                Id = citizenSuggestion.ProblemPlace.Id,
                                Name = citizenSuggestion.ProblemPlace.Name
                            }
                            : null,
                    Description = citizenSuggestion.Description,
                    HasAnswer = citizenSuggestion.HasAnswer,
                    AnswerText = citizenSuggestion.AnswerText,
                    Comments =
                        commentService.GetAll()
                            .Where(x => x.CitizenSuggestion.Id == suggestion.Id)
                            .Select(x => new Comment
                            {
                                SuggestionId = suggestion.Id,
                                Question = x.Question,
                                CreationDate = x.CreationDate,
                                Answer = x.Answer
                            })
                            .ToArray(),
                    Files = filesService.GetAll()
                        .Where(x => x.CitizenSuggestion.Id == suggestion.Id)
                        .ToList()
                        .Where(x => this.FileManager.IsExistsFile(x.DocumentFile))
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
            var filesService = Container.ResolveDomain<CitizenSuggestionFiles>();
            var suggestionCommentFilesService = Container.ResolveDomain<SuggestionCommentFiles>();
            var fileManager = Container.Resolve<IFileManager>();

            try
            {
                var citizenSuggestion = citizenSuggestionService.Get(id);

                var commentFiles = suggestionCommentFilesService.GetAll()
                    .Where(x => x.SuggestionComment.CitizenSuggestion.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        CommentId = x.SuggestionComment.Id,
                        x.DocumentFile
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.CommentId)
                    .ToDictionary(x => x.Key, y => y.Select(x => new File
                    {
                        Id = x.Id,
                        Base64 = fileManager.GetBase64String(x.DocumentFile),
                        FileName = x.DocumentFile.FullName
                    }).ToArray());

                var comments = commentService.GetAll()
                    .Where(x => x.CitizenSuggestion.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        SuggestionId = id,
                        x.Question,
                        CreationDate = x.AnswerDate ?? DateTime.MinValue,
                        x.Answer,
                        QuestionDate = x.CreationDate
                    })
                    .ToArray()
                    .Select(x => new Comment
                    {
                        SuggestionId = x.SuggestionId,
                        Question = x.Question,
                        Answer = x.Answer,
                        QuestionDate = x.QuestionDate,
                        CreationDate = x.CreationDate,
                        Files = commentFiles.Get(x.Id)
                    })
                    .ToArray();

                return new Suggestion
                {
                    Id = citizenSuggestion.Id,
                    StateId = citizenSuggestion.State != null ? (long?)citizenSuggestion.State.Id : null,
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
                    ProblemPlace = new ProblemPlace
                    {
                        Id = citizenSuggestion.ProblemPlace.Id,
                        Name = citizenSuggestion.ProblemPlace.Name
                    },
                    Description = citizenSuggestion.Description,
                    HasAnswer = citizenSuggestion.HasAnswer,
                    AnswerText = citizenSuggestion.AnswerText,
                    AnswerDate = citizenSuggestion.AnswerDate ?? DateTime.MinValue,
                    Comments = comments,
                    Files = filesService.GetAll()
                        .Where(x => x.CitizenSuggestion.Id == citizenSuggestion.Id)
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
                Container.Release(filesService);
                Container.Release(suggestionCommentFilesService);
                Container.Release(fileManager);
            }
        }

        public long AddComment(Comment comment)
        {

            var commentService = Container.Resolve<IDomainService<SuggestionComment>>();
            var suggestionService = Container.Resolve<IDomainService<CitizenSuggestion>>();

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
                Answer = comment.Answer
            };

            commentService.Save(sugComment);
            suggestion.Open();
            suggestionService.Update(suggestion);
            return sugComment.Id;
        }
    }

}