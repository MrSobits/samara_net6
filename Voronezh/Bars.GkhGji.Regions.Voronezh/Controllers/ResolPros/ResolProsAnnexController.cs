namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class ResolProsAnnexController : GkhGji.Controllers.ResolProsAnnexController<ResolProsAnnex>
    {
        public ResolProsAnnexController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
            stamp = Properties.Resources.stamp_Voronezh;
        }
    }
}