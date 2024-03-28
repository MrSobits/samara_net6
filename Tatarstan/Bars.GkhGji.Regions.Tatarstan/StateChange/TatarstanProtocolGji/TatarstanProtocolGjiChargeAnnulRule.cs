namespace Bars.GkhGji.Regions.Tatarstan.StateChange.TatarstanProtocolGji
{
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Правило для проверки заполненности поля "Причина Аннулирования" и формирования файла начисления для ГИС ГМП
    /// </summary>
    public class TatarstanProtocolGjiChargeAnnulRule : BaseTatarstanProtocolGjiChargeRule
    {
        /// <inheritdoc/>
        public override string Id => "gji_document_protocol_gji_rt_charge_annul_rule";

        /// <inheritdoc/>
        public override string Name => "Протокол - Проверка заполненности поля \"Причина аннулирования\"";

        /// <inheritdoc/>
        public override string Description => "Проверяет заполненность поля \"Причина аннулирования\" в карточке протокола и формирует файл с начислением для ГИС ГМП";

        /// <inheritdoc/>
        public override ChargeStatus ChargeStatus => ChargeStatus.Annul;

        /// <inheritdoc/>
        protected override ValidateResult AfterValidationAction(TatarstanProtocolGjiContragent protocol)
        {
            if (string.IsNullOrWhiteSpace(protocol.AnnulReason))
            {
                return ValidateResult.No("Не заполнено поле \"Причина аннулирования\"");
            }

            return base.AfterValidationAction(protocol);
        }
    }
}