namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Enums;

    public class TypeWorkCrHistoryViewModel : BaseViewModel<TypeWorkCrHistory>
    {
        public IDomainService<TypeWorkCrRemoval> TypeRemovalDomain { get; set; }  

        public override IDataResult List(IDomainService<TypeWorkCrHistory> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAs<long?>("objectCrId") ?? loadParams.Filter.GetAs<long>("objectCrId");

            var dict = TypeRemovalDomain.GetAll()
                .Where(x => x.TypeWorkCr.ObjectCr.Id == objectCrId)
                .Select(x => new
                {
                    twId = x.TypeWorkCr.Id,
                    NameDoc = x.Description,
                    x.FileDoc,
                    x.DateDoc,
                    x.ObjectCreateDate,
                    x.TypeReason
                })
                .AsEnumerable()
                .GroupBy(x => x.twId)
                .ToDictionary(x => x.Key, y => new
                {
                    RemoveRec = y.Where(x => x.TypeReason != TypeWorkCrReason.NotSet).OrderByDescending(z => z.ObjectCreateDate).FirstOrDefault(),
                    AddRec = y.Where(x => x.TypeReason == TypeWorkCrReason.NotSet).OrderByDescending(z => z.ObjectCreateDate).FirstOrDefault()
                });
            
            var data = domainService.GetAll()
                .Where(x => x.TypeWorkCr.ObjectCr.Id == objectCrId)
                .Select(x => new
                {
                    x.Id,
                    x.TypeAction,
                    x.TypeReason,
                    WorkName = x.TypeWorkCr.Work.Name,
                    TypeWorkCr = x.TypeWorkCr.Id,
                    FinanceSourceName = x.FinanceSource.Name,
                    x.Volume,
                    x.Sum,
                    x.YearRepair,
                    x.NewYearRepair,
                    x.UserName,
                    x.ObjectCreateDate,
                    x.StructElement
                })
                .AsEnumerable()
                .Select(x =>
                {
                    var documentRec = x.TypeReason == TypeWorkCrReason.NotSet
                        ? dict.Get(x.TypeWorkCr).Return(y => y.AddRec)
                        : dict.Get(x.TypeWorkCr).Return(y => y.RemoveRec);

                    return new
                    {
                        x.Id,
                        x.TypeAction,
                        x.TypeReason,
                        x.WorkName,
                        x.TypeWorkCr,
                        x.FinanceSourceName,
                        x.Volume,
                        x.Sum,
                        x.YearRepair,
                        x.NewYearRepair,
                        x.UserName,
                        x.StructElement,
                        ObjectCreateDate = x.ObjectCreateDate.ToUniversalTime(),
                        FileDoc = documentRec.Return(z => z.FileDoc),
                        NameDoc = documentRec.Return(z => z.NameDoc),
                        DateDoc = documentRec.Return(z => z.DateDoc)
                    };
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.OrderByDescending(x => x.ObjectCreateDate).Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}