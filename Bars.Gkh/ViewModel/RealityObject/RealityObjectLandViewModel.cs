namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectLandViewModel : BaseViewModel<RealityObjectLand>
    {
        public override IDataResult List(IDomainService<RealityObjectLand> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.CadastrNumber,
                    x.DateLastRegistration,
                    x.DocumentName,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.File
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

    }
}