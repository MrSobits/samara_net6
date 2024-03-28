namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Качественный параметр очередности
    /// </summary>
    public class QualityPriorityParam : BaseImportableEntity
    {
        public virtual string Code { get; set; }

        public virtual int Point { get; set; }

        public virtual int Value { get; set; }
    }
}