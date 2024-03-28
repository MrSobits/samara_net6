namespace Bars.Gkh.RegOperator.Controllers.Owner
{
    using B4;
    using Domain;
    using Entities.Owner;
    using Microsoft.AspNetCore.Mvc;

    using Bars.Gkh.Domain;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.RegOperator.Tasks.UnacceptedPayment;
    using System;

    /// <summary>
    /// Контролер "Собственник в исковом заявлении"
    /// </summary>
    public class LawsuitOwnerInfoController : B4.Alt.DataController<LawsuitOwnerInfo>
    {
        public ILawsuitOwnerInfoService Service { get; set; }
        private readonly ITaskManager _taskManager;
        public LawsuitOwnerInfoController(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.GetInfo(baseParams));
        }

        /// <summary>
        /// Расчитать задолженность
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        [ActionPermission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DebtCalculate")]
        public ActionResult DebtCalculate(BaseParams baseParams)
        {
            return this.Service.DebtCalculate(baseParams).ToJsonResult();
        }       

        /// <summary>
        /// Расчитать дату начала задолженности
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        [ActionPermission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DebtCalculate")]
        public ActionResult GetDebtStartDateCalculate(BaseParams baseParams)
        {
            return this.Service.GetDebtStartDateCalculate(baseParams).ToJsonResult();
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var exportName = baseParams.Params.GetAs<string>("exportName") ?? "LawsuitReferenceCalculationDataExport";
            var export = Container.Resolve<IDataExportService>(exportName);
            return export != null ? export.ExportData(baseParams) : null;
        }

        /// <summary>
        /// Расчитать дату начала задолженности
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        [ActionPermission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DebtCalculate")]
        public ActionResult DebtStartDateCalculate(BaseParams baseParams)
        {
            return this.Service.DebtStartDateCalculate(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Расчитать дату начала задолженности массово в экзекуторе
        /// </summary>
        /// <param name="baseParams">ids - идентификаторы собственников <see cref="LawsuitOwnerInfo"/></param>
        [ActionPermission("Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DebtCalculate")]
        public ActionResult DebtStartDateCalculateMass(BaseParams baseParams)
        {
            try
            {
                var result = _taskManager.CreateTasks(new MassDebtStartDateCalculateTaskProvider(), baseParams);
                if (result.Success)
                {
                    return JsSuccess("Задача расчёта эталонных оплат успешно поставлена");
                }
                return JsFailure("Ошибка при постановке задачи расчёта эталонных оплат");
            }
            catch (Exception e)
            {
                return JsFailure($"Ошибка: {e.Message}; {e.StackTrace}");
            }
        }
    }
}