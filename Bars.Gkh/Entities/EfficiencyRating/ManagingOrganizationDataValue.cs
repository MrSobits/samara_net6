namespace Bars.Gkh.Entities.EfficiencyRating
{
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Объект управляющей организации
    /// </summary>
    public class ManagingOrganizationDataValue : BaseDataValue, IHasParent<ManagingOrganizationDataValue>
    {
        /// <summary>
        /// Тип мета-информации для конструктора
        /// </summary>
        public override DataMetaObjectType ObjectType => DataMetaObjectType.EfficientcyRating;

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganizationEfficiencyRating EfManagingOrganization { get; set; }

        /// <summary>
        /// Динамика роста
        /// </summary>
        public virtual decimal Dynamics { get; set; }

        /// <inheritdoc />
        ManagingOrganizationDataValue IHasParent<ManagingOrganizationDataValue>.Parent => this.Parent as ManagingOrganizationDataValue;
    }
}
