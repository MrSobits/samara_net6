namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Enums;

    using Entities;
    using Enums;
    using Gkh.Domain;

    /// <summary>
    /// The ViewModel
    /// </summary>
    public class OverhaulProposalWorkViewModel : BaseViewModel<OverhaulProposalWork>
    {     

        public override IDataResult List(IDomainService<OverhaulProposalWork> domainService, BaseParams baseParams)
        {           
            var loadParams = GetLoadParam(baseParams);
            var proposalId = baseParams.Params.GetAsId("proposalId");
                
            var data = domainService.GetAll()
                .Where(x => x.OverhaulProposal.Id == proposalId)                   
                .Select(x => new
                {
                    x.Id,
                    WorkName = x.Work.Name,
                    x.Work.TypeWork,
                    UnitMeasureName = x.Work.UnitMeasure.Name,                       
                    x.Volume,                      
                    x.Sum,
                    x.Description,
                    x.DateStartWork,
                    x.DateEndWork                     
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);           
          
        }
      
    }
}