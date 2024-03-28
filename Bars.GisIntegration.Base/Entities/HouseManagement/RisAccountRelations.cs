namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using global::Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Доли обременения
    /// </summary>
    public class RisAccountRelations : BaseRisEntity
    {
        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual RisAccount Account { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual RisHouse House { get; set; }

        /// <summary>
        /// Жилое помещение
        /// </summary>
        public virtual ResidentialPremises ResidentialPremise { get; set; }

        /// <summary>
        /// Нежилое помещение
        /// </summary>
        public virtual NonResidentialPremises NonResidentialPremises { get; set; }

        /// <summary>
        /// Жилая комната
        /// </summary>
        public virtual LivingRoom LivingRoom { get; set; }
    }
}
