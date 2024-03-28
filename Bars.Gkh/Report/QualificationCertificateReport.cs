namespace Bars.Gkh.Report
{
    using Bars.B4;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using B4.Modules.Reports;
    using System.Linq;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Config;
    using Slepov.Russian.Morpher;
    using Bars.B4.Utils;

    class QualificationCertificateReport : GkhBaseStimulReport
    {
        public QualificationCertificateReport()
            : base(new ReportTemplateBinary(Properties.Resources.QualificationCertificate))
        {
        }

        public IDomainService<PersonQualificationCertificate> CertificateDomain { get; set; }

        public IGkhParams GkhParams { get; set; }

        private Склонятель _morpher;

        protected Склонятель GetMorpher()
        {
            return _morpher ?? (_morpher = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ=="));
        }


        public override Stream GetTemplate()
        {
            return new MemoryStream(Properties.Resources.QualificationCertificate);
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                    {
                        Code = "QualificationCertificate_1",
                        Name = "QualificationCertificate",
                        Description =
                            "Реестр квалификационных аттестатов",
                        Template = Properties.Resources.QualificationCertificate
                    }
            };
        }

        public override string CodeForm
        {
            get { return "QualificationCertificate"; }
        }

        public override string Name
        {
            get { return "Реестр квалификационных аттестатов"; }
        }

        public override string Description
        {
            get { return ""; }
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "QualificationCertificate"; }
        }

        private String ToShortString(DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToShortDateString() : "";
        }

        public override StiExportFormat ExportFormat => StiExportFormat.Excel2007;

        public override void PrepareReport(ReportParams reportParams)
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
                .Select(x => new
                {
                    ФИО = x.FullName,
                    ДатаМестоРождения =
                        String.Format("{0},{1}",
                            ToShortString(x.Birthdate),
                            x.AddressBirth),
                    НомерАттестата = String.Format("{0},{1}", x.Number, x.BlankNumber),
                    ОснованиеВыдачи = x.RteId > 0 ?
                                        String.Format("Протокол №{0} от {1}", x.RteProtocolNum, ToShortString(x.RteProtocolDate)) : "",
                    ОснованиеВыдачиДубликата =   x.HasDuplicate ? 
                                                 String.Format("Заявление №{0} от {1}", x.DuplicateNumber, ToShortString(x.DuplicateIssuedDate)) :
                                                 "",
                    ОснованиеАннулирования = x.HasCancelled ?
                                                 String.Format("Протокол №{0} от {1}", x.CancelNumber, ToShortString(x.CancelProtocolDate)) :
                                                 "",
                    СведенияОбОтмене = x.HasRenewed ?
                                                 String.Format("Судебный акт №{0} от {1}", x.CourtActNumber, ToShortString(x.CourtActDate)) :
                                                 "",
                }).ToList();


            this.DataSources.Add(new MetaData
            {
                SourceName = "КвалификационныеАттестаты",
                MetaType = nameof(Object),
                Data = allCerts
            });

            var prm = GkhParams.GetParams();
                
            var regionName = prm.ContainsKey("RegionName") ? prm["RegionName"].ToStr() : "";

            var name = GetMorpher().Проанализировать(regionName);

            this.ReportParams["НаименованиеРегиона"] = name != null ? name.Родительный : "";
        }
    }
}
