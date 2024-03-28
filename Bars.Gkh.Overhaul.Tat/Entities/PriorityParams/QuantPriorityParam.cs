namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using B4.DataAccess;

    public class QuantPriorityParam : BaseEntity
    {
        public virtual string Code { get; set; }

        public virtual string MinValue { get; set; }

        public virtual string MaxValue { get; set; }

        public virtual int Point { get; set; }
    }
}
