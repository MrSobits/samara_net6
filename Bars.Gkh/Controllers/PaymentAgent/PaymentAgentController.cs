using System.Collections;
using Bars.Gkh.DomainService;

namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Entities;

    // Пустышка на тот случай если гдето от этого касса наследовались
    public class PaymentAgentController : PaymentAgentController<PaymentAgent>
    {
        // Внимание !!! методы добавлять в Generic класс
    }

    public class PaymentAgentController<T> : B4.Alt.DataController<T>
        where T : PaymentAgent
    {
        
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("PaymentAgentDataExport");
            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally 
            {
                Container.Release(export);
            }
        }

        public ActionResult ListForExport(BaseParams baseParams)
        {
            var service = Container.Resolve<IPaymentAgentService>();
            try
            {
                var result = (ListDataResult)service.ListForExport(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
} 