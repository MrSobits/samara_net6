namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class OwnerProtocolTypeDecisionViewModel : BaseViewModel<OwnerProtocolTypeDecision>
    {
        public override IDataResult List(IDomainService<OwnerProtocolTypeDecision> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var docId = baseParams.Params.GetAs<long>("docId");

            if (docId != 0)
            {
                var data = domainService.GetAll()
                    .Where(x => x.OwnerProtocolType.Id == docId)
                    .Select(item => new
                {
                    item.Id,
                    OwnerProtocolType = item.OwnerProtocolType.Id,
                    item.Name
                }).Filter(loadParam, Container);

                int totalCount = data.Count();

                var returnData = data.Order(loadParam).Paging(loadParam).ToList();

                return new ListDataResult(returnData, totalCount);
            }
            //else if (protocolId != 0)
            //{
            //    decisionList =
            //       ownerMeetingProt.GetAll()
            //           .Join(domainService.GetAll(), x => x.OwnerProtocolType.Id, y => y.OwnerProtocolType.Id, (x, y) => new { OwnerMeetingProtocol = x, OwnerProtocolTypeDecision = y })
            //           .Where(x => x.OwnerMeetingProtocol.Id == protocolId)
            //           .Select(x => x.OwnerProtocolTypeDecision.Id)
            //           .Distinct()
            //           .ToList();

            //    var data = domainService.GetAll().WhereIf(decisionList.Count > 0, x => decisionList.Contains(x.Id)).Select(x => new { x.Id, x.Name }).Filter(loadParam, Container);

            //    int totalCount = data.Count();

            //    return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            //}
            else
            {
                var data = domainService.GetAll()
                    .Where(x => x.Id == 0);
                int totalCount = data.Count();
                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
        }
    }
}