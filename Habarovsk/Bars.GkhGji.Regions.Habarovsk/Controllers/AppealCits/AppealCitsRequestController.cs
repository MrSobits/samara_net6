﻿namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class AppealCitsRequestController : GkhGji.Controllers.AppealCitsRequestController<AppealCitsRequest>
    {
        public AppealCitsRequestController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
            //stamp = Properties.Resources.stamp_Voronezh;
        }


    }
}
