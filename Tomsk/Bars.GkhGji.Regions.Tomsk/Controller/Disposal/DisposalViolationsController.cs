namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.DomainService;

    public class DisposalViolationsController : B4.Alt.DataController<Disposal>
    {

        public IDisposalViolationsService Service { get; set; }

        //Данный метод поулчает неустраненне нарушения по предписанию данного распоряжения
        public ActionResult GetListViolations(BaseParams baseParams)
        {
            return Service.GetListNotRemovedViolations(baseParams);
        }
    }
}
