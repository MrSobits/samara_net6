namespace Bars.GkhGji.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Fasterflect;

    using Microsoft.Extensions.Logging;

    public class BaseDispHeadController : BaseDispHeadController<BaseDispHead>
    {
    }

    public class BaseDispHeadController<T> : FileStorageDataController<T>
        where T : BaseDispHead
    {
        /// <inheritdoc />
        public override ActionResult Update(BaseParams baseParams)
        {
            var inspectionDocGjiReferenceDomain = this.Container.Resolve<IDomainService<InspectionDocGjiReference>>();

            using (var tr = this.Container.Resolve<IDataTransaction>())
            using (this.Container.Using(inspectionDocGjiReferenceDomain))
            {
                try
                {
                    var result = (JsonNetResult)base.Update(baseParams);
                    var resultSuccess = (bool)result.Data.GetPropertyValue("success");

                    if (!resultSuccess)
                    {
                        tr.Rollback();
                        return result;
                    }

                    var resultData = result.Data.GetPropertyValue("data");
                    var entity = ((List<T>)resultData).First();

                    var prevDocs = inspectionDocGjiReferenceDomain.GetAll()
                        .Where(x => x.Inspection.Id == entity.Id)
                        .Select(x => new { x.Id, DocumentId = x.Document.Id })
                        .ToList();

                    foreach (var prevDoc in prevDocs)
                    {
                        if (prevDoc.DocumentId != entity.PrevDocument?.Id)
                        {
                            inspectionDocGjiReferenceDomain.Delete(prevDoc.Id);
                        }
                    }

                    if (entity.PrevDocument != null && prevDocs.All(x => x.DocumentId != entity.PrevDocument.Id))
                    {
                        inspectionDocGjiReferenceDomain.Save(new InspectionDocGjiReference
                        {
                            Document = entity.PrevDocument,
                            Inspection = entity,
                            TypeReference = TypeInspectionDocGjiReference.DispHeadPrevDocument
                        });
                    }

                    tr.Commit();

                    return result;
                }
                catch (Exception e)
                {
                    tr.Rollback();
                    var logManager = this.Container.Resolve<ILogger>();
                    logManager.LogError(e, "Ошибка");
                    return new BaseDataResult(false, "При сохранении возникла ошибка").ToJsonResult();
                }
            }
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseDispHeadService>();
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

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("BaseDispHeadDataExport");
            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally 
            {
                Container.Release(export);
            }
        }
    }
}