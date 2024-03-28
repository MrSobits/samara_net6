namespace Bars.GkhGji.Regions.Tatarstan.StateChange.TatarstanProtocolGji
{
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Правило для формирования файла начисления для ГИС ГМП
    /// </summary>
    public class TatarstanProtocolGjiChargeSendRule : BaseTatarstanProtocolGjiChargeRule
    {
        /// <inheritdoc/>
        public override string Id => "gji_document_protocol_gji_rt_charge_send_rule";

        /// <inheritdoc/>
        public override string Name => "Протокол - Формировать файл начисления для ГИС ГМП";

        /// <inheritdoc/>
        public override string Description => "Формирует файл с начислением для ГИС ГМП";

        /// <inheritdoc/>
        public override ChargeStatus ChargeStatus => ChargeStatus.Send;
    }
}