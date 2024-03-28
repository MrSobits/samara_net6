namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class TransitAccountController : B4.Alt.DataController<TransitAccount>
    {
        public ActionResult ExportToTxt(BaseParams baseParams)
        {
            var service = Container.Resolve<ITransitAccountService>();
            try
            {
                var result = service.ExportToTxt(baseParams);
                return new JsonNetResult(result.Data);
            }
            catch (Exception e)
            {
                return JsFailure(e.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
        
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<ITransitAccountService>();
            try
            {
                var result = service.GetInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult MakeDebetList()
        {
            var service = Container.Resolve<ITransitAccountService>();
            try
            {
                service.MakeDebetList();
                return JsSuccess();
            }
            catch 
            {
                return this.JsFailure("Ошибка при формировании дебета");
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult MakeCreditList()
        {
            var service = Container.Resolve<ITransitAccountService>();
            try
            {
                service.MakeCreditList();
                return JsSuccess();
            }
            catch (Exception ex)
            {
                return this.JsFailure(ex.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult DebetList(BaseParams baseParams)
        {
            var service = Container.Resolve<ITransitAccountService>();
            try
            {
                var totalCount = 0;
                var data = service.DebetList(baseParams, true, ref totalCount);
                return new JsonListResult(data, totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult CreaditList(BaseParams baseParams)
        {
            var service = Container.Resolve<ITransitAccountService>();
            try
            {
                var totalCount = 0;
                var data = service.CreditList(baseParams, true, ref totalCount);
                return new JsonListResult(data, totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}