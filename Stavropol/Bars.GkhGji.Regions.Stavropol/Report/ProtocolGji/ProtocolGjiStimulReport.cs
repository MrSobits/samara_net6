namespace Bars.GkhGji.Regions.Stavropol.Report.ProtocolGji
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using B4;
    using B4.Modules.Reports;
    using Gkh.Entities;
    using Gkh.Report;
    using GkhGji.Report;
    using Bars.GkhGji.Regions.Stavropol.Properties;
    using Bars.GkhGji.Entities;
    using Bars.B4.Utils;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils;
    using System.Linq;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    public class ProtocolGjiStimulReport : GjiBaseStimulReport
    {
        public ProtocolGjiStimulReport()
            : base(new ReportTemplateBinary(Resources.BlockGJI_ExecutiveDocProtocol))
        {
        }

        #region Properties

        public override string Id
        {
            get { return "Protocol"; }
        }

        public override string CodeForm
        {
            get { return "Protocol"; }
        }

        public override string Name
        {
            get { return "Протокол"; }
        }

        public override string Description
        {
            get { return "Протокол"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        protected override string CodeTemplate { get; set; }

        #endregion Properties

        #region Fields

        private long _documentId;
        private Protocol _protocol;

        #endregion Fields

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _documentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
            
            var protocolDomain = Container.ResolveDomain<Protocol>();

            using (Container.Using(protocolDomain))
            {
                _protocol = protocolDomain.FirstOrDefault(x => x.Id == _documentId);
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "BlockGJI_ExecutiveDocProtocol",
                    Name = "Протокол",
                    Description = "Любой другой случай",
                    Template = Resources.BlockGJI_ExecutiveDocProtocol
                }
            };
        }

        public override Stream GetTemplate()
        {
            GetCodeTemplate();
            return base.GetTemplate();
        }
        
        private void GetCodeTemplate()
        {
            CodeTemplate = "BlockGJI_ExecutiveDocProtocol";
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (_protocol == null)
            {
                return;
            }

            FillCommonFields(_protocol);
            FillProtocolData();
            FillProtocolInspectors();
            FillProtocolContragent();
            FillContragentBank();
            FillContragentContact();
            FillArcticlesLaw();
            FillPrescriptionInfo();
            FillActCheckInfo();
        }

        protected void FillProtocolData()
        {
            this.ReportParams["Номер"] = _protocol.DocumentNumber;
            this.ReportParams["ДатаПротокола"] = _protocol.DocumentDate.HasValue
                ? _protocol.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                : null;
        }

        protected void FillProtocolInspectors()
        {
            var docinspDomain = Container.ResolveDomain<DocumentGjiInspector>();
            using (Container.Using(docinspDomain))
            {
                var inspectors = docinspDomain.GetAll()
                     .Where(x => x.DocumentGji.Id == _documentId)
                     .Select(x => x.Inspector)
                     .ToArray();

                if (inspectors.Any())
                {
                    var firstInspector = inspectors.First();

                    this.ReportParams["Инспектор"] = firstInspector.FioAblative;
                    this.ReportParams["ИнспекторФамИО"] = firstInspector.ShortFio;
                }
            }
        }

        protected void FillProtocolContragent()
        {
            var contragent = _protocol.Return(x => x.Contragent);
            if (contragent != null)
            {
                this.ReportParams["Контрагент"] = contragent.ShortName;
                this.ReportParams["ИНН"] = contragent.Inn;
                this.ReportParams["ОГРН"] = contragent.Ogrn;
                this.ReportParams["КонтрагентСокр"] = contragent.ShortName;
                this.ReportParams["КПП"] = contragent.Kpp;
                this.ReportParams["АдресФакт"] = contragent.FactAddress;
                this.ReportParams["АдресКонтрагента"] = contragent.FiasJuridicalAddress != null
                    ? contragent.FiasJuridicalAddress.AddressName
                    : contragent.AddressOutsideSubject;
            }
        }

        protected void FillContragentBank()
        {
            if (_protocol.Contragent == null)
            {
                return;
            }

            var contragentBankDomain = Container.ResolveDomain<ContragentBank>();

            using (Container.Using(contragentBankDomain))
            {
                var contragentBank = contragentBankDomain.GetAll()
                    .Where(x => x.Contragent.Id == _protocol.Contragent.Id)
                    .Select(x => new
                    {
                        x.Bik,
                        x.CorrAccount,
                        x.SettlementAccount
                    })
                    .FirstOrDefault();

                if (contragentBank != null)
                {
                    this.ReportParams["КСчёт"] = contragentBank.CorrAccount;
                    this.ReportParams["РСчет"] = contragentBank.SettlementAccount;
                    this.ReportParams["БИК"] = contragentBank.Bik;
                }
            }
        }

        protected void FillContragentContact()
        {
            if (_protocol.Contragent == null)
            {
                return;
            }

            var contragentContactDomain = Container.ResolveDomain<ContragentContact>();

            using (Container.Using(contragentContactDomain))
            {
                var positionCodes = new[] {"1", "4"};
                var contact =
                    contragentContactDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == _protocol.Contragent.Id &&
                                                                         positionCodes.Contains(x.Position.Code));
                if (contact != null)
                {
                    this.ReportParams["РукУпрОрг"] = string.Format("{0} {1}", contact.Position.Name, contact.FullName);
                    this.ReportParams["ТлфРуководителяУО"] = contact.Phone;
                }
            }
        }

        protected void FillArcticlesLaw()
        {
            var articleDomain = Container.ResolveDomain<ProtocolArticleLaw>();
            using (Container.Using(articleDomain))
            {
                var articles = articleDomain.GetAll()
                    .Where(x => x.Protocol.Id == _protocol.Id)
                    .AggregateWithSeparator(x => x.ArticleLaw.Name, ", ");

                this.ReportParams["СтатьяЗакона"] = articles;
            }
        }

        protected void FillPrescriptionInfo()
        {
            var prescriptionDomain = Container.ResolveDomain<Prescription>();
            using (Container.Using(prescriptionDomain))
            {
                var prescription = prescriptionDomain.GetAll().FirstOrDefault(x => x.Inspection.Id == _protocol.Inspection.Id);
                if (prescription != null)
                {
                    this.ReportParams["ДатаПредписания"] = prescription.DocumentDate.HasValue
                        ? prescription.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                        : null;
                    this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;
                }
            }
        }

        protected void FillActCheckInfo()
        {
            var actCheckDomain = Container.ResolveDomain<ActCheck>();
            var actCheckPeriodDomain = Container.ResolveDomain<ActCheckPeriod>();
            var actCheckRealityObjectDomain = Container.Resolve<IDomainService<ActCheckRealityObject>>();

            using (Container.Using(actCheckDomain))
            {
                var actCheck = actCheckDomain.GetAll().FirstOrDefault(x => x.Inspection.Id == _protocol.Inspection.Id);

                if (actCheck != null)
                {
                    using (Container.Using(actCheckRealityObjectDomain))
                    {
                        var realityObjects = actCheckRealityObjectDomain.GetAll().Where(x => x.ActCheck.Id == actCheck.Id);
                        if (realityObjects.Count() == 1)
                        {
                            this.ReportParams["Описание"] = realityObjects.First().Description;
                        }
                    }

                    using (Container.Using(actCheckPeriodDomain))
                    {
                        var actCheckPeriod = actCheckPeriodDomain.GetAll()
                            .OrderByDescending(x => x.DateCheck)
                            .FirstOrDefault(x => x.ActCheck.Id == actCheck.Id);
                        
                        if (actCheckPeriod != null)
                        {
                            this.ReportParams["ДатаВремПровПроверки"] =
                                actCheckPeriod.DateCheck.HasValue
                                    ? actCheckPeriod.DateCheck.Value.ToString("g", new CultureInfo("ru-RU"))
                                    : null;
                        }
                    }
                }
            }
        }
    }
}