namespace Bars.Gkh.DataProviders
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.DataProviders.Meta;
    using Bars.Gkh.Entities;
    using Castle.Windsor;

    class QualificationCertificateData : BaseCollectionDataProvider<QalificationCertMeta>
    {
        public IDomainService<PersonQualificationCertificate> CertificateDomain { get; set; }

        public QualificationCertificateData(IWindsorContainer container) : base(container)
        {
        }

        protected override IQueryable<QalificationCertMeta> GetDataInternal(BaseParams baseParams)
        {
            var allCerts = CertificateDomain.GetAll()
              .Select(x => new
              {
                  x.Person.FullName,
                  x.Person.Birthdate,
                  x.Person.AddressBirth,
                  x.Number,
                  x.BlankNumber,
                  RteId = x.RequestToExam != null ? x.RequestToExam.Id : 0,
                  RteProtocolNum = x.RequestToExam != null ? x.RequestToExam.ProtocolNum : null,
                  RteProtocolDate = x.RequestToExam != null ? x.RequestToExam.ProtocolDate : DateTime.MinValue,
                  x.HasDuplicate,
                  x.DuplicateNumber,
                  x.DuplicateIssuedDate,
                  x.HasCancelled,
                  x.CancelNumber,
                  x.CancelProtocolDate,
                  x.HasRenewed,
                  x.CourtActNumber,
                  x.CourtActDate
              })
              .OrderBy(x => x.FullName)
              .ToList()
              .Select(x => new QalificationCertMeta
              {
                  ФИО = x.FullName,
                  ДатаМестоРождения =
                      String.Format("{0},{1}",
                          ToShortString(x.Birthdate),
                          x.AddressBirth),
                  НомерАттестата = String.Format("{0},{1}", x.Number, x.BlankNumber),
                  ОснованиеВыдачи = x.RteId > 0 ?
                                      String.Format("Протокол №{0} от {1}", x.RteProtocolNum, ToShortString(x.RteProtocolDate)) : "",
                  ОснованиеВыдачиДубликата = x.HasDuplicate ?
                                               String.Format("Заявление №{0} от {1}", x.DuplicateNumber, ToShortString(x.DuplicateIssuedDate)) :
                                               "",
                  ОснованиеАннулирования = x.HasCancelled ?
                                               String.Format("Протокол №{0} от {1}", x.CancelNumber, ToShortString(x.CancelProtocolDate)) :
                                               "",
                  СведенияОбОтмене = x.HasRenewed ?
                                               String.Format("Судебный акт №{0} от {1}", x.CourtActNumber, ToShortString(x.CourtActDate)) :
                                               "",
              }).ToList();


            return allCerts.AsQueryable();
        }

        private String ToShortString(DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToShortDateString() : "";
        }

        public override string Name
        {
            get { return "QualificationCertificateData"; }
        }

        public override string Description
        {
            get { return "Реестр квалификационных аттестатов"; }
        }
    }
}
