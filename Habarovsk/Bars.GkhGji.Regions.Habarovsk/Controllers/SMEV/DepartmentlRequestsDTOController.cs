using Bars.B4;
using Bars.Gkh.Domain;
using Bars.GkhGji.Regions.Habarovsk.DomainService.InterdepartmentalRequests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Bars.GkhGji.Regions.Habarovsk.Controllers.SMEV
{
    public class DepartmentlRequestsDTOController : BaseController
    {
        public ActionResult GetListInterdepartmentalRequests(BaseParams baseParams)
        {
            var protocolService = Container.Resolve<IIRDTOService>();
            try
            {
                return protocolService.GetListIRDTO(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }
    }
}
