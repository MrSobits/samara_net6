namespace Bars.GkhGji.Regions.Smolensk.Report
{
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Gkh.Entities;
    using Gkh.Report;
    using GkhGji.Entities;
    using GkhGji.Enums;

    public abstract class GjiBaseStimulReport : GkhBaseStimulReport
    {
        public IDomainService<DocumentGjiChildren> DocChildDomain { get; set; }

        protected GjiBaseStimulReport(IReportTemplate reportTemplate)
            : base(reportTemplate)
        {
        }

        protected DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji seachingTypeDoc)
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

        protected void FillCommonFields(DocumentGji doc)
        {
            this.ReportParams["ИдентификаторДокументаГЖИ"] = doc.Id.ToString();
            this.ReportParams["СтрокаПодключениякБД"] = Container.Resolve<IDbConfigProvider>().ConnectionString;
        }
    }
}
