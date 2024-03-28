namespace Bars.Gkh.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
   using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// Меню должников ПИР
    /// </summary>
    public class MenuBuildContractClaimWorkController : BaseMenuController
    {
        public IDomainService<BaseClaimWork> ClaimWorkDomain { get; set; }

        public IDomainService<DocumentClw> DocumentClwDomain { get; set; }

        /// <summary>
        /// Получить меню должников ПИР
        /// </summary>
        /// <param name="storeParams"></param>
        /// <returns></returns>
        public ActionResult GetClaimWorkMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAs("objectId", 0L);
            var type = storeParams.Params.GetAs("type", string.Empty);

            //изначально получаем пункт меню у конкретного основания
            var menuItems = this.GetMenuItems(type) ?? new List<MenuItem>();

            // потом получаем общие документы
            if (id > 0)
            {
                return new JsonNetResult(menuItems.Union(this.GetMenu(id) ?? new List<MenuItem>()));
            }

            return new JsonNetResult(menuItems);
        }

        /// <summary>
        /// Получить пункт меню
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual MenuItem GetItem(DocumentClw document)
        {
            var item = new MenuItem();
            item.AddOption("docId", document.Id);

            switch (document.DocumentType)
            {
                case ClaimWorkDocumentType.Notification:
                    {
                        item.Caption = "Уведомление";
                        item.Href = "claimworkbc/BuildContractClaimWork/{0}/{1}/notification";
                        break;
                    }

                case ClaimWorkDocumentType.ActViolIdentification:
                    {
                        item.Caption = "Акт выявления нарушений";
                        item.Href = "claimworkbc/BuildContractClaimWork/{0}/{1}/actviolidentification";
                        break;
                    }

                case ClaimWorkDocumentType.Pretension:
                    {
                        item.Caption = "Претензия";
                        item.Href = "claimworkbc/BuildContractClaimWork/{0}/{1}/pretension";
                        break;
                    }

                case ClaimWorkDocumentType.Lawsuit:
                    {
                        item.Caption = "Исковое заявление";
                        item.Href = "claimworkbc/BuildContractClaimWork/{0}/{1}/lawsuit";
                        break;
                    }

                case ClaimWorkDocumentType.CourtOrderClaim:
                    {
                        item.Caption = "Заявление о выдаче судебного приказа";
                        item.Href = "claimworkbc/BuildContractClaimWork/{0}/{1}/lawsuit";
                        break;
                    }
            }

            return item;
        }

        /// <summary>
        /// Получение меню
        /// </summary>
        /// <param name="claimWorkId">Основание работы по неплательщикам</param>
        /// <returns></returns>
        public virtual IEnumerable<MenuItem> GetMenu(long claimWorkId)
        {
            var list = new List<MenuItem>();

            if (claimWorkId > 0)
            {
                var service = this.Container.ResolveAll<IAuthorizationService>().FirstOrDefault();
                var userIdentity = this.Container.Resolve<IUserIdentity>();

                list.AddRange(
                    this.FilterInacessibleItems(
                        this.DocumentClwDomain.GetAll()
                                     .Where(x => x.ClaimWork.Id == claimWorkId)
                                     .OrderBy(x => x.ObjectCreateDate)
                                     .Select(x => this.GetItem(x)), 
                        service, 
                        userIdentity));
            }

            return list;
        }
    }
}