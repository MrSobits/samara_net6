namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Controllers
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Domain;
    using Entities;
    using Gkh.Domain;
    using RegOperator.DomainService.PersonalAccount;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    public class MassDebtWorkSSPController : BaseController
    {
        #region Fields

        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        public IDomainService<Room> RoomDomain { get; set; }

        public IDomainService<LawsuitOwnerInfo> LawSuitOwnerInfoDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }


        /// <summary>
        /// Сервис лицевых счетов
        /// </summary>
        public IPersonalAccountService PersonalAccountService { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        #endregion

        #region Public methods
      
        /// <summary>
        /// Получить собственников по документу ПИР
        /// </summary>
        public ActionResult GetListOwnerByDocId(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var parentId = baseParams.Params.ContainsKey("Lawsuit")
            ? baseParams.Params.GetAs<long>("Lawsuit")
            : 0;
            int totalCount;

            if(parentId>0)
            {
                var data = LawSuitOwnerInfoDomain.GetAll()
                    .Where(x => x.Lawsuit.Id == parentId)
                    .Select(x=> new
                    {
                        x.Id,
                        x.Name
                    }).AsQueryable()
                .Filter(loadParams, this.Container);

                totalCount = data.Count();

                var result = new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
               

                return result.ToJsonResult();
            }
            else
                return this.JsFailure("Собственники не найдены");

         
        }

        #endregion 
    }
}
