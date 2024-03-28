namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System;
    using System.Linq;
    using B4;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class ShortProgramProtocolViewModel : BaseViewModel<ShortProgramProtocol>
    {
        public override IDataResult List(IDomainService<ShortProgramProtocol> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domainService.GetAll()
                .Where(x => x.ShortObject.Id == objectId)
                .Select(x => new
                    {
                        x.Id,
                        x.DocumentName,
                        x.DocumentNum,
                        x.Description,
                        DateFrom = x.DateFrom.HasValue && x.DateFrom.Value != DateTime.MinValue ? x.DateFrom : null,
                        x.CountAccept,
                        x.CountVote,
                        x.CountVoteGeneral,
                        x.GradeOccupant,
                        x.GradeClient,
                        Contragent = x.Contragent.Name,
                        x.File
                    })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}