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
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using GkhGji.Entities.Dict;
    using GkhGji.Report;

    /// <summary>
    /// Отчёт "Предписание"
    /// </summary>
    public class PrescriptionGjiStimulReport : GjiBaseStimulReport
    {
        private long DocumentId { get; set; }

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
        /// <summary>
        /// Конструктор отчёта
        /// </summary>
        public PrescriptionGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.Prescription))
        {
        }
        
        /// <summary>
        /// Формат экспорта
        /// </summary>
        public override StiExportFormat ExportFormat 
        {
            get { return StiExportFormat.Word2007; }
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public override string Id
        {
            get { return "TomskPrescriptionGji"; }
        }

        /// <summary>
        /// Код формы
        /// </summary>
        public override string CodeForm
        {
            get { return "Prescription"; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Предписание"; }
        }
        
        /// <summary>
        /// Описание
        /// </summary>
        public override string Description
        {
            get { return "Предписание"; }
        }

        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Установка параметров пользователя
        /// </summary>
        /// <param name="userParamsValues">Параметры</param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        /// <summary>
        /// Получение информации о шаблоне
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "TomskPrescriptionGji",
                                   Name = "Prescription",
                                   Description = "Предписание",
                                   Template = Properties.Resources.Prescription
                               }
                       };
        }

        /// <summary>
        /// Подготовка отчёта
        /// </summary>
        /// <param name="reportParams">Параметры</param>
        public override void PrepareReport(ReportParams reportParams)
        {

            var prescriptionDomain = Container.Resolve<IDomainService<Prescription>>();
            var documentGjiDomain = Container.Resolve<IDomainService<DocumentGji>>();
            var inspectionRoDomain = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var prescrViolDomain = Container.Resolve<IDomainService<PrescriptionViol>>();
            var docInspectorDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();

            try
            {
                var prescription = prescriptionDomain.GetAll().FirstOrDefault(x => x.Id == DocumentId);

                if (prescription == null)
                    return;

                if (prescription.Stage == null)
                    return;

                if (prescription.Stage.Parent == null)
                    return;

                var documentGji = documentGjiDomain.GetAll()
                    .FirstOrDefault(x => x.Stage.Id == prescription.Stage.Parent.Id);

                if (documentGji == null)
                    return;

                FillCommonFields(prescription);

                this.ReportParams["ДатаПредписания"] = prescription.DocumentDate.HasValue
                                                                     ? prescription.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                                                                     : string.Empty;

                this.ReportParams["НомерПредписания"] = prescription.DocumentNumber;

                FillRoInfo(prescription);
                FillContragentInfo(prescription);

                this.ReportParams["ПричинаИнспектирования"] = string.Empty;

                if (documentGji.TypeDocumentGji == TypeDocumentGji.Disposal)
                {
                    var disposal = (Disposal) documentGji;

                    if (disposal.KindCheck != null)
                    {
                        if (disposal.KindCheck.Code == TypeCheck.PlannedExit || disposal.KindCheck.Code == TypeCheck.PlannedDocumentation)
                        {
                            this.ReportParams["ПричинаИнспектирования"] = "плановая проверка";
                        }
                        else if (disposal.KindCheck.Code == TypeCheck.NotPlannedExit || disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation
                            || disposal.KindCheck.Code == TypeCheck.InspectionSurvey)
                        {
                            this.ReportParams["ПричинаИнспектирования"] = "обращение жильцов";
                        }
                    }
                }

                if (prescription.Executant != null)
                {
                    var shortName = prescription.Contragent != null ? prescription.Contragent.ShortName : string.Empty;
                    var physicalPerson = prescription.PhysicalPerson;

                    switch (prescription.Executant.Code)
                    {
                        case "0":
                        case "9":
                        case "2":
                        case "4":
                        case "8":
                        case "18":
                            this.ReportParams["КомуВыдано"] = shortName;
                            break;
                        case "10":
                        case "1":
                        case "3":
                        case "5":
                        case "19":
                            this.ReportParams["КомуВыдано"] = shortName + ", " + physicalPerson;
                            break;
                        case "6":
                        case "7":
                            this.ReportParams["КомуВыдано"] = physicalPerson;
                            break;
                    }
                }

                var listRealityObjs = inspectionRoDomain.GetAll()
                .Where(x => x.Inspection.Id == documentGji.Inspection.Id)
                .Select(x => new
                {
                    city = x.RealityObject.FiasAddress.PlaceName,
                    street = x.RealityObject.FiasAddress.StreetName,
                    house = x.RealityObject.FiasAddress.House,
                    housing = x.RealityObject.FiasAddress.Housing
                })
                .ToList();

                if (listRealityObjs.Any())
                {
                    var city = listRealityObjs.First().city;
                    var strtmp = listRealityObjs.Aggregate(
                        string.Empty,
                        (total, next) =>
                        string.Format(
                            "{0} {1} {2}{3}",
                            total == string.Empty ? "" : total + ";",
                            next.street,
                            next.house,
                            !string.IsNullOrEmpty(next.housing) ? " корп." + next.housing : string.Empty));

                    this.ReportParams["ДомаИАдреса"] = city + strtmp;
                }

                var violations = prescrViolDomain.GetAll()
                    .Where(x => x.Document.Id == prescription.Id)
                    .Select(x => new
                    {
                        ViolationId = x.InspectionViolation.Violation.Id,
                        ViolationCodePin = x.InspectionViolation.Violation.CodePin,
                        ViolationName = x.InspectionViolation.Violation.Name,
                        x.Action,
                        x.Description,
                        x.InspectionViolation.DatePlanRemoval,
                        x.InspectionViolation.Violation.PpRf170,
                        x.InspectionViolation.Violation.PpRf25,
                        x.InspectionViolation.Violation.PpRf307,
                        x.InspectionViolation.Violation.PpRf491,
                        x.InspectionViolation.Violation.OtherNormativeDocs
                    })
                    .ToList();


                var strCodePin = new StringBuilder();

                var i = 0;
                var addedList = new List<long>();
                var dataViol = new List<PrescriptionViolRecord>();
                foreach (var viol in violations.Where(viol => !addedList.Contains(viol.ViolationId)))
                {
                    addedList.Add(viol.ViolationId);

                    var rec = new PrescriptionViolRecord();
                    rec.НомерПП = ++i;
                    rec.Подробнее = viol.Description;
                    rec.Мероприятия = viol.Action;

                    var strDateRemoval = viol.DatePlanRemoval.HasValue
                                                    ? viol.DatePlanRemoval.Value.ToShortDateString()
                                                    : string.Empty;
                    rec.СрокИсполнения = strDateRemoval;
                    if (strCodePin.Length > 0)
                    {
                        strCodePin.Append(", ");
                    }
                    dataViol.Add(rec);
                    strCodePin.Append(viol.ViolationCodePin.Replace("ПиН ", string.Empty));
                }

                FillNormativeDocItems(addedList);

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Нарушения",
                    MetaType = nameof(PrescriptionViolRecord),
                    Data = dataViol
                });

                this.ReportParams["ПиН"] = strCodePin.ToString();

                var insp = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => new
                    {
                        x.Inspector.Fio,
                        x.Inspector.Position,
                        x.Inspector.ShortFio,
                        x.Inspector.FioAblative
                    })
                    .ToArray();

                if (insp.Length > 0)
                {
                    var firstInsp = insp.First();
                    this.ReportParams["ДолжностьИнспектора"] = firstInsp.Position;

                    if (firstInsp != null && !string.IsNullOrEmpty(firstInsp.ShortFio))
                    {
                        this.ReportParams["ИнспекторФамИО"] = firstInsp.ShortFio;
                    }
                }
            }
            finally 
            {
                Container.Release(prescriptionDomain);
                Container.Release(documentGjiDomain);
                Container.Release(inspectionRoDomain);
                Container.Release(prescrViolDomain);
                Container.Release(docInspectorDomain);
            }

        }

        /// <summary>
        ///  Добавить переменную "ПереченьНарушенныхНПА"
        /// - Наименование пункта и НПА связанных с нарушениями из таблицы на вкладке Нарушения в Предписании
        /// с группировкой по НПА
        /// Например "п. 1, п. 2 Тестовый НПА; п. 1 Другой НПА;"
        /// </summary>
        /// <param name="violationIds"></param>
        private void FillNormativeDocItems(IEnumerable<long> violationIds)
        {
            var violNormDocItemGjiDomain = Container.Resolve<IDomainService<ViolationNormativeDocItemGji>>();

            var violNormDoc = violNormDocItemGjiDomain.GetAll()
                .Where(x => violationIds.Contains(x.ViolationGji.Id))
                .Select(x => new
                {
                    NormativeDocName = x.NormativeDocItem.NormativeDoc.Name,
                    NormativeDocItemName = x.NormativeDocItem.Number
                }).Distinct().AsEnumerable().GroupBy(x => x.NormativeDocName);

            var violNormDocItemsSB = new StringBuilder();
            foreach (var group in violNormDoc)
            {
                violNormDocItemsSB.AppendFormat("{0} {1}; ",
                    string.Join(", ", @group.Select(g => g.NormativeDocItemName)), @group.Key);
            }
            this.ReportParams["ПереченьНарушенныхНПА"] = violNormDocItemsSB.ToStr();
        }

        private void FillRoInfo(Prescription prescription)
        {
            var prescrViolDomain = Container.Resolve<IDomainService<PrescriptionViol>>();
            var roStrElDomain = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var roDomain = Container.Resolve<IDomainService<PrescriptionViol>>();
            var manOrgRoDomain = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            try
            {
                var floors = string.Empty;
                var wallMaterial = string.Empty;
                var systems = string.Empty;

                var realityObj = roDomain.GetAll()
                    .Where(x => x.Document.Id == prescription.Id)
                    .Select(x => x.InspectionViolation.RealityObject)
                    .ToList()
                    .Distinct(x => x.Id);

                // если дом один, то выводим адрес дома и номер дома, если домов нет или больше 1 - ничего не выводим
                if (realityObj.Count() == 1)
                {
                    var firstPrescriptionRo = realityObj.FirstOrDefault();

                    if (firstPrescriptionRo != null)
                    {
                        wallMaterial = firstPrescriptionRo.WallMaterial != null ? firstPrescriptionRo.WallMaterial.Name : "";
                    
                        if (firstPrescriptionRo.MaximumFloors != null)
                        {
                            var floor = firstPrescriptionRo.MaximumFloors.ToLong();
                            if (floorsWords.ContainsKey(floor))
                            {
                                floors = floorsWords[floor];
                            }
                        }

                    }

                    string typeControl = string.Empty;

                    var date = prescription.DocumentDate ?? DateTime.Now.Date;

                    var typeCurrentContract = manOrgRoDomain.GetAll()
                        .Where(x => x.RealityObject.Id == firstPrescriptionRo.Id)
                        .Where(x => x.ManOrgContract.StartDate <= date)
                        .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= date)
                        .OrderByDescending(x => x.ManOrgContract.EndDate)
                        .Select(x => new
                        {
                            x.ManOrgContract.TypeContractManOrgRealObj,
                            TypeManagement = (TypeManagementManOrg?)x.ManOrgContract.ManagingOrganization.TypeManagement
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

                    this.ReportParams["СпособУправления"] = typeControl;

                }

                var robjectFilter = prescrViolDomain.GetAll()
                    .Where(x => x.Document.Id == prescription.Id)
                    .Select(x => x.InspectionViolation.RealityObject.Id);

                var manOrgs = manOrgRoDomain.GetAll()
                    .Where(x => robjectFilter.Contains(x.RealityObject.Id))
                    .OrderByDescending(x => x.ManOrgContract.EndDate)
                    .Select(x => x.ManOrgContract.ManagingOrganization.Contragent.Name);

                this.ReportParams["УправляющаяОрганизация"] = manOrgs.Any() ? String.Join(", ", manOrgs) : "Отсутствует";

                var roCeo = roStrElDomain.GetAll()
                    .Where(x => robjectFilter.Any(y => y == x.RealityObject.Id))
                    .Select(x => x.StructuralElement.Group.CommonEstateObject.Name)
                    .Where(x => x != null)
                    .AsEnumerable()
                    //приводим к нормированному виду, дабы лишние пробелы и регистр не портили картину
                    .Select(x => x.ToUpper().Replace(" ", ""))
                    .Distinct();

                var addStr = new List<string>();

                foreach (var ceo in roCeo)
                {
                    switch (ceo)
                    {
                        case "ХВС":
                            addStr.Add("холодного водоснабжения");
                            break;
                        case "ГВС":
                            addStr.Add("горячего водоснабжения");
                            break;
                        case "СИСТЕМАВОДООТВЕДЕНИЯМКД":
                            addStr.Add("водоотведения");
                            break;
                        case "ЭЛЕКТРОСНАБЖЕНИЕ":
                            addStr.Add("электроснабжения");
                            break;
                        case "ОТОПЛЕНИЕ":
                            addStr.Add("теплоснабжения");
                            break;
                    }
                }

                if (addStr.Count > 0)
                {
                    systems = addStr.AggregateWithSeparator(", ");    
                }

                var characteristic = string.Empty;
                if (!string.IsNullOrEmpty(floors))
                {
                    characteristic += floors;
                }

                if (!string.IsNullOrEmpty(wallMaterial))
                {
                    if (!string.IsNullOrEmpty(characteristic))
                    {
                        characteristic += ", ";
                    }

                    characteristic += wallMaterial;
                }

                if (!string.IsNullOrEmpty(systems))
                {
                    if (!string.IsNullOrEmpty(characteristic))
                    {
                        characteristic += ", оборудован централизованными системами ";
                    }

                    characteristic += systems;
                }

                this.ReportParams["ХарактеристикаОбъекта"] = characteristic;

            }
            finally
            {
                Container.Release(roDomain);
                Container.Release(manOrgRoDomain);
                Container.Release(prescrViolDomain);
                Container.Release(roStrElDomain);
            }
        }

        private void FillContragentInfo(Prescription prescription)
        {
            var contragent = prescription.Contragent;

            if (contragent != null)
            {
                this.ReportParams["ИНН"] = contragent.Inn;
                this.ReportParams["КПП"] = contragent.Kpp;

                if (contragent.FiasJuridicalAddress != null)
                {
                    var subStr = contragent.FiasJuridicalAddress.AddressName.Split(',');

                    var newAddr = new StringBuilder();

                    foreach (var rec in subStr)
                    {
                        if (newAddr.Length > 0)
                        {
                            newAddr.Append(',');
                        }

                        if (rec.Contains("р-н."))
                        {
                            var mu = rec.Replace("р-н.", "") + " район";
                            newAddr.Append(mu);
                            continue;
                        }

                        newAddr.Append(rec);
                    }

                    this.ReportParams["АдресКонтрагента"] = newAddr.ToString();
                }
                else
                {
                    this.ReportParams["АдресКонтрагента"] = contragent.JuridicalAddress;
                }

                this.ReportParams["АдресКонтрагентаФакт"] = contragent != null
                    ? contragent.FactAddress
                    : string.Empty;

                if (contragent.FiasJuridicalAddress != null)
                {
                    var subStr = contragent.FiasJuridicalAddress.AddressName.Split(',');

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

                    this.ReportParams["АдресКонтрагентаЮр"] = newAddr.ToString();
                }

            }
        }

        protected class PrescriptionViolRecord
        {
            public int НомерПП { get; set; }

            public string Подробнее { get; set; }

            public string Мероприятия { get; set; }

            public string СрокИсполнения { get; set; }
        }
    }

}