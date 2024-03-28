using Bars.GkhCr.DomainService;

namespace Bars.GkhCr.Export
{
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;
    using B4.Modules.DataExport.Domain;
    using Bars.B4.DataAccess;

    public class OverhaulProposalDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var programId = baseParams.Params.GetAs("programId", string.Empty);
            var municipalityId = baseParams.Params.GetAs("municipalityId", string.Empty);
            var programIds = !string.IsNullOrEmpty(programId) ? programId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            var municipalityIds = !string.IsNullOrEmpty(municipalityId) ? municipalityId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];

            var repo = Container.ResolveRepository<OverhaulProposal>();
            try
            {
                return repo.GetAll()
                       .WhereIf(programIds.Length > 0, x => programIds.Contains(x.ProgramCr.Id))
                   .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
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
                    .Filter(loadParams, Container)
                    .Order(loadParams)
                    .ToList();
            }
            finally
            {
                Container.Release(repo);
            }
        }
    }
}