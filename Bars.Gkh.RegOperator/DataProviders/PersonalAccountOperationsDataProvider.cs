namespace Bars.Gkh.RegOperator.DataProviders
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.Utils;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities;
    using Castle.Windsor;
    using System.Collections.Generic;
    
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Провайдер для отчета по операциям по лицевому счету
    /// </summary>
    public class PersonalAccountOperationsDataProvider : BaseCollectionDataProvider<PersonalAccountOperations>
    {
        private readonly ITariffAreaCache tariffAreaService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container"></param>
        public PersonalAccountOperationsDataProvider(IWindsorContainer container)
            : base(container)
        {
            this.tariffAreaService = container.Resolve<ITariffAreaCache>();
        }

        /// <summary>
        /// Получение данных по лицевому счету
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        protected override IQueryable<PersonalAccountOperations> GetDataInternal(BaseParams baseParams)
        {
            var personalAccountDomain = this.Container.ResolveDomain<BasePersonalAccount>();

            try
            {
                var personalAccount = personalAccountDomain.Get(this.AccountId);

                var summaries = personalAccount.Summaries.ToList();

                //забираем все оплаты с ЛС
                var paymentsDate = this.Container.Resolve<IDomainService<PersonalAccountPaymentTransfer>>().GetAll()
                                .Where(y => y.Owner.Id == personalAccount.Id)
                                .Where(y => y.Operation.IsCancelled != true)
                                .OrderByDescending(y => y.PaymentDate);

                //получение тарифа, площади и доли
                var info = new PersonalAccountRecord
                {
                    AccountId = personalAccount.Id,
                    RoomId = personalAccount.Room.Id,
                    RealityObjectId = personalAccount.Room.RealityObject.Id
                };

                this.tariffAreaService.Init(info, summaries.Select(x => x.Period).ToArray());
                var tariffAreaDict = new Dictionary<long, TariffAreaRecord>();
                var periodPaymentDict = new Dictionary<long, DateTime>();
                foreach (var periodSummary in summaries)
                {
                    var record = this.tariffAreaService.GetTariffArea(personalAccount, periodSummary.Period);
                    if (record != null)
                    {
                        tariffAreaDict.Add(periodSummary.Period.Id, record);
                    }
                }

                //получаем максимальные даты по каждому периоду
                foreach (var periodPayment in paymentsDate)
                {
                    if (!periodPaymentDict.ContainsKey(periodPayment.ChargePeriod.Id))
                        periodPaymentDict.Add(periodPayment.ChargePeriod.Id, periodPayment.PaymentDate);
                }

                var operations = summaries
                     .OrderBy(x => x.Period.StartDate)
                     .Select(x => new
                     {
                         x.Period.Id,
                         x.Period.Name,
                         x.SaldoIn,
                         x.SaldoOut,
                         x.ChargeTariff,
                         x.Penalty,
                         x.RecalcByBaseTariff,
                         x.RecalcByDecisionTariff,
                         x.RecalcByPenalty,
                         SaldoChange = x.GetTotalChange(),
                         PeriodId = x.Period.Id,
                         PerfWorkCharge = x.GetTotalPerformedWorkCharge(),
                         TariffPayment = x.TariffPayment + x.TariffDecisionPayment,
                         x.PenaltyPayment
                     })
                        .AsEnumerable()
                        .Select(x =>
                        {
                            var recalcBase = x.RecalcByBaseTariff.RegopRoundDecimal(2);
                            var recalcDecision = x.RecalcByDecisionTariff.RegopRoundDecimal(2);
                            var recalcPenalty = x.RecalcByPenalty.RegopRoundDecimal(2);

                            return new PersonalAccountOperations
                            {
                                Период = x.Name,
                                ВходящееСальдо = x.SaldoIn.RegopRoundDecimal(2),
                                НачисленоВзносов = x.ChargeTariff.RegopRoundDecimal(2),
                                НачисленоПени = x.Penalty.RegopRoundDecimal(2),

                                ПерерасчетБазовый = recalcBase,
                                ПерерасчетРешений = recalcDecision,
                                ПерерасчетПени = recalcPenalty,
                                Перерасчет = recalcBase + recalcDecision + recalcPenalty,

                                УплаченоВзносов = x.TariffPayment.RegopRoundDecimal(2),
                                УплаченоПени = x.PenaltyPayment.RegopRoundDecimal(2),
                                ДатаОплаты = periodPaymentDict.ContainsKey(x.PeriodId) ? periodPaymentDict[x.PeriodId].ToShortDateString() : null,

                                Зачет = x.PerfWorkCharge.RegopRoundDecimal(2),
                                УстановкаИзменениеСальдо = x.SaldoChange.RegopRoundDecimal(2),
                                ИсходящееСальдо = x.SaldoOut.RegopRoundDecimal(2),
                                Тариф = Math.Max(tariffAreaDict[x.Id].BaseTariff, tariffAreaDict[x.Id].Tariff),
                                Площадь = tariffAreaDict[x.Id].RoomArea,
                                Доля = tariffAreaDict[x.Id].AreaShare,
                                PeriodId = x.PeriodId
                            };
                        })
                    .ToArray();

                return operations.AsQueryable();
            }
            finally
            {
                this.tariffAreaService.Dispose();
                this.Container.Release(personalAccountDomain);
            }
        }

        /// <summary>
        /// Наименование провайдера
        /// </summary>
        public override string Name
        {
            get { return "Отчет по лицевому счету"; }
        }

        /// <summary>
        /// Ключ провайдера
        /// </summary>
        public override string Key
        {
            get { return typeof(PersonalAccountOperationsDataProvider).Name; }
        }

        /// <summary>
        /// Описание провайдера
        /// </summary>
        public override string Description
        {
            get { return this.Name; }
        }

        /// <summary>
        /// Id лс
        /// </summary>
        public long AccountId { get; set; }
    }
}