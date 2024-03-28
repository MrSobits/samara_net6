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
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Акт проверки отчета по ГЖИ
    /// </summary>
    public class ActCheckGjiReport : GjiBaseReport
    {
        private long DocumentId { get; set; }
        /// <summary>
        /// Распоряжение ГЖИ
        /// </summary>
        protected Disposal ParentDisposal;
        /// <summary>
        /// Код шаблона
        /// </summary>
        protected override string CodeTemplate { get; set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        public ActCheckGjiReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ActSurvey_1))
        {
        }
        /// <summary>
        /// ID
        /// </summary>
        public override string Id
        {
            get { return "ActCheck"; }
        }
        /// <summary>
        /// Код
        /// </summary>
        public override string CodeForm
        {
            get { return "ActCheck"; }
        }
        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Акт проверки"; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        public override string Description
        {
            get { return "Акт проверки"; }
        }
        /// <summary>
        /// 
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
                                   Code = "BlockGJI_ActSurvey_4",
                                   Name = "ActSurvey",
                                   Description =
                                       "Акт проверки на 1 дом, код вида проверки 5 (Инспекционное обследование) и Тип обследования с кодом 22",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_4
                               },
                           new TemplateInfo
                               {
                                   Code = "BlockGJI_ActSurvey_2",
                                   Name = "ActSurvey",
                                   Description = "Акт проверки на 1 дом, код вида проверки 5 (Инспекционное обследование)",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_2
                               },
                           new TemplateInfo
                               {
                                   Code = "BlockGJI_ActSurvey_1",
                                   Name = "ActSurvey",
                                   Description = "Акт проверки на 1 дом, Любой вид проверки, кроме инспекционной",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_1
                               },
                           new TemplateInfo
                               {
                                   Code = "BlockGJI_ActSurvey_3",
                                   Name = "ActSurvey",
                                   Description = "Акт проверки общий, Основание проверки - Плановая проверки юр.лица",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_1
                               },
                           new TemplateInfo
                               {
                                   Code = "BlockGJI_ActSurvey_all",
                                   Name = "ActSurvey",
                                   Description = "Акт проверки общий",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_all
                               },
                           new TemplateInfo
                               {
                                   Code = "BlockGJI_ActSurvey_5",
                                   Name = "ActSurvey",
                                   Description = "Акт проверки на 1 дом, код вида проверки 5 (Инспекционное обследование) и Тип обследования с кодом 20",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_4
                               }

                       };
        }
        /// <summary>
        /// Подготовка данных для отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {

            var actCheckDomain = Container.Resolve<IDomainService<ActCheck>>();
            var disposalDomain = Container.Resolve<IDomainService<Disposal>>();
            var typeSurveyDomain = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var inspectionRoDomain = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var zonalInspectionMuDomain = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();
            var containerInspectorDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var contragentContactDomain = Container.Resolve<IDomainService<ContragentContact>>();
            var actRoDomain = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var typeSurveyGoalDomain = Container.Resolve<IDomainService<TypeSurveyGoalInspGji>>();
            var actAnnexDomain = Container.Resolve<IDomainService<ActCheckAnnex>>();
            var disposalExpertDomain = Container.Resolve<IDomainService<DisposalExpert>>();
            var disposalProvidedDocDomain = this.Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var typeSurveyInspDomain = Container.Resolve<IDomainService<TypeSurveyInspFoundationGji>>();
            var actCheckWitnessDomain = Container.Resolve<IDomainService<ActCheckWitness>>();
            var docChildrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var insappealCitsDomain = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var appealCitsRo = Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var zonalInsInspectorDomain = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var documentGjiDomain = Container.Resolve<IDomainService<DocumentGji>>();

            var act = actCheckDomain.Get(DocumentId);
            if (act == null)
            {
                throw new ReportProviderException("Не удалось получить акт проверки");
            }

            var disposal = disposalDomain.Load(GetParentDocument(act, TypeDocumentGji.Disposal).Id);

            var queryTypeSurveys = typeSurveyDomain
                                             .GetAll()
                                             .Where(x => x.Disposal.Id == disposal.Id);
            var typeSurveys = queryTypeSurveys.Select(x => x.TypeSurvey).ToArray();

            // зональную инспекцию получаем через муниципальное образование первого дома
            var firstRo = inspectionRoDomain.GetAll()
                .Where(x => x.Inspection.Id == act.Inspection.Id)
                .Select(x => x.RealityObject)
                .FirstOrDefault();

            if (firstRo != null)
            {
                var zonalInspection = zonalInspectionMuDomain.GetAll()
                    .Where(x => x.Municipality.Id == firstRo.Municipality.Id)
                    .Select(x => x.ZonalInspection)
                    .FirstOrDefault();

                if (zonalInspection != null)
                {
                    reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык"] = zonalInspection.BlankName;
                    reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык1"] = zonalInspection.Name;
                    reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык(ТворП)"] = zonalInspection.NameAblative;
                    reportParams.SimpleReportParams["ЗональноеНаименование2ГосЯзык"] = zonalInspection.BlankNameSecond;
                    reportParams.SimpleReportParams["Адрес1ГосЯзык"] = zonalInspection.Address;
                    reportParams.SimpleReportParams["Адрес2ГосЯзык"] = zonalInspection.AddressSecond;
                    reportParams.SimpleReportParams["Телефон"] = zonalInspection.Phone;
                    reportParams.SimpleReportParams["Email"] = zonalInspection.Email;
                    
                    reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык(РП)"] = zonalInspection.NameGenetive;
                    reportParams.SimpleReportParams["ЗональноеНаименование1ГосЯзык(ВП)"] = zonalInspection.NameAccusative;
                }
            }
            
            reportParams.SimpleReportParams["ДатаАкта"] = act.DocumentDate.HasValue 
                ? act.DocumentDate.Value.ToString("d MMMM yyyy") 
                : string.Empty;
            reportParams.SimpleReportParams["НомерАкта"] = act.DocumentNumber;
            reportParams.SimpleReportParams["ПроверПлощадь"] = act.Area.HasValue ? act.Area.Value.ToStr() : string.Empty;
            reportParams.SimpleReportParams["ВремяСоставленияАкта"] = act.ObjectCreateDate.ToShortTimeString();

            if (act.Inspection.Contragent != null)
            {
                var contragent = act.Inspection.Contragent;

                reportParams.SimpleReportParams["УправОрг"] = contragent.Name;
                reportParams.SimpleReportParams["ИНН"] = contragent.Inn;

                if (contragent.OrganizationForm != null)
                {
                    reportParams.SimpleReportParams["ТипЛица"] = contragent.OrganizationForm.Code != "91"
                                                                     ? "юридического лица"
                                                                     : "индивидуального предпринимателя";
                }

                reportParams.SimpleReportParams["УправОрг(РП)"] = contragent.Name;
                reportParams.SimpleReportParams["СокрУправОрг"] = contragent.ShortName;

                var headContragent = contragentContactDomain.GetAll()
                    .Where(x => x.Contragent.Id == contragent.Id)
                    .Where(x => x.DateStartWork.HasValue)
                    .Where(x => x.DateStartWork.Value <= DateTime.Now)
                    .Where(x => !x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)
                    .FirstOrDefault(x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

                if (headContragent != null)
                {
                    reportParams.SimpleReportParams["РуководительЮЛ"] = string.Format(
                        "{0} {1} {2} {3}",
                        headContragent.Position != null ? headContragent.Position.Name : string.Empty,
                        headContragent.Surname,
                        headContragent.Name,
                        headContragent.Patronymic);
                }

                reportParams.SimpleReportParams["АдресКонтрагентаФакт"] = contragent.FiasFactAddress.Return(x => x.AddressName);
            }

            FillSectionViolations(reportParams, act, disposal.Id);

            ParentDisposal = disposal;

            // Дома акта проверки
            var actCheckRealtyObjects = actRoDomain.GetAll()
                                   .Where(x => x.ActCheck.Id == DocumentId)
                                   .Where(x => x.RealityObject != null)
                                   .Select(x => new { x.RealityObject, x.HaveViolation, x.Description, x.NotRevealedViolations }).ToArray();

            if (actCheckRealtyObjects.All(x => x.HaveViolation == YesNoNotSet.No))
            {
                reportParams.SimpleReportParams["НеВыявлено"] = actCheckRealtyObjects
                    .Select(x => x.Description)
                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? "; " + y : y));
            }

            GetCodeTemplate(act, disposal, typeSurveys);

            var queryTypeSurveyIds = queryTypeSurveys.Select(x => x.TypeSurvey.Id);
            var strGoalInspGji = typeSurveyGoalDomain
                                         .GetAll()
                                         .Where(x => queryTypeSurveyIds.Contains(x.TypeSurvey.Id))
                                         .Select(x => x.SurveyPurpose.Name)
                                         .AsEnumerable()
                                         .Aggregate(string.Empty, (t, n) => t + (!string.IsNullOrEmpty(t) ? ("; " + n) : n));

            switch (CodeTemplate)
            {
                case "BlockGJI_ActSurvey_5":
                case "BlockGJI_ActSurvey_1":
                case "BlockGJI_ActSurvey_4":
                    reportParams.SimpleReportParams["ЦельОбследования"] = strGoalInspGji;
                    break;
            }

            reportParams.SimpleReportParams["Цель"] = strGoalInspGji;

            var actCheckAnnex = actAnnexDomain.GetAll()
                    .Where(x => x.ActCheck.Id == DocumentId)
                    .Select(x => x.Name)
                    .AsEnumerable()
                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

            reportParams.SimpleReportParams["ПрилагаемыеДокументы"] = actCheckAnnex;

            if (actCheckRealtyObjects.Length > 0)
            {
                var realtyObject = actCheckRealtyObjects.Select(x => x.RealityObject).First();
                if (realtyObject.FiasAddress != null)
                {
                    reportParams.SimpleReportParams["НомерДома"] = realtyObject.FiasAddress.House;
                    reportParams.SimpleReportParams["АдресДома"] = realtyObject.FiasAddress.PlaceName + ", " + realtyObject.FiasAddress.StreetName;
                    reportParams.SimpleReportParams["НаселенныйПункт"] = realtyObject.FiasAddress.PlaceName;
                }

                if (actCheckRealtyObjects.Length == 1)
                {
                    reportParams.SimpleReportParams["МатериалСтен"] = realtyObject.WallMaterial != null
                                                                          ? realtyObject.WallMaterial.Name
                                                                          : string.Empty;
                    reportParams.SimpleReportParams["ОбщаяПлощадь"] = realtyObject.AreaMkd.HasValue
                                                                          ? realtyObject.AreaMkd.Value.ToStr()
                                                                          : string.Empty;
                    reportParams.SimpleReportParams["Этажей"] = realtyObject.Floors.HasValue
                                                                          ? realtyObject.Floors.Value.ToStr()
                                                                          : string.Empty;
                    reportParams.SimpleReportParams["ГодСдачи"] = realtyObject.DateCommissioning.HasValue
                                                                          ? realtyObject.DateCommissioning.Value.ToShortDateString()
                                                                          : string.Empty;
                    reportParams.SimpleReportParams["Кровля"] = realtyObject.RoofingMaterial != null
                                                                          ? realtyObject.RoofingMaterial.Name
                                                                          : string.Empty;
                    reportParams.SimpleReportParams["Подвал"] = realtyObject.HavingBasement;
                    reportParams.SimpleReportParams["Секций"] = realtyObject.NumberEntrances.ToLong();

	                if (realtyObject.FiasAddress != null)
	                {
						var housing = !string.IsNullOrEmpty(realtyObject.FiasAddress.Housing) ?
										", корп. " + realtyObject.FiasAddress.Housing :
										string.Empty;

						reportParams.SimpleReportParams["УлицаДом1"] = string.Format("{0}, {1}, д.{2}{3} ", realtyObject.FiasAddress.PlaceName, realtyObject.FiasAddress.StreetName, realtyObject.FiasAddress.House, housing);
						reportParams.SimpleReportParams["ДомАдрес"] = realtyObject.FiasAddress.AddressName;
						reportParams.SimpleReportParams["ДомАдрес1"] = string.Format("{0}, {1}", realtyObject.FiasAddress.StreetName, realtyObject.FiasAddress.PlaceName);
	                }
                    
                    reportParams.SimpleReportParams["Описание"] = actCheckRealtyObjects[0].Description;
                }
                
                if (actCheckRealtyObjects.Length > 0)
                {
                    var realObjs = new StringBuilder();

                    foreach (var realityObject in actCheckRealtyObjects.Select(x => x.RealityObject))
                    {
                        if (realObjs.Length > 0)
                            realObjs.Append("; ");

	                    if (realtyObject.FiasAddress != null)
	                    {
							var housing = !string.IsNullOrEmpty(realtyObject.FiasAddress.Housing) ?
										  ", корп. " + realtyObject.FiasAddress.Housing :
										  string.Empty;

							realObjs.AppendFormat("{0}, д.{1}{2}", realityObject.FiasAddress.StreetName, realityObject.FiasAddress.House, housing);  
	                    }
                    }

                    reportParams.SimpleReportParams["УлицаДом"] = String.Format("{0}.", realObjs);

	                if (realtyObject.FiasAddress != null)
	                {
		                reportParams.SimpleReportParams["ДомаИАдреса"] = String.Format("{0}, {1}. ",
			                actCheckRealtyObjects.FirstOrDefault().RealityObject.FiasAddress.PlaceName, realObjs);
	                }
                }

                //reportParams.SimpleReportParams["ДомаИАдреса"] = actCheckRealtyObjects
                //    .Select(x => x.RealityObject.Address)
                //    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? "; " + y : y));

                reportParams.SimpleReportParams["НевыявленныеНарушения"] = string.Join(", ", actCheckRealtyObjects.Select(x => x.NotRevealedViolations).ToArray());
            }

            reportParams.SimpleReportParams["ОбщаяПлощадьСумма"] = actCheckRealtyObjects.Select(x => x.RealityObject.AreaMkd).Sum();

            if (disposal != null)
            {
                reportParams.SimpleReportParams["ДатаРаспоряжения"] = disposal.DocumentDate.HasValue
                                                                          ? disposal.DocumentDate.Value.ToString("d MMMM yyyy")
                                                                          : string.Empty;

                reportParams.SimpleReportParams["НомерРаспоряжения"] = disposal.DocumentNumber;

                var kindCheckName = disposal.KindCheck != null ? disposal.KindCheck.Name : "";
                reportParams.SimpleReportParams["ВидПроверки"] = kindCheckName;
                reportParams.SimpleReportParams["ВидОбследования"] = kindCheckName;

                if (disposal.IssuedDisposal != null)
                {
                    if (!String.IsNullOrEmpty(disposal.IssuedDisposal.PositionGenitive))
                    {
                        reportParams.SimpleReportParams["КодИнспектора(РодП)"] = disposal.IssuedDisposal.PositionGenitive.ToLower();
                        reportParams.SimpleReportParams["КодИнспектора(ТворП)"] = disposal.IssuedDisposal.PositionAblative.ToLower();
                    }
                    
                    reportParams.SimpleReportParams["Руководитель(РодП)"] = disposal.IssuedDisposal.FioGenitive;
                    reportParams.SimpleReportParams["Руководитель(ТворП)"] = disposal.IssuedDisposal.FioAblative;
                }
                
                reportParams.SimpleReportParams["ВидОбследования(РП)"] = GetTypeCheckAblative(disposal.KindCheck);
                if (disposal.DateStart.HasValue && disposal.DateEnd.HasValue)
                {
                    var startDate = disposal.DateStart;

                    int countDays = 0;

                    while (startDate.Value.Date != disposal.DateEnd.Value.Date)
                    {
                        if (startDate.Value.DayOfWeek != DayOfWeek.Sunday && startDate.Value.DayOfWeek != DayOfWeek.Saturday)
                        {
                            countDays++;
                        }

                        startDate = startDate.Value.AddDays(1);
                    }

                    reportParams.SimpleReportParams["ПродолжительностьПроверки"] = countDays;
                }

                reportParams.SimpleReportParams["ПродолжительностьПроверкиТюмень"] = act.DocumentDate > disposal.DateStart ? (act.DocumentDate - disposal.DateStart).Value.Days : 0;

                reportParams.SimpleReportParams["Эксперты"] = disposalExpertDomain.GetAll()
                        .Where(x => x.Disposal.Id == disposal.Id)
                        .Select(x => x.Expert)
                        .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Name : y.Name));

                if (disposal.IssuedDisposal != null)
                {
                    reportParams.SimpleReportParams["КодРуководителяФИО"] = string.Format(
                        "{0} - {1}",
                        disposal.IssuedDisposal.Code,
                        string.IsNullOrEmpty(disposal.IssuedDisposal.ShortFio) ? disposal.IssuedDisposal.Fio : disposal.IssuedDisposal.ShortFio);
                }

                reportParams.SimpleReportParams["НачалоПериода"] = disposal.DateStart.HasValue ? disposal.DateStart.Value.ToShortDateString() : string.Empty;
                reportParams.SimpleReportParams["ОкончаниеПериода"] = disposal.DateEnd.HasValue ? disposal.DateEnd.Value.ToShortDateString() : string.Empty;

                var disposalProvidedDocs = disposalProvidedDocDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => x.ProvidedDoc.Name)
                    .AsEnumerable()
                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                reportParams.SimpleReportParams["ПредоставляемыеДокументы"] = disposalProvidedDocs;

                if (disposal.Inspection.TypeBase == TypeBase.DisposalHead)
                {
                    reportParams.SimpleReportParams["ПравовоеОснованиеПроверки"] =
                        string.Format("на основании ст.20 Жилищного кодекса РФ, приказа начальника ГЖИ РТ № {0} от {1}",
                            reportParams.SimpleReportParams["НомерРаспоряжения"],
                            reportParams.SimpleReportParams["ДатаРаспоряжения"]);
                }

                var inspFounds = typeSurveyInspDomain.GetAll()
                                            .Where(x => queryTypeSurveyIds.Contains(x.TypeSurvey.Id))
                                            .Select(x => new { x.NormativeDoc.Name, x.Code })
                                            .OrderBy(x => x.Code)
                                            .ToArray()
                                            .Select(x => x.Name)
                                            .Distinct()
                                            .ToList();

                reportParams.SimpleReportParams["ПравовоеОснование"] = inspFounds.FirstOrDefault();
            }

            var inspectors = containerInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == DocumentId)
                .Select(x => new
                {
                    x.Inspector.Id,
                    x.Inspector.Fio,
                    x.Inspector.Position,
                    x.Inspector.ShortFio,
                    x.Inspector.FioAblative
                })
                .ToArray();

            reportParams.SimpleReportParams["ИнспекторыИКоды"] = inspectors
                .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Fio + " - " + y.Position : y.Fio + " - " + y.Position));

            reportParams.SimpleReportParams["Инспектор"] = string.Join(", ", inspectors.Select(x => x.Fio).ToArray());

            reportParams.SimpleReportParams["ЧислоЛицПроводившихПроверку"] = inspectors.Length > 1 ? "Лица" : "Лицо";

            var firstInspector = inspectors.FirstOrDefault();
            if (firstInspector != null && !string.IsNullOrEmpty(firstInspector.ShortFio))
            {
                reportParams.SimpleReportParams["ИнспекторФамИО"] = firstInspector.ShortFio;
                reportParams.SimpleReportParams["ИнспекторФамИОТП"] = !string.IsNullOrEmpty(firstInspector.FioAblative) ?
                    string.Format("{0} {1}", firstInspector.FioAblative.Split(' ')[0], firstInspector.ShortFio.Substring(firstInspector.ShortFio.IndexOf(' '))) : string.Empty;
            }

            var witness = actCheckWitnessDomain.GetAll()
                   .Where(x => x.ActCheck.Id == DocumentId)
                   .Select(x => new { x.Fio, x.Position, x.IsFamiliar })
                   .ToArray();

            var allWitness = witness
                .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Position + " - " + y.Fio : y.Position + " - " + y.Fio));
            var witnessIsFamiliar = witness
                .Where(x => x.IsFamiliar)
                .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Fio : y.Fio));

            reportParams.SimpleReportParams["ДЛприПроверке"] = allWitness;
            reportParams.SimpleReportParams["Ознакомлен"] = !string.IsNullOrEmpty(witnessIsFamiliar)
                                                                ? witnessIsFamiliar.TrimEnd(new[] { ',', ' ' })
                                                                : string.Empty;
            reportParams.SimpleReportParams["ЧислоЛицПривлеченныеЛица"] = witness.Length > 0 ? "Лица, привлеченные" : "Лицо, привлеченное";

            var docs = documentGjiDomain.GetAll()
                .Where(x => x.Stage.Parent.Id == act.Stage.Parent.Id)
                .Where(x => x.TypeDocumentGji == TypeDocumentGji.Protocol || x.TypeDocumentGji == TypeDocumentGji.Prescription)
                .ToList();

            var docPrescriptionNames = new List<string>();
            var docProtocolNames = new List<string>();
            foreach (var doc in docs)
            {
                var date = doc.DocumentDate.HasValue ? doc.DocumentDate.Value.ToShortDateString() : string.Empty;
                switch (doc.TypeDocumentGji)
                {
                    case TypeDocumentGji.Prescription:
                        docPrescriptionNames.Add(string.Format("№{0} от {1}г.", doc.DocumentNumber, date));
                        break;
                    case TypeDocumentGji.Protocol:
                        docProtocolNames.Add(string.Format("составлен №{0} от {1}г.", doc.DocumentNumber, date));
                        break;
                }
            }

            reportParams.SimpleReportParams["РеквизитыПредписаний"] = docPrescriptionNames.Aggregate("", (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));
            reportParams.SimpleReportParams["РеквизитыПротоколов"] = docProtocolNames.Count > 0
                ? docProtocolNames.Aggregate("", (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y))
                : "не составлен";

            FillCommonFields(reportParams, act);
            FillViolations(reportParams, docChildrenDomain);
            FillCheckPeriods(reportParams, act);

            if (act.TypeActCheck == TypeActCheckGji.ActCheckDocumentGji)
            {
                var actRemovals = docChildrenDomain.GetAll()
                    .Where(x => x.Parent.Id == act.Id && x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                    .Select(x => new { x.Children.DocumentDate, x.Children.DocumentNumber })
                    .ToArray();

                var actRemovalName = new StringBuilder();

                foreach (var actRem in actRemovals)
                {
                    if (actRemovalName.Length > 0)
                    {
                        actRemovalName.Append(", ");
                    }

                    actRemovalName.AppendFormat(
                        "№{0} от {1}",
                        actRem.DocumentNumber,
                        actRem.DocumentDate.HasValue ? actRem.DocumentDate.Value.ToShortDateString() : string.Empty);
                }

                reportParams.SimpleReportParams["РеквизитыАктовПровИспПредписания"] = actRemovalName.ToString();
            }

            var quryAppeals = insappealCitsDomain.GetAll()
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

            var appealsRo = appealCitsRo
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
            if (act.Inspection.TypeBase == TypeBase.CitizenStatement && disposal.TypeDisposal != TypeDisposalGji.DocumentGji)
            {
                strPurposeDetails = reportParams.SimpleReportParams["РеквизитыОбращения"].ToString();
            }
            else
            {
                strPurposeDetails = reportParams.SimpleReportParams.ПараметрСуществуетВСписке("ДомаИАдреса") ? reportParams.SimpleReportParams["ДомаИАдреса"].ToString() : string.Empty;
            }

            reportParams.SimpleReportParams["ЦельРеквизиты"] = strPurposeDetails;

            var queryInspectorId = containerInspectorDomain.GetAll()
                                                     .Where(x => x.DocumentGji.Id == disposal.Id)
                                                     .Select(x => x.Inspector.Id);

            var listLocality = zonalInsInspectorDomain.GetAll()
                                .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                                .Select(x => x.ZonalInspection.Locality)
                                .Distinct()
                                .ToList();

            reportParams.SimpleReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);            

            this.FillRegionParams(reportParams, act);

            Container.Release(actCheckDomain);
            Container.Release(disposalDomain);
            Container.Release(typeSurveyDomain);
            Container.Release(typeSurveyDomain);
            Container.Release(inspectionRoDomain);
            Container.Release(zonalInspectionMuDomain);
            Container.Release(containerInspectorDomain);
            Container.Release(contragentContactDomain);
            Container.Release(actRoDomain);
            Container.Release(typeSurveyGoalDomain);
            Container.Release(actAnnexDomain);
            Container.Release(disposalExpertDomain);
            Container.Release(disposalProvidedDocDomain);
            Container.Release(typeSurveyInspDomain);
            Container.Release(actCheckWitnessDomain);
            Container.Release(docChildrenDomain);
            Container.Release(insappealCitsDomain);
            Container.Release(appealCitsRo);
            Container.Release(zonalInsInspectorDomain);

        }

        private void FillViolations(ReportParams reportParams, IDomainService<DocumentGjiChildren> serviceDocumentGjiChildren)
        {

            var prescriptionDomain = Container.Resolve<IDomainService<Prescription>>();
            var prescriptionGjiViolationDomain = Container.Resolve<IDomainService<PrescriptionViol>>();

            var prescriptionId = serviceDocumentGjiChildren.GetAll()
                  .Where(x => x.Parent.Id == DocumentId && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                  .Select(x => x.Parent.Id)
                  .FirstOrDefault();

            var prescription = prescriptionDomain.Load(prescriptionId);
            if (prescription == null)
            {
                return;
            }

            var sectionViolations = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНарушений");


            var queryInspViols = prescriptionGjiViolationDomain.GetAll().Where(x => x.Document.Id == prescription.Id);
            var inspViols = queryInspViols
                .Select(x => new
                            {
                                x.InspectionViolation.Violation,
                                inspId = x.InspectionViolation.Id,
                                x.Action,
                                x.InspectionViolation.DateFactRemoval,
                                CodePIN = x.InspectionViolation.Violation.CodePin
                            })
                .ToArray();

            int i = 1;
            foreach (var inspViol in inspViols)
            {
                sectionViolations.ДобавитьСтроку();
                sectionViolations["Номер1"] = i++;
                sectionViolations["Пункт"] = inspViol.CodePIN;
                sectionViolations["ТекстНарушения"] = inspViol.Violation.Name;
                sectionViolations["Мероприятие"] = inspViol.Action;
                sectionViolations["СрокУстранения"] = inspViol.DateFactRemoval.HasValue ? inspViol.DateFactRemoval.Value.ToShortDateString() : string.Empty;
            }
           
            var viols = inspViols
                .Select(x => string.Format("{0} {1}", x.CodePIN, x.Violation.Name))
                .Distinct()
                .Aggregate(string.Empty, (current, viol) => current + (!string.IsNullOrEmpty(current) ? string.Format(", {0}", viol) : viol));

            reportParams.SimpleReportParams["Нарушения"] = viols;

            Container.Release(prescriptionDomain);
            Container.Release(prescriptionGjiViolationDomain);
        }

        protected virtual void FillCheckPeriods(ReportParams reportParams, DocumentGji doc)
        {

            var actPeriodDomain = Container.Resolve<IDomainService<ActCheckPeriod>>();

            var periodsCheck = actPeriodDomain.GetAll()
                .Where(x => x.ActCheck.Id == doc.Id && x.DateStart.HasValue && x.DateEnd.HasValue)
                .ToArray();

            var dc1 = new StringBuilder();
            var dc2 = new StringBuilder();
            var startInspectionDate = new StringBuilder();
            var startInspectionTime = new StringBuilder();
            var endInspectionTime = new StringBuilder();

            foreach (var actCheckGjiPeriod in periodsCheck)
            {
                if (dc1.Length > 0)
                {
                    dc1.Append(" ## ");
                }

                var st = actCheckGjiPeriod.DateStart.Value.TimeOfDay;
                var et = actCheckGjiPeriod.DateEnd.Value.TimeOfDay;

                var checkLength = et - st;

                var date1 = actCheckGjiPeriod.DateCheck.HasValue ? actCheckGjiPeriod.DateCheck.Value.ToString("D", new CultureInfo("ru-RU")) : string.Empty;

                dc1.AppendFormat(
                    "{0} с {1} час. {2} мин. до {3} час. {4} мин. Продолжительность: {5} час. {6} мин.",
                    date1,
                    st.Hours, 
                                 st.Minutes,
                                 et.Hours,
                                 et.Minutes, 
                                 checkLength.Hours,
                                 checkLength.Minutes);
                dc2.AppendFormat(
                    "{0} с {1} час. {2} мин. до {3} час. {4} мин. Продолжительность: {5} час. {6} мин.\n",
                    date1,
                    st.Hours,
                                 st.Minutes,
                                 et.Hours,
                                 et.Minutes,
                                 checkLength.Hours,
                                 checkLength.Minutes);

                if (actCheckGjiPeriod.DateCheck.HasValue)
                {
                    startInspectionDate.Append(actCheckGjiPeriod.DateCheck.Value.ToString("dd MMMM yyyy"));
                }

                startInspectionTime.Append(actCheckGjiPeriod.DateStart.Value.Hour + " ч." + actCheckGjiPeriod.DateStart.Value.Minute + " мин.");
                endInspectionTime.Append(actCheckGjiPeriod.DateEnd.Value.Hour + " ч." + actCheckGjiPeriod.DateEnd.Value.Minute + " мин.");
            }

            reportParams.SimpleReportParams["ДатаВремяПроверки"] = dc1.ToString();
            reportParams.SimpleReportParams["ДатаВремяПроверкиПеренос"] = dc2.ToString();
            reportParams.SimpleReportParams["ДатаПроверкиНачало"] = startInspectionDate.ToString();
            reportParams.SimpleReportParams["ВремяНачала"] = startInspectionTime.ToString();
            reportParams.SimpleReportParams["ВремяОкончания"] = endInspectionTime.ToString();


            Container.Release(actPeriodDomain);
        }
        /// <summary>
        /// Получение шаблона
        /// </summary>
        /// <param name="act"></param>
        /// <param name="disposal"></param>
        /// <param name="types"></param>
        protected virtual void GetCodeTemplate(ActCheck act, Disposal disposal, TypeSurveyGji[] types)
        {
            CodeTemplate = "BlockGJI_ActSurvey_1";

            // акт проверки на 1 дом
            if (act.TypeActCheck == TypeActCheckGji.ActCheckIndividual)
            {
                if (disposal.KindCheck != null)
                {
                    if (disposal.KindCheck.Code == TypeCheck.InspectionSurvey)
                    {
                        if (types.Any(x => x.Code == "22"))
                        {
                            CodeTemplate = "BlockGJI_ActSurvey_4";
                            return;
                        }

                        if (types.Any(x => x.Code == "20"))
                        {
                            CodeTemplate = "BlockGJI_ActSurvey_5";
                            return;
                        }

                        CodeTemplate = "BlockGJI_ActSurvey_2";
                        return;
                    }
                }

                CodeTemplate = "BlockGJI_ActSurvey_1";
                return;
            }

            // акт проверки общий
            if (act.TypeActCheck == TypeActCheckGji.ActCheckGeneral)
            {
                if (act.Inspection.TypeBase == TypeBase.PlanJuridicalPerson)
                {
                    CodeTemplate = "BlockGJI_ActSurvey_3";
                    return;
                }

                CodeTemplate = "BlockGJI_ActSurvey_all";
            }              
        }

        private string GetTypeCheckAblative(KindCheckGji kindCheck)
        {
            var result = "";

            var dictTypeCheckAblative = new Dictionary<TypeCheck, string>()
                                            {
                                                { TypeCheck.PlannedExit, "плановой выездной"},
                                                { TypeCheck.NotPlannedExit, "внеплановой выездной"},
                                                { TypeCheck.PlannedDocumentation, "плановой документарной" },
                                                { TypeCheck.NotPlannedDocumentation, "внеплановой документарной" },
                                                { TypeCheck.InspectionSurvey, "внеплановой выездной" },
                                                { TypeCheck.PlannedDocumentationExit, "плановой документарной и выездной" },
                                                { TypeCheck.VisualSurvey, "о внеплановой проверке технического состояния жилого помещения" },
                                                { TypeCheck.NotPlannedDocumentationExit, "внеплановой документарной и выездной" },
                                                { TypeCheck.PlannedInspectionVisit, "о плановом инспекционном визите" },
                                                { TypeCheck.NotPlannedInspectionVisit, "о внеплановом инспекционном визите" }
                                            };
            if (kindCheck != null)
            {
                result = dictTypeCheckAblative[kindCheck.Code];
            }

            return result;
        }
        /// <summary>
        /// Заполнение секции нарушения
        /// </summary>
        /// <param name="reportParams"></param>
        /// <param name="act"></param>
        /// <param name="disposalId"></param>
        protected virtual void FillSectionViolations(ReportParams reportParams, ActCheck act, long disposalId = 0)
        {
            var actCheckViolationDomain = Container.Resolve<IDomainService<ActCheckViolation>>();

            var serviceActCheckViolation = actCheckViolationDomain.GetAll();
            var queryActViols = serviceActCheckViolation.Where(x => x.Document.Id == act.Id);
            var actViols = queryActViols.Select(x => x.InspectionViolation.Violation)
                             .ToArray();

            #region Секция нарушений

            if (actViols.Length > 0)
            {
                var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНарушений");

                var addedViols = new HashSet<long>();

                int i = 1;
                foreach (var viol in actViols.Where(viol => !addedViols.Contains(viol.Id)))
                {
                    addedViols.Add(viol.Id);

                    section.ДобавитьСтроку();
                    section["Номер1"] = i++;
                    section["Пункт"] = viol.CodePin;
                    section["ТекстНарушения"] = viol.Name;
                }
            }

            #endregion

            Container.Release(actCheckViolationDomain);
        }
    }
}