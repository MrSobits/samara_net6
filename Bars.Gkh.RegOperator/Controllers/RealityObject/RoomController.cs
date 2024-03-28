namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Контроллер <see cref="Room"/>
    /// </summary>
    public class RoomController : Bars.Gkh.Controllers.RealityObj.RoomController
    {
        /// <summary>
        /// Метод возвращает суммарную долю собственности по помещению
        /// </summary>
        public ActionResult GetTotalAreaShare(BaseParams baseParams)
        {
            var roomId = baseParams.Params.GetAsId();
            var accountDomain = this.Container.ResolveDomain<BasePersonalAccount>();

            using (this.Container.Using(accountDomain))
            {
                var totalAreaShare = accountDomain.GetAll()
                    .Where(x => x.Room.Id == roomId)
                    .Sum(x => (decimal?)x.AreaShare) ?? 0m;

                return this.JsSuccess(totalAreaShare);
            }
        }
    }
}