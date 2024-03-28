namespace Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    public class TatarstanProtocolGjiViolation : BaseEntity
    {
        public virtual ViolationGji ViolationGji { get; set; }

        public virtual TatarstanProtocolGji TatarstanProtocolGji { get; set; }
    }   
}
