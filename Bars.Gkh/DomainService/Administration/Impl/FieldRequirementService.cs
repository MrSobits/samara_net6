namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.IoC;

    using Entities;
    using Castle.Windsor;

    class FieldRequirementService : IFieldRequirementService
    {
        public IWindsorContainer Container { get; set; }

        public IEnumerable<FieldRequirementInfo> GetAllRequirements()
        {
            var reqSources = this.Container.ResolveAll<IFieldRequirementSource>();
            using (this.Container.Using(reqSources))
            {
                return reqSources.SelectMany(source => source.GetFieldRequirements().Values).ToList();
            }
        }

        public FieldRequirementInfo GetFieldRequirement(string requirementId)
        {
            return GetAllRequirements().FirstOrDefault(r => r.RequirementId == requirementId);
        }

        public string GetFieldRequirementPath(string requirementId)
        {
            string sPath = null;

            var dict = this.GetAllRequirements()
                .Where(x => x.IsNamespace)
                .GroupBy(x => x.RequirementId)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            int pos = 0;
            while (pos != -1)
            {
                pos = requirementId.IndexOf('.', pos + 1);
                if (pos != -1)
                {
                    var sub = requirementId.Substring(0, pos);
                    if (dict.ContainsKey(sub))
                    {
                        sPath += dict[sub].Description + ": ";
                    }
                }
            }

            return sPath;
        }
    }
}
