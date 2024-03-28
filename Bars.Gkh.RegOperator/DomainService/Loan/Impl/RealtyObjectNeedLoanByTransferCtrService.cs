namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Exceptions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Сервис получения потребности в займах на основе заявок на перечисление средств подрядчикам
    /// </summary>
    public class RealtyObjectNeedLoanByTransferCtrService : RealtyObjectNeedLoanBaseService
    {
        /// <summary>
         /// Источник получения потребности
         /// </summary>
        public override LoanFormationType LoanFormationType => LoanFormationType.ByTransferCtr;

        /// <summary>
        /// Сервис счетов домов
        /// </summary>
        public ICalcAccountService CalcAccountService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="TransferCtr"/>
        /// </summary>
        public IDomainService<TransferCtr> TransferCtrDomainService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="TransferCtrPaymentDetail"/>
        /// </summary>
        public IDomainService<TransferCtrPaymentDetail> TransferCtrPaymentDetailDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ObjectCr"/>
        /// </summary>
        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="MoneyLock"/>
        /// </summary>
        public IDomainService<MoneyLock> MoneyLockDomain { get; set; }

        /// <summary>
        /// Получение домов, нуждающихся в займах
        /// </summary>
        public override IEnumerable<RealtyObjectNeedLoan> ListRealtyObjectNeedLoan(Municipality municipality, ProgramCr program)
        {
            ArgumentChecker.NotNull(municipality, nameof(municipality));
            ArgumentChecker.NotNull(program, nameof(program));

            var filterQuery = this.TransferCtrDomainService.GetAll()
                .Where(x => x.ObjectCr.RealityObject.Municipality.Id == municipality.Id
                            || x.ObjectCr.RealityObject.MoSettlement.Id == municipality.Id)
                .Where(x => x.ObjectCr.ProgramCr.Id == program.Id)
                .Where(x => x.Sum > 0);

            return this.ListRealtyObjectNeedLoan(filterQuery, program);
        }

        /// <summary>
        /// Получение домов, нуждающихся в займах
        /// </summary>
        public override IEnumerable<RealtyObjectNeedLoan> ListRealtyObjectNeedLoan(RealityObject[] robjects, ProgramCr program)
        {
            ArgumentChecker.NotNull(program, nameof(program));

            if (robjects.IsEmpty())
            {
                throw new GkhException("Не указаны жилые дома");
            }
            var roIds = robjects.Select(x => x.Id).ToArray();

            var filterQuery = this.TransferCtrDomainService.GetAll()
                .Where(x => roIds.Contains(x.ObjectCr.RealityObject.Id))
                .Where(x => x.Sum > 0);

            return this.ListRealtyObjectNeedLoan(filterQuery, program);
        }

        private IEnumerable<RealtyObjectNeedLoan> ListRealtyObjectNeedLoan(IQueryable<TransferCtr> filterQuery, ProgramCr program)
        {
            var roIdsQuery = filterQuery.Select(x => x.ObjectCr.RealityObject.Id);
            var calcAccounts = this.CalcAccountService.GetRobjectsAccounts(roIdsQuery, DateTime.Today);
            var roIds = roIdsQuery.Distinct().ToArray();

            var objects = this.ObjectCrDomain.GetAll()
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .Where(x => x.ProgramCr.Id == program.Id)
                .Select(
                    x => new
                    {
                        x.RealityObject.Id,
                        x.RealityObject.Address,
                        x.ProgramCr.Period.DateStart.Year,
                        MunicipalityId = (long?)x.RealityObject.Municipality.Id,
                        SettlementId = (long?)x.RealityObject.MoSettlement.Id,
                        Settlement = x.RealityObject.MoSettlement.Name,
                        Municipality = x.RealityObject.Municipality.Name
                    })
                .AsEnumerable()
                .Where(x => calcAccounts.ContainsKey(x.Id))
                .Select(
                    x => new RealtyObjectNeedLoan
                    {
                        Id = x.Id,
                        Address = x.Address,
                        Year = x.Year,
                        CalcAccountNumber = calcAccounts[x.Id].AccountNumber,
                        Municipality = x.Municipality,
                        Settlement = x.Settlement,
                        MunicipalityId = x.MunicipalityId ?? 0,
                        SettlementId = x.SettlementId ?? 0
                    })
                .ToArray();

            this.FillCollection(objects);

            this.FillCurrentBalances(objects);

            this.FillNeedSumAndWorkNames(objects);

            return objects.Where(x => x.NeedSum > 0);
        }

        private void FillNeedSumAndWorkNames(RealtyObjectNeedLoan[] objects)
        {
            var roIds = objects.Select(x => x.Id).ToArray();

            var filterQuery = this.TransferCtrDomainService.GetAll()
                .Where(x => roIds.Contains(x.ObjectCr.RealityObject.Id))
                .Where(x => x.Sum > 0 && x.Sum != x.PaidSum);

            // словарь содержит инфорамацию о том, сколько ещё необходимо заплатить по акту
            // группировка идёт по домам, вложенная структура: Пара(акт, сумма к оплате)
            var roWorksSum = filterQuery
                .Select(x => new
                {
                    x.Id,
                    x.TransferGuid,
                    RoId = x.ObjectCr.RealityObject.Id,
                    x.Sum,
                    x.PaidSum
                })
                .ToList()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key,
                    x => x.Select(y =>
                    new KeyValuePair<long, decimal>(y.Id, y.Sum - y.PaidSum))
                    .ToList());

            // словарь по актам домов, содержит Название работы и сумму, чтобы в дальнейшем вернуть
            // список работ
            var filterDict = filterQuery
                .GroupBy(x => x.ObjectCr.RealityObject.Id)
                .ToDictionary(x => x.Key, x => x
                    .Where(y => roWorksSum[y.ObjectCr.RealityObject.Id].Any(z => z.Key == y.Id && z.Value > 0))
                    .OrderBy(y => y.Id)
                    .Select(y => TransferCtrProxy.Create(y, roWorksSum))
                    .ToList());

            // зарезервированные займы, которые пока не учлись на счете оплат дома
            var currentReservedLoans = this.RealityObjectLoanDomain.GetAll()
                .Where(x => roIds.Contains(x.LoanTaker.RealityObject.Id))
                .Where(x => !x.Operations.Any())
                .Select(x => new
                {
                    x.LoanTaker.RealityObject.Id,
                    Sum = x.LoanSum
                })
                .ToList()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Sum(z => z.Sum));

            foreach (var obj in objects)
            {
                var balanceAndLoans = obj.CurrentBalance + currentReservedLoans.Get(obj.Id);

                obj.WorkNames = RealtyObjectNeedLoanByTransferCtrService.GetWorkNames(filterDict, obj.Id, balanceAndLoans);

                var roWorksSumValue = roWorksSum.Get(obj.Id)?.Sum(x => x.Value) ?? 0;
                obj.NeedSum = roWorksSumValue > balanceAndLoans
                    ? roWorksSumValue - balanceAndLoans
                    : 0m;
            }
        }

        private static string GetWorkNames(IDictionary<long, List<TransferCtrProxy>> actsByRo, long roId, decimal balanceAndLoans)
        {
            var moneyLeft = balanceAndLoans;
            var works = actsByRo.Get(roId);

            var workNames = new List<string>();
            foreach (var work in works ?? Enumerable.Empty<TransferCtrProxy>())
            {
                //если средств для оплаты уже не хватает, то записываем вид работ в список
                if (moneyLeft < work.Sum)
                {
                    workNames.Add(work.WorkName);
                }

                moneyLeft -= work.Sum;
            }

            return string.Join(", ", workNames.Distinct());
        }

        private class TransferCtrProxy
        {
            /// <summary>
            /// Наименование работ
            /// </summary>
            public string WorkName { get; }

            /// <summary>
            /// Сумма к оплате (учитывает оплаченное)
            /// </summary>
            public decimal Sum { get; }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="actName">Вид работы</param>
            /// <param name="sum">Оставшаяся сумма к оплате</param>
            public TransferCtrProxy(string actName, decimal sum)
            {
                this.WorkName = actName;
                this.Sum = sum;
            }

            /// <summary>
            /// Создать экземпляр <see cref="TransferCtrProxy"/>
            /// </summary>
            /// <param name="act">Акт выполненных работ</param>
            /// <param name="payments">Словарь оплат по домам, оттуда будем выдёргивать необходимую сумму</param>
            /// <returns>Класс-прокси</returns>
            public static TransferCtrProxy Create(TransferCtr act,
                IDictionary<long, List<KeyValuePair<long, decimal>>> payments)
            {
                var sum = payments.Get(act.ObjectCr.RealityObject.Id).ReturnSafe(x => x.First(s => s.Key == act.Id).Return(s => s.Value));
                return new TransferCtrProxy(act.TypeWorkCr.Work.Name, sum);
            }
        }
    }
}