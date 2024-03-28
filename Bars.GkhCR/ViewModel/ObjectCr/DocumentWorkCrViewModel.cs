namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;
    using Gkh.Domain;

    public class DocumentWorkCrViewModel : BaseViewModel<DocumentWorkCr>
    {
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        public IGkhUserManager UserManager { get; set; }

        public override IDataResult List(IDomainService<DocumentWorkCr> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAsId("objectCrId");
            var twId = baseParams.Params.GetAsId("twId");

            if (objectCrId == 0)
            {
                objectCrId = loadParams.Filter.GetAsId("objectCrId");
            }

            if (twId == 0)
            {
                twId = loadParams.Filter.GetAsId("twId");
            }

            Operator thisOperator = UserManager.GetActiveOperator();
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
                .Where(x => x.ObjectCr.Id == objectCrId)
                .WhereIf(twId > 0, x => x.TypeWork.Id == twId)
                .WhereIf(contragentList.Count > 0, x => contragentList.Contains(x.Contragent.Id))
                .Select(x => new
                {
                    x.Id,
                    ContragentName = x.Contragent.Name,
                    x.DocumentName,
                    x.DocumentNum,
                    x.DateFrom,
                    x.Description,
                    x.File
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}