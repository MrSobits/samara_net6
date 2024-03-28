namespace Bars.Gkh.RegOperator.DataProviders
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities;
    using Castle.Windsor;
    using Domain.Repository;
    using DomainService.PersonalAccount;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Провадйер для отчета по лицевому счету
    /// </summary>
    public class PersonalAccountReferenceDataProvider : BaseCollectionDataProvider<PersonalAccountReference>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container"></param>
        public PersonalAccountReferenceDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Получение данных по лицевому счету
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        protected override IQueryable<PersonalAccountReference> GetDataInternal(BaseParams baseParams)
        {
            var personalAccountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var chargePeriodRepo = this.Container.Resolve<IChargePeriodRepository>();
            var personalAccountServiceDomain = this.Container.Resolve<IPersonalAccountService>();
            var personalAccountService = this.Container.Resolve<IPersonalAccountService>();
            var fromService = baseParams.Params.GetAs<bool>("isFromService");

            try
            {
                var personalAccount = personalAccountDomain.GetAll().First(x => this.AccountId == x.Id);
                var currentPeriod = chargePeriodRepo.GetCurrentPeriod();
                var summaries = personalAccount.Summaries.ToList();

                Func<decimal, string> formatDecimal = x =>
                {
                    var xStr = x.ToString("G");

                    return xStr.Contains(',') ? xStr.TrimEnd('0').TrimEnd(',') : xStr;
                };

                var result = new List<PersonalAccountReference>();
                result.Add(new PersonalAccountReference
                {
                    НомерСчета = personalAccount.PersonalAccountNum,
                    ВнешнийЛС = personalAccount.PersAccNumExternalSystems,
                    ДатаОтчета = DateTime.Now.ToShortDateString(),
                    Абонент = fromService? "Абонент": personalAccount.AccountOwner.Name,
                    Адрес = personalAccount.Room.RealityObject.Address + ", кв. " + personalAccount.Room.RoomNum,
                    Площадь = formatDecimal(personalAccount.Room.Area),
                    Доля = formatDecimal(personalAccount.AreaShare),
                    Тариф =
                        formatDecimal(personalAccountService.GetTariff(personalAccount, currentPeriod.StartDate)),
                    ДатаОткрытия = personalAccount.OpenDate.ToShortDateString(),
                    ДатаЗакрытия = personalAccount.CloseDate > personalAccount.OpenDate
                        ? personalAccount.CloseDate.ToShortDateString()
                        : "не закрыт",
                    НачисленоПоМинимальному = summaries.SafeSum(x => x.GetChargedByBaseTariff() + x.BaseTariffChange).RegopRoundDecimal(2),
                    НачисленоРешение = summaries.SafeSum(x => x.GetChargedByDecisionTariff() + x.DecisionTariffChange).RegopRoundDecimal(2),
                    НачисленоПени = summaries.SafeSum(x => x.Penalty + x.RecalcByPenalty + x.PenaltyChange).RegopRoundDecimal(2),
                    ИтогоНачислено = summaries.SafeSum(x => x.GetTotalCharge() + x.GetTotalChange() - x.GetTotalPerformedWorkCharge()).RegopRoundDecimal(2),
                    УплаченоПоМинимальному = summaries.SafeSum(x => x.TariffPayment).RegopRoundDecimal(2),
                    УплаченоРешение = summaries.SafeSum(x => x.TariffDecisionPayment).RegopRoundDecimal(2),
                    УплаченоПени = summaries.SafeSum(x => x.PenaltyPayment).RegopRoundDecimal(2),
                    ИтогоУплачено = summaries.SafeSum(x => x.GetTotalPayment()).RegopRoundDecimal(2),
                    ЗадолженностьПоМинимальному = summaries.SafeSum(x => x.GetBaseTariffDebt()).RegopRoundDecimal(2),
                    ЗадолженностьРешение = summaries.SafeSum(x => x.GetDecisionTariffDebt()).RegopRoundDecimal(2),
                    ЗадолженностьПени = summaries.SafeSum(x => x.GetPenaltyDebt()).RegopRoundDecimal(2),
                    ЗачетСредств = summaries.SafeSum(x => x.GetTotalPerformedWorkCharge()).RegopRoundDecimal(2),
                    ИтогоЗадолженность =
                        summaries.SafeSum(x => x.GetBaseTariffDebt() + x.GetDecisionTariffDebt() + x.GetPenaltyDebt() - x.GetTotalPerformedWorkCharge()).RegopRoundDecimal(2)                
                });

                return result.AsQueryable();

            }
            finally
            {
                this.Container.Release(personalAccountDomain);
                this.Container.Release(chargePeriodRepo);
                this.Container.Release(personalAccountServiceDomain);
                this.Container.Release(personalAccountService);
            }
        }

        /// <summary>
        /// Наименование провайдера отчета
        /// </summary>
        public override string Name
        {
            get { return "Справка по лицевому счету"; }
        }

        /// <summary>
        /// Ключ провайдера
        /// </summary>
        public override string Key
        {
            get { return typeof(PersonalAccountReferenceDataProvider).Name; }
        }

        /// <summary>
        /// Описание провайдера
        /// </summary>
        public override string Description
        {
            get { return Name; }
        }

        /// <summary>
        /// Id лс
        /// </summary>
        public long AccountId { get; set; }
    }
}