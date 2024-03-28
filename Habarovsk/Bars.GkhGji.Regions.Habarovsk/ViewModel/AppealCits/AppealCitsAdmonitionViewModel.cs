namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using B4;

    using Bars.B4.Utils;

    using Entities;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Utils;

    public class AppealCitsAdmonitionViewModel : BaseViewModel<AppealCitsAdmonition>
    {
        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }
        public IDomainService<AppCitAdmonVoilation> AppCitAdmonVoilationDomain { get; set; }
        public IDomainService<AppCitAdmonAppeal> AppCitAdmonAppealDomain { get; set; }
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        public override IDataResult List(IDomainService<AppealCitsAdmonition> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
            var isFiltered = baseParams.Params.GetAs<bool>("isFiltered");

            if (!isFiltered)
            {
                var data = AppCitAdmonAppealDomain.GetAll()
                 .Where(x => x.AppealCits.Id == appealCitizensId)
                 .Select(x => new
                 {
                     Id = x.AppealCitsAdmonition.Id,
                     DocumentName = x.AppealCitsAdmonition.DocumentName,
                     DocumentDate = x.AppealCitsAdmonition.DocumentDate,
                     PerfomanceDate = x.AppealCitsAdmonition.PerfomanceDate,
                     PerfomanceFactDate = x.AppealCitsAdmonition.PerfomanceFactDate,
                     Contragent = x.AppealCitsAdmonition.Contragent.Name,
                     File = x.AppealCitsAdmonition.File,
                     Inspector = x.AppealCitsAdmonition.Inspector.Fio,
                     AnswerFile = x.AppealCitsAdmonition.AnswerFile,
                     Executor = x.AppealCitsAdmonition.Executor.Fio,
                     DocumentNumber = x.AppealCitsAdmonition.DocumentNumber,
                     AppealNumber = x.AppealCits.NumberGji,
                     DateFrom = x.AppealCits.DateFrom,
                     FIO = x.AppealCitsAdmonition.FIO,
                     x.AppealCitsAdmonition.KindKND,
                     x.AppealCitsAdmonition.PayerType,
                     x.AppealCitsAdmonition.ERKNMID
                 })
                 .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
            }
            else
            {
                var appealCitsRealityObject = AppealCitsRealityObjectDomain.GetAll();

                var dateStart2 = loadParams.Filter.GetAs("dateStart", new DateTime());
                var dateEnd2 = loadParams.Filter.GetAs("dateEnd", new DateTime());

                var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
                var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
                Dictionary<long, string> dict = new Dictionary<long, string>();
                var admViolDomain = AppCitAdmonVoilationDomain.GetAll()
                     .Where(x => x.AppealCitsAdmonition.PerfomanceDate.HasValue
                            ? x.AppealCitsAdmonition.PerfomanceDate.Value >= dateStart && x.AppealCitsAdmonition.PerfomanceDate.Value <= dateEnd
                            : 1 == 1)
                            .Select(x => new
                            {
                                x.AppealCitsAdmonition.Id,
                                x.ViolationGji.Name
                            }).AsEnumerable();
                foreach (var zap in admViolDomain)
                {
                    if (!dict.ContainsKey(zap.Id))
                    {
                        dict.Add(zap.Id, zap.Name);
                    }
                    else
                    {
                        dict[zap.Id] += "; " + zap.Name;
                    }
                }

                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator?.Inspector == null)
                {
                    var contragent = thisOperator.Contragent;
                    var contragentList = OperatorContragentDomain.GetAll()
                     .Where(x => x.Contragent != null)
                     .Where(x => x.Operator == thisOperator)
                     .Select(x => x.Contragent.Id).Distinct().ToList();
                    if (contragent != null)
                    {
                        if (!contragentList.Contains(contragent.Id))
                        {
                            contragentList.Add(contragent.Id);
                        }
                    }

                    var data = domainService.GetAll()
                    .Where(x => contragentList.Contains(x.Contragent.Id))
                    .Join(
                        appealCitsRealityObject.AsEnumerable(),
                        x => x.AppealCits.Id,
                        y => y.AppealCits.Id,
                        (x, y) => new
                        {
                            x.Id,
                            x.DocumentName,
                            x.PerfomanceDate,
                            x.PerfomanceFactDate,
                            Contragent = x.Contragent != null ? x.Contragent.Name : "",
                            x.File,
                            x.PayerType,
                            x.SignedFile,
                            x.FIO,
                            x.Signature,
                            AppealNumber = GetAppealNumber(x.Id),
                            x.KindKND,
                            Inspector = x.Inspector.Fio,
                            Municipality = y.RealityObject.Municipality.Name,
                            Address = y.RealityObject.Address,
                            Violations = dict.ContainsKey(x.Id) ? dict[x.Id] : "",
                            x.AnswerFile,
                            x.SignedAnswerFile,
                            x.AnswerSignature,
                            x.ERKNMID,
                            Executor = x.Executor.Fio,
                            x.DocumentNumber,
                            x.DocumentDate
                        })
                    .Where(x => x.PerfomanceDate.HasValue
                            ? x.PerfomanceDate.Value >= dateStart && x.PerfomanceDate.Value <= dateEnd
                            : 1 == 1)
                    .Filter(loadParams, Container);

                    int totalCount = data.Count();

                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
                }
                else
                {
                    var data = domainService.GetAll()
                    .Join(
                        appealCitsRealityObject.AsEnumerable(),
                        x => x.AppealCits.Id,
                        y => y.AppealCits.Id,
                        (x, y) => new
                        {
                            x.Id,
                            x.DocumentName,
                            x.PerfomanceDate,
                            x.PerfomanceFactDate,
                            Contragent = x.Contragent != null ? x.Contragent.Name : "",
                            x.File,
                            x.SignedFile,
                            x.PayerType,
                            x.FIO,
                            x.Signature,
                            AppealNumber = GetAppealNumber(x.Id),
                            Inspector = x.Inspector.Fio,
                            Municipality = y.RealityObject.Municipality.Name,
                            Address = y.RealityObject.Address,
                            Violations = dict.ContainsKey(x.Id) ? dict[x.Id] : "",
                            x.AnswerFile,
                            x.SignedAnswerFile,
                            x.AnswerSignature,
                            x.KindKND,
                            Executor = x.Executor.Fio,
                            x.DocumentNumber,
                            x.DocumentDate
                        })
                    .Where(x => x.PerfomanceDate.HasValue
                            ? x.PerfomanceDate.Value >= dateStart && x.PerfomanceDate.Value <= dateEnd
                            : 1 == 1)
                    .Filter(loadParams, Container);

                    int totalCount = data.Count();

                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
                }

                    
            }
        }

        private string GetAppealNumber(long admonId)
        {
            return AppCitAdmonAppealDomain.GetAll().Where(x => x.AppealCitsAdmonition.Id == admonId).AsEnumerable()
                .AggregateWithSeparator(x => x.AppealCits.NumberGji, ";");
        }
    }
}