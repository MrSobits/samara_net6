namespace Bars.GkhCr.Regions.Tatarstan.ViewModel.RealityObjectOutdoorProgram
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram;

    public class RealityObjectOutdoorProgramChangeJournalViewModel : BaseViewModel<RealityObjectOutdoorProgramChangeJournal>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<RealityObjectOutdoorProgramChangeJournal> domainService, BaseParams baseParams)
        {
            var programId = baseParams.Params.GetAsId("outdoorProgramId");

            return domainService.GetAll()
                .Where(x => x.RealityObjectOutdoorProgram.Id == programId)
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
