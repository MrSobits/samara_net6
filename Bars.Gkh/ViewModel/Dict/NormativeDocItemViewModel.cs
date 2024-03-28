namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Gkh.Entities.Dicts;

    public class NormativeDocItemViewModel : BaseViewModel<NormativeDocItem>
    {
        public override IDataResult Get(IDomainService<NormativeDocItem> domainService, BaseParams baseParams)
        {
            var result = base.Get(domainService, baseParams);
            var item = result.Data as NormativeDocItem;
            if (item == null)
            {
                return new BaseDataResult(null);
            }

            return new BaseDataResult(new
            {
                item.Id,
                item.NormativeDoc,
                item.Number,
                item.Text,
                NormativeDocName = item.NormativeDoc.Name,
                NormativeDocId = item.NormativeDoc.Id
            });
        }

        public override IDataResult List(IDomainService<NormativeDocItem> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            object value;
            var docId = baseParams.Params.TryGetValue("docId", out value)
                                  ? value.ToLong()
                                  : 0;

            var data = domainService.GetAll();
            if (docId != 0)
            {
                data = data.Where(x => x.NormativeDoc.Id == docId);
            }

            var result = data.Select(item => new
            {
                item.Id,
                item.NormativeDoc,
                item.Number,
                item.Text,
                NormativeDocName = item.NormativeDoc.Name,
                NormativeDocId = item.NormativeDoc.Id
            }).Filter(loadParam, Container);

            int totalCount = result.Count();

            var loadAll = baseParams.Params.TryGetValue("loadAll", out value) && value.ToBool();
            var returnData = loadAll ? result.Order(loadParam).ToList() : result.Order(loadParam).Paging(loadParam).ToList();

            return new ListDataResult(returnData, totalCount);
        }
    }
}