namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;

    public class ProtocolCrViewModel : BaseViewModel<ProtocolCr>
    {
        public override IDataResult List(IDomainService<ProtocolCr> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAsId("objectCrId");
            var twId = baseParams.Params.GetAsId("twId");

            if (objectCrId == 0)
            {
                objectCrId = loadParams.Filter.GetAs("objectCrId", 0l);
            }

            if (twId == 0)
            {
                twId = loadParams.Filter.GetAs("twId", 0l);
            }

            var data = domainService.GetAll()
                .Where(x => x.ObjectCr.Id == objectCrId)
                .WhereIf(twId > 0, x => x.TypeWork.Id == twId)
                .Select(x => new
                {
                    x.Id,
                    x.TypeDocumentCr,
                    x.DocumentName,
                    x.DocumentNum,
                    x.Description,
                    DateFrom = x.DateFrom.HasValue && x.DateFrom.Value != DateTime.MinValue ? x.DateFrom : null,
                    x.CountAccept,
                    x.CountVote,
                    x.CountVoteGeneral,
                    x.GradeOccupant,
                    x.GradeClient,
                    ContragentName = x.Contragent.Name,
                    x.File
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}