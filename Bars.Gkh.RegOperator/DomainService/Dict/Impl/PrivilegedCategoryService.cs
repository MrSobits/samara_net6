namespace Bars.Gkh.RegOperator.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.Dict;

    using Castle.Windsor;

    public class PrivilegedCategoryService : IPrivilegedCategoryService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<PrivilegedCategory> PrivilegedCategoryDomain { get; set; }

        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var data = PrivilegedCategoryDomain.GetAll()
                .Select(x => new { x.Id, x.Name, x.DateFrom })
                .OrderByDescending(x => x.DateFrom)
                .ToList()
                .GroupBy(x => x.Name)
                .Select(x => x.First())
                .OrderBy(x => x.Name);

            var totalCount = data.Count();

            return new ListDataResult(data, totalCount);
        }
    }
}