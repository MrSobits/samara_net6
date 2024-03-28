namespace Bars.Gkh.Modules.Gkh1468.Map.Dict
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Modules.Gkh1468.Entities.Dict;

    /// <summary>
    /// Маппинг <see cref="TypeCustomer"/>
    /// </summary>
    public class TypeCustomerMap : BaseImportableEntityMap<TypeCustomer>
    {
        /// <inheritdoc />
        public TypeCustomerMap()
            : base("Bars.Gkh.Modules.Gkh1468.Entities.Dict", "GKH_DICT_TYPE_CUSTOMER")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull().Length(255);
        }
    }
}