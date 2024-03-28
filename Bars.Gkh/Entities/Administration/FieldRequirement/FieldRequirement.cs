namespace Bars.Gkh.Entities
{
    using B4.DataAccess;

    public class FieldRequirement : BaseEntity
    {
        public virtual string RequirementId { get; set; }

        //не харниемое свойство
        public virtual bool Required { get; set; }
    }
}