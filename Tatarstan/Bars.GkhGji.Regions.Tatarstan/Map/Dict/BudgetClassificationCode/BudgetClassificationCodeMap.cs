namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict.BudgetClassificationCode
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.BudgetClassificationCode;

    public class BudgetClassificationCodeMap : BaseEntityMap<BudgetClassificationCode>
    {
        /// <inheritdoc />
        public BudgetClassificationCodeMap()
            : base(typeof(BudgetClassificationCode).FullName, "GJI_DICT_KBK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Kbk, nameof(BudgetClassificationCode.Kbk)).Column("KBK").NotNull();
            this.Property(x => x.StartDate, nameof(BudgetClassificationCode.StartDate)).Column("START_DATE").NotNull();
            this.Property(x => x.EndDate, nameof(BudgetClassificationCode.EndDate)).Column("END_DATE").NotNull();
        }
    }
}
