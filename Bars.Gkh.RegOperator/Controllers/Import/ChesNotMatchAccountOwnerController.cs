namespace Bars.Gkh.RegOperator.Controllers.Import
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;

    public class ChesNotMatchAccountOwnerController : B4.Alt.DataController<ChesNotMatchAccountOwner>
    {
        public IChesComparingService ChesComparingService { get; set; }

        /// <summary>
        /// Сопоставить владельца
        /// </summary>
        public ActionResult MatchOwner(BaseParams baseParams)
        {
            return this.ChesComparingService.MatchOwner(baseParams).ToJsonResult();
        }
    }
}