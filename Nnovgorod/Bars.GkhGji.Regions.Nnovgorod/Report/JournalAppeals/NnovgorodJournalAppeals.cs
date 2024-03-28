namespace Bars.GkhGji.Regions.Nnovgorod.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Report;

    using Castle.Windsor;

    /// <summary>
    /// Отчёт "Журнал обращений"
    /// </summary>
    public class NnovgorodJournalAppeals : JournalAppeals
    {

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption
        {
            get { return "Журнал обращений (Нижний Новгород)"; }
        }
        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Журнал обращений (Нижний Новгород)"; }
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
            var appealCitsQuery = this.AppealCitizensViewDomain.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains((long)x.MunicipalityId) || !x.MunicipalityId.HasValue)
                .Where(x => x.AppealCits.DateFrom.HasValue && x.AppealCits.DateFrom.Value >= this.dateStart && x.AppealCits.DateFrom.Value <= this.dateEnd);

            var appealCits = new Dictionary<long, AppealCitProxy>();
            var addressDict = new Dictionary<long, string>();

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
                    x.AppealCits.Description,
                    PreviousAppealCits = x.AppealCits.PreviousAppealCits.NumberGji ?? string.Empty,
                    ManagingOrganization = x.AppealCits.ManagingOrganization.Contragent.Name ?? string.Empty,
                    Address = x.RealObjAddresses,
                    Surety = x.AppealCits.Surety.Fio,
                    SuretyResolve = x.AppealCits.SuretyResolve.Name,
                    SuretyDate =
                            x.AppealCits.SuretyDate.HasValue && x.AppealCits.SuretyDate.Value != DateTime.MinValue
                                ? x.AppealCits.SuretyDate.Value.ToShortDateString()
                                : string.Empty,
                    Executant = x.AppealCits.Executant.Fio,
                    Tester = x.AppealCits.Tester.Fio,
                    CheckTime =
                            x.AppealCits.CheckTime.HasValue && x.AppealCits.CheckTime.Value != DateTime.MinValue
                                ? x.AppealCits.CheckTime.Value.ToShortDateString()
                                : string.Empty
                })
                .AsEnumerable().GroupBy(x => x.NumberGji).Select(
                    x => new
                    {
                        x.First().Id,
                        x.First().DateFrom,
                        x.First().NumberGji,
                        x.First().Municipality,
                        x.First().Correspondent,
                        x.First().IsFile,
                        x.First().KindStatement,
                        x.First().Description,
                        x.First().PreviousAppealCits,
                        x.First().ManagingOrganization,
                        x.First().Surety,
                        x.First().Address,
                        x.First().SuretyResolve,
                        SuretyDate = x.First().SuretyDate.ToDateTime(),
                        CheckTime = x.First().CheckTime.ToDateTime(),
                        x.First().Executant,
                        x.First().Tester,
                    })
                .ToList();

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
                        Tester = x.First().Tester
                    });

            addressDict = appealCitsWithRo
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => y.Address).Aggregate((current, next) => string.Format("{0}, {1}", current, next)));

            var appealCitsIds = appealCitsQuery.Select(x => x.AppealCits.Id).Distinct().ToArray();

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

                FillAppealCitsSection(section, num, appealCit.Value, addressDict[appealCit.Key], sources, statSubjectAndFeature, answerAppeal);
                num++;

                if (baseStatements == null || baseStatements.Count == 0)
                {
                    continue;
                }

                var answers = answersDict.ContainsKey(appealCit.Value.Id)
                    ? answersDict[appealCit.Value.Id]
                    : null;

                section["signControl"] = baseStatements.First().SignControl;
                section["expired"] = baseStatements.First().SignControl == "На контроле" ? "1" : string.Empty;

                if (answers != null)
                {
                    section["countAnswer"] = answers.Count;
                }

                var documents = documentsDict.Where(x => baseStatements.Any(y => y.BaseStatementId == x.Key));

                section["disposals"] = documents.AggregateWithSeparator(x => x.Value.Disposal, ";");
                section["actsChecks"] = documents.AggregateWithSeparator(x => x.Value.ActCheck, ";");
                section["prescriptions"] = documents.AggregateWithSeparator(x => x.Value.Prescription, ";");
                section["protocols"] = documents.AggregateWithSeparator(x => x.Value.Protocol, ";");
                section["resolution"] = documents.AggregateWithSeparator(x => x.Value.Resolution, ";");

                section["definitions"] =
                    definitionsDict.Where(x => baseStatements.Any(y => y.BaseStatementId == x.Key)).AggregateWithSeparator(x => x.Value, ";");

            }
        }
    }
}
