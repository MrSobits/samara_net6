using Bars.B4;
using Bars.B4.Application;
using Bars.B4.DataAccess;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Entities;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Modules.Tasks.Common.Utils;
using Bars.Gkh.Regions.Voronezh.Entities;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.RegOperator.Entities.ValueObjects;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Bars.Gkh.Regions.Voronezh.Tasks
{
    public class SetSSPPaymentsExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public IDomainService<LawSuitDebtWorkSSP> DebtWorkSSPDomain { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }
        public IDomainService<PersonalAccountPaymentTransfer> PersonalAccountPaymentTransferDomain { get; set; }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                var sw = new Stopwatch();
                var report = new StringBuilder();
                var totalCount = DebtWorkSSPDomain.GetAll().Where(x => x.LawsuitOwnerInfo != null && x.LawsuitOwnerInfo.PersonalAccount != null && x.CbDateSsp.HasValue).Count();
                int cnt = 0;
                DebtWorkSSPDomain.GetAll().Where(x=> x.LawsuitOwnerInfo != null && x.LawsuitOwnerInfo.PersonalAccount != null && x.CbDateSsp.HasValue).ToList().ForEach(x =>
                {
                    cnt++;
                    var persAccAllTransfers = PersonalAccountPaymentTransferDomain.GetAll()
            .Where(y => y.Owner.Id == x.LawsuitOwnerInfo.PersonalAccount.Id)
            .Where(y => y.OperationDate>=x.CbDateSsp)
            .Select(y=> new
            {
                y.Amount,
                y.Reason
            }).ToList();

                    if(persAccAllTransfers.Count>0)
                    {


                    }

                    //Оплаты
                     var persAccAllChargeTransfers = persAccAllTransfers.Where(
                        y => y.Reason == "Оплата по базовому тарифу" ||
                            y.Reason == "Оплата пени" ||
                            y.Reason == "Оплата по тарифу решения").Sum(y=> y.Amount);

                    //Отмены оплат
                    var persAccAllReturnTransfers = persAccAllTransfers.Where(
                        y => y.Reason == "Отмена оплаты по базовому тарифу" ||
                            y.Reason == "Отмена оплаты по тарифу решения" ||
                            y.Reason == "Отмена оплаты пени" ||
                            y.Reason == "Возврат взносов на КР").Sum(y => y.Amount);
                    x.CbSumStep = persAccAllChargeTransfers - persAccAllReturnTransfers >= x.CbSumRepayment ? x.CbSumRepayment : persAccAllChargeTransfers - persAccAllReturnTransfers;
                    var sspcont = ApplicationContext.Current.Container.Resolve<IRepository<LawSuitDebtWorkSSP>>();
                    sspcont.Update(x);
                    indicator.Indicate(null, Convert.ToUInt16(Decimal.Round(cnt/totalCount*100,0)), $"Обработано {cnt} из {totalCount}");

                });

                return new BaseDataResult(report.ToString());
            }
            catch(Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
    }
}
