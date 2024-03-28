namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Suggestion;

    /// <summary>
    /// Представление для <see cref="CitizenSuggestion"/>
    /// </summary>
    public class CitizenSuggestionViewModel : Bars.Gkh.ViewModel.CitizenSuggestionViewModel
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<CitizenSuggestion> domainService, BaseParams baseParams)
        {
            var suggestionCommentDomain = this.Container.ResolveDomain<SuggestionComment>();

            try
            {
                var loadParams = this.GetLoadParam(baseParams);

                var data = domainService.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.Number,
                        x.ApplicantAddress,
                        x.ApplicantEmail,
                        x.ApplicantFio,
                        x.ApplicantPhone,
                        x.CreationDate,
                        x.Description,
                        ProblemPlace = x.ProblemPlace != null ? x.ProblemPlace.Name : null,
                        x.RealityObject.Address,
                        x.Flat.RoomNum,
                        RoomAddress = x.RealityObject.Address + ", кв. " + x.Flat.RoomNum,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                        Settlement = x.RealityObject.MoSettlement.Name,
                        RubricName = x.Rubric.Name,
                        x.AnswerText,
                        x.HasAnswer,
                        AllHasAnswer = false
                    })
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();
                var list = data
                    .OrderIf(loadParams.Order.Length == 0, false, x => x.CreationDate)
                    .Order(loadParams)
                    .Paging(loadParams)
                    .ToList();

                var suggIds = list.Select(x => x.Id).ToList();
                var suggestionAnswers = suggestionCommentDomain.GetAll()
                    .Where(x => suggIds.Contains(x.CitizenSuggestion.Id))
                    .Select(x => new
                    {
                        x.CitizenSuggestion.Id,
                        x.Answer
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.All(x => !x.Answer.IsEmpty()));

                list = list.Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.Number,
                        x.ApplicantAddress,
                        x.ApplicantEmail,
                        x.ApplicantFio,
                        x.ApplicantPhone,
                        CreationDate = x.CreationDate.Date,
                        x.Description,
                        x.ProblemPlace,
                        x.Address,
                        x.RoomNum,
                        x.RoomAddress,
                        x.MunicipalityName,
                        x.Settlement,
                        x.RubricName,
                        x.AnswerText,
                        x.HasAnswer,
                        AllHasAnswer = !x.AnswerText.IsEmpty() && (!suggestionAnswers.ContainsKey(x.Id) || suggestionAnswers.Get(x.Id))
                    })
                    .ToList();

                return new ListDataResult(list, totalCount);
            }
            finally
            {
                this.Container.Release(suggestionCommentDomain);
            }
        }
    }
}