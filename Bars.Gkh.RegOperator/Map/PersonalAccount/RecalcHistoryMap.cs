namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    /// <summary>Маппинг для "История перерасчета"</summary>
    public class RecalcHistoryMap : BaseEntityMap<RecalcHistory>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RecalcHistoryMap()
            :
                base("История перерасчета", "REGOP_RECALC_HISTORY")
        {
        }

        /// <summary>
        /// Сопоставление
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("ACCOUNT_ID").NotNull().Fetch();
            this.Reference(x => x.CalcPeriod, "Период, для которого считается пени").Column("CALC_PERIOD_ID").NotNull().Fetch();
            this.Reference(x => x.RecalcPeriod, "Период, для которого происходит перерасчет пени").Column("RECALC_PERIOD_ID").NotNull().Fetch();

            this.Property(x => x.RecalcSum, "Сумма перерасчета (дельта)").Column("RECALC_SUM").DefaultValue(0m).NotNull();
            this.Property(x => x.UnacceptedChargeGuid, "Ссылка на неподтвержденное начисление").Column("UNACCEPTED_GUID").Length(250).NotNull();
            this.Property(x => x.RecalcType, "Тип перерасчета (по базовому тарифу, по тарифу решения или пени)").Column("RECALC_TYPE").NotNull();
        }
    }
}