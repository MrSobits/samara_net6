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

    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Utils;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Провайдер для отчета по начислениям и перерасчету пени по лицевому счету
    /// </summary>
    public class PersonalAccountPenaltyDataProvider : BaseCollectionDataProvider<PersonalAccountPenaltyInfo>
    {
        
        public PersonalAccountPenaltyDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Получение данных по лицевому счету
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        protected override IQueryable<PersonalAccountPenaltyInfo> GetDataInternal(BaseParams baseParams)
        {
            var penaltyConstant = 1M / PersonalAccountPenaltyDataProvider.PenaltyConstant;

            var chargeDomain = this.Container.ResolveDomain<PersonalAccountCharge>();
            var calcParamTraceDomain = this.Container.ResolveDomain<CalculationParameterTrace>();

            try
            {
                var result = new List<PersonalAccountPenaltyInfo>();

                //получаем все начисления пени за все периоды
                var chargesDict = chargeDomain.GetAll()
                    .Where(x => x.BasePersonalAccount.Id == this.AccountId && x.IsActive)
                    .Select(x => new
                    {
                        x.ChargePeriod,
                        x.Guid
                    })
                    .ToDictionary(x => x.Guid, x => x.ChargePeriod);

                var data = calcParamTraceDomain.GetAll()
                    .Where(y => chargesDict.Keys.Contains(y.CalculationGuid))
                    .Where(x => x.CalculationType == CalculationTraceType.Penalty)
                    .Select(
                        x => new
                        {
                            x.ParameterValues,
                            x.DateStart,
                            x.DateEnd,
                            x.CalculationGuid
                        })
                    .AsEnumerable()
                    .Select(
                        x => new
                        {
                            x.DateStart,
                            x.DateEnd,
                            Percentage = x.ParameterValues.Get("payment_penalty_percentage").ToDecimal(),
                            PenaltyDebt = x.ParameterValues.Get("penalty_debt").ToDecimal().RegopRoundDecimal(2),
                            CalcType = x.ParameterValues.Get("numberdays_delay").To<NumberDaysDelay>().GetDisplayName(),
                            x.ParameterValues,
                            x.CalculationGuid
                        })
                    .Where(x => x.PenaltyDebt > 0)
                    .ToList();

                data.GroupBy(x => new { x.DateStart, DateEnd = x.DateEnd ?? DateTime.MinValue })
                    .ForEach(
                        x =>
                        {
                            var countDays = (x.Key.DateEnd - x.Key.DateStart).Days + 1;
                            var first = x.First();
                            var summary = (x.Sum(y => countDays * y.PenaltyDebt) * penaltyConstant * first.Percentage.ToDivisional()).RegopRoundDecimal(2);

                            result.Add(
                                new PersonalAccountPenaltyInfo
                                {
                                    Период = "{0} - {1}".FormatUsing(x.Key.DateStart.ToShortDateString(), x.Key.DateEnd.ToShortDateString()),
                                    Задолженность = first.PenaltyDebt,
                                    СтавкаРефинансирования = first.Percentage,
                                    Итого = summary,
                                    ЧислоДнейПросрочки = countDays,
                                    НаименованиеПериода = chargesDict[first.CalculationGuid].Name
                                });
                        });

                return result.AsQueryable();
            }
            finally
            {
                this.Container.Release(chargeDomain);
                this.Container.Release(calcParamTraceDomain);
            }
        }

        /// <summary>
        /// Наименование провайдера
        /// </summary>
        public override string Name
        {
            get { return "Информация по пени"; }
        }

        /// <summary>
        /// Ключ провайдера
        /// </summary>
        public override string Key
        {
            get { return typeof(PersonalAccountPenaltyDataProvider).Name; }
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

        private const int PenaltyConstant = 300;
    }
}