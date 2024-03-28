namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    // Место возникновения проблемы
    public class AppealCitsRealityObject : BaseGkhEntity
    {
        public virtual AppealCits AppealCits { get; set; }

        public virtual RealityObject RealityObject { get; set; }
    }
}