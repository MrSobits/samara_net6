using System.Collections.Generic;

namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using DomainService;
    using B4;
    using Entities;

    public class FieldRequirementViewModel : BaseViewModel<FieldRequirement>
    {
        public override IDataResult List(IDomainService<FieldRequirement> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var fieldReqservice = Container.Resolve<IFieldRequirementService>();
            if (fieldReqservice != null)
            {
                var list = domain.GetAll().ToList();
                var dict = new Dictionary<string, long>();

                foreach (var item in list)
                {
                    if (!dict.ContainsKey(item.RequirementId))
                    {
                        dict.Add(item.RequirementId, item.Id);
                    }
                }

                var data = fieldReqservice.GetAllRequirements()
                    .Where(x => !x.IsNamespace)
                    .Select(x => new 
                    {
                        ObjectName = fieldReqservice.GetFieldRequirementPath(x.RequirementId),
                        x.RequirementId,
                        RecId = dict.ContainsKey(x.RequirementId) ? dict[x.RequirementId] : 0,
                        FieldName = x.Description,
                        Required = dict.ContainsKey(x.RequirementId)
                    })
                    .AsQueryable()
                    .Filter(loadParams, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }

            return new ListDataResult(null, 0);
        }
    }
}