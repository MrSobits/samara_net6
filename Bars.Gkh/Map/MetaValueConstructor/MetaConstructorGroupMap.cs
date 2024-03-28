namespace Bars.Gkh.Map.MetaValueConstructor
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    /// <summary>
    /// Маппинг <see cref="MetaConstructorGroup"/>
    /// </summary>
    public class MetaConstructorGroupMap : BaseEntityMap<MetaConstructorGroup>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public MetaConstructorGroupMap()
            : base("Bars.Gkh.MetaValueConstructor.DomainModel.MetaConstructorGroup", "GKH_CONSTRUCTOR_GROUP")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ConstructorType, "Тип описателя").Column("DESCR_TYPE").NotNull();
        }
    }
}