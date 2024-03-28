namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BuilderDocumentViewModel : BaseViewModel<BuilderDocument>
    {
        public override IDataResult List(IDomainService<BuilderDocument> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var builderId = baseParams.Params.GetAs<long>("builderId");

            var data = domain.GetAll()
                .Where(x => x.Builder.Id == builderId)
                .Select(x => new
                {
                    x.Id,
                    x.BuilderDocumentType,
                    Period = x.Period.Name,
                    x.Description,
                    x.DocumentNum,
                    x.DocumentName,
                    x.DocumentDate,
                    x.File
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}