namespace Bars.GkhGji.Regions.Voronezh.Controllers.Protocol197
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class Protocol197AnnexController : BaseChelyabinsk.Controllers.ActRemoval.Protocol197AnnexController<Protocol197Annex>
    {
        public Protocol197AnnexController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
            stamp = Properties.Resources.stamp_Voronezh;
        }
    }
}