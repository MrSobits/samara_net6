using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Map.Dict
{
    public class ApplicantCategoryMap:BaseEntityMap<ApplicantCategory>
    {
        public ApplicantCategoryMap()
            : base("Категории заявителя", "GJI_DICT_APPLICANT_CATEGORY")
        {

        }

        protected override void Map()
        {
            this.Property(x => x.Code, "Код записи").Column("CODE").Length(3).NotNull();
            this.Property(x => x.Name, "Наименование категории").Column("NAME").Length(100).NotNull();
        }
    }
}
