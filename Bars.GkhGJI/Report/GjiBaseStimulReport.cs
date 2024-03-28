namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using DomainService;
    using Gkh.Utils;
    using Slepov.Russian.Morpher;
    using Utils = Utils.Utils;

    public abstract class GjiBaseStimulReport : GkhBaseStimulReport
    {
        public IDomainService<DocumentGjiChildren> DocChildDomain { get; set; }

        protected GjiBaseStimulReport(IReportTemplate reportTemplate)
            : base(reportTemplate)
        {
        }

     
        protected void FillCommonFields(DocumentGji doc)
        {
            this.ReportParams["ИдентификаторДокументаГЖИ"] = doc.Id.ToString();
            this.ReportParams["СтрокаПодключениякБД"] = Container.Resolve<IDbConfigProvider>().ConnectionString;

            // зональную инспекцию получаем через муниципальное образование первого дома
            var firstRo = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                .Where(x => x.Inspection.Id == doc.Inspection.Id)
                .Select(x => x.RealityObject)
                .FirstOrDefault();

            var docType = doc.Inspection.TypeBase;

            if (docType == TypeBase.ProsecutorsResolution)
            {
                firstRo = Container.Resolve<IDomainService<ResolProsRealityObject>>().GetAll()
                         .Where(x => x.ResolPros.Inspection.Id == doc.Inspection.Id)
                         .Select(x => x.RealityObject)
                         .FirstOrDefault();
            }

            if (firstRo != null)
            {
                var zonalInspection = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
                    .Where(x => x.Municipality.Id == firstRo.Municipality.Id)
                    .Select(x => x.ZonalInspection)
                    .FirstOrDefault();

                if (zonalInspection != null)
                {
                    this.ReportParams["ЗональноеНаименование1ГосЯзык"] = zonalInspection.BlankName;
                    this.ReportParams["ЗональноеНаименование2ГосЯзык"] = zonalInspection.BlankNameSecond;
                    this.ReportParams["ЗональноеНаименование1ГосЯзык1"] = zonalInspection.ZoneName;
                    this.ReportParams["Адрес1ГосЯзык"] = zonalInspection.Address;
                    this.ReportParams["Адрес2ГосЯзык"] = zonalInspection.AddressSecond;
                    this.ReportParams["Телефон"] = zonalInspection.Phone;
                    this.ReportParams["Email"] = zonalInspection.Email;
                    this.ReportParams["ЗональноеНаименование1ГосЯзык(ТворП)"] = zonalInspection.NameAblative;
                    this.ReportParams["ЗональноеНаименование1ГосЯзык(РП)"] = zonalInspection.NameGenetive;
                    this.ReportParams["ЗональноеНаименование1ГосЯзык(ВП)"] = zonalInspection.NameAccusative;
                }

                if (doc.Inspection != null)
                {
                    if (doc.Inspection.Contragent != null)
                    {
                        this.ReportParams["УправОрг(РП)"] = doc.Inspection.Contragent.NameGenitive;
                        this.ReportParams["УправОрг"] = doc.Inspection.Contragent.Name;
                        this.ReportParams["АдресКонтрагента"] = doc.Inspection.Contragent.FiasJuridicalAddress != null
                            ? doc.Inspection.Contragent.FiasJuridicalAddress.AddressName
                            : "";
                    }

                    if (doc.Inspection.TypeBase == TypeBase.CitizenStatement)
                    {
                        var appeals =
                            Container.Resolve<IDomainService<InspectionAppealCits>>()
                                     .GetAll()
                                     .Where(x => x.Inspection.Id == doc.Inspection.Id).ToList();

                        if (appeals.Count > 0)
                        {
                            this.ReportParams["Корреспондент"] = String.Join(",", appeals.Select(x => x.AppealCits.Correspondent));
                            this.ReportParams["АдресКорреспондента"] = String.Join(",",
                                appeals.Select(x => x.AppealCits.CorrespondentAddress));
                        }
                    }
                }
            }
        }

        protected virtual DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji seachingTypeDoc)
        {
            var result = document;

            if (document.TypeDocumentGji != seachingTypeDoc)
            {
                var docs =
                    DocChildDomain.GetAll()
                        .Where(x => x.Children.Id == document.Id)
                        .Select(x => x.Parent)
                        .ToList();

                foreach (var doc in docs)
                {
                    if (result != document && result.TypeDocumentGji == seachingTypeDoc)
                    {
                        return result;
                    }
                    result = GetParentDocument(doc, seachingTypeDoc);
                }
            }

            if (result != null)
            {
                return result.TypeDocumentGji == seachingTypeDoc ? result : null;
            }

            return null;
        }

        protected DocumentGji GetChildDocument(DocumentGji document, TypeDocumentGji seachingTypeDoc)
        {
            var result = document;

            if (document.TypeDocumentGji != seachingTypeDoc)
            {
                var docs = DocChildDomain.GetAll()
                    .Where(x => x.Parent.Id == document.Id)
                    .Select(x => x.Children)
                    .ToList();

                foreach (var doc in docs)
                {
                    result = GetChildDocument(doc, seachingTypeDoc);
                }
            }

            if (result != null)
            {
                return result.TypeDocumentGji == seachingTypeDoc ? result : null;
            }

            return null;
        }

        protected Disposal GetMainDisposal(InspectionGji inspection)
        {
            if (inspection == null)
            {
                return null;
            }

            var disposalDomain = Container.ResolveDomain<Disposal>();

            using (Container.Using(disposalDomain))
            {
                return disposalDomain.GetAll()
                    .Where(x => x.TypeDisposal == TypeDisposalGji.Base)
                    .FirstOrDefault(x => x.Inspection.Id == inspection.Id);
            }
        }

        protected Inspector FillFirstInspector(DocumentGji document)
        {
            if (document == null)
            {
                return null;
            }

            Inspector inspector = null;

            var docinspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            
            using (Container.Using(docinspectorDomain))
            {
                var inspectors = docinspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == document.Id)
                    .Select(x => x.Inspector)
                    .ToArray();

                if (inspectors.Any())
                {
                    inspector = inspectors.First();
                    FillInspectorFio(inspector);
                    FillInspectorPosition(inspector);
                }
            }

            return inspector;
        }

        protected void FillInspectorFio(Inspector inspector, string prefix = "ИнспекторФио")
        {
            if (inspector == null || string.IsNullOrEmpty(prefix))
            {
                return;
            }

            this.ReportParams[prefix + "Сокр"] = inspector.ShortFio;

            this.ReportParams[prefix] = inspector.Fio;
            this.ReportParams[prefix + "Рп"] = inspector.FioGenitive;
            this.ReportParams[prefix + "Дп"] = inspector.FioDative;
            this.ReportParams[prefix + "Вп"] = inspector.FioAccusative;
            this.ReportParams[prefix + "Тп"] = inspector.FioAblative;
            this.ReportParams[prefix + "Пп"] = inspector.FioPrepositional;
        }

        protected void FillInspectorPosition(Inspector inspector, string prefix = "ИнспекторДолжность")
        {
            if (inspector == null || string.IsNullOrEmpty(prefix))
            {
                return;
            }

            this.ReportParams[prefix] = inspector.Position;
            this.ReportParams[prefix + "Рп"] = inspector.PositionGenitive;
            this.ReportParams[prefix + "Дп"] = inspector.PositionDative;
            this.ReportParams[prefix + "Вп"] = inspector.PositionAccusative;
            this.ReportParams[prefix + "Тп"] = inspector.PositionAblative;
            this.ReportParams[prefix + "Пп"] = inspector.PositionPrepositional;
        }

        protected void FillContragent(Contragent contragent, string prefix = "Контрагент")
        {
            if (contragent == null || string.IsNullOrEmpty(prefix))
            {
                return;
            }

            this.ReportParams["УправОргСокр"] = contragent.ShortName;
            this.ReportParams["АдресКонтрагентаФакт"] = contragent.FiasFactAddress.AddressName;

            this.ReportParams[prefix] = contragent.Name;
            this.ReportParams[prefix + "Рп"] = contragent.NameGenitive;
            this.ReportParams[prefix + "Дп"] = contragent.NameDative;
            this.ReportParams[prefix + "Вп"] = contragent.NameAccusative;
            this.ReportParams[prefix + "Тп"] = contragent.NameAblative;
            this.ReportParams[prefix + "Пп"] = contragent.NamePrepositional;

            this.ReportParams[prefix + "Сокр"] = contragent.ShortName;

            this.ReportParams[prefix + "ЮрАдрес"] = contragent.JuridicalAddress;
            this.ReportParams[prefix + "ПочтАдрес"] = contragent.MailingAddress;
            this.ReportParams[prefix + "ФактАдрес"] = contragent.FactAddress;
            this.ReportParams[prefix + "АдресЗаПределами"] = contragent.AddressOutsideSubject;

            this.ReportParams[prefix + "ЮрАдресПолный"] = contragent.FiasJuridicalAddress.Return(x => x.AddressName);
            this.ReportParams[prefix + "ПочтАдресПолный"] = contragent.FiasMailingAddress.Return(x => x.AddressName);
            this.ReportParams[prefix + "ФактАдресПолный"] = contragent.FiasFactAddress.Return(x => x.AddressName);
            this.ReportParams[prefix + "АдресЗаПределамиПолный"] = contragent.FiasOutsideSubjectAddress.Return(x => x.AddressName);

            this.ReportParams[prefix + "ИНН"] = contragent.Inn;
            this.ReportParams[prefix + "КПП"] = contragent.Kpp;
            this.ReportParams[prefix + "ДатаРегистрации"] =
                contragent.DateRegistration.HasValue
                    ? contragent.DateRegistration.Value.ToShortDateString()
                    : string.Empty;
        }

        protected void FillDocument(DocumentGji document, string prefix = "Документ", string dateFormat = "dd.MM.yyyy")
        {
            var docdate = document.DocumentDate.HasValue
                ? document.DocumentDate.Value.ToString(dateFormat, new CultureInfo("ru-RU"))
                : null;

            this.ReportParams[prefix + "Номер"] = document.DocumentNumber;
            this.ReportParams[prefix + "Дата"] = docdate;
            this.ReportParams[prefix + "НомерДата"] = string.Format("№ {0} от {1}", document.DocumentNumber, docdate);
        }

        protected string CutFio(string fio)
        {
            if (fio.IsEmpty())
            {
                return "";
            }

            var splits = fio.Split(' ');

            var surname = splits[0];

            var result = new StringBuilder(surname);
            result.Append(" ");

            for (int i = 1; i < splits.Length; i++)
            {
                result.AppendFormat("{0}.", splits[i].Substring(0, 1).ToUpper());
            }

            return result.ToString();
        }

        protected string GetPeriodInHours(DateTime dateStart, DateTime dateEnd)
        {
            var time = dateEnd - dateStart;
            return string.Format("{0} час. {1} мин.", time.Hours,
                time.Minutes.ToString().PadLeft(2));
        }

        protected string DateToString(DateTime? date, string format = "dd.MM.yyyy")
        {
            return date.HasValue ? date.Value.ToString(format) : string.Empty;
        }

        protected string DateTimeToTimeString(DateTime? date, string format = "t")
        {
            return date.HasValue ? date.Value.ToString(format) : string.Empty;
        }

        protected void RegInspectorsDataSource(IEnumerable<Inspector> inspectiors, string dataSourceName = "Инспекторы")
        {
            DataSources.Add(new MetaData
            {
                SourceName = dataSourceName,
                Data = inspectiors.ToList().Select(x => new Инспектор(x)),
                MetaType = nameof(Инспектор)
            });
        }

        protected void RegViolDataSource(IEnumerable<InspectionGjiViol> violations, string dataSourceName = "Нарушения")
        {
            DataSources.Add(new MetaData
            {
                SourceName = dataSourceName,
                Data = violations.ToList().Select(x => new Нарушение(x)),
                MetaType = nameof(Нарушение)
            });
        }

        protected string DaysSuffix(int count)
        {
            var lastCipher = count.ToString().Last().ToInt();
            if (lastCipher == 0)
            {
                return "дней";
            }

            if (lastCipher == 1)
            {
                return "день";
            }

            if (lastCipher < 5)
            {
                return "дня";
            }

            return "дней";
        }

        protected string Coalesce(params string[] items)
        {
            if (items == null || !items.Any())
            {
                return null;
            }

            return items.Coalesce();
        }

        private Склонятель _morpher;

        protected Склонятель GetMorpher()
        {
            return _morpher ?? (_morpher = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ=="));
        }

        /// <summary>
        /// Получить основание документа (поле документ-основание)
        /// </summary>
        /// <returns></returns>
        protected string GetDocumentBase(DocumentGji doc)
        {
            if (doc == null) return "";

            if (doc.TypeDocumentGji == TypeDocumentGji.Disposal) return GetDisposalBase(doc);

            var parents = Container.ResolveDomain<DocumentGjiChildren>().GetAll()
                .Where(x => x.Children.Id == doc.Id)
                .Select(x => new
                {
                    parentId = x.Parent.Id,
                    x.Parent.TypeDocumentGji,
                    x.Parent.DocumentDate,
                    x.Parent.DocumentNumber
                })
                .ToList();

            var result =
                parents.AggregateWithSeparator(x =>
                    string.Format("{0} №{1} от {2}",
                        Utils.GetDocumentName(x.TypeDocumentGji),
                        x.DocumentNumber,
                        x.DocumentDate.ToDateString()),
                    ", ");

            return result;
        }

        private string GetDisposalBase(DocumentGji document)
        {
            string baseName = null;

            try
            {
                var disposalInfo = Container.Resolve<IDisposalService>().GetInfo(document.Id);
                baseName = disposalInfo.Return(x => x.BaseName);
            }
            catch (Exception)
            {
                //be silent
            }

            return baseName;
        }

        #region Мета-класы

        protected class Инспектор
        {
            public string Код { get; protected set; }

            #region ФИО

            public string ФИО { get; set; }
            public string ФИО_РП { get; set; }
            public string ФИО_ДП { get; set; }
            public string ФИО_ВП { get; set; }
            public string ФИО_ТП { get; set; }
            public string ФИО_ПП { get; set; }

            #endregion

            public string ФамилияИО { get; set; }
            public string Телефон { get; set; }
            public bool Начальник { get; set; }

            #region Должность

            public string Должность { get; set; }
            public string Должность_РП { get; set; }
            public string Должность_ДП { get; set; }
            public string Должность_ВП { get; set; }
            public string Должность_ТП { get; set; }
            public string Должность_ПП { get; set; }

            #endregion

            public Инспектор(Inspector inspector)
            {
                if (inspector != null)
                {
                    Код = inspector.Code;

                    ФИО = inspector.Fio;
                    ФИО_ВП = inspector.FioAccusative;
                    ФИО_ДП = inspector.FioDative;
                    ФИО_ПП = inspector.FioPrepositional;
                    ФИО_РП = inspector.FioGenitive;
                    ФИО_ТП = inspector.FioAblative;

                    ФамилияИО = inspector.ShortFio;
                    Телефон = inspector.Phone;
                    Начальник = inspector.IsHead;

                    Должность = inspector.Position;
                    Должность_ВП = inspector.PositionAccusative;
                    Должность_ДП = inspector.PositionDative;
                    Должность_ПП = inspector.PositionPrepositional;
                    Должность_РП = inspector.PositionGenitive;
                    Должность_ТП = inspector.PositionAblative;
                }
            }
        }

        protected class Нарушение
        {
            public Нарушение(InspectionGjiViol violation)
            {
                Наименование = violation.Violation.Name;
                ЖКРФ = violation.Violation.GkRf;
                КодПИН = violation.Violation.CodePin;
                Описание = violation.Description;
                ППРФ25 = violation.Violation.PpRf25;
                ППРФ307 = violation.Violation.PpRf307;
                ППРФ491 = violation.Violation.PpRf491;
                ППРФ170 = violation.Violation.PpRf170;
                ПрочиеНормативныеДокументы = violation.Violation.OtherNormativeDocs;
                МероприятиеПоУстранению = violation.Action;
                ПлановаяДатаУстранения = violation.DatePlanRemoval.HasValue
                    ? violation.DatePlanRemoval.Value.ToString("dd.MM.yyyy")
                    : string.Empty;
                ФактДатаУстранения = violation.DateFactRemoval.HasValue
                    ? violation.DateFactRemoval.Value.ToString("dd.MM.yyyy")
                    : string.Empty;
                СуммаРабот = violation.SumAmountWorkRemoval.HasValue ? violation.SumAmountWorkRemoval.Value : 0M;
            }

            public string Наименование { get; set; }
            public string ЖКРФ { get; set; }
            public string КодПИН { get; set; }
            public string Описание { get; set; }
            public string ППРФ25 { get; set; }
            public string ППРФ307 { get; set; }
            public string ППРФ491 { get; set; }
            public string ППРФ170 { get; set; }
            public string ПрочиеНормативныеДокументы { get; set; }
            public string МероприятиеПоУстранению { get; set; }
            public string ПлановаяДатаУстранения { get; set; }
            public string ФактДатаУстранения { get; set; }
            public decimal СуммаРабот { get; set; }

        }

        #endregion
    }
}