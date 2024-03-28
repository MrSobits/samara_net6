namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;

    public class SMEVComplaintsLongText : BaseEntity
    {
        public virtual SMEVComplaints SMEVComplaints { get; set; }
        public virtual byte[] PauseResolutionPetition { get; set; }
        public virtual byte[] RenewTermPetition { get; set; }
    }
}
