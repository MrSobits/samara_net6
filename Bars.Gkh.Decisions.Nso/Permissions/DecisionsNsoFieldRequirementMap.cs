namespace Bars.Gkh.Decisions.Nso.Permissions
{
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Decisions.Nso.Entities;

    /// <summary>
    /// Обязательность полей
    /// </summary>
    public class DecisionsNsoFieldRequirementMap : FieldRequirementMap
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public DecisionsNsoFieldRequirementMap()
        {
            this.Namespace("Gkh.RealityObject.Register.DecisionProtocolsField", "Протоколы решений(карточка дома): Поля");
            this.Requirement("Gkh.RealityObject.Register.DecisionProtocolsField.AccountNum", "Номер спец. счета");
        }
    }
}