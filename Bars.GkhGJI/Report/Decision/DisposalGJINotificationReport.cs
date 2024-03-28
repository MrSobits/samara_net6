namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Contracts;
    /// <summary>
    /// 
    /// </summary>
    public class DisposalGjiNotificationReport : GjiBaseReport
    {
        private long DocumentId { get; set; }
        /// <summary>
        /// Код шаблона
        /// </summary>
        protected override string CodeTemplate { get; set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        public DisposalGjiNotificationReport() : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_InstructionNotification))
        {
        }
        /// <summary>
        /// Названия отчета
        /// </summary>
        public override string ReportGeneratorName
        {
            get { return "DocIoGenerator"; }
        }
        /// <summary>
        /// Установка параметров
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }
        /// <summary>
        /// Информация о шаблоне
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "BlockGJI_InstructionNotification",
                            Name = "DisposalNotification",
                            Description = "Уведомление о проверке из приказа",
                            Template = Properties.Resources.BlockGJI_InstructionNotification
                        }
                };
        }
        /// <summary>
        /// ID
        /// </summary>
        public override string Id
        {
            get { return "DisposalNotification"; }
        }
        /// <summary>
        /// Код
        /// </summary>
        public override string CodeForm
        {
            get { return "Disposal"; }
        }
        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Уведомление о проверке"; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        public override string Description
        {
            get { return "Уведомление о проверке (из приказа)"; }
        }
        /// <summary>
        /// Подготовка данных для отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var disposal = Container.Resolve<IDomainService<Disposal>>().Load(DocumentId);

            if (disposal == null)
            {
                var disposalText = Container.Resolve<IDisposalText>().SubjectiveCase;
                throw new ReportProviderException(string.Format("Не удалось получить {0}", disposalText.ToLower()));
            }

            this.FillCommonFields(reportParams, disposal);

            // зональную инспекцию получаем через муниципальное образование первого дома
            var realityObjs = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                .Select(x => x.RealityObject)
                .ToArray();

            var sectionAddress = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияАдрДомовУведом");

            foreach (var ro in realityObjs)
            {
                sectionAddress.ДобавитьСтроку();
                sectionAddress["ДомаИАдресаУведом"] = ro.Address;
            }

            reportParams.SimpleReportParams["ДомАдресУведом"] = string.Join("; ", realityObjs.Select(x => x.Address).ToList());
            reportParams.SimpleReportParams["DispId"] = disposal.Id;

            var firstRo = realityObjs.FirstOrDefault();

            if (firstRo != null)
            {
                var zonalInspection = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
                    .Where(x => x.Municipality.Id == firstRo.Municipality.Id)
                    .Select(x => x.ZonalInspection)
                    .FirstOrDefault();

                if (zonalInspection != null)
                {
                    reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык"] = zonalInspection.BlankName;
                    reportParams.SimpleReportParams["НаименованиеУправления"] = zonalInspection.ZoneName;
                    reportParams.SimpleReportParams["ЗональноеНаименование2ГосЯзык"] = zonalInspection.BlankNameSecond;
                    reportParams.SimpleReportParams["Адрес1ГосЯзык"] = zonalInspection.Address;
                    reportParams.SimpleReportParams["Адрес2ГосЯзык"] = zonalInspection.AddressSecond;
                    reportParams.SimpleReportParams["Телефон"] = zonalInspection.Phone;
                    reportParams.SimpleReportParams["Email"] = zonalInspection.Email;
                }

                if (realityObjs.Count() == 1 && firstRo.FiasAddress != null)
                {
                    if (firstRo.FiasAddress != null)
                    {
                        reportParams.SimpleReportParams["НомерДома"] = firstRo.FiasAddress.House;
                        reportParams.SimpleReportParams["Улица"] = firstRo.FiasAddress.StreetName;
                        reportParams.SimpleReportParams["НасПункт"] = firstRo.FiasAddress.PlaceName;
                        reportParams.SimpleReportParams["АдресОбъектаПравонарушения"] = firstRo.FiasAddress.AddressName;
                    }
                }
            }

            if (realityObjs.Length > 0)
            {
                var placeName = firstRo != null ? firstRo.FiasAddress.PlaceName + ", " : string.Empty;

                var listAddress = realityObjs.Select(x => string.Format("{0}, д. {1}", x.FiasAddress.StreetName, x.FiasAddress.House))
                                             .ToList();

                var addressStr = string.Join("; ", listAddress);

                reportParams.SimpleReportParams["ДомаИАдреса"] = placeName + addressStr;
            }

            var cultureInfo = new CultureInfo("ru-RU");

            var contragent = disposal.Inspection.Contragent;

            if (contragent != null)
            {
                reportParams.SimpleReportParams["УправОрг"] = contragent.Name;
                reportParams.SimpleReportParams["ИНН"] = contragent.Inn;
                reportParams.SimpleReportParams["Адрес"] = contragent.JuridicalAddress;

                var headContragent =
                    Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                            .Where(x => x.Contragent.Id == contragent.Id && x.DateStartWork.HasValue
                                         && (x.DateStartWork.Value <= DateTime.Now && (!x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)))
                            .FirstOrDefault(x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

                if (headContragent != null)
                {
                    reportParams.SimpleReportParams["ФИОРукОрг"] =
                        headContragent.Position.Name + " - " + headContragent.FullName;


                    reportParams.SimpleReportParams["ДолжностьРуководителяДатПадеж"] = headContragent.Position.NameDative;
                    reportParams.SimpleReportParams["ФИОРуководителяДатПадеж"] = String.Format("{0} {1} {2}",
                        headContragent.SurnameDative, headContragent.NameDative, headContragent.PatronymicDative);
                }

                if (contragent.FiasFactAddress != null)
                {
                    var subStr = contragent.FiasFactAddress.AddressName.Split(',');

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

                    reportParams.SimpleReportParams["АдресКонтрагентаФакт"] = newAddr;
                }
                else
                {
                    reportParams.SimpleReportParams["АдресКонтрагентаФакт"] = contragent.FactAddress;
                }

                reportParams.SimpleReportParams["АдресКонтрагента"] = contragent.FiasJuridicalAddress != null ? contragent.FiasJuridicalAddress.AddressName : contragent.AddressOutsideSubject ;
            }

            reportParams.SimpleReportParams["ВидОбследования"] = disposal.KindCheck != null ? disposal.KindCheck.Name : "";

            reportParams.SimpleReportParams["Распоряжение"] = string.Format(
                "{0} от {1}",
                disposal.DocumentNumber,
                disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToString("dd MMMM yyyy") : string.Empty);

            var act = GetChildDocument(disposal, TypeDocumentGji.ActCheck);

            reportParams.SimpleReportParams["НомерПроверки"] = string.Format(
                    "{0} от {1}",
                    disposal.DocumentNum,
                    disposal.DocumentDate.HasValue ? disposal.DocumentDate.Value.ToString("D", cultureInfo) : string.Empty);

            reportParams.SimpleReportParams["ДолжностьОтветственного"] = disposal.ResponsibleExecution != null
                    ? disposal.ResponsibleExecution.Position
                    : string.Empty;
            reportParams.SimpleReportParams["ТелефонОтветственного"] = disposal.ResponsibleExecution != null
                ? disposal.ResponsibleExecution.Phone
                : string.Empty;
            reportParams.SimpleReportParams["Ответственный"] = disposal.ResponsibleExecution != null
                ? disposal.ResponsibleExecution.ShortFio
                : string.Empty;

            if (act != null)
            {
                reportParams.SimpleReportParams["ДатаПроверки"] = 
                    act.DocumentDate.HasValue
                        ? act.DocumentDate.Value.ToString("D", cultureInfo)
                        : string.Empty;
            }

            var inspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                .Where(x => x.DocumentGji.Id == disposal.Id)
                                .Select(x => x.Inspector)
                                .ToArray();

            if (inspectors.Any())
            {
                reportParams.SimpleReportParams["Инспектор"] = inspectors.Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Fio : y.Fio));

                reportParams.SimpleReportParams["Должность"] = inspectors.FirstOrDefault().Position;
                reportParams.SimpleReportParams["Исполнитель"] = inspectors.FirstOrDefault().FioAblative;
            }

            reportParams.SimpleReportParams["Начальник"] = disposal.IssuedDisposal != null
                                                               ? disposal.IssuedDisposal.ShortFio
                                                               : string.Empty;

            reportParams.SimpleReportParams["НомерРаспоряжения"] = disposal.DocumentNumber;
            reportParams.SimpleReportParams["ДатаРаспоряжения"] = disposal.DocumentDate.ToDateTime().ToShortDateString();
            reportParams.SimpleReportParams["КодРуководителя"] = disposal.IssuedDisposal != null
                                                               ? disposal.IssuedDisposal.Code
                                                               : string.Empty;

            if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
            {
                reportParams.SimpleReportParams["Цель"] = "Проверка исполнения предписания";

                var basePrescription = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                    .Where(x => x.Children.Id == disposal.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                    .Select(x => x.Parent)
                    .FirstOrDefault();

                if (basePrescription != null)
                {
                    reportParams.SimpleReportParams["ОснованиеОбследования"] =
                        string.Format(
                            "уведомление о проведении внеплановой выездной проверки с целью проверки исполнения предписания № {0} от {1}",
                            basePrescription.DocumentNumber,
                            basePrescription.DocumentDate.HasValue  ? basePrescription.DocumentDate.Value.ToString("D", cultureInfo) : string.Empty);
                }
            }

            if (disposal.IssuedDisposal != null)
            {
                reportParams.SimpleReportParams["РуководительДолжность"] = disposal.IssuedDisposal.Position;
                reportParams.SimpleReportParams["РуководительФИОСокр"] = disposal.IssuedDisposal.ShortFio;
            }

            var listDisposalProvidedDoc = Container.Resolve<IDomainService<DisposalProvidedDoc>>()
                         .GetAll()
                         .Where(x => x.Disposal.Id == disposal.Id)
                         .Select(x => x.ProvidedDoc.Name)
                         .ToList();

            reportParams.SimpleReportParams["ПредоставляемыеДокументы"] = string.Join(",", listDisposalProvidedDoc);

            var queryTypeSurveysId = Container.Resolve<IDomainService<DisposalTypeSurvey>>().GetAll()
                                       .Where(x => x.Disposal.Id == disposal.Id)
                                       .Select(x => x.TypeSurvey.Id);

            var listTypeSurveyGoalInspGji = Container.Resolve<IDomainService<TypeSurveyGoalInspGji>>().GetAll()
                                                     .Where(x => queryTypeSurveysId.Contains(x.TypeSurvey.Id))
                                                     .Select(x => new { x.SurveyPurpose.Name, x.Code })
                                                     .OrderBy(x => x.Code)
                                                     .ToArray()
                                                     .Select(x => x.Name)
                                                     .Distinct()
                                                     .ToList();

            var inspFounds = Container.Resolve<IDomainService<TypeSurveyInspFoundationGji>>().GetAll()
                                        .Where(x => queryTypeSurveysId.Contains(x.TypeSurvey.Id))
                                        .Select(x => new { x.NormativeDoc.Name, x.Code })
                                        .OrderBy(x => x.Code)
                                        .ToArray()
                                        .Select(x => x.Name)
                                        .Distinct()
                                        .ToList();

            var inspFoundation = new StringBuilder();

            foreach (var foundation in inspFounds.Where(x => !string.IsNullOrEmpty(x)))
            {
                if (inspFoundation.Length > 0)
                    inspFoundation.Append(";\n- ");

                inspFoundation.Append(foundation);
            }

            if (inspFoundation.Length > 0)
                inspFoundation.Append(".");

            reportParams.SimpleReportParams["ПравовоеОснование"] = inspFoundation.ToString();

            reportParams.SimpleReportParams["ЦельУведомления"] = string.Join(", ", listTypeSurveyGoalInspGji);

            var quryAppeals = Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                        .Where(x => x.Inspection.Id == disposal.Inspection.Id);

                var appeals = quryAppeals.Select(x => new
                                        {
                                            x.Id,
                                            appealId = x.AppealCits.Id,
                                            x.AppealCits.Correspondent,
                                            x.AppealCits.DocumentNumber,
                                            x.AppealCits.CorrespondentAddress,
                                            x.AppealCits.NumberGji,
                                            x.AppealCits.DateFrom,
                                            x.AppealCits.TypeCorrespondent
                                        })
                                    .ToList();

                var quryAppealsId = quryAppeals.Select(x => x.AppealCits.Id);

                var appealsRo = Container.Resolve<IDomainService<AppealCitsRealityObject>>()
                                         .GetAll()
                                         .Where(x => quryAppealsId.Contains(x.AppealCits.Id))
                                         .Select(x => new
                                                         {
                                                             appId = x.AppealCits.Id,
                                                             muName = x.RealityObject.Municipality.Name,
                                                             address = x.RealityObject.Address
                                                         })
                                         .ToList();

            if (appeals.Count > 0)
            {
                var fioCorr = new StringBuilder();
                var appealsNumDate = new StringBuilder();
                var appealAddress = new StringBuilder();

                foreach (var appeal in appeals)
                {
                    if (!string.IsNullOrEmpty(appeal.Correspondent))
                    {
                        if (fioCorr.Length > 0) fioCorr.Append(", ");

                        fioCorr.Append(appeal.Correspondent);
                    }                    

                    var appealsNumDateStr = string.Empty;
                    if (!string.IsNullOrEmpty(appeal.NumberGji))
                    {
                        if (appealsNumDate.Length > 0) appealsNumDate.Append(", ");

                        appealsNumDateStr = string.Format(
                            "{0} от {1}",
                            appeal.NumberGji,
                            appeal.DateFrom.HasValue ? appeal.DateFrom.Value.ToShortDateString() : string.Empty);


                        appealsNumDate.AppendFormat(appealsNumDateStr);                        
                    }

                    var addressRo = string.Empty;
                    var appealRo = appealsRo.Where(x => x.appId == appeal.appealId).ToList();
                    if (appealRo.Count > 0)
                    {
                        var item = appealRo.First();
                        addressRo = string.Format("{0}, {1}", item.muName, item.address);
                    }

                    var appealsDocNumDateStr = string.Empty;
                    if (!string.IsNullOrEmpty(appeal.DocumentNumber))
                    {
                        appealsDocNumDateStr = string.Format("{0} от {1}", 
                            appeal.DocumentNumber, 
                            appeal.DateFrom.HasValue ? appeal.DateFrom.Value.ToShortDateString() : string.Empty);
                    }

                    if (!string.IsNullOrEmpty(appeal.Correspondent))
                    {
                        if (appealAddress.Length > 0) appealAddress.Append("; ");

                        appealAddress.AppendFormat("{0} (вх. {1}), проживающего по адресу {2}", appeal.Correspondent, appealsDocNumDateStr, addressRo);
                    }
                }

                reportParams.SimpleReportParams["ФИООбр"] = fioCorr.ToString();
                reportParams.SimpleReportParams["Обращения"] = appealsNumDate.ToString();
                reportParams.SimpleReportParams["АдресЗаявителя"] = string.Join("; ", appealsRo.Select(x => string.Format("{0}, {1}", x.muName, x.address)));
                reportParams.SimpleReportParams["РеквизитыОбращения"] = appealAddress.ToString();
            }

            var strPurposeDetails = string.Empty;
            if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement && disposal.TypeDisposal != TypeDisposalGji.DocumentGji)
            {
                strPurposeDetails = reportParams.SimpleReportParams["РеквизитыОбращения"] != null
                    ? reportParams.SimpleReportParams["РеквизитыОбращения"].ToString()
                    : string.Empty;
            }
            else
            {
                strPurposeDetails = reportParams.SimpleReportParams["ДомАдресУведом"].ToString();
            }

            reportParams.SimpleReportParams["ЦельРеквизиты"] = strPurposeDetails;


            var queryInspectorId = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                                     .Where(x => x.DocumentGji.Id == disposal.Id)
                                                     .Select(x => x.Inspector.Id);

            var listLocality = Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
                                .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                                .Select(x => x.ZonalInspection.Locality)
                                .Distinct()
                                .ToList();

            var zonalInspectionName = Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
                                .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                                .Select(x => x.ZonalInspection.Name)
                                .Distinct()
                                .ToList();

            reportParams.SimpleReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);
            reportParams.SimpleReportParams["НаименованиеОтдела"] = string.Join("; ", zonalInspectionName);


            FillRegionParams(reportParams, disposal);
        }
    }
}