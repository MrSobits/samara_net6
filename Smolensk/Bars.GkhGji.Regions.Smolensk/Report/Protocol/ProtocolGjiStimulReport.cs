using System.Text;
using Bars.Gkh.Overhaul.Entities;
using Bars.GkhGji.Regions.Smolensk.Entities;
using Bars.GkhGji.Regions.Smolensk.Entities.Protocol;

namespace Bars.GkhGji.Regions.Smolensk.Report.Protocol
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;

    using Gkh.Entities;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using GkhGji.Enums;

    public class ProtocolGjiStimulReport : GjiBaseStimulReport
    {
        public ProtocolGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.SmolenskProtocol))
        {
        }

        #region Properties

        public override string Id
        {
            get { return "SmolenskProtocolGji"; }
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

        protected long DocumentId;

        protected ProtocolSmol Protocol;

        protected Disposal disposal;

        #endregion Fields

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            //Поскольку во многих местах нужен родительский Disposal то берем его как поле объекта и получаем здесь

            var protocolDomain = Container.ResolveDomain<ProtocolSmol>();

            using (Container.Using(protocolDomain))
            {
                Protocol = protocolDomain.FirstOrDefault(x => x.Id == DocumentId);

                var dispParent = GetParentDocument(Protocol, TypeDocumentGji.Disposal);
                if (dispParent != null)
                {
                    disposal = dispParent as Disposal;
                }
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "SmolenskProtocolGji",
                    Name = "Protocol",
                    Description = "Протокол",
                    Template = Properties.Resources.SmolenskProtocol
                }
            };
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
            CodeTemplate = "SmolenskProtocolGji";
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (Protocol == null)
            {
                return;
            }

            FillCommonFields(Protocol);

            FillProtocolData();

            FillProtocolInspectors();

            FillArticlesLaw();

            FillAddress();

            FillParentActData();

            FillProtocolViolations();
        }

        private void FillParentActData()
        {
            var gjiChildDomain = Container.ResolveDomain<DocumentGjiChildren>();
            using (Container.Using(gjiChildDomain))
            {
                var parent = gjiChildDomain.GetAll().FirstOrDefault(x => x.Children.Id == DocumentId && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck);
                if (parent != null)
                {
                    this.ReportParams["ДатаАкта"] = parent.Parent.DocumentDate.HasValue
                        ? parent.Parent.DocumentDate.Value.ToString("dd.MM.yyyy г.")
                        : string.Empty;
                }
            }
        }

        protected void FillProtocolData()
        {

            var physPersonInfoDomain = Container.ResolveDomain<DocumentGJIPhysPersonInfo>();
            var protViolDescriptionDomain = Container.ResolveDomain<ProtocolViolationDescription>();
            try
            {
                this.ReportParams["Номер"] = Protocol.DocumentNumber;
                this.ReportParams["ДатаПротокола"] = Protocol.DocumentDate.HasValue
                    ? Protocol.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                    : null;

                // не используется
                this.ReportParams["ЗамечанияНарушителя"] = string.Empty;
                this.ReportParams["ХарактеристикаОбъекта"] = string.Empty;

                this.ReportParams["ФизическоеЛицо"] = Protocol.PhysicalPerson;

                var protViolDescription = protViolDescriptionDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == Protocol.Id);

                var violationDescription = string.Empty;
                var explanationsComments = string.Empty;

                if (protViolDescription != null)
                {
                    violationDescription = Encoding.UTF8.GetString(protViolDescription.ViolationDescription);
                    explanationsComments = Encoding.UTF8.GetString(protViolDescription.ExplanationsComments);
                }

                this.ReportParams["ОписаниеНарушения"] = violationDescription.IsEmpty() ? Protocol.ViolationDescription : violationDescription;
                this.ReportParams["Объяснения"] = explanationsComments.IsEmpty() ? Protocol.ExplanationsComments : explanationsComments;

                if (Protocol.Executant != null)
                {
                    var listTypeContragent = new List<string> { "0", "9", "11", "8", "15", "18", "4", "17" };
                    var listTypeContrPhysicalPerson = new List<string> { "1", "10", "12", "13", "16", "19", "5" };
                    var listTypePhysicalPerson = new List<string> { "6", "7", "14", "20", "2", "3" };

                    var contragent = Protocol.Return(x => x.Contragent);
                    var executant = Protocol.Return(x => x.Executant);

                    var physicalInfo = physPersonInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == Protocol.Id);
                    var ctrgContract = GetContragentContact(contragent.Return(x => x.Id));

                    if (listTypeContragent.Contains(executant.Return(x => x.Code)))
                    {
                        this.ReportParams["НаКогоСоставлен"] = contragent != null ? "{0} ({1})".FormatUsing(contragent.Name, contragent.ShortName) : string.Empty;

                        this.ReportParams["Ознакомлен"] = ctrgContract != null ? ctrgContract.Return(x => x.FullName) : string.Empty;

                        GetContragentBankData(this.ReportParams, contragent);
                    }
                    else if (listTypeContrPhysicalPerson.Contains(executant.Return(x => x.Code)))
                    {
                        this.ReportParams["НаКогоСоставлен"] = contragent != null ? "{0} ({1})".FormatUsing(contragent.Name, contragent.ShortName) : string.Empty + ", " + Protocol.PhysicalPerson;

                        this.ReportParams["АдресКонтрагента"] =
                            string.Format(@"юридический адрес - {0}\фактический адрес - {1}; {2}",
                                contragent.Return(x => x.JuridicalAddress),
                                contragent.Return(x => x.FactAddress),
                                Protocol.PhysicalPersonInfo);

                        this.ReportParams["Ознакомлен"] = "{0}; {1}".FormatUsing(ctrgContract != null ? ctrgContract.Return(x => x.FullName) : string.Empty, Protocol.PhysicalPerson);

                        GetContragentBankData(this.ReportParams, contragent);
                    }
                    else if (listTypePhysicalPerson.Contains(executant.Return(x => x.Code)))
                    {
                        this.ReportParams["НаКогоСоставлен"] = Protocol.PhysicalPerson;
                        this.ReportParams["АдресКонтрагента"] = Protocol.PhysicalPersonInfo;
                        this.ReportParams["Ознакомлен"] = Protocol.PhysicalPerson;
                    }

                    var listJurPersonsCode = new List<string>
                    {
                        "15",
                        "9",
                        "0",
                        "4",
                        "8",
                        "17",
                        "18",
                        "11",
                        "16",
                        "10",
                        "12",
                        "13",
                        "1",
                        "5",
                        "19"
                    };
                    var listPhysicalPersonCode = new List<string> { "6", "7", "14" };

                    if (listJurPersonsCode.Contains(executant.Return(x => x.Code)))
                    {
                        this.ReportParams["АдресКонтрагента"] =
                            string.Format(@"юридический адрес - ""{0}""\фактический адрес - ""{1}""",
                                contragent.Return(x => x.JuridicalAddress),
                                contragent.Return(x => x.FactAddress));
                    }
                    else if (physicalInfo != null && listPhysicalPersonCode.Contains(executant.Return(x => x.Code)))
                    {
                        this.ReportParams["АдресКонтрагента"] = physicalInfo.PhysPersonAddress;
                    }

                    var listExec1 = new List<string> { "6", "7", "14", "20", "2", "3" };
                    var listExec2 = new List<string> { "1", "10", "12", "13", "16", "19", "5" };
                    var listExec3 = new List<string> { "0", "9", "11", "8", "15", "18", "4", "17" };

                    if (physicalInfo != null)
                    {
                        this.ReportParams["АдресТелефон"] = physicalInfo.PhysPersonAddress;
                        this.ReportParams["МестоРаботы"] = physicalInfo.PhysPersonJob;
                        this.ReportParams["Должность"] = physicalInfo.PhysPersonPosition;
                        this.ReportParams["ДатаМестоРождения"] = physicalInfo.PhysPersonBirthdayAndPlace;

                        if (listExec1.Contains(executant.Return(x => x.Code)))
                        {
                            this.ReportParams["ИныеСведения"] = physicalInfo.PhysPersonDocument;
                        }
                        else if (listExec2.Contains(executant.Return(x => x.Code)))
                        {
                            this.ReportParams["ИныеСведения"] = physicalInfo.PhysPersonPosition;
                        }
                    }


                    if (listExec3.Contains(executant.Return(x => x.Code)))
                    {
                        this.ReportParams["ИныеСведения"] = ctrgContract != null ? "{0} {1} {2}".FormatUsing(ctrgContract.Position != null ? ctrgContract.Position.Name : string.Empty,
                            contragent != null ? contragent.Name : string.Empty, ctrgContract.FullName) : string.Empty;
                    }
                }
            }
            finally
            {
                Container.Release(physPersonInfoDomain);
                Container.Release(protViolDescriptionDomain);
            }
        }

        protected void FillProtocolInspectors()
        {
            var docinspDomain = Container.ResolveDomain<DocumentGjiInspector>();
            using (Container.Using(docinspDomain))
            {
                var inspectors = docinspDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => x.Inspector)
                    .ToArray();

                if (inspectors.Any())
                {
                    var firstInspector = inspectors.First();

                    this.ReportParams["ДолжностьИнспектора"] = firstInspector.Position;
                    this.ReportParams["ИнспекторФамИО"] = firstInspector.ShortFio;
                    this.ReportParams["ИнспекторФИО"] = firstInspector.Fio;
                }
            }
        }

        protected void FillArticlesLaw()
        {
            var articleDomain = Container.ResolveDomain<ProtocolArticleLaw>();

            using (Container.Using(articleDomain))
            {
                var articles = articleDomain.GetAll()
                    .Where(x => x.Protocol.Id == Protocol.Id)
                    .Select(x => new
                    {
                        x.ArticleLaw.Name,
                        x.ArticleLaw.Description
                    })
                    .ToArray()
                    .Select(x =>
                            {
                                if (!string.IsNullOrEmpty(x.Description))
                                {
                                    return string.Format("{0} '{1}'", x.Name, x.Description);
                                }
                                
                                return x.Name;
                            })
                    .ToArray();

                this.ReportParams["СтатьяЗакона"] = articles.AggregateWithSeparator(", ");
            }
        }

        protected void FillAddress()
        {
            var protviolDomain = Container.ResolveDomain<ProtocolViolation>();

            using (Container.Using(protviolDomain))
            {
                var protocolRo = protviolDomain.GetAll()
                    .Where(x => x.Document.Id == Protocol.Id)
                    .Select(x => new
                    {
                        x.InspectionViolation.RealityObject.Id,
                        x.InspectionViolation.RealityObject.Address,
                        x.InspectionViolation.RealityObject.FiasAddress.AddressName,
                        x.InspectionViolation.RealityObject.FiasAddress.PlaceName,
                        x.InspectionViolation.RealityObject.FiasAddress.StreetName,
                        x.InspectionViolation.RealityObject.FiasAddress.House,
                        x.InspectionViolation.RealityObject.FiasAddress.Housing,
                        Municipality = x.InspectionViolation.RealityObject.Municipality.Name
                    })
                    .AsEnumerable()
                    .Distinct(x => x.Id)
                    .ToArray();

                if (protocolRo.Any())
                {
                    this.ReportParams["АдресСовершенияПравонарушенияПолный"] = protocolRo.AggregateWithSeparator(x => x.AddressName, "; ");
                }
            }
        }

        protected void FillProtocolViolations()
        {
            var protocolViolDomain = Container.ResolveDomain<ProtocolViolation>();

            using (Container.Using(protocolViolDomain))
            {
                var violations = protocolViolDomain.GetAll()
                    .Where(x => x.Document.Id == Protocol.Id)
                    .Select(x => new { x.Id, x.InspectionViolation, x.InspectionViolation.Violation })
                    .Select(x => new
                    {
                        x.Id,
                        x.InspectionViolation.DateFactRemoval,
                        x.Violation.Name,
                        x.Violation.Description,
                        x.Violation.CodePin,
                        x.Violation.PpRf170,
                        x.Violation.PpRf25,
                        x.Violation.PpRf307,
                        x.Violation.PpRf491,
                        x.Violation.OtherNormativeDocs
                    });

                var i = 0;

                Func<string> getOrderNum = () => (++i).ToString();

                var viols = violations.Select(x => new { НомерПП = getOrderNum(), ТекстНарушения = x.Name, Описание = x.Description }).ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Нарушения",
                    MetaType = nameof(Object),
                    Data = viols
                });
            }
        }

        protected void GetContragentBankData(Dictionary<string, object> Report, Contragent contragent)
        {
            var contragentBankDomain = Container.ResolveDomain<ContragentBankCreditOrg>();

            using (Container.Using(contragentBankDomain))
            {
                var contragentBank = contragentBankDomain.GetAll()
                    .Where(x => x.Contragent.Id == contragent.Id)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Bik,
                        x.CorrAccount,
                        x.SettlementAccount,
                        CreditOrgName = x.CreditOrg.Name
                    })
                    .FirstOrDefault();

                if (contragentBank != null)
                {
                    Report["НаименованиеБанка"] = contragentBank.CreditOrgName;
                    Report["РасчетныйСчет"] = contragentBank.SettlementAccount;
                    Report["КорСчет"] = contragentBank.CorrAccount;
                    Report["БИК"] = contragentBank.Bik;
                }

                Report["ИНН"] = contragent.Inn;
                Report["КПП"] = contragent.Kpp;
            }
        }

        protected ContragentContact GetContragentContact(long contragentId)
        {
            var contrContactDomain = Container.Resolve<IDomainService<ContragentContact>>();
            using (Container.Using(contrContactDomain))
            {
                return contrContactDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == contragentId);
            }
        }
    }
}
