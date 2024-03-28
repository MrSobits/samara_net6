using System.Collections.Generic;
using Bars.B4.DataAccess;

namespace Bars.GkhGji.Controllers.Dict
{
    using System.Linq;
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;

    using Castle.MicroKernel.Registration;

    using Microsoft.AspNetCore.Mvc;

    using NHibernate.Mapping.ByCode.Impl;

    public class FeatureViolGjiController : B4.Alt.DataController<FeatureViolGji>
    {
        public ActionResult GetTree(BaseParams baseParams)
        {
            var result = CollectNode(null, DomainService.GetAll().ToList(), new List<ViolationFeatureGji>(), true);

            return new JsonNetResult(result);
        }

        private List<Node> CollectNode(FeatureViolGji parent, List<FeatureViolGji> groups, List<ViolationFeatureGji> violations, bool showCodes, bool onlyWithViol = false)
        {
            var nodes = new List<Node>();
            var query = groups.Where(x => x.Parent.Return(p => p.Id) == parent.Return(p => p.Id));
            foreach (var group in query)
            {
                var node = new Node(group, showCodes);
                nodes.Add(node);
                node.children.AddRange(CollectNode(@group, groups, violations, showCodes, onlyWithViol));
            }

            if (parent != null)
            {
                foreach (var viol in violations.Where(x => x.FeatureViolGji != null && x.FeatureViolGji.Id == parent.Id))
                {
                    nodes.Add(new Node(viol.Id, viol.ViolationGji.Name, viol.ViolationGji.NormativeDocNames, true, viol.ViolationGji.Id));
                }
            }

            var result = nodes;

            if (onlyWithViol)
            {
                result = result.Where(x => x.HasViolations).ToList();
            }

            return result;
        }

        public ActionResult GetTreeForViolations(BaseParams baseParams)
        {
            var filter = baseParams.Params.GetAs<string>("filter").ToStr().ToLowerInvariant();
            var groups = DomainService.GetAll().ToList();
            var viols = Container.ResolveDomain<ViolationFeatureGji>().GetAll()
                .Where(x => x.FeatureViolGji != null)
                .WhereIf(!string.IsNullOrEmpty(filter),  
                    x => x.ViolationGji.Name.ToLowerInvariant().Contains(filter) ||
                    x.ViolationGji.NormativeDocNames.ToLowerInvariant().Contains(filter))
                .ToList();

            var result = CollectNode(null, groups, viols, false, true);

            return new JsonNetResult(result);
        }

        private class Node
        {

            public Node(FeatureViolGji fv, List<Node> childs, bool showCodes)
                : this(fv, showCodes)
            {
                children = childs;
            }

            public Node(FeatureViolGji fv, bool showCodes)
                : this(fv.Id, fv.Name, showCodes ? fv.Code : "") //для группы и подгрупп код не отображать
            {
                if (fv.Parent != null)
                {
                    Parent = fv.Parent.Id;
                }
            }

            public Node(long id, string name, string code, bool isViolation = false, long violId = 0)
            {
                Id = id;
                Name = name;
                Code = code;
                ViolationGjiId = violId;
                children = new List<Node>();
                IsViolation = isViolation;
            }

            public bool IsViolation { get; set; }

            public long Parent { get; set; }

            public long Id { get; set; }

            public string Name { get; set; }

            public string Code { get; set; }

            public long ViolationGjiId { get; set; }

            public bool HasViolations
            {
                get { return IsViolation || children.Any(x => x.HasViolations); }
            }

            public List<Node> children { get; set; }

            public bool leaf {
                get { return !children.Any(); }
            }
        }
    }
}
