namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class CategoryCSMKDViewModel : BaseViewModel<CategoryCSMKD>
    {
        public override IDataResult List(IDomainService<CategoryCSMKD> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    TypeCategoryCS = x.TypeCategoryCS.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        } 
    }
}