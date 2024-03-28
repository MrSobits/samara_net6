namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Formulas;

    public class FormulaController : BaseController
    {
        private readonly FormulaParameterContainer _prmContainer;
        private readonly IFormulaService _formulaService;

        public FormulaController(
            FormulaParameterContainer prmContainer,
            IFormulaService formulaService)
        {
            _prmContainer = prmContainer;
            _formulaService = formulaService;
        }

        public ActionResult List(BaseParams @params)
        {
            var scope = @params.Params.GetAs<string>("scope");

            return new JsonListResult(_prmContainer.ListParametersByScope(scope));
        }

        public ActionResult CheckFormula(BaseParams @params)
        {
            return new JsonNetResult(_formulaService.CheckFormula(@params.Params.GetAs<string>("formula")));
        }
    }
}