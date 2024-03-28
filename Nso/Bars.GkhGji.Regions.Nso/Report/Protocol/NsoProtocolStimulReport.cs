namespace Bars.GkhGji.Regions.Nso.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;

    using DomainService;
    using Entities;
    using Gkh.Entities.Dicts;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.DomainService;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using GkhGji.Report;
    using Properties;
    using Slepov.Russian.Morpher;
    using Bars.GkhGji.Regions.Nso.DomainService.Impl;

	/// <summary>
	/// Отчет для протокола
	/// </summary>
	public class NsoProtocolStimulReport : GjiBaseStimulReport
    {
		/// <summary>
		/// Конструктор
		/// </summary>
        public NsoProtocolStimulReport()
            : base(new ReportTemplateBinary(Resources.NsoProtocol))
        {
        }

        #region Properties

		/// <summary>
		/// Идентификатор отчета
		/// </summary>
        public override string Id
        {
            get { return "NsoProtocolGji"; }
        }

		/// <summary>
		/// Код формы
		/// </summary>
        public override string CodeForm
        {
            get { return "Protocol"; }
        }

		/// <summary>
		/// Наименование
		/// </summary>
        public override string Name
        {
            get { return "Протокол"; }
        }

		/// <summary>
		/// Описание
		/// </summary>
        public override string Description
        {
            get { return "Протокол"; }
        }

		/// <summary>
		/// Формат экспорта
		/// </summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }


		/// <summary>
		/// код шаблона
		/// </summary>
		protected override string CodeTemplate { get; set; }

        #endregion Properties

        protected long DocumentId;

		/// <summary>
		/// Установить пользовательские параметры
		/// </summary>
		/// <param name="userParamsValues">Значения пользовательских параметров</param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

        }

		/// <summary>
		/// Получить информацию о шаблоне
		/// </summary>
		/// <returns>Информация о шаблоне</returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "NsoProtocolGji",
                    Name = "Protocol",
                    Description = "Протокол",
                    Template = Resources.NsoProtocol
                }
            };
        }

		/// <summary>
		/// Получить шаблон
		/// </summary>
		/// <returns>Шаблон</returns>
        public override Stream GetTemplate()
        {
            GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
            CodeTemplate = "NsoProtocolGji";
        }

		/// <summary>
		/// Подготовить отчет
		/// </summary>
		/// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var protocolDomain = Container.ResolveDomain<NsoProtocol>();
            var protocolViolDomain = Container.ResolveDomain<ProtocolViolation>();
            var prescriptionViolDomain = Container.ResolveDomain<PrescriptionViol>();
            var actRemovalViolDomain = Container.ResolveDomain<ActRemovalViolation>();
            var protocolArticleLawDomain = Container.ResolveDomain<ProtocolArticleLaw>();
            var protocolActivityDirDomain = Container.ResolveDomain<ProtocolActivityDirection>();
            var protocolAnnexDomain = Container.ResolveDomain<ProtocolAnnex>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var protocolLongTextDomain = Container.ResolveDomain<ProtocolLongText>();
            var nsoDocLongTextDomain = Container.ResolveDomain<NsoDocumentLongText>();
            var childrenDomain = Container.ResolveDomain<DocumentGjiChildren>();
            var inspGjiRoDomain = Container.ResolveDomain<InspectionGjiRealityObject>();
            var survSubjRequir = Container.ResolveDomain<NsoProtocolSurveySubjectRequirement>();
            var normativeDocDomain = Container.ResolveDomain<NormativeDoc>();
            var disposalService = Container.Resolve<IDisposalService>();
            var nsoProtocolService = Container.Resolve<IProtocolService>();

            try
            {
                var protocol = protocolDomain.FirstOrDefault(x => x.Id == DocumentId);

                if (protocol == null)
                {
                    return;
                }

                FillCommonFields(protocol);

                // 1) СоставлениеПротокола
                var protocolData = new
                {
                    МестоСоставления = protocol.FormatPlace,
                    Дата = protocol.DocumentDate.HasValue ? protocol.DocumentDate.Value.ToShortDateString() : string.Empty,
                    Время = string.Format("{0} : {1}", protocol.FormatHour, protocol.FormatMinute)
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "СоставлениеПротокола",
                    MetaType = nameof(Object),
                    Data = protocolData
                });

                var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

                var actCheck = GetParentDocument(protocol, TypeDocumentGji.ActCheck);

                NsoProtocolGetInfoProxy protocolInfo = null; 
                var baseResult = nsoProtocolService.GetInfo(protocol.Id);
                if (baseResult.Success)
                {
                    protocolInfo = (NsoProtocolGetInfoProxy)baseResult.Data;
                }

                var disposal = actCheck != null
                    ? GetParentDocument(actCheck, TypeDocumentGji.Disposal)
                    : null;

                var disposalInfo = disposal != null
                    ? disposalService.GetInfo(disposal.Id)
                    : null;

                // 2) Реквизиты
                var requisite = new
                {
                    НомерДокумента = protocol.DocumentNumber,
                    ДокументОснование = protocolInfo != null ? protocolInfo.BaseName : string.Empty,
                    ДокументыВСуде = protocol.ToCourt,
                    ДатаПередачиДокументов = protocol.DateToCourt.HasValue ? protocol.DateToCourt.Value.ToShortDateString() : string.Empty,
                    ОснованиеОбследования = disposalInfo != null ? disposalInfo.BaseName : string.Empty
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Реквизиты",
                    MetaType = nameof(Object),
                    Data = requisite
                });

                // 3) Инспектор
                var inspector = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => x.Inspector)
                    .FirstOrDefault();

                var inspectorInfo = new
                {
                    ФамилияИнициалыИнспектора = inspector != null ? inspector.ShortFio : string.Empty,
                    ФамилияИнициалыИнспектораТП = inspector != null
                        ? inspector.FioAblative.IsNotEmpty()
                            ? inspector.FioAblative
                            : inspector.ShortFio
                        : string.Empty,
                    ДолжностьИнспектора = inspector != null ? inspector.Position : string.Empty,
                    ДолжностьИнспектораТП = inspector != null
                        ? inspector.PositionAblative.IsNotEmpty()
                            ? inspector.PositionAblative
                            : inspector.Position
                        : string.Empty
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Инспектор",
                    MetaType = nameof(Object),
                    Data = inspectorInfo
                });


                // 4) ДокументВыдан
                var documentIssued = new
                {
                    ТипИсполнителя = protocol.Executant.Name,
                    ТипИсполнителяТП = protocol.Executant != null
                        ? ТворПадеж(protocol.Executant.Name, склонятель)
                        : string.Empty,
                    ЮридическоеЛицо = protocol.Contragent != null
                        ? protocol.Contragent.Name
                        : string.Empty,
                    ЮридическоеЛицоРП = protocol.Contragent != null
                        ? protocol.Contragent.NameGenitive
                        : string.Empty,
                    ЮридическоеЛицоДП = protocol.Contragent != null
                        ? protocol.Contragent.NameDative
                        : string.Empty,
                    ЮридическоеЛицоТП = protocol.Contragent != null
                        ? protocol.Contragent.NameAblative
                        : string.Empty,
                    ОрганизационноПравоваяФорма = protocol.Contragent != null && protocol.Contragent.OrganizationForm != null
                        ? protocol.Contragent.OrganizationForm.Name
                        : string.Empty,
                    ИНН = protocol.Contragent != null
                        ? protocol.Contragent.Inn
                        : string.Empty,
                    ДатаРегистрации = protocol.Contragent != null
                        ? protocol.Contragent.DateRegistration.HasValue
                            ? protocol.Contragent.DateRegistration.Value.ToShortDateString()
                            : string.Empty
                        : string.Empty,
                    ЮридическийАдрес = protocol.Contragent != null
                        ? protocol.Contragent.JuridicalAddress
                        : string.Empty,
                    ПочтовыйАдрес = protocol.Contragent != null
                        ? protocol.Contragent.MailingAddress
                        : string.Empty,
                    АдресЗаПределамиСубъекта = protocol.Contragent != null
                        ? protocol.Contragent.AddressOutsideSubject
                        : string.Empty,
                    ФизическоеЛицо = protocol.PhysicalPerson,
                    ФизическоеЛицоРП = РодПадеж(protocol.PhysicalPerson, склонятель),
                    ФизическоеЛицоДП = ДатПадеж(protocol.PhysicalPerson, склонятель),
                    ФизическоеЛицоТП = ТворПадеж(protocol.PhysicalPerson, склонятель),
                    Должность = protocol.PersonPosition,
                    ДолжностьТП = ТворПадеж(protocol.PersonPosition, склонятель),
                    ДатаМестоРождения = protocol.PersonBirthDatePlace,
                    АдресРегистрации = protocol.PersonRegistrationAddress,
                    ФактическийАдрес = protocol.PersonFactAddress,
                    ВПрисутсвииОтсутствии = protocol.TypePresence.GetEnumMeta().Display,
                    Представитель = protocol.Representative,
                    ВидРеквизитыОснования = protocol.ReasonTypeRequisites
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ДокументВыдан",
                    MetaType = nameof(Object),
                    Data = documentIssued
                });

                // 5) ПереченьТребований
                var survSubjReq = new
                {
                    ПереченьТребований = survSubjRequir.GetAll()
                        .Where(x => x.Protocol.Id == DocumentId)
                        .Select(x => x.Requirement.Name)
                        .ToList()
                        .AggregateWithSeparator(", ")
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ПереченьТребований",
                    MetaType = nameof(Object),
                    Data = survSubjReq
                });

                // 6) УведомлениеОСоставлении
                var notif = new
                {
                    ВрученоЧерезКанцелярию = protocol.NotifDeliveredThroughOffice,
                    ДатаУведомления = protocol.FormatDate.HasValue ? protocol.FormatDate.Value.ToShortDateString() : string.Empty,
                    НомерРегистрации = protocol.NotifNumber
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "УведомлениеОСоставлении",
                    MetaType = nameof(Object),
                    Data = notif
                });

                // 7) Рассмотрение
                var proceed = new
                {
                    Дата = protocol.DateOfProceedings,
                    Время = string.Format("{0} : {1}", protocol.HourOfProceedings, protocol.MinuteOfProceedings),
                    Место = protocol.ProceedingsPlace,
                    КоличествоЭкземпляров = protocol.ProceedingCopyNum
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Рассмотрение",
                    MetaType = nameof(Object),
                    Data = proceed
                });

                // 8) ЗамечанияНарушителя
                var remarks = new
                {
                    ЗамечанияНарушителя = protocol.Remarks
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ЗамечанияНарушителя",
                    MetaType = nameof(Object),
                    Data = remarks
                });

                // 9) Нарушения
                var violations = protocolViolDomain.GetAll()
                    .Where(x => x.Document.Id == DocumentId)
                    .Select(x => new
                    {
                        ТекстНарушения = x.InspectionViolation.Violation.Name,
                        АдресаДомов = x.InspectionViolation.RealityObject.Address,
                        ПунктНПД = x.InspectionViolation.Violation.NormativeDocNames,
                        СокращенноеНаименованиеНПД = x.InspectionViolation.Violation.NormativeDocNames,
                        Описание = x.Description
                    })
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Нарушения",
                    MetaType = nameof(Object),
                    Data = violations
                });

                // 10) СведенияОНарушении
                var protocolLongText = protocolLongTextDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == DocumentId);
                var violData = new
                {
                    ДатаПравонарушения = protocol.DateOfViolation.HasValue 
                        ? protocol.DateOfViolation.Value.ToShortDateString()
                        : string.Empty,
                    ВремяПравонарушения = string.Format("{0} : {1}", protocol.HourOfViolation, protocol.MinuteOfViolation),
                    Свидетели = protocolLongText != null ? protocolLongText.Witnesses.GetString() : string.Empty,
                    Потерпевшие = protocolLongText != null ? protocolLongText.Victims.GetString() : string.Empty,
                    НаименованиеТребования = protocol.ResolveViolationClaim != null
                        ? protocol.ResolveViolationClaim.Name
                        : string.Empty
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "СведенияОНарушении",
                    MetaType = nameof(Object),
                    Data = violData
                });

                // 11) СтатьиЗакона
                var articles = protocolArticleLawDomain.GetAll()
                    .Where(x => x.Protocol.Id == DocumentId)
                    .Select(x => new
                    {
                        СтатьяЗакона = x.ArticleLaw.Name,
                        Описание = x.Description
                    })
                    .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "СтатьиЗакона",
                    MetaType = nameof(Object),
                    Data = articles
                });

                // 12) Приложения
                var annex = new
                {
                    ПриложенияНаименования = protocolAnnexDomain.GetAll()
                        .Where(x => x.Protocol.Id == DocumentId)
                        .Select(x => x.Name)
                        .ToList()
                        .AggregateWithSeparator(", ")
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Приложения",
                    MetaType = nameof(Object),
                    Data = annex
                });
                
                // 13) СведенияСвидетельствущиеАктПроверки
                var personViolInfo = string.Empty;
                var personViolActionInfo = string.Empty;
                if (actCheck != null)
                {
                    var nsoLongText = nsoDocLongTextDomain.GetAll().FirstOrDefault(x => x.DocumentGji.Id == actCheck.Id);

                    if (nsoLongText != null)
                    {
                        personViolInfo = nsoLongText.PersonViolationInfo.GetString();
                        personViolActionInfo = nsoLongText.PersonViolationActionInfo.GetString();
                    }
                }
                
                var actCheckPersonViolInfo = new
                {
                    СведенияОЛицахДопустившихНарушения = personViolInfo,
                    СведенияСвидетельствующие = personViolActionInfo
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "СведенияСвидетельствущиеАктПроверки",
                    MetaType = nameof(Object),
                    Data = actCheckPersonViolInfo
                });

                // 14) СведенияСвидетельствущиеАктПроверкиПредписания
                var actRemoval = GetParentDocument(protocol, TypeDocumentGji.ActRemoval);
                var actRemovalPersonViolInfo = new
                {
                    СведенияОЛицахДопустившихНарушения = personViolInfo,
                    СведенияСвидетельствующие = personViolActionInfo
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "СведенияСвидетельствущиеАктПроверкиПредписания",
                    MetaType = nameof(Object),
                    Data = actRemovalPersonViolInfo
                });

                // 15) Деятельность
                var baseDoc = new
                {
                    НаправлениеДеятельности = protocolActivityDirDomain.GetAll()
                        .Where(x => x.Protocol.Id == protocol.Id)
                        .ToList()
                        .Select(x => x.ActivityDirection.Name)
                        .AggregateWithSeparator(", "),
                    ПравовоеОснование = protocol.NormativeDoc != null
                        ? protocol.NormativeDoc.Name
                        : string.Empty
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Деятельность",
                    MetaType = nameof(Object),
                    Data = baseDoc
                });

                // 16) Приказ
                var disposalData = new
                {
                    Дата = disposal != null
                        ? disposal.DocumentDate.HasValue
                            ? disposal.DocumentDate.Value.ToShortDateString()
                            : string.Empty
                        : string.Empty,
                    НомерДокумента = disposal != null ? disposal.DocumentNumber : string.Empty
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Приказ",
                    MetaType = nameof(Object),
                    Data = disposalData
                });

                // 17) АктПроверки
                var achtCheckData = new
                {
                    Дата = actCheck != null
                        ? actCheck.DocumentDate.HasValue
                            ? actCheck.DocumentDate.Value.ToShortDateString()
                            : string.Empty
                        : string.Empty,
                    НомерДокумента = actCheck != null ? actCheck.DocumentNumber : string.Empty
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "АктПроверки",
                    MetaType = nameof(Object),
                    Data = achtCheckData
                });

                // 18) Предписание
                // сначала получаем акт проверки из которого создан акт проверки предписания
                var actCheckByActRemoval = actRemoval != null
                    ? GetParentDocument(actRemoval, TypeDocumentGji.ActCheck)
                    : null;
                // далее по акту проверки получаем приказ
                var disposalByActRemoval = actCheckByActRemoval != null
                    ? GetParentDocument(actCheckByActRemoval, TypeDocumentGji.Disposal)
                    : null;
                // и в итоге по приказу получаем предписание
                var prescription = disposalByActRemoval != null
                    ? GetParentDocument(disposalByActRemoval, TypeDocumentGji.Prescription)
                    : null;

                var prescriptionData = new PrescrViolProxy
                {
                    Дата = prescription != null
                        ? prescription.DocumentDate.HasValue
                            ? prescription.DocumentDate.Value.ToShortDateString()
                            : string.Empty
                        : string.Empty,
                    НомерПредписания = prescription != null ? prescription.DocumentNumber : string.Empty,
                    НомераНевыполненныхПунктов = string.Empty
                };

                if (prescription != null)
                {
                    var prescrViols = prescriptionViolDomain.GetAll()
                        .Where(x => x.Document.Id == prescription.Id)
                        .Where(x => !x.DateFactRemoval.HasValue)
                        .OrderBy(x => x.InspectionViolation.RealityObject.Municipality.Name)
                        .ThenBy(x => x.InspectionViolation.RealityObject.Address)
                        .Select(x => new
                        {
                            x.Id,
                            x.InspectionViolation.DateFactRemoval
                        })
                        .ToList();

                    var numbers = new StringBuilder();

                    if (prescrViols.Any(x => x.DateFactRemoval.HasValue))
                    {
                        foreach (var prescrViol in prescrViols)
                        {
                            if (!prescrViol.DateFactRemoval.HasValue)
                            {
                                if (numbers.Length > 0)
                                {
                                    numbers.Append(", ");
                                }

                                numbers.Append((prescrViols.IndexOf(prescrViol) + 1).ToStr());
                            }
                        }
                    }

                    prescriptionData.НомераНевыполненныхПунктов = numbers.ToStr();
                }

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Предписание",
                    MetaType = nameof(PrescrViolProxy),
                    Data = prescriptionData
                });
                
                // 19) ВыполнениеПредписания
                var prescrActRemovalViol = new List<ViolProxy>();

                if (prescription != null)
                {
                    var i = 0;
                    var prescrViols = prescriptionViolDomain.GetAll()
                        .Where(x => x.Document.Id == prescription.Id)
                        .OrderBy(x => x.InspectionViolation.RealityObject.Municipality.Name)
                        .ThenBy(x => x.InspectionViolation.RealityObject.Address)
                        .Select(x => new PrescrViolIndexProxy
                        {
                            Id = x.Id,
                            Index = 0
                        })
                        .ToList();

                    foreach (var prescrViol in prescrViols)
                    {
                        prescrViol.Index = ++i;
                    }

                    prescrActRemovalViol = prescriptionViolDomain.GetAll()
                        .Join(actRemovalViolDomain.GetAll(),
                            x => x.InspectionViolation.Id,
                            y => y.InspectionViolation.Id,
                            (x, y) => new { Prescr = x, ActRemoval = y })
                        .Where(x => x.Prescr.Document.Id == prescription.Id)
                        .Where(x => !x.Prescr.InspectionViolation.DateFactRemoval.HasValue)
                        .ToList()
                        .Select(x => new ViolProxy
                        {
                            НомерПункта = prescrViols.FirstOrDefault(y => y.Id == x.Prescr.Id).Index.ToStr(),
                            Мероприятие = x.Prescr.Action,
                            ОписаниеОбстоятельств = x.ActRemoval.CircumstancesDescription,
                            ДатаФактИсполнения = x.ActRemoval.DateFactRemoval.HasValue
                                ? x.ActRemoval.DateFactRemoval.Value.ToShortDateString()
                                : string.Empty,
                            СрокУстранения = x.ActRemoval.DatePlanRemoval.HasValue
                                ? x.ActRemoval.DatePlanRemoval.Value.ToShortDateString()
                                : string.Empty,
                            Адрес = x.ActRemoval.InspectionViolation.RealityObject.Address
                        })
                        .ToList();
                }

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ВыполнениеПредписания",
                    MetaType = nameof(ViolProxy),
                    Data = prescrActRemovalViol
                });

                // 20) ПриказНаПроверкуПредписания
                var disposalRemoval = actRemoval != null
                    ? GetParentDocument(actRemoval, TypeDocumentGji.Disposal)
                    : null;

                var dispRemovalData = new
                {
                    Дата = disposalRemoval != null
                        ? disposalRemoval.DocumentDate.HasValue
                            ? disposalRemoval.DocumentDate.Value.ToShortDateString()
                            : string.Empty
                        : string.Empty,
                    НомерДокумента = disposalRemoval != null
                        ? disposalRemoval.DocumentNumber
                        : string.Empty
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ПриказНаПроверкуПредписания",
                    MetaType = nameof(Object),
                    Data = dispRemovalData
                });

                // 21) АктПроверкиПредписания
                var actRemovalData = new
                {
                    Дата = actRemoval != null
                        ? actRemoval.DocumentDate.HasValue
                            ? actRemoval.DocumentDate.Value.ToShortDateString()
                            : string.Empty
                        : string.Empty,
                    НомерДокумента = actRemoval != null
                        ? actRemoval.DocumentNumber
                        : string.Empty
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "АктПроверкиПредписания",
                    MetaType = nameof(Object),
                    Data = actRemovalData
                });

                // 22) Дома
                var houses = new HouseProxy();
                if (disposalRemoval != null)
                {
                    var prescriptionsIds =
                        childrenDomain.GetAll()
                            .Where(x => x.Children.Id == disposalRemoval.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                            .Select(x => x.Parent.Id)
                            .ToList();

                    var listObjects =
                        prescriptionViolDomain.GetAll()
                            .Where(x => prescriptionsIds.Contains(x.Document.Id))
                            .Select(x => x.InspectionViolation.RealityObject.Id)
                            .Distinct()
                            .ToList();

                    var anyHouses = inspGjiRoDomain.GetAll()
                        .Any(x => x.Inspection.Id == disposalRemoval.Inspection.Id
                                  && listObjects.Contains(x.RealityObject.Id));

                    houses = new HouseProxy
                    {
                        Дома = anyHouses
                    };
                }

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Дома",
                    MetaType = nameof(HouseProxy),
                    Data = houses
                });


            }
            finally
            {
                Container.Release(protocolDomain);
                Container.Release(protocolViolDomain);
                Container.Release(prescriptionViolDomain);
                Container.Release(actRemovalViolDomain);
                Container.Release(protocolArticleLawDomain);
                Container.Release(protocolActivityDirDomain);
                Container.Release(protocolAnnexDomain);
                Container.Release(docInspectorDomain);
                Container.Release(protocolLongTextDomain);
                Container.Release(nsoDocLongTextDomain);
                Container.Release(childrenDomain);
                Container.Release(inspGjiRoDomain);
                Container.Release(survSubjRequir);
                Container.Release(normativeDocDomain);
                Container.Release(disposalService); 
                Container.Release(nsoProtocolService);
            }
        }

        public class HouseProxy
        {
            public bool Дома { get; set; }
        }

        public class ViolProxy
        {
            public string НомерПункта { get; set; }
            public string Мероприятие { get; set; }
            public string ОписаниеОбстоятельств { get; set; }
            public string ДатаФактИсполнения { get; set; }
            public string СрокУстранения { get; set; }
            public string Адрес { get; set; }
        }

        public class PrescrViolProxy
        {
            public string Дата { get; set; }
            public string НомерПредписания { get; set; }
            public string НомераНевыполненныхПунктов { get; set; }
        }

        public class PrescrViolIndexProxy
        {
            public long Id { get; set; }
            public int Index { get; set; }
        }

        private string ТворПадеж(string word, Склонятель склонятель)
        {
            if (string.IsNullOrEmpty(word))
            {
                return string.Empty;
            }

            var analyzed = склонятель.Проанализировать(word);
            return analyzed.Творительный;
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