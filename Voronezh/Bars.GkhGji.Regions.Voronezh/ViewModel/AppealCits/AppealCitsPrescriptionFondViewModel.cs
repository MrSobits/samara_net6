namespace Bars.GkhGji.Regions.Voronezh.ViewModel
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

    public class AppealCitsPrescriptionFondViewModel : BaseViewModel<AppealCitsPrescriptionFond>
    {
        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }
        public IDomainService<AppCitPrFondVoilation> AppCitPrFondVoilationDomain { get; set; }
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        public override IDataResult List(IDomainService<AppealCitsPrescriptionFond> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
            var isFiltered = baseParams.Params.GetAs<bool>("isFiltered");

            if (!isFiltered)
            {
                var data = domainService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentName,
                    x.PerfomanceDate,
                    x.PerfomanceFactDate,
                    Contragent = x.Contragent.Name,
                    x.File,
                    x.AppealCits.Number,
                    x.SignedFile,
                    x.KindKNDGJI,
                    x.Signature,
                    Inspector = x.Inspector.Fio,
                    x.AnswerFile,
                    x.SignedAnswerFile,
                    x.AnswerSignature,
                    Executor = x.Executor.Fio,
                    x.DocumentNumber,
                    x.DocumentDate,
                    MassBuildContract = x.MassBuildContract.DocumentNum
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
                var admViolDomain = AppCitPrFondVoilationDomain.GetAll()
                     .Where(x => x.AppealCitsPrescriptionFond.PerfomanceDate.HasValue
                            ? x.AppealCitsPrescriptionFond.PerfomanceDate.Value >= dateStart && x.AppealCitsPrescriptionFond.PerfomanceDate.Value <= dateEnd
                            : 1 == 1)
                            .Select(x => new
                            {
                                x.AppealCitsPrescriptionFond.Id,
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
                            Contragent = x.Contragent.Name,
                            x.File,
                            x.SignedFile,
                            x.AppealCits.Number,
                            x.Signature,
                            x.KindKNDGJI,
                            Inspector = x.Inspector.Fio,
                            Municipality = y.RealityObject.Municipality.Name,
                            Address = y.RealityObject.Address,
                            Violations = dict.ContainsKey(x.Id) ? dict[x.Id] : "",
                            x.AnswerFile,
                            x.SignedAnswerFile,
                            x.AnswerSignature,
                            Executor = x.Executor.Fio,
                            x.DocumentNumber,
                            x.DocumentDate,
                            MassBuildContract = x.MassBuildContract.DocumentNum
                        })
                    .Where(x => x.PerfomanceDate.HasValue
                            ? x.PerfomanceDate.Value >= dateStart && x.PerfomanceDate.Value <= dateEnd
                            : 1 == 1)
                    .Filter(loadParams, Container);

                    int totalCount = data.Count();

                    return new ListDataResult(data.ToArray(), totalCount);
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
                            Contragent = x.Contragent.Name,
                            x.File,
                            x.SignedFile,
                            x.Signature,
                            x.AppealCits.Number,
                            Inspector = x.Inspector.Fio,
                            Municipality = y.RealityObject.Municipality.Name,
                            Address = y.RealityObject.Address,
                            Violations = dict.ContainsKey(x.Id) ? dict[x.Id] : "",
                            x.AnswerFile,
                            x.SignedAnswerFile,
                            x.AnswerSignature,
                            x.KindKNDGJI,
                            Executor = x.Executor.Fio,
                            x.DocumentNumber,
                            x.DocumentDate,
                            MassBuildContract = x.MassBuildContract.DocumentNum
                        })
                    .Where(x => x.PerfomanceDate.HasValue
                            ? x.PerfomanceDate.Value >= dateStart && x.PerfomanceDate.Value <= dateEnd
                            : 1 == 1)
                    .Filter(loadParams, Container);

                    int totalCount = data.Count();

                    return new ListDataResult(data.ToArray(), totalCount);
                }

                    
            }
        }
    }
}