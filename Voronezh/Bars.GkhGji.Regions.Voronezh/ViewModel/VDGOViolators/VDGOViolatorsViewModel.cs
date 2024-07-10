namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using B4;
    using Entities;
    using Bars.Gkh.Entities;
    using Gkh.Authentification;

    using System.Linq;

    using Bars.Gkh.Utils;

    public class VDGOViolatorsViewModel : BaseViewModel<VDGOViolators>
    {
        public IGkhUserManager UserManager { get; set; }
        
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        
        public override IDataResult List(IDomainService<VDGOViolators> domainService, BaseParams baseParams)
        {
            var thisOperator = UserManager.GetActiveOperator();
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

                return domainService.GetAll()
                    .Where(x => contragentList.Contains(x.Contragent.Id) || (x.MinOrgContragent != null && contragentList.Contains(x.MinOrgContragent.Id)))
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
                    .ToListDataResult(baseParams.GetLoadParam());
            }

            return domainService.GetAll()
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
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}
