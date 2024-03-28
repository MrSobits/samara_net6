namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    /// <summary>
    /// ViewModel для RealityObjectBlock
    /// </summary>
    public class RealityObjectBlockViewModel : BaseViewModel<RealityObjectBlock>
    {
        public override IDataResult List(IDomainService<RealityObjectBlock> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.Number,
                    x.AreaLiving,
                    x.AreaTotal,
                    x.CadastralNumber
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}