namespace Bars.GkhGji.Regions.Stavropol.Report.ResolutionGji
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Stavropol.Properties;
    using Bars.GkhGji.Report;

    public class ResolutionGjiDefinitionStimulReport : GjiBaseStimulReport
    {
        public ResolutionGjiDefinitionStimulReport()
            : base(new ReportTemplateBinary(Resources.Stavrapol_Resolution_Definition_1))
        {
        }

        #region Fields

        private long _definitionId;
        private Resolution _resolution;
        private ResolutionDefinition _definition;

        private const string StavrapolResolutionDefinition1 = "Stavrapol_Resolution_Definition_1";
        private const string StavrapolResolutionDefinition2 = "Stavrapol_Resolution_Definition_2";
        private const string StavrapolResolutionDefinition3 = "Stavrapol_Resolution_Definition_3";
        private const string StavrapolResolutionDefinition4 = "Stavrapol_Resolution_Definition_4";
        private const string StavrapolResolutionDefinition5 = "Stavrapol_Resolution_Definition_5";

        #endregion Fields

        #region Properties

        public override string Id
        {
            get { return "ResolutionDefinition"; }
        }

        public override string CodeForm
        {
            get { return "ResolutionDefinition"; }
        }

        public override string Name
        {
            get { return "Определение постановления"; }
        }

        public override string Description
        {
            get { return "Определение постановления"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        protected override string CodeTemplate { get; set; }

        #endregion Properties

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _definitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();

            var resolutionDefinitionDomain = Container.ResolveDomain<ResolutionDefinition>();
            using (Container.Using(resolutionDefinitionDomain))
            {
                _definition = resolutionDefinitionDomain.GetAll().FirstOrDefault(x => x.Id == _definitionId);
            }

            if (_definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            _resolution = _definition.Resolution;
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = StavrapolResolutionDefinition1,
                    Code = StavrapolResolutionDefinition1,
                    Description = "Определение о продлении срока рассмотрения дела об административном правонарушении",
                    Template = Resources.Stavrapol_Resolution_Definition_1
                },
                new TemplateInfo
                {
                    Name = StavrapolResolutionDefinition2,
                    Code = StavrapolResolutionDefinition2,
                    Description = "Определение о рассрочке исполнения постановления",
                    Template = Resources.Stavrapol_Resolution_Definition_2
                },
                new TemplateInfo
                {
                    Name = StavrapolResolutionDefinition3,
                    Code = StavrapolResolutionDefinition3,
                    Description =
                        "Определение об отсрочке исполнения постановления о назначении административного наказания",
                    Template = Resources.Stavrapol_Resolution_Definition_3
                },
                new TemplateInfo
                {
                    Name = StavrapolResolutionDefinition4,
                    Code = StavrapolResolutionDefinition4,
                    Description =
                        "Определение о назначении места и времени рассмотрения дела об административном правонарушении",
                    Template = Resources.Stavrapol_Resolution_Definition_4
                },
                
                new TemplateInfo
                {
                    Name = StavrapolResolutionDefinition5,
                    Code = StavrapolResolutionDefinition5,
                    Description =
                        "Определение об отложении рассмотрения дела об административном правонарушении",
                    Template = Resources.Stavrapol_Resolution_Definition_5
                }
            };
        }

        public override Stream GetTemplate()
        {
            switch (_definition.TypeDefinition)
            {
                case TypeDefinitionResolution.ProlongationReview:
                    CodeTemplate = StavrapolResolutionDefinition1;
                    break;
                case TypeDefinitionResolution.Installment:
                    CodeTemplate = StavrapolResolutionDefinition2;
                    break;
                case TypeDefinitionResolution.Deferment:
                    CodeTemplate = StavrapolResolutionDefinition3;
                    break;
                case TypeDefinitionResolution.AppointmentPlaceTime:
                    CodeTemplate = StavrapolResolutionDefinition4;
                    break;
                case TypeDefinitionResolution.SuspenseReviewAppeal:
                    CodeTemplate = StavrapolResolutionDefinition5;
                    break;
            }

            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            FillCommonInfo();
            switch (_definition.TypeDefinition)
            {
                case TypeDefinitionResolution.ProlongationReview:
                    FillProlongatinoInfo();
                    break;
                case TypeDefinitionResolution.Installment:
                    FillInstallmentInfo();
                    break;
                case TypeDefinitionResolution.Deferment:
                    FillDefermentInfo();
                    break;
                case TypeDefinitionResolution.AppointmentPlaceTime:
                    FillAppointmentPlaceTimeInfo();
                    break;
                case TypeDefinitionResolution.SuspenseReviewAppeal:
                    FillSuspenseReviewAppealInfo();
                    break;
            }
        }

        private void FillCommonInfo()
        {
            this.ReportParams["НомерОпределения"] = _definition.DocumentNumber != null
                ? _definition.DocumentNumber.ToString()
                : _definition.DocumentNum;
            this.ReportParams["ДатаОпределения"] = _definition.DocumentDate.HasValue
                ? _definition.DocumentDate.Value.ToShortDateString()
                : string.Empty;

            if (_definition.IssuedDefinition != null)
            {
                this.ReportParams["Руководитель"] = _definition.IssuedDefinition.Fio;
                this.ReportParams["РуководительФИО"] = _definition.IssuedDefinition.ShortFio;
                this.ReportParams["ДолжностьРуководителя"] = _definition.IssuedDefinition.Position;
            }

            this.ReportParams["ДатаПостановления"] = _resolution.DocumentDate.HasValue
               ? _resolution.DocumentDate.Value.ToShortDateString()
               : string.Empty;

            this.ReportParams["НомерПостановления"] = _resolution.DocumentNumber;

            if (_resolution.Contragent != null)
            {
                this.ReportParams["Контрагент"] = _resolution.Contragent.Name;
                this.ReportParams["КонтрагентСокр"] = _resolution.Contragent.ShortName;
                this.ReportParams["АдресКонтрагента"] = _resolution.Contragent.FiasJuridicalAddress != null
                    ? _resolution.Contragent.FiasJuridicalAddress.AddressName
                    : _resolution.Contragent.AddressOutsideSubject;

                var contragentContactDomain = Container.ResolveDomain<ContragentContact>();
                using (Container.Using(contragentContactDomain))
                {
                    var positionCodes = new[] { "1", "4" };
                    var contact =
                        contragentContactDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == _resolution.Contragent.Id &&
                                                                             positionCodes.Contains(x.Position.Code));
                    if (contact != null)
                    {
                        this.ReportParams["ФИОРукОрг"] = string.Format("{0} {1}", contact.Position.Name, contact.FullName);
                    }
                }
            }
        }

        private void FillProlongatinoInfo()
        {
            this.ReportParams["Правонарушитель"] = _resolution.Contragent != null
                ? _resolution.Contragent.Name
                : _resolution.PhysicalPerson;

            var dispId = GetParentDocument(_resolution, TypeDocumentGji.Disposal);
            if (dispId != null)
            {
                var disposalDomain = Container.ResolveDomain<Disposal>();
                using (Container.Using(disposalDomain))
                {
                    var disposal = disposalDomain.Load(dispId.Id);
                    if (disposal != null)
                    {
                        var documentGjiDomain = Container.ResolveDomain<DocumentGjiInspector>();
                        using (Container.Using(documentGjiDomain))
                        {
                            var queryInspectorIds = documentGjiDomain.GetAll()
                                .Where(x => x.DocumentGji.Id == disposal.Id)
                                .Select(x => x.Inspector.Id);

                            var zonalInspectionDomain = Container.ResolveDomain<ZonalInspectionInspector>();
                            using (Container.Using(zonalInspectionDomain))
                            {
                                var listLocality = zonalInspectionDomain.GetAll()
                                    .Where(x => queryInspectorIds.Contains(x.Inspector.Id))
                                    .Select(x => x.ZonalInspection.Locality)
                                    .Distinct()
                                    .ToList();

                                this.ReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);
                            }
                        }
                    }
                }
            }

            this.ReportParams["ДатаИВремяРассмотренияДела"] = _definition.ExecutionDate.HasValue
                ? _definition.ExecutionDate.Value.ToString("g", new CultureInfo("ru-RU"))
                : null;

            var actDoc = GetParentDocument(_resolution, TypeDocumentGji.ActCheck);
            if (actDoc != null)
            {
                this.ReportParams["ДатаАкта"] = actDoc.DocumentDate != null
                    ? actDoc.DocumentDate.Value.ToShortDateString()
                    : string.Empty;

                this.ReportParams["НомерАкта"] = actDoc.DocumentNumber;
            }

            var protocol = GetParentDocument(_resolution, TypeDocumentGji.Protocol);
            if (protocol != null)
            {
                this.ReportParams["ДатаПротокола"] = protocol.DocumentDate != null
                    ? protocol.DocumentDate.Value.ToShortDateString()
                    : string.Empty;

                this.ReportParams["НомерПротокола"] = protocol.DocumentNumber;

                var protocolViolationDomain = Container.ResolveDomain<ProtocolViolation>();
                using (Container.Using(protocolViolationDomain))
                {
                    var realitiObjects = protocolViolationDomain.GetAll()
                        .Where(x => x.Document.Id == protocol.Id)
                        .Select(x => new
                        {
                            Municipality = x.InspectionViolation.RealityObject != null
                                ? x.InspectionViolation.RealityObject.Municipality.Name
                                : string.Empty,
                            RealityObject = x.InspectionViolation.RealityObject != null
                                ? x.InspectionViolation.RealityObject.Address
                                : string.Empty,
                            RealityObjectId = x.InspectionViolation.RealityObject != null
                                ? x.InspectionViolation.RealityObject.Id
                                : 0,
                        }).ToList()
                        .GroupBy(x => new { x.RealityObjectId, x.Municipality, x.RealityObject })
                        .Select(x => new
                        {
                            x.Key.RealityObject
                        })
                        .AggregateWithSeparator(x => x.RealityObject, ", ");

                    this.ReportParams["АдресНарушения"] = realitiObjects;
                }

                var docinspDomain = Container.ResolveDomain<DocumentGjiInspector>();
                using (Container.Using(docinspDomain))
                {
                    var inspectors = docinspDomain.GetAll()
                         .Where(x => x.DocumentGji.Id == protocol.Id)
                         .Select(x => x.Inspector)
                         .ToArray();

                    if (inspectors.Any())
                    {
                        var firstInspector = inspectors.First();
                        this.ReportParams["Инспектор"] = firstInspector.FioAblative;
                    }
                }
            }
        }

        private void FillInstallmentInfo()
        {
            if (_resolution.Contragent != null)
            {
                this.ReportParams["ИНН"] = _resolution.Contragent.Inn;
            }

            this.ReportParams["Штраф"] = _resolution.PenaltyAmount.ToString();
            this.ReportParams["Описание"] = _definition.Description;
        }

        private void FillDefermentInfo()
        {
            if (_resolution.Contragent != null)
            {
                this.ReportParams["ИНН"] = _resolution.Contragent.Inn;
            }

            this.ReportParams["Штраф"] = _resolution.PenaltyAmount.ToString();
            this.ReportParams["ДатаИсполн"] = _definition.ExecutionDate.HasValue
                ? _definition.ExecutionDate.Value.ToShortDateString()
                : string.Empty;
        }

        private void FillSuspenseReviewAppealInfo()
        {
            // статьи в области охраны собственности
            var protectionActricles = new[]
            {
                "ст.7.21 ч.1 КоАП РФ",
                "ст.7.21 ч.2 КоАП РФ",
                "ст.7.22 КоАП РФ",
                "ст.7.23 КоАП РФ",
                "ст. 7.23.1 КоАП РФ"
            };

            // статьи в промышленности, строительстве и энергетике
            var industryActicles = new[]
            {
                "ст.9.16 ч.4 КоАП РФ",
                "ст. 9.16 ч.5 КоАП РФ"
            };

            // статьи против порядка управления
            var againstOrderArticles = new[]
            {
                "ст.19.4 ч.1 КоАП РФ",
                "ст.19.5 ч.1 КоАП РФ",
                "ст.19.6 КоАП РФ",
                "ст.19.7 КоАП РФ"
            };

            // статьи, посягающее на общественный порядок и общественную безопасность
            var againstPublicOrderArticles = new[]
            {
                "ст. 20.25 ч.1 КоАП РФ"
            };

            var articles = GetArticleLaws();
            if (articles.Any(p => protectionActricles.Any(n => p == n)))
            {
                this.ReportParams["СтатьяЗакона"] = "в области охраны собственности";
            }
            else if (articles.Any(p => industryActicles.Any(n => p == n)))
            {
                this.ReportParams["СтатьяЗакона"] = "в промышленности, строительстве и энергетике";
            }
            else if (articles.Any(p => againstOrderArticles.Any(n => p == n)))
            {
                this.ReportParams["СтатьяЗакона"] = "против порядка управления";
            }
            else if (articles.Any(p => againstPublicOrderArticles.Any(n => p == n)))
            {
                this.ReportParams["СтатьяЗакона"] = "посягающее на общественный порядок и общественную безопасность";
            }

            if (_resolution.Contragent != null && _resolution.Executant != null)
            {
                string contragent = null;
                string contragentAddress = null;
                string withRespect = null; // кода "тип исполнителя" 0,9,11,8,15,18,4,17,6,7,14,20,2,3,1,10,12,13,16,19,5

                // Коды исполнителя для поля Контрагент(юр. лицо)
                var typesForJurPerson = new[] { "0", "9", "11", "8", "15", "18", "4", "17" };

                // Коды исполнителя для поля Контрагент(должностное лицо)
                var typesForPerson = new[] { "1", "10", "12", "13", "16", "19", "5" };

                // Коды исполнителя для поля Физическое лицо (должностное лицо)
                var typesForPhysPerson = new[] { "6", "7", "14", "20", "2", "3" };

                if (typesForJurPerson.Contains(_resolution.Executant.Code))
                {
                    contragent = _resolution.Contragent.NameGenitive;
                    contragentAddress = _resolution.Contragent.FiasJuridicalAddress != null
                        ? _resolution.Contragent.FiasJuridicalAddress.AddressName
                        : _resolution.Contragent.JuridicalAddress;
                    withRespect = _resolution.Executant.Code;
                }
                else if (typesForPerson.Contains(_resolution.Executant.Code))
                {
                    contragent = _resolution.PhysicalPerson;
                    contragentAddress = _resolution.PhysicalPersonInfo;
                    withRespect = _resolution.Executant.Code;
                }
                else if (typesForPhysPerson.Contains(_resolution.Executant.Code))
                {
                    contragent = _resolution.PhysicalPerson;
                    contragentAddress = _resolution.PhysicalPersonInfo;
                    withRespect = _resolution.Executant.Code;
                }

                if (contragent != null)
                {
                    this.ReportParams["Контрагент"] = contragent;
                }
                if (contragentAddress != null)
                {
                    this.ReportParams["АдресКонтрагента"] = contragentAddress;
                }
                if (withRespect != null)
                {
                    this.ReportParams["Вотношении"] = withRespect;
                }
            }
            var docinspDomain = Container.ResolveDomain<DocumentGjiInspector>();
            using (Container.Using(docinspDomain))
            {
                var inspectors = docinspDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == _resolution.Id)
                    .Select(x => x.Inspector)
                    .ToArray();

                if (inspectors.Any())
                {
                    var firstInspector = inspectors.First();
                    this.ReportParams["РуководительФИО"] = firstInspector.ShortFio;
                }
            }

            if (_definition.ExecutionDate.HasValue)
            {
                var executionDate = _definition.ExecutionDate.Value;
                this.ReportParams["ДатаИВремяРассмотренияДела"] = string.Format("{0:dd MMMM yyyy} в {0:HH} час. {0:mm} мин.",
                    executionDate);
            }
        }

        private void FillAppointmentPlaceTimeInfo()
        {
            // переменные точно такие же
            FillSuspenseReviewAppealInfo();
        }

        private IList<string> GetArticleLaws()
        {
            var articleDomain = Container.ResolveDomain<ResolProsArticleLaw>();
            using (Container.Using(articleDomain))
            {
                var articles = articleDomain.GetAll()
                    .Where(x => x.ResolPros.Id == _resolution.Id)
                    .Select(x => x.ArticleLaw.Name)
                    .ToList();

                return articles;
            }
        }
    }
}