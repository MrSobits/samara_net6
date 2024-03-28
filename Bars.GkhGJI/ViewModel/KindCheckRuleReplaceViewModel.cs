namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using Entities;
    using Enums;
    using Rules;

#warning Проверить оптимальность кода
    public class KindCheckRuleReplaceViewModel : BaseViewModel<KindCheckRuleReplace>
    {
        public override IDataResult List(IDomainService<KindCheckRuleReplace> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var rules = Container.ResolveAll<IKindCheckRule>().Select(x => new {x.Name, x.DefaultCode, x.Code});

            var result = new List<Proxy>();

            var dictData = domainService.GetAll()
                .Select(x => new {x.Id, x.Code, x.RuleCode})
                .ToDictionary(x => x.RuleCode, y => new {y.Id, y.Code});

            foreach (var rule in rules)
            {
                if (dictData.ContainsKey(rule.Code))
                {
                    result.Add(new Proxy
                        {
                            Id = dictData[rule.Code].Id,
                            Name = rule.Name,
                            Code = dictData[rule.Code].Code,
                            RuleCode = rule.Code
                        });
                }
                else
                {
                    result.Add(new Proxy {Id = null, Name = rule.Name, Code = rule.DefaultCode, RuleCode = rule.Code});
                }
            }

            int totalCount = result.Count;

            return new ListDataResult(result.AsQueryable().Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        private class Proxy
        {
            public long? Id;
            public string Name;
            public TypeCheck Code;
            public string RuleCode;
        }
    }
}