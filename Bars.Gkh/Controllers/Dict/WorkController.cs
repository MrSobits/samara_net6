namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Справочник "Виды работ"
    /// </summary>
    public class WorkController : B4.Alt.DataController<Work>
    {
        public ActionResult ListWorksRealityObjectByPeriod(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Container.Resolve<IWorkService>().ListWorksRealityObjectByPeriod(baseParams);

            if (result.Success)
            {
                return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
            }

            return JsonNetResult.Message(result.Message);
        }

        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Container.Resolve<IWorkService>().ListWithoutPaging(baseParams);

            if (result.Success)
            {
                return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
            }

            return JsonNetResult.Message(result.Message);
        }

        /// <summary>
        /// Добавить связанные записи справочника "Работы по содержанию и ремонту МКД"
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult AddContentRepairMkdWorks(BaseParams baseParams)
        {
            var result = (BaseDataResult)this.Container.Resolve<IWorkService>().AddContentRepairMkdWorks(baseParams, this.DomainService);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Удалить связанную запись справочника "Работы по содержанию и ремонту МКД"
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult DeleteContentRepairMkdWork(BaseParams baseParams)
        {
            var result = (BaseDataResult)this.Container.Resolve<IWorkService>().DeleteContentRepairMkdWork(baseParams, this.DomainService);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}