using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.RegOperator.Regions.Chelyabinsk.DomainService;
using Microsoft.AspNetCore.Mvc;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Controllers
{
    public class AgentPIRExecuteController : BaseController
    {
        public IAgentPIRExecuteService Service { get; set; }

        public ActionResult GetListPersonalAccountDebtor(BaseParams baseParams)
        {
            return this.Service.GetListPersonalAccountDebtor(baseParams).ToJsonResult();
        }

        public ActionResult AddPersonalAccountDebtor(BaseParams baseParams)
        {
            return this.Service.AddPersonalAccountDebtor(baseParams).ToJsonResult();
        }

        public ActionResult GetListPayment(BaseParams baseParams)
        {
            return this.Service.GetListPayment(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Расчитать дату начала задолженности
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        public ActionResult DebtStartDateCalculate(BaseParams baseParams)
        {
            return this.Service.DebtStartDateCalculate(baseParams).ToJsonResult();
        }
    }
}
