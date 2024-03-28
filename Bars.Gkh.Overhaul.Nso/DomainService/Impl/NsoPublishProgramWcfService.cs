namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Services.DataContracts;

    using Castle.Windsor;

    public class NsoPublishProgramWcfService : IPublishProgramWcfService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<PublishedProgramRecord> PublishedPrgRecDomain { get; set; }

        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public PublishProgRecWcfProxy[] GetPublishProgramRecs(long muId)
        {
            return PublishedPrgRecDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .WhereIf(muId > 0, x => x.Stage2.Stage3Version.RealityObject.Municipality.Id == muId)
                    .Select(x => new PublishProgRecWcfProxy
                    {
                        Municipality = x.Stage2.Stage3Version.RealityObject.Municipality.Name,
                        Address = x.Stage2.Stage3Version.RealityObject.Address,
                        PublishDate = x.ObjectCreateDate.ToShortDateString(),
                        Ceo = x.CommonEstateobject,
                        PublishYear = x.PublishedYear.ToString(),
                        Number = x.IndexNumber
                    })
                    .OrderBy(x => x.Municipality)
                    .ThenBy(x => x.Number)
                    .ThenBy(x => x.PublishYear)
                    .ToArray();
        }
    }
}
