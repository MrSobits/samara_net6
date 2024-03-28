namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Представление <see cref="QualificationDocument"/>
    /// </summary>
    public class QualificationDocumentViewModel : BaseViewModel<QualificationDocument>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<QualificationDocument> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var typeDocument = loadParams.Filter.GetAs<QualificationDocumentType>("typeDocument");
            var qualificationId = loadParams.Filter.GetAsId("qualificationId");

            var query = domainService.GetAll()
                .Where(x => x.QualificationCertificate.Id == qualificationId)
                .Where(x => x.DocumentType == typeDocument)
                .Filter(loadParams, this.Container);

            return new ListDataResult(query.Order(loadParams).Paging(loadParams).ToList(), query.Count());
        }
    }
}