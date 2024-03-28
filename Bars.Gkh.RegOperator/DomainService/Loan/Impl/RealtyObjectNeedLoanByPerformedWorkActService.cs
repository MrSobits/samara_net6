namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Exceptions;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Сервис получения потребности в займах на основе актов выполненных работ
    /// </summary>
    public class RealtyObjectNeedLoanByPerformedWorkActService : RealtyObjectNeedLoanBaseService
    {
        /// <summary>
        /// Репозиторий источников займа
        /// </summary>
        public ILoanSourceRepository LoanSourceRepository { get; set; }

        /// <summary>
        /// Источник потребностей
        /// </summary>
        public override LoanFormationType LoanFormationType => LoanFormationType.ByPerformedWorkAct;

        /// <summary>
        /// Получение домов, нуждающихся в займах
        /// </summary>
        public override IEnumerable<RealtyObjectNeedLoan> ListRealtyObjectNeedLoan(Municipality municipality, ProgramCr program)
        {
            ArgumentChecker.NotNull(municipality, nameof(municipality));
            ArgumentChecker.NotNull(program, nameof(program));

            var actDomain = this.Container.ResolveDomain<PerformedWorkAct>();

            var filterQuery = actDomain.GetAll()
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

            var actDomain = this.Container.ResolveDomain<PerformedWorkAct>();

            var roIds = robjects.Select(x => x.Id).ToArray();

            var filterQuery = actDomain.GetAll()
                .Where(x => roIds.Contains(x.ObjectCr.RealityObject.Id))
                .Where(x => x.Sum > 0);

            return this.ListRealtyObjectNeedLoan(filterQuery, program);
        }

        /// <summary>
        /// Получение домов, нуждающихся в займах
        /// </summary>
        private IEnumerable<RealtyObjectNeedLoan> ListRealtyObjectNeedLoan(IQueryable<PerformedWorkAct> filterQuery, ProgramCr program)
        {
            var objectCrDomain = this.Container.ResolveDomain<ObjectCr>();

            var roIdsQuery = filterQuery.Select(x => x.ObjectCr.RealityObject.Id);

            var calcAccountService = this.Container.Resolve<ICalcAccountService>();

            using (this.Container.Using(calcAccountService))
            {
                var calcAccounts = calcAccountService.GetRobjectsAccounts(roIdsQuery, DateTime.Today);

                var roIds = roIdsQuery.Distinct().ToArray();

                var objects = objectCrDomain.GetAll()
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
        }

        /// <summary>
        /// заполнить потребности и названия работ
        /// </summary>
        private void FillNeedSumAndWorkNames(RealtyObjectNeedLoan[] objects)
        {
            var loanDomain = this.Container.ResolveDomain<RealityObjectLoan>();
            var actDomain = this.Container.ResolveDomain<PerformedWorkAct>();
            var actPaymentDomain = this.Container.ResolveDomain<PerformedWorkActPayment>();

            var roIds = objects.Select(x => x.Id).ToArray();

            var filterQuery = actDomain.GetAll()
                .Where(x => roIds.Contains(x.ObjectCr.RealityObject.Id))
                //.Where(x => x.State.Name == "Утверждено")
                .Where(x => x.Sum > 0);

            // оплаты по актам
            var actPayments = actPaymentDomain.GetAll()
                .Where(x => filterQuery.Any(y => y.Id == x.PerformedWorkAct.Id))
                .Select(x => new
                {
                    x.PerformedWorkAct.Id,
                    x.Sum
                })
                .ToList()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.Sum));

            // словарь содержит инфорамацию о том, сколько ещё необходимо заплатить по акту
            // группировка идёт по домам, вложенная структура: Пара(акт, сумма к оплате)
            var roWorksSum = filterQuery
                .Select(x => new
                {
                    x.Id,
                    RoId = x.ObjectCr.RealityObject.Id,
                    Sum = x.Sum ?? 0m
                })
                .ToList()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key,
                    x => x.Select(y =>
                    new KeyValuePair<long, decimal>(y.Id, y.Sum - actPayments.Get(y.Id)))
                    .ToList());

            // словарь по актам домов, содержит Название работы и сумму, чтобы в дальнейшем вернуть
            // список работ
            var filterDict = filterQuery
                .GroupBy(x => x.Realty.Id)
                .ToDictionary(x => x.Key, x => x
                    .Where(y => roWorksSum[y.Realty.Id].Any(z => z.Key == y.Id && z.Value > 0))
                    .OrderBy(y => y.Id)
                    .Select(y => PerformedWorkActProxy.Create(y, roWorksSum))
                    .ToList());

            // зарезервированные займы, которые пока не учлись на счете оплат дома
            var currentReservedLoans = loanDomain.GetAll()
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

                obj.WorkNames = filterDict.Count>0? RealtyObjectNeedLoanByPerformedWorkActService.GetWorkNames(filterDict, obj.Id, balanceAndLoans):"";

                var roWorksSumValue = filterDict.ContainsKey(obj.Id)? roWorksSum.Get(obj.Id).Sum(x => x.Value):0m;
                obj.NeedSum = roWorksSumValue > balanceAndLoans
                    ? roWorksSumValue - balanceAndLoans
                    : 0m;
            }
        }

        private static string GetWorkNames(IDictionary<long, List<PerformedWorkActProxy>> actsByRo, long roId, decimal balanceAndLoans)
        {
            var moneyLeft = balanceAndLoans;
            var works = actsByRo.Get(roId);

            var workNames = new List<string>();
            if (works != null)
            {
                foreach (var work in works)
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
            else return "";
        }

        /// <summary>
        /// Класс для хранения пары "Наименование работы" и "Сумма по акту".
        /// <remarks>Необходим для генерации строки "Перечень работ" в реестре "Управление займами"</remarks>
        /// </summary>
        private class PerformedWorkActProxy
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
            public PerformedWorkActProxy(string actName, decimal sum)
            {
                this.WorkName = actName;
                this.Sum = sum;
            }

            /// <summary>
            /// Создать экземпляр <see cref="PerformedWorkActProxy"/>
            /// </summary>
            /// <param name="act">Акт выполненных работ</param>
            /// <param name="payments">Словарь оплат по домам, оттуда будем выдёргивать необходимую сумму</param>
            /// <returns>Класс-прокси</returns>
            public static PerformedWorkActProxy Create(PerformedWorkAct act,
                IDictionary<long, List<KeyValuePair<long, decimal>>> payments)
            {
                var sum = payments.Get(act.Realty.Id).ReturnSafe(x => x.First(s => s.Key == act.Id).Return(s => s.Value));

                return new PerformedWorkActProxy(act.TypeWorkCr.Work.Name, sum);
            }
        }
    }
}