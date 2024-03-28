namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{
    using B4;
    using Entities;
    using Bars.Gkh.Entities;
    using Gkh.Authentification;
    using System.Collections.Generic;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class VDGOViolatorsViewModel : BaseViewModel<VDGOViolators>
    {
        
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        public override IDataResult List(IDomainService<VDGOViolators> domainService, BaseParams baseParams)
        {

            var loadParams = GetLoadParam(baseParams);
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator?.Inspector == null)
            {
                var contragent = thisOperator.Contragent;

                var contragentList = OperatorContragentDomain.GetAll()
                    .Where(x => x.Contragent != null)
                    .Where(x => x.Operator == thisOperator)
                    .Select(x => x.Contragent.Id).Distinct().ToList();
                if (contragent != null)
                {
                    if (!contragentList.Contains(contragent.Id))
                    {
                        contragentList.Add(contragent.Id);
                    }
                }
                var data = domainService.GetAll()
                .Where(x => contragentList.Contains(x.Contragent.Id) ||(x.MinOrgContragent != null && contragentList.Contains(x.MinOrgContragent.Id)))
                .Select(x => new
                {
                    x.Id,
                    Contragent = x.Contragent.Name,
                    MinOrgContragent = x.MinOrgContragent.Name,
                    Address = x.Address.Address,
                    x.NotificationDate,
                    x.NotificationNumber,
                    x.FIO,
                    x.Email,
                    x.PhoneNumber,
                    x.DateExecution,
                    x.MarkOfExecution,
                    x.Description,
                    File = x.File.Name,
                    NotificationFile = x.NotificationFile.Name,
                    x.MarkOfMessage

                })
                .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else
            {
                var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Contragent = x.Contragent.Name,
                    MinOrgContragent = x.MinOrgContragent.Name,
                    Address = x.Address.Address,
                    x.NotificationDate,
                    x.NotificationNumber,
                    x.FIO,
                    x.Email,
                    x.PhoneNumber,
                    x.DateExecution,
                    x.MarkOfExecution,
                    x.Description,
                    File = x.File.Name,
                    x.NotificationFile

                })
                .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }


                
        }
    }
}
