namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;
    using Bars.Gkh.Domain;

    public class PersonQualificationCertificateViewModel : BaseViewModel<PersonQualificationCertificate>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<PersonQualificationCertificate> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var personId = loadParams.Filter.GetAs("personId", 0L);

            if (personId == 0)
            {
                var data = domain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.IssuedDate,
                        personId = x.Person.Id,
                        x.Person,
                        x.Number,
                        x.IssuedBy,
                        x.Person.FullName,
                        x.TypeCancelation,
                        x.ApplicationDate,
                        x.BlankNumber,
                        x.CancelationDate,
                        x.CancelNumber,
                        x.EndDate,
                        x.File,
                        x.IsFromAnotherRegion,
                        x.State
                    }
                    ).AsQueryable()
                .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }

            var requestDomain = this.Container.Resolve<IDomainService<PersonRequestToExam>>();

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

            try
            {
                var data = domain.GetAll()
                .Where(x => x.Person.Id == personId)
                .Select(x => new
                {
                    x.Id,
                    personId = x.Person.Id,
                    requestId = x.RequestToExam != null ? x.RequestToExam.Id : 0,
                    x.Number,
                    x.IssuedBy,
                    x.IssuedDate,
                    x.EndDate,
                    x.TypeCancelation,
                    x.CancelationDate,
                    x.State
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    RequestToExamName = dictRequests.ContainsKey(x.requestId) ? dictRequests[x.requestId] : null,
                    x.Number,
                    x.IssuedDate,
                    x.IssuedBy,
                    x.EndDate,
                    x.TypeCancelation,
                    x.CancelationDate,
                    x.State
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
                record.RequestToExam.Name = string.Format("№ {0} от {1}", record.RequestToExam.RequestNum,
                    record.RequestToExam.RequestDate.ToShortDateString());
            }

            return new BaseDataResult(record);
        }
    }
}