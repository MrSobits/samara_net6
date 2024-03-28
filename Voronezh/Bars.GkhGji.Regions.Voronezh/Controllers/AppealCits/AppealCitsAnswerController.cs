namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities;
    // TODO: Заменить Itextsharp
    //using iTextSharp.text;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class AppealCitsAnswerController : GkhGji.Controllers.AppealCitsAnswerController<AppealCitsAnswer>
    {
        public AppealCitsAnswerController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
            stamp = Properties.Resources.stamp_Voronezh;
           // blazon = Image.GetInstance(Properties.Resources.blazon_Voronezh);
        }
    }
}