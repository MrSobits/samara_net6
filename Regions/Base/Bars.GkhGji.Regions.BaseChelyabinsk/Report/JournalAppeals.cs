namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Entities.AppealCits;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;
    using Bars.GkhGji.Report;

    /// <summary>
    /// Отчёт "Журнал обращений"
    /// </summary>
    public class JournalAppeals : BasePrintForm
    {
        protected DateTime dateStart = DateTime.MinValue;
        protected DateTime dateEnd = DateTime.MaxValue;
        protected long[] municipalityIds;

        /// <summary>
        /// Конструктор отчёта
        /// </summary>
        public JournalAppeals()
            : base(new ReportTemplateBinary(Properties.Resources.JournalAppeals))
        {
        }

        public IAppealCitsService<ViewAppealCitizens> AppealCitsService { get; set; }

        /// <summary>
        /// Windsor-контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис обращений
        /// </summary>
        public IDomainService<ViewAppealCitizens> AppealCitizensViewDomain { get; set; }


        /// <summary>
        /// Разрешения
        /// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.JournalAppeals";
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption
        {
            get { return "Журнал обращений (Челябинск)"; }
        }

        /// <summary>
        /// Наименование группы
        /// </summary>
        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        /// <summary>
        /// Контроллер параметров
        /// </summary>
        public override string ParamsController
        {
            get { return "B4.controller.report.JournalAppeals"; }
        }


        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Журнал обращений"; }
        }

        /// <summary>
        /// Установка параметров
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        public override void SetUserParams(BaseParams baseParams)
        {
            this.dateStart = baseParams.Params["dateStart"].ToDateTime();
            this.dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            var m = baseParams.Params["municipalityIds"].ToString();
            this.municipalityIds = !string.IsNullOrEmpty(m) ? m.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
        }

        /// <summary>
        /// Генератор отчёта
        /// </summary>
        public override string ReportGenerator { get; set; }

        public class ExecutantsAppCit
        {
            public string Author { get; set; }
            public string Executant { get; set; }
            public string Controller { get; set; }
        }
         

        /// <summary>
        /// Подготовка отчёта
        /// </summary>
        /// <param name="reportParams">Параметры отчёта</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["period"] = string.Format(
                "{0} по {1}", this.dateStart.ToShortDateString(), this.dateEnd.ToShortDateString());

            // Получим обращения в выбранных Муниципальных образований, если они есть, если нет то все у кого забиты дома
            var appealCitsQuery = this.AppealCitsService.FilterByActiveAppealCits(this.AppealCitizensViewDomain.GetAll(), x => x.AppealCits.State)
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains((long)x.MunicipalityId) || !x.MunicipalityId.HasValue)
                .Where(x => x.AppealCits.DateFrom.HasValue && x.AppealCits.DateFrom.Value >= this.dateStart && x.AppealCits.DateFrom.Value <= this.dateEnd);

            // Идентификатор МО = Регистрация обращений (для Самары)
            var regAppealsMunicipalityId =
                this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                    .Where(x => x.Name == "Регистрация обращений")
                    .Select(x => x.Id)
                    .FirstOrDefault();

            // исполнители для Челябинска
            var appCitExecutantDomain = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();

            var appCitExecunants = appCitExecutantDomain.GetAll()
                .Where(x => appealCitsQuery.Any(y => y.AppealCits.Id == x.AppealCits.Id))
                .Select(x => new
                {
                    x.Id,
                    x.AppealCits,
                    Author = x.Author != null ? x.Author.Fio : "",
                    Executant = x.Executant != null ? x.Executant.Fio : "",
                    Controller = x.Controller != null ? x.Controller.Fio : ""
                });
            Dictionary<long, ExecutantsAppCit> executantDict = new Dictionary<long, ExecutantsAppCit>();
            foreach (var zap in appCitExecunants)
            {
                if (!executantDict.ContainsKey(zap.AppealCits.Id))
                {
                    executantDict.Add(zap.AppealCits.Id, new ExecutantsAppCit { Author = zap.Author, Controller = zap.Controller, Executant = zap.Executant });
                }
                //else
                //{
                //    if(zap.Executant != "")
                //    executantDict[zap.AppealCits.Id].Executant = executantDict[zap.AppealCits.Id].Executant + "; " + zap.Executant;
                //    if (zap.Author != "")
                //        executantDict[zap.AppealCits.Id].Author = executantDict[zap.AppealCits.Id].Author + "; " + zap.Author;
                //    if (zap.Controller != "")
                //        executantDict[zap.AppealCits.Id].Controller = executantDict[zap.AppealCits.Id].Controller + "; " + zap.Controller;
                //}
            }
              
                

            var appealCits = new Dictionary<long, AppealCitProxy>();
            var addressDict = new Dictionary<long, string>();

            //Меняем исполнителей
            var appealCitsWithRo = appealCitsQuery
               .Select(x => new
               {
                   x.AppealCits.Id,
                   x.AppealCits.DateFrom,
                   NumberGji = string.IsNullOrEmpty(x.AppealCits.NumberGji) ? x.AppealCits.DocumentNumber : x.AppealCits.NumberGji,
                   x.Municipality,
                   x.AppealCits.Correspondent,
                   IsFile = x.AppealCits.File != null,
                   KindStatement = x.AppealCits.KindStatement.Name,
                   //x.AppealCits.Description,
                   PreviousAppealCits = x.AppealCits.PreviousAppealCits.NumberGji ?? string.Empty,
                   ManagingOrganization = x.AppealCits.ManagingOrganization.Contragent.Name ?? string.Empty,
                   Address = x.RealObjAddresses,
                   Surety = executantDict.ContainsKey(x.AppealCits.Id)? executantDict[x.AppealCits.Id].Author: "",
                   SuretyResolve = x.AppealCits.SuretyResolve.Name,
                   SuretyDate = x.AppealCits.SuretyDate.HasValue && x.AppealCits.SuretyDate.Value != DateTime.MinValue ? x.AppealCits.SuretyDate.Value.ToShortDateString() : string.Empty,
                   Executant = executantDict.ContainsKey(x.AppealCits.Id) ? executantDict[x.AppealCits.Id].Executant : "",
                   Tester = executantDict.ContainsKey(x.AppealCits.Id) ? executantDict[x.AppealCits.Id].Controller : "",
                   CheckTime = x.AppealCits.CheckTime.HasValue && x.AppealCits.CheckTime.Value != DateTime.MinValue ? x.AppealCits.CheckTime.Value.ToShortDateString() : string.Empty
               })
               .ToList();

            //var appealCitsWithRo = appealCitsQuery
            //    .Select(x => new
            //    {
            //        x.AppealCits.Id,
            //        x.AppealCits.DateFrom,
            //        NumberGji = string.IsNullOrEmpty(x.AppealCits.NumberGji) ? x.AppealCits.DocumentNumber : x.AppealCits.NumberGji,
            //        x.Municipality,
            //        x.AppealCits.Correspondent,
            //        IsFile = x.AppealCits.File != null,
            //        KindStatement = x.AppealCits.KindStatement.Name,
            //        x.AppealCits.Description,
            //        PreviousAppealCits = x.AppealCits.PreviousAppealCits.NumberGji ?? string.Empty,
            //        ManagingOrganization = x.AppealCits.ManagingOrganization.Contragent.Name ?? string.Empty,
            //        Address = x.RealObjAddresses,
            //        Surety = x.AppealCits.Surety.Fio,
            //        SuretyResolve = x.AppealCits.SuretyResolve.Name,
            //        SuretyDate = x.AppealCits.SuretyDate.HasValue && x.AppealCits.SuretyDate.Value != DateTime.MinValue ? x.AppealCits.SuretyDate.Value.ToShortDateString() : string.Empty,
            //        Executant = x.AppealCits.Executant.Fio,
            //        Tester = x.AppealCits.Tester.Fio,
            //        CheckTime = x.AppealCits.CheckTime.HasValue && x.AppealCits.CheckTime.Value != DateTime.MinValue ? x.AppealCits.CheckTime.Value.ToShortDateString() : string.Empty
            //    })
            //    .ToList();

            // проверка на наличие МО = Регистрация обращений (для Самары)
            if (regAppealsMunicipalityId > 0)
            {
                var appealCitsWithRoIdsQuery = appealCitsQuery.Select(x => x.AppealCits.Id);
                //Меняем исполнителей
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
                   //x.Description,
                   PreviousAppealCits = x.PreviousAppealCits.NumberGji ?? string.Empty,
                   ManagingOrganization = x.ManagingOrganization.Contragent.Name ?? string.Empty,
                   Address = string.Empty,
                   Surety = executantDict.ContainsKey(x.Id) ? executantDict[x.Id].Author : "",
                   SuretyResolve = x.SuretyResolve.Name,
                   SuretyDate = x.SuretyDate.HasValue && x.SuretyDate.Value != DateTime.MinValue ? x.SuretyDate.Value.ToShortDateString() : string.Empty,
                   Executant = executantDict.ContainsKey(x.Id) ? executantDict[x.Id].Executant : "",
                   Tester = executantDict.ContainsKey(x.Id) ? executantDict[x.Id].Controller : "",
                   CheckTime = x.CheckTime.HasValue && x.CheckTime.Value != DateTime.MinValue ? x.CheckTime.Value.ToShortDateString() : string.Empty
               }).ToList();

                //var appealCitsWithOutRo = this.Container.Resolve<IDomainService<AppealCits>>().GetAll()
                //.Where(x => x.DateFrom.HasValue && x.DateFrom.Value >= this.dateStart && x.DateFrom.Value <= this.dateEnd)
                //.Where(x => !appealCitsWithRoIdsQuery.Contains(x.Id))
                //.Select(x => new
                //{
                //    x.Id,
                //    x.DateFrom,
                //    x.NumberGji,
                //    Municipality = "Регистрация обращений",
                //    x.Correspondent,
                //    IsFile = x.File != null,
                //    KindStatement = x.KindStatement.Name,
                //    x.Description,
                //    PreviousAppealCits = x.PreviousAppealCits.NumberGji ?? string.Empty,
                //    ManagingOrganization = x.ManagingOrganization.Contragent.Name ?? string.Empty,
                //    Address = string.Empty,
                //    Surety = x.Surety.Fio,
                //    SuretyResolve = x.SuretyResolve.Name,
                //    SuretyDate = x.SuretyDate.HasValue && x.SuretyDate.Value != DateTime.MinValue ? x.SuretyDate.Value.ToShortDateString() : string.Empty,
                //    Executant = x.Executant.Fio,
                //    Tester = x.Tester.Fio,
                //    CheckTime = x.CheckTime.HasValue && x.CheckTime.Value != DateTime.MinValue ? x.CheckTime.Value.ToShortDateString() : string.Empty
                //}).ToList();

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
                            //Description = x.First().Description,
                            PreviousAppealCits = x.First().PreviousAppealCits,
                            ManagingOrganization = x.First().ManagingOrganization,
                            Surety = x.First().Surety,
                            SuretyResolve = x.First().SuretyResolve,
                            SuretyDate = x.First().SuretyDate.ToDateTime(),
                            CheckTime = x.First().CheckTime.ToDateTime(),
                            Executant = x.First().Executant,
                            Tester = x.First().Tester
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
                            //Description = x.First().Description,
                            PreviousAppealCits = x.First().PreviousAppealCits,
                            ManagingOrganization = x.First().ManagingOrganization,
                            Surety = x.First().Surety,
                            SuretyResolve = x.First().SuretyResolve,
                            SuretyDate = x.First().SuretyDate.ToDateTime(),
                            CheckTime = x.First().CheckTime.ToDateTime(),
                            Executant = x.First().Executant,
                            Tester = x.First().Tester

                        });

                addressDict = appealCitsWithRo
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(y => y.Address).Aggregate((current, next) => string.Format("{0}, {1}", current, next)));
            }

            var appealCitsIds = appealCits.Select(x => x.Key).ToArray();

            var appealCitsAnswerDomain = this.Container.Resolve<IDomainService<AppealCitsAnswer>>();

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

                JournalAppeals.FillAppealCitsSection(section, num, appealCit.Value, addressDict[appealCit.Key], sources, statSubjectAndFeature, answerAppeal);
                num++;

                if (baseStatements == null || baseStatements.Count == 0)
                {
                    continue;
                }

                for (var index = 0; index < baseStatements.Count; index++)
                {

                    if (index > 0)
                    {
                        JournalAppeals.FillAppealCitsSection(section, num, appealCit.Value, addressDict[appealCit.Key], sources, statSubjectAndFeature, answerAppeal);
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

        protected static void FillAppealCitsSection(
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
            //section["description"] = appealCit.Description;
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
        protected void FillVerticalSection(ReportParams reportParams)
        {
            var numCol = 26;
            var servStatSubjectGji = this.Container.Resolve<IDomainService<StatSubjectGji>>();
            var servFeatureViolGji = this.Container.Resolve<IDomainService<StatSubsubjectGji>>();

            // получаем все тематики из справочника
            var statSubjects = servStatSubjectGji.GetAll().Where(x => x.Code != null).Select(x => new { x.Name, x.Code }).ToDictionary(x => x.Code, y => y.Name);

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
            // проходим по списку всех кодов  и добавляем столбцы с переменными в вертикальную секцию
            servFeatureViolGji.GetAll().ToList().ForEach(x =>
            {
                section.ДобавитьСтроку();

                section["name"] = x.Name;
                section["numCol"] = ++numCol;
                section["code"] = string.Format("$featureCode_{0}$", x.Code);
            });

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
        protected void FillSignControl(List<BaseStatementProxy> baseStatementDict, IDictionary<long, AnswerProxy> answers)
        {
            var servInspectionGjiViol = this.Container.Resolve<IDomainService<InspectionGjiViol>>();
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

        protected Dictionary<long, InfoForDocuments> GetDocuments(ICollection<long> baseStatementIds)
        {
            var servDocumentGji = this.Container.Resolve<IDomainService<DocumentGji>>();

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

        protected Dictionary<long, string> GetDefinitions(ICollection<long> baseStatementIds)
        {
            var servActCheckDefinition = this.Container.Resolve<IDomainService<ActCheckDefinition>>();
            var servProtocolDefinition = this.Container.Resolve<IDomainService<ProtocolDefinition>>();
            var servResolutionDefinition = this.Container.Resolve<IDomainService<ResolutionDefinition>>();

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

        protected List<BaseStatementProxy> GetBaseStatement(ICollection<long> appealCitsIds)
        {
            var servBaseStatementAppealCits = this.Container.Resolve<IDomainService<InspectionAppealCits>>();

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

        protected Dictionary<long, SourcesProxy> GetSources(ICollection<long> appealCitsIds)
        {
            var servAppealCitsSource = this.Container.Resolve<IDomainService<AppealCitsSource>>();

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

        protected Dictionary<long, AnswerProxy> GetAnswers(ICollection<long> appealCitsIds)
        {
            var servAppealCitsAnswer = this.Container.Resolve<IDomainService<AppealCitsAnswer>>();

            var answerList = new List<AnswerProxy>();
            for (var i = 0; i < appealCitsIds.Count; i += 1000)
            {
                var takeCount = appealCitsIds.Count - i < 1000 ? appealCitsIds.Count - i : 1000;
                var tempList = appealCitsIds.Skip(i).Take(takeCount).ToArray();

                answerList.AddRange(this.AppealCitsService.FilterByActiveAppealCits(servAppealCitsAnswer.GetAll(), x => x.AppealCits.State)
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

        protected Dictionary<long, StatSubjectAndFeature> GetStatSubject(ICollection<long> appealCitsIds)
        {
            var servStatSubject = this.Container.Resolve<IDomainService<AppealCitsStatSubject>>();

            var statSubjectList = new List<AppCitStatSubject>();

            for (var i = 0; i < appealCitsIds.Count; i += 1000)
            {
                var takeCount = appealCitsIds.Count - i < 1000 ? appealCitsIds.Count - i : 1000;
                var tempList = appealCitsIds.Skip(i).Take(takeCount).ToArray();

                statSubjectList.AddRange(this.AppealCitsService.FilterByActiveAppealCits(servStatSubject.GetAll(), x => x.AppealCits.State)
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
    }
}
