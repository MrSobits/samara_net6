using Bars.Gkh.Overhaul.Tat.Entities;

namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Services.DataContracts;

    using Castle.Windsor;

    public class TatPublishProgramWcfService : IPublishProgramWcfService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<VersionRecordStage2> Stage2Domain { get; set; }

        public PublishProgRecWcfProxy[] GetPublishProgramRecs(long muId)
        {
            return Stage2Domain.GetAll()
                        .Where(x => x.Stage3Version.ProgramVersion.IsMain && x.Stage3Version.ProgramVersion.State.FinalState)
                        .WhereIf(muId > 0, x => x.Stage3Version.ProgramVersion.Municipality.Id == muId)
                        .OrderBy(x => x.Stage3Version.RealityObject.Municipality.Name)
                        .ThenBy(x => x.Stage3Version.Year)
                        .Select(x => new PublishProgRecWcfProxy
                        {
                            Municipality = x.Stage3Version.RealityObject.Municipality.Name,
                            Address = x.Stage3Version.RealityObject.Address,
                            PublishDate = x.ObjectCreateDate.ToShortDateString(),
                            Ceo = x.CommonEstateObject.Name,
                            PublishYear = x.Stage3Version.CorrectYear.ToString(),
                            Number = x.Stage3Version.IndexNumber
                        })
                        .ToArray();
           
        }
    }
}