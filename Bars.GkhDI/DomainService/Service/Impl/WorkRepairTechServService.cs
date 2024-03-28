using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhDi.Entities;
using Castle.Windsor;

namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;

    using Bars.GkhDi.Enums;

    using Newtonsoft.Json.Linq;

    public class WorkRepairTechServService : IWorkRepairTechServService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddWorks(BaseParams baseParams)
        {
            try
            {
                var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");
                var objectIds = baseParams.Params["objectIds"].ToStr().Split(',');

                var service = this.Container.Resolve<IDomainService<WorkRepairTechServ>>();

                // получаем у контроллера работы что бы не добавлять их повторно
                var exsistingWorkRepairTechServ = service
                    .GetAll()
                    .Where(x => x.BaseService.Id == baseServiceId)
                    .Select(x => x.WorkTo.Id)
                    .ToList();

                foreach (var id in objectIds)
                {
                    if (exsistingWorkRepairTechServ.Contains(id.ToLong()))
                    {
                        continue;
                    }
                    var newId = id.ToLong();

                    var newWorkRepairTechServ = new WorkRepairTechServ
                    {
                        WorkTo = new WorkTo { Id = newId },
                        BaseService = new BaseService { Id = baseServiceId }
                    };

                    service.Save(newWorkRepairTechServ);
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        /// <summary>
        /// метод для получения справочника Работ по ТО в виде дерева
        /// </summary>
        /// <param name="storeParams"></param>
        /// <returns></returns>
        public IDataResult ListTree(BaseParams baseParams)
        {
            try
            {
                var data = this.Container.Resolve<IDomainService<WorkTo>>().GetAll()
                        .Where(x => !x.IsNotActual)
                        .Select(x => new
                            {
                                x.Id,
                                x.Name,
                                Group = x.GroupWorkTo.Name
                            })
                        .ToList()
                        .GroupBy(x => x.Group)
                        .ToDictionary(x => x.Key, y => y.Select(z => new WorkTreeNode { Id = z.Id, Name = z.Name }));

                var tree = ConvertDictToTree(data);

                return new BaseDataResult(tree["children"])
                           {
                               Success = true
                           };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        public IDataResult ListSelected(BaseParams baseParams)
        {
            var realityObjId = baseParams.Params.GetAs<long>("realityObjId");
            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var disclosureInfo = this.Container.Resolve<IDomainService<DisclosureInfo>>().Load(disclosureInfoId);

            var serviceTemplateService = this.Container.Resolve<IDomainService<TemplateService>>();
            var templateServiceList = serviceTemplateService.GetAll().Where(x => x.KindServiceDi == KindServiceDi.Repair).ToList();

            // Получаем все услуги с типом КР у данного дома
            var repairServiceList = this.Container.Resolve<IDomainService<RepairService>>()
                .GetAll()
                .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == realityObjId
                            && x.DisclosureInfoRealityObj.PeriodDi.Id == disclosureInfo.PeriodDi.Id)
                .ToList();

            var data = new List<object>();
            foreach (var templateService in templateServiceList)
            {
                var count = 0;
                foreach (var repairService in repairServiceList)
                {
                    if (templateService.Id == repairService.TemplateService.Id)
                    {
                        data.Add(new
                        {
                            templateService.Id,
                            templateService.Name,
                            RepairServiceId = repairService.Id,
                            ProviderName = repairService.Provider != null ? repairService.Provider.Name : string.Empty,
                            repairService.SumWorkTo,
                            TypeColor = 0
                        });
                        count++;
                    }
                }

                if (count == 0)
                {
                    data.Add(new
                    {
                        templateService.Id,
                        templateService.Name,
                        TypeColor = 2
                    });
                }
            }

            return new ListDataResult(data.ToList(), data.Count);
        }

        public IDataResult SaveRepairService(BaseParams baseParams)
        {
            try
            {
                var realityObjId = baseParams.Params["realityObjId"].ToLong();
                var disclosureInfoId = baseParams.Params["disclosureInfoId"].ToLong();

                var disclosureInfo = this.Container.Resolve<IDomainService<DisclosureInfo>>().Get(disclosureInfoId);
                
                var service = this.Container.Resolve<IDomainService<RepairService>>();
                var repairServiceList = service.GetAll()
                    .WhereIf(realityObjId > 0 && disclosureInfo != null, 
                    x => x.DisclosureInfoRealityObj.RealityObject.Id == realityObjId && x.DisclosureInfoRealityObj.PeriodDi.Id == disclosureInfo.PeriodDi.Id)
                    .ToList();

                var records = baseParams.Params["records"]
                    .As<List<object>>()
                    .Select(x => x.As<DynamicDictionary>().ReadClass<RepairServiceProxy>())
                    .ToList();


                foreach (var rec in records.Where(x => x.RepairServiceId > 0))
                {
                    var repairService = repairServiceList.FirstOrDefault(x => x.Id == rec.RepairServiceId);

                    if (repairService == null)
                    {
                        continue;
                    }

                    repairService.SumWorkTo = rec.SumWorkTo;

                    if (repairService.Id > 0)
                    {
                        service.Update(repairService);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }


        private class RepairServiceProxy
        {
            public long RepairServiceId { get; set; }

            public decimal? SumWorkTo { get; set; }
        }

        /// <summary>
        /// конвертация словаря в дерево
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private JObject ConvertDictToTree(Dictionary<string, IEnumerable<WorkTreeNode>> dict)
        {
            var root = new JObject();

            var groups = new JArray();

            foreach (var pair in dict)
            {
                var group = new JObject();

                group["id"] = pair.Key;
                group["text"] = pair.Key;

                var children = new JArray();

                foreach (var rec in pair.Value)
                {
                    var leaf = new JObject();

                    leaf["id"] = rec.Id;
                    leaf["text"] = rec.Name;
                    leaf["leaf"] = true;
                    leaf["checked"] = false;

                    children.Add(leaf);
                }

                group["children"] = children;

                groups.Add(group);
            }

            root["children"] = groups;

            return root;
        }

        /// <summary>
        /// Вспомогательный класс
        /// </summary>
        private class WorkTreeNode
        {
            public long Id;
            public string Name;
        }
    }
}
