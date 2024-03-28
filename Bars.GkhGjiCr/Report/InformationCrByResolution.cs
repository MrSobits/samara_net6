namespace Bars.GkhGjiCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Отчет - Информация по капремонту по постановлениям КМ РТ
    /// </summary>
    public class InformationCrByResolution : BasePrintForm
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        private long programCr;

        private List<string> fields = new List<string> { "домовВПрограммеВсего",
                         "домовВПрограммеКР",
                         "домовВПрограммеПУ",
                         "домовВсего",
                         "домовКр",
                         "домовПу",                         
                         "системытеплоснабжения",
                         "системыГвс",
                         "системыХвс",
                         "канализации",
                         "газоснабжение",
                         "электроснабжение",
                         "начатыРаботыВсего",
                         "начатыРаботыКр",
                         "начатыРаботыПу",
                         "подвалы",
                         "крыша",
                         "лифт",
                         "лифтшахта",
                         "подъезд",
                         "фасад",
                         "утеплениефасада",
                         "фундамент",
                         "противопожарнаяАвтоматика",
                         "остановленыГжи",
                         "договоровГжиПринято",
                         "отклоненоДоговоровГжи",
                         "внесеноВРеестрвсего",
                         "внесеноВРеестрСмр",
                         "внесеноВРеестрПу",
                         "внесеноВРеестрЛифты",
                         "внесеноВРеестрЛифтыЭнергообслед",
                         "представленыГрафики",
                         "количествоОбследований",
                         //"домовДляГосЭкспертизы",                        
                         "объектыСработой1019",
                         "выданоПредписаний",
                         "закрытоАктом",
                         "составленоПротоколов",
                         "принятоАктов",
                         "завершеныКапРемонтомВсего",
                         "завершеныКапРемонтомКР",
                         "завершеныКапРемонтомПУ",                         
                         "подписаноЖиВсего",
                         "подписаноЖиКР",
                         "подписаноЖиПУ",
                         "домовВСледующейПрограмме",
                         "обследованоГжиВСледующейПргорамме",
                         "дефектныхВедомостей",
                         "программаСледПериод"
                         };

        private List<string> complexRepair = new List<string> { "7", "8", "9", "10", "11", "29" };

        private ProgrammCr nextProgramm;

        /// <summary>
        /// Конструктор
        /// </summary>
        public InformationCrByResolution()
            : base(new ReportTemplateBinary(Properties.Resources.InformationCrByResolution))
        {
        }

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get
            {
                return "Информация по капремонту по постановлениям КМ РТ";
            }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Desciption
        {
            get
            {
                return "Информация по капремонту по постановлениям КМ РТ";
            }
        }

        /// <summary>
        /// Наименование группы отчётов
        /// </summary>
        public override string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        /// <summary>
        /// Установить параметры отчета
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCr = baseParams.Params.GetAs<long>("programCr");
        }

        /// <summary>
        /// Генератор отчёта
        /// </summary>
        public override string ReportGenerator { get; set; }

        /// <summary>
        /// Контроллер отчёта
        /// </summary>
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.InformationCrByResolution";
            }
        }

        /// <summary>
        /// Необходимые разрешения
        /// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.InformationCrByResolution";
            }
        }

        /// <summary>
        /// Класс прокси
        /// </summary>
        private sealed class ProgrammCr
        {
            /// <summary>
            /// Идентификатор программы
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Имя программы
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Начало программы
            /// </summary>
            public DateTime? DateStart { get; set; }

            /// <summary>
            /// Конец программы
            /// </summary>
            public DateTime? DateEnd { get; set; }
        }

        /// <summary>
        /// Класс-прокси "Документ ГЖИ"
        /// </summary>
        public sealed class DocGji
        {
            /// <summary>
            /// Количество дефектных ведомостей
            /// </summary>
            public int DefectsCount { get; set; }

            /// <summary>
            /// Количество домов
            /// </summary>
            public int HousesCount { get; set; }

            /// <summary>
            /// Количество обследований
            /// </summary>
            public int ActsCurrentProgramm { get; set; }

            /// <summary>
            /// Количество обследований ГЖИ в следующей программе
            /// </summary>
            public int ActsNextProgramm { get; set; }

            /// <summary>
            /// Выдано предписаний
            /// </summary>
            public int Prescriptions { get; set; }

            /// <summary>
            /// Составлено протоколов
            /// </summary>
            public int PrescriptionsWithActRemovals { get; set; }
            
            /// <summary>
            /// Закрыто актом
            /// </summary>
            public int PrescriptionCountWithActs { get; set; }

            /// <summary>
            /// Принято актов
            /// </summary>
            public int PerfWorkActsByMun { get; set; }
        }

        /// <summary>
        /// Подготовить отчёт
        /// </summary>
        /// <param name="reportParams">Параметры отчёта</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var programm = this.Container.Resolve<IDomainService<ProgramCr>>().GetAll().First(x => x.Id == this.programCr);
            reportParams.SimpleReportParams["НаименованиеПрограммы"] = programm.Name;
            reportParams.SimpleReportParams["ДатаОтчета"] = DateTime.Today.ToShortDateString();
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("секция");
            var totals = reportParams.SimpleReportParams;
            var data = this.PrepareResult();

            reportParams.SimpleReportParams["программаСледПериод"] = this.nextProgramm.ReturnSafe(x => x.Name);
            var number = 0;

            foreach (var municipality in data.Where(municipality => municipality.Key != "Итого"))
            {
                section.ДобавитьСтроку();
                section["номер"] = ++number;
                section["район"] = municipality.Key;
                foreach (var value in municipality.Value)
                {
                    section[value.Key] = value.Value;
                }
            }

            foreach (var total in data["Итого"])
            {
                totals[total.Key] = total.Value;
            }

        }

        /// <summary>
        /// Информация о документах ГЖИ
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, DocGji> DataAboutDocGji()
        {
            var serviceDocumentGjiChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceDisposalTypeSurvey = this.Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var serviceActCheckRealtyObject = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var servicePresriptionViolations = this.Container.Resolve<IDomainService<PrescriptionViol>>();
            var serviceActRemoval = this.Container.Resolve<IDomainService<ActRemoval>>();
            var serviceProgramCr = this.Container.Resolve<IDomainService<ProgramCr>>();
            var serviceObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>();
            var serviceDefectList = this.Container.Resolve<IDomainService<DefectList>>();

            //получение текущей программы
            var currentProgramm = serviceProgramCr.GetAll()
                .Where(x => x.Id == this.programCr)
                .Select(x => new
                {
                    x.Id,
                    x.Period.DateEnd,
                    x.Period.DateStart
                })
                .FirstOrDefault();

            //получение следующей программы
            this.nextProgramm = serviceProgramCr.GetAll()
                .Where(x => x.Id != this.programCr && x.Period.DateStart > currentProgramm.DateStart)
                .Select(
                    x => new ProgrammCr()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        DateStart = x.Period.DateStart,
                        DateEnd = x.Period.DateEnd
                    })
                .OrderBy(x => x.DateStart)
                .FirstOrDefault();

            //получение объектов по след. программе
            var nextProgramObjectCrId = this.nextProgramm.ReturnSafe(y => y.Id);
            var nextProgrammObjectCrQuery = serviceObjectCr.GetAll()
                .Where(x => x.ProgramCr.Id == nextProgramObjectCrId);

            //получение Id объектов КР по след. программе
            var nextProgrammObjectCrIdsQuery = nextProgrammObjectCrQuery.Select(x => x.Id);

            //получение Id домов по след. программе
            var nextProgrammRoIdsQuery = nextProgrammObjectCrQuery.Select(x => x.RealityObject.Id);

            //получение распоряжений с типом обследования 4
            var disposalOnCrQuery = serviceDisposalTypeSurvey.GetAll().Where(x => x.TypeSurvey.Code == "4");

            //получение Id домов по текущей программе
            var currentProgrammRoIdsQuery = serviceObjectCr.GetAll()
                .Where(x => x.ProgramCr.Id == currentProgramm.Id)
                .Select(x => x.RealityObject.Id);

            //словарь МО по количеству дефектных ведомостей
            var defectCount = serviceDefectList.GetAll()
                .Where(x => nextProgrammObjectCrIdsQuery.Contains(x.ObjectCr.Id))
                .Where(x => x.State.Name == "Согласовано ГЖИ")
                .GroupBy(x => x.ObjectCr.RealityObject.Municipality.Name)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToDictionary(x => x.Key, x => x.count);

            //Словарь Мо по количеству домов для след. программы
            var countHouses = nextProgrammObjectCrQuery
                .GroupBy(x => x.RealityObject.Municipality.Name)
                .Select(x => new
                {
                    x.Key,
                    count = x.Select(y => y.RealityObject.Id).Distinct().Count()
                })
                .ToDictionary(x => x.Key, x => x.count);

            //Словарь  МО по количеству актов вып. работ
            var perfWorkActsByMun = this.Container.Resolve<IDomainService<PerformedWorkAct>>().GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.Id == this.programCr)
                    .Where(x => x.TypeWorkCr.Work.TypeWork == TypeWork.Work)
                    .Where(x => x.State.Name == "Принято ГЖИ" || x.State.Name == "Принят ТОДК")
                    .GroupBy(x => x.ObjectCr.RealityObject.Municipality.Name)
                    .Select(x => new { x.Key, count = x.Count() })
                    .ToDictionary(x => x.Key, y => y.count);

            //словарь МО по количеству предписаний для текущей программы
            var prescriptionsCount = servicePresriptionViolations.GetAll()
               .Where(x => currentProgrammRoIdsQuery.Contains(x.InspectionViolation.RealityObject.Id))
               .Where(x => disposalOnCrQuery.Any(y => y.Disposal.Inspection.Id == x.Document.Inspection.Id && y.Disposal.TypeDisposal == TypeDisposalGji.Base))
               .Where(x => x.Document.DocumentDate >= currentProgramm.DateStart)  // Тут по логике дата начала ТЕКУЩЕЙ программы КР
               .Where(x => x.Document.DocumentDate <= currentProgramm.DateEnd)  // Тут по логике дата окончания ТЕКУЩЕЙ программы КР
               .GroupBy(x => x.InspectionViolation.RealityObject.Municipality.Name)
               .Select(x => new { x.Key, count = x.Select(y => y.Document.Id).Distinct().Count() })
               .ToDictionary(x => x.Key, x => x.count);

            //Словарь МО по количеству актов проверки для след. программы
            var actsNextProgramm = serviceDocumentGjiChildren.GetAll()
                .Join(
                    serviceActCheckRealtyObject.GetAll(),
                    x => x.Children.Id,
                    y => y.ActCheck.Id,
                    (x, y) => new { DocumentGjiChildren = x, ActCheckRealtyObject = y })//связь DocumentGjiChildren и ActCheckRealtyObject
                .Where(x => nextProgrammRoIdsQuery.Contains(x.ActCheckRealtyObject.RealityObject.Id))
                .Where(x => disposalOnCrQuery.Any(y => y.Disposal.Id == x.DocumentGjiChildren.Parent.Id))//берутся только распоряженияс типом обследования 4
                .Where(x => x.DocumentGjiChildren.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                .Where(x => x.DocumentGjiChildren.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                .Where(x => x.DocumentGjiChildren.Children.DocumentDate >= currentProgramm.DateStart)  // Тут по логике дата начала ТЕКУЩЕЙ программы КР
                .Where(x => x.DocumentGjiChildren.Children.DocumentDate <= currentProgramm.DateEnd)    // Тут по логике дата окончания ТЕКУЩЕЙ программы КР
                .Select(x => new { x.ActCheckRealtyObject.RealityObject.Municipality.Name, x.DocumentGjiChildren.Children.Id })
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Id).Distinct().Count());

            //Словарь МО по количеству актов проверки для текущей программы
            var actsCurrentProgramm = serviceDocumentGjiChildren.GetAll()
                .Join(
                    serviceActCheckRealtyObject.GetAll(),
                    x => x.Children.Id,
                    y => y.ActCheck.Id,
                    (x, y) => new { DocumentGjiChildren = x, ActCheckRealtyObject = y })//связь DocumentGjiChildren и ActCheckRealtyObject
                .Where(x => currentProgrammRoIdsQuery.Contains(x.ActCheckRealtyObject.RealityObject.Id))
                .Where(x => disposalOnCrQuery.Any(y => y.Disposal.Id == x.DocumentGjiChildren.Parent.Id))//берутся только распоряженияс типом обследования 4
                .Where(x => x.DocumentGjiChildren.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                .Where(x => x.DocumentGjiChildren.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                .Where(x => x.DocumentGjiChildren.Children.DocumentDate >= currentProgramm.DateStart)  // Тут по логике дата начала ТЕКУЩЕЙ программы КР
                .Where(x => x.DocumentGjiChildren.Children.DocumentDate <= currentProgramm.DateEnd)    // Тут по логике дата окончания ТЕКУЩЕЙ программы КР
                .Select(x => new { x.ActCheckRealtyObject.RealityObject.Municipality.Name, x.DocumentGjiChildren.Children.Id })
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Id).Distinct().Count());

            var actRemovalHasProtocolQuery = serviceDocumentGjiChildren.GetAll()
                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol);

            //Словарь МО по количеству предписаний закрытых актами и количеству предписаний, по которым составлены протоколы
            var prescriptionsWithActRemovals = servicePresriptionViolations.GetAll()
                .Join(
                    serviceDocumentGjiChildren.GetAll(),
                    x => x.Document.Id,
                    y => y.Parent.Id,
                    (a, b) => new { PresciptionViolation = a, DocumentGjiChildren = b })//связь PresriptionViolations и DocumentGjiChildren
                .Join(
                    serviceActRemoval.GetAll(),
                    x => x.DocumentGjiChildren.Children.Id,
                    y => y.Id,
                    (c, d) => new { c.DocumentGjiChildren, c.PresciptionViolation, ActRemoval = d })//связь DocumentGjiChildren и ActRemoval
                .Where(x => currentProgrammRoIdsQuery.Contains(x.PresciptionViolation.InspectionViolation.RealityObject.Id))
                .Where(x => disposalOnCrQuery.Any(y => y.Disposal.Inspection.Id == x.PresciptionViolation.Document.Inspection.Id && y.Disposal.TypeDisposal == TypeDisposalGji.Base))
                .Where(x => x.PresciptionViolation.Document.DocumentDate >= currentProgramm.DateStart)  // Тут по логике дата начала ТЕКУЩЕЙ программы КР
                .Where(x => x.PresciptionViolation.Document.DocumentDate <= currentProgramm.DateEnd)  // Тут по логике дата окончания ТЕКУЩЕЙ программы КР
                .Where(x => x.DocumentGjiChildren.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                .Where(x => x.DocumentGjiChildren.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                .Select(x => new
                {
                    MuName = x.PresciptionViolation.InspectionViolation.RealityObject.Municipality.Name,
                    prescriptionId = x.DocumentGjiChildren.Parent.Id,
                    actRemovalHasProtocol = actRemovalHasProtocolQuery.Any(y => y.Parent.Id == x.ActRemoval.Id),
                    x.ActRemoval.TypeRemoval
                })
                .AsEnumerable()
                .GroupBy(x => x.MuName)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var prescriptionCountWithActs = x.Where(y => y.TypeRemoval == YesNoNotSet.Yes).Select(y => y.prescriptionId).Distinct().Count();
                        var prescriptionCountWithActsWithProtocol = x.Where(y => y.TypeRemoval == YesNoNotSet.No && y.actRemovalHasProtocol).Select(y => y.prescriptionId).Distinct().Count();

                        return new { prescriptionCountWithActs, prescriptionCountWithActsWithProtocol };
                    });

            var result = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Select(x => x.Name)
                .ToList()
                .GroupBy(x => x)
                .ToDictionary(x => x.Key,
                    x => new
                        DocGji()
                    {
                        PerfWorkActsByMun = perfWorkActsByMun.ContainsKey(x.Key) ? perfWorkActsByMun[x.Key] : 0,
                        DefectsCount = defectCount.ContainsKey(x.Key) ? defectCount[x.Key] : 0,
                        HousesCount = countHouses.ContainsKey(x.Key) ? countHouses[x.Key] : 0,
                        ActsCurrentProgramm = actsCurrentProgramm.ContainsKey(x.Key) ? actsCurrentProgramm[x.Key] : 0,
                        ActsNextProgramm = actsNextProgramm.ContainsKey(x.Key) ? actsNextProgramm[x.Key] : 0,
                        Prescriptions = prescriptionsCount.ContainsKey(x.Key) ? prescriptionsCount[x.Key] : 0,
                        PrescriptionsWithActRemovals = prescriptionsWithActRemovals.ContainsKey(x.Key) ? prescriptionsWithActRemovals[x.Key].prescriptionCountWithActsWithProtocol : 0,
                        PrescriptionCountWithActs = prescriptionsWithActRemovals.ContainsKey(x.Key) ? prescriptionsWithActRemovals[x.Key].prescriptionCountWithActs : 0
                    });

            return result;
        }

        private Dictionary<string, Dictionary<string, double>> PrepareResult()
        {
            var muData = new Dictionary<string, Dictionary<string, double>>();

            var municipalities = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Select(x => new { x.Name })
                .OrderBy(x => x.Name)
                .ToList();

            #region Работы

            var works = this.Container.Resolve<IDomainService<TypeWorkCr>>();

            var worksObject = works.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCr)
                .Where(x => x.Work.TypeWork == TypeWork.Work)
                .Select(x => new
                {
                    x.Id,
                    MuName = x.ObjectCr.RealityObject.Municipality.Name,
                    x.Work.Code,
                    x.PercentOfCompletion,
                    x.Work.TypeWork,
                    ObjectCrId = x.ObjectCr.Id,
                    x.DateStartWork,
                    x.DateEndWork
                });
            #endregion

            #region buildContract

            var buildContracts = this.Container.Resolve<IDomainService<BuildContract>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCr)
                .Select(x => new
                {
                    MuName = x.ObjectCr.RealityObject.Municipality.Name,
                    State = x.State.Name,
                    x.DateAcceptOnReg,
                    x.DateCancelReg,
                    x.TypeContractBuild,
                    ObjectCrId = x.ObjectCr.Id
                })
                .ToList();
            #endregion

            #region Мониторингсмр
            var monitoringSmrTmpReg = this.Container.Resolve<IDomainService<MonitoringSmr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCr)
                .Where(x => x.State.Name == "Утверждено ГЖИ")
                .Select(x => x.ObjectCr.Id);

            var monitoringSmr = worksObject
                .Where(x => monitoringSmrTmpReg.Contains(x.ObjectCrId))
                .Select(x => new
                {
                    x.MuName,
                    x.DateEndWork,
                    x.DateStartWork,
                    x.ObjectCrId
                })
                .ToList();
            #endregion

            #region DocumentsGJIOfRelstyObjects

            var datasAboutDocGji = this.DataAboutDocGji();

            muData["Итого"] = this.fields.ToDictionary(x => x + "Итого", x => 0d);

            //Query объектов КР по текущей программе
            var municipalityHouseQuery = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => x.ProgramCr.Id == this.programCr)
                .Select(x => new
                {
                    x.Id,
                    RealityObjectId = x.RealityObject.Id,
                    MunicipalityName = x.RealityObject.Municipality.Name,
                    x.DateStopWorkGji,
                    x.DateAcceptCrGji
                });

            var municipalitysHouses = municipalityHouseQuery
                .ToList()
                .OrderBy(x => x.MunicipalityName)
                .GroupBy(x => x.MunicipalityName)
                .ToDictionary(x => x.Key, x => x.ToList());

            foreach (var municipality in municipalitysHouses.Keys)
            {
                muData[municipality] = this.fields.ToDictionary(x => x, x => 0d);

                if (datasAboutDocGji.ContainsKey(municipality))
                {
                    var data = muData[municipality];
                    var docGjiData = datasAboutDocGji[municipality];

                    data["количествоОбследований"] = docGjiData.ActsCurrentProgramm;
                    data["выданоПредписаний"] = docGjiData.Prescriptions;
                    data["закрытоАктом"] = docGjiData.PrescriptionCountWithActs;
                    data["составленоПротоколов"] = docGjiData.PrescriptionsWithActRemovals;
                    data["принятоАктов"] = docGjiData.PerfWorkActsByMun;
                    data["домовВСледующейПрограмме"] = docGjiData.HousesCount;
                    data["обследованоГжиВСледующейПргорамме"] = docGjiData.ActsNextProgramm;
                    data["дефектныхВедомостей"] = docGjiData.DefectsCount;
                }
            }

            var monitoringSmrDict = monitoringSmr
                .GroupBy(q => q.MuName)
                .ToDictionary(q => q.Key,
                    q =>
                        q.GroupBy(x => x.ObjectCrId)
                        .ToDictionary(z => z.Key, z => z.All(x => x.DateEndWork != null && x.DateStartWork != null).ToInt()));

            var buildContractsDict = buildContracts
                .GroupBy(q => q.MuName)
                .ToDictionary(q => q.Key,
                    q =>
                        q.GroupBy(x => x.ObjectCrId)
                        .ToDictionary(z => z.Key, z =>
                        {
                            var countDateAcceptOnReg = z.Count(x => x.DateAcceptOnReg != null);

                            var countDateCancelReg = z.Count(x => x.DateCancelReg != null);

                            var smr = z.Count(x => x.State == "Утверждено ГЖИ" && x.TypeContractBuild == TypeContractBuild.Smr);

                            var device = z.Count(x => x.State == "Утверждено ГЖИ" && x.TypeContractBuild == TypeContractBuild.Device);

                            var lift = z.Count(x => x.State == "Утверждено ГЖИ" && x.TypeContractBuild == TypeContractBuild.Lift);

                            var energySurvey = z.Count(x => x.State == "Утверждено ГЖИ" && x.TypeContractBuild == TypeContractBuild.EnergySurvey);

                            return new BuildContracts
                            {
                                CountDateAcceptOnReg = countDateAcceptOnReg,
                                Smr = smr,
                                Lift = lift,
                                Device = device,
                                EnergySurvey = energySurvey,
                                CountDateCancelReg = countDateCancelReg
                            };
                        }));

            var worksOfObjectCrDict = worksObject
                .Select(x => new
                {
                    x.MuName,
                    x.ObjectCrId,
                    x.Code,
                    x.PercentOfCompletion
                })
                .ToList()
                .GroupBy(w => w.MuName)
                .ToDictionary(w => w.Key,
                    w =>
                        w.GroupBy(z => z.ObjectCrId)
                        .ToDictionary(z => z.Key, z =>
                        {
                            var complexRepairByCode = z.All(y => this.complexRepair.Contains(y.Code));

                            var count = z.Count(x => x.PercentOfCompletion > 0);

                            var percentOfCompletionAndCode = z
                                .GroupBy(x => x.Code)
                                .ToDictionary(q => q.Key, q => q.Any(x => x.PercentOfCompletion > 0));

                            var beginWork = z.Where(x => this.complexRepair.Contains(x.Code)).Any(x => x.PercentOfCompletion > 0) ? 1 : 0;

                            var endWork = z.All(x => x.PercentOfCompletion == 100).ToInt();

                            return new WorksOfObjectCr
                            {
                                ComplexRepairByCode = complexRepairByCode,
                                PercentOfCompletionAndCode = percentOfCompletionAndCode,
                                BeginWork = beginWork,
                                EndWork = endWork,
                                Count = count
                            };
                        }));

            #endregion

            foreach (var municipality in municipalities.OrderBy(x => x.Name))
            {
                if (!municipalitysHouses.ContainsKey(municipality.Name)) continue;

                var municipalityHouse = municipalitysHouses[municipality.Name];

                var worksOfObjectCrByMu = worksOfObjectCrDict.ContainsKey(municipality.Name) ? worksOfObjectCrDict[municipality.Name] : new Dictionary<long, WorksOfObjectCr>();
                var monitoringSmrByMu = monitoringSmrDict.ContainsKey(municipality.Name) ? monitoringSmrDict[municipality.Name] : new Dictionary<long, int>();
                var buildContractsByMu = buildContractsDict.ContainsKey(municipality.Name) ? buildContractsDict[municipality.Name] : new Dictionary<long, BuildContracts>();

                var data = muData[municipality.Name];

                foreach (var house in municipalityHouse)
                {
                    var helperVar = 0;

                    var worksOfObjectCr = worksOfObjectCrByMu.ContainsKey(house.Id) ? worksOfObjectCrByMu[house.Id] : new WorksOfObjectCr();
                    var buildContractsHouse = buildContractsByMu.ContainsKey(house.Id) ? buildContractsByMu[house.Id] : new BuildContracts();

                    var complexOrNot = 2;

                    if (worksOfObjectCr.ComplexRepairByCode)
                    {
                        data["домовВПрограммеПУ"]++;
                        complexOrNot = 0;
                    }
                    else
                    {
                        data["домовВПрограммеКР"]++;
                        complexOrNot = 1;
                    }

                    helperVar = worksOfObjectCr.Count;
                    if (helperVar > 0)
                    {
                        switch (complexOrNot)
                        {
                            case 1:
                                data["домовКр"]++;
                                break;
                            case 0:
                                data["домовПу"]++;
                                break;
                        }
                    }

                    Func<string, int> supplyByCode = code => worksOfObjectCr.PercentOfCompletionAndCode != null && worksOfObjectCr.PercentOfCompletionAndCode.ContainsKey(code) ? worksOfObjectCr.PercentOfCompletionAndCode[code].ToInt() : 0;

                    data["системытеплоснабжения"] += supplyByCode("1");
                    data["системыГвс"] += supplyByCode("2");
                    data["системыХвс"] += supplyByCode("3");
                    data["канализации"] += supplyByCode("4");
                    data["газоснабжение"] += supplyByCode("5");
                    data["электроснабжение"] += supplyByCode("6");

                    switch (complexOrNot)
                    {
                        case 1:

                            data["начатыРаботыКр"] += worksOfObjectCr.BeginWork;

                            if (house.DateAcceptCrGji != null)
                            {
                                data["подписаноЖиКР"]++;
                            }
                            else
                            {
                                data["завершеныКапРемонтомКР"] += worksOfObjectCr.EndWork;
                            }
                            break;

                        case 0:

                            data["начатыРаботыПу"] += worksOfObjectCr.BeginWork;

                            if (house.DateAcceptCrGji != null)
                            {
                                data["подписаноЖиПУ"]++;
                            }
                            else
                            {
                                data["завершеныКапРемонтомПУ"] += worksOfObjectCr.EndWork;
                            }
                            break;
                    }

                    data["подвалы"] += supplyByCode("12");
                    data["крыша"] += supplyByCode("13");
                    data["лифт"] += supplyByCode("14");
                    data["лифтшахта"] += supplyByCode("15");
                    data["подъезд"] += supplyByCode("21");
                    data["фасад"] += supplyByCode("16");
                    data["утеплениефасада"] += supplyByCode("17");
                    data["фундамент"] += supplyByCode("18");
                    data["противопожарнаяАвтоматика"] += supplyByCode("19");
                    data["остановленыГжи"] += house.DateStopWorkGji.HasValue ? 1 : 0;
                    data["договоровГжиПринято"] += buildContractsHouse.CountDateAcceptOnReg;
                    data["отклоненоДоговоровГжи"] += buildContractsHouse.CountDateCancelReg;
                    data["внесеноВРеестрСмр"] += buildContractsHouse.Smr;
                    data["внесеноВРеестрПу"] += buildContractsHouse.Device;
                    data["внесеноВРеестрЛифты"] += buildContractsHouse.Lift;
                    data["внесеноВРеестрЛифтыЭнергообслед"] += buildContractsHouse.EnergySurvey;
                    data["представленыГрафики"] += monitoringSmrByMu.ContainsKey(house.Id) ? monitoringSmrByMu[house.Id] : 0;
                    data["подписаноЖиВсего"] = data["подписаноЖиПУ"] + data["подписаноЖиКР"];
                    data["завершеныКапРемонтомВсего"] = data["завершеныКапРемонтомКР"] + data["завершеныКапРемонтомПУ"];
                    data["внесеноВРеестрвсего"] = data["внесеноВРеестрСмр"] + data["внесеноВРеестрПу"] + data["внесеноВРеестрЛифты"] + data["внесеноВРеестрЛифтыЭнергообслед"];
                }

                data["домовВПрограммеВсего"] = municipalityHouse.Count;
                data["домовВсего"] = data["домовПу"] + data["домовКр"];
                data["начатыРаботыВсего"] = data["начатыРаботыКр"] + data["начатыРаботыПу"];

                foreach (var field in this.fields)
                {
                    muData["Итого"][field + "Итого"] += data[field];
                }
            }

            return muData;
        }
    }

    internal sealed class WorksOfObjectCr
    {
        public bool ComplexRepairByCode { get; set; }
        public Dictionary<string, bool> PercentOfCompletionAndCode { get; set; }
        public int BeginWork { get; set; }
        public int EndWork { get; set; }
        public int Count { get; set; }
    }

    sealed class BuildContracts
    {
        public int CountDateAcceptOnReg { get; set; }
        public int Smr { get; set; }
        public int Lift { get; set; }
        public int EnergySurvey { get; set; }
        public int Device { get; set; }
        public int CountDateCancelReg { get; set; }
    }
}