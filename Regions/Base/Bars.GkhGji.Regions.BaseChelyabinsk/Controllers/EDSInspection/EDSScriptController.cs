namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;

    /// <summary>
    /// Контроллер для Документ ГЖИ
    /// </summary>
    public class EDSScriptController : BaseController
    {
       

        private IFileManager _fileManager;
        private IDomainService<FileInfo> _fileDomain;
        public EDSScriptController(IFileManager fileManager, IDomainService<FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }

        public ActionResult ListEDSDocuments(BaseParams baseParams)
        {
            var appealService = Container.Resolve<IEDSDocumentService>();
            try
            {
                return appealService.ListEDSDocuments(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }
        public ActionResult ListEDSNotice(BaseParams baseParams)
        {
            var appealService = Container.Resolve<IEDSDocumentService>();
            try
            {
                return appealService.ListEDSNotice(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

        public ActionResult ListEDSMotivRequst(BaseParams baseParams)
        {
            var appealService = Container.Resolve<IEDSDocumentService>();
            try
            {
                return appealService.ListEDSMotivRequst(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

        public ActionResult ListEDSDocumentsForSign(BaseParams baseParams)
        {
            var appealService = Container.Resolve<IEDSDocumentService>();
            try
            {
                var result = appealService.ListEDSDocumentsForSign(baseParams);
                if (result != null)
                {
                    return result.ToJsonResult();
                }
                else
                    return null;
            }
            finally
            {

            }
        }

        public ActionResult GetListGjiDoc(BaseParams baseParams)
        {
            var protocolService = Container.Resolve<IEDSDocumentService>();
            try
            {
                return protocolService.GetListGjiDoc(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }       

    }
}