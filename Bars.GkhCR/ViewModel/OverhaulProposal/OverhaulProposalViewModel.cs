using Bars.Gkh.Domain;

namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Enums;
    using Entities;

    public class OverhaulProposalViewModel : BaseViewModel<OverhaulProposal>
    {
        public override IDataResult List(IDomainService<OverhaulProposal> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var programId = baseParams.Params.GetAs("programId", string.Empty);
            var municipalityId = baseParams.Params.GetAs("municipalityId", string.Empty);
            var programIds = !string.IsNullOrEmpty(programId) ? programId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            var municipalityIds = !string.IsNullOrEmpty(municipalityId) ? municipalityId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];

            var data = domainService.GetAll()
                 .WhereIf(programIds.Length > 0, x => programIds.Contains(x.ProgramCr.Id))
                   .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id) )
               .Select(x => new
                    {
                        x.Id,
                        x.ProgramNum,
                        x.Description,
                        ObjectCr = x.ObjectCr.RealityObject.Address,
                        Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                        ProgramCr = x.ProgramCr.Name,
                        x.State,
                        Apartments = x.ObjectCr.RealityObject.NumberApartments,
                        Entryes = x.ObjectCr.RealityObject.NumberEntrances,
                        Index = x.ObjectCr.RealityObject.FiasAddress.PostCode
               })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.ObjectCr)
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = loadParams.Order.Length == 0 ? data.Paging(loadParams) : data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<OverhaulProposal> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var obj =
                domainService.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        ObjectCr = new {x.ObjectCr.Id, x.ObjectCr.RealityObject.Address },
                      x.ProgramCr,
                      x.ProgramNum,
                      x.DateEndBuilder,
                      x.DateStartWork
                    })
                    .FirstOrDefault();

            return new BaseDataResult(obj);
        }
    }
}