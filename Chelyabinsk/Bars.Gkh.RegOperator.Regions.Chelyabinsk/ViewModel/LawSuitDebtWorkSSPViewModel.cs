namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.ViewModel
{
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using System.Linq;

    using B4;
    using Entities;

    public class LawSuitDebtWorkSSPViewModel : BaseViewModel<LawSuitDebtWorkSSP>
    {
        public override IDataResult List(IDomainService<LawSuitDebtWorkSSP> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
                     
            var lawsuitId = baseParams.Params.GetAs("Lawsuit", 0L);
            var docId = loadParams.Filter.GetAs("Lawsuit", 0L);

            if (lawsuitId > 0)
            {
                var data = domain.GetAll()
                    .Where(x => x.Lawsuit.Id == lawsuitId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Lawsuit,
                        x.CbDateDocument,
                        x.CbDateInitiated,
                        x.CbDateSsp,
                        x.CbDateStopped,
                        x.CbDebtSum,
                        x.CbDocumentType,
                        x.CbFactInitiated,
                        x.CbFile,
                        x.CbIsStopped,
                        x.CbNumberDocument,
                        LawsuitOwnerInfo = x.LawsuitOwnerInfo.Name
                    })
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else
            {
                var data = domain.GetAll()
                    .Where(x => x.Lawsuit.Id == docId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Lawsuit,
                        x.CbDateDocument,
                        x.CbDateInitiated,
                        x.CbDateSsp,
                        x.CbDateStopped,
                        x.CbDebtSum,
                        x.CbDocumentType,
                        x.CbFactInitiated,
                        x.CbFile,
                        x.CbIsStopped,
                        x.CbNumberDocument,
                        LawsuitOwnerInfo = x.LawsuitOwnerInfo.Name
                    })
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}