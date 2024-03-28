namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.Entity;


    /// <summary>Маппинг для "Представление "Должник""</summary>
    public class ViewDebtorExportMap : PersistentObjectMap<ViewDebtorExport>
    {

        public ViewDebtorExportMap() :
                base("Представление \"Должник\"", "VIEW_CLW_DEBTOR_EXPORT")
        {
        }

        protected override void Map()
        {
            Property(x => x.PersonalAccountId, "Id аккаунта").Column("PERS_ACC_ID");
            Property(x => x.MunicipalityId, "Id муниципального образования").Column("MUNICIPALITY_ID");
            Property(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_NAME");
            Property(x => x.Settlement, "МО - поселение").Column("SETTLEMENT");
            Property(x => x.RoomAddress, "Адрес").Column("ROOM_ADDRESS");
            Property(x => x.StateId, "Статус").Column("STATE_ID");
            Property(x => x.State, "Статус").Column("STATE_NAME");
            Property(x => x.PersonalAccountNum, "Номер ЛС").Column("PERS_ACC_NUM");
            Property(x => x.AccountOwnerId, "Id собственника").Column("OWNER_ID");
            Property(x => x.AccountOwner, "Собственник").Column("OWNER_NAME");
            Property(x => x.OwnerType, "Тип собственника").Column("OWNER_TYPE");
            Property(x => x.DebtSum, "Сумма долга").Column("DEBT_SUM");
            Property(x => x.DebtBaseTariffSum, "Сумма по базовому тарифу").Column("BASE_TARIFF_SUM");
            Property(x => x.DebtDecisionTariffSum, "Сумма по тарифу решения").Column("DECISION_TARIFF_SUM");
            Property(x => x.ExpirationDaysCount, "Количество дней просрочки").Column("EXPIRATION_DAYS_COUNT");
            Property(x => x.ExpirationMonthCount, "Количество месяцев просрочки").Column("EXPIRATION_MONTH_COUNT");
            Property(x => x.PenaltyDebt, "Сумма пени").Column("PENALTY_DEBT");
            Property(x => x.HasClaimWork, "Наличие ПИР").Column("HAS_CLAIMWORK");
            Property(x => x.CourtType, "Тип суда").Column("COURT_TYPE");
            Property(x => x.JurInstitution, "Суд").Column("JUR_INST");
            Property(x => x.UserName, "Пользователь, начавший ПИР").Column("USER_NAME");
            Property(x => x.RealityObjectId, "Id дома").Column("RO_ID");
            Property(x => x.OwnerArea, "Площадь в собственности").Column("OWNER_AREA");
            Property(x => x.Underage, "Несовершеннолетний").Column("UNDERAGE");
            Property(x => x.ExtractExists, "Наличие выписки из ЕГРН").Column("EXTRACT_EXISTS");
            Property(x => x.AccountRosregMatched, "Сопоставлена ли выписка с ЛС").Column("ACC_ROSREG_MATCH");
            Property(x => x.ProcessedByTheAgent, "Обрабатывается ли ЛС агентом").Column("PROCESSED_DY_AGENT");
            Property(x => x.RoomArea, "Площадь помещения").Column("ROOM_AREA");
            Property(x => x.Separate, "Долевая собственность").Column("SEPARATE");
            Property(x => x.ClaimworkId, "Id ПИР").Column("CLAIMWORK_ID");
            Property(x => x.LastClwDebt, "Сумма долга по последней ПИР").Column("LAST_CLW_DEBT");
            Property(x => x.PaymentsSum, "Сумма оплат после последней ПИР").Column("PAYMENTS_SUM");
            Property(x => x.MewClaimDebt, "Новая сумма долга").Column("NEW_DEBT");
            Property(x => x.LastPirPeriod, "Последний период в ПИР").Column("LAST_CLW_PERIOD");
        }
    }
}
