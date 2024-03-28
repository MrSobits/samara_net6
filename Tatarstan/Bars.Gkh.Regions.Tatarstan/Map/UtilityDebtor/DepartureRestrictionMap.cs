namespace Bars.Gkh.Regions.Tatarstan.Map.UtilityDebtor
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;

    /// <summary>
    /// Маппинг для "Постановление об ограничении выезда из РФ"
    /// </summary>
    public class DepartureRestrictionMap : JoinedSubClassMap<DepartureRestriction>
    {
        public DepartureRestrictionMap()
            : base("Постановление об ограничении выезда из РФ", "CLW_DEPARTURE_RESTRICTION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.AccountOwner, "Абонент").Column("ACCOUNT_OWNER").Length(150);
            this.Property(x => x.OwnerType, "Тип абонента").Column("OWNER_TYPE").DefaultValue(OwnerType.Individual).NotNull();

            this.Property(x => x.AccountOwnerBankDetails, "Реквизиты физ. лица").Column("OWNER_BANK_DETAILS").Length(255);
            this.Property(x => x.Year, "Год").Column("YEAR").Length(4);
            this.Property(x => x.Number, "Номер").Column("NUMBER").Length(50);
            this.Property(x => x.SubNumber, "Подномер").Column("SUBNUMBER").Length(10);
            this.Property(x => x.Document, "Документ-основание").Column("DOCUMENT").Length(50);
            this.Property(x => x.DeliveryDate, "Дата вручения").Column("DELIVERY_DATE");
            this.Property(x => x.IsСanceled, "Аннулировано").Column("IS_CANCELED").DefaultValue(false).NotNull();
            this.Property(x => x.CancelReason, "Причина аннулирования").Column("CANCEL_REASON").Length(150);
            this.Property(x => x.Official, "Должностное лицо").Column("OFFICIAL").Length(255);
            this.Property(x => x.Location, "Местонахождение").Column("LOCATION").Length(255);
            this.Property(x => x.Creditor, "Взыскатель").Column("CREDITOR").Length(255);
            this.Property(x => x.BankDetails, "Реквизиты").Column("BANK_DETAILS").Length(255);

            this.Reference(x => x.JurInstitution, "Подразделение ОСП").Column("JUR_INSTITUTION_ID").Fetch();
        }
    }
}