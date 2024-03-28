namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;

    using Newtonsoft.Json.Linq;
    using Bars.B4.Utils;
    using System.Linq;
    using Castle.Windsor;
    using Overhaul.Entities;

    class JobService : IJobService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListTree(BaseParams baseParams)
        {
            try
            {
                var workName = baseParams.Params["workName"].To<string>();
                if (!workName.IsEmpty())
                {
                    workName = workName.ToLower();
                }

                var data = this.Container.Resolve<IDomainService<Job>>().GetAll()
                    .WhereIf(!workName.IsEmpty(), x => x.Name.ToLower().Contains(workName))
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            Work = x.Work.Name,
                            UnitMeasure = x.UnitMeasure.Name
                        })
                        .ToList()
                        .GroupBy(x => x.Work)
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