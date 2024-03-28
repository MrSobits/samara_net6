namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using B4;
    using Entities;
    using System.Linq;

    public class ERKNMDictFileViewModel : BaseViewModel<ERKNMDictFile>
    {
        public override IDataResult List(IDomainService<ERKNMDictFile> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("ERKNMDictId", 0L);

            var data = domain.GetAll()
                .Where(x => x.ERKNMDict.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.ERKNMDict,
                    x.FileInfo,
                    x.SMEVFileType
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}