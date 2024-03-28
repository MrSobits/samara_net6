namespace Bars.Gkh.Overhaul.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Entities;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Сервис ООИ
    /// </summary>
    public class StructuralElementService : IStructuralElementService
    {
        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult ListTree(BaseParams baseParams)
        {
            var objectId = baseParams.Params.GetAs<long>("object");
            var realityObjId = baseParams.Params.GetAs<long>("realityObjectId");

            var objectElementService = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var dontAllowedElementIds =
                objectElementService.GetAll()
                    .Where(x => x.RealityObject.Id == realityObjId && !x.StructuralElement.Group.CommonEstateObject.MultipleObject)
                    .Select(x => x.StructuralElement.Id)
                    .ToList();

            var service = this.Container.Resolve<IDomainService<StructuralElement>>();

            var dict = service.GetAll()
                .WhereIf(objectId > 0, x => x.Group.CommonEstateObject.Id == objectId)
                .Where(x => !dontAllowedElementIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    GroupId = x.Group.Id,
                    GroupName = x.Group.Name,
                    Object = x.Group.CommonEstateObject
                })
                .ToList()
                .GroupBy(x => x.GroupId);

            var a = dict.ToDictionary(x => x.Key);

            var root = new JObject();
            var groups = new JArray();

            foreach (var pair in a)
            {
                var group = new JObject();

                group["id"] = "group" + pair.Value.Select(x => x.Object.Id).FirstOrDefault() + pair.Key;
                group["text"] = pair.Value.Select(x => x.GroupName).FirstOrDefault();

                var elems = new JArray();

                foreach (var item in pair.Value)
                {
                    var elem = new JObject();

                    elem["id"] = item.Id;
                    elem["text"] = item.Name;
                    elem["leaf"] = true;
                    elem["checked"] = false;

                    elems.Add(elem);
                }

                group["children"] = elems;

                groups.Add(group);
            }

            root["children"] = groups;

            this.Container.Release(service);
            this.Container.Release(objectElementService);

            return new BaseDataResult(root);
        }

        /// <inheritdoc />
        public IDataResult GetAttributes(BaseParams baseParams)
        {
            var elementId = baseParams.Params.GetAs<long>("element");
            var roelementId = baseParams.Params.GetAs<long>("roelement");

            if (roelementId > 0)
            {
                var elementservice = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
                var valuesService = this.Container.Resolve<IDomainService<RealityObjectStructuralElementAttributeValue>>();
                var element = elementservice.Get(roelementId);

                var values = valuesService.GetAll().Where(x => x.Object.Id == roelementId).ToList();

                var attributesService = this.Container.Resolve<IDomainService<StructuralElementGroupAttribute>>();

                var attributes = attributesService.GetAll()
                    .Where(x => x.Group.Id == element.StructuralElement.Group.Id)
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        x.AttributeType,
                        x.Name,
                        x.IsNeeded,
                        Value =
                            values.FirstOrDefault(y => y.Attribute.Id == x.Id) != null
                                ? values.FirstOrDefault(y => y.Attribute.Id == x.Id).Value
                                : string.Empty,
                        ValueId =
                            values.FirstOrDefault(y => y.Attribute.Id == x.Id) != null
                                ? values.FirstOrDefault(y => y.Attribute.Id == x.Id).Id
                                : 0,
                        x.Hint
                    })
                    .ToList();
                return new BaseDataResult(new
                {
                    Group = element.StructuralElement.Group.CommonEstateObject,
                    Attributes = new ListDataResult(attributes, attributes.Count)
                });
            }

            if (elementId > 0)
            {
                var elementservice = this.Container.Resolve<IDomainService<StructuralElement>>();
                var element = elementservice.Get(elementId);
                var attributesService = this.Container.Resolve<IDomainService<StructuralElementGroupAttribute>>();

                var attributes = attributesService.GetAll().Where(x => x.Group.Id == element.Group.Id)
                    .Select(x => new
                    {
                        x.Id,
                        x.AttributeType,
                        x.Name,
                        x.IsNeeded,
                        Value = string.Empty,
                        ValueId = 0
                    })
                    .ToList();

                return new BaseDataResult(new
                {
                    Group = element.Group.CommonEstateObject,
                    Attributes = new ListDataResult(attributes, attributes.Count)
                });
            }

           return BaseDataResult.Error("Не найден ООИ");
        }

        /// <inheritdoc />
        public IDataResult IsStructElForRequiredGroupsAdded(BaseParams baseParams)
        {
            var robjId = baseParams.Params.GetAs<long>("robjId");
            var serviceRobjStructEl = this.Container.ResolveDomain<RealityObjectStructuralElement>();
            var serviceGroups = this.Container.ResolveDomain<StructuralElementGroup>();

            var robjStrElsGroupIds = serviceRobjStructEl.GetAll()
                .Where(x => x.RealityObject.Id == robjId)
                .Select(x => x.StructuralElement.Group.Id)
                .Distinct()
                .ToList();

            var result =
                serviceGroups.GetAll()
                    .Where(x => x.Required && x.CommonEstateObject.IncludedInSubjectProgramm)
                    .Select(x => x.Id)
                    .ToList()
                    .All(robjStrElsGroupIds.Contains);

            return new BaseDataResult(new { IsAdded = result, AddedStructElRequiredGroups = robjStrElsGroupIds });
        }
    }
}