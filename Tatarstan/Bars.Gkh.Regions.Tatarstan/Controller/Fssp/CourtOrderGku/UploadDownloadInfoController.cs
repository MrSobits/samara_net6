namespace Bars.Gkh.Regions.Tatarstan.Controller.Fssp.CourtOrderGku
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;
    using Bars.Gkh.Regions.Tatarstan.Services.ServicesContracts.Fssp.CourtOrderGku;

    public class UploadDownloadInfoController : B4.Alt.DataController<UploadDownloadInfo>
    {
        public ActionResult Import(BaseParams baseParams)
        {
            var importService = this.Container.Resolve<ICourtOrderInfoImportService>();

            using (this.Container.Using(importService))
            {
                try
                {
                    var result = importService.Import(baseParams);

                    if (result.Success)
                    {
                        return new JsonNetResult(new
                        {
                            success = result.Success,
                            title = string.Empty,
                            message = "Файл добавлен в очередь загрузки"
                        });
                    }

                    return JsonNetResult.Failure(result.Message);
                }
                catch (Exception e)
                {
                    return JsonNetResult.Failure(e.Message);
                }
            }
        }
    }
}