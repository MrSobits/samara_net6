namespace Bars.GkhGji.Regions.Samara.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Samara.Entities;
    using Castle.Windsor;

    public class JournalAppeals : BasePrintForm
    {
        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private long[] municipalityIds;

        public JournalAppeals()
            : base(new ReportTemplateBinary(Properties.Resources.JournalAppeals))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.JournalAppeals";
            }
        }

        public override string Desciption
        {
            get { return "Журнал обращений (Самара)"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.JournalAppeals"; }
        }

        public override string Name
        {
            get { return "Журнал обращений (Самара)"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            var m = baseParams.Params["municipalityIds"].ToString();
            this.municipalityIds = !string.IsNullOrEmpty(m) ? m.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var servAppealCits = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            reportParams.SimpleReportParams["period"] = string.Format(
                "{0} по {1}", this.dateStart.ToShortDateString(), this.dateEnd.ToShortDateString());

            // Получим обращения в выбранных Муниципальных образований, если они есть, если нет то все у кого забиты дома
            var appealCitsQuery = servAppealCits.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.AppealCits.DateFrom.HasValue && x.AppealCits.DateFrom.Value >= this.dateStart && x.AppealCits.DateFrom.Value <= this.dateEnd);
                
            // Идентификатор МО = Регистрация обращений (для Самары)
            var regAppealsMunicipalityId =
                this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                    .Where(x => x.Name == "Регистрация обращений")
                    .Select(x => x.Id)
                    .FirstOrDefault();
            
            var appealCits = new Dictionary<long, AppealCitProxy>();
            var addressDict = new Dictionary<long, string>();

            var testers = Container.ResolveDomain<AppealCitsTester>().GetAll()
                    .Select(x => new
                    {
                        x.AppealCits.Id,
                        x.Tester.Fio
                    })
                    .ToList()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Fio).AggregateWithSeparator(", "));

            var appealCitsWithRo = appealCitsQuery
                .Select(x => new
                        {
                            x.AppealCits.Id,
                            x.AppealCits.DateFrom,
                            x.AppealCits.NumberGji,
                            Municipality = x.RealityObject.Municipality.Name,
                            x.AppealCits.Correspondent,
                            IsFile = x.AppealCits.File != null,
                            KindStatement = x.AppealCits.KindStatement.Name,
                            x.AppealCits.Description,
                    PreviousAppealCits = x.AppealCits.PreviousAppealCits.NumberGji ?? string.Empty,
                    ManagingOrganization = x.AppealCits.ManagingOrganization.Contragent.Name ?? string.Empty,
                            x.RealityObject.Address,
                            Surety = x.AppealCits.Surety.Fio,
                            SuretyResolve = x.AppealCits.SuretyResolve.Name,
                            SuretyDate = x.AppealCits.SuretyDate.HasValue && x.AppealCits.SuretyDate.Value != DateTime.MinValue ? x.AppealCits.SuretyDate.Value.ToShortDateString() : string.Empty,
                            Executant = x.AppealCits.Executant.Fio,
                            Tester = string.Empty,
                            CheckTime = x.AppealCits.CheckTime.HasValue && x.AppealCits.CheckTime.Value != DateTime.MinValue ? x.AppealCits.CheckTime.Value.ToShortDateString() : string.Empty
                        })
                .ToList();


            // проверка на наличие МО = Регистрация обращений (для Самары)
            if (regAppealsMunicipalityId > 0)
            {
                var appealCitsWithRoIdsQuery = appealCitsQuery.Select(x => x.AppealCits.Id);

                var appealCitsWithOutRo = this.Container.Resolve<IDomainService<AppealCits>>().GetAll()
                .Where(x => x.DateFrom.HasValue && x.DateFrom.Value >= this.dateStart && x.DateFrom.Value <= this.dateEnd)
                .Where(x => !appealCitsWithRoIdsQuery.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.DateFrom,
                    x.NumberGji,
                    Municipality = "Регистрация обращений",
                    x.Correspondent,
                    IsFile = x.File != null,
                    KindStatement = x.KindStatement.Name,
                    x.Description,
                    PreviousAppealCits = x.PreviousAppealCits.NumberGji ?? string.Empty,
                    ManagingOrganization = x.ManagingOrganization.Contragent.Name ?? string.Empty,
                    Address = string.Empty,
                    Surety = x.Surety.Fio,
                    SuretyResolve = x.SuretyResolve.Name,
                    SuretyDate = x.SuretyDate.HasValue && x.SuretyDate.Value != DateTime.MinValue ? x.SuretyDate.Value.ToShortDateString() : string.Empty,
                    Executant = x.Executant.Fio,
                    Tester = string.Empty,
                    CheckTime = x.CheckTime.HasValue && x.CheckTime.Value != DateTime.MinValue ? x.CheckTime.Value.ToShortDateString() : string.Empty
                }).ToList();

                var appeals = appealCitsWithRo.Union(appealCitsWithOutRo).ToList();
                appealCits = appeals
                    .OrderBy(x => x.Municipality)
                    .ThenBy(x => x.Address)
                    .ThenBy(x => x.DateFrom)
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key,
                        x => new AppealCitProxy
                        {
                            Id = x.First().Id,
                            DateFrom = x.First().DateFrom,
                            NumberGji = x.First().NumberGji,
                            Municipality = x.First().Municipality,
                            Correspondent = x.First().Correspondent,
                            IsFile = x.First().IsFile,
                            KindStatement = x.First().KindStatement,
                            Description = x.First().Description,
                            PreviousAppealCits = x.First().PreviousAppealCits,
                            ManagingOrganization = x.First().ManagingOrganization,
                            Surety = x.First().Surety,
                            SuretyResolve = x.First().SuretyResolve,
                            SuretyDate = x.First().SuretyDate.ToDateTime(),
                            CheckTime = x.First().CheckTime.ToDateTime(),
                            Executant = x.First().Executant,
                            Tester = testers.Get(x.First().Id)
                        });

                addressDict = appeals
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(y => y.Address).Aggregate((current, next) => string.Format("{0}, {1}", current, next)));
            }
            else
            {
                appealCits = appealCitsWithRo
                    .OrderBy(x => x.Municipality)
                    .ThenBy(x => x.Address)
                    .ThenBy(x => x.DateFrom)
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key,
                        x => new AppealCitProxy
                        {
                            Id = x.First().Id,
                            DateFrom = x.First().DateFrom,
                            NumberGji = x.First().NumberGji,
                            Municipality = x.First().Municipality,
                            Correspondent = x.First().Correspondent,
                            IsFile = x.First().IsFile,
                            KindStatement = x.First().KindStatement,
                            Description = x.First().Description,
                            PreviousAppealCits = x.First().PreviousAppealCits,
                            ManagingOrganization = x.First().ManagingOrganization,
                            Surety = x.First().Surety,
                            SuretyResolve = x.First().SuretyResolve,
                            SuretyDate = x.First().SuretyDate.ToDateTime(),
                            CheckTime = x.First().CheckTime.ToDateTime(),
                            Executant = x.First().Executant,
                            Tester = testers.Get(x.First().Id)
                            
                        });

                addressDict = appealCitsWithRo
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(y => y.Address).Aggregate((current, next) => string.Format("{0}, {1}", current, next)));
            }
                        
            var appealCitsIds = appealCits.Select(x => x.Key).ToArray();

            var appealCitsAnswerDomain = Container.Resolve<IDomainService<AppealCitsAnswer>>();

            var answer = new List<AnswerProxy>();

            for (var i = 0; i < appealCitsIds.Length; i += 1000)
            {
                var takeCount = appealCitsIds.Length - i < 1000 ? appealCitsIds.Length - i : 1000;
                var tempList = appealCitsIds.Skip(i).Take(takeCount).ToArray();

                answer.AddRange(appealCitsAnswerDomain.GetAll()
                    .Where(x => tempList.Contains(x.AppealCits.Id))
                    .Select(x => new AnswerProxy
                        {
                        AppealCitsId = x.AppealCits.Id,
                        DocumentNumber = x.DocumentNumber,
                        DocumentDate = x.DocumentDate.ToStr()
                    }));
            }

            var answerData = answer.GroupBy(x => x.AppealCitsId).ToDictionary(x => x.Key, x => x.First());
            
            var sourcesDict = this.GetSources(appealCitsIds);

            var baseStatementList = this.GetBaseStatement(appealCitsIds);
            var answersDict = this.GetAnswers(appealCitsIds);
            var baseStatementIds = baseStatementList.Select(x => x.BaseStatementId).Distinct().ToArray();

            this.FillSignControl(baseStatementList, answersDict);

            var documentsDict = this.GetDocuments(baseStatementIds);
            var definitionsDict = this.GetDefinitions(baseStatementIds);

            var statSubjectAndFeatureDict = this.GetStatSubject(appealCitsIds);

            this.FillVerticalSection(reportParams);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var num = 1;
            var baseStatementDict = baseStatementList.GroupBy(x => x.AppealCitsId)
                                                     .ToDictionary(x => x.Key, x => x.ToList());
            foreach (var appealCit in appealCits)
            {
                var baseStatements = baseStatementDict.ContainsKey(appealCit.Value.Id)
                                         ? baseStatementDict[appealCit.Value.Id]
                                         : null;

                var sources = sourcesDict.ContainsKey(appealCit.Value.Id) ? sourcesDict[appealCit.Value.Id] : null;
                var statSubjectAndFeature = statSubjectAndFeatureDict.ContainsKey(appealCit.Value.Id)
                                                ? statSubjectAndFeatureDict[appealCit.Value.Id]
                                                : null;
                var answerAppeal = answerData.ContainsKey(appealCit.Value.Id) ? answerData[appealCit.Value.Id] : null;

                FillAppealCitsSection(section, num, appealCit.Value, addressDict[appealCit.Key], sources, statSubjectAndFeature, answerAppeal);
                num++;

                if (baseStatements == null || baseStatements.Count == 0)
                {
                    continue;
                }

                for (var index = 0; index < baseStatements.Count; index++)
                {

                    if (index > 0)
                    {
                        FillAppealCitsSection(section, num, appealCit.Value, addressDict[appealCit.Key], sources, statSubjectAndFeature, answerAppeal);
                        num++;
                    }

                    var baseStatement = baseStatements[index];
                    var answers = answersDict.ContainsKey(baseStatement.AppealCitsId)
                                      ? answersDict[baseStatement.AppealCitsId]
                                      : null;
                    
                    section["signControl"] = baseStatement.SignControl;
                    section["expired"] = baseStatement.SignControl == "На контроле" ? "1" : string.Empty;

                    if (answers != null)
                    {
                        section["countAnswer"] = answers.Count;
                    }

                    var documents = documentsDict.ContainsKey(baseStatement.BaseStatementId)
                                        ? documentsDict[baseStatement.BaseStatementId]
                                        : null;
                    if (documents != null)
                    {
                        section["disposals"] = documents.Disposal;
                        section["actsChecks"] = documents.ActCheck;
                        section["prescriptions"] = documents.Prescription;
                        section["protocols"] = documents.Protocol;
                        section["resolution"] = documents.Resolution;
                    }

                    var definitions = definitionsDict.ContainsKey(baseStatement.BaseStatementId)
                                          ? definitionsDict[baseStatement.BaseStatementId]
                                          : null;
                    section["definitions"] = definitions;
                }
            }
        }

        private static void FillAppealCitsSection(
            Section section,
            int num,
            AppealCitProxy appealCit,
            string address,
            SourcesProxy sources,
            StatSubjectAndFeature statSubjectAndFeature,
            AnswerProxy answersData
            )
        {
            section.ДобавитьСтроку();
            section["num"] = num;
            section["dateFrom"] = appealCit.DateFrom;
            section["numberGji"] = appealCit.NumberGji;
            section["municipality"] = appealCit.Municipality;
            section["address"] = address;
            section["correspondent"] = appealCit.Correspondent;
            section["isFile"] = appealCit.IsFile ? "1" : string.Empty;
            section["kindStatement"] = appealCit.KindStatement;
            section["description"] = appealCit.Description;
            section["previousAppealCits"] = appealCit.PreviousAppealCits;
            section["manOrg"] = appealCit.ManagingOrganization;
            section["surety"] = appealCit.Surety;
            section["resolveGji"] = appealCit.SuretyResolve;
            section["suretyDate"] = appealCit.SuretyDate.HasValue && appealCit.SuretyDate.Value != DateTime.MinValue ? appealCit.SuretyDate.Value.ToShortDateString() : string.Empty;
            section["checkTime"] = appealCit.CheckTime.HasValue && appealCit.CheckTime.Value != DateTime.MinValue ? appealCit.CheckTime.Value.ToShortDateString() : string.Empty;
            section["executant"] = appealCit.Executant;
            section["tester"] = appealCit.Tester;

            if (answersData != null)
            {
                section["answerDocumentNumber"] = answersData.DocumentNumber;
                section["answerDocumentDate"] = answersData.DocumentDate; 
            }
            
            if (sources != null)
            {
                section["revenueForm"] = sources.RevenueForm;
                section["revenueSource"] = sources.RevenueSource;
                section["revenueSourceNumber"] = sources.RevenueSourceNumber;
                section["revenueDate"] = sources.RevenueDate;
            }

            if (statSubjectAndFeature != null)
            {
                long sumStatSubject = 0;
                foreach (var statSubject in statSubjectAndFeature.StatSubject)
                {
                    section[string.Format("statSubjectCode_{0}", statSubject.Key)] = statSubject.Value;
                    sumStatSubject += statSubject.Value.ToLong();
                }

                section["statSubjectCode_Sum"] = sumStatSubject;

                long sumFeature = 0;
                foreach (var feature in statSubjectAndFeature.Feature)
                {
                    section[string.Format("featureCode_{0}", feature.Key)] = feature.Value;
                    sumFeature += feature.Value.ToLong();
                }

                section["featureCode_Sum"] = sumFeature;
            }
        }

        /// <summary>
        /// заполнение вертикальной секции
        /// </summary>
        /// <param name="reportParams"></param>
        private void FillVerticalSection(ReportParams reportParams)
        {
            var numCol = 26;
            var servStatSubjectGji = Container.Resolve<IDomainService<StatSubjectGji>>();
            var servFeatureViolGji = Container.Resolve<IDomainService<StatSubsubjectGji>>();

            // получаем все тематики из справочника
            var statSubjects = servStatSubjectGji.GetAll().Select(x => new { x.Name, x.Code }).ToDictionary(x => x.Code, y => y.Name);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("verticalSection");

            section.ДобавитьСтроку();
            section["name"] = "Кол-во тематик";
            section["numCol"] = ++numCol;
            section["code"] = "$statSubjectCode_Sum$";

            // проходим по списку всех кодов и добавляем столбцы с переменными в вертикальную секцию
            foreach (var rec in statSubjects)
            {
                section.ДобавитьСтроку();

                section["name"] = rec.Value;
                section["numCol"] = ++numCol;
                section["code"] = string.Format("$statSubjectCode_{0}$", rec.Key);
            }

            section.ДобавитьСтроку();
            section["name"] = "Кол-во доп. тематик";
            section["numCol"] = ++numCol;
            section["code"] = "$featureCode_Sum$";

            // получаем все характеристики из справочника
            var features = servFeatureViolGji.GetAll().Select(x => new { x.Name, x.Code }).ToDictionary(x => x.Code, y => y.Name);

            // проходим по списку всех кодов  и добавляем столбцы с переменными в вертикальную секцию
            foreach (var rec in features)
            {
                section.ДобавитьСтроку();

                section["name"] = rec.Value;
                section["numCol"] = ++numCol;
                section["code"] = string.Format("$featureCode_{0}$", rec.Key);
            }

            reportParams.SimpleReportParams["numColManOrg"] = ++numCol;
            reportParams.SimpleReportParams["numColNote"] = ++numCol;
            reportParams.SimpleReportParams["numColFioReg"] = ++numCol;

            reportParams.SimpleReportParams["numColDisposals"] = ++numCol;
            reportParams.SimpleReportParams["numColActs"] = ++numCol;
            reportParams.SimpleReportParams["numColPrescriptions"] = ++numCol;
            reportParams.SimpleReportParams["numColProtocols"] = ++numCol;
            reportParams.SimpleReportParams["numColResolution"] = ++numCol;
            reportParams.SimpleReportParams["numColDefinitions"] = ++numCol;
        }

        /// <summary>
        /// Заполнение признака контроля
        /// </summary>
        /// <param name="baseStatementDict"></param>
        /// <param name="answers"></param>
        private void FillSignControl(List<BaseStatementProxy> baseStatementDict, IDictionary<long, AnswerProxy> answers)
        {
            var servInspectionGjiViol = Container.Resolve<IDomainService<InspectionGjiViol>>();
            var baseStatementWithOneAndTwo = baseStatementDict.Where(x => x.SuretyResolveCode == "1" || x.SuretyResolveCode == "2").Select(x => x.BaseStatementId).ToArray();

            var viol = new List<KeyValuePair<long, DateTime?>>();
            for (var i = 0; i < baseStatementWithOneAndTwo.Length; i += 1000)
            {
                var takeCount = baseStatementWithOneAndTwo.Length - i < 1000 ? baseStatementWithOneAndTwo.Length - i : 1000;
                var tempList = baseStatementWithOneAndTwo.Skip(i).Take(takeCount).ToArray();

                viol.AddRange(servInspectionGjiViol.GetAll()
                             .Where(x => tempList.Contains(x.Inspection.Id))
                             .Select(x => new KeyValuePair<long, DateTime?>(x.Inspection.Id, x.DateFactRemoval))
                             .ToList());
            }

            // Ставим true если все нарушения закрыты
            var violDict = viol.GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.All(y => y.Value.HasValue));

            foreach (var baseStatement in baseStatementDict.Where(x => x.SuretyResolveCode == "1" || x.SuretyResolveCode == "2"))
            {
                // Если нет нарушений в violDict, значит их не нашли или не добавили, поэтому ставим  "Снято с контроля"
                var closedViol = !violDict.ContainsKey(baseStatement.BaseStatementId) || violDict[baseStatement.BaseStatementId];
                baseStatement.SignControl = closedViol ? "Снято с контроля" : "На контроле";
            }

            foreach (var baseStatement in baseStatementDict.Where(x => x.SuretyResolveCode == "3" || x.SuretyResolveCode == "4"))
            {
                var isDocumentNumber = answers.ContainsKey(baseStatement.AppealCitsId) && answers[baseStatement.AppealCitsId].IsDocumentNumber;
                baseStatement.SignControl = isDocumentNumber ? "Снято с контроля" : "На контроле";
            }

            foreach (var baseStatement in baseStatementDict.Where(x => x.SuretyResolveCode == "5"))
            {
                baseStatement.SignControl = "Снято с контроля";
            }

            foreach (var baseStatement in baseStatementDict.Where(x => string.IsNullOrEmpty(x.SignControl)))
            {
                baseStatement.SignControl = "Снято с контроля";
            }
        }

        private Dictionary<long, InfoForDocuments> GetDocuments(ICollection<long> baseStatementIds)
        {
            var servDocumentGji = Container.Resolve<IDomainService<DocumentGji>>();

            var documentList = new List<DocumentGji>();

            for (var i = 0; i < baseStatementIds.Count; i += 1000)
            {
                var takeCount = baseStatementIds.Count - i < 1000 ? baseStatementIds.Count - i : 1000;
                var tempList = baseStatementIds.Skip(i).Take(takeCount).ToArray();

                documentList.AddRange(servDocumentGji.GetAll()
                    .Where(x => tempList.Contains(x.Inspection.Id))
                    .Where(x => x.TypeDocumentGji != TypeDocumentGji.ActRemoval)
                    .Where(x => x.TypeDocumentGji != TypeDocumentGji.ResolutionProsecutor)
                    .Where(x => x.TypeDocumentGji != TypeDocumentGji.Presentation)
                    .Select(x => new DocumentGji
                    {
                        Inspection = new InspectionGji { Id = x.Inspection.Id },
                        TypeDocumentGji = x.TypeDocumentGji,
                        DocumentNumber = x.DocumentNumber,
                        DocumentDate = x.DocumentDate
                    }));
            }

            var docsDict = documentList
                  .GroupBy(x => x.Inspection.Id)
                  .ToDictionary(
                   x => x.Key,
                   x => new 
                   {
                       Disposal = x.Where(y => y.TypeDocumentGji == TypeDocumentGji.Disposal).Select(y => y.DocumentNumber + (y.DocumentDate.HasValue ? " от " + y.DocumentDate.Value.ToShortDateString() : string.Empty)).ToArray(),
                       // В связи с задачей 38855 заменил акт обследования на акт проверки
                       ActCheck = x.Where(y => y.TypeDocumentGji == TypeDocumentGji.ActCheck).Select(y => y.DocumentNumber + (y.DocumentDate.HasValue ? " от " + y.DocumentDate.Value.ToShortDateString() : string.Empty)).ToArray(),
                       // ActSurvey = x.Where(y => y.TypeDocumentGji == TypeDocumentGji.ActSurvey).Select(y => y.DocumentNumber + (y.DocumentDate.HasValue ? " от " + y.DocumentDate.Value.ToShortDateString() : string.Empty)).ToArray(),
                       Prescription = x.Where(y => y.TypeDocumentGji == TypeDocumentGji.Prescription).Select(y => y.DocumentNumber + (y.DocumentDate.HasValue ? " от " + y.DocumentDate.Value.ToShortDateString() : string.Empty)).ToArray(),
                       Protocol = x.Where(y => y.TypeDocumentGji == TypeDocumentGji.Protocol).Select(y => y.DocumentNumber + (y.DocumentDate.HasValue ? " от " + y.DocumentDate.Value.ToShortDateString() : string.Empty)).ToArray(),
                       Resolution = x.Where(y => y.TypeDocumentGji == TypeDocumentGji.Resolution).Select(y => y.DocumentNumber + (y.DocumentDate.HasValue ? " от " + y.DocumentDate.Value.ToShortDateString() : string.Empty)).ToArray(),
                   });

            return docsDict
                 .ToDictionary(
                  x => x.Key,
                  x => new InfoForDocuments
                  {
                      Disposal = x.Value.Disposal.Length > 0 ? x.Value.Disposal.Aggregate((current, next) => string.Format("{0}, {1}", current, next)) : string.Empty,
                      ActCheck = x.Value.ActCheck.Length > 0 ? x.Value.ActCheck.Aggregate((current, next) => string.Format("{0}, {1}", current, next)) : string.Empty,
                      Prescription = x.Value.Prescription.Length > 0 ? x.Value.Prescription.Aggregate((current, next) => string.Format("{0}, {1}", current, next)) : string.Empty,
                      Protocol = x.Value.Protocol.Length > 0 ? x.Value.Protocol.Aggregate((current, next) => string.Format("{0}, {1}", current, next)) : string.Empty,
                      Resolution = x.Value.Resolution.Length > 0 ? x.Value.Resolution.Aggregate((current, next) => string.Format("{0}, {1}", current, next)) : string.Empty
                  });
        }

        private Dictionary<long, string> GetDefinitions(ICollection<long> baseStatementIds)
        {
            var servActCheckDefinition = Container.Resolve<IDomainService<ActCheckDefinition>>();
            var servProtocolDefinition = Container.Resolve<IDomainService<ProtocolDefinition>>();
            var servResolutionDefinition = Container.Resolve<IDomainService<ResolutionDefinition>>();

            var documentList = new List<DefinitionProxy>();

            for (var i = 0; i < baseStatementIds.Count; i += 1000)
            {
                var takeCount = baseStatementIds.Count - i < 1000 ? baseStatementIds.Count - i : 1000;
                var tempList = baseStatementIds.Skip(i).Take(takeCount).ToArray();

                documentList.AddRange(servActCheckDefinition.GetAll()
                             .Where(x => tempList.Contains(x.ActCheck.Inspection.Id))
                             .Select(x => new DefinitionProxy
                                              {
                                                  InspectionId = x.ActCheck.Inspection.Id,
                                                  DocumentDate = x.DocumentDate,
                                                  DocumentNum = x.DocumentNum
                                              })
                             .ToList());
            }

            for (var i = 0; i < baseStatementIds.Count; i += 1000)
            {
                var takeCount = baseStatementIds.Count - i < 1000 ? baseStatementIds.Count - i : 1000;
                var tempList = baseStatementIds.Skip(i).Take(takeCount).ToArray();

                documentList.AddRange(servProtocolDefinition.GetAll()
                             .Where(x => tempList.Contains(x.Protocol.Inspection.Id))
                             .Select(x => new DefinitionProxy
                             {
                                 InspectionId = x.Protocol.Inspection.Id,
                                 DocumentDate = x.DocumentDate,
                                 DocumentNum = x.DocumentNum
                             })
                             .ToList());
            }

            for (var i = 0; i < baseStatementIds.Count; i += 1000)
            {
                var takeCount = baseStatementIds.Count - i < 1000 ? baseStatementIds.Count - i : 1000;
                var tempList = baseStatementIds.Skip(i).Take(takeCount).ToArray();

                documentList.AddRange(servResolutionDefinition.GetAll()
                             .Where(x => tempList.Contains(x.Resolution.Inspection.Id))
                             .Select(x => new DefinitionProxy
                             {
                                 InspectionId = x.Resolution.Inspection.Id,
                                 DocumentDate = x.DocumentDate,
                                 DocumentNum = x.DocumentNum
                             })
                             .ToList());
            }

            return documentList
                  .GroupBy(x => x.InspectionId)
                  .ToDictionary(
                   x => x.Key,
                   x => x.Select(y => y.DocumentDate.HasValue ? string.Format("{0} от {1}", y.DocumentNum, y.DocumentDate.Value.ToShortDateString()) : y.DocumentNum)
                       .Aggregate((current, next) => string.Format("{0}, {1}", current, next)));
        }

        private List<BaseStatementProxy> GetBaseStatement(ICollection<long> appealCitsIds)
        {
            var servBaseStatementAppealCits = Container.Resolve<IDomainService<InspectionAppealCits>>();

            var baseStatementList = new List<BaseStatementProxy>();

            for (var i = 0; i < appealCitsIds.Count; i += 1000)
            {
                var takeCount = appealCitsIds.Count - i < 1000 ? appealCitsIds.Count - i : 1000;
                var tempList = appealCitsIds.Skip(i).Take(takeCount).ToArray();

                baseStatementList.AddRange(servBaseStatementAppealCits.GetAll()
                             .Where(x => tempList.Contains(x.AppealCits.Id))
                             .Select(
                                               x =>
                                               new BaseStatementProxy
                                               {
                                                   AppealCitsId = x.AppealCits.Id,
                                                   BaseStatementId = x.Inspection.Id,
                                                   SuretyResolveCode = x.AppealCits.SuretyResolve.Code
                             })
                             .ToList());
            }

            return baseStatementList;
        }

        private Dictionary<long, SourcesProxy> GetSources(ICollection<long> appealCitsIds)
        {
            var servAppealCitsSource = Container.Resolve<IDomainService<AppealCitsSource>>();

            var sourcesList = new List<SourcesProxy>();

            for (var i = 0; i < appealCitsIds.Count; i += 1000)
            {
                var takeCount = appealCitsIds.Count - i < 1000 ? appealCitsIds.Count - i : 1000;
                var tempList = appealCitsIds.Skip(i).Take(takeCount).ToArray();

                sourcesList.AddRange(servAppealCitsSource.GetAll()
                             .Where(x => tempList.Contains(x.AppealCits.Id))
                             .Select(x => new SourcesProxy
                                       {
                                           AppealCitsId = x.AppealCits.Id,
                                           RevenueForm = x.RevenueForm.Name,
                                           RevenueSource = x.RevenueSource.Name,
                                           RevenueSourceNumber = x.RevenueSourceNumber,
                                           RevenueDate = x.RevenueDate.HasValue ? x.RevenueDate.Value.ToShortDateString() : string.Empty
                                       })
                             .ToList());
            }

            return sourcesList
                  .GroupBy(x => x.AppealCitsId)
                  .ToDictionary(
                   x => x.Key,
                   x => new 
                   {
                       RevenueForm = x.Select(y => y.RevenueForm).ToArray(),
                       RevenueSource = x.Select(y => y.RevenueSource).ToArray(),
                       RevenueSourceNumber = x.Where(y => !string.IsNullOrEmpty(y.RevenueSourceNumber)).Select(y => y.RevenueSourceNumber).ToArray(),
                       RevenueDate = x.Select(y => y.RevenueDate).ToArray()
                   })
                   .ToDictionary(
                   x => x.Key,
                   x => new SourcesProxy
                   {
                       RevenueForm = x.Value.RevenueForm.Length > 0 ? x.Value.RevenueForm.Aggregate((current, next) => string.Format("{0}, {1}", current, next)) : string.Empty,
                       RevenueSource = x.Value.RevenueSource.Length > 0 ? x.Value.RevenueSource.Aggregate((current, next) => string.Format("{0}, {1}", current, next)) : string.Empty,
                       RevenueSourceNumber = x.Value.RevenueSourceNumber.Length > 0 ? x.Value.RevenueSourceNumber.Aggregate((current, next) => string.Format("{0}, {1}", current, next)) : string.Empty,
                       RevenueDate = x.Value.RevenueDate.Length > 0 ? x.Value.RevenueDate.Aggregate((current, next) => string.Format("{0}, {1}", current, next)) : string.Empty,
                   });
        }

        private Dictionary<long, AnswerProxy> GetAnswers(ICollection<long> appealCitsIds)
        {
            var servAppealCitsAnswer = Container.Resolve<IDomainService<AppealCitsAnswer>>();

            var answerList = new List<AnswerProxy>();
            for (var i = 0; i < appealCitsIds.Count; i += 1000)
            {
                var takeCount = appealCitsIds.Count - i < 1000 ? appealCitsIds.Count - i : 1000;
                var tempList = appealCitsIds.Skip(i).Take(takeCount).ToArray();

                answerList.AddRange(servAppealCitsAnswer.GetAll()
                             .Where(x => tempList.Contains(x.AppealCits.Id))
                             .Select(x => new AnswerProxy
                             {
                                 AppealCitsId = x.AppealCits.Id
                             })
                             .ToList());
            }

            return answerList
                  .GroupBy(x => x.AppealCitsId)
                  .ToDictionary(
                   x => x.Key,
                   x => new
                   {
                       AppealCitsId = x.Key,
                       DocumentNumber = x.Where(y => !string.IsNullOrEmpty(y.DocumentNumber)).Select(y => y.DocumentNumber).ToArray(),
                       DocumentDate = x.Where(y => !string.IsNullOrEmpty(y.DocumentDate)).Select(y => y.DocumentDate).ToArray(),
                       Count = x.Count()
                   })
                  .ToDictionary(
                   x => x.Key,
                   x => new AnswerProxy
                   {
                       AppealCitsId = x.Value.AppealCitsId,
                       DocumentNumber = x.Value.DocumentNumber.Length > 0 ? x.Value.DocumentNumber.Aggregate((current, next) => string.Format("{0}, {1}", current, next)) : string.Empty,
                       DocumentDate = x.Value.DocumentDate.Length > 0 ? x.Value.DocumentDate.Aggregate((current, next) => string.Format("{0}, {1}", current, next)) : string.Empty,
                       Count = x.Value.Count
                   });
        }

        private Dictionary<long, StatSubjectAndFeature> GetStatSubject(ICollection<long> appealCitsIds)
        {
            var servStatSubject = Container.Resolve<IDomainService<AppealCitsStatSubject>>();

            var statSubjectList = new List<AppCitStatSubject>();

            for (var i = 0; i < appealCitsIds.Count; i += 1000)
            {
                var takeCount = appealCitsIds.Count - i < 1000 ? appealCitsIds.Count - i : 1000;
                var tempList = appealCitsIds.Skip(i).Take(takeCount).ToArray();

                statSubjectList.AddRange(servStatSubject.GetAll()
                             .Where(x => tempList.Contains(x.AppealCits.Id))
                             .Select(x => new AppCitStatSubject
                             {
                                 AppCitId = x.AppealCits.Id,
                                 SubjectCode = x.Subject.Code,
                                 SubsubjectCode = x.Subsubject.Code
                             }));
            }

            return statSubjectList
                .GroupBy(x => x.AppCitId)
                                  .ToDictionary(
                                      x => x.Key,
                                      x =>
                                      new StatSubjectAndFeature
                                          {
                        StatSubject = x.Where(y => y.SubjectCode != null).Select(y => y.SubjectCode).Distinct().ToDictionary(y => y, y => "1"),
                        Feature = x.Where(y => y.SubsubjectCode != null).Select(y => y.SubsubjectCode).Distinct().ToDictionary(y => y, y => "1")
                                          });
        }

        private class AppealCitProxy
        {
            public long Id { get; set; }

            public DateTime? DateFrom { get; set; }

            public string NumberGji { get; set; }

            public string Municipality { get; set; }

            public string Correspondent { get; set; }

            public bool IsFile { get; set; }

            public string KindStatement { get; set; }

            public string Description { get; set; }

            public string PreviousAppealCits { get; set; }

            public string ManagingOrganization { get; set; }

            public string Surety { get; set; }

            public string SuretyResolve { get; set; }

            public DateTime? SuretyDate { get; set; }

            public string Executant { get; set; }

            public string Tester { get; set; }

            public DateTime? CheckTime { get; set; }

        }

        private class AnswerProxy
        {
            public long AppealCitsId { get; set; }

            public string DocumentNumber { get; set; }

            public bool IsDocumentNumber
            {
                get
                {
                    return !string.IsNullOrEmpty(DocumentNumber);
                }
            }

            public string DocumentDate { get; set; }

            public int Count { get; set; }
        }

        private  class SourcesProxy
        {
            public long AppealCitsId { get; set; }

            public string RevenueForm { get; set; }

            public string RevenueSource { get; set; }

            public string RevenueSourceNumber { get; set; }

            public string RevenueDate { get; set; }
        }

        private class StatSubjectAndFeature
        {
            public Dictionary<string, string> StatSubject { get; set; }

            public Dictionary<string, string> Feature { get; set; }
        }

        private class AppCitStatSubject
        {
            public long AppCitId { get; set; }

            public string SubjectCode { get; set; }

            public string SubsubjectCode { get; set; }
        }

        private class BaseStatementProxy
        {
            public long AppealCitsId { get; set; }

            public long BaseStatementId { get; set; }

            public string Surety { get; set; }

            public string SuretyResolve { get; set; }

            public string SuretyResolveCode { get; set; }

            public string SuretyDate { get; set; }

            public string CheckTime { get; set; }

            public string Executant { get; set; }

            public string Tester { get; set; }

            public string SignControl { get; set; }

        }

        private sealed class InfoForDocuments
        {
            public string Disposal { get; set; }

            public string ActCheck { get; set; }

            public string Prescription { get; set; }

            public string Protocol { get; set; }

            public string Resolution { get; set; }
        }

        private sealed class DefinitionProxy
        {
            public long InspectionId { get; set; }

            public DateTime? DocumentDate { get; set; }

            public string DocumentNum { get; set; }
        }
    }
}
