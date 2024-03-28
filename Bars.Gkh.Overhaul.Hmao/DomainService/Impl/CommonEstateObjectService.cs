namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Entities.RealEstateType;
    using Gkh.Report;
    using Overhaul.Entities;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class CommonEstateObjectService : Overhaul.DomainService.Impl.CommonEstateObjectService
    {
        public IDataResult ListTree(BaseParams baseParams)
        {
            var root = new JObject();
            root["children"] = new JArray();

            var elementDomain = Container.Resolve<IDomainService<StructuralElement>>();
            var objectElementService = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var realEstElementService = Container.Resolve<IDomainService<RealEstateTypeStructElement>>();
            var realityObj = baseParams.Params.GetAs<long>("realityObjectId");
            var realEstateTypeId = baseParams.Params.GetAs<long>("realEstateTypeId");
            var onlyreqs = baseParams.Params.GetAs<bool>("onlyreqs");
            var detailed = baseParams.Params.GetAs<bool>("detailed");
            var findValue = baseParams.Params.GetAs<string>("findValue");
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
                    .WhereIf(!string.IsNullOrEmpty(findValue),
                        x => x.Name.ToLower().Contains(findValue.ToLower()) ||
                            x.Group.Name.ToLower().Contains(findValue.ToLower()) ||
                            x.Group.CommonEstateObject.Name.ToLower().Contains(findValue.ToLower()))
                    .WhereIf(onlyreqs, x => x.Group.Required)
                    .WhereIf(!detailed, x => x.Group.UseInCalc)
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

        public override Stream PrintReport(BaseParams baseParams)
        {
            var stream = new MemoryStream();

            var report = Container.Resolve<IGkhBaseReport>("StructElementListHmao");

            var reportProvider = Container.Resolve<IGkhReportProvider>();

            //собираем сборку отчета и формируем reportParams
            var reportParams = new ReportParams();
            report.PrepareReport(reportParams);

            //получаем Генератор отчета
            var generatorName = report.GetReportGenerator();

            var generator = Container.Resolve<IReportGenerator>(generatorName);

            reportProvider.GenerateReport(report, stream, generator, reportParams);

            return stream;
        }

        public override IDataResult ListForRealObj(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var realObjId = baseParams.Params.GetAs<long>("realObjId");
            var realObjStructElemDomain = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var roMissingCommonEstObjDomain = Container.Resolve<IDomainService<RealityObjectMissingCeo>>();

            var data = Container.Resolve<IDomainService<CommonEstateObject>>().GetAll()
                .Where(x => x.IncludedInSubjectProgramm)
                .Where(x => !realObjStructElemDomain.GetAll()
                            .Any(y => y.RealityObject.Id == realObjId 
                                && y.StructuralElement.Group.CommonEstateObject.Id == x.Id) 
                        && !roMissingCommonEstObjDomain.GetAll()
                            .Any(y => y.RealityObject.Id == realObjId 
                                && y.MissingCommonEstateObject.Id == x.Id))
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public override IDataResult AddWorks(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var serviceJob = Container.Resolve<IDomainService<Job>>();
                var serviceStructuralElementWork = Container.Resolve<IDomainService<StructuralElementWork>>();
                var serviceStructuralElement = Container.Resolve<IDomainService<StructuralElement>>();

                try
                {
                    var elementId = baseParams.Params.GetAs<long>("elementId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

                    // получаем у контроллера услуги что бы не добавлять их повторно
                    var existRecs =
                        serviceStructuralElementWork.GetAll()
                            .Where(x => x.StructuralElement.Id == elementId)
                            .Where(x => objectIds.Contains(x.Job.Id))
                            .Select(x => x.Job.Id)
                            .ToList();

                    var elemObj = serviceStructuralElement.Load(elementId);

                    foreach (var id in objectIds)
                    {
                        if (existRecs.Contains(id))
                        {
                            continue;
                        }

                        var job = serviceJob.FirstOrDefault(x => x.Id == id);

                        if (job != null)
                        {
                            var newStructuralElementWork =
                                new StructuralElementWork
                                {
                                    Job = job,
                                    StructuralElement = elemObj
                                };

                            serviceStructuralElementWork.Save(newStructuralElementWork);
                        }
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(serviceJob);
                    Container.Release(serviceStructuralElementWork);
                    Container.Release(serviceStructuralElement);
                }
            }
        }
    }

    public class ConstElNode
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("leaf")]
        public bool Leaf { get { return Children == null || Children.Count == 0; } }

        [JsonProperty("expanded")]
        public bool Expanded { get { return false; } }

        public string UnitMeasure { get; set; }

        public long ElemId { get; set; }

        [JsonProperty("children")]
        public List<ConstElNode> Children { get; set; }

        [JsonProperty("checked", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Checked { get; set; }

        [JsonProperty("multiple")]
        public bool Multiple { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        [JsonProperty("added")]
        public bool Added { get; set; }

        [JsonProperty("groupid", NullValueHandling = NullValueHandling.Ignore)]
        public long? GroupId { get; set; }
    }
}
