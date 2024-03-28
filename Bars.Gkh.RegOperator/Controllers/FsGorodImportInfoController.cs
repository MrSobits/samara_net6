namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Imports.FsGorod;
    using Newtonsoft.Json;

    public class FsGorodImportInfoController : B4.Alt.DataController<FsGorodImportInfo>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var entity = DomainService.Get(id);

            if (entity == null)
            {
                throw new ArgumentException(string.Format("Настройка с идентификатором {0} не найдена.", id));
            }

            var json = JsonConvert.SerializeObject(FsGorodImportInfoProxy.FromEntity(entity));

            return new JsonNetResult(json);
        }
    }
}
