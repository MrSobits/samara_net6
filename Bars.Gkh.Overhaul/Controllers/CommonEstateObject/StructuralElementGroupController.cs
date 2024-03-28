namespace Bars.Gkh.Overhaul.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Formulas;

    public class StructuralElementGroupController : B4.Alt.DataController<StructuralElementGroup>
    {
        public ActionResult GetParamsList(BaseParams baseParams)
        {
            var service = Container.Resolve<IFormulaService>();
            var formula = baseParams.Params.GetAs<string>("formula");

            return new JsonNetResult(service.GetParamsList(formula));
        }

        public ActionResult CheckFormula(BaseParams baseParams)
        {
            var service = Container.Resolve<IFormulaService>();
            var formula = baseParams.Params.GetAs<string>("formula");

            return new JsonNetResult(service.CheckFormula(formula));
        }

        public ActionResult TransformFormulas()
        {
            var codesToAdd = new Dictionary<string, string> { {"d", "RoBuildYear"}, {"e", "DpkrEndYearResolver"} };
            var paramResolvers = Container.ResolveAll<IFormulaParameter>().Where(x => codesToAdd.Values.Contains(x.Code));

            var allGroups = DomainService.GetAll().ToList();

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                foreach (var group in allGroups)
                {
                    foreach (var code in codesToAdd)
                    {
                        if (group.FormulaParams.All(x => x.ValueResolverCode != code.Value)
                            && paramResolvers.Any(x => x.Code == code.Value))
                        {
                            var resolver = paramResolvers.FirstOrDefault(x => x.Code == code.Value);
                            group.FormulaParams.Add(new FormulaParams
                            {
                                Name = code.Key,
                                ValueResolverCode = resolver.Code,
                                ValueResolverName = resolver.DisplayName
                            });
                        }
                    }

                    group.Formula = "if(a!=0, if(a + b < c, c, a + b), if(d!=0, if(d + b < c, c, d + b), e))";

                    DomainService.Update(group);
                }

                try
                {
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }

            return this.JsSuccess();
        }
    }
}