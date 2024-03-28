namespace Bars.Gkh.Overhaul.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;
    using Newtonsoft.Json.Linq;
    using NHibernate.Linq;

    public class PaysizeRecordService : IPaysizeRecordService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListTree(BaseParams baseParams)
        {
            var paysizeId = baseParams.Params.GetAsId("paysizeId");
            var domain = Container.ResolveDomain<PaysizeRecord>();

            var data = domain.GetAll()
                .Fetch(x => x.Municipality)
                .ThenFetch(x => x.ParentMo)
                .Where(x => x.Paysize.Id == paysizeId)
                .OrderBy(x => x.Municipality.Name)
                .ToList()
                .GroupBy(x => x.Municipality.Name)
                .Select(x => x.First())
                .ToList();

            var child = data
                .Where(x => x.Municipality.ParentMo != null)
                .ToList();

            var parent = data
                .Where(x => x.Municipality.ParentMo == null)
                .ToList();

            var tree = ConvertDictToTree(parent, child);

            return new BaseDataResult(tree["children"]);
        }

        /// <summary>
        /// конвертация словаря в дерево
        /// </summary>
        private JObject ConvertDictToTree(IEnumerable<PaysizeRecord> parents, List<PaysizeRecord> child)
        {
            var root = new JObject();

            var groups = new JArray();

            foreach (var pair in parents)
            {
                var group = new JObject();

                if (pair.Municipality.Name != null)
                {
                    var id = pair.Municipality.Id;

                    group["id"] = pair.Id;
                    group["Id"] = pair.Id;
                    group["Municipality"] = pair.Municipality.Id;
                    group["Paysize"] = pair.Paysize.Id;
                    group["Name"] = pair.Municipality.Name;
                    group["Value"] = pair.Value;
                    group["text"] = pair.Municipality.Name;
                    group["expanded"] = true;

                    var children = new JArray();

                    var hisChild = child
                        .Where(x => x.Municipality.ParentMo.Id == id)
                        .OrderBy(x => x.Municipality.Name);

                    foreach (var rec in hisChild)
                    {
                        var leaf = new JObject();

                        leaf["id"] = rec.Id;
                        leaf["Id"] = rec.Id;
                        leaf["Municipality"] = rec.Municipality.Id;
                        leaf["Paysize"] = rec.Paysize.Id;
                        leaf["Name"] = rec.Municipality.Name;
                        leaf["Value"] = rec.Value;
                        leaf["text"] = rec.Municipality.Name;
                        leaf["leaf"] = true;

                        children.Add(leaf);
                    }

                    if (children.Any())
                    {
                        group["children"] = children;
                    }
                    else
                    {
                        group["leaf"] = true;
                    }

                    groups.Add(group);
                }
            }

            root["children"] = groups;

            return root;
        }
    }
}