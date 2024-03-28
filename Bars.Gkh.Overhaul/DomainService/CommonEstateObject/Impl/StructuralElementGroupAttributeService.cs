namespace Bars.Gkh.Overhaul.DomainService.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Formulas;
    using Castle.Windsor;

    public class StructuralElementGroupAttributeService : IStructuralElementGroupAttributeService
    {
        public IWindsorContainer Container { get; set; }
        
        public IDataResult ListWithResolvers(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var group = loadParam.Filter.GetAs<long>("group");

            var list =
                Container.Resolve<IDomainService<StructuralElementGroupAttribute>>().GetAll()
                    .Where(x => x.Group.Id == group)
                    .Select(x => new
                    {
                        Id = x.Id.ToString(), 
                        x.Name, 
                        ValueResolverCode = string.Empty, 
                        ValueResolverName = string.Empty
                    })
                    .Order(loadParam)
                    .ToList();

            var resolvers =
                Container.ResolveAll<IFormulaParameter>()
                    .Select(x => new
                    {
                        Id = x.Code,
                        Name = x.DisplayName,
                        ValueResolverCode = x.Code,
                        ValueResolverName = x.DisplayName
                    })
                    .ToList();

            resolvers.AddRange(list);

            return new ListDataResult(resolvers, resolvers.Count);
        }
    }
}