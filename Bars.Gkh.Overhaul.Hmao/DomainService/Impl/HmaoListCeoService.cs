namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Entities.RealEstateType;
    using Overhaul.DomainService;
    using Castle.Windsor;
    using Newtonsoft.Json.Linq;
    using Overhaul.Entities;

    public class HmaoListCeoService : IListCeoService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListTree(BaseParams baseParams)
        {
            var root = new JObject();
            root["children"] = new JArray();

            var elementDomain = Container.Resolve<IDomainService<StructuralElement>>();
            var objectElementService = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var realEstElementService = Container.Resolve<IDomainService<RealEstateTypeStructElement>>();
            var realityObj = baseParams.Params.GetAs<long>("realityObjectId");
            var realEstateTypeId = baseParams.Params.GetAs<long>("realEstateTypeId");
            var findValue = baseParams.Params.GetAs<string>("findValue");
            var required = baseParams.Params.GetAs<bool>("required");
            IQueryable<long> addedElements;
            if (realEstateTypeId == 0)
            {
                addedElements = objectElementService.GetAll()
                    .WhereIf(realityObj > 0, x => x.RealityObject.Id == realityObj)
                    .Select(x => x.StructuralElement.Id);
            }
            else
            {
                addedElements = realEstElementService.GetAll()
                    .Where(x => x.RealEstateType.Id == realEstateTypeId)
                    .Select(x => x.StructuralElement.Id);
            }

            var data =
                elementDomain.GetAll()
                    .Where(x => !addedElements.Contains(x.Id) || (realEstateTypeId == 0 && x.Group.CommonEstateObject.MultipleObject))
                    .Where(x => x.Group.CommonEstateObject.IncludedInSubjectProgramm)
                    .WhereIf(!string.IsNullOrEmpty(findValue), x => x.Name.ToLower().Contains(findValue.ToLower()) 
                            || x.Group.Name.ToLower().Contains(findValue.ToLower()) 
                            || x.Group.CommonEstateObject.Name.ToLower().Contains(findValue.ToLower()))
                    .WhereIf(required,x => x.Group.Required)
                    .Where(x => x.Group.UseInCalc)
                    .Select(x => new
                    {
                        id = x.Id,
                        ooi = x.Group.CommonEstateObject.Name,
                        group = x.Group.Name,
                        name = x.Name,
                        ooiId = x.Group.CommonEstateObject.Id,
                        groupId = x.Group.Id,
                        required = x.Group.Required,
                        UnitMeasure = x.UnitMeasure.Name,
                        multiple = x.Group.CommonEstateObject.MultipleObject
                    })
                    .OrderBy(x => x.ooi)
                    .ThenBy(x => x.group)
                    .ThenBy(x => x.name)
                    .ToList();

            // обязательные группы  множ констр. элементов которые уже добавлены (нужен для проверки заполнения обязательных конструктивных элементов)
            var addedMultipleStructElementGroupIds =
                objectElementService.GetAll()
                    .Where(x => x.RealityObject.Id == realityObj)
                    .Where(x => x.StructuralElement.Group.Required)
                    .Where(x => x.StructuralElement.Group.CommonEstateObject.MultipleObject)
                    .Select(x => x.StructuralElement.Group.Id)
                    .ToList();

            var commons = new Dictionary<long, ConstElNode>();
            var groups = new Dictionary<long, ConstElNode>();
            var i = -1;
            foreach (var item in data)
            {
                var el = new ConstElNode
                    {
                        Id = item.id,
                        Text = item.name,
                        Type = "elem",
                        Checked = false,
                        UnitMeasure = item.UnitMeasure,
                        Multiple = item.multiple,
                        ElemId = item.id
                    };

                ConstElNode ooi;
                if (commons.ContainsKey(item.ooiId))
                {
                    ooi = commons[item.ooiId];
                }
                else
                {
                    ooi = new ConstElNode
                    {
                        Id = i--,
                        Text = item.ooi,
                        Type = "common",
                        Children = new List<ConstElNode>()
                    };
                    commons.Add(item.ooiId, ooi);
                }

                ConstElNode group;
                if (groups.ContainsKey(item.groupId))
                {
                    group = groups[item.groupId];
                }
                else
                {
                    group = new ConstElNode
                        {
                            Id = i--,
                            Text = item.group,
                            Type = "group",
                            Children = new List<ConstElNode>(),
                            Required = item.required,
                            Added = addedMultipleStructElementGroupIds.Contains(item.groupId),
                            GroupId = item.groupId
                        };

                    groups.Add(item.groupId, group);
                }

                group.Children.Add(el);

                if (!ooi.Children.Contains(group))
                {
                    ooi.Children.Add(group);
                }
            }

            return new BaseDataResult(new { children = commons.Values });
        }
    }
}
