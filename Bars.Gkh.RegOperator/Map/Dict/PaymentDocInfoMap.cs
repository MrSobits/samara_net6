namespace Bars.Gkh.RegOperator.Map.Dict
{
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Справочник "Информационное поле в документе оплаты Физ.лица""</summary>
    public class PaymentDocInfoMap : BaseImportableEntityMap<PaymentDocInfo>
    {

        public PaymentDocInfoMap() :
                base("Справочник \"Информационное поле в документе оплаты Физ.лица\"", "REGOP_PAYMENT_DOC_INFO")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Municipality, "Муниципальный район").Column("MUNICIPALITY_ID").Fetch();
            this.Reference(x => x.MoSettlement, "Муниципальное образование").Column("SETTLEMENT_ID").Fetch();
            this.Property(x => x.LocalityAoGuid, "AOGUID населенного пункта").Column("LOCALITY_AOGUID");
            this.Property(x => x.LocalityName, "Наименование населенного пункта").Column("LOCALITY_NAME");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").Fetch();
            this.Property(x => x.FundFormationType, "Способ формирования фонда").Column("FUND_FORM_TYPE");
            this.Property(x => x.DateStart, "Дата начала").Column("DATE_START").NotNull();
            this.Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            this.Property(x => x.Information, "Информация").Column("INFORMATION").Length(4000);
            this.Property(x => x.IsForRegion, "Для всего региона").Column("IS_FOR_REGION").DefaultValue(false);
        }
    }
}
