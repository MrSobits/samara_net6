namespace Bars.Gkh.RegOperator.DataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    using Castle.Windsor;

    /// <summary>
    /// Провадйер для отчета по оплатам по личевому счету
    /// </summary>
    public class PersonalAccountLawPaymentsDataProvider : BaseCollectionDataProvider<PersonalAccountLawPaymentsInfo>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container"></param>
        public PersonalAccountLawPaymentsDataProvider(IWindsorContainer container)
            : base(container)

        {
        }

        /// <summary>
        /// Получение данных по лицевому счету
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns> PersonalAccountPaymentsInfo </returns>
        protected override IQueryable<PersonalAccountLawPaymentsInfo> GetDataInternal(BaseParams baseParams)
        {
            var lawCalcDomain = this.Container.ResolveDomain<LawsuitReferenceCalculation>();
            var personalAccountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var periodDomain = this.Container.ResolveDomain<ChargePeriod>();

            try
            {
                var persAcc = personalAccountDomain.Get(this.AccountId);

                IQueryable<PersonalAccountLawPaymentsInfo> lawQuery = new List<PersonalAccountLawPaymentsInfo>().AsQueryable();
                long maxLawsuit;
                var maxLawsuits = lawCalcDomain.GetAll()
                                    .Where(x => x.PersonalAccountId.Id == persAcc.Id).ToList();

                if (maxLawsuits.Count > 0)
                {
                    maxLawsuit = maxLawsuits.Max(x => x.Lawsuit.Id);

                    lawQuery = lawCalcDomain.GetAll()
                        .Where(x => x.PersonalAccountId.Id == persAcc.Id && x.Lawsuit.Id == maxLawsuit)
                        .Select(x => new
                        {
                            x.Id,
                            x.PaymentDate,
                            x.PeriodId,
                            x.PersonalAccountId,
                            x.RoomArea,
                            x.TarifDebt,
                            x.TariffCharged,
                            x.TarifPayment,
                            x.BaseTariff,
                            x.AreaShare,
                            x.Lawsuit,
                            x.Description
                        })
                        .ToList()
                        .Select(x =>
                        {
                            var periodName = periodDomain.GetAll().Where(y => y.Id == x.PeriodId.Id).Select(y => y.Name).FirstOrDefault();

                            return new PersonalAccountLawPaymentsInfo
                            {
                                AreaShare = x.AreaShare,
                                BaseTariff = x.BaseTariff,
                                PeriodId = x.PeriodId.Id,
                                PeriodName = periodName,
                                RoomArea = x.RoomArea,
                                PaymentDate = x.PaymentDate,
                                TarifDebt = x.TarifDebt,
                                TariffCharged = x.TariffCharged,
                                TarifPayment = x.TarifPayment,
                                Description = x.Description
                            };
                        })
                        .OrderBy(x => x.PeriodId)
                        .AsQueryable();
                }
                return lawQuery;
                
            }
            finally
            {
                this.Container.Release(periodDomain); 
                this.Container.Release(lawCalcDomain);
                this.Container.Release(personalAccountDomain);
            }
        }

        /// <summary>
        /// Наименование провайдера
        /// </summary>
        public override string Name
        {
            get { return "Информация по оплатам"; }
        }

        /// <summary>
        /// Ключ провайдера
        /// </summary>
        public override string Key
        {
            get { return typeof(PersonalAccountLawPaymentsDataProvider).Name; }
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