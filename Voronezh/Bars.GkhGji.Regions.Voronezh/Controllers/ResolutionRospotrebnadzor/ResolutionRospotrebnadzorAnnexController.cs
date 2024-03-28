namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class ResolutionRospotrebnadzorAnnexController : GkhGji.Controllers.ResolutionRospotrebnadzorAnnexController<ResolutionRospotrebnadzorAnnex>
    {
        private IFileManager _fileManager;
        private IDomainService<FileInfo> _fileDomain;
        public ResolutionRospotrebnadzorAnnexController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
            stamp = Properties.Resources.stamp_Voronezh;
        }
    }
}