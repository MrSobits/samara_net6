namespace Bars.GkhGji.Regions.Tatarstan.StateChange
{
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Правило для проверки заполненности поля "Причина аннулирования"
    /// </summary>
    public class ResolutionGisChargeAnnulRule : BaseResolutionGisChargeRule

    {
        public override string Id => "gji_resolution_gis_charge_annul_rule";

        public override string Name => "Постановление - Проверка заполненности поля \"Причина аннулирования\"";

        public override string TypeId => "gji_document_resol";

        public override string Description => "Проверяет заполненность поля \"Причина аннулирования\" и формирует файл с начислением для ГИС ГМП";

        protected override ChargeStatus ChargeStatus => ChargeStatus.Annul;

        protected override GisChargeToSend GenerateGisCharge(TatarstanResolution resolution, TatarstanProtocolMvd protocolMvd, PayerType payerType, DynamicDictionary config)
        {
            return new GisChargeToSend
            {
                Document = resolution,
                JsonObject = new GisChargeJson
                    {
                        PatternCode = resolution.PatternDict?.PatternCode,
                        SupplierBillId = resolution.GisUin,
                        OperationName = resolution.AbandonReason,
                        ChargeStatus = this.ChargeStatus.ToString("D")
                    }
            };
        }
    }
}