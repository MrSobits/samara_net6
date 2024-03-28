namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits
{
    using GkhGji.Map;

    public class ViewAppealCitizensBaseChelyabinskMap : ViewAppealCitizensMap<ViewAppealCitizensBaseChelyabinsk>
    {
        protected override void Map()
        {
            base.Map();
            this.Property(x => x.SubjectsName, "Наименование тематики").Column("SUBJECTS_NAME");
            this.Property(x => x.SubjectExecutantsFio, "Фио инспектора").Column("EXECUTANTS_FIO");
        }
    }
}
