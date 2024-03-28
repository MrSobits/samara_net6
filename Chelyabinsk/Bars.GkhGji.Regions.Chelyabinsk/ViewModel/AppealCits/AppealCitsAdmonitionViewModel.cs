namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using System.Linq;
    using System;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Entities;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Utils;

    public class AppealCitsAdmonitionViewModel : BaseViewModel<AppealCitsAdmonition>
    {
        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }
        public IDomainService<AppCitAdmonAppeal> AppCitAdmonAppealDomain { get; set; }
        public override IDataResult List(IDomainService<AppealCitsAdmonition> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);          
            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
            var isFiltered = baseParams.Params.GetAs<bool>("isFiltered");
            if (!isFiltered && appealCitizensId>0)
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
                var dateStart2 = loadParams.Filter.GetAs("dateStart", new DateTime());
                var dateEnd2 = loadParams.Filter.GetAs("dateEnd", new DateTime());

                var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
                var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");

                var data = domainService.GetAll()
                      .Where(x => x.PerfomanceDate.HasValue
                            ? x.PerfomanceDate.Value >= dateStart && x.PerfomanceDate.Value <= dateEnd
                            : 1 == 1)
                  .Select(x => new
                        {
                            x.Id,
                            x.DocumentName,
                            x.PayerType,
                            x.DocumentDate,
                            x.PerfomanceDate,
                            x.PerfomanceFactDate,
                            Contragent = x.Contragent!= null? x.Contragent.Name:"",
                            x.File,
                            x.SignedFile,
                            x.Signature,
                            Inspector = x.Inspector.Fio,
                            Municipality = x.RealityObject != null? x.RealityObject.Municipality.Name:"",
                            Address = x.RealityObject != null? x.RealityObject.Address:"",
                            x.AnswerFile,
                            Executor = x.Executor.Fio,
                            FIO = x.FIO,
                            x.DocumentNumber,
                            AppealNumber = GetAppealNumber(x.Id),
                            x.AppealCits.DateFrom,
                            x.ERKNMID,
                            x.KindKND
                        })
                  
                    .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }

        private string GetAppealNumber(long admonId)
        {
            return AppCitAdmonAppealDomain.GetAll().Where(x => x.AppealCitsAdmonition.Id == admonId).AsEnumerable()
                .AggregateWithSeparator(x => x.AppealCits.NumberGji + " от " + x.AppealCits.DateFrom.Value.ToString("dd.MM.yyyy"), ";");
        }
    }
}