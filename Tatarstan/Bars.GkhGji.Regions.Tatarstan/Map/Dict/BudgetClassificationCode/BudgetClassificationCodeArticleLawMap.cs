namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict.BudgetClassificationCode
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.BudgetClassificationCode;

    public class BudgetClassificationCodeArticleLawMap : BaseEntityMap<BudgetClassificationCodeArticleLaw>
    {
        /// <inheritdoc />
        public BudgetClassificationCodeArticleLawMap()
            : base(typeof(BudgetClassificationCodeArticleLaw).FullName, "GJI_DICT_KBK_ARTICLE_LAW")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.BudgetClassificationCode, nameof(BudgetClassificationCodeArticleLaw.BudgetClassificationCode)).Column("KBK_ID").NotNull().Fetch();
            this.Reference(x => x.ArticleLaw, nameof(BudgetClassificationCodeArticleLaw.ArticleLaw)).Column("ARTICLE_LAW_ID").NotNull().Fetch();
        }
    }
}