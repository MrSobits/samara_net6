namespace Bars.GkhGji.Regions.Tyumen.Interceptors.Suggestion
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.IoC;

    using Gkh.Entities.Suggestion;
    using Gkh.Enums;

    using GkhGji.Entities;

    public class CitizenSuggestionServiceInterceptor : EmptyDomainInterceptor<CitizenSuggestion>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CitizenSuggestion> service, CitizenSuggestion entity)
        {
            return ValidationAnswerDate(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<CitizenSuggestion> service, CitizenSuggestion entity)
        {
            var result = ValidationAnswerDate(entity);
            if (!result.Success)
            {
                return result;
            }

            return Success();
        }

        private IDataResult ValidationAnswerDate(CitizenSuggestion entity)
        {
            var commentDomain = Container.Resolve<IDomainService<SuggestionComment>>();

            try
            {
                if (entity.AnswerDate.HasValue && entity.AnswerDate.Value < entity.CreationDate)
                {
                    return Failure(string.Format("Дата ответа не может быть меньше даты обращения = '{0}'", entity.CreationDate.ToShortDateString()));
                }

                if (entity.AnswerDate.HasValue && entity.Deadline.HasValue && entity.AnswerDate.Value > entity.Deadline.Value)
                {
                    return
                        Failure(string.Format("Дата ответа должна входит в диапазон между 'Датой обращения'='{0}' и 'Контрольным сроком'='{1}' ", entity.CreationDate.ToShortDateString(),
                            entity.Deadline.Value.ToShortDateString()));
                }

                var commentDates = commentDomain.GetAll()
                    .Where(x => x.CitizenSuggestion.Id == entity.Id && x.AnswerDate.HasValue && !x.IsFirst)
                    .Select(x => x.AnswerDate.Value)
                    .ToList();

                foreach (var date in commentDates)
                {
                    if (date < entity.CreationDate.Date)
                    {
                        return Failure(string.Format(
                            "В таблице 'Дополнительные вопросы' значение 'Дата' " +
                            "не может быть меньше 'Даты обращения'='{0}'",
                            entity.CreationDate.ToShortDateString()));
                    }

                    if (entity.Deadline.HasValue && date > entity.Deadline.Value)
                    {
                        return Failure(string.Format(
                            "В таблице 'Дополнительные вопросы' значение 'Дата'" +
                            " должна входит в диапазон между 'Датой обращения'='{0}' и 'Контрольным сроком'='{1}' ",
                            entity.CreationDate.ToShortDateString(),
                            entity.Deadline.Value.ToShortDateString()));
                    }
                }

                return Success();
            }
            finally
            {
                Container.Release(commentDomain);
            }
        }

        public override IDataResult AfterUpdateAction(IDomainService<CitizenSuggestion> service, CitizenSuggestion entity)
        {
            if (entity.GetCurrentExecutorType() == ExecutorType.Gji)
            {
                CreateAppealCits(entity);
            }

            return Success();
        }

        private void CreateAppealCits(CitizenSuggestion entity)
        {
            var appealCitsDomain = Container.ResolveDomain<AppealCits>();
            var appealCitsRealObjDomain = Container.ResolveDomain<AppealCitsRealityObject>();

            using(Container.Using(appealCitsDomain))
            {
                var appealExists = appealCitsDomain.GetAll().Count(x => x.DocumentNumber == entity.Number) > 0;

                if (appealExists)
                {
                    return;
                }

                var apppealCits = new AppealCits
                {
                    DocumentNumber = entity.Number,
                    DateFrom = entity.CreationDate,
                    CheckTime = entity.Deadline,
                    Correspondent = entity.ApplicantFio,
                    Description = entity.Description,
                    DescriptionLocationProblem = entity.ProblemPlace != null ? entity.ProblemPlace.Name : null,
                    Email = entity.ApplicantEmail,
                    Phone = entity.ApplicantPhone,
                    CorrespondentAddress = entity.ApplicantAddress
                };

                appealCitsDomain.Save(apppealCits);

                appealCitsRealObjDomain.Save(new AppealCitsRealityObject
                {
                    AppealCits = apppealCits,
                    RealityObject = entity.RealityObject
                });
            }
        }
    }
}