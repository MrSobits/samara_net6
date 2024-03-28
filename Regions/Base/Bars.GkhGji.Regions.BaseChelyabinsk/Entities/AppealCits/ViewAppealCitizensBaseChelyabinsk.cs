namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits
{
    public class ViewAppealCitizensBaseChelyabinsk : GkhGji.Entities.ViewAppealCitizens
    {
        /// <summary>
        /// Наименование тематики
        /// </summary>
        public virtual string SubjectsName { get; set; }

        /// <summary>
        /// Фио инспектора
        /// </summary>
        public virtual string SubjectExecutantsFio { get; set; }
    }
}