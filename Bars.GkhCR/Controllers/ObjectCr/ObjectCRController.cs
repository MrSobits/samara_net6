namespace Bars.GkhCr.Controllers
{
    using System;
    using System.Collections;

    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.DomainService;

    /// <summary>
    /// Контроллер <see cref="Entities.ObjectCr"/>
    /// </summary>
    public class ObjectCrController : B4.Alt.DataController<Entities.ObjectCr>
    {
        /// <summary>
        /// Сервис
        /// </summary>
        public IObjectCrService Service { get; set; }

        /// <summary>
        /// Экспорт
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ObjectCrDataExport");
            return export?.ExportData(baseParams);
        }

        /// <summary>
        /// Экспорт
        /// </summary>
        public ActionResult ExportProposalList(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("OverhaulProposalDataExport");
            return export?.ExportData(baseParams);
        }


        /// <summary>
        /// Массовое изменение статуса
        /// </summary>
        public ActionResult MassChangeState(BaseParams baseParams)
        {
            var result = this.Service.MassChangeState(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить дома в программе
        /// </summary>
        public ActionResult RealityObjectByProgramm(BaseParams baseParams)
        {
            var listResult = (ListDataResult)this.Service.RealityObjectsByProgramm(baseParams);
            return new JsonNetResult(new { success = true, data = listResult.Data, totalCount = listResult.TotalCount });
        }

        /// <summary>
        /// Получить статусы
        /// </summary>
        public ActionResult GetTypeStates(BaseParams baseParams)
        {
            var listResult = (ListDataResult)this.Service.GetTypeStates(baseParams);
            return new JsonNetResult(new { success = true, data = listResult.Data, totalCount = listResult.TotalCount });
        }

        /// <summary>
        /// Создать контракт
        /// </summary>
        public ActionResult CreateContract(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs("lotIds", string.Empty).ToLongArray();
            var objectId = baseParams.Params.GetAs("objectId", string.Empty).ToLong();

            var result = this.Service.CreateContract(objectId, ids);

            return new JsonNetResult(new { success = result.Success, message = result.Message });
        }

        /// <summary>
        /// Получить подрядчиков
        /// </summary>
        public ActionResult GetBuilders(BaseParams baseParams)
        {
            var listResult = (ListDataResult)this.Service.GetBuilders(baseParams);
            return new JsonNetResult(new { success = true, data = listResult.Data, totalCount = listResult.TotalCount });
        }

        /// <summary>
        /// Получить тип изменения 
        /// </summary>
        public ActionResult GetTypeChangeProgramCr(BaseParams baseParams)
        {
            var result = this.Service.GetTypeChangeProgramCr(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Список конкурсов
        /// </summary>
        public ActionResult ListCompetitions(BaseParams baseParams)
        {
            var listResult = (ListDataResult)this.Resolve<IObjectCrCompetitionService>().ListCompetitions(baseParams);
            return new JsonNetResult(new { success = listResult.Success, data = listResult.Data, totalCount = listResult.TotalCount });
        }

        /// <summary>
        /// Список договоров подряда
        /// </summary>
        public ActionResult GetBuildContractsForMassBuild(BaseParams baseParams)
        {
            int totalCount;
            var result = Service.GetBuildContractsForMassBuild(baseParams, true, out totalCount);

            return result.Success ? new JsonListResult((IList)result.Data, totalCount) : JsFailure(result.Message);
        }

        /// <summary>
        /// Восстановить
        /// </summary>
        public ActionResult Recover(BaseParams baseParams)
        {
            var result = this.Service.Recover(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить признак Используется добавление работ из ДПКР
        /// </summary>
        public ActionResult UseAddWorkFromLongProgram(BaseParams baseParams)
        {
            var result = this.Service.UseAddWorkFromLongProgram(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить дополнительные параметры
        /// </summary>
        public ActionResult GetAdditionalParams(BaseParams baseParams)
        {
            var result = this.Service.GetAdditionalParams(baseParams);

            return result.ToJsonResult();
        }

        /// <summary>
        /// Получить перечень работ по объекту
        /// </summary>     
        public ActionResult GetListCrObjectWorksByObjectId(BaseParams baseParams)
        {
            int totalCount;
            var result = Service.GetListCrObjectWorksByObjectId(baseParams, true, out totalCount);

            return result.Success ? new JsonListResult((IList)result.Data, totalCount) : JsFailure(result.Message);
        }

        /// <summary>
        /// Получить перечень работ по программе
        /// </summary>
        public ActionResult GetListDistinctWorksByProgramId(BaseParams baseParams)
        {
            int totalCount;
            var result = Service.GetListDistinctWorksByProgramId(baseParams, true, out totalCount);

            return result.Success ? new JsonListResult((IList)result.Data, totalCount) : JsFailure(result.Message);
        }

        /// <summary>
        /// Получить перечень домов по программе и работам
        /// </summary>
        public ActionResult GetListObjectCRByMassBuilderId(BaseParams baseParams)
        {
            int totalCount;
            var result = Service.GetListObjectCRByMassBuilderId(baseParams, true, out totalCount);

            return result.Success ? new JsonListResult((IList)result.Data, totalCount) : JsFailure(result.Message);
        }

        public ActionResult AddOverhaulProposalWork(BaseParams baseParams)
        {
            var result = Service.AddOverhaulProposalWork(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddMassBuilderContractWork(BaseParams baseParams)
        {
            var result = Service.AddMassBuilderContractWork(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ChangeBuildContractStateFromMassBuild(BaseParams baseParams)
        {
            var result = Service.ChangeBuildContractStateFromMassBuild(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddMassBuilderContractCRWork(BaseParams baseParams)
        {
            var result = Service.AddMassBuilderContractCRWork(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddMassBuilderContractObjectCr(BaseParams baseParams)
        {
            var result = Service.AddMassBuilderContractObjectCr(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить список без фильтрации по оператору
        /// </summary>
        public ActionResult ListWithoutFilter(BaseParams baseParams)
        {
            var result = this.Service.ListWithoutFilter(baseParams);

            return result.ToJsonResult();
        }

        public ActionResult SendFKRToGZHIMail(BaseParams baseParams, Int64 taskId)
        {
            var result = Service.SendFKRToGZHIMail(baseParams, taskId);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}