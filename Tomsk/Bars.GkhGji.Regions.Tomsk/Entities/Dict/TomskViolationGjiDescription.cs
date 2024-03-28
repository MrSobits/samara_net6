namespace Bars.GkhGji.Regions.Tomsk.Entities.Dict
{
    using Bars.B4.DataAccess;

    public class TomskViolationGjiDescription : BaseEntity
    {
        public virtual TomskViolationGji ViolationGji { get; set; }

        public virtual byte[] RuleOfLaw { get; set; }
    }
}