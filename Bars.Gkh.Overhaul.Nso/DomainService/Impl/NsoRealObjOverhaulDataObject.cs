namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;

    /// <inheritdoc />
    public class NsoRealObjOverhaulDataObject : BaseRealObjOverhaulDataObject
    {
        public IDomainService<PublishedProgramRecord> PublishProgramRecordDomain { get; set; }

        /// <inheritdoc />
        public override DateTime? PublishDate =>
            this.PublishProgramRecordDomain.GetAll()
                .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                .Where(x => x.Stage2.Stage3Version.RealityObject.Id == realityObject.Id)
                .OrderByDescending(x => x.PublishedProgram.ObjectEditDate)
                .Select(x => x.PublishedProgram.PublishDate)
                .FirstOrDefault();
    }
}