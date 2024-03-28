namespace Bars.GkhGji.Regions.Samara.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    public class AppealCitsTester : BaseEntity
    {
        public virtual AppealCits AppealCits { get; set; }

        public virtual Inspector Tester { get; set; }
    }
}
