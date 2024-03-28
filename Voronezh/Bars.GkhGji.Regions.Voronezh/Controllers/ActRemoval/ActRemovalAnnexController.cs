namespace Bars.GkhGji.Regions.Voronezh.Controllers.ActRemoval
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class ActRemovalAnnexController : BaseChelyabinsk.Controllers.ActRemoval.ActRemovalAnnexController<ActRemovalAnnex>
    {
        public ActRemovalAnnexController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
            stamp = Properties.Resources.stamp_Voronezh;
        }
    }
}