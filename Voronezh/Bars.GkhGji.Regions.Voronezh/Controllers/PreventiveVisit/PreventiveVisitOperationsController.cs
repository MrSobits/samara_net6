namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Castle.Windsor;
    using Entities;
    using Enums;
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    ////// костыль 
    //using System.Xml;
    //using System.Xml.Linq;
    ////// костыль - end

    /// <summary>
    /// Контроллер для запросов в ГИС ГМП для получения платежей
    /// </summary>
    public class PreventiveVisitOperationsController : BaseController
    {
        #region Fields

        private IFileManager _fileManager;

        private IDomainService<FileInfo> _fileDomain;



        /// <summary>
        /// Менеджер задач
        /// </summary>
        private readonly ITaskManager _taskManager;

        private readonly IWindsorContainer _container;

        #endregion

        #region Constructors

        public PreventiveVisitOperationsController(IWindsorContainer container, IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<IPreventiveVisitService>().AddRealityObjects(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IPreventiveVisitService>();
            try
            {
                var result = service.GetInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult GetListRoForResultPV(BaseParams baseParams)
        {
            var service = Container.Resolve<IPreventiveVisitService>();
            try
            {
                return service.GetListRoForResultPV(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

        #endregion

    }
}