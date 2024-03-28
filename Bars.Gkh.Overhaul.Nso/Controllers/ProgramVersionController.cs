using System.Collections;

namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class ProgramVersionController : B4.Alt.DataController<ProgramVersion>
    {
        public ActionResult CopyProgram(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IProgramVersionService>().CopyProgram(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ChangeRecordsIndex(BaseParams baseParams)
        {
            var result = Container.Resolve<IProgramVersionService>().ChangeRecordsIndex(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ActualizeNewRecords(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeNewRecords(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ActualizeSum(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeSum(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ActualizeYear(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeYear(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult GetWarningMessage(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.GetWarningMessage(baseParams);
                return result.Success ? JsSuccess(result.Message) : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult GetDeletedEntriesList(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = (ListDataResult)service.GetDeletedEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ActualizeDeletedEntries(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeDeletedEntries(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ActualizeOrder(BaseParams baseParams)
        {
            var service = Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeOrder(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}