namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using System;

    using B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Entities;

    public class PersonRequestToExamViewModel : BaseViewModel<PersonRequestToExam>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<PersonPlaceWork> PlaceWorkDomain { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<PersonRequestToExam> domain, BaseParams baseParams)
        {
            var contragentList = this.UserManager.GetContragentIds();
            var currDate = DateTime.Now;

            var loadParams = this.GetLoadParam(baseParams);

            var personId = loadParams.Filter.GetAs("personId", 0L);
            var showAll = loadParams.Filter.GetAs("showAll", false);

            if (personId == 0)
            {
                personId = baseParams.Params.GetAsId("personId");
            }

            var data = domain.GetAll()
                .WhereIf(showAll && contragentList.Any(), x => this.PlaceWorkDomain.GetAll().Any(y => y.Person.Id == x.Person.Id && contragentList.Contains(y.Contragent.Id) && y.StartDate <= currDate && (!y.EndDate.HasValue || y.EndDate >= currDate)))
                .WhereIf(!showAll, x => x.Person.Id == personId)
                .Select(x => new
                {
                    x.Id,
                    x.RequestNum,
                    x.RequestDate,
                    x.Person.FullName,
                    Person = x.Person.Id,
                    x.State,
                    Name = "№ " + x.RequestNum+" от "+x.RequestDate.ToShortDateString() 
                    // Внимание, Так делаю только потому что изначально знаю что поле Скрыто от рук пользоватле, нужно для того чтобы при выборе ихз спраовчника сразуже поставлять в филд корректное требуемое сочетани
                    // Если данне поле будет в дальнейшем выведено в грид и будет сорироватся или фильтрвоатся то нужно переделывать
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<PersonRequestToExam> domainService, BaseParams baseParams)
        {
            var qualCertificateDomain = this.Container.ResolveDomain<PersonQualificationCertificate>();

            try
            {
                var id = baseParams.Params.GetAsId("Id");
                var record = domainService.Get(id);

                var hasCert = qualCertificateDomain.GetAll().Any(x => x.RequestToExam.Id == id);

                return new BaseDataResult(record != null
                    ? new
                        {
                            record.Id,
                            record.Person,
                            record.RequestSupplyMethod,
                            record.RequestNum,
                            record.RequestDate,
                            record.RequestTime,
                            record.RequestFile,
							record.PersonalDataConsentFile,
                            record.NotificationNum,
                            record.NotificationDate,
                            record.IsDenied,
                            record.ExamDate,
                            record.ExamTime,
                            record.CorrectAnswersPercent,
                            record.ProtocolNum,
                            record.ProtocolDate,
                            record.ProtocolFile,
                            record.ResultNotificationNum,
                            record.ResultNotificationDate,
                            record.MailingDate,
                            record.State,
                            HasCert = hasCert
                        }
                    : null);
            }
            finally
            {
                this.Container.Release(qualCertificateDomain);
            }
        }
    }
}