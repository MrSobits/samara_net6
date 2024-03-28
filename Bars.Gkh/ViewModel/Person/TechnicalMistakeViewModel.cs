namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Представление <see cref="TechnicalMistake"/>
    /// </summary>
    public class TechnicalMistakeViewModel : BaseViewModel<TechnicalMistake>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TechnicalMistake> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var qualificationId = loadParams.Filter.GetAsId("qualificationId");

            var query = domainService.GetAll()
                .Where(x => x.QualificationCertificate.Id == qualificationId)
                .Filter(loadParams, this.Container);

            return new ListDataResult(query.Order(loadParams).Paging(loadParams).ToList(), query.Count());
        }
    }
}