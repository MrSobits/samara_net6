namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using DomainModelServices;
    using Gkh.Domain;
    using Gkh.Entities;

    /// <summary>
    /// Контроллер
    /// </summary>
    public class RealityObjectAccountUpdaterController : BaseController
    {
        /// <summary>
        /// Обновить баланс
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns></returns>
        public ActionResult UpdateAccount(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var updater = this.Container.Resolve<IRealityObjectAccountUpdater>();
            var repo = this.Container.ResolveRepository<RealityObject>();

            using (this.Container.Using(updater, repo))
            {
                updater.UpdateBalance(repo.GetAll().Where(x => x.Id == id));
            }

            return this.JsSuccess();
        }
    }
}