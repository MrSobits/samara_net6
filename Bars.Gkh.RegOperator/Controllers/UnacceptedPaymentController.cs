namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using B4.IoC;

    using Bars.B4.Utils;
    
    using DomainService;
    using Entities;

    public class UnacceptedPaymentController : B4.Alt.DataController<UnacceptedPayment>
    {
        public IUnacceptedPaymentService Service { get; set; }

        public ActionResult AcceptPayments(BaseParams baseParams)
        {
            MarkUnacceptedPaymemntPacketsAsInProgress(baseParams);
            var result = Service.AcceptPayments(baseParams);
            return result.Success
                ? new JsonNetResult(new { sucess = true, message = result.Message })
                : JsFailure(result.Message);
        }

        public ActionResult CancelPayments(BaseParams baseParams)
        {
            var result = Resolve<IUnacceptedPaymentService>().CancelPayments(baseParams);
            return result.Success
                ? new JsonNetResult(new { sucess = true, message = result.Message })
                : JsFailure(result.Message);
        }

        public ActionResult RemovePayments(BaseParams baseParams)
        {
            var result = Resolve<IUnacceptedPaymentService>().RemovePayments(baseParams);
            return result.Success
                ? new JsonNetResult(new { sucess = true, message = result.Message })
                : JsFailure(result.Message);
        }

        private void MarkUnacceptedPaymemntPacketsAsInProgress(BaseParams baseParams)
        {
            var packetIds = baseParams.Params.GetAs<long[]>("packetIds", ignoreCase: true);
            var unacceptedPacketsDomain = Container.ResolveDomain<UnacceptedPaymentPacket>();
            using (Container.Using(unacceptedPacketsDomain))
            {
                var packets = unacceptedPacketsDomain.GetAll().Where(x => packetIds.Contains(x.Id));

                packets.ForEach(x =>
                {
                    x.SetInProgress();
                    unacceptedPacketsDomain.Update(x);
                });
            }
        }
    }
}