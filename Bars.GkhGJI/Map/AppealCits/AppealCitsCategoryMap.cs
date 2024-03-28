namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    public class AppealCitsCategoryMap : BaseEntityMap<AppealCitsCategory>
    {
        public AppealCitsCategoryMap()
            : base("Категория заявителя", "GJI_APPEAL_DECLARANT_CATEGORY")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.AppealCits, "Обращение граждан").Column("CITIZENS_ID");
            this.Reference(x => x.ApplicantCategory, "Категория заявителя").Column("CATEGORY_ID").Fetch();
        }
    }
}
