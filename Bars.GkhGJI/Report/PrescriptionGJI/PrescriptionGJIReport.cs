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
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Entities.Dict;

    public class PrescriptionGjiReport : GjiBaseReport
    {
        private long DocumentId { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomainService { get; set; }

        public IDomainService<ActCheckWitness> ActCheckWitnessDomainService { get; set; }

        private Dictionary<long, string> floorsWords = new Dictionary<long, string>()
                                                          {
            {1, "одноэтажный"},
            {2, "двухэтажный"},
            {3, "трехэтажный"},
            {4, "четырехэтажный"},
            {5, "пятиэтажный"},
            {6, "шестиэтажный"},
            {7, "семиэтажный"},
            {8, "восьмиэтажный"},
            {9, "девятиэтажный"},
            {10, "десятиэтажный"},
            {11, "одиннадцатиэтажный"},
            {12, "двенадцатиэтажный"},
            {13, "тринадцатиэтажный"},
            {14, "четырнадцатиэтажный"},
            {15, "пятнадцатиэтажный"},
            {16, "шестнадцатиэтажный"},
            {17, "семнадцатиэтажный"},
            {18, "восемнадцатиэтажный"},
            {19, "девятнадцатиэтажный"},
            {20, "двадцатиэтажный"},
            {21, "двадцатиодноэтажный"},
            {22, "двадцатидвухэтажный"},
            {23, "двадцатитрехэтажный"},
            {24, "двадцатичетырехэтажный"},
            {25, "двадцатипятиэтажный}"}
        };

        /// <inheritdoc />
        protected override string CodeTemplate { get; set; }

        /// <inheritdoc />
        public PrescriptionGjiReport() 
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ExecutiveDocPrescription))
        {
        }

        /// <inheritdoc />
        public override string Id => "Prescription";

        /// <inheritdoc />
        public override string CodeForm => "Prescription";

        /// <inheritdoc />
        public override string Name => "Предписание";

        /// <inheritdoc />
        public override string Description => "Предписание";

        /// <inheritdoc />
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        /// <inheritdoc />
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "PrescriptionGJI",
                    Code = "BlockGJI_ExecutiveDocPrescription_1",
                    Description = "Тип обследования с кодом 22",
                    Template = Properties.Resources.BlockGJI_ExecutiveDocPrescription_1
                },
                new TemplateInfo
                {
                    Name = "PrescriptionGJI",
                    Code = "BlockGJI_ExecutiveDocPrescription",
                    Description = "Любой другой случай",
                    Template = Properties.Resources.BlockGJI_ExecutiveDocPrescription
                }
            };
        }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            var actCheckDomain = this.Container.Resolve<IDomainService<ActCheck>>();
            var actCheckInspectedPartDomain = this.Container.Resolve<IDomainService<ActCheckInspectedPart>>();
            var disposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var disposalTypeSurveyDomain = this.Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var documentGjiChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var inspectionAppealCitsDomain = this.Container.Resolve<IDomainService<InspectionAppealCits>>();
            var inspectionGjiRealityObjectDomain = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var manOrgContractRealityObjectDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var prescriptionDomain = this.Container.Resolve<IDomainService<Prescription>>();
            var prescriptionViolDomain = this.Container.Resolve<IDomainService<PrescriptionViol>>();
            var violationNormativeDocItemGjiDomain = this.Container.Resolve<IDomainService<ViolationNormativeDocItemGji>>();
            var zonalInspectionInspectorDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();

            try
            {
                var prescription = prescriptionDomain.Load(this.DocumentId);

                if (prescription == null)
                {
                    throw new ReportProviderException("Не удалось получить предписание");
                }

                // Заполняем общие поля
                this.FillCommonFields(reportParams, prescription);

                this.CodeTemplate = "BlockGJI_ExecutiveDocPrescription";

                var baseDisposal = this.GetParentDocument(prescription, TypeDocumentGji.Disposal);

                if (baseDisposal != null)
                {
                    var typeSurveys = disposalTypeSurveyDomain.GetAll()
                        .Where(x => x.Disposal.Id == baseDisposal.Id)
                        .Select(x => x.TypeSurvey)
                        .ToList();

                    reportParams.SimpleReportParams["ПричинаОбследования"] = 
                        typeSurveys.Aggregate("", (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Name : y.Name));

                    GetTemplateCode(typeSurveys);

                    var issuedDisposal = disposalDomain.GetAll().Where(x => x.Id == baseDisposal.Id).Select(x => x.IssuedDisposal).FirstOrDefault();

                    if (issuedDisposal != null)
                    {
                        reportParams.SimpleReportParams["РуководительФИО"] =
                            $"{issuedDisposal.Position} - {(string.IsNullOrEmpty(issuedDisposal.ShortFio) ? issuedDisposal.Fio : issuedDisposal.ShortFio)}";
                    }

                    var queryInspectorId = this.DocumentGjiInspectorDomainService.GetAll()
                        .Where(x => x.DocumentGji.Id == baseDisposal.Id)
                        .Select(x => x.Inspector.Id);

                    var listLocality = zonalInspectionInspectorDomain.GetAll()
                        .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                        .Select(x => x.ZonalInspection.Locality)
                        .Distinct()
                        .ToList();

                    reportParams.SimpleReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);
                }

                var actDoc = documentGjiChildrenDomain.GetAll()
                    .Where(x => x.Children.Id == prescription.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Select(x => x.Parent)
                    .FirstOrDefault();

                if (actDoc != null)
                {
                    var act = actCheckDomain.GetAll().FirstOrDefault(x => x.Id == actDoc.Id);

                    reportParams.SimpleReportParams["Кв"] = act?.Flat;

                    reportParams.SimpleReportParams["ИнспектируемаяЧасть"] = actCheckInspectedPartDomain.GetAll()
                        .Where(x => x.ActCheck.Id == act.Id)
                        .Select(x => x.InspectedPart)
                        .Aggregate("", (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Name : y.Name));

                    reportParams.SimpleReportParams["НомерАктаПроверки"] = actDoc.DocumentNumber;
                    reportParams.SimpleReportParams["ДатаАктаПроверки"] = actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToShortDateString() : string.Empty;

                    var firstInspector = this.DocumentGjiInspectorDomainService.GetAll()
                        .Where(x => x.DocumentGji.Id == DocumentId)
                        .Select(x => new
                        {
                            x.Inspector.ShortFio,
                            x.Inspector.FioAblative
                        })
                        .FirstOrDefault();

                    if (firstInspector != null && !string.IsNullOrEmpty(firstInspector.ShortFio))
                    {
                        reportParams.SimpleReportParams["ИнспекторФамИО"] = firstInspector.ShortFio;
                        reportParams.SimpleReportParams["ИнспекторФамИОТП"] =
                            $"{firstInspector.FioAblative.Split(' ')[0]} {firstInspector.ShortFio.Substring(firstInspector.ShortFio.IndexOf(' '))}";
                    }

                    reportParams.SimpleReportParams["ДатаАкта"] = act.DocumentDate.ToDateTime().ToString("d MMMM yyyy");
                    reportParams.SimpleReportParams["НомерАкта"] = act.DocumentNumber;

                    var firstWitness = this.ActCheckWitnessDomainService.GetAll().Where(x => x.ActCheck.Id == actDoc.Id)
                        .Select(x => new { x.Fio, x.Position })
                        .FirstOrDefault();

                    reportParams.SimpleReportParams["Вприсутствии"] = firstWitness != null
                        ? $"{firstWitness.Fio} - {firstWitness.Position}"
                        : string.Empty;
                }

                var disposal = disposalDomain.GetAll()
                    .FirstOrDefault(x => x.Stage.Id == prescription.Stage.Parent.Id);

                if (disposal != null)
                {
                    reportParams.SimpleReportParams["ДатаРаспоряжения"] = disposal.DocumentDate.ToDateTime().ToString("d MMMM yyyy");
                    reportParams.SimpleReportParams["НомерРаспоряжения"] = disposal.DocumentNumber;
                }

                reportParams.SimpleReportParams["ДатаПредписания"] = prescription.DocumentDate.HasValue
                    ? prescription.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                    : string.Empty;
                reportParams.SimpleReportParams["НомерПредписания"] = prescription.DocumentNumber;

                var realityObj = prescriptionViolDomain.GetAll()
                    .Where(x => x.Document.Id == prescription.Id)
                    .Select(x => x.InspectionViolation.RealityObject)
                    .ToList()
                    .Distinct(x => x.Id);

                // Если дом один, то выводим адрес дома и номер дома, если домов нет или больше 1 - ничего не выводим
                if (realityObj.Count() == 1)
                {
                    var firstPrescriptionRo = realityObj.FirstOrDefault();

                    if (firstPrescriptionRo != null)
                    {
                        reportParams.SimpleReportParams["Район"] = firstPrescriptionRo.Municipality != null
                            ? firstPrescriptionRo.Municipality.Name
                            : "";
                        reportParams.SimpleReportParams["Стена"] = firstPrescriptionRo.WallMaterial?.Name ?? string.Empty;
                        reportParams.SimpleReportParams["Кровля"] = firstPrescriptionRo.RoofingMaterial?.Name ?? string.Empty;
                        reportParams.SimpleReportParams["Этажей"] = firstPrescriptionRo.MaximumFloors;
                        reportParams.SimpleReportParams["Квартир"] = firstPrescriptionRo.NumberApartments;
                        reportParams.SimpleReportParams["ОбщаяПлощадь"] = firstPrescriptionRo.AreaMkd;
                        reportParams.SimpleReportParams["ГодЭксп"] = firstPrescriptionRo.DateCommissioning.HasValue
                            ? firstPrescriptionRo.DateCommissioning.Value.ToShortDateString()
                            : "";
                        reportParams.SimpleReportParams["Подвал"] = firstPrescriptionRo.HavingBasement;

                        reportParams.SimpleReportParams["ОбследПл"] = firstPrescriptionRo.AreaMkd.HasValue
                            ? firstPrescriptionRo.AreaMkd.Value.RoundDecimal(2).ToStr()
                            : "";

                        if (firstPrescriptionRo.FiasAddress != null)
                        {
                            var fias = firstPrescriptionRo.FiasAddress;

                            reportParams.SimpleReportParams["АдресДома"] = fias.PlaceName + ", " + fias.StreetName;
                            reportParams.SimpleReportParams["НаселенныйПункт"] = fias.PlaceName;
                            reportParams.SimpleReportParams["НомерДома"] = fias.House;
                            reportParams.SimpleReportParams["Корпус"] = fias.Housing;
                            reportParams.SimpleReportParams["Секций"] = firstPrescriptionRo.NumberEntrances;
                            reportParams.SimpleReportParams["Улица"] = fias.StreetName;
                        }

                        if (firstPrescriptionRo.MaximumFloors != null)
                        {
                            var floor = firstPrescriptionRo.MaximumFloors.ToLong();
                            if (this.floorsWords.ContainsKey(floor))
                            {
                                reportParams.SimpleReportParams["ЭтажейПропис"] = this.floorsWords[floor];
                            }
                        }

                    }
                }

                var contragent = prescription.Contragent;

                if (contragent != null)
                {
                    reportParams.SimpleReportParams["ИНН"] = contragent.Inn;
                    reportParams.SimpleReportParams["КПП"] = contragent.Kpp;

                    reportParams.SimpleReportParams["АдресКонтрагента"] = contragent.FiasJuridicalAddress.IsNotNull()
                        ? contragent.FiasJuridicalAddress.AddressName.Replace("р-н.", "район")
                        : contragent.JuridicalAddress;
                }

                if (prescription.Inspection.TypeBase == TypeBase.CitizenStatement)
                {
                    reportParams.SimpleReportParams["ПричинаОбследования"] = "По жалобе";
                }

                if (prescription.Executant.IsNotNull())
                {
                    var code = prescription.Executant.Code;

                    if (new[] { "0", "4", "8", "10", "11", "15", "18" }.Contains(code))
                    {
                        reportParams.SimpleReportParams["КомуВыдано"] = prescription.Contragent?.Name;
                    }

                    if (new[] { "1", "5", "9", "12", "13", "16", "19" }.Contains(code))
                    {
                        var result = prescription.Contragent?.Name;
                        var delimiter = !string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(prescription.PhysicalPerson) ? ", " : string.Empty;
                        reportParams.SimpleReportParams["КомуВыдано"] = result + delimiter + prescription.PhysicalPerson;
                    }

                    if (new[] { "0", "4", "8", "9", "11", "15", "18" }.Contains(code))
                    {
                        reportParams.SimpleReportParams["КомуВыдано1"] = prescription.Contragent?.Name;
                    }

                    if (new[] { "1", "5", "10", "12", "13", "16", "19" }.Contains(code))
                    {
                        var contName = prescription.Contragent?.Name;
                        var delimiter = !string.IsNullOrEmpty(contName) && !string.IsNullOrEmpty(prescription.PhysicalPerson) ? " - " : string.Empty;
                        reportParams.SimpleReportParams["КомуВыданоДЛ"] = prescription.PhysicalPerson + delimiter + contName;
                    }

                    if (new[] { "6", "7", "14" }.Contains(code))
                    {
                        reportParams.SimpleReportParams["КомуВыдано"] = prescription.PhysicalPerson;
                        reportParams.SimpleReportParams["КомуВыдано1"] = prescription.PhysicalPerson;
                        reportParams.SimpleReportParams["КомуВыданоДЛ"] = prescription.PhysicalPerson;
                    }
                }

                reportParams.SimpleReportParams["Реквизиты"] = prescription.PhysicalPersonInfo;

                string typeControl = null;
                var firstRo = realityObj.FirstOrDefault();

                if (firstRo != null)
                {
                    var date = prescription.DocumentDate ?? DateTime.Now.Date;

                    var typeCurrentContract = manOrgContractRealityObjectDomain.GetAll()
                        .Where(x => x.RealityObject.Id == firstRo.Id)
                        .Where(x => x.ManOrgContract.StartDate <= date)
                        .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= date)
                        .OrderByDescending(x => x.ManOrgContract.EndDate)
                        .Select(x => new
                        {
                            x.ManOrgContract.TypeContractManOrgRealObj,
                            TypeManagement = (TypeManagementManOrg?) x.ManOrgContract.ManagingOrganization.TypeManagement
                        })
                        .FirstOrDefault();

                    if (typeCurrentContract != null)
                    {
                        if (typeCurrentContract.TypeManagement.HasValue)
                        {
                            switch (typeCurrentContract.TypeManagement.Value)
                            {
                                case TypeManagementManOrg.TSJ:
                                    typeControl = "2";
                                    break;
                                case TypeManagementManOrg.UK:
                                    typeControl = "3";
                                    break;
                            }
                        }
                        else
                        {
                            switch (typeCurrentContract.TypeContractManOrgRealObj)
                            {
                                case TypeContractManOrg.DirectManag:
                                    typeControl = "1";
                                    break;
                                case TypeContractManOrg.JskTsj:
                                    typeControl = "2";
                                    break;
                                //Ук
                                case TypeContractManOrg.ManagingOrgJskTsj:
                                case TypeContractManOrg.ManagingOrgOwners:
                                    typeControl = "3";
                                    break;
                            }
                        }
                    }
                }

                reportParams.SimpleReportParams["СпособУправления"] = typeControl;

                #region Секция нарушений

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНарушений");

                var violations = prescriptionViolDomain.GetAll()
                    .Where(x => x.Document.Id == prescription.Id)
                    .Select(x => new
                    {
                        ViolationId = x.InspectionViolation.Violation.Id,
                        ViolationCodePin = x.InspectionViolation.Violation.CodePin,
                        ViolationName = x.InspectionViolation.Violation.Name,
                        x.Action,
                        x.Description,
                        DatePlanRemoval = x.DatePlanRemoval ?? x.InspectionViolation.DatePlanRemoval,
                        x.InspectionViolation.Violation.PpRf170,
                        x.InspectionViolation.Violation.PpRf25,
                        x.InspectionViolation.Violation.PpRf307,
                        x.InspectionViolation.Violation.PpRf491,
                        x.InspectionViolation.Violation.OtherNormativeDocs
                    })
                    .AsEnumerable()
                    .Distinct()
                    .ToList();

                var violIds = violations.Select(x => x.ViolationId);

                var npaDict = violationNormativeDocItemGjiDomain.GetAll()
                    .Where(x => violIds.Contains(x.ViolationGji.Id))
                    .GroupBy(x => x.ViolationGji.Id)
                    .ToDictionary(x => x.Key, y => string.Join(", ", y.Select(x => x.NormativeDocItem.NormativeDoc.FullName)));

                reportParams.SimpleReportParams["ВыявленыНарушения"] = string.Join(",\r\n", violations
                    .Select(x =>
                    {
                        var npaNames = npaDict.TryGetValue(x.ViolationId, out var names) ? $" - {names}" : string.Empty;
                        return $"{x.ViolationName}{npaNames}";
                    }));

                var strCodePin = new StringBuilder();

                var i = 0;
                var addedList = new List<long>();
                foreach (var viol in violations.Where(viol => !addedList.Contains(viol.ViolationId)))
                {
                    addedList.Add(viol.ViolationId);

                    section.ДобавитьСтроку();
                    section["Номер1"] = ++i;
                    section["Пункт"] = viol.ViolationCodePin;
                    section["ТекстНарушения"] = viol.ViolationName;
                    section["Мероприятие"] = viol.Action;
                    section["Мероприятия"] = viol.Action;
                    section["Подробнее"] = viol.Description;
                    var strDateRemoval = viol.DatePlanRemoval.HasValue
                        ? viol.DatePlanRemoval.Value.ToShortDateString()
                        : string.Empty;

                    section["СрокУстранения"] = strDateRemoval;
                    section["СрокиИсполнения"] = strDateRemoval;
                    section["СрокИсполненияПредписания"] = strDateRemoval;

                    section["ПП_РФ_170"] = viol.PpRf170;
                    section["ПП_РФ_25"] = viol.PpRf25;
                    section["ПП_РФ_307"] = viol.PpRf307;
                    section["ПП_РФ_491"] = viol.PpRf491;
                    section["Прочие_норм_док"] = viol.OtherNormativeDocs;

                    if (strCodePin.Length > 0)
                    {
                        strCodePin.Append(", ");
                    }

                    strCodePin.Append(viol.ViolationCodePin.Replace("ПиН ", string.Empty));
                }

                reportParams.SimpleReportParams["ПиН"] = strCodePin;

                #endregion

                if (this.CodeTemplate == "BlockGJI_ExecutiveDocPrescription")
                {
                    var inspectors = this.DocumentGjiInspectorDomainService.GetAll()
                        .Where(x => x.DocumentGji.Id == this.DocumentId)
                        .Select(x => new
                        {
                            x.Inspector.Fio,
                            x.Inspector.Position,
                            x.Inspector.ShortFio,
                            x.Inspector.FioAblative
                        })
                        .ToArray();

                    reportParams.SimpleReportParams["ИнспекторыИКоды"] =
                        inspectors.AggregateWithSeparator(x => x.Fio + " - " + x.Position, ", ");

                    if (prescription.Inspection.TypeBase == TypeBase.CitizenStatement)
                    {
                        var listBaseStatementAppealCits = inspectionAppealCitsDomain.GetAll()
                            .Where(x => x.Inspection.Id == prescription.Inspection.Id)
                            .Select(x => x.AppealCits.DocumentNumber)
                            .ToList();

                        reportParams.SimpleReportParams["НомерОбращения"] =
                            listBaseStatementAppealCits.Aggregate(string.Empty, (total, next) => total == string.Empty ? next : total + ", " + next);
                    }

                    if (disposal?.Inspection != null)
                    {
                        var listRealityObjs = inspectionGjiRealityObjectDomain.GetAll()
                            .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                            .Select(x => new
                            {
                                city = x.RealityObject.FiasAddress.PlaceName,
                                street = x.RealityObject.FiasAddress.StreetName,
                                house = x.RealityObject.FiasAddress.House
                            })
                            .ToList();

                        if (listRealityObjs.Count > 0)
                        {
                            var city = listRealityObjs.FirstOrDefault().city;
                            var strtmp = listRealityObjs.Aggregate(
                                string.Empty,
                                (total, next) => $"{(total == string.Empty ? "" : total + ";")} {next.street} {next.house}");

                            reportParams.SimpleReportParams["ДомаИАдреса"] = city + strtmp;
                        }
                    }                
                }

                var instFioPositions = DocumentGjiInspectorDomainService.GetAll()
                    .Where(x => x.DocumentGji.Id == prescription.Id)
                    .Select(x => new { x.Inspector.FioAblative, x.Inspector.PositionAblative })
                    .ToList();

                reportParams.SimpleReportParams["ИнспекторДолжностьТП"] = string.Join(", ", instFioPositions.Select(x =>
                    $"{x.FioAblative} - {x.PositionAblative}").ToArray());

                var insp = DocumentGjiInspectorDomainService.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => new
                    {
                        x.Inspector.Fio,
                        x.Inspector.Position,
                        x.Inspector.ShortFio,
                        x.Inspector.FioAblative
                    })
                    .ToArray();

                reportParams.SimpleReportParams["ИнспекторДолжность"] = insp.AggregateWithSeparator(x => x.Fio + " - " + x.Position, ", ");

                if (insp.Length > 0)
                {
                    var firstInsp = insp.First();
                    reportParams.SimpleReportParams["ДолжностьИнспектора"] = firstInsp.Position;
                }

                this.FillRegionParams(reportParams, prescription);
            }
            finally
            {
                this.Container.Release(actCheckDomain);
                this.Container.Release(actCheckInspectedPartDomain);
                this.Container.Release(disposalDomain);
                this.Container.Release(disposalTypeSurveyDomain);
                this.Container.Release(documentGjiChildrenDomain);
                this.Container.Release(inspectionAppealCitsDomain);
                this.Container.Release(inspectionAppealCitsDomain);
                this.Container.Release(inspectionGjiRealityObjectDomain);
                this.Container.Release(manOrgContractRealityObjectDomain);
                this.Container.Release(prescriptionDomain);
                this.Container.Release(prescriptionViolDomain);
                this.Container.Release(violationNormativeDocItemGjiDomain);
                this.Container.Release(zonalInspectionInspectorDomain);
            }
        }

        /// <summary>
        /// Получение кода шаблона
        /// </summary>
        /// <param name="typeSurveys">Типы обследования ЖКИ</param>
        protected virtual void GetTemplateCode(List<TypeSurveyGji> typeSurveys)
        {
            this.CodeTemplate = "BlockGJI_ExecutiveDocPrescription";

            if (typeSurveys.Any(x => x.Code == "22"))
            {
                this.CodeTemplate = "BlockGJI_ExecutiveDocPrescription_1";
            }
        }
    }
}