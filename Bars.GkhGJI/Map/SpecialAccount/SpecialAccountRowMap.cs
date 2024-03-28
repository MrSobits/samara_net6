namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>Маппинг для "Решение судебных участков"</summary>
    public class SpecialAccountRowMap : BaseEntityMap<SpecialAccountRow>
    {

        public SpecialAccountRowMap() :
                base("Строки отчета по спецсчетам", "GJI_SPECIAL_ACCOUNT_ROW")
        {
        }

        protected override void Map()
        {
            Reference(x => x.Municipality, "МО").Column("MO_ID").Fetch();
            Reference(x => x.RealityObject, "МКД").Column("RO_ID").Fetch();
            Property(x => x.AmmountDebt, "Сумма задолженности").Column("AMMOUNT_DEBT");
            Property(x => x.Tariff, "Сумма задолженности").Column("TARIFF");
            Property(x => x.AccuracyArea, "Начисляемая площадь").Column("ACCURACY_AREA");
            Property(x => x.StartDate, "Дата начала начислений").Column("START_DATE");
            Property(x => x.Accured, "Начислено").Column("ACCURED");
            Property(x => x.Contracts, "Договоры").Column("CONTRACTS");
            Property(x => x.TransferTotal, "Всего израсходовано").Column("TRANSFER_TOTAL");
            Property(x => x.AccuredTotal, "Начислено всего").Column("ACCURED_TOTAL");
            Property(x => x.IncomingTotal, "Собрано всего").Column("INCOMING_TOTAL");
            Property(x => x.Ballance, "Размер остатка").Column("BALLANCE");
            Property(x => x.Incoming, "Поступление взносов").Column("INCOMING");
            Property(x => x.SpecialAccountNum, "Номер специального счета").Column("SPECIAL_ACCOUNT_NUM");
            Reference(x => x.SpecialAccountReport, "SpecialAccountReport").Column("REPORT_ID").Fetch();
            Property(x => x.Transfer, "Размер перечислений").Column("TRANSFER");
            Property(x => x.AmountDebtForPeriod, "Размер задолженности по оплате за период").Column("INF_AMMOUNT_ARREAR_PAY_CONT_MAJOR_REPAIRS");
            Property(x => x.AmountDebtCredit, "Задолженности по кредитным договорам").Column("AMMOUNT_DEBT_CREDIT");
        }
    }
}
