namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using B4.DataAccess;

    /// <summary>
    /// Количественный параметр очередности
    /// </summary>
    public class QuantPriorityParam : BaseEntity
    {
        public virtual string Code { get; set; }

        public virtual string MinValue { get; set; }

        public virtual string MaxValue { get; set; }

        public virtual int Point { get; set; }
    }
}
