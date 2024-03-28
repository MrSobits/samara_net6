namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using B4.DataAccess;

    /// <summary>
    /// Качественный параметр очередности
    /// </summary>
    public class QualityPriorityParam : BaseEntity
    {
        public virtual string Code { get; set; }

        public virtual int Point { get; set; }

        public virtual int Value { get; set; }
    }
}