namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.Gkh.Domain;

    using DomainService;
    using Entities;

    public class ProgramVersionController : B4.Alt.DataController<ProgramVersion>
    {
        public IProgramVersionService Service { get; set; }

        public ActionResult CopyProgram(BaseParams baseParams)
        {
            var result = (BaseDataResult) this.Container.Resolve<IProgramVersionService>().CopyProgram(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetMainVersionByMunicipality(BaseParams baseParams)
        {
            var result = this.Service.GetMainVersionByMunicipality(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ActualizeFromShortCr(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeFromShortCr(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult ActualizeNewRecords(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeNewRecords(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult ActualizeSum(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeSum(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult ActualizeYear(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeYear(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult GetWarningMessage(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.GetWarningMessage(baseParams);
                return result.Success ? this.JsSuccess(result.Message) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult GetDeletedEntriesList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.GetDeletedEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult ActualizeDeletedEntries(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeDeletedEntries(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult ActualizeGroup(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeGroup(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult ActualizeOrder(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeOrder(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult ListMainVersions(BaseParams baseParams)
        {
            var result = this.Service.ListMainVersions(baseParams);
            return result.ToJsonResult();
        }
    }
}