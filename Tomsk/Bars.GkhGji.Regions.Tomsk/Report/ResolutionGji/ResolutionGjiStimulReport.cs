namespace Bars.GkhGji.Regions.Tomsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Dict;
    using Bars.GkhGji.Regions.Tomsk.Enums;
    using Bars.GkhGji.Report;

    using Slepov.Russian.Morpher;
    using TypeDocumentGji = Bars.GkhGji.Enums.TypeDocumentGji;

    public class ResolutionGjiStimulReport : GjiBaseStimulReport
    {
        private long DocumentId { get; set; }

        private TomskResolution _resolution;

        public ResolutionGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.Resolution))
        {
        }
        
        public override StiExportFormat ExportFormat 
        {
            get { return StiExportFormat.Word2007; }
        }

        public override string Id
        {
            get { return "TomskResolutionGji"; }
        }

        public override string CodeForm
        {
            get { return "Resolution"; }
        }

        public override string Name
        {
            get { return "Постановление"; }
        }
        
        public override string Description
        {
            get { return "Постановление"; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            var resolutionDomain = Container.Resolve<IDomainService<TomskResolution>>();

            try
            {
                _resolution = resolutionDomain.GetAll().FirstOrDefault(x => x.Id == DocumentId);

                if (_resolution == null)
                {
                    throw new Exception("Неудалось получить постановление по Id " + DocumentId);
                }
            }
            finally
            {
                Container.Release(resolutionDomain);
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "ResolutionGJI",
                    Code = "TomskBlockGJI_Resolution",
                    Description = "Шаблон на все случаи жизни",
                    Template = Properties.Resources.Resolution
                }
            };
        }
        
        public override void PrepareReport(ReportParams reportParams)
        {
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            var actCheckTimeDomain = Container.Resolve<IDomainService<ActCheckTime>>();
            var childrenService = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var protocolArticleDomain = Container.Resolve<IDomainService<ProtocolArticleLaw>>();
            var protocolViolationDomain = Container.Resolve<IDomainService<ProtocolViolation>>();
            var contragentContactDomain = Container.Resolve<IDomainService<ContragentContact>>();
            var protocolDomain = Container.Resolve<IDomainService<Protocol>>();
            var protocolDefinitionDomain = Container.Resolve<IDomainService<TomskProtocolDefinition>>();
            var documentPhysInfoDomain = Container.Resolve<IDomainService<DocumentPhysInfo>>();
            var tomskArticleLawGjiDomain = Container.Resolve<IDomainService<TomskArticleLawGji>>();
            var adminCaseArticleLawDomain = Container.Resolve<IDomainService<AdministrativeCaseArticleLaw>>();
            var tomskResolutionDescriptionDomain = Container.Resolve<IDomainService<TomskResolutionDescription>>();
            var protocolDescriptionDomain = Container.Resolve<IDomainService<ProtocolDescription>>();
            var prescriptionViolDomain = Container.Resolve<IDomainService<PrescriptionViol>>();
            var violDescripDomain = Container.Resolve<IDomainService<TomskViolationGjiDescription>>();
            var violDomain = Container.Resolve<IDomainService<TomskViolationGji>>();
            var prescriptionDomain = Container.Resolve<IDomainService<Prescription>>();
            var disposalDomain = Container.Resolve<IDomainService<Disposal>>();

            try
            {
                FillCommonFields(_resolution);
                
                #region Поля из Постановления
                this.ReportParams["Номер"] = _resolution.DocumentNumber;

                this.ReportParams["Дата"] = _resolution.DocumentDate.HasValue
                                                          ? _resolution.DocumentDate.Value.ToShortDateString()
                                                          : string.Empty;

                this.ReportParams["ЕГРЮЛ"] = _resolution.DateWriteOut.HasValue
                                                          ? _resolution.DateWriteOut.Value.ToShortDateString()
                                                          : string.Empty;

                this.ReportParams["Ходатайство"] = _resolution.HasPetition.GetEnumMeta().Display;
                this.ReportParams["Присутствовали"] = _resolution.FioAttend;
                this.ReportParams["Основание"] = _resolution.Inspection.TypeBase == TypeBase.CitizenStatement ? "обращение" : "_______________";
                this.ReportParams["Контрагент"] = _resolution.Contragent != null ? _resolution.Contragent.ShortName : string.Empty;
                this.ReportParams["ФактАдресКонтрагента"] = _resolution.Contragent != null ? _resolution.Contragent.FactAddress : string.Empty;
                this.ReportParams["Вотношении"] = _resolution.Executant != null ? _resolution.Executant.Code : string.Empty;
                this.ReportParams["ВидСанкции"] = _resolution.Sanction != null ? _resolution.Sanction.Code : string.Empty;
                this.ReportParams["ОснованиеПрекращения"] = _resolution.TypeTerminationBasement.HasValue ?
                                                 _resolution.TypeTerminationBasement == TypeTerminationBasement.NotSet ? "0" :
                                                 _resolution.TypeTerminationBasement == TypeTerminationBasement.Absence ? "3" :
                                                 _resolution.TypeTerminationBasement == TypeTerminationBasement.AbsenceEvent ? "1" :
                                                 _resolution.TypeTerminationBasement == TypeTerminationBasement.Expiration ? "4" :
                                                 _resolution.TypeTerminationBasement == TypeTerminationBasement.Insignificance ? "2" : "0" : "0"; 

                var resolDescription = tomskResolutionDescriptionDomain.GetAll().FirstOrDefault(x => x.Resolution.Id == _resolution.Id);

                var pettition = string.Empty;
                var explanation = string.Empty;
                if (resolDescription != null)
                {
                    if(resolDescription.PetitionText != null)
                        pettition = Encoding.UTF8.GetString(resolDescription.PetitionText);

                    if(resolDescription.ExplanationText != null)
                        explanation = Encoding.UTF8.GetString(resolDescription.ExplanationText);
                    
                }

                this.ReportParams["ТекстХодатайства"] = !string.IsNullOrEmpty(pettition) ? pettition : _resolution.PetitionText;
                this.ReportParams["Пояснения"] = !string.IsNullOrEmpty(explanation) ? explanation : _resolution.ExplanationText;

                /* 
                 * Дебильное условие придумал НАИЛ 
                 * ПолФизЛица. Постановление-Реквизиты-Документ выдан- Пол физ. лица. Если физ лицо не активно то (3) если М то(1) если Ж то (2).
                 */

                var gender = 3.ToString(CultureInfo.InvariantCulture);

                if (_resolution.PhysicalPersonGender == TypeGender.Male)
                {
                    gender = 1.ToString(CultureInfo.InvariantCulture);
                }
                else if (_resolution.PhysicalPersonGender == TypeGender.Female)
                {
                    gender = 2.ToString(CultureInfo.InvariantCulture);
                }

                this.ReportParams["ПолФизЛица"] = gender;

                this.ReportParams["РеквизитыФизЛица"] = _resolution.PhysicalPersonInfo;

                if (_resolution.DocumentTime.HasValue)
                {
                    var hour = _resolution.DocumentTime.Value.Hour;
                    var min = _resolution.DocumentTime.Value.Minute;

                    this.ReportParams["ВремяСоставления"] = string.Format(
                        "{0}{1} час. {2}{3} мин.",
                        hour < 10 ? "0" : string.Empty, hour,
                        min < 10 ? "0" : string.Empty, min);
                }

                // Если указано Должностное лицо, то берем из него данные
                if (_resolution.Official != null)
                {
                    this.ReportParams["КодДлВынесшегоПостановление"] = _resolution.Official.Position;
                    this.ReportParams["ФИОДлВынесшегоПостановление"] = _resolution.Official.Fio;
                    this.ReportParams["КодРуководителяФИО_сИнициалами"] = _resolution.Official.ShortFio;
                }

                // Штрафы
                if (_resolution.PenaltyAmount.HasValue)
                {
                    this.ReportParams["Штраф"] = _resolution.PenaltyAmount.Value.RoundDecimal(2).ToString();
                }

                // По исполнителю получаем поле Кого и поле Кого(РП)
                if (_resolution.Executant != null)
                {
                    this.ReportParams["ТипИсполнителя"] = _resolution.Executant.Return(x => x.Code);

                    switch (_resolution.Executant.Code)
                    {
                        case "0":
                        case "2":
                        case "4":
                        case "8":
                        case "9":
                        case "18":
                            {
                                var contragentName = _resolution.Contragent != null ? _resolution.Contragent.ShortName : string.Empty;
                                this.ReportParams["Кого"] = contragentName;
                            }
                        
                            break;
                        case "1":
                        case "3":
                        case "5":
                        case "10":
                        case "19":
                            {
                                var result = _resolution.Contragent != null ? _resolution.Contragent.ShortName : string.Empty;

                                if (!string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(_resolution.PhysicalPerson))
                                {
                                    result += ", ";
                                }

                                result += _resolution.PhysicalPerson;

                                this.ReportParams["Кого"] = result;
                            }

                            break;
                        case "6":
                        case "7":
                            {
                                this.ReportParams["Кого"] = _resolution.PhysicalPerson;
                            }
                       
                            break;
                    }
                }

                // Если задан контрагент то берем оттуда поля
                if (_resolution.Contragent != null)
                {
                    this.ReportParams["УправОргРП"] = _resolution.Contragent.NameGenitive;
                    this.ReportParams["ОГРН"] = _resolution.Contragent.Ogrn;
                    this.ReportParams["ИНН"] = _resolution.Contragent.Inn;
                    this.ReportParams["КПП"] = _resolution.Contragent.Kpp;

                    if (_resolution.Contragent.FiasJuridicalAddress != null)
                    {
                        var subStr = _resolution.Contragent.FiasJuridicalAddress.AddressName.Split(',');

                        var newAddr = new StringBuilder();

                        foreach (var rec in subStr)
                        {
                            if (newAddr.Length > 0)
                            {
                                newAddr.Append(',');
                            }

                            if (rec.Contains("р-н."))
                            {
                                var mu = rec.Replace("р-н.", string.Empty) + " район";
                                newAddr.Append(mu);
                                continue;
                            }

                            newAddr.Append(rec);
                        }

                        this.ReportParams["ЮрАдресКонтрагента"] = newAddr.ToString();
                    }

                    // получаем текущего руководителя
                    var headContragent = contragentContactDomain.GetAll()
                        .Where(x => x.Contragent.Id == _resolution.Contragent.Id && x.DateStartWork.HasValue
                                     && (x.DateStartWork.Value <= DateTime.Now && (!x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)))
                        .FirstOrDefault(x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

                    if (headContragent != null)
                    {
                        this.ReportParams["ФИОРукОрг"] = string.Format(
                            "{0} {1}", headContragent.Position != null ? headContragent.Position.Name : string.Empty, headContragent.FullName);
                    }
                }
                #endregion

                #region Поля по Протоколу

                // Получаем родительский протокол и из него берем данные
                var protocolDocument = GetParentDocument(_resolution, TypeDocumentGji.Protocol) ??
                    protocolDomain.GetAll().FirstOrDefault(x => x.Stage.Parent.Id == _resolution.Stage.Parent.Id);
                  
                if (protocolDocument != null)
                {
                    var protocol = protocolDomain.GetAll().FirstOrDefault(x => x.Id == protocolDocument.Id);
                    this.ReportParams["НомерПротокола"] = protocol.Return(x => x.DocumentNumber);
                    this.ReportParams["ДатаПротокола"] = protocolDocument.DocumentDate.HasValue
                        ? protocolDocument.DocumentDate.Value.ToString("dd.MM.yyyy г.")
                        : null;

                    var physicalPersonAll = склонятель.Проанализировать(protocol.PhysicalPerson ?? string.Empty);

                    if (physicalPersonAll != null)
                    {
                        this.ReportParams["ФизЛицоРП"] = physicalPersonAll.Родительный;
                        this.ReportParams["ФизЛицоТП"] = physicalPersonAll.Творительный;
                        this.ReportParams["ФизЛицоДП"] = physicalPersonAll.Дательный;
                    }

                    this.ReportParams["ФизЛицо"] = protocol.Return(x => x.PhysicalPerson);
                    this.ReportParams["ДатаРассмотренияДела"] = protocol.DateOfProceedings.HasValue
                          ? protocol.DateOfProceedings.Value.ToShortDateString()
                          : string.Empty;

                    var physInfo = documentPhysInfoDomain.GetAll().FirstOrDefault(x => x.Document.Id == protocol.Id);

                    if (physInfo != null)
                    {
                        this.ReportParams["МестоРаботы"] = physInfo.Return(x => x.PhysJob);
                        this.ReportParams["Должность"] = physInfo.Return(x => x.PhysPosition);


                        var physInfoAll = склонятель.Проанализировать(physInfo.PhysPosition ?? string.Empty);
                        if (physInfoAll != null)
                        {
                            this.ReportParams["ДолжностьРП"] = physInfoAll.Родительный;
                            this.ReportParams["ДолжностьТП"] = physInfoAll.Творительный;
                            this.ReportParams["ДолжностьДП"] = physInfoAll.Дательный;
                        }

                        this.ReportParams["ДатаМестоРождения"] = physInfo.Return(x => x.PhysBirthdayAndPlace);
                        this.ReportParams["ДокументУдостовЛичность"] = physInfo.Return(x => x.PhysIdentityDoc);
                    }


                    var protocolDescription = protocolDescriptionDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == protocol.Id);

                    var protocolDesc = string.Empty;
                    var protocolDescSet = string.Empty;
                    if (protocolDescription != null)
                    {
                        protocolDesc = protocolDescription.Description != null ? Encoding.UTF8.GetString(protocolDescription.Description) : string.Empty;
                        protocolDescSet = protocolDescription.DescriptionSet != null
                                                      ? Encoding.UTF8.GetString(protocolDescription.DescriptionSet)
                                                      : string.Empty;
                    }

                    this.ReportParams["УстановилПодр"] = !string.IsNullOrEmpty(protocolDesc) ? protocolDesc : protocol.Description;
                    this.ReportParams["УстановилПост"] = !string.IsNullOrEmpty(protocolDescSet) ? protocolDescSet : string.Empty;


                    var articles = protocolArticleDomain.GetAll()
                        .Where(x => x.Protocol.Id == protocolDocument.Id)
                        .Select(x => new
                        {
                            x.ArticleLaw.Id,
                            x.ArticleLaw.Part,
                            x.ArticleLaw.Article,
                            x.Description,
                            ArtDescription = x.ArticleLaw.Description,
                            x.ArticleLaw.Code,
                            x.ArticleLaw.Name
                        })
                        .ToList();

                    if (articles.Any())
                    {
                        this.ReportParams["СтатьяЗакона"] = articles.Select(x => x.Name).FirstOrDefault();

                        var firstArticleId = articles.Select(x => x.Id).FirstOrDefault();
                        var tomskArticleLaw = tomskArticleLawGjiDomain.GetAll().FirstOrDefault(x => x.Id == firstArticleId);

                        if (tomskArticleLaw != null)
                        {
                            this.ReportParams["ШтрафДляЛицаЮР"] = tomskArticleLaw.JurPersonPenalty;
                            this.ReportParams["ШтрафДляЛицаФИЗ"] = tomskArticleLaw.PhysPersonPenalty;
                            this.ReportParams["ШтрафДляЛицаДЛ"] = tomskArticleLaw.OffPersonPenalty;
                        }

                        this.ReportParams["ЧастьРаздельно"] = articles.Distinct(x => x.Part).AggregateWithSeparator(x => x.Part, ",");
                        this.ReportParams["СтатьяРаздельно"] = articles.Distinct(x => x.Article).AggregateWithSeparator(x => x.Article, ",");

                        var codeList = articles.Select(x => x.Code).Distinct().ToList();
                        var descriptionsDict = articles.GroupBy(x => x.Code)
                            .ToDictionary(x => x.Key, x => x.Select(y => y.Description).AggregateWithSeparator(","));

                        var description = articles.AggregateWithSeparator(x => x.Description, ", ");

                        this.ReportParams["ОписСтатьи"] = description;




                        var state1 = "ст. 26, 29, 30 Жилищного кодекса РФ, п. 1.7.1.  Правил и норм эксплуатации жилищного фонда (утв. постановлением Госстроя РФ от 27 сентября 2003 г. № 170)";
                        var state3 = "ч. 1, ч. 2.3 ст. 161 Жилищного кодекса РФ";
                        var state4 = "п. 4 ст. 3 ЖК РФ; ст. 161 ЖК РФ";
                        var state5 = "ч. 1, ч. 2.3 ст. 161 ЖК РФ";
                        var state6 = "п. 8 ст. 12 Федерального закона от 23 ноября 2009 г. № 261-ФЗ «Об энергосбережении и о повышении энергетической эффективности и о внесении изменений в отдельные законодательные акты Российской Федерации»";
                        var state7 = "п.п. 7 и 8 ст. 12 Федерального закона от 23.11.2009 № 261-ФЗ «Об энергосбережении и о повышении энергетической эффективности и о внесении изменений в отдельные законодательные акты РФ»";

                        var articleStr = new List<string>();

                        if (codeList.Contains("1") || codeList.Contains("2"))
                        {
                            articleStr.Add(state1);
                        }
                        if (codeList.Contains("3"))
                        {
                            articleStr.Add(state3);
                        }
                        if (codeList.Contains("4"))
                        {
                            articleStr.Add(state4);
                        }
                        if (codeList.Contains("5"))
                        {
                            articleStr.Add(state5);
                        }
                        if (codeList.Contains("6"))
                        {
                            articleStr.Add(state6);
                        }
                        if (codeList.Contains("7"))
                        {
                            articleStr.Add(state7);
                        }

                        this.ReportParams["НарушенныеТребования"] = articleStr.AggregateWithSeparator("; ");

                        var fullStateDiscription = new List<string>();

                        if (codeList.Contains("1"))
                        {
                            fullStateDiscription.Add("нарушения правил пользования жилыми помещениями. " +
                                                     "В указанной квартире выполнена самовольное переустройство, " +
                                                     "а именно: указанные работы по переустройству организовал и выполнил___________________." +
                                                     "Разрешительные документы на переустройство указанного жилого помещения, предусмотренные ст. 26 ЖК РФ, отсутствуют.");
                        }
                        if (codeList.Contains("2"))
                        {
                            fullStateDiscription.Add("нарушения правил пользования жилыми помещениями. " +
                                                     "В указанной квартире выполнена самовольная перепланировка, " +
                                                     "а именно: указанные работы по перепланировке организовал и выполнил___________________. " +
                                                     "Разрешительные документы на перепланировку указанного жилого помещения, предусмотренные  ст. 26 ЖК РФ, отсутствуют.");
                        }
                        if (codeList.Contains("3"))
                        {
                            fullStateDiscription.Add("нарушения правил содержания и ремонта жилого дома, а именно: " + descriptionsDict.Get("3"));
                        }
                        if (codeList.Contains("4"))
                        {
                            fullStateDiscription.Add("нарушения режима обеспечения населения коммунальными услугами, а именно: " + descriptionsDict.Get("4"));
                        }
                        if (codeList.Contains("5"))
                        {
                            fullStateDiscription.Add("нарушения требований законодательства о раскрытии информации организациями, осуществляющими деятельность в сфере управления многоквартирными домами, а именно: " + descriptionsDict.Get("5"));
                        }
                        if (codeList.Contains("8"))
                        {
                            fullStateDiscription.Add("неповиновение законному требованию должностного лица органа, осуществляющего государственный надзор (контроль), а именно: " + descriptionsDict.Get("8"));
                        }
                        if (codeList.Contains("10"))
                        {
                            fullStateDiscription.Add("невыполнение в установленный срок законного предписания органа (должностного лица), осуществляющего государственный надзор (контроль), об устранении нарушений законодательства, а именно: " + descriptionsDict.Get("10"));
                        }
                        if (codeList.Contains("6") || codeList.Contains("7"))
                        {
                            var code6 = descriptionsDict.Get("6");
                            var code7 = descriptionsDict.Get("7");

                            fullStateDiscription.Add("нарушения законодательства об энергосбережении и о повышении энергетической эффективности, а именно: " 
                                + (code6.IsNotEmpty() && code7.IsNotEmpty() ? 
                                                string.Format("{0}, {1}", code6, code7) : 
                                                code6.IsNotEmpty() ? 
                                                        code6 : 
                                                        code7.IsNotEmpty() ? 
                                                            code7 : 
                                                            string.Empty));
                        }
                        if (codeList.Contains("11"))
                        {
                            fullStateDiscription.Add("непредставление или несвоевременное представление в государственный орган (должностному лицу) " +
                                                     "сведений (информации), представление которых предусмотрено законом и необходимо для осуществления " +
                                                     "этим органом (должностным лицом) его законной деятельности, а равно представление в государственный " +
                                                     "орган таких сведений в неполном объеме или в искаженном виде, а именно: " + descriptionsDict.Get("11"));
                        }
                        if (codeList.Contains("9"))
                        {
                            fullStateDiscription.Add("воспрепятствование законной деятельности должностного лица органа государственного контроля (надзора), повлекшие невозможность проведения или завершения проверки, а именно: " + descriptionsDict.Get("9"));
                        }

                        this.ReportParams["Установил"] = fullStateDiscription.Any() ? fullStateDiscription.AggregateWithSeparator(";") : string.Empty;

                    }
                   
                    var protocolRoAdr = protocolViolationDomain.GetAll()
                                            .Where(x => x.Document.Id == protocolDocument.Id)
                                            .Where(x => x.InspectionViolation != null && x.InspectionViolation.RealityObject != null)
                                            .Select(x => x.InspectionViolation.RealityObject.Address)
                                            .Distinct()
                                            .ToList();

                    if (protocolRoAdr.Any())
                    {
                        this.ReportParams["АдресОбъектаПравонарушения"] = protocolRoAdr.Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));
                    }

                    if (protocol.DateOfProceedings.HasValue)
                    {
                        var dateProcessing = protocol.DateOfProceedings.Value.ToShortDateString();

                        dateProcessing += " " + string.Format(
                                                    "{0}{1} час. {2}{3} мин.",
                                                    protocol.HourOfProceedings < 10 ? "0" : string.Empty, protocol.HourOfProceedings,
                                                    protocol.MinuteOfProceedings < 10 ? "0" : string.Empty, protocol.MinuteOfProceedings);

                        this.ReportParams["ДатаИВремяРассмотренияДела"] = dateProcessing;
                    }
                }

                #endregion

                #region Поля по Определению Протокола
                if (protocolDocument != null)
                {
                    var protocolDef = protocolDefinitionDomain.GetAll()
                                                .Where(x => x.Protocol.Id == protocolDocument.Id)
                                                .Where(
                                                    x => x.TypeDefinition == TypeDefinitionProtocol.TimeAndPlaceHearing)
                                                .WhereIf(
                                                    _resolution.DocumentDate.HasValue,
                                                    x =>
                                                    x.DocumentDate.HasValue
                                                    && x.DocumentDate.Value <= _resolution.DocumentDate.Value)
                                                .OrderByDescending(x => x.DocumentDate)
                                                .FirstOrDefault();

                    if (protocolDef != null)
                    {
                        this.ReportParams["ДатаОпределения"] = protocolDef.DocumentDate.HasValue
                                                        ? protocolDef.DocumentDate.Value.ToString("dd.MM.yyyy г.")
                                                        : string.Empty;

                        this.ReportParams["НомерОпределения"] = protocolDef.DocumentNum;
                        this.ReportParams["МестоРассмотрения"] = protocolDef.PlaceReview;
                    }
                }
                #endregion

                #region Поля по предписанию

                var parentStageId = _resolution.Stage.Parent != null ? _resolution.Stage.Parent.Id : 0;
                var prescription =
                    prescriptionDomain.GetAll()
                        .Where(x => x.Stage.Parent.Id == parentStageId)
                        .Where(x => x.TypeDocumentGji == TypeDocumentGji.Prescription)
                        .FirstOrDefault();

                if (prescription != null)
                {
                    var violQuery = prescriptionViolDomain.GetAll().Where(x => x.Document.Id == prescription.Id);

                    var descriptions = violDescripDomain.GetAll()
                        .Where(x => violQuery.Any(y => y.InspectionViolation.Violation.Id == x.ViolationGji.Id))
                        .Where(x => x.ViolationGji.RuleOfLaw != null)
                        .Select(x => new { Информация = x.ViolationGji.RuleOfLaw })
                        .ToList();

                    if (!descriptions.Any())
                    {
                        descriptions = violDomain.GetAll()
                            .Where(x => violQuery.Any(y => y.InspectionViolation.Violation.Id == x.Id))
                            .Select(x => new { Информация = x.RuleOfLaw })
                            .ToList();
                    }

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "НормыПрава",
                        MetaType = nameof(Object),
                        Data = descriptions
                    });
                }
                #endregion

                #region Поля по приказу

                var disposalDoc = GetParentDocument(_resolution, TypeDocumentGji.Disposal);
                Disposal disposal = null;
                if (disposalDoc != null)
                {
                    disposal = disposalDomain.GetAll().FirstOrDefault(x => x.Id == disposalDoc.Id);
                }
                else
                {
                    // если не получилось получить родительский приказ, т ополучаем главный приказ проверки
                    disposal =
                        disposalDomain.GetAll()
                                      .Where(x => x.Inspection.Id == _resolution.Inspection.Id)
                                      .OrderBy(x => x.Id)
                                      .FirstOrDefault();
                }

                if (disposal != null)
                {
                    this.ReportParams["ДатаПриказа"] = disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToString("dd.MM.yyyy г."): string.Empty;
                    this.ReportParams["НомерПриказа"] = disposal.DocumentNumber;
                    this.ReportParams["НомерДела"] = disposal.DocumentNumber;

                    if (disposal.IssuedDisposal != null)
                        this.ReportParams["КодРуководителяФИО"] = disposal.IssuedDisposal.Return(x => x.Code);
                }
                #endregion

                #region Поля по Акту проверки
                var act = GetParentDocument(_resolution, TypeDocumentGji.ActCheck);

                if (act != null)
                {
                    this.ReportParams["НомерАкта"] = act.DocumentNumber;
                    this.ReportParams["ДатаАкта"] = act.DocumentDate.HasValue ? act.DocumentDate.Value.ToString("dd.MM.yyyy г.") : string.Empty;
                }
                #endregion

                #region Поля по Админ делу
                var adminCase = GetParentDocument(_resolution, TypeDocumentGji.AdministrativeCase);

                if (adminCase != null)
                {
                    this.ReportParams["ТипПроверки"] = "административного расследования";
                    this.ReportParams["НомерДела"] = adminCase.DocumentNumber;

                    var article = adminCaseArticleLawDomain.GetAll()
                        .Where(x => x.AdministrativeCase.Id == adminCase.Id)
                        .Select(x => new
                        {
                            x.ArticleLaw.Id,
                            x.ArticleLaw.Code,
                            x.ArticleLaw.Name
                        })
                        .FirstOrDefault();

                    if (article != null)
                    {
                        this.ReportParams["СтатьяЗакона"] = article.Name;
                        var tomskArticleLaw = tomskArticleLawGjiDomain.GetAll().FirstOrDefault(x => x.Id == article.Id);

                        if (tomskArticleLaw != null)
                        {
                            this.ReportParams["ШтрафДляЛицаЮР"] = tomskArticleLaw.JurPersonPenalty;
                            this.ReportParams["ШтрафДляЛицаФИЗ"] = tomskArticleLaw.PhysPersonPenalty;
                            this.ReportParams["ШтрафДляЛицаДЛ"] = tomskArticleLaw.OffPersonPenalty;
                        }
                        
                    }
                }
                else
                {
                    this.ReportParams["ТипПроверки"] = "проверки";
                }
                
                #endregion
            }
            finally 
            {
                Container.Release(actCheckTimeDomain);
                Container.Release(childrenService);
                Container.Release(protocolArticleDomain);
                Container.Release(protocolViolationDomain);
                Container.Release(protocolDomain);
                Container.Release(protocolDefinitionDomain);
                Container.Release(documentPhysInfoDomain);
                Container.Release(prescriptionDomain);
                Container.Release(contragentContactDomain);
                Container.Release(tomskArticleLawGjiDomain);
                Container.Release(adminCaseArticleLawDomain);
                Container.Release(tomskResolutionDescriptionDomain);
                Container.Release(protocolDescriptionDomain);
                Container.Release(prescriptionViolDomain);
                Container.Release(violDescripDomain);
                Container.Release(violDomain);
                Container.Release(disposalDomain);
            }
        }
    }
}