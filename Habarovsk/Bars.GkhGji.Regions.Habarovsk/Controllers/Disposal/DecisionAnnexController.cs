namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities;
    // TODO: Заменить Isharp
   // using iTextSharp.text;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class DecisionAnnexController : GkhGji.Controllers.DecisionAnnexController<DecisionAnnex>
    {
        public DecisionAnnexController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
            stamp = Properties.Resources.stamp_Voronezh;
           // blazon = Image.GetInstance(Properties.Resources.blazon_Voronezh);
        }
    }
}