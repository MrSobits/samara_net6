namespace Bars.Gkh.Regions.Perm.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class PersonQualificationCertificateViewModel : BaseViewModel<PersonQualificationCertificate>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<PersonQualificationCertificate> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var personId = loadParams.Filter.GetAs("personId", 0L);

            var requestDomain = this.Container.ResolveDomain<PersonRequestToExam>();
            var qualificationDocumentDomain = this.Container.ResolveDomain<QualificationDocument>();

            try
            {
                var dictRequests = requestDomain.GetAll()
                .Where(x => x.Person.Id == personId)
                .Select(x => new
                {
                    PersonId = x.Person.Id,
                    x.Id,
                    x.RequestNum,
                    x.RequestDate
                })
                .ToList()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(z => "№ " + z.RequestNum + " от " + z.RequestDate.ToShortDateString()).FirstOrDefault());

                var qualificationDocuments = qualificationDocumentDomain.GetAll()
                    .Where(x => x.QualificationCertificate.Person.Id == personId)
                    .ToList();

                var lastBlankNumber = qualificationDocuments
                    .GroupBy(x => x.QualificationCertificate.Id)
                    .ToDictionary(
                        x => x.Key,
                        x => x.OrderByDescending(y => y.IssuedDate).ThenByDescending(y => y.Id).Select(y => y.Number).FirstOrDefault());

                var renewLastDate = qualificationDocuments
                    .Where(x => x.DocumentType == QualificationDocumentType.Renew)
                    .GroupBy(x => x.QualificationCertificate.Id)
                    .ToDictionary(
                        x => x.Key,
                        x => x.OrderByDescending(y => y.IssuedDate).ThenByDescending(y => y.Id).Select(y => y.IssuedDate).FirstOrDefault());

                var dublicateLastDate = qualificationDocuments
                    .Where(x => x.DocumentType == QualificationDocumentType.Duplicate)
                    .GroupBy(x => x.QualificationCertificate.Id)
                    .ToDictionary(
                        x => x.Key,
                        x => x.OrderByDescending(y => y.IssuedDate).ThenByDescending(y => y.Id).Select(y => y.IssuedDate).FirstOrDefault());

                var data = domain.GetAll()
                .Where(x => x.Person.Id == personId)
                .Select(x => new
                {
                    x.Id,
                    personId = x.Person.Id,
                    requestId = x.RequestToExam != null ? x.RequestToExam.Id : 0,
                    x.Number,
                    x.IssuedDate,
                    x.EndDate,
                    x.TypeCancelation,
                    x.CancelationDate,
                    x.HasDuplicate,
                    x.BlankNumber,
                    x.RequestToExam.ProtocolDate,
                    ProtocolNumber = x.RequestToExam.ProtocolNum,
                    x.IsFromAnotherRegion,
                    HasRenewed = x.HasCancelled ? x.HasRenewed : (bool?)null
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    RequestToExamName = dictRequests.ContainsKey(x.requestId) ? dictRequests[x.requestId] : null,
                    x.Number,
                    x.IssuedDate,
                    x.EndDate,
                    x.TypeCancelation,
                    x.CancelationDate,
                    BlankNumber = x.HasDuplicate ? lastBlankNumber.Get(x.Id) : x.BlankNumber,
                    x.ProtocolDate,
                    x.ProtocolNumber,
                    DublicateIssuedDate = dublicateLastDate.Get(x.Id),
                    RenewIssuedDate = renewLastDate.Get(x.Id),
                    x.IsFromAnotherRegion,
                    x.HasRenewed
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(requestDomain);
            }
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<PersonQualificationCertificate> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("Id");
            var record = domainService.Get(id);

            if (record.RequestToExam != null)
            {
                record.RequestToExam.Name = $"№ {record.RequestToExam.RequestNum} от {record.RequestToExam.RequestDate.ToShortDateString()}";
            }

            return new BaseDataResult(record);
        }
    }
}