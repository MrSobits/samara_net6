namespace Bars.Gkh.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
   using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// Меню должников ПИР
    /// </summary>
    public class MenuClaimWorkController : BaseMenuController
    {
        public IDomainService<BaseClaimWork> ClaimWorkDomain { get; set; }
        public IDomainService<CourtOrderClaim> CourtOrderClaimDomain { get; set; }
        public IDomainService<Lawsuit> LawsuitClaimDomain { get; set; }
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
                        item.Href = "claimwork/{0}/{1}/{2}/notification";
                        break;
                    }

                case ClaimWorkDocumentType.ActViolIdentification:
                    {
                        item.Caption = "Акт выявления нарушений";
                        item.Href = "claimwork/{0}/{1}/{2}/actviolidentification";
                        break;
                    }

                case ClaimWorkDocumentType.Pretension:
                    {
                        item.Caption = "Претензия";
                        item.Href = "claimwork/{0}/{1}/{2}/pretension";
                        break;
                    }

                case ClaimWorkDocumentType.Lawsuit:
                    {
                        item.Caption = $"Исковое заявление {GetDocDate(document)}";
                        item.Href = "claimwork/{0}/{1}/{2}/lawsuit";
                        break;
                    }

                case ClaimWorkDocumentType.CourtOrderClaim:
                    {
                        item.Caption = $"ЗВСП {GetDocDate(document)}";
                        item.Href = "claimwork/{0}/{1}/{2}/lawsuit";
                        break;
                    }

                case ClaimWorkDocumentType.ExecutoryProcess:
                    {
                        item.Caption = "Исполнительное производство";
                        item.Href = "claimwork/{0}/{1}/{2}/execprocess";
                        item.AddRequiredPermission("Clw.ClaimWork.UtilityDebtor.ExecutoryProcess.View");
                        break;
                    }

                case ClaimWorkDocumentType.SeizureOfProperty:
                    {
                        item.Caption = "Постановление о наложении ареста на имущество";
                        item.Href = "claimwork/{0}/{1}/{2}/propseizure";
                        item.AddRequiredPermission("Clw.ClaimWork.UtilityDebtor.SeizureOfProperty.View");
                        break;
                    }

                case ClaimWorkDocumentType.DepartureRestriction:
                    {
                        item.Caption = "Постановление об ограничении выезда из РФ";
                        item.Href = "claimwork/{0}/{1}/{2}/departrestrict";
                        item.AddRequiredPermission("Clw.ClaimWork.UtilityDebtor.DepartureRestriction.View");
                        break;
                    }

                case ClaimWorkDocumentType.RestructDebt:
                {
                    item.Caption = ClaimWorkDocumentType.RestructDebt.GetDisplayName();
                    item.Href = "claimwork/{0}/{1}/{2}/restructdebt";
                    break;
                }

                case ClaimWorkDocumentType.RestructDebtAmicAgr:
                {
                    item.Caption = ClaimWorkDocumentType.RestructDebtAmicAgr.GetDisplayName();
                    item.Href = "claimwork/{0}/{1}/{2}/restructdebtamicagr";
                    break;
                }
            }

            return item;
        }
        private string GetDocDate(DocumentClw document)
        {
            if (document is CourtOrderClaim)
            {
                var clwDocument = CourtOrderClaimDomain.GetAll().FirstOrDefault(x => x.Id == document.Id);
                string str = "";
                if (clwDocument != null)
                {
                    str = " " + clwDocument.BidNumber;
                    if (clwDocument.BidDate.HasValue)
                    {
                        str += " от " + clwDocument.BidDate.Value.ToString("dd.MM.yyyy");
                    }
                }
                else if (document.DocumentDate.HasValue)
                {
                    return " от " + document.DocumentDate.Value.ToString("dd.MM.yyyy");
                }
                return str;
            }
            else 
            {
                var courtorder = LawsuitClaimDomain.GetAll().FirstOrDefault(x => x.Id == document.Id);
                string str = "";
                if (courtorder != null)
                {
                    str = " " + courtorder.BidNumber;
                    if (courtorder.BidDate.HasValue)
                    {
                        str += " от " + courtorder.BidDate.Value.ToString("dd.MM.yyyy");
                    }
                }
                else if (document.DocumentDate.HasValue)
                {
                    return " от " + document.DocumentDate.Value.ToString("dd.MM.yyyy");
                }
                return str;
            }
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
                var menuItems = this.DocumentClwDomain.GetAll()
                    .Where(x => x.ClaimWork.Id == claimWorkId)
                    .OrderBy(x => x.ObjectCreateDate)
                    .Select(this.GetItem)
                    .ToList();

                list.AddRange(
                    this.FilterInacessibleItems(menuItems, 
                        service, 
                        userIdentity));
            }

            return list;
        }
    }
}