namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict.BudgetClassificationCode
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.BudgetClassificationCode;

    public class BudgetClassificationCodeMunicipalityMap : BaseEntityMap<BudgetClassificationCodeMunicipality>
    {
        /// <inheritdoc />
        public BudgetClassificationCodeMunicipalityMap()
            : base(typeof(BudgetClassificationCodeMunicipality).FullName, "GJI_DICT_KBK_MUNICIPALITY")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.BudgetClassificationCode, nameof(BudgetClassificationCodeMunicipality.BudgetClassificationCode)).Column("KBK_ID").NotNull().Fetch();
            this.Reference(x => x.Municipality, nameof(BudgetClassificationCodeMunicipality.Municipality)).Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
