namespace Bars.Gkh.RegOperator.Modules.ClaimWork.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;

    using Bars.B4.DataAccess;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Контроллер для основания претензионно-исковой работы для неплательщиков
    /// </summary>
    public class DebtorClaimworkRegoperatorController : BaseController
    {
        public IRepository<ClaimWorkAccountDetail> ClaimWorkAccountDetailRepository { get; set; }
        public IRepository<RestructDebt> RestructDebtRepository { get; set; }
        public IRepository<RestructDebtSchedule> RestructDebtScheduleRepository { get; set; }

        /// <summary>
        /// Получить ПИР по абоненту
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        public ActionResult GetByOwner(BaseParams baseParams)
        {
            return this.JsSuccess();
        }

        public ActionResult CheckClaimWork(BaseParams baseParams)
        {
            var persAccId = baseParams.Params.GetAsId();

            var claimworkId = this.ClaimWorkAccountDetailRepository.GetAll()
                .Where(x => x.PersonalAccount.Id == persAccId)
                //.Where(x => !x.ClaimWork.IsDebtPaid)
                .OrderByDescending(x => x.StartingDate)
                .Select(x => (long?) x.ClaimWork.Id)
                .FirstOrDefault();

            if (!claimworkId.HasValue)
            {
                return this.JsFailure("Не найден активный документ ПИР");
            }

            var existsScheduleQuery = this.RestructDebtScheduleRepository.GetAll()
                .Where(x => x.PersonalAccount.Id == persAccId);

            object claimworkInfo = this.RestructDebtRepository.GetAll()
                .Where(x => x.ClaimWork.Id == claimworkId)
                .Where(x => existsScheduleQuery.Any(y => y.RestructDebt.Id == x.Id))
                .Select(x => new
                {
                    ClaimWorkId = (long?) x.ClaimWork.Id,
                    RestructDebtId = x.Id,
                    x.DocumentType,
                    x.DocumentDate
                })
                .AsEnumerable()
                .GroupBy(x => x.ClaimWorkId)
                .Select(x => new
                {
                    ClaimWorkId = x.Key,
                    x.OrderByDescending(y => y.DocumentDate).FirstOrDefault(y => y.DocumentType == ClaimWorkDocumentType.RestructDebt)?.RestructDebtId,
                    RestructDebtAmicAgrId = x.OrderByDescending(y => y.DocumentDate)
                        .FirstOrDefault(y => y.DocumentType == ClaimWorkDocumentType.RestructDebtAmicAgr)?.RestructDebtId
                })
                .FirstOrDefault();

            return this.JsSuccess(claimworkInfo ?? new { ClaimWorkId = claimworkId });
        }
    }
}