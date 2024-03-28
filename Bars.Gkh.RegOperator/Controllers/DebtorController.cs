namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.DataExport.Domain;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Tasks.Debtors;
    using Bars.Gkh.Utils;

    using DomainService.PersonalAccount;
    using Entities.PersonalAccount;

    public class DebtorController : B4.Alt.DataController<Debtor>
    {
        public IDebtorService Service { get; set; }

        public ActionResult Clear(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.Clear(baseParams));
        }

        public ActionResult CreateClaimWorks (BaseParams baseParams)
        {
            var taskManager = this.Container.Resolve<ITaskManager>();
            try
            {
                return taskManager.CreateTasks(new DebtorClaimWorkTaskProvider(), baseParams).ToJsonResult();
            }
            finally
            {
                this.Container.Release(taskManager);
            }
        }

        public ActionResult UpdateJurInstitution(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.UpdateJurInstitution(baseParams));
        }

        public ActionResult MakeNew(BaseParams baseParams)
        {
            var result = this.Service.Create(baseParams);
            return new JsonNetResult(new {success = result.Success, message = result.Message, data = result.Data});
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("DebtorExport");
            try
            {
                return export?.ExportData(baseParams);
            }
            finally
            {
                this.Container.Release(export);
            }
        }

        public ActionResult ListPenaltyDetail(BaseParams baseParams)
        {
            var documentClwDomain = this.Container.ResolveDomain<DocumentClw>();
            var clwAccountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            using (this.Container.Using(documentClwDomain, clwAccountDetailDomain))
            {
                var docId = baseParams.Params.GetAsId();
                var doc = documentClwDomain.Get(docId);

                return clwAccountDetailDomain.GetAll()
                    .Where(x => x.ClaimWork.Id == doc.ClaimWork.Id)
                    .ToListDataResult(baseParams.GetLoadParam())
                    .ToJsonResult();
            }
        }

        /// <summary>
        /// Детализация операций по периоду
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        public ActionResult ListOperationDetails(BaseParams baseParams)
        {
            return this.Service.GetPaymentsOperationDetail(baseParams).ToJsonResult();
        }
    }
}