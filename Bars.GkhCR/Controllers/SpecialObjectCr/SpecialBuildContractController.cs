namespace Bars.GkhCr.Controllers
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class SpecialBuildContractController : FileStorageDataController<SpecialBuildContract>
    {
        public ActionResult AddTypeWorks(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISpecialBuildContractService>();

            using (this.Container.Using(service))
            {
                var result = (BaseDataResult)service.AddTypeWorks(baseParams);
                return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
            }
        }

        public override ActionResult Delete(BaseParams baseParams)
        {
            var domain = this.Container.Resolve<IDomainService<SpecialBuildContract>>();
            try
            {
                var result = domain.Delete(baseParams);
                return new JsonNetResult(new { success = result.Success, message = result.Message, data = result.Data }) { ContentType = "text/html; charset=utf-8" };
            }
            catch (ValidationException exc)
            {
                var result = JsonNetResult.Failure(exc.Message);
                result.ContentType = "text/html; charset=utf-8";
                return result;
            }
            finally
            {
                this.Container.Release(domain);
            }
        }

        public ActionResult ListAvailableBuilders(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISpecialBuildContractService>();
            using (this.Container.Using(service))
            {
                var result = service.ListAvailableBuilders(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
        }
    }
}