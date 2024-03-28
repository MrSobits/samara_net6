namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class ProtocolAnnexController : GkhGji.Controllers.ProtocolAnnexController<ProtocolAnnex>
    {
        public ProtocolAnnexController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
            stamp = Properties.Resources.stamp_Voronezh;
        }
    }
}