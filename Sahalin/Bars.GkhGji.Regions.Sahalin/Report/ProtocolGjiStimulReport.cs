namespace Bars.GkhGji.Regions.Sahalin.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Nso.Entities;
    using Bars.GkhGji.Report;
    using Slepov.Russian.Morpher;

    public class ProtocolGjiStimulReport : GjiBaseStimulReport
    {
        public ProtocolGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.SahalinProtocol))
        {
        }

        #region Properties

        public override string Id
        {
            get { return "SahalinProtocolGji"; }
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

        protected long DocumentId;

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "SahalinProtocolGji",
                    Name = "Protocol",
                    Description = "Протокол",
                    Template = Properties.Resources.SahalinProtocol
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
            CodeTemplate = "SahalinProtocolGji";
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var protocolDomain = Container.ResolveDomain<NsoProtocol>();
            var protocolViolDomain = Container.ResolveDomain<ProtocolViolation>();
            var protocolArticleLawDomain = Container.ResolveDomain<ProtocolArticleLaw>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var protocolLongTextDomain = Container.ResolveDomain<ProtocolLongText>();

            try
            {
                var protocol = protocolDomain.FirstOrDefault(x => x.Id == DocumentId);

                if (protocol == null)
                {
                    return;
                }
                FillCommonFields(protocol);

                if (protocol.Contragent != null)
                {
                    var contragent = new
                    {
                        АдресФакт = protocol.Contragent.FactAddress,
                        ЮрАдрес = protocol.Contragent.JuridicalAddress,
                        Наименование = protocol.Contragent.Name,
                        ИНН = protocol.Contragent.Inn,
                        ОГРН = protocol.Contragent.Ogrn,
                        ДатаРегистрации = protocol.Contragent.DateRegistration.ToDateTime()
                    };

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "Контрагенты",
                        MetaType = nameof(Object),
                        Data = contragent
                    });
                }

                var actCheck = GetParentDocument(protocol, TypeDocumentGji.ActCheck);

                if (actCheck != null)
                {
                    var actCheckProxy = new
                    {
                        Номер = actCheck.DocumentNumber,
                        Дата = actCheck.DocumentDate.HasValue
                            ? actCheck.DocumentDate.Value.ToShortDateString()
                            : string.Empty,
                    };

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "АктПроверки",
                        MetaType = nameof(Object),
                        Data = actCheckProxy
                    });
                }

                var roInfos = protocolViolDomain.GetAll()
                    .Where(x => x.Document.Id == DocumentId)
                    .Select(x => new
                    {
                        Municipality =
                            x.InspectionViolation.RealityObject != null
                                ? x.InspectionViolation.RealityObject.Municipality.Name
                                : "",
                        PlaceName =
                            x.InspectionViolation.RealityObject != null
                                ? x.InspectionViolation.RealityObject.FiasAddress.PlaceName
                                : "",
                        RealityObject =
                            x.InspectionViolation.RealityObject != null
                                ? x.InspectionViolation.RealityObject.Address
                                : "",
                        Id = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0,
                    })
                    .OrderBy(x => x.Municipality)
                    .ThenBy(x => x.RealityObject)
                    .ToList()
                    .Select(x => new
                    {
                        Адрес = x.RealityObject,
                        МестоСоставления = x.PlaceName
                    }).ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "АдресДома",
                    MetaType = nameof(Object),
                    Data = roInfos
                });

                var articles = protocolArticleLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == DocumentId)
                    .Select(x => new
                    {
                        СтатьяЗакона = x.ArticleLaw.Name
                    })
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "СтатьяЗакона",
                    MetaType = nameof(Object),
                    Data = articles
                });

                var inspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => new
                    {
                        ФамилияИО = x.Inspector.ShortFio,
                        Должность = x.Inspector.Position
                    })
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Инспекторы",
                    MetaType = nameof(Object),
                    Data = inspectors
                });

                var protocolLongDesc = protocolLongTextDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == DocumentId);

                var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");
                var protocolProxy = new
                {
                    Дата = protocol.DocumentDate.HasValue
                        ? protocol.DocumentDate.Value.ToShortDateString()
                        : string.Empty,
                    ДатаВремяРасмДела = protocol.DateOfProceedings.HasValue
                        ? protocol.DateOfProceedings.Value.ToShortDateString()
                        : string.Empty,
                    ДатаВремяРасмДела_Час = protocol.HourOfProceedings,
                    ДатаВремяРасмДела_Мин = protocol.MinuteOfProceedings,
                    МестоРасмДела = protocol.ProceedingsPlace,
                    Установил =
                        protocolLongDesc != null && protocolLongDesc.Description != null
                            ? Encoding.UTF8.GetString(protocolLongDesc.Description)
                            : protocol.Description,
                    ФизЛицо = protocol.PhysicalPerson,
                    ФизЛицоРП = РодПадеж(protocol.PhysicalPerson, склонятель),
                    ФизЛицоДП = ДатПадеж(protocol.PhysicalPerson, склонятель),
                    АдресТелефон = protocol.PersonRegistrationAddress,
                    МестоРаботы = protocol.PersonJob,
                    Должность = protocol.PersonPosition,
                    ДолжностьРП = РодПадеж(protocol.PersonPosition, склонятель),
                    ДатаМестоРождения = protocol.PersonBirthDatePlace,
                    ДокументУдостовЛичность = protocol.PersonDoc
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Протокол",
                    MetaType = nameof(Object),
                    Data = protocolProxy
                });

                this.ReportParams["ТипИсполнителя"] = protocol.Executant != null ? protocol.Executant.Code : string.Empty;

            }
            finally
            {
                Container.Release(protocolDomain);
                Container.Release(protocolViolDomain);
                Container.Release(protocolArticleLawDomain);
                Container.Release(docInspectorDomain);
                Container.Release(protocolLongTextDomain);
            }
        }

        private string ДатПадеж(string word, Склонятель склонятель)
        {
            if (string.IsNullOrEmpty(word))
            {
                return string.Empty;
            }

            var analyzed = склонятель.Проанализировать(word);
            return analyzed.Дательный;
        }
        private string РодПадеж(string word, Склонятель склонятель)
        {
            if (string.IsNullOrEmpty(word))
            {
                return string.Empty;
            }

            var analyzed = склонятель.Проанализировать(word);
            return analyzed.Родительный;
        }
    }
}
