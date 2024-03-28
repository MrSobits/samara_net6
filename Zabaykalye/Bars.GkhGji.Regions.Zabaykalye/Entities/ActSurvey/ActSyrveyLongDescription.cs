namespace Bars.GkhGji.Regions.Zabaykalye.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    public class ActSurveyLongDescription : BaseEntity
    {
        public virtual ActSurvey ActSurvey { get; set; }

        public virtual byte[] Description { get; set; }
    }
}
