using Bars.Gkh.Authentification;

namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class PersonService : IPersonService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Person> Domain { get; set; }

        public IDomainService<PersonPlaceWork> PlaceWorkDomain { get; set; }

        public IDomainService<ContragentContact> ContactDomain { get; set; }

        public IDomainService<PersonQualificationCertificate> QualDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        public IDomainService<Position> PositionDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<FieldRequirement> FieldRequirementDomain { get; set; }

        public IGkhUserManager UserManager { get; set; }

        public IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            var contragentList = UserManager.GetContragentIds();

            var loadParams = baseParams.GetLoadParam();

            var currDate = DateTime.Now;

            var ctDist = PlaceWorkDomain.GetAll()
                .Where(x => !x.EndDate.HasValue || x.EndDate > DateTime.Now)
                .Select(x => new
                {
                    x.Person.Id,
                    ContragentName = x.Contragent.Name,
                    x.StartDate
                })
                 .AsEnumerable()
                    .GroupBy(x => x.Id)
                     .ToDictionary(
                           x => x.Key,
                        y => y.OrderByDescending(z => z.StartDate).Select(z => z.ContragentName).FirstOrDefault());

            //new
            //{
            //    ContragentName = y.OrderByDescending(z => z.StartDate).Select(z => z.ContragentName).FirstOrDefault()
            //});

            var dictQual =
                QualDomain.GetAll()
                    .Where(x => x.Person != null && x.IssuedDate != null)
                    .WhereIf(contragentList.Any(), x => PlaceWorkDomain.GetAll().Any(y => y.Person.Id == x.Person.Id && contragentList.Contains(y.Contragent.Id) && y.StartDate <= currDate && (!y.EndDate.HasValue || y.EndDate >= currDate)))
                    .Select(x => new { x.Id, personId = x.Person.Id, x.IssuedDate, x.Number, x.EndDate })
                    .AsEnumerable()
                    .GroupBy(x => x.personId)
                    .ToDictionary(
                        x => x.Key,
                        y =>
                        new
                        {
                            IssuedDate = y.OrderByDescending(z => z.IssuedDate).Select(z => z.IssuedDate).FirstOrDefault(),
                            Number = y.OrderByDescending(z => z.IssuedDate).Select(z => z.Number).FirstOrDefault(),
                            EndDate = y.OrderByDescending(z => z.EndDate).Select(z => z.EndDate).FirstOrDefault()
                        });

            var dataEnumerable = Domain.GetAll()
                                    .WhereIf(contragentList.Any(), x => PlaceWorkDomain.GetAll().Any(y => y.Person.Id == x.Id && contragentList.Contains(y.Contragent.Id) && y.StartDate <= currDate && (!y.EndDate.HasValue || y.EndDate >= currDate)))
                                    .Select(x => new
                                    {
                                        x.Id,
                                        x.FullName,
                                        x.Inn,
                                        x.AddressLive,
                                        x.State
                                    })
                                    .AsEnumerable();

            var data =
                dataEnumerable.Select(
                    x =>
                    new
                    {
                        x.Id,
                        x.FullName,
                        x.Inn,
                        x.AddressLive,
                        x.State,
                        QcNumber = dictQual.ContainsKey(x.Id) ? dictQual[x.Id].Number : "",
                        QcIssuedDate = dictQual.ContainsKey(x.Id) ? (DateTime?)dictQual[x.Id].IssuedDate : null,
                        QcEndDate = dictQual.ContainsKey(x.Id) ? dictQual[x.Id].EndDate : null,
                        ContragentName = ctDist.ContainsKey(x.Id) ? ctDist[x.Id] : ""
                    }).AsQueryable().Filter(loadParams, Container);

            totalCount = dataEnumerable.Count();

            return isPaging ? data.Order(loadParams).Paging(loadParams).ToList() : data.Order(loadParams).ToList();
        }

        public IDataResult GetContactDetails(BaseParams baseParams)
        {
            var contactId = baseParams.Params.GetAs("contactId", 0L);

            var contact = ContactDomain.GetAll().FirstOrDefault(x => x.Id == contactId);
            if (contact == null)
            {
                return new BaseDataResult(false, string.Format("Не удалось определить контактную информацию по Id {0}", contactId));
            }

            object details;
            // мы же хотим иметь информацию об актуальном месте работы
            if (contact.DateStartWork.HasValue && contact.DateStartWork <= DateTime.Today && (!contact.DateEndWork.HasValue || contact.DateEndWork >= DateTime.Today))
            {
                details =
                    new
                    {
                        contact.Name,
                        contact.Surname,
                        contact.Patronymic,
                        contact.Email,
                        contact.Phone,
                        StartDate = contact.DateStartWork,
                        EndDate = contact.DateEndWork,
                        Position = contact.Position != null ? new { contact.Position.Name, contact.Position.Id } : null,
                        Contragent = new { contact.Contragent.Name, contact.Contragent.Id }
                    };
            }
            else
            {
                details = new { contact.Name, contact.Surname, contact.Patronymic, contact.Email, contact.Phone };
            }

            return new BaseDataResult(details);
        }

        public IDataResult AddWorkPlace(BaseParams baseParams)
        {
            var personId = baseParams.Params.GetAs("personId", 0L);
            var contragentId = baseParams.Params.GetAs("contragentId", 0L);
            var startDate = baseParams.Params.GetAs("startDate", DateTime.MinValue).Return(x => x == DateTime.MinValue ? null : (DateTime?)x);
            var endDate = baseParams.Params.GetAs("endDate", DateTime.MinValue).Return(x => x == DateTime.MinValue ? null : (DateTime?)x);
            var positionId = baseParams.Params.GetAs("positionId", 0L);

            var existPerms = FieldRequirementDomain.GetAll()
                .Select(x => x.RequirementId)
                .Distinct()
                .ToList();

            if (startDate == null && existPerms.Contains("GkhGji.PersonRegisterGji.Field.StartDate_Rqrd"))
            {
                return new BaseDataResult(false, "Дата начала работы не заполнена");
            }

            var person = Domain.Get(personId);
            if (person == null)
            {
                return new BaseDataResult(false, string.Format("Не удалось определить должностное лицо по Id {0}", personId));
            }

            var contragent = ContragentDomain.Get(contragentId);
            if (contragent == null && existPerms.Contains("GkhGji.PersonRegisterGji.Field.Contragent_Rqrd"))
            {
                return new BaseDataResult(false, string.Format("Не удалось определить контрагента по Id {0}", contragentId));
            }

            var position = PositionDomain.Get(positionId);

            PlaceWorkDomain.Save(new PersonPlaceWork { Person = person, Contragent = contragent, StartDate = startDate, EndDate = endDate, Position = position });

            return new BaseDataResult();
        }

        public IDataResult ChangeOfCertificateStatus()
        {
            var stateEnd = StateDomain.GetAll().Where(x=>x.TypeId == "gkh_person_qc").FirstOrDefault(x => x.Code == "Expired");
            var qualCertificates = QualDomain.GetAll().Where(x => x.State.StartState);
            foreach (var item in qualCertificates)
            {
                if (item.EndDate.HasValue && item.EndDate < DateTime.Now)
                {
                    item.State = stateEnd;
                    QualDomain.Update(item);
                }
            }
            return new BaseDataResult(true);
        }
    }
}