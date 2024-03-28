namespace Bars.GkhGji.Regions.Smolensk.Report.ProtocolGji
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Entities.Protocol;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using B4;
    using System.IO;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary> Уведомление о рассмотрении протокола </summary>
    public class NoticeOfProtocolStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        public NoticeOfProtocolStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.NoticeOfSmolenskProtocol))
        {

        }

        #endregion .ctor

        #region Properties

        public override string Id
        {
            get { return "ProtocolNotification"; }
        }

        public override string Name
        {
            get { return "Уведомление о рассмотрении протокола"; }
        }

        public override string Description
        {
            get { return "Уведомление о рассмотрении протокола"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string CodeForm
        {
            get { return "Protocol"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        #endregion Properties

        protected long DocumentId;

        protected ProtocolSmol Protocol;

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            var protocolDomain = Container.ResolveDomain<ProtocolSmol>();

            using (Container.Using(protocolDomain))
            {
                Protocol = protocolDomain.FirstOrDefault(x => x.Id == DocumentId);
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "NoticeOfSmolenskProtocol",
                    Name = "NoticeOfSmolenskProtocol",
                    Description = "Уведомление о рассмотрении протокола",
                    Template = Properties.Resources.NoticeOfSmolenskProtocol
                }
            };
        }

        private void GetCodeTemplate()
        {
            CodeTemplate = "NoticeOfSmolenskProtocol";
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
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
        }
        protected void FillProtocolData()
        {
            this.ReportParams["НомерПротокола"] = Protocol.DocumentNumber;
            this.ReportParams["ДатаУведомления"] = Protocol.NoticeDocDate.HasValue ? Protocol.NoticeDocDate.Value.ToShortDateString() : string.Empty;
            this.ReportParams["НомерУведомления"] = Protocol.NoticeDocNumber;

            if (Protocol.Executant != null)
            {
                var firstExecutantCodeList = new List<string> { "0", "9", "11", "8", "15", "18", "4" };
                var secondExecutantCodeList = new List<string> { "1", "5", "6", "7", "10", "12", "13", "14", "16", "19" };

                var contragent = Protocol.Return(x => x.Contragent);
                var executant = Protocol.Return(x => x.Executant);

                if (firstExecutantCodeList.Contains(executant.Return(x => x.Code)))
                {
                    this.ReportParams["УправОрг"] = Protocol.Contragent.Name;
                    this.ReportParams["АдресКонтрагентаФакт"] = contragent.Return(x => x.FactAddress);
                }

                else if (secondExecutantCodeList.Contains(executant.Return(x => x.Code)))
                {
                    this.ReportParams["УправОрг"] = Protocol.PhysicalPerson;
                    this.ReportParams["АдресКонтрагентаФакт"] = Protocol.PhysicalPersonInfo;
                }
            }

            this.ReportParams["НаселенныйПункт"] =
                Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                    .Where(x => x.Document.Id == Protocol.Id && x.InspectionViolation.RealityObject.FiasAddress != null)
                    .Select(x => x.InspectionViolation.RealityObject.FiasAddress.PlaceName)
                    .FirstOrDefault();
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

                    this.ReportParams["КодИнспектора"] = firstInspector.Position;
                    this.ReportParams["ИнспекторФамИО"] = firstInspector.ShortFio;
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
                        x.ArticleLaw.Code,
                        x.ArticleLaw.Name,
                        x.ArticleLaw.Description,
                        x.ArticleLaw.Part,
                        x.ArticleLaw.Article
                    })
                    .ToArray();

                this.ReportParams["СтатьяЗакона"] = articles.AggregateWithSeparator(x => x.Name, ", ");
            }
        }
    }
}
